using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Health;

using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services.Remoting.V2.FabricTransport.Runtime;

namespace Cogito.ServiceFabric.Services
{

    /// <summary>
    /// Reliable service base class which provides an <see cref="Microsoft.ServiceFabric.Data.IReliableStateManager"/> and provides some additional utility methods.
    /// </summary>
    public abstract class StatefulService :
        Microsoft.ServiceFabric.Services.Runtime.StatefulService
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
        static StatefulService()
        {
            fabric = new Lazy<FabricClient>(() => new FabricClient(), true);
        }

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        public StatefulService(StatefulServiceContext serviceContext)
            : base(serviceContext)
        {

        }

        /// <summary>
        /// Override this method to supply the communication listeners for the service instance. By default this method
        /// returns a <see cref="FabricTransportServiceRemotingListener"/>.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            yield return new ServiceReplicaListener(ctx => new FabricTransportServiceRemotingListener(ctx, (IService)this));
        }

        /// <summary>
        /// Reports the given <see cref="HealthInformation"/>.
        /// </summary>
        /// <param name="healthInformation"></param>
        protected void ReportHealth(HealthInformation healthInformation)
        {
            Fabric.HealthManager.ReportHealth(
                new StatefulServiceReplicaHealthReport(
                    Context.PartitionId,
                    Context.ReplicaId,
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
            if (string.IsNullOrWhiteSpace(sourceId))
                throw new ArgumentOutOfRangeException(nameof(sourceId));
            if (string.IsNullOrWhiteSpace(property))
                throw new ArgumentOutOfRangeException(nameof(property));
            if (description != null && description.Length > 4000)
                throw new ArgumentOutOfRangeException(nameof(property));

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
            if (string.IsNullOrWhiteSpace(sourceId))
                throw new ArgumentOutOfRangeException(nameof(sourceId));
            if (string.IsNullOrWhiteSpace(property))
                throw new ArgumentOutOfRangeException(nameof(property));
            if (exception == null)
                throw new ArgumentNullException(nameof(exception));

            ReportHealth(
                sourceId,
                property,
                state,
                exception.ToString().Left(4000),
                timeToLive,
                removeWhenExpired);
        }

        /// <summary>
        /// <see cref="Uri"/> of the application this <see cref="StatefulService"/> is a part of.
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
            if (packageName == null)
                throw new ArgumentNullException(nameof(packageName));

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
            if (packageName == null)
                throw new ArgumentNullException(nameof(packageName));
            if (sectionName == null)
                throw new ArgumentNullException(nameof(sectionName));
            if (parameterName == null)
                throw new ArgumentNullException(nameof(parameterName));

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
            if (sectionName == null)
                throw new ArgumentNullException(nameof(sectionName));
            if (parameterName == null)
                throw new ArgumentNullException(nameof(parameterName));

            return DefaultConfigurationPackage?.Settings.Sections[sectionName]?.Parameters[parameterName]?.Value;
        }

    }

}
