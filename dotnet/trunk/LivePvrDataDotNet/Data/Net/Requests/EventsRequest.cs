/*
 *      Copyright 2012 Battams, Derek
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
using JsonExSerializer;

namespace LivePvrData.Data.Net.Requests
{
    /// <summary>
    /// A class representing the details of an EventsRequest query to be sent to the web service for processing.
    /// An EventsRequest queries the web service for a list of events of a given type that the web service is able
    /// to monitor on a given date.
    /// </summary>
    public class EventsRequest : Request
    {
        private string type;
        private long start;

        public EventsRequest() { } // For JSON serialization only!

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">The type of event to query (i.e. "NHL Hockey")</param>
        /// <param name="start">The date to query events for, as a Unix timestamp</param>
        public EventsRequest(string type, long start)
        {
            this.type = type;
            this.start = start;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">The type of event to query (i.e. "NHL Hockey")</param>
        /// <param name="start">The date to query events for; MUST be UTC; time of day is ignored in object, only the date is used</param>
        public EventsRequest(string type, DateTime start) : this(type, Utils.UnixTimestamp(start)) { }

        /// <summary>
        /// Get the type of events this request will be querying for
        /// </summary>
        [JsonExProperty("type")]
        public string Type { get { return type; } protected set { type = value; } }

        /// <summary>
        /// Get the Unix timestamp representation of the date the query will be searching for; 
        /// </summary>
        [JsonExProperty("start")]
        public long StartTimestamp { get { return start; } protected set { start = value; } }

        /// <summary>
        /// Get the DateTime representation of the date the query will be searching for; the time of day could be anything in this object, but it always ignored internally.
        /// Only the date portion of this object is relevant as far as the web service is concerned.
        /// </summary>
        [JsonExIgnore]
        public DateTime StartDateTime { get { return new DateTime(1000L * start); } protected set { throw new InvalidOperationException("Cannot modify property."); } }
    }
}
