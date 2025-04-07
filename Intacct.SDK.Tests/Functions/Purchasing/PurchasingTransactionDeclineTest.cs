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

using Intacct.SDK.Functions.Purchasing;
using Intacct.SDK.Tests.Xml;
using Xunit;

namespace Intacct.SDK.Tests.Functions.Purchasing
{
    public class PurchasingTransactionDeclineTest : XmlObjectTestHelper
    {
        [Fact]
        public void GetXmlTest()
        {
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<function controlid=""unittest"">
    <decline>
        <PODOCUMENT>
            <DOCID>Purchase Order-PO0213</DOCID>
            <COMMENT>Need to wait on this</COMMENT>
        </PODOCUMENT>
    </decline>
</function>";

            PurchasingTransactionDecline record = new PurchasingTransactionDecline("unittest")

            {
                DocumentId = "Purchase Order-PO0213",
                Comment = "Need to wait on this"
            };

            this.CompareXml(expected, record);
        }
    }
}