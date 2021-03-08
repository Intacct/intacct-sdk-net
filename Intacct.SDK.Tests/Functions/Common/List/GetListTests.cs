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

using Xunit;
using Intacct.SDK.Functions.Common.List;
using Intacct.SDK.Tests.Xml;
using System.Collections.Generic;
using System;

namespace Intacct.SDK.Tests.Functions.Common.List
{
    public class GetListTests : XmlObjectTestHelper
    {
		[Fact]
		public void MinimalConstructor()
		{
            // arrange
            string controlId = Guid.NewGuid().ToString();

            string expected = String.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<function controlid=""{0}"">
    <get_list object=""supdoc"" start=""0"" maxitems=""100"" showprivate=""true"">
        <filter />
        <fields />
        <sorts />
    </get_list>
</function>", controlId);

            // act
            GetList getList = new GetList("supdoc", controlId);

            // assert
            this.CompareXml(expected, getList);
        }

        [Fact]
        public void ExpressionFilterProvided()
        {
            // arrange
            string controlId = Guid.NewGuid().ToString();

            string expected = String.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<function controlid=""{0}"">
    <get_list object=""supdoc"" start=""0"" maxitems=""100"" showprivate=""true"">
        <filter>
            <expression>
                <field>FieldA</field>
                <operator>=</operator>
                <value>Lorem Ipsum</value>
            </expression>
        </filter>
        <fields />
        <sorts />
    </get_list>
</function>",controlId);

            ExpressionFilter expression = new ExpressionFilter()
            { 
                Field = "FieldA",
                Operator = "=",
                Value = "Lorem Ipsum"
            };

            // act
            GetList getList = new GetList("supdoc", controlId)
            {
                Expression = expression
            };

            // assert
            this.CompareXml(expected, getList);
        }

        [Fact]
        public void FieldsProvided()
        {
            // arrange
            string controlId = Guid.NewGuid().ToString();

            string expected = String.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<function controlid=""{0}"">
    <get_list object=""supdoc"" start=""0"" maxitems=""100"" showprivate=""true"">
        <filter />
        <fields>
            <field>FieldA</field>
            <field>FieldB</field>
        </fields>
        <sorts />
    </get_list>
</function>",controlId);

            List<string> fields = new List<string>();
            fields.Add("FieldA");
            fields.Add("FieldB");

            // act
            GetList getList = new GetList("supdoc", controlId)
            {
                Fields = fields
            };

            // assert
            this.CompareXml(expected, getList);
        }

        [Fact]
        public void SortsProvided()
        {
            // arrange
            string controlId = Guid.NewGuid().ToString();

            string expected = String.Format(@"<?xml version=""1.0"" encoding=""utf-8""?>
<function controlid=""{0}"">
    <get_list object=""supdoc"" start=""0"" maxitems=""100"" showprivate=""true"">
        <filter />
        <fields />
        <sorts>
            <sortfield order=""asc"">FieldA</sortfield>
            <sortfield order=""desc"">FieldB</sortfield>
        </sorts>
    </get_list>
</function>",controlId);

            List<SortedField> sortedFields = new List<SortedField>();
            sortedFields.Add(new SortedField("FieldA"));
            sortedFields.Add(new SortedField("FieldB","desc"));

            // act
            GetList getList = new GetList("supdoc", controlId)
            {
                SortedFields = sortedFields
            };

            // assert
            this.CompareXml(expected, getList);
        }

    }
}
