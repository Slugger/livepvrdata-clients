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
    public class Event
    {
        private string[] participants;
        private string status;
        private bool isComplete;

        public Event() { } // For JSON serialization only!
        public Event(string[] participants, string status, bool isComplete)
        {
            this.participants = participants;
            this.status = status;
            this.isComplete = isComplete;
        }

        [JsonExProperty("participants")]
        public string[] Participants { get { return participants; } protected set { participants = value; } }

        [JsonExProperty("status")]
        public string Status { get { return status; } protected set { status = value; } }

        [JsonExProperty("isComplete")]
        public bool IsComplete { get { return isComplete; } protected set { isComplete = value; } }

        [JsonExIgnore]
        public string Description { get { return String.Format("{0} vs. {1}", participants[0], participants[1]); } protected set { throw new InvalidOperationException("Cannot modify property."); } }

        public bool isParticipating(string name)
        {
            foreach (string s in participants)
                if (s.Equals(name))
                    return true;
            return false;
        }

        public bool isParticipating(string[] names)
        {
            int matches = 0;
            foreach(string s in participants)
                if(isParticipating(s) && ++matches == 2)
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
