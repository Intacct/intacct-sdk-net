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
using Intacct.SDK.Xml;

namespace Intacct.SDK.Functions
{
    public abstract class AbstractFunction : IFunction
    {
        private string _controlId;
        public string ControlId
        {
            get => _controlId;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    value = Guid.NewGuid().ToString();
                }

                if (value.Length < 1 || value.Length > 256)
                {
                    throw new ArgumentException("Control ID must be between 1 and 256 characters in length");
                }

                _controlId = value;
            }
        }

        protected AbstractFunction()
        {
            _controlId = Guid.NewGuid().ToString();
        }

        protected AbstractFunction(string controlId)
        {
            ControlId = controlId;
        }
        
        public abstract void WriteXml(ref IaXmlWriter xml);
    }
}