using System;
using System.Reflection;

namespace Cogito.ServiceFabric
{

    /// <summary>
    /// Provides methods to access the Service Fabric environment.
    /// </summary>
    public static class FabricEnvironment
    {

        /// <summary>
        /// Gets the Service Fabric assembly, handling errors that might occur if it is unavailable within the current environment.
        /// </summary>
        /// <returns></returns>
        static Assembly LoadFabricAssembly()
        {
            try
            {
                return AppDomain.CurrentDomain.Load("System.Fabric");
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the <see cref="FabricRuntime"/> type.
        /// </summary>
        static Type FabricRuntimeType => LoadFabricAssembly()?.GetType("System.Fabric.FabricRuntime");

        /// <summary>
        /// Gets the <see cref="FabricRuntime.GetActivationContext"/> method.
        /// </summary>
        static Lazy<MethodInfo> GetActivationContextMethod => new Lazy<MethodInfo>(() => FabricRuntimeType?.GetMethod("GetActivationContext", BindingFlags.Static | BindingFlags.Public), true);

        /// <summary>
        /// Returns <c>true</c> if the application is hosted within Service Fabric.
        /// </summary>
        public static bool IsFabric => !string.IsNullOrWhiteSpace(ApplicationName);

        /// <summary>
        /// Returns <c>true</c> if the application is hosted within a Service Fabric Implicit Host.
        /// </summary>
        public static bool IsImplicitHost => IsFabric && CanGetActivationContext();

        /// <summary>
        /// Returns <c>true</c> if the application is hosted within a Service Fabric Explicit Host (Guest Executable).
        /// </summary>
        public static bool IsExplicitHost => IsFabric && !IsImplicitHost;

        /// <summary>
        /// Gets the name of the Fabric application.
        /// </summary>
        public static string ApplicationName => Environment.GetEnvironmentVariable("Fabric_ApplicationName");

        /// <summary>
        /// Gets the name of the currently executing code package.
        /// </summary>
        public static string CodePackageName => Environment.GetEnvironmentVariable("Fabric_CodePackageName");

        /// <summary>
        /// Gets the IP address or FQDN of the current node.
        /// </summary>
        public static string NodeIPAddressOrFQDN => Environment.GetEnvironmentVariable("Fabric_NodeIPOrFQDN");

        /// <summary>
        /// Gets the name of the current node.
        /// </summary>
        public static string NodeName => Environment.GetEnvironmentVariable("Fabric_NodeName");

        /// <summary>
        /// Gets the name of the current service.
        /// </summary>
        public static string ServiceName => Environment.GetEnvironmentVariable("Fabric_ServiceName");

        /// <summary>
        /// Gets the service package name of the current service.
        /// </summary>
        public static string ServicePackageName => Environment.GetEnvironmentVariable("Fabric_ServicePackageName");

        /// <summary>
        /// Attempts to retrieve the activation context. Will fail if not inside an implicit host.
        /// </summary>
        /// <returns></returns>
        static bool CanGetActivationContext()
        {
            try
            {
                return GetActivationContextMethod?.Value?.Invoke(null, new object[0]) != null;
            }
            catch
            {
                return false;
            }
        }

    }

}
