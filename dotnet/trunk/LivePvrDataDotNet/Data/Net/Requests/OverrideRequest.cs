/*
 *      Copyright 2011-2012 Battams, Derek
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
    /// Encapsulates an IMMUTABLE override map request.  Since these objects are immutable, using the default constructor is useless for all users.
    /// The default constructor only exists to support the JSON serialization used for web communication.  Unfortunately, the serializer requires
    /// the default constructor to be public.
    /// </summary>
    public class OverrideRequest : Request
    {
        private string epgName;
        private string feedName;

        /// <summary>
        /// Default constructor, DO NOT USE!  Used by JSON serializer only!
        /// </summary>
        public OverrideRequest() {}

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="epgName">The name of the team as found in the EPG feed</param>
        /// <param name="feedName">The name of the team as found in the livepvrdata monitoring feed</param>
        public OverrideRequest(string epgName, string feedName)
        {
            this.epgName = epgName;
            this.feedName = feedName;
        }

        /// <summary>
        /// The EPG name of the override entry (i.e. "Michigan State")
        /// </summary>
        [JsonExProperty("epgName")]
        public string EpgName { get { return epgName; } protected set { epgName = value; } }
        
        /// <summary>
        /// The monitor feed name of the entry; get this from events.jsp on the livepvrdata.com site
        /// </summary>
        [JsonExProperty("feedName")]
        public string FeedName { get { return feedName; } protected set { feedName = value; } }
    }
}
