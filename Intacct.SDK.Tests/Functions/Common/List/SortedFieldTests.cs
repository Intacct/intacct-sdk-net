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

	public class SortedFieldTests
	{
		[Fact]
		public void ParameterLessConstructor()
        {
			// arrange/act
			SortedField sortedField = new SortedField();

			// assert
			Assert.Null(sortedField.Name);
			Assert.Equal("asc", sortedField.Order);
		}

		[Fact]
		public void NameProvided()
		{
			// arrange/act
			SortedField sortedField = new SortedField()
			{
				Name = "ColumnA",
			};

			// assert
			Assert.Equal("ColumnA", sortedField.Name);
			Assert.Equal("asc", sortedField.Order);
		}

		[Fact]
		public void NameOrderProvided()
		{
			// arrange/act
			SortedField sortedField = new SortedField()
			{
				Name = "ColumnA",
				Order = "desc"
			};

			// assert
			Assert.Equal("ColumnA", sortedField.Name);
			Assert.Equal("desc", sortedField.Order);
		}
	}

}
