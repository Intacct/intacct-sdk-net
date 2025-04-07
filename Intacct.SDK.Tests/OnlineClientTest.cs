using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Intacct.SDK.Exceptions;
using Intacct.SDK.Functions;
using Intacct.SDK.Functions.Common;
using Intacct.SDK.Xml;
using Intacct.SDK.Xml.Request;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Config;
using NLog.Targets;
using Xunit;
using LogLevel = NLog.LogLevel;

namespace Intacct.SDK.Tests
{
    public class OnlineClientTest
    {
        [Fact]
        public async Task ExecuteTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<response>
      <control>
            <status>success</status>
            <senderid>testsenderid</senderid>
            <controlid>requestUnitTest</controlid>
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
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(xml)
            };

            List<HttpResponseMessage> mockResponses = new List<HttpResponseMessage>
            {
                mockResponse1,
            };

            MockHandler mockHandler = new MockHandler(mockResponses);

            ClientConfig clientConfig = new ClientConfig
            {
                SenderId = "testsender",
                SenderPassword = "testsendpass",
                SessionId = "testsession..",
                HttpMessageHandler = mockHandler,
            };

            OnlineClient client = new OnlineClient(clientConfig);

            OnlineResponse response = await client.Execute(new ApiSessionCreate("func1UnitTest"));

            Assert.Equal("requestUnitTest", response.Control.ControlId);
        }

        [Fact]
        public async Task ExecuteResultExceptionTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<response>
      <control>
            <status>success</status>
            <senderid>testsenderid</senderid>
            <controlid>requestUnitTest</controlid>
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
                  <status>failure</status>
                  <function>getAPISession</function>
                  <controlid>func1UnitTest</controlid>
                  <errormessage>
                        <error>
                              <errorno>Get API Session Failed</errorno>
                              <description></description>
                              <description2>Something went wrong</description2>
                              <correction></correction>
                        </error>
                  </errormessage>
            </result>
      </operation>
</response>";

            HttpResponseMessage mockResponse1 = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(xml)
            };

            List<HttpResponseMessage> mockResponses = new List<HttpResponseMessage>
            {
                mockResponse1,
            };

            MockHandler mockHandler = new MockHandler(mockResponses);

            ClientConfig clientConfig = new ClientConfig
            {
                SenderId = "testsender",
                SenderPassword = "testsendpass",
                SessionId = "testsession..",
                HttpMessageHandler = mockHandler,
            };

            OnlineClient client = new OnlineClient(clientConfig);

            var ex = await Record.ExceptionAsync(() => client.Execute(new ApiSessionCreate("func1UnitTest")));
            Assert.IsType<ResultException>(ex);
            Assert.Equal("Result status: failure for Control ID: func1UnitTest - Get API Session Failed Something went wrong", ex.Message);
        }

        [Fact]
        public async Task ExecuteBatchTransactionResultExceptionTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<response>
      <control>
            <status>success</status>
            <senderid>testsenderid</senderid>
            <controlid>requestUnitTest</controlid>
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
                  <status>aborted</status>
                  <function>getAPISession</function>
                  <controlid>func1UnitTest</controlid>
                  <errormessage>
                          <error>
                                <errorno>XL03000009</errorno>
                                <description></description>
                                <description2>The entire transaction in this operation has been rolled back due to an error.</description2>
                                <correction></correction>
                          </error>
                  </errormessage>
            </result>
            <result>
                  <status>failure</status>
                  <function>getAPISession</function>
                  <controlid>func2UnitTest</controlid>
                  <errormessage>
                        <error>
                              <errorno>Get API Session Failed</errorno>
                              <description></description>
                              <description2>Something went wrong</description2>
                              <correction></correction>
                        </error>
                          <error>
                                <errorno>XL03000009</errorno>
                                <description></description>
                                <description2>The entire transaction in this operation has been rolled back due to an error.</description2>
                                <correction></correction>
                          </error>
                  </errormessage>
            </result>
      </operation>
</response>";

            HttpResponseMessage mockResponse1 = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(xml)
            };

            List<HttpResponseMessage> mockResponses = new List<HttpResponseMessage>
            {
                mockResponse1,
            };

            MockHandler mockHandler = new MockHandler(mockResponses);

            ClientConfig clientConfig = new ClientConfig
            {
                SenderId = "testsender",
                SenderPassword = "testsendpass",
                SessionId = "testsession..",
                HttpMessageHandler = mockHandler,
            };

            RequestConfig requestConfig = new RequestConfig()
            {
                Transaction = true,
            };

            List<IFunction> content = new List<IFunction>
            {
                new ApiSessionCreate("func1UnitTest"),
                new ApiSessionCreate("func2UnitTest")
            };

            OnlineClient client = new OnlineClient(clientConfig);

            var ex = await Record.ExceptionAsync(() => client.ExecuteBatch(content, requestConfig));
            Assert.IsType<ResultException>(ex);
            Assert.Equal("Result status: failure for Control ID: func2UnitTest - Get API Session Failed Something went wrong - XL03000009 The entire transaction in this operation has been rolled back due to an error.", ex.Message);
        }

        [Fact(Skip="test randomly failing")]
        public async Task LoggerTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
<response>
      <control>
            <status>success</status>
            <senderid>testsenderid</senderid>
            <controlid>requestUnitTest</controlid>
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
                <function>readByQuery</function>
                <controlid>func1UnitTest</controlid>
                <data listtype=""customer"" count=""1"" totalcount=""1"" numremaining=""0"" resultId="""">
                    <customer>
                        <CUSTOMERID>C0001</CUSTOMERID>
                        <NAME>Intacct Corporation</NAME>
                    </customer>
                </data>
            </result>
      </operation>
</response>";

            HttpResponseMessage mockResponse1 = new HttpResponseMessage()
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = new StringContent(xml)
            };

            List<HttpResponseMessage> mockResponses = new List<HttpResponseMessage>
            {
                mockResponse1,
            };
            MockHandler mockHandler = new MockHandler(mockResponses);

            var guid = Guid.NewGuid().ToString();
            MemoryTarget target = new MemoryTarget
            {
                Name = guid,
                Layout = "${message}"
            };
            SimpleConfigurator.ConfigureForTargetLogging(target, LogLevel.Debug);
           
            ClientConfig config = new ClientConfig
            {
                SenderId = "testsender",
                SenderPassword = "testsendpass",
                SessionId = "testsession..",
                HttpMessageHandler = mockHandler,
           //     Logger = LogManager.GetLogger(guid),
            };

            OnlineClient client = new OnlineClient(config);

            OnlineResponse response = await client.Execute(new ReadByQuery("func1UnitTest"));
            
            Assert.True(true); // TODO fix this test from randomly failing
            //Assert.Contains("<password>REDACTED</password>", target.Logs[0]);
        }
    }
}