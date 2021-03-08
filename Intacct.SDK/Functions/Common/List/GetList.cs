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
using System;

namespace Intacct.SDK.Functions.Common.List
{
    public class GetList : AbstractFunction
    {
        public int start;
        public int maxitems;
        public bool showprivate;
        public string ObjectName;

        public System.Collections.Generic.List<string> Fields = new System.Collections.Generic.List<string>();
        public System.Collections.Generic.List<SortedField> SortedFields = new System.Collections.Generic.List<SortedField>();

        public ExpressionFilter Expression;

        public GetList(string objectName)
        {
            base.ControlId = Guid.NewGuid().ToString();
            this.start = 0;
            this.maxitems = 100;
            this.showprivate = true;
            this.ObjectName = objectName;
        }

        public GetList(string objectName, string controlId)
        {
            base.ControlId = controlId;

            this.start = 0;
            this.maxitems = 100;
            this.showprivate = true;
            this.ObjectName = objectName;
        }

        public override void WriteXml(ref IaXmlWriter xml)
        {
            xml.WriteStartDocument();

            xml.WriteStartElement("function");
            xml.WriteAttribute("controlid", ControlId, true);

            xml.WriteStartElement("get_list");
            xml.WriteAttributeString("object", this.ObjectName);
            xml.WriteAttributeString("start", this.start.ToString());
            xml.WriteAttributeString("maxitems", this.maxitems.ToString());
            xml.WriteAttributeString("showprivate", this.showprivate.ToString().ToLower());

            xml.WriteStartElement("filter");

            if (this.Expression != null)
            {
                xml.WriteStartElement("expression");
                xml.WriteElementString("field", this.Expression.Field);
                xml.WriteElementString("operator", this.Expression.Operator);
                xml.WriteElementString("value", this.Expression.Value);
                xml.WriteEndElement(); // </expression>
            }

            // if ( this.logical ) 
            // {
            //     xml.WriteStartElement("logical");
            //     xml.WriteEndElement(); // </logical>
            // }

            xml.WriteEndElement(); // </filter>

            xml.WriteStartElement("fields");
            foreach (string Field in this.Fields)
            {
                xml.WriteElementString("field", Field);
            }
            xml.WriteEndElement(); // </fields>

            xml.WriteStartElement("sorts");
            foreach (SortedField SortedField in this.SortedFields)
            {
                xml.WriteStartElement("sortfield");
                xml.WriteAttributeString("order", SortedField.Order);
                xml.WriteString(SortedField.Name);
                xml.WriteEndElement(); // </sortfield>
            }
            xml.WriteEndElement(); // </sorts>

            xml.WriteEndElement(); // </get_list> 

            xml.WriteEndElement(); // </function> 

            xml.WriteEndDocument();
            xml.Flush();
            xml.Close();

        }
    }

}
