/*
 *      Copyright 2011 Battams, Derek
 *       
 *       Licensed under the Apache License, Version 2.0 (the "License");
 *       you may not use this file except in compliance with the License.
 *       You may obtain a copy of the License at
 *
 *          http://www.apache.org/licenses/LICENSE-2.0
 *
 *       Unless required by applicable law or agreed to in writing, software
 *       distributed under the License is distributed on an "AS IS" BASIS,
 *       WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *       See the License for the specific language governing permissions and
 *       limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Livepvrdata4cs.Net.Requests
{
    /// <summary>
    /// Request is the base abstract class for all web service requests.  The web service expects some subclass of Request as input for all requests.
    /// </summary>
    abstract public class Request
    {
        /// <summary>
        /// Returns a dump of the object as a string
        /// </summary>
        /// <returns>The current state of the object, as a string</returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(GetType().ToString() + " [");
            foreach (PropertyInfo pi in GetType().GetProperties())
            {
                object val = pi.GetValue(this, null);
                sb.Append(pi.Name + "=" + (val != null ? val.ToString() : "<null>") + ", ");
            }
            sb.Append("]");
            return sb.ToString();
        }

    }
}
