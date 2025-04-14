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
using Intacct.SDK.Functions.Common.Query.Comparison.InList;
using Xunit;

namespace Intacct.SDK.Tests.Functions.Common.Query.Comparison.InList
{
    public class InListStringTest
    {
        [Fact]
        public void ToStringTest()
        {
            InListString condition = new InListString()
            {
                Field = "VENDORID",
                ValuesList = new List<string>()
                {
                    "V0001",
                    "V0002",
                    "V0003",
                }
            };
            
            Assert.Equal("VENDORID IN ('V0001','V0002','V0003')", condition.ToString());
        }
        
        [Fact]
        public void ToStringNotTest()
        {
            InListString condition = new InListString()
            {
                Negate = true,
                Field = "VENDORID",
                ValuesList = new List<string>()
                {
                    "V0001",
                    "V0002",
                    "V0003",
                }
            };
            
            Assert.Equal("VENDORID NOT IN ('V0001','V0002','V0003')", condition.ToString());
        }
        
        [Fact]
        public void ToStringEscapeQuotesTest()
        {
            InListString condition = new InListString()
            {
                Field = "VENDORID",
                ValuesList = new List<string>()
                {
                    "bob's pizza",
                    "bill's pizza",
                    "sally's pizza",
                }
            };
            
            Assert.Equal(@"VENDORID IN ('bob\'s pizza','bill\'s pizza','sally\'s pizza')", condition.ToString());
        }
        
        [Fact]
        public void OneToStringTest()
        {
            InListString condition = new InListString()
            {
                Field = "VENDORID",
                ValuesList = new List<string>()
                {
                    "V0001",
                }
            };
            
            Assert.Equal("VENDORID IN ('V0001')", condition.ToString());
        }
    }
}