using System;
using System.Fabric;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

using Microsoft.AspNetCore.Http;

namespace Cogito.ServiceFabric.AspNetCore
{

    /// <summary>
    /// Detects the Service Fabric Reverse Proxy and fixes up requests for ASP.Net.
    /// </summary>
    public class ServiceFabricReverseProxyMiddleware
    {

        readonly static XNamespace sf = "http://schemas.microsoft.com/2011/01/fabric";

        readonly RequestDelegate next;
        readonly ServiceContext context;
        readonly AsyncLazy<XDocument> getClusterManifest;
        readonly AsyncLazy<int?> getApplicationGatewayEndpointPort;

        /// <summary>
        /// Initializes a new instance.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="context"></param>
        public ServiceFabricReverseProxyMiddleware(RequestDelegate next, ServiceContext context)
        {
            this.next = next ?? throw new ArgumentNullException(nameof(next));
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.getClusterManifest = AsyncLazy.Create(() => GetClusterManifest());
            this.getApplicationGatewayEndpointPort = AsyncLazy.Create(() => GetApplicationGatewayEndpointPort());
        }

        /// <summary>
        /// Loads the cluster manifest.
        /// </summary>
        /// <returns></returns>
        async Task<XDocument> GetClusterManifest()
        {
            using (var c = new FabricClient())
                return XDocument.Parse(await c.ClusterManager.GetClusterManifestAsync());
        }

        /// <summary>
        /// Gets the application gateway endpoint port.
        /// </summary>
        /// <returns></returns>
        async Task<int?> GetApplicationGatewayEndpointPort()
        {
            var manifest = await getClusterManifest;
            if (manifest == null)
                throw new InvalidOperationException("Unable to retrieve cluster manifest.");

            // attempts to derive the reverse proxy port from the cluster manifest
            return (int?)manifest
                .Element(sf + "ClusterManifest")
                .Element(sf + "NodeTypes")
                .Elements(sf + "NodeType").First(i => (string)i.Attribute("Name") == context.NodeContext.NodeType)
                .Element(sf + "Endpoints")
                .Element(sf + "HttpApplicationGatewayEndpoint")
                .Attributes("Port")
                .FirstOrDefault();
        }

        /// <summary>
        /// Gets the given HTTP header.
        /// </summary>
        /// <param name="http"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        string[] GetHeaderValue(HttpContext http, string name)
        {
            return http.Request.Headers.TryGetValue(name, out var v) ? v.ToArray() : new string[0];
        }

        /// <summary>
        /// Gets the original forwarded port. This should be the reverse proxy port.
        /// </summary>
        /// <param name="http"></param>
        /// <returns></returns>
        int? GetForwardedPort(HttpContext http)
        {
            var h = GetHeaderValue(http, "X-Forwarded-Host");
            if (h.Length > 0)
            {
                var p = h[0].Split(':');
                if (p.Length >= 2)
                    if (int.TryParse(p[1], out var port))
                        return port;
            }

            return null;
        }

        /// <summary>
        /// Detects whether the forwarded port matches the port number of the reverse proxy and rewrites the ASP.Net
        /// request appropriately.
        /// </summary>
        /// <param name="http"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext http)
        {
            if (http == null)
                throw new ArgumentNullException(nameof(http));

            // load reverse proxy port
            var proxyPort = await getApplicationGatewayEndpointPort;
            if (proxyPort == 0)
            {
                await next(http);
                return;
            }

            // find original forwarded port
            var forwardedPort = GetForwardedPort(http);
            if (forwardedPort == proxyPort)
            {
                if (context is StatelessServiceContext == false)
                {
                    await next(http);
                    return;
                }

                // save away the original path for restoration
                var pathBase = http.Request.PathBase;

                try
                {
                    // replace with servie name, which we know was the real path
                    http.Request.PathBase = context.ServiceName.AbsolutePath;
                    await next(http);
                }
                finally
                {
                    http.Request.PathBase = pathBase;
                }

                return;
            }

            await next(http);
        }

    }

}
