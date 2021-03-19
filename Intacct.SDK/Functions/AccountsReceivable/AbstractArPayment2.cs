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

using System;
using System.Collections.Generic;

namespace Intacct.SDK.Functions.AccountsReceivable
{
    public abstract class AbstractArPayment2 : AbstractFunction
    {
        /// <summary>
        /// ARPYMTDETAILS
        /// </summary>
        public List<ArPaymentItem2> ApplyToTransactions = new List<ArPaymentItem2>();

        /// <summary>
        /// FINANCIALENTITY
        /// </summary>
        public string BankAccountId;

        /// <summary>
        /// BASECURR
        /// </summary>
        public string BaseCurrency;

        /// <summary>
        /// AMOUNTOPAY
        /// </summary>
        public decimal? BasePaymentAmount;

        public string BillToPayName;
        public string CustomerId;
        public string Description;

        /// <summary>
        /// EXCH_RATE_TYPE_ID
        /// </summary>
        public string ExchangeRateType;

        /// <summary>
        /// EXCHANGE_RATE
        /// </summary>
        public decimal? ExchangeRateValue;

        /// <summary>
        /// OVERPAYMENTAMOUNT
        /// </summary>
        public decimal? OverpaymentAmount;

        public string OverpaymentDepartmentId;
        public string OverpaymentLocationId;

        /// <summary>
        /// PAYMENTDATE
        /// </summary>
        public DateTime? PaymentDate;

        public string PaymentMethod;

        /// <summary>
        /// RECEIPTDATE
        /// </summary>
        public DateTime? ReceivedDate;

        /// <summary>
        /// DOCNUMBER
        /// </summary>
        public string ReferenceNumber;

        /// <summary>
        /// PRBATCH
        /// </summary>
        public int? SummaryRecordNo;

        /// <summary>
        /// CURRENCY
        /// </summary>
        public string TransactionCurrency;

        /// <summary>
        /// TRX_AMOUNTTOPAY
        /// </summary>
        public decimal? TransactionPaymentAmount;

        /// <summary>
        /// UNDEPOSITEDACCOUNTNO
        /// </summary>
        public string UndepositedFundsGlAccountNo;

        /// <summary>
        /// WHENPAID
        /// </summary>
        public DateTime? WhenPaidDate;

        #region Unmappable AbstractArPayment Fields

        /// public string CreditCardType;

        // public DateTime? ExchangeRateDate;

        // public string AuthorizationCode;

        // public int? RecordNo;

        #endregion Unmappable AbstractArPayment Fields

        protected AbstractArPayment2(string controlId = null) : base(controlId)
        {
        }
    }
}