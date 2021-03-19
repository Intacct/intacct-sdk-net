/*
 * Copyright 2020 Sage Intacct, Inc.
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

using Intacct.SDK.Logging;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Intacct.SDK.Xml
{
    public class LoggingHandler : DelegatingHandler
    {
        private ILogger _logger;

        private LogLevel _logLevel;
        private MessageFormatter _logMessageFormatter;

        public LoggingHandler(HttpMessageHandler innerHandler, ILogger logger, MessageFormatter logMessageFormat, LogLevel logLevel) : base(innerHandler)
        {
            this._logger = logger;
            this._logMessageFormatter = logMessageFormat;
            this._logLevel = logLevel;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpResponseMessage response = null;

            try
            {
                response = await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
                _logger.Log(_logLevel, _logMessageFormatter.Format(request, response));
            }
            catch (Exception ex)
            {
                _logger.Log(_logLevel, ex, _logMessageFormatter.Format(request, response));
                throw;
            }

            return response;
        }
    }
}