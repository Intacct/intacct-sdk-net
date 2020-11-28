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

namespace Intacct.SDK.Functions.AccountsPayable
{
    public class VendorCreate : AbstractVendor
    {

        public VendorCreate(string controlId = null) : base(controlId)
        {
        }

        public override void WriteXml(ref IaXmlWriter xml)
        {
            xml.WriteStartElement("function");
            xml.WriteAttribute("controlid", ControlId, true);

            xml.WriteStartElement("create");
            xml.WriteStartElement("VENDOR");

            xml.WriteElement("VENDORID", VendorId);

            xml.WriteElement("NAME", VendorName, true);

            xml.WriteStartElement("DISPLAYCONTACT");

            // CONTACTNAME is auto created as '[VendorName](V[VendorID])'

            xml.WriteElement("PRINTAS", string.IsNullOrWhiteSpace(PrintAs) ? VendorName : PrintAs, true);

            xml.WriteElement("COMPANYNAME", CompanyName);
            xml.WriteElement("TAXABLE", Taxable);
            // TAXID passed in with VENDOR element below
            xml.WriteElement("TAXGROUP", ContactTaxGroupName);
            xml.WriteElement("PREFIX", Prefix);
            xml.WriteElement("FIRSTNAME", FirstName);
            xml.WriteElement("LASTNAME", LastName);
            xml.WriteElement("INITIAL", MiddleName);
            xml.WriteElement("PHONE1", PrimaryPhoneNo);
            xml.WriteElement("PHONE2", SecondaryPhoneNo);
            xml.WriteElement("CELLPHONE", CellularPhoneNo);
            xml.WriteElement("PAGER", PagerNo);
            xml.WriteElement("FAX", FaxNo);
            xml.WriteElement("EMAIL1", PrimaryEmailAddress);
            xml.WriteElement("EMAIL2", SecondaryEmailAddress);
            xml.WriteElement("URL1", PrimaryUrl);
            xml.WriteElement("URL2", SecondaryUrl);

            WriteXmlMailAddress(ref xml);

            xml.WriteEndElement(); //DISPLAYCONTACT

            xml.WriteElement("ONETIME", OneTime);

            if (Active == true)
            {
                xml.WriteElement("STATUS", "active");
            }
            else if (Active == false)
            {
                xml.WriteElement("STATUS", "inactive");
            }

            xml.WriteElement("HIDEDISPLAYCONTACT", ExcludedFromContactList);
            xml.WriteElement("VENDTYPE", VendorTypeId);
            xml.WriteElement("PARENTID", ParentVendorId);
            xml.WriteElement("GLGROUP", GlGroupName);
            xml.WriteElement("TAXID", TaxId);
            xml.WriteElement("NAME1099", Form1099Name);
            xml.WriteElement("FORM1099TYPE", Form1099Type);
            xml.WriteElement("FORM1099BOX", Form1099Box);
            xml.WriteElement("SUPDOCID", AttachmentsId);
            xml.WriteElement("APACCOUNT", DefaultExpenseGlAccountNo);
            xml.WriteElement("CREDITLIMIT", CreditLimit);
            xml.WriteElement("ONHOLD", OnHold);
            xml.WriteElement("DONOTCUTCHECK", DoNotPay);
            xml.WriteElement("COMMENTS", Comments);
            xml.WriteElement("CURRENCY", DefaultCurrency);

            if (!string.IsNullOrWhiteSpace(PrimaryContactName))
            {
                xml.WriteStartElement("CONTACTINFO");
                xml.WriteElement("CONTACTNAME", PrimaryContactName);
                xml.WriteEndElement(); //CONTACTINFO
            }
            if (!string.IsNullOrWhiteSpace(PayToContactName))
            {
                xml.WriteStartElement("PAYTO");
                xml.WriteElement("CONTACTNAME", PayToContactName);
                xml.WriteEndElement(); //PAYTO
            }
            if (!string.IsNullOrWhiteSpace(ReturnToContactName))
            {
                xml.WriteStartElement("RETURNTO");
                xml.WriteElement("CONTACTNAME", ReturnToContactName);
                xml.WriteEndElement(); //RETURNTO
            }

            WriteXmlContactListInfo(ref xml);

            xml.WriteElement("PAYMETHODKEY", PreferredPaymentMethod);
            xml.WriteElement("MERGEPAYMENTREQ", MergePaymentRequests);
            xml.WriteElement("PAYMENTNOTIFY", SendAutomaticPaymentNotification);
            xml.WriteElement("BILLINGTYPE", VendorBillingType);
            // TODO: Default bill payment date

            xml.WriteElement("PAYMENTPRIORITY", PaymentPriority);
            xml.WriteElement("TERMNAME", PaymentTerm);
            xml.WriteElement("DISPLAYTERMDISCOUNT", TermDiscountDisplayedOnCheckStub);
            xml.WriteElement("ACHENABLED", AchEnabled);
            xml.WriteElement("ACHBANKROUTINGNUMBER", AchBankRoutingNo);
            xml.WriteElement("ACHACCOUNTNUMBER", AchBankAccountNo);
            xml.WriteElement("ACHACCOUNTTYPE", AchBankAccountType);
            xml.WriteElement("ACHREMITTANCETYPE", AchBankAccountClass);

            // TODO: Check delivery and ACH payment services fields

            xml.WriteElement("VENDORACCOUNTNO", VendorAccountNo);
            xml.WriteElement("DISPLAYACCTNOCHECK", LocationAssignedAccountNoDisplayedOnCheckStub);

            // TODO: Location assigned account numbers

            xml.WriteElement("OBJECTRESTRICTION", RestrictionType);
            if (RestrictedLocations.Count > 0)
            {
                xml.WriteElement("RESTRICTEDLOCATIONS",
                    string.Join(IaXmlWriter.IntacctMultiSelectGlue, RestrictedLocations));
            }
            if (RestrictedDepartments.Count > 0)
            {
                xml.WriteElement("RESTRICTEDDEPARTMENTS",
                    string.Join(IaXmlWriter.IntacctMultiSelectGlue, RestrictedDepartments));
            }

            xml.WriteCustomFieldsImplicit(CustomFields);

            xml.WriteEndElement(); //VENDOR
            xml.WriteEndElement(); //create

            xml.WriteEndElement(); //function
        }
    }
}