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
using System.IO;
using System.Text;
using System.Xml;
using Intacct.SDK.Functions.AccountsReceivable;
using Intacct.SDK.Functions;
using Intacct.SDK.Tests.Xml;
using Intacct.SDK.Xml;
using Xunit;
using Intacct.SDK.Functions.Company;

namespace Intacct.SDK.Tests.Functions.AccountsReceivable
{
    public class CustomerUpdateTest : XmlObjectTestHelper
    {
        [Fact]
        public void GetXmlTest()
        {
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<function controlid=""unittest"">
    <update>
        <CUSTOMER>
            <CUSTOMERID>C1234</CUSTOMERID>
            <DISPLAYCONTACT />
        </CUSTOMER>
    </update>
</function>";

            CustomerUpdate record = new CustomerUpdate("unittest")
            {
                CustomerId = "C1234",
            };
            this.CompareXml(expected, record);
        }

        [Fact]
        public void GetAllXmlTest()
        {
            string expected = @"<?xml version=""1.0"" encoding=""utf-8""?>
<function controlid=""unittest"">
    <update>
        <CUSTOMER>
            <CUSTOMERID>C1234</CUSTOMERID>
            <NAME>Intacct Corp</NAME>
            <DISPLAYCONTACT>
                <PRINTAS>Intacct Corporation</PRINTAS>
                <COMPANYNAME>Intacct Corp.</COMPANYNAME>
                <TAXABLE>true</TAXABLE>
                <TAXGROUP>CA</TAXGROUP>
                <PREFIX>Mr</PREFIX>
                <FIRSTNAME>Bill</FIRSTNAME>
                <LASTNAME>Smith</LASTNAME>
                <INITIAL>G</INITIAL>
                <PHONE1>12</PHONE1>
                <PHONE2>34</PHONE2>
                <CELLPHONE>56</CELLPHONE>
                <PAGER>78</PAGER>
                <FAX>90</FAX>
                <EMAIL1>noreply@intacct.com</EMAIL1>
                <EMAIL2>noreplyagain@intacct.com</EMAIL2>
                <URL1>www.intacct.com</URL1>
                <URL2>us.intacct.com</URL2>
                <MAILADDRESS>
                    <ADDRESS1>300 Park Ave</ADDRESS1>
                    <ADDRESS2>Ste 1400</ADDRESS2>
                    <CITY>San Jose</CITY>
                    <STATE>CA</STATE>
                    <ZIP>95110</ZIP>
                    <COUNTRY>United States</COUNTRY>
                </MAILADDRESS>
            </DISPLAYCONTACT>
            <ONETIME>false</ONETIME>
            <STATUS>active</STATUS>
            <HIDEDISPLAYCONTACT>false</HIDEDISPLAYCONTACT>
            <CUSTTYPE>SaaS</CUSTTYPE>
            <PARENTID>C5678</PARENTID>
            <GLGROUP>Group</GLGROUP>
            <TERRITORYID>NE</TERRITORYID>
            <SUPDOCID>A1234</SUPDOCID>
            <TERMNAME>N30</TERMNAME>
            <OFFSETGLACCOUNTNO>1200</OFFSETGLACCOUNTNO>
            <ARACCOUNT>4000</ARACCOUNT>
            <SHIPPINGMETHOD>USPS</SHIPPINGMETHOD>
            <RESALENO>123</RESALENO>
            <TAXID>12-3456789</TAXID>
            <CREDITLIMIT>1234.56</CREDITLIMIT>
            <ONHOLD>false</ONHOLD>
            <DELIVERYOPTIONS>Print#~#E-Mail</DELIVERYOPTIONS>
            <CUSTMESSAGEID>hello</CUSTMESSAGEID>
            <COMMENTS>my comment</COMMENTS>
            <CURRENCY>USD</CURRENCY>
            <ARINVOICEPRINTTEMPLATEID>temp1</ARINVOICEPRINTTEMPLATEID>
            <OEQUOTEPRINTTEMPLATEID>temp2</OEQUOTEPRINTTEMPLATEID>
            <OEORDERPRINTTEMPLATEID>temp3</OEORDERPRINTTEMPLATEID>
            <OELISTPRINTTEMPLATEID>temp4</OELISTPRINTTEMPLATEID>
            <OEINVOICEPRINTTEMPLATEID>temp5</OEINVOICEPRINTTEMPLATEID>
            <OEADJPRINTTEMPLATEID>temp6</OEADJPRINTTEMPLATEID>
            <OEOTHERPRINTTEMPLATEID>temp7</OEOTHERPRINTTEMPLATEID>
            <CONTACTINFO>
                <CONTACTNAME>primary</CONTACTNAME>
            </CONTACTINFO>
            <BILLTO>
                <CONTACTNAME>bill to</CONTACTNAME>
            </BILLTO>
            <SHIPTO>
                <CONTACTNAME>ship to</CONTACTNAME>
            </SHIPTO>
            <CONTACT_LIST_INFO>
                <CATEGORYNAME>Primary Contact</CATEGORYNAME>
                <CONTACT>
                    <NAME>primary</NAME>
                </CONTACT>
            </CONTACT_LIST_INFO>
            <CONTACT_LIST_INFO>
                <CATEGORYNAME>Billing Contact</CATEGORYNAME>
                <CONTACT>
                    <NAME>bill to</NAME>
                </CONTACT>
            </CONTACT_LIST_INFO>
            <CONTACT_LIST_INFO>
                <CATEGORYNAME>Shipping Contact</CATEGORYNAME>
                <CONTACT>
                    <NAME>ship to</NAME>
                </CONTACT>
            </CONTACT_LIST_INFO>
            <OBJECTRESTRICTION>Restricted</OBJECTRESTRICTION>
            <RESTRICTEDLOCATIONS>100#~#200</RESTRICTEDLOCATIONS>
            <RESTRICTEDDEPARTMENTS>D100#~#D200</RESTRICTEDDEPARTMENTS>
            <CUSTOMFIELD1>Hello</CUSTOMFIELD1>
        </CUSTOMER>
    </update>
</function>";

            CustomerUpdate record = new CustomerUpdate("unittest") {
                CustomerId = "C1234",
                CustomerName = "Intacct Corp",
                PrintAs = "Intacct Corporation",
                CompanyName = "Intacct Corp.",
                Taxable = true,
                TaxId = "12-3456789",
                ContactTaxGroupName = "CA",
                Prefix = "Mr",
                FirstName = "Bill",
                LastName = "Smith",
                MiddleName = "G",
                PrimaryPhoneNo = "12",
                SecondaryPhoneNo = "34",
                CellularPhoneNo = "56",
                PagerNo = "78",
                FaxNo = "90",
                PrimaryEmailAddress = "noreply@intacct.com",
                SecondaryEmailAddress = "noreplyagain@intacct.com",
                PrimaryUrl = "www.intacct.com",
                SecondaryUrl = "us.intacct.com",
                AddressLine1 = "300 Park Ave",
                AddressLine2 = "Ste 1400",
                City = "San Jose",
                StateProvince = "CA",
                ZipPostalCode = "95110",
                Country = "United States",
                OneTime = false,
                Active = true,
                ExcludedFromContactList = false,
                CustomerTypeId = "SaaS",
                ParentCustomerId = "C5678",
                GlGroupName = "Group",
                TerritoryId = "NE",
                AttachmentsId = "A1234",
                PaymentTerm = "N30",
                OffsetArGlAccountNo = "1200",
                DefaultRevenueGlAccountNo = "4000",
                ShippingMethod = "USPS",
                ResaleNumber = "123",
                CreditLimit = 1234.56M,
                OnHold = false,
                DeliveryMethod = "Print#~#E-Mail",
                DefaultInvoiceMessage = "hello",
                Comments = "my comment",
                DefaultCurrency = "USD",
                PrintOptionArInvoiceTemplateName = "temp1",
                PrintOptionOeQuoteTemplateName = "temp2",
                PrintOptionOeOrderTemplateName = "temp3",
                PrintOptionOeListTemplateName = "temp4",
                PrintOptionOeInvoiceTemplateName = "temp5",
                PrintOptionOeAdjustmentTemplateName = "temp6",
                PrintOptionOeOtherTemplateName = "temp7",
                PrimaryContactName = "primary",
                BillToContactName = "bill to",
                ShipToContactName = "ship to",
                ContactList = new List<ContactListInfo>
                {
                    new ContactListInfo { CategoryName = "Primary Contact", Contact = "primary" },
                    new ContactListInfo { CategoryName = "Billing Contact", Contact = "bill to" },
                    new ContactListInfo { CategoryName = "Shipping Contact", Contact = "ship to" },
                },
                RestrictionType = "Restricted",
                RestrictedLocations = new List<string>()
                {
                    "100",
                    "200",
                },
                RestrictedDepartments = new List<string>()
                {
                    "D100",
                    "D200",
                },
                CustomFields = new Dictionary<string, dynamic>
                {
                    { "CUSTOMFIELD1", "Hello" }
                }
            };
            this.CompareXml(expected, record);
        }
    }
}