﻿/*
 * Copyright 2017 Intacct Corporation.
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

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Intacct.Sdk.Tests.Helpers;
using Intacct.Sdk.Credentials;
using System.Net.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using IniParser;
using System.IO;
using System.Text;
using IniParser.Model;
using System.IO.Abstractions.TestingHelpers;

namespace Intacct.Sdk.Tests.Credentials
{

    [TestClass()]
    public class ProfileCredentialProviderTest
    {

        [TestMethod()]
        public void GetCredentialsFromDefaultProfileTest()
        {
            string ini = @"
[default]
sender_id = defsenderid
sender_password = defsenderpass
company_id = defcompanyid
user_id = defuserid
user_password = defuserpass
endpoint_url = https://unittest.intacct.com/ia/xmlgw.phtml

[unittest]
company_id = inicompanyid
user_id = iniuserid
user_password = iniuserpass";

            string tempFile = Path.Combine(Path.GetTempPath(), ".intacct", "credentials.ini");

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { tempFile, new MockFileData(ini) },
            });

            ClientConfig config = new ClientConfig()
            {
                ProfileFile = tempFile,
            };

            ClientConfig loginCreds = ProfileCredentialProvider.GetLoginCredentials(config, fileSystem);
            
            Assert.AreEqual("defcompanyid", loginCreds.CompanyId);
            Assert.AreEqual("defuserid", loginCreds.UserId);
            Assert.AreEqual("defuserpass", loginCreds.UserPassword);

            ClientConfig senderCreds = ProfileCredentialProvider.GetSenderCredentials(config, fileSystem);
            
            Assert.AreEqual("defsenderid", senderCreds.SenderId);
            Assert.AreEqual("defsenderpass", senderCreds.SenderPassword);
            Assert.AreEqual("https://unittest.intacct.com/ia/xmlgw.phtml", senderCreds.EndpointUrl);
        }

        [TestMethod()]
        public void GetLoginCredentialsFromSpecificProfileTest()
        {
            string ini = @"
[default]
sender_id = defsenderid
sender_password = defsenderpass
company_id = defcompanyid
user_id = defuserid
user_password = defuserpass

[unittest]
company_id = inicompanyid
user_id = iniuserid
user_password = iniuserpass";

            string tempFile = Path.Combine(Path.GetTempPath(), ".intacct", "credentials.ini");

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { tempFile, new MockFileData(ini) },
            });

            ClientConfig config = new ClientConfig()
            {
                ProfileFile = tempFile,
                ProfileName = "unittest",
            };

            ClientConfig loginCreds = ProfileCredentialProvider.GetLoginCredentials(config, fileSystem);
            
            Assert.AreEqual("inicompanyid", loginCreds.CompanyId);
            Assert.AreEqual("iniuserid", loginCreds.UserId);
            Assert.AreEqual("iniuserpass", loginCreds.UserPassword);
        }

        [TestMethod()]
        [ExpectedExceptionWithMessage(typeof(ArgumentException), "Profile name \"default\" not found in credentials file")]
        public void GetLoginCredentialsMissingDefault()
        {
            string ini = @"
[notdefault]
sender_id = testsenderid
sender_password = testsenderpass";

            string tempFile = Path.Combine(Path.GetTempPath(), ".intacct", "credentials.ini");

            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { tempFile, new MockFileData(ini) },
            });

            ClientConfig config = new ClientConfig()
            {
                ProfileFile = tempFile,
            };

            ClientConfig loginCreds = ProfileCredentialProvider.GetLoginCredentials(config, fileSystem);
        }
    }
}