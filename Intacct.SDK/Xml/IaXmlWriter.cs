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
using System.IO;
using System.Xml;

namespace Intacct.SDK.Xml
{
    public class IaXmlWriter : XmlWriter
    {
        public const string IntacctDateFormat = "MM/dd/yyyy";

        public const string IntacctDateTimeFormat = "MM/dd/yyyy HH:mm:ss";

        public const string IntacctMultiSelectGlue = "#~#";

        private readonly XmlWriter _writer;

        public IaXmlWriter(Stream output, XmlWriterSettings settings)
        {
            _writer = Create(output, settings);
        }

        public override WriteState WriteState => _writer.WriteState;

        public override void Flush()
        {
            _writer.Flush();
        }

        public override string LookupPrefix(string ns)
        {
            return _writer.LookupPrefix(ns);
        }

        public void WriteAttribute(string localName, string value, bool writeNull = false)
        {
            if (!string.IsNullOrEmpty(value) || writeNull == true)
            {
                _writer.WriteAttributeString(localName, value);
            }
        }

        public void WriteAttribute(string localName, int? value, bool writeNull = false)
        {
            if (value == null)
            {
                if (writeNull == true)
                {
                    _writer.WriteAttributeString(localName, "");
                }
            }
            else
            {
                _writer.WriteAttributeString(localName, value.ToString());
            }
        }

        public void WriteAttribute(string localName, bool value)
        {
            string val = (value == true) ? "true" : "false";
            _writer.WriteAttributeString(localName, val);
        }

        public override void WriteBase64(byte[] buffer, int index, int count)
        {
            _writer.WriteBase64(buffer, index, count);
        }

        public override void WriteCData(string text)
        {
            _writer.WriteCData(text);
        }

        public override void WriteCharEntity(char ch)
        {
            _writer.WriteCharEntity(ch);
        }

        public override void WriteChars(char[] buffer, int index, int count)
        {
            _writer.WriteChars(buffer, index, count);
        }

        public override void WriteComment(string text)
        {
            _writer.WriteComment(text);
        }

        public void WriteCustomFieldsExplicit(Dictionary<string, dynamic> customFields)
        {
            if (customFields.Count > 0)
            {
                WriteStartElement("customfields");
                foreach (KeyValuePair<string, dynamic> customField in customFields)
                {
                    WriteStartElement("customfield");
                    WriteElement("customfieldname", customField.Key, true);

                    if (customField.Value == null)
                    {
                        WriteEmptyElement("customfieldvalue");
                    }
                    else
                    {
                        WriteElement("customfieldvalue", customField.Value, true);
                    }
                    WriteEndElement(); //customfield
                }
                WriteEndElement(); //customfields
            }
        }

        public void WriteCustomFieldsImplicit(Dictionary<string, dynamic> customFields)
        {
            if (customFields.Count > 0)
            {
                foreach (KeyValuePair<string, dynamic> customField in customFields)
                {
                    if (customField.Value == null)
                    {
                        WriteEmptyElement(customField.Key);
                    }
                    else
                    {
                        WriteElement(customField.Key, customField.Value, true);
                    }
                }
            }
        }

        public void WriteDateMMddyyyy(DateTime date)
        {
            _writer.WriteRaw(date.ToString("MM/dd/yyyy"));
        }

        public void WriteDateSplitElements(DateTime date, bool writeNull = true)
        {
            WriteElement("year", date.ToString("yyyy"), writeNull);
            WriteElement("month", date.ToString("MM"), writeNull);
            WriteElement("day", date.ToString("dd"), writeNull);
        }

        public override void WriteDocType(string name, string pubid, string sysid, string subset)
        {
            _writer.WriteDocType(name, pubid, sysid, subset);
        }

        public void WriteElement(string localName, string value, bool writeNull = false)
        {
            if (value == null)
            {
                if (writeNull == true)
                {
                    _writer.WriteElementString(localName, value);
                }
            }
            else
            {
                // if value == "" we are writing it
                _writer.WriteElementString(localName, value);
            }
        }

        // XML Helper Functions
        public void WriteElement(string localName, int value)
        {
            _writer.WriteElementString(localName, value.ToString());
        }

        public void WriteElement(string localName, int? value, bool writeNull = false)
        {
            if (value == null)
            {
                if (writeNull == true)
                {
                    _writer.WriteElementString(localName, "");
                }
            }
            else
            {
                _writer.WriteElementString(localName, value.ToString());
            }
        }

        public void WriteElement(string localName, decimal value)
        {
            _writer.WriteElementString(localName, value.ToString());
        }

        public void WriteElement(string localName, decimal? value, bool writeNull = false)
        {
            if (value == null)
            {
                if (writeNull == true)
                {
                    _writer.WriteElementString(localName, "");
                }
            }
            else
            {
                _writer.WriteElementString(localName, value.ToString());
            }
        }

        public void WriteElement(string localName, bool value)
        {
            string val = value == true ? "true" : "false";
            _writer.WriteElementString(localName, val);
        }

        public void WriteElement(string localName, bool? value, bool writeNull = false)
        {
            if (value == true)
            {
                _writer.WriteElementString(localName, "true");
            }
            else if (value == false)
            {
                _writer.WriteElementString(localName, "false");
            }
            else
            {
                if (writeNull == true)
                {
                    _writer.WriteElementString(localName, "");
                }
            }
        }

        public void WriteElement(string localName, DateTime value, string format = IntacctDateTimeFormat)
        {
            _writer.WriteElementString(localName, value == default(DateTime) ? "" : value.ToString(format));
        }

        public void WriteElement(string localName, DateTime? value, string format = IntacctDateTimeFormat, bool writeNull = false)
        {
            if (!value.HasValue)
            {
                if (writeNull == true)
                {
                    _writer.WriteElementString(localName, "");
                }
            }
            else
            {
                _writer.WriteElementString(localName, value == default(DateTime) ? "" : value.Value.ToString(format));
            }
        }

        public void WriteEmptyElement(string localName)
        {
            _writer.WriteStartElement(localName);
            _writer.WriteEndElement();
        }

        public override void WriteEndAttribute()
        {
            _writer.WriteEndAttribute();
        }

        public override void WriteEndDocument()
        {
            _writer.WriteEndDocument();
        }

        public override void WriteEndElement()
        {
            _writer.WriteEndElement();
        }

        public override void WriteEntityRef(string name)
        {
            _writer.WriteEntityRef(name);
        }

        public override void WriteFullEndElement()
        {
            _writer.WriteFullEndElement();
        }

        public override void WriteProcessingInstruction(string name, string text)
        {
            _writer.WriteProcessingInstruction(name, text);
        }

        public override void WriteRaw(char[] buffer, int index, int count)
        {
            _writer.WriteRaw(buffer, index, count);
        }

        public override void WriteRaw(string data)
        {
            _writer.WriteRaw(data);
        }

        public new void WriteStartAttribute(string localName)
        {
            _writer.WriteStartAttribute(localName);
        }

        public new void WriteStartAttribute(string localName, string ns)
        {
            _writer.WriteStartAttribute(localName, ns);
        }

        public override void WriteStartAttribute(string prefix, string localName, string ns)
        {
            _writer.WriteStartAttribute(prefix, localName, ns);
        }

        public override void WriteStartDocument()
        {
            _writer.WriteStartDocument();
        }

        public override void WriteStartDocument(bool standalone)
        {
            _writer.WriteStartDocument(standalone);
        }

        public new void WriteStartElement(string localName)
        {
            _writer.WriteStartElement(localName);
        }

        public new void WriteStartElement(string localName, string ns)
        {
            _writer.WriteStartElement(localName, ns);
        }

        public override void WriteStartElement(string prefix, string localName, string ns)
        {
            _writer.WriteStartElement(prefix, localName, ns);
        }

        public override void WriteString(string text)
        {
            _writer.WriteString(text);
        }

        public override void WriteSurrogateCharEntity(char lowChar, char highChar)
        {
            _writer.WriteSurrogateCharEntity(lowChar, highChar);
        }

        public override void WriteWhitespace(string ws)
        {
            _writer.WriteWhitespace(ws);
        }
    }
}