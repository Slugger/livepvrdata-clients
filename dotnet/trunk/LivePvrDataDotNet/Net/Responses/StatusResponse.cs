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
using System;
using System.Text;
using System.Reflection;

namespace LivePvrData.Net.Responses
{
    /// <summary>
    /// A successful event query request will result in a StatusResponse object being returned; these objects contain all of the details about the queried event
    /// </summary>
    public class StatusResponse : Response
    {
        private string title;
        private string status;
        private bool isValid;
        private bool isActive;
        private bool isComplete;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="title">The title of the event</param>
        /// <param name="status">The status of the event as read from the monitor source; this is an arbitrary value and is not usually useful except for Live PVR Data development; provided as a courtesy only</param>
        /// <param name="isValid">Is this response valid?  In other words, was the query successful?  If this value if false then you CANNOT trust the other boolean properties in the object.</param>
        /// <param name="isActive">Is the event still active?</param>
        /// <param name="isComplete">Is the event complete?</param>
        public StatusResponse(string title, string status, bool isValid, bool isActive, bool isComplete) : base(false)
        {
            this.title = title;
            this.status = status;
            this.isValid = isValid;
            this.isActive = isActive;
            this.isComplete = isComplete;
        }

        /// <summary>
        /// Constructor for JSON serialization; DO NOT USE!
        /// </summary>
        public StatusResponse() : base(false) { }

        /// <summary>
        /// The title of the event queried, normalized (i.e. "NFL Football", "MLB Baseball", etc.)
        /// </summary>
        [JsonExProperty("title")]
        public string Title { get { return title; } protected set { title = value; } }

        /// <summary>
        /// The status of the event as received from the monitor source.  This is an arbitrary string, but is processed by the server to determin the values of the other booleans in this object.  It is provided as a courtesy only and is mainly used for debugging purposes.
        /// </summary>
        [JsonExProperty("status")]
        public string Status { get { return status; } protected set { status = value; } }

        /// <summary>
        /// Are the results of this query valid?  If this value is False then the other boolean values should NOT be trusted/used
        /// </summary>
        [JsonExProperty("isValid")]
        public bool IsValid { get { return isValid; } protected set { isValid = value; } }

        /// <summary>
        /// Is the event still in progress?
        /// </summary>
        [JsonExProperty("isActive")]
        public bool IsActive { get { return isActive; } protected set { isActive = value; } }

        /// <summary>
        /// Is the event complete?
        /// </summary>
        [JsonExProperty("isComplete")]
        public bool IsComplete { get { return isComplete; } protected set { isComplete = value; } }
    }
}
