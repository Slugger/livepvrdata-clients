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
using JsonExSerializer;

namespace Livepvrdata4cs.Net.Requests
{
    /// <summary>
    /// An IMMUTABLE object that encapsulates an event status query.  Since these objects are immutable, the default constructor is useless to
    /// ALL users.  It is only provided for JSON serializaton of the object.
    /// </summary>
    public class StatusRequest : Request
    {
        private string type;
        private string details;
        private string[] teams;
        private long start;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">The type of the event being monitored; this is simply the program title ("MLB Baseball", "NFL Football", etc.)</param>
        /// <param name="details">Details of the event to be monitored (i.e. "Miami Dolphins at Buffalo Bills"); when possible, specify the teams array instead; ignored if the teams array is non-null</param>
        /// <param name="teams">A two element array of teams involved in the event; they must be unique, non-null, non-empty strings; if the array is non-null then the details argument is ignored; when possible, use the array argument.</param>
        /// <param name="start">The start time of the event being monitored, as a Unix timestamp value; successive calls to query the same event should use the same start time over and over; do NOT use the current time on each call!</param>
        public StatusRequest(string type, string details, string[] teams, long? start)
        {
            this.type = type;
            this.details = details;
            this.teams = teams;
            if (start != null)
                this.start = (long)start;
            else
                this.start = Utils.UnixTimestampNow();
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">The type of event; the program title</param>
        /// <param name="details">The episode details</param>
        /// <param name="teams">The two element array of teams; must be unique, non-null, non-empty strings; if this arg is not null then the details arg is ignored</param>
        /// <param name="start">The start time of the event; successive calls to query the same event should be using the same start time; do NOT pass the current time!</param>
        public StatusRequest(string type, string details, string[] teams, DateTime start) : this(type, details, teams, start != null ? (long?)Utils.UnixTimestamp(start) : null) { }

        /// <summary>
        /// Default constructor; DO NOT USE!  Required for JSON serialization.
        /// </summary>
        public StatusRequest() { }

        /// <summary>
        /// The type of event being monitored; this is the program title (i.e. "NFL Football", "MLB Baseball", etc.)
        /// </summary>
        [JsonExProperty("type")]
        public string Type { get { return type; } protected set { type = value; } }

        /// <summary>
        /// A description of the event; used when you can't provide the teams array, but the teams array is preferred.
        /// 
        /// The web service deals with the parsing and extraction of team info from this string, but it must be in a certain format.
        /// Zap2It episode strings are in the expected format so just pass the episode info along in this case.
        /// </summary>
        [JsonExProperty("details")]
        public string Details { get { return details; } protected set { details = value; } }

        /// <summary>
        /// An array of teams involved in the event; if provided it must be exactly two elements, both unique, non-null and non-empty strings; this is the preferred way to provide team info for an event to be queried
        /// </summary>
        [JsonExProperty("teams")]
        public string[] Teams { get { return teams; } protected set { teams = value; } }

        /// <summary>
        /// The start time of the event being queried.  Successive calls to query the same event should use the same time over and over.  Do NOT
        /// simply pass the current time of day on each call to this method!
        /// </summary>
        [JsonExProperty("start")]
        public long Start { get { return start; } protected set { start = value; } }
    }
}