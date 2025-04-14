/*
 * Copyright 2022 Sage Intacct, Inc.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License"). You may not
 * use this file except in compliance with the License. You may obtain a copy 
 * of the License at
 * 
 * http://www.apache.org/licenses/LICENSE-2.0
 * 
 * or in the "LICENSE" file accompanying this file. This file is distributed on 
 * an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either 
 * express or implied. See the License for the specific language governing 
 * permissions and limitations under the License.
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Intacct.SDK.Credentials;
using Intacct.SDK.Functions;
using Microsoft.Extensions.Logging;

namespace Intacct.SDK.Xml
{
    public class RequestHandler
    {
        public const string Version = "3.2.2";

        public ClientConfig ClientConfig;

        public RequestConfig RequestConfig;

        public string EndpointUrl;

        public RequestHandler(ClientConfig clientConfig, RequestConfig requestConfig)
        {
            this.EndpointUrl = !string.IsNullOrEmpty(clientConfig.EndpointUrl) ? clientConfig.EndpointUrl : new Endpoint(clientConfig).ToString();

            this.ClientConfig = clientConfig;

            this.RequestConfig = requestConfig;
        }
        
        public async Task<OnlineResponse> ExecuteOnline(List<IFunction> content)
        {
            if (!string.IsNullOrEmpty(this.RequestConfig.PolicyId))
            {
                this.RequestConfig.PolicyId = "";
            }

            RequestBlock request = new RequestBlock(this.ClientConfig, this.RequestConfig, content);

            OnlineResponse response = new OnlineResponse(await Execute(request.WriteXml()).ConfigureAwait(false));

            return response;
        }

        public async Task<OfflineResponse> ExecuteOffline(List<IFunction> content)
        {
            if (string.IsNullOrWhiteSpace(this.RequestConfig.PolicyId))
            {
                throw new ArgumentException("Required Policy ID not supplied in config for offline request");
            }

            if (
                this.ClientConfig.Logger != null
                && (
                    !string.IsNullOrEmpty(this.ClientConfig.SessionId)
                    || this.ClientConfig.Credentials.GetType() == typeof(SessionCredentials)
                )
            )
            {
                // Log warning if using session ID for offline execution
                this.ClientConfig.Logger.LogWarning("Offline execution sent to Intacct using Session-based credentials. Use Login-based credentials instead to avoid session timeouts.");
            }

            RequestBlock request = new RequestBlock(this.ClientConfig, this.RequestConfig, content);

            OfflineResponse response = new OfflineResponse(await Execute(request.WriteXml()).ConfigureAwait(false));

            return response;
        }
        
        private HttpMessageHandler GetHttpMessageHandler()
        {
            if (this.ClientConfig.MockHandler != null)
            {
                if (this.ClientConfig.Logger != null)
                {
                    return new LoggingHandler(this.ClientConfig.MockHandler, this.ClientConfig.Logger, this.ClientConfig.LogMessageFormatter, this.ClientConfig.LogLevel);
                }
                else
                {
                    return this.ClientConfig.MockHandler;
                }
            }
            else
            {
                HttpClientHandler httpClientHandler = new HttpClientHandler();
                
                if (this.ClientConfig.Logger != null)
                {
                    return new LoggingHandler(httpClientHandler, this.ClientConfig.Logger, this.ClientConfig.LogMessageFormatter, this.ClientConfig.LogLevel);
                }
                else
                {
                    return httpClientHandler;
                }
            }
        }

        private static int ExponentialDelay(int retries)
        {
            return (int)Math.Pow(2, retries - 1);
        }

        private async Task<Stream> Execute(Stream requestXml)
        {
            HttpClient client = new HttpClient(GetHttpMessageHandler());
            client.Timeout = this.RequestConfig.MaxTimeout;
            client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip,deflate");
            client.DefaultRequestHeaders.UserAgent.ParseAdd("intacct-sdk-net-client/" + RequestHandler.Version);

            requestXml.Position = 0;
            StreamContent content = new StreamContent(requestXml);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/xml");
            
            for (int attempt = 0; attempt <= this.RequestConfig.MaxRetries; attempt++)
            {
                if (attempt > 0)
                {
                    // Delay this retry based on exponential delay
                    await Task.Delay(ExponentialDelay(attempt));
                }
                HttpResponseMessage response = await client.PostAsync(this.EndpointUrl, content).ConfigureAwait(false);

                int httpCode = (int)response.StatusCode;
                if (response.IsSuccessStatusCode)
                {
                    Stream stream= await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                    return stream;
                }
                else if (Array.Exists(this.RequestConfig.NoRetryServerErrorCodes, element => element == httpCode))
                {
                    // Do not retry this explicitly set 500 level server error
                    response.EnsureSuccessStatusCode();
                }
                else if (httpCode >= 500 && httpCode <= 599)
                {
                    // Retry 500 level server errors
                    continue;
                }
                else
                {
                    string contentType = response.Content.Headers.ContentType.MediaType;
                    if (contentType == "text/xml" || contentType == "application/xml")
                    {
                        Stream stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
                        return stream;
                    }
                    
                    // Throw exception for non-500 level errors
                    response.EnsureSuccessStatusCode();
                }
            }

            throw new HttpRequestException("Request retry count exceeded max retry count: " + this.RequestConfig.MaxRetries.ToString());
        }
    }
}
