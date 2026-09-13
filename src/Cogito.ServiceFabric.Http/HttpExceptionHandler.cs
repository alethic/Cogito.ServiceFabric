using System;
using System.Net;
using System.Net.Sockets;

using Microsoft.ServiceFabric.Services.Communication.Client;

namespace Cogito.ServiceFabric.Http
{

    /// <summary>
    /// Attempts to handle exceptions from the <see cref="HttpCommunicationClient"/> in a graceful manner.
    /// </summary>
    public class HttpExceptionHandler :
        IExceptionHandler
    {

        public bool TryHandleException(ExceptionInformation exceptionInformation, OperationRetrySettings retrySettings, out ExceptionHandlingResult result)
        {
            if (exceptionInformation.Exception is TimeoutException)
            {
                result = new ExceptionHandlingRetryResult(exceptionInformation.Exception, false, retrySettings, retrySettings.DefaultMaxRetryCountForNonTransientErrors);
                return true;
            }

            if (exceptionInformation.Exception is ProtocolViolationException)
            {
                result = new ExceptionHandlingThrowResult();
                return true;
            }

            if (exceptionInformation.Exception is SocketException)
            {
                result = new ExceptionHandlingRetryResult(exceptionInformation.Exception, false, retrySettings, retrySettings.DefaultMaxRetryCountForNonTransientErrors);
                return true;
            }

            var we = exceptionInformation.Exception as WebException;
            if (we == null)
                we = exceptionInformation.Exception.InnerException as WebException;

            if (we != null)
            {
                if (we.Status == WebExceptionStatus.ProtocolError)
                {
                    var errorResponse = we.Response as HttpWebResponse;

                    if (errorResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        // This could either mean we requested an endpoint that does not exist in the service API (a user error)
                        // or the address that was resolved by fabric client is stale (transient runtime error) in which we should re-resolve.
                        result = new ExceptionHandlingRetryResult(exceptionInformation.Exception, false, retrySettings, retrySettings.DefaultMaxRetryCountForNonTransientErrors);
                        return true;
                    }

                    if (errorResponse.StatusCode == HttpStatusCode.InternalServerError)
                    {
                        // The address is correct, but the server processing failed.
                        // This could be due to conflicts when writing the word to the dictionary.
                        // Retry the operation without re-resolving the address.
                        result = new ExceptionHandlingRetryResult(exceptionInformation.Exception, true, retrySettings, retrySettings.DefaultMaxRetryCountForTransientErrors);
                        return true;
                    }
                }

                if (we.Status == WebExceptionStatus.Timeout ||
                    we.Status == WebExceptionStatus.RequestCanceled ||
                    we.Status == WebExceptionStatus.ConnectionClosed ||
                    we.Status == WebExceptionStatus.ConnectFailure)
                {
                    result = new ExceptionHandlingRetryResult(exceptionInformation.Exception, false, retrySettings, retrySettings.DefaultMaxRetryCountForNonTransientErrors);
                    return true;
                }
            }

            result = null;
            return false;
        }

    }

}
