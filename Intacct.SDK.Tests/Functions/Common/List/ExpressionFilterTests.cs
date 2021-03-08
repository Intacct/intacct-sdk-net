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

namespace Intacct.SDK.Tests.Functions.Common.List
{
    public class ExpressionFilterTests
    {
		[Fact]
		public void ParameterLessConstructor()
		{
            // arrange/act
            ExpressionFilter expressionFilter = new ExpressionFilter();

			// assert
			Assert.Null(expressionFilter.Field);
			Assert.Null(expressionFilter.Operator);
			Assert.Null(expressionFilter.Value);
		}

		[Fact]
		public void FieldOperatorValueProvided()
		{
			// arrange/act
			ExpressionFilter expressionFilter = new ExpressionFilter()
			{
				Field = "supdocname",
				Operator = "=",
				Value = "foo bar"
			};

			// assert
			Assert.Equal("supdocname", expressionFilter.Field);
			Assert.Equal("=", expressionFilter.Operator);
			Assert.Equal("foo bar", expressionFilter.Value);
		}
	}

}
