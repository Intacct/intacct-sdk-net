﻿/*
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

using System.Collections.Generic;
using Intacct.SDK.Functions.Projects;
using Intacct.SDK.Tests.Xml;
using Xunit;

namespace Intacct.SDK.Tests.Functions.Projects
{
    public class ProjectCreateTest : XmlObjectTestHelper
    {
        [Fact]
        public void GetXmlTest()
        {
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<function controlid=""unittest"">
    <create>
        <PROJECT>
            <NAME>hello world</NAME>
            <PROJECTCATEGORY>Contract</PROJECTCATEGORY>
        </PROJECT>
    </create>
</function>";

            ProjectCreate record = new ProjectCreate("unittest")
            {
                ProjectName = "hello world",
                ProjectCategory = "Contract"
            };
            this.CompareXml(expected, record);
        }
        
        [Fact]
        public void GetXmlWithNullValueCustomFieldTest()
        {
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<function controlid=""unittest"">
    <create>
        <PROJECT>
            <NAME>hello world</NAME>
            <PROJECTCATEGORY>Contract</PROJECTCATEGORY>
            <customfield1 />
        </PROJECT>
    </create>
</function>";

            ProjectCreate record = new ProjectCreate("unittest")
            {
                ProjectName = "hello world",
                ProjectCategory = "Contract",
                CustomFields = new Dictionary<string, dynamic>
                {
                    { "customfield1", null }
                },
            };
            this.CompareXml(expected, record);
        }
    }
}