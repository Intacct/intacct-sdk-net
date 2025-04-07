using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Intacct.SDK.Functions;
using Intacct.SDK.Xml;
using Intacct.SDK.Xml.Request;
using Xunit;

namespace Intacct.SDK.Tests
{
    public class OfflineClientTest
    {
        [Fact]
        public async Task ExecuteTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
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

            ClientConfig clientConfig = new ClientConfig()
            {
                SenderId = "testsender",
                SenderPassword = "testsendpass",
                SessionId = "testsession..",
                HttpMessageHandler = mockHandler,
            };

            RequestConfig requestConfig = new RequestConfig()
            {
                PolicyId = "asyncPolicyId",
            };

            OfflineClient client = new OfflineClient(clientConfig);

            OfflineResponse response = await client.Execute(new ApiSessionCreate("func1UnitTest"), requestConfig);

            Assert.Equal("requestUnitTest", response.Control.ControlId);
        }

        [Fact]
        public async Task ExecuteBatchTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
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

            ClientConfig clientConfig = new ClientConfig()
            {
                SenderId = "testsender",
                SenderPassword = "testsendpass",
                SessionId = "testsession..",
                HttpMessageHandler = mockHandler,
            };

            RequestConfig requestConfig = new RequestConfig()
            {
                PolicyId = "asyncPolicyId",
            };

            OfflineClient client = new OfflineClient(clientConfig);

            List<IFunction> contentBlock = new List<IFunction>
            {
                new ApiSessionCreate("func1UnitTest")
            };

            OfflineResponse response = await client.ExecuteBatch(contentBlock, requestConfig);

            Assert.Equal("requestUnitTest", response.Control.ControlId);
        }
    }
}