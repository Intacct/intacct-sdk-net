﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Intacct.SDK.Xml.Request;
using Xunit;

namespace Intacct.SDK.Tests
{
    public class SessionProviderTest
    {
        [Fact]
        public async Task FromLoginCredentialsTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
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
                  <function>getSession</function>
                  <controlid>testControlId</controlid>
                  <data>
                        <api>
                              <sessionid>fAkESesSiOnId..</sessionid>
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

            ClientConfig config = new ClientConfig()
            {
                SenderId = "testsenderid",
                SenderPassword = "pass123!",
                CompanyId = "testcompany",
                UserId = "testuser",
                UserPassword = "testpass",
                HttpMessageHandler = mockHandler,
            };

            ClientConfig sessionCreds = await SessionProvider.Factory(config);

            Assert.Equal("fAkESesSiOnId..", sessionCreds.SessionId);
            Assert.Equal("https://unittest.intacct.com/ia/xml/xmlgw.phtml", sessionCreds.EndpointUrl);
            Assert.Equal("", sessionCreds.EntityId);
        }
          
        [Fact]
        public async Task FromLoginCredentialsWithEntityTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
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
                  <locationid>testentity</locationid>
                  <sessiontimestamp>2015-12-06T15:57:08-08:00</sessiontimestamp>
            </authentication>
            <result>
                  <status>success</status>
                  <function>getSession</function>
                  <controlid>testControlId</controlid>
                  <data>
                        <api>
                              <sessionid>fAkESesSiOnId..</sessionid>
                              <endpoint>https://unittest.intacct.com/ia/xml/xmlgw.phtml</endpoint>
                              <locationid>testentity</locationid>
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

            ClientConfig config = new ClientConfig()
            {
                SenderId = "testsenderid",
                SenderPassword = "pass123!",
                CompanyId = "testcompany",
                EntityId= "testentity",
                UserId = "testuser",
                UserPassword = "testpass",
                HttpMessageHandler = mockHandler,
            };

            ClientConfig sessionCreds = await SessionProvider.Factory(config);

            Assert.Equal("fAkESesSiOnId..", sessionCreds.SessionId);
            Assert.Equal("https://unittest.intacct.com/ia/xml/xmlgw.phtml", sessionCreds.EndpointUrl);
            Assert.Equal("testentity", sessionCreds.EntityId);
        }

        [Fact]
        public async Task FromSessionCredentialsTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
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
                  <function>getSession</function>
                  <controlid>testControlId</controlid>
                  <data>
                        <api>
                              <sessionid>fAkESesSiOnId..</sessionid>
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

            ClientConfig config = new ClientConfig()
            {
                SenderId = "testsenderid",
                SenderPassword = "pass123!",
                SessionId = "fAkESesSiOnId..",
                EndpointUrl = "https://unittest.intacct.com/ia/xml/xmlgw.phtml",
                HttpMessageHandler = mockHandler,
            };

            ClientConfig sessionCreds = await SessionProvider.Factory(config);
            
            Assert.Equal("fAkESesSiOnId..", sessionCreds.SessionId);
            Assert.Equal("https://unittest.intacct.com/ia/xml/xmlgw.phtml", sessionCreds.EndpointUrl);
            Assert.Equal("", sessionCreds.EntityId);
        }

        [Fact]
        public async Task FromTopLevelSessionCredentialsWithEntityOverrideTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
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
                  <function>getSession</function>
                  <controlid>testControlId</controlid>
                  <data>
                        <api>
                              <sessionid>fAkESesSiOnId..</sessionid>
                              <endpoint>https://unittest.intacct.com/ia/xml/xmlgw.phtml</endpoint>
                              <locationid>testentity</locationid>
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

            ClientConfig config = new ClientConfig()
            {
                SenderId = "testsenderid",
                SenderPassword = "pass123!",
                SessionId = "fAkESesSiOnId..",
                EndpointUrl = "https://unittest.intacct.com/ia/xml/xmlgw.phtml",
                HttpMessageHandler = mockHandler,
                EntityId = "testentity",
            };

            ClientConfig sessionCreds = await SessionProvider.Factory(config);
            
            Assert.Equal("fAkESesSiOnId..", sessionCreds.SessionId);
            Assert.Equal("https://unittest.intacct.com/ia/xml/xmlgw.phtml", sessionCreds.EndpointUrl);
            Assert.Equal("testentity", sessionCreds.EntityId);
        }

        [Fact]
        public async Task FromPrivateEntitySessionCredentialsWithDifferentEntityOverrideTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
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
                  <locationid>entityA</locationid>
                  <sessiontimestamp>2015-12-06T15:57:08-08:00</sessiontimestamp>
            </authentication>
            <result>
                  <status>success</status>
                  <function>getSession</function>
                  <controlid>testControlId</controlid>
                  <data>
                        <api>
                              <sessionid>EntityBSession..</sessionid>
                              <endpoint>https://unittest.intacct.com/ia/xml/xmlgw.phtml</endpoint>
                              <locationid>entityB</locationid>
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

            ClientConfig config = new ClientConfig()
            {
                SenderId = "testsenderid",
                SenderPassword = "pass123!",
                SessionId = "EntityAsession..",
                EndpointUrl = "https://unittest.intacct.com/ia/xml/xmlgw.phtml",
                HttpMessageHandler = mockHandler,
                EntityId = "entityB",
            };

            ClientConfig sessionCreds = await SessionProvider.Factory(config);
            
            Assert.Equal("EntityBSession..", sessionCreds.SessionId);
            Assert.Equal("https://unittest.intacct.com/ia/xml/xmlgw.phtml", sessionCreds.EndpointUrl);
            Assert.Equal("entityB", sessionCreds.EntityId);
        }
        
        [Fact]
        public async Task FromSessionCredentialsUsingEnvironmentSenderTest()
        {
            string xml = @"<?xml version=""1.0"" encoding=""utf-8""?>
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
                  <function>getSession</function>
                  <controlid>testControlId</controlid>
                  <data>
                        <api>
                              <sessionid>fAkESesSiOnId..</sessionid>
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

            Environment.SetEnvironmentVariable("INTACCT_SENDER_ID", "envsender");
            Environment.SetEnvironmentVariable("INTACCT_SENDER_PASSWORD", "envpass");

            ClientConfig config = new ClientConfig()
            {
                SessionId = "fAkESesSiOnId..",
                EndpointUrl = "https://unittest.intacct.com/ia/xml/xmlgw.phtml",
                HttpMessageHandler = mockHandler,
            };

            ClientConfig sessionCreds = await SessionProvider.Factory(config);
            
            Assert.Equal("fAkESesSiOnId..", sessionCreds.SessionId);
            Assert.Equal("https://unittest.intacct.com/ia/xml/xmlgw.phtml", sessionCreds.EndpointUrl);
            Assert.Equal("envsender", sessionCreds.SenderId);
            Assert.Equal("envpass", sessionCreds.SenderPassword);

            Environment.SetEnvironmentVariable("INTACCT_SENDER_ID", null);
            Environment.SetEnvironmentVariable("INTACCT_SENDER_PASSWORD", null);
        }
    }
}