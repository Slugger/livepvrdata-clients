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

namespace LivePvrData.Data.Net.Responses
{
    public class EventsResponse : Response
    {
        private Event[] events;

        public EventsResponse() : base(false) { } // For JSON serialization only!
        public EventsResponse(Event[] events) : base(false)
        {
            this.events = events;
        }

        [JsonExProperty("events")]
        public Event[] Events { get { return events; } protected set { events = value; } }
    }
}
