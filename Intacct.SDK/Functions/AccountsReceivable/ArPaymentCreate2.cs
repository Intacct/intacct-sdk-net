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
    /// <summary>
    /// This class creates a payment using the non-legacy version of the API.
    /// </summary>
    public class ArPaymentCreate2 : AbstractArPayment2
    {
        public ArPaymentCreate2(string controlId = null) : base(controlId)
        {
        }

        public override void WriteXml(ref IaXmlWriter xml)
        {
            xml.WriteStartElement("function");
            xml.WriteAttribute("controlid", ControlId, true);

            xml.WriteStartElement("create");

            xml.WriteStartElement("arpymt");

            if (!string.IsNullOrWhiteSpace(UndepositedFundsGlAccountNo))
            {
                xml.WriteElement("undepositedaccountno", BankAccountId); // undepfundsacct
            }
            else
            {
                xml.WriteElement("financialentity", BankAccountId); // bankaccountid
            }

            xml.WriteElement("paymentmethod", PaymentMethod, true);
            xml.WriteElement("customerid", CustomerId, true);
            xml.WriteElement("docnumber", ReferenceNumber); // "refid"
            xml.WriteElement("description", Description);

            //if (ExchangeRateDate.HasValue)
            //{
            //    xml.WriteStartElement("exchratedate");
            //    xml.WriteDateSplitElements2(ExchangeRateDate.Value);
            //    xml.WriteEndElement(); //exchratedate
            //}

            if (!string.IsNullOrWhiteSpace(ExchangeRateType))
            {
                xml.WriteElement("exch_rate_type_id", ExchangeRateType); // "exchratetype"
            }
            else if (ExchangeRateValue.HasValue)
            {
                xml.WriteElement("exchange_rate", ExchangeRateValue); // "exchrate"
            }
            else if (!string.IsNullOrWhiteSpace(BaseCurrency) || !string.IsNullOrWhiteSpace(TransactionCurrency))
            {
                xml.WriteElement("exchratetype", ExchangeRateType, true);
            }

            xml.WriteStartElement("receiptdate"); // "datereceived"
            xml.WriteDateMMddyyyy(ReceivedDate.Value); // Required element
            xml.WriteEndElement(); //receiptdate

            if (PaymentDate.HasValue)
            {
                xml.WriteStartElement("paymentdate");
                xml.WriteDateMMddyyyy(PaymentDate.Value);
                xml.WriteEndElement(); //paymentdate
            }

            xml.WriteElement("amounttopay", BasePaymentAmount, true); // "translatedamount"
            xml.WriteElement("trx_amounttopay", TransactionPaymentAmount, true); // paymentamount

            xml.WriteElement("prbatch", SummaryRecordNo); // batchkey

            if (WhenPaidDate.HasValue)
            {
                xml.WriteStartElement("whenpaid");
                xml.WriteDateMMddyyyy(WhenPaidDate.Value);
                xml.WriteEndElement(); //paymentdate
            }

            xml.WriteElement("currency", TransactionCurrency);
            xml.WriteElement("basecurr", BaseCurrency);

            xml.WriteElement("undepositedaccountno", UndepositedFundsGlAccountNo);

            xml.WriteElement("overpaymentamount", OverpaymentAmount, true);
            xml.WriteElement("overpaymentlocationid", OverpaymentLocationId); // "overpaylocid"
            xml.WriteElement("overpaymentdepartmentid", OverpaymentDepartmentId); // "overpaydeptid"

            xml.WriteElement("billtopayname", BillToPayName);

            //xml.WriteElement("cctype", CreditCardType);
            //xml.WriteElement("authcode", AuthorizationCode);

            if (ApplyToTransactions.Count > 0)
            {
                xml.WriteStartElement("arpymtdetails"); // "arpaymentitem"

                foreach (ArPaymentItem2 applyToTransaction in ApplyToTransactions)
                {
                    applyToTransaction.WriteXml(ref xml);
                }

                xml.WriteEndElement(); // arpymtdetails
            }

            // TODO online payment methods

            xml.WriteEndElement(); //create

            xml.WriteEndElement(); //arpymt

            xml.WriteEndElement(); //function
        }
    }
}