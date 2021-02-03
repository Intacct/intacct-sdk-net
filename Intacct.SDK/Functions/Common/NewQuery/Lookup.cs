using Intacct.SDK.Xml;

namespace Intacct.SDK.Functions.Common.NewQuery
{
    public class Lookup : AbstractFunction
    {
        public string Object;

        public Lookup(string controlId = "") : base(controlId)
        {
        }

        public override void WriteXml(ref IaXmlWriter xml)
        {
            xml.WriteStartElement("function");
            xml.WriteAttributeString("controlid", ControlId);

            xml.WriteStartElement("lookup");
            xml.WriteElementString("object", Object);
            xml.WriteEndElement(); //lookup

            xml.WriteEndElement(); //function
        }
    }
}
