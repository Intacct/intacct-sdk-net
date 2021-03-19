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
using System.IO;
using System.Text;
using System.Xml;
using Xunit;

namespace Intacct.SDK.Tests.Xml
{
    public class XmlObjectTestHelper
    {
        public void CompareXml(string expected, IXmlObject apiFunction)
        {
            Stream stream = new MemoryStream();
            XmlWriterSettings xmlSettings = new XmlWriterSettings
            {
                Encoding = Encoding.UTF8,
                Indent = true,
                IndentChars = "    "
            };

            IaXmlWriter xml = new IaXmlWriter(stream, xmlSettings);

            apiFunction.WriteXml(ref xml);

            xml.Flush();
            stream.Position = 0;
            StreamReader reader = new StreamReader(stream);

            var foo = reader.ReadToEnd();
            Assert.Equal(expected, foo);
        }
    }
}