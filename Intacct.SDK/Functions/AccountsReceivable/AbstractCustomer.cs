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
using Intacct.SDK.Functions.Company;
using Intacct.SDK.Xml;

namespace Intacct.SDK.Functions.AccountsReceivable
{
    public abstract class AbstractCustomer : AbstractFunction
    {

        public string CustomerId;

        public string CustomerName;

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

        public string CustomerTypeId;

        public string SalesRepEmployeeId;

        public string ParentCustomerId;

        public string GlGroupName;

        public string TerritoryId;

        public string AttachmentsId;

        public string PaymentTerm;

        public string OffsetArGlAccountNo;

        public string DefaultRevenueGlAccountNo;

        public string ShippingMethod;

        public string ResaleNumber;

        public bool? Taxable;

        public string ContactTaxGroupName;

        public string TaxId;

        public decimal? CreditLimit;

        public bool? OnHold;

        public string DeliveryMethod;

        public string DefaultInvoiceMessage;

        public string Comments;

        public string DefaultCurrency;

        public string PrintOptionArInvoiceTemplateName;

        public string PrintOptionOeQuoteTemplateName;

        public string PrintOptionOeOrderTemplateName;

        public string PrintOptionOeListTemplateName;

        public string PrintOptionOeInvoiceTemplateName;

        public string PrintOptionOeAdjustmentTemplateName;

        public string PrintOptionOeOtherTemplateName;

        // TODO Email template options

        public string PrimaryContactName;

        public string BillToContactName;

        public string ShipToContactName;

        public List<ContactListInfo> ContactList = new List<ContactListInfo>();

        public string RestrictionType;

        public List<string> RestrictedLocations = new List<string>();

        public List<string> RestrictedDepartments = new List<string>();

        public Dictionary<string, dynamic> CustomFields = new Dictionary<string, dynamic>();

        protected AbstractCustomer(string controlId = null) : base(controlId)
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

                xml.WriteElement("ADDRESS1", AddressLine1);
                xml.WriteElement("ADDRESS2", AddressLine2);
                xml.WriteElement("CITY", City);
                xml.WriteElement("STATE", StateProvince);
                xml.WriteElement("ZIP", ZipPostalCode);
                xml.WriteElement("COUNTRY", Country);
                xml.WriteElement("COUNTRYCODE", IsoCountryCode);

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