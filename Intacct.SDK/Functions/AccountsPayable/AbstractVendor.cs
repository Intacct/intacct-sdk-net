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

using System.Collections.Generic;
using Intacct.SDK.Functions.Company;
using Intacct.SDK.Xml;

namespace Intacct.SDK.Functions.AccountsPayable
{
    public abstract class AbstractVendor : AbstractFunction
    {

        public string VendorId;

        public string VendorName;

        public bool? OneTime;

        public bool? Active;

        public string LastName;

        public string FirstName;

        public string MiddleName;

        public string Prefix;

        public string CompanyName;

        public string PrintAs;

        public string PrimaryPhoneNo;

        public string SecondaryPhoneNo;

        public string CellularPhoneNo;

        public string PagerNo;

        public string FaxNo;

        public string PrimaryEmailAddress;

        public string SecondaryEmailAddress;

        public string PrimaryUrl;

        public string SecondaryUrl;

        public string AddressLine1;

        public string AddressLine2;

        public string City;

        public string StateProvince;

        public string ZipPostalCode;

        public string Country;

        public string IsoCountryCode;

        public bool? ExcludedFromContactList;

        public string VendorTypeId;

        public string ParentVendorId;

        public string GlGroupName;

        public string TaxId;

        public string Form1099Name;

        public string Form1099Type;

        public string Form1099Box;

        public string AttachmentsId;

        public string DefaultExpenseGlAccountNo;

        public bool? Taxable;

        public string ContactTaxGroupName;

        public decimal? CreditLimit;

        public bool? OnHold;

        public bool? DoNotPay;

        public string Comments;

        public string DefaultCurrency;

        public string PrimaryContactName;

        public string PayToContactName;

        public string ReturnToContactName;

        public List<ContactListInfo> ContactList = new List<ContactListInfo>();

        public string PreferredPaymentMethod;

        public bool? SendAutomaticPaymentNotification;

        public bool? MergePaymentRequests;

        public string VendorBillingType;

        public string PaymentPriority;

        public string PaymentTerm;

        public bool? TermDiscountDisplayedOnCheckStub;

        public bool? AchEnabled;

        public string AchBankRoutingNo;

        public string AchBankAccountNo;

        public string AchBankAccountType;

        public string AchBankAccountClass;

        // TODO Check delivery and ACH payment services

        public string VendorAccountNo;

        public bool? LocationAssignedAccountNoDisplayedOnCheckStub;

        // TODO Location assigned vendor account no's

        public string RestrictionType;

        public List<string> RestrictedLocations = new List<string>();

        public List<string> RestrictedDepartments = new List<string>();

        public Dictionary<string, dynamic> CustomFields = new Dictionary<string, dynamic>();

        protected AbstractVendor(string controlId = null) : base(controlId)
        {
        }

        protected void WriteXmlMailAddress(ref IaXmlWriter xml)
        {
            if (
                !string.IsNullOrWhiteSpace(AddressLine1)
                || !string.IsNullOrWhiteSpace(AddressLine2)
                || !string.IsNullOrWhiteSpace(City)
                || !string.IsNullOrWhiteSpace(StateProvince)
                || !string.IsNullOrWhiteSpace(ZipPostalCode)
                || !string.IsNullOrWhiteSpace(Country)
                || !string.IsNullOrWhiteSpace(IsoCountryCode)
            )
            {
                xml.WriteStartElement("MAILADDRESS");

                xml.WriteElementString("ADDRESS1", AddressLine1);
                xml.WriteElementString("ADDRESS2", AddressLine2);
                xml.WriteElementString("CITY", City);
                xml.WriteElementString("STATE", StateProvince);
                xml.WriteElementString("ZIP", ZipPostalCode);
                xml.WriteElementString("COUNTRY", Country);
                xml.WriteElementString("ISOCOUNTRYCODE", IsoCountryCode);

                xml.WriteEndElement(); //MAILADDRESS
            }
        }

        protected void WriteXmlContactListInfo(ref IaXmlWriter xml)
        {
            if (this.ContactList != null)
            {
                foreach (ContactListInfo contactListInfo in this.ContactList)
                {
                    contactListInfo.WriteXmlContactListInfo(ref xml);
                }
            }
        }
    }
}