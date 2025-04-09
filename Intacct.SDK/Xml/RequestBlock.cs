/*
 * Copyright 2022 Sage Intacct, Inc.
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
using Intacct.SDK.Functions;
using Intacct.SDK.Xml.Request;

namespace Intacct.SDK.Xml
{
    public class RequestBlock
    {
        public Encoding Encoding;

        public ControlBlock Control;

        public OperationBlock Operation;
        
        public RequestBlock(ClientConfig clientConfig, RequestConfig requestConfig, List<IFunction> content)
        {
            this.Encoding = requestConfig.Encoding;
            this.Control = new ControlBlock(clientConfig, requestConfig);
            this.Operation = new OperationBlock(clientConfig, requestConfig, content);
        }
        
        public Stream WriteXml()
        {
            Stream stream = new MemoryStream();
            XmlWriterSettings xmlSettings = new XmlWriterSettings
            {
                Encoding = this.Encoding,
                Indent = false,
                NewLineHandling = NewLineHandling.Replace,
                NewLineOnAttributes = false
                NewLineChars = ""
            };

            IaXmlWriter xml = new IaXmlWriter(stream, xmlSettings);
            xml.WriteStartElement("request");

            this.Control.WriteXml(ref xml); // Create control block

            this.Operation.WriteXml(ref xml); // Create operation block

            xml.WriteEndElement(); // request

            xml.Flush();

            return stream;
        }
    }
}
