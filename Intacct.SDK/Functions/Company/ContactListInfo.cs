using Intacct.SDK.Xml;
using System;
using System.Collections.Generic;
using System.Text;

namespace Intacct.SDK.Functions.Company
{
    public class ContactListInfo
    {
        public string CategoryName;

        public string Contact;

        public void WriteXmlContactListInfo(ref IaXmlWriter xml)
        {
            xml.WriteStartElement("CONTACT_LIST_INFO");

            xml.WriteElement("CATEGORYNAME", this.CategoryName);

            xml.WriteStartElement("CONTACT");
            xml.WriteElement("NAME", this.Contact);
            xml.WriteEndElement();

            xml.WriteEndElement();
        }
    }
}
