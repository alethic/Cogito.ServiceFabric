using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Fabric;
using System.Fabric.Health;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;

namespace Cogito.ServiceFabric
{

    /// <summary>
    /// Reliable service base class which provides some additional utility methods.
    /// </summary>
    public abstract class StatelessService :
        Microsoft.ServiceFabric.Services.Runtime.StatelessService
    {

        static readonly Lazy<FabricClient> fabric;

        /// <summary>
        /// Gets a reference to the <see cref="FabricClient"/>.
        /// </summary>
        static protected FabricClient Fabric
        {
            get { return fabric.Value; }
        }

        /// <summary>
        /// Initializes the static instance.
        /// </summary>
        static StatelessService()
        {
            fabric = new Lazy<FabricClient>(() => new FabricClient(), true);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public StatelessService(StatelessServiceContext serviceContext)
            : base(serviceContext)
        {
            Contract.Requires<ArgumentNullException>(serviceContext != null);
        }

        /// <summary>
        /// Override this method to supply the communication listeners for the service instance. By default this method
        /// returns a <see cref="FabricTransportServiceRemotingListener"/>.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            yield return new ServiceInstanceListener(ctx => new FabricTransportServiceRemotingListener(ctx, (IService)this));
        }

        /// <summary>
        /// Reports the given <see cref="HealthInformation"/>.
        /// </summary>
        /// <param name="healthInformation"></param>
        protected void ReportHealth(HealthInformation healthInformation)
        {
            Fabric.HealthManager.ReportHealth(
                new StatelessServiceInstanceHealthReport(
                    Context.PartitionId,
                    Context.InstanceId,
                    healthInformation));
        }

        /// <summary>
        /// Reports the given set of health information.
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="property"></param>
        /// <param name="state"></param>
        /// <param name="description"></param>
        /// <param name="timeToLive"></param>
        /// <param name="removeWhenExpired"></param>
        protected void ReportHealth(
            string sourceId,
            string property,
            HealthState state,
            string description = null,
            TimeSpan? timeToLive = null,
            bool? removeWhenExpired = null)
        {
            Contract.Requires<ArgumentNullException>(sourceId != null);
            Contract.Requires<ArgumentNullException>(property != null);
            Contract.Requires<ArgumentOutOfRangeException>(description == null || description.Length <= 4000);

            var i = new HealthInformation(sourceId, property, state);
            if (description != null)
                i.Description = description;
            if (timeToLive != null)
                i.TimeToLive = (TimeSpan)timeToLive;
            if (removeWhenExpired != null)
                i.RemoveWhenExpired = (bool)removeWhenExpired;
            ReportHealth(i);
        }

        /// <summary>
        /// Reports an <see cref="Exception"/> as health information.
        /// </summary>
        /// <param name="sourceId"></param>
        /// <param name="property"></param>
        /// <param name="exception"></param>
        /// <param name="state"></param>
        /// <param name="timeToLive"></param>
        /// <param name="removeWhenExpired"></param>
        protected void ReportException(
            string sourceId,
            string property,
            Exception exception,
            HealthState state = HealthState.Error,
            TimeSpan? timeToLive = null,
            bool? removeWhenExpired = null)
        {
            Contract.Requires<ArgumentNullException>(sourceId != null);
            Contract.Requires<ArgumentNullException>(property != null);
            Contract.Requires<ArgumentNullException>(exception != null);

            ReportHealth(
                sourceId,
                property,
                state,
                exception.ToString().Left(4000),
                timeToLive,
                removeWhenExpired);
        }

        /// <summary>
        /// Default implementation of RunAsync. Configures the service and dispatches to the RunTaskAsync method until
        /// canceled.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected sealed override async Task RunAsync(CancellationToken cancellationToken)
        {
            // enter method
            await RunEnterAsync(cancellationToken);

            // repeat run task until signaled to exit
            while (!cancellationToken.IsCancellationRequested)
                await RunLoopAsync(cancellationToken);

            // exit method
            await RunExitAsync(new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token);
        }

        /// <summary>
        /// Invoked when the service is run.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual Task RunEnterAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Invoked when the service is exiting.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual Task RunExitAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(true);
        }

        /// <summary>
        /// Override this method to implement the run loop.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected virtual Task RunLoopAsync(CancellationToken cancellationToken)
        {
            return Task.Delay(TimeSpan.FromSeconds(5));
        }

        /// <summary>
        /// <see cref="Uri"/> of the application this <see cref="StatelessService"/> is a part of.
        /// </summary>
        protected Uri ApplicationName
        {
            get { return new Uri(CodePackageActivationContext.ApplicationName + "/"); }
        }

        /// <summary>
        /// Gets the code package activation context passed to the service replica.
        /// </summary>
        protected ICodePackageActivationContext CodePackageActivationContext
        {
            get { return Context.CodePackageActivationContext; }
        }

        /// <summary>
        /// Gets the config package object corresponding to the package name.
        /// </summary>
        /// <param name="packageName"></param>
        /// <returns></returns>
        protected ConfigurationPackage GetConfigurationPackage(string packageName)
        {
            Contract.Requires<ArgumentNullException>(packageName != null);

            return CodePackageActivationContext.GetConfigurationPackageObject(packageName);
        }

        /// <summary>
        /// Name of the default configuration package.
        /// </summary>
        protected string DefaultConfigurationPackageName
        {
            get { return "Config"; }
        }

        /// <summary>
        /// Gets the default config package object.
        /// </summary>
        /// <returns></returns>
        protected ConfigurationPackage DefaultConfigurationPackage
        {
            get { return GetConfigurationPackage(DefaultConfigurationPackageName); }
        }

        /// <summary>
        /// Gets the configuration parameter from the specified section of the specified package.
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="sectionName"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        protected string GetConfigurationPackageParameterValue(string packageName, string sectionName, string parameterName)
        {
            Contract.Requires<ArgumentNullException>(packageName != null);
            Contract.Requires<ArgumentNullException>(sectionName != null);
            Contract.Requires<ArgumentNullException>(parameterName != null);

            return GetConfigurationPackage(packageName)?.Settings.Sections[sectionName]?.Parameters[parameterName]?.Value;
        }

        /// <summary>
        /// Gets the configuration parameter from the specified section.
        /// </summary>
        /// <param name="sectionName"></param>
        /// <param name="parameterName"></param>
        /// <returns></returns>
        protected string GetDefaultConfigurationPackageParameterValue(string sectionName, string parameterName)
        {
            Contract.Requires<ArgumentNullException>(sectionName != null);
            Contract.Requires<ArgumentNullException>(parameterName != null);

            return DefaultConfigurationPackage?.Settings.Sections[sectionName]?.Parameters[parameterName]?.Value;
        }

    }

}
