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

using Intacct.SDK.Functions.AccountsReceivable;
using Intacct.SDK.Tests.Xml;
using System;
using Xunit;

namespace Intacct.SDK.Tests.Functions.AccountsReceivable
{
    public class ArPaymentCreate2Test : XmlObjectTestHelper
    {
        [Fact]
        public void GetXmlTest()
        {
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<function controlid=""unittest"">
    <create>
        <arpymt>
            <paymentmethod>Printed Check</paymentmethod>
            <customerid>C0020</customerid>
            <docnumber>1000</docnumber>
            <description>Test Memo</description>
            <receiptdate>06/30/2021</receiptdate>
            <amounttopay />
            <trx_amounttopay>1922.12</trx_amounttopay>
            <prbatch>123</prbatch>
            <whenpaid>07/01/2021</whenpaid>
            <overpaymentamount />
            <overpaymentlocationid>1020</overpaymentlocationid>
            <overpaymentdepartmentid>900</overpaymentdepartmentid>
        </arpymt>
    </create>
</function>";

            ArPaymentCreate2 record = new ArPaymentCreate2("unittest")
            {
                CustomerId = "C0020",
                TransactionPaymentAmount = 1922.12M,
                SummaryRecordNo = 123,
                ReceivedDate = new DateTime(2021, 06, 30),
                PaymentMethod = "Printed Check",
                ReferenceNumber = "1000",
                Description = "Test Memo",
                OverpaymentDepartmentId = "900",
                OverpaymentLocationId = "1020",
                WhenPaidDate = new DateTime(2021, 07, 01)
            };

            this.CompareXml(expected, record);
        }
    }
}