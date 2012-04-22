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
using JsonExSerializer;
using System.Text;
using System.Reflection;

namespace LivePvrData.Data.Net.Responses
{
    /// <summary>
    /// All responses from the web service return a subclass of Response.  Use the <code>isError()</code> on these objects to determine if the response is an error or not.
    /// </summary>
    abstract public class Response
    {
        private bool isError;
        /// <summary>
        /// Denotes if this instance represents an error response; if this property is true then the instance can safely be cast to an ErrorResponse object.
        /// </summary>
        [JsonExProperty("isError")]
        public bool IsError { get { return isError; } protected set { isError = value; } }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="isError">If this instance is representing an error response then this argument should be true, false otherwise</param>
        public Response(bool isError) { this.isError = isError; }

        /// <summary>
        /// Dumps the state of the object to a string
        /// </summary>
        /// <returns>The state of the object, as a string</returns>
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
