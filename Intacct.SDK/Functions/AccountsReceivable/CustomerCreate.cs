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
    public class CustomerCreate : AbstractCustomer
    {

        public CustomerCreate(string controlId = null) : base(controlId)
        {
        }

        public override void WriteXml(ref IaXmlWriter xml)
        {
            xml.WriteStartElement("function");
            xml.WriteAttribute("controlid", ControlId, true);

            xml.WriteStartElement("create");
            xml.WriteStartElement("CUSTOMER");

            xml.WriteElement("CUSTOMERID", CustomerId);

            xml.WriteElement("NAME", CustomerName, true);

            xml.WriteStartElement("DISPLAYCONTACT");

            // CONTACTNAME is auto created as '[CustomerName](C[CustomerID])'

            xml.WriteElement("PRINTAS", string.IsNullOrWhiteSpace(PrintAs) ? CustomerName : PrintAs, true);

            xml.WriteElement("COMPANYNAME", CompanyName);
            xml.WriteElement("TAXABLE", Taxable);
            // TAXID passed in with element below
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
            xml.WriteElement("CUSTTYPE", CustomerTypeId);
            xml.WriteElement("CUSTREPID", SalesRepEmployeeId);
            xml.WriteElement("PARENTID", ParentCustomerId);
            xml.WriteElement("GLGROUP", GlGroupName);
            xml.WriteElement("TERRITORYID", TerritoryId);
            xml.WriteElement("SUPDOCID", AttachmentsId);
            xml.WriteElement("TERMNAME", PaymentTerm);
            xml.WriteElement("OFFSETGLACCOUNTNO", OffsetArGlAccountNo);
            xml.WriteElement("ARACCOUNT", DefaultRevenueGlAccountNo);
            xml.WriteElement("SHIPPINGMETHOD", ShippingMethod);
            xml.WriteElement("RESALENO", ResaleNumber);
            xml.WriteElement("TAXID", TaxId);
            xml.WriteElement("CREDITLIMIT", CreditLimit);
            xml.WriteElement("ONHOLD", OnHold);
            xml.WriteElement("DELIVERYOPTIONS", DeliveryMethod);
            xml.WriteElement("CUSTMESSAGEID", DefaultInvoiceMessage);
            xml.WriteElement("COMMENTS", Comments);
            xml.WriteElement("CURRENCY", DefaultCurrency);

            xml.WriteElement("ARINVOICEPRINTTEMPLATEID", PrintOptionArInvoiceTemplateName);
            xml.WriteElement("OEQUOTEPRINTTEMPLATEID", PrintOptionOeQuoteTemplateName);
            xml.WriteElement("OEORDERPRINTTEMPLATEID", PrintOptionOeOrderTemplateName);
            xml.WriteElement("OELISTPRINTTEMPLATEID", PrintOptionOeListTemplateName);
            xml.WriteElement("OEINVOICEPRINTTEMPLATEID", PrintOptionOeInvoiceTemplateName);
            xml.WriteElement("OEADJPRINTTEMPLATEID", PrintOptionOeAdjustmentTemplateName);
            xml.WriteElement("OEOTHERPRINTTEMPLATEID", PrintOptionOeOtherTemplateName);

            if (!string.IsNullOrWhiteSpace(PrimaryContactName))
            {
                xml.WriteStartElement("CONTACTINFO");
                xml.WriteElement("CONTACTNAME", PrimaryContactName);
                xml.WriteEndElement(); //CONTACTINFO
            }
            if (!string.IsNullOrWhiteSpace(BillToContactName))
            {
                xml.WriteStartElement("BILLTO");
                xml.WriteElement("CONTACTNAME", BillToContactName);
                xml.WriteEndElement(); //BILLTO
            }
            if (!string.IsNullOrWhiteSpace(ShipToContactName))
            {
                xml.WriteStartElement("SHIPTO");
                xml.WriteElement("CONTACTNAME", ShipToContactName);
                xml.WriteEndElement(); //SHIPTO
            }

            WriteXmlContactListInfo(ref xml);

            xml.WriteElement("OBJECTRESTRICTION", RestrictionType);
            if (RestrictedLocations.Count > 0)
            {
                xml.WriteElement("RESTRICTEDLOCATIONS", string.Join(IaXmlWriter.IntacctMultiSelectGlue, RestrictedLocations));
            }
            if (RestrictedDepartments.Count > 0)
            {
                xml.WriteElement("RESTRICTEDDEPARTMENTS", string.Join(IaXmlWriter.IntacctMultiSelectGlue, RestrictedDepartments));
            }

            // TODO salesforce tab

            xml.WriteCustomFieldsImplicit(CustomFields);

            xml.WriteEndElement(); //CUSTOMER
            xml.WriteEndElement(); //create

            xml.WriteEndElement(); //function
        }

    }
}