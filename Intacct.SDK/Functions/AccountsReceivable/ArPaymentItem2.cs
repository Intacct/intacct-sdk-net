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

using Intacct.SDK.Xml;

namespace Intacct.SDK.Functions.AccountsReceivable
{
    public class ArPaymentItem2 : IXmlObject
    {
        public decimal? AmountToApply;
        public int? ApplyToRecordId;

        public ArPaymentItem2()
        {
        }

        public void WriteXml(ref IaXmlWriter xml)
        {
            xml.WriteStartElement("arpymtdetail"); // "arpaymentitem"

            xml.WriteElement("recordkey", ApplyToRecordId, true); // "invoicekey"
            xml.WriteElement("trx_paymentamount", AmountToApply, true); // "amount"

            xml.WriteEndElement(); // arpymtdetail
        }
    }
}