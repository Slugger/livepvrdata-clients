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

namespace LivePvrData.Data
{
    /// <summary>
    /// Encapsulates the details and status of an event that the web service is capable of monitoring
    /// </summary>
    public class Event
    {
        private string[] participants;
        private string status;
        private bool isComplete;

        /// <summary>
        /// Do NOT use; for JSON serialization only!
        /// </summary>
        public Event() { }

        /// <summary>
        /// Create an Event as received from the web service
        /// </summary>
        /// <param name="participants">The participants of the event (i.e. teams, "Miami Dolphins", etc.)</param>
        /// <param name="status">The current status of the event as reproted by the web service (i.e. "3rd quarter", "2:14 1st Period", etc.).  This status is for informational purposes only and can't be used to determine much on its own; the object constructor must tell this ctor the meaning of the status via the <code>isComplete</code> argument.</param>
        /// <param name="isComplete">An interpretation of the status argument denoting if this event is complete.</param>
        public Event(string[] participants, string status, bool isComplete)
        {
            this.participants = participants;
            this.status = status;
            this.isComplete = isComplete;
        }

        /// <summary>
        /// Get the list of participants in the event, represented as an array of strings
        /// </summary>
        [JsonExProperty("participants")]
        public string[] Participants { get { return participants; } protected set { participants = value; } }

        /// <summary>
        /// Get the status of the event; this is a free form string that requires interpretation; it is provided for informational/debugging purposes only
        /// </summary>
        [JsonExProperty("status")]
        public string Status { get { return status; } protected set { status = value; } }

        /// <summary>
        /// The definitive interpretation of the <code>Status</code> property; denotes whether this event is complete or still in progress at the time this object was created
        /// </summary>
        [JsonExProperty("isComplete")]
        public bool IsComplete { get { return isComplete; } protected set { isComplete = value; } }

        /// <summary>
        /// A user friendly description of event
        /// </summary>
        [JsonExIgnore]
        public string Description { get { return String.Format("{0} vs. {1}", participants[0], participants[1]); } protected set { throw new InvalidOperationException("Cannot modify property."); } }

        /// <summary>
        /// Determine if the given participant name is participating in the event represented by this object
        /// </summary>
        /// <param name="name">The name of a participant (i.e. a team name)</param>
        /// <returns>True if the given name is participating in this event or false otherwise</returns>
        public bool IsParticipating(string name)
        {
            foreach (string s in participants)
                if (s.Equals(name))
                    return true;
            return false;
        }

        /// <summary>
        /// Determine if all names given are participating in this event
        /// </summary>
        /// <param name="names">An array of strings with EXACTLY size=2 elements; each name must be a participant in this event</param>
        /// <returns>True if both names given are participating in this event or false otherwise</returns>
        public bool IsParticipating(string[] names)
        {
            int matches = 0;
            foreach(string s in participants)
                if(IsParticipating(s) && ++matches == 2)
                    return true;
            return false;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder("Event[participants={");
            foreach(string s in participants)
                sb.Append(String.Format("{0}, ", s));
            sb.Append(String.Format("}}, status={0}, isComplete={1}]", status, isComplete));
            return sb.ToString();
        }
    }
}
