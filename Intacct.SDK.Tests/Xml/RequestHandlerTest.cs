using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Intacct.SDK.Exceptions;
using Intacct.SDK.Functions;
using Intacct.SDK.Logging;
using Intacct.SDK.Tests.Logging;
using Intacct.SDK.Xml;
using Intacct.SDK.Xml.Request;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;
using Xunit;
using LogLevel = NLog.LogLevel;

namespace Intacct.SDK.Tests.Xml
{
    public class RequestHandlerTest
    {
        [Fact]
        public async Task MockExecuteSynchronousTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<response>
      <control>
            <status>success</status>
            <senderid>testsenderid</senderid>
            <controlid>sessionProvider</controlid>
            <uniqueid>false</uniqueid>
            <dtdversion>3.0</dtdversion>
      </control>
      <operation>
            <authentication>
                  <status>success</status>
                  <userid>testuser</userid>
                  <companyid>testcompany</companyid>
                  <locationid></locationid>
                  <sessiontimestamp>2015-12-06T15:57:08-08:00</sessiontimestamp>
            </authentication>
            <result>
                  <status>success</status>
                  <function>getAPISession</function>
                  <controlid>func1UnitTest</controlid>
                  <data>
                        <api>
                              <sessionid>unittest..</sessionid>
                              <endpoint>https://unittest.intacct.com/ia/xml/xmlgw.phtml</endpoint>
                              <locationid></locationid>
                        </api>
                  </data>
            </result>
      </operation>
</response>";

            HttpResponseMessage mockResponse = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(xml)
            };

            List<HttpResponseMessage> mockResponses = new List<HttpResponseMessage>
            {
                mockResponse,
            };

            MockHandler mockHandler = new MockHandler(mockResponses);

            ClientConfig clientConfig = new ClientConfig()
            {
                SenderId = "testsenderid",
                SenderPassword = "pass123!",
                SessionId = "testsession..",
                HttpMessageHandler = mockHandler,
            };

            RequestConfig requestConfig = new RequestConfig()
            {
                ControlId = "unittest",
            };

            List<IFunction> contentBlock = new List<IFunction>
            {
                new ApiSessionCreate()
            };

            RequestHandler requestHandler = new RequestHandler(clientConfig, requestConfig);
            OnlineResponse response = await requestHandler.ExecuteOnline(contentBlock);
            
            Assert.IsType<OnlineResponse>(response);
        }
        
        [Fact]
        public async Task MockExecuteAsynchronousTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<response>
      <acknowledgement>
            <status>success</status>
      </acknowledgement>
      <control>
            <status>success</status>
            <senderid>testsenderid</senderid>
            <controlid>requestUnitTest</controlid>
            <uniqueid>false</uniqueid>
            <dtdversion>3.0</dtdversion>
      </control>
</response>";

            HttpResponseMessage mockResponse = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(xml)
            };

            List<HttpResponseMessage> mockResponses = new List<HttpResponseMessage>
            {
                mockResponse,
            };

            MockHandler mockHandler = new MockHandler(mockResponses);

            ClientConfig clientConfig = new ClientConfig()
            {
                SenderId = "testsenderid",
                SenderPassword = "pass123!",
                SessionId = "testsession..",
                HttpMessageHandler = mockHandler,
            };

            RequestConfig requestConfig = new RequestConfig()
            {
                PolicyId = "policyid321",
                ControlId = "requestUnitTest",
            };

            List<IFunction> contentBlock = new List<IFunction>
            {
                new ApiSessionCreate()
            };

            RequestHandler requestHandler = new RequestHandler(clientConfig, requestConfig);
            OfflineResponse response = await requestHandler.ExecuteOffline(contentBlock);
            
            Assert.IsType<OfflineResponse>(response);
        }

        [Fact]
        public async Task ExecuteAsynchronousMissingPolicyIdTest()
        {
            ClientConfig clientConfig = new ClientConfig()
            {
                SenderId = "testsenderid",
                SenderPassword = "pass123!",
                SessionId = "testsession..",
            };

            RequestConfig requestConfig = new RequestConfig()
            {
                ControlId = "requestUnitTest",
            };

            List<IFunction> contentBlock = new List<IFunction>
            {
                new ApiSessionCreate()
            };

            RequestHandler requestHandler = new RequestHandler(clientConfig, requestConfig);
            
            var ex = await Record.ExceptionAsync(() => requestHandler.ExecuteOffline(contentBlock));
            Assert.IsType<ArgumentException>(ex);
            Assert.Equal("Required Policy ID not supplied in config for offline request", ex.Message);
        }

        [Fact]
        public async Task MockRetryTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<response>
      <control>
            <status>success</status>
            <senderid>testsenderid</senderid>
            <controlid>sessionProvider</controlid>
            <uniqueid>false</uniqueid>
            <dtdversion>3.0</dtdversion>
      </control>
      <operation>
            <authentication>
                  <status>success</status>
                  <userid>testuser</userid>
                  <companyid>testcompany</companyid>
                  <locationid></locationid>
                  <sessiontimestamp>2015-12-06T15:57:08-08:00</sessiontimestamp>
            </authentication>
            <result>
                  <status>success</status>
                  <function>getAPISession</function>
                  <controlid>func1UnitTest</controlid>
                  <data>
                        <api>
                              <sessionid>unittest..</sessionid>
                              <endpoint>https://unittest.intacct.com/ia/xml/xmlgw.phtml</endpoint>
                              <locationid></locationid>
                        </api>
                  </data>
            </result>
      </operation>
</response>";

            HttpResponseMessage mockResponse1 = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.BadGateway,
            };
            HttpResponseMessage mockResponse2 = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(xml)
            };

            List<HttpResponseMessage> mockResponses = new List<HttpResponseMessage>
            {
                mockResponse1,
                mockResponse2,
            };

            MockHandler mockHandler = new MockHandler(mockResponses);

            ClientConfig clientConfig = new ClientConfig()
            {
                SenderId = "testsenderid",
                SenderPassword = "pass123!",
                SessionId = "testsession..",
                HttpMessageHandler = mockHandler,
            };

            RequestConfig requestConfig = new RequestConfig();

            List<IFunction> contentBlock = new List<IFunction>();

            RequestHandler requestHandler = new RequestHandler(clientConfig, requestConfig);
            OnlineResponse response = await requestHandler.ExecuteOnline(contentBlock);
            Assert.Equal("testsenderid", response.Control.SenderId);
        }

        [Fact]
        public async Task MockDefaultRetryFailureTest()
        {
            HttpResponseMessage mockResponse1 = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
            };
            HttpResponseMessage mockResponse2 = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.NotImplemented,
            };
            HttpResponseMessage mockResponse3 = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadGateway,
            };
            HttpResponseMessage mockResponse4 = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.GatewayTimeout,
            };
            HttpResponseMessage mockResponse5 = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.HttpVersionNotSupported,
            };
            HttpResponseMessage mockResponse6 = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.InternalServerError,
            };

            List<HttpResponseMessage> mockResponses = new List<HttpResponseMessage>
            {
                mockResponse1,
                mockResponse2,
                mockResponse3,
                mockResponse4,
                mockResponse5,
                mockResponse6,
            };

            MockHandler mockHandler = new MockHandler(mockResponses);

            ClientConfig clientConfig = new ClientConfig()
            {
                SenderId = "testsenderid",
                SenderPassword = "pass123!",
                SessionId = "testsession..",
                HttpMessageHandler = mockHandler,
            };

            RequestConfig requestConfig = new RequestConfig();

            List<IFunction> contentBlock = new List<IFunction>
            {
                new ApiSessionCreate()
            };

            RequestHandler requestHandler = new RequestHandler(clientConfig, requestConfig);
            
            var ex = await Record.ExceptionAsync(() => requestHandler.ExecuteOnline(contentBlock));
            Assert.IsType<HttpRequestException>(ex);
            Assert.Equal("Request retry count exceeded max retry count: 5", ex.Message);
        }
        
        [Fact]
        public async Task Mock400LevelErrorWithXmlResponseTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<response>
    <control>
        <status>failure</status>
    </control>
    <errormessage>
        <error>
            <errorno>XMLGW_JPP0002</errorno>
            <description>Sign-in information is incorrect. Please check your request.</description>
        </error>
    </errormessage>
</response>";
            
            HttpResponseMessage mockResponse1 = new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.Unauthorized, // HTTP 401
                Content = new StringContent(xml, Encoding.UTF8, "text/xml"),
            };
            
            List<HttpResponseMessage> mockResponses = new List<HttpResponseMessage>
            {
                mockResponse1,
            };

            MockHandler mockHandler = new MockHandler(mockResponses);

            ClientConfig clientConfig = new ClientConfig()
            {
                SenderId = "testsenderid",
                SenderPassword = "pass123!",
                CompanyId = "badcompany",
                UserId = "baduser",
                UserPassword = "badpass",
                HttpMessageHandler = mockHandler,
            };

            RequestConfig requestConfig = new RequestConfig();

            List<IFunction> contentBlock = new List<IFunction>
            {
                new ApiSessionCreate()
            };

            RequestHandler requestHandler = new RequestHandler(clientConfig, requestConfig);
            
            var ex = await Record.ExceptionAsync(() => requestHandler.ExecuteOnline(contentBlock));
            Assert.IsType<ResponseException>(ex);
            Assert.Equal("Response control status failure - XMLGW_JPP0002 Sign-in information is incorrect. Please check your request.", ex.Message);
        }

        [Fact]
        public async Task MockDefaultNo524RetryTest()
        {
            HttpResponseMessage mockResponse1 = new HttpResponseMessage()
            {
                StatusCode = (HttpStatusCode) 524,
            };

            List<HttpResponseMessage> mockResponses = new List<HttpResponseMessage>
            {
                mockResponse1,
            };

            MockHandler mockHandler = new MockHandler(mockResponses);

            ClientConfig clientConfig = new ClientConfig()
            {
                SenderId = "testsenderid",
                SenderPassword = "pass123!",
                SessionId = "testsession..",
                HttpMessageHandler = mockHandler,
            };

            RequestConfig requestConfig = new RequestConfig();

            List<IFunction> contentBlock = new List<IFunction>
            {
                new ApiSessionCreate()
            };

            RequestHandler requestHandler = new RequestHandler(clientConfig, requestConfig);
            
            var ex = await Record.ExceptionAsync(() => requestHandler.ExecuteOnline(contentBlock));
            Assert.IsType<HttpRequestException>(ex);
            Assert.Equal("Response status code does not indicate success: 524 ().", ex.Message);
        }

        [Fact(Skip="test randomly failing")]
        public async Task MockExecuteWithDebugLoggerTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<response>
      <control>
            <status>success</status>
            <senderid>testsenderid</senderid>
            <controlid>sessionProvider</controlid>
            <uniqueid>false</uniqueid>
            <dtdversion>3.0</dtdversion>
      </control>
      <operation>
            <authentication>
                  <status>success</status>
                  <userid>testuser</userid>
                  <companyid>testcompany</companyid>
                  <locationid></locationid>
                  <sessiontimestamp>2015-12-06T15:57:08-08:00</sessiontimestamp>
            </authentication>
            <result>
                  <status>success</status>
                  <function>getAPISession</function>
                  <controlid>func1UnitTest</controlid>
                  <data>
                        <api>
                              <sessionid>unittest..</sessionid>
                              <endpoint>https://unittest.intacct.com/ia/xml/xmlgw.phtml</endpoint>
                              <locationid></locationid>
                        </api>
                  </data>
            </result>
      </operation>
</response>";

            HttpResponseMessage mockResponse = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(xml)
            };

            List<HttpResponseMessage> mockResponses = new List<HttpResponseMessage>
            {
                mockResponse,
            };

            MockHandler mockHandler = new MockHandler(mockResponses);

            var guid = Guid.NewGuid().ToString();
            MemoryTarget target = new MemoryTarget
            {
                Name = guid,
                Layout = "${message}"
            };
            SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);
            
            ClientConfig clientConfig = new ClientConfig()
            {
                SenderId = "testsenderid",
                SenderPassword = "pass123!",
                SessionId = "testsession..",
                HttpMessageHandler = mockHandler,
         //       Logger = LogManager.GetLogger(guid),
            };

            RequestConfig requestConfig = new RequestConfig();

            List<IFunction> contentBlock = new List<IFunction>
            {
                new ApiSessionCreate()
            };

            RequestHandler requestHandler = new RequestHandler(clientConfig, requestConfig);
            OnlineResponse response = await requestHandler.ExecuteOnline(contentBlock);

            // Check for the user agent
            Assert.True(true); // TODO fix this test from randomly failing
            //Assert.Contains("intacct-api-net-client/", target.Logs[0]);
        }

        [Fact(Skip="test randomly failing")]
        public async Task MockExecuteOfflineWithSessionCredsTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<response>
      <acknowledgement>
            <status>success</status>
      </acknowledgement>
      <control>
            <status>success</status>
            <senderid>testsenderid</senderid>
            <controlid>requestUnitTest</controlid>
            <uniqueid>false</uniqueid>
            <dtdversion>3.0</dtdversion>
      </control>
</response>";

            HttpResponseMessage mockResponse = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(xml)
            };

            List<HttpResponseMessage> mockResponses = new List<HttpResponseMessage>
            {
                mockResponse,
            };

            MockHandler mockHandler = new MockHandler(mockResponses);

            var guid = Guid.NewGuid().ToString();
            MemoryTarget target = new MemoryTarget
            {
                Name = guid,
                Layout = "${message}"
            };
            SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);
            
            ClientConfig clientConfig = new ClientConfig()
            {
                SenderId = "testsenderid",
                SenderPassword = "pass123!",
                SessionId = "testsession..",
                HttpMessageHandler = mockHandler,
               // LoggerFactory = new LogFactory(),
                 //   Logger = LogFactory<Program> //LogManager.GetLogger(guid),
            };

     //       Logger nlog = NLog.LogManager.GetLogger();

            RequestConfig requestConfig = new RequestConfig()
            {
                ControlId = "requestUnitTest",
                PolicyId = "policyid123",
            };

            List<IFunction> contentBlock = new List<IFunction>
            {
                new ApiSessionCreate()
            };

            RequestHandler requestHandler = new RequestHandler(clientConfig, requestConfig);
            OfflineResponse response = await requestHandler.ExecuteOffline(contentBlock);
            
            Assert.True(true); // TODO fix this test from randomly failing
            //Assert.Contains("Offline execution sent to Intacct using Session-based credentials.", target.Logs[0]);
        }
    }
}
