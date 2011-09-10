﻿/*
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
using System.Net;
using System.Web;
using Livepvrdata4cs.Net.Responses;
using Livepvrdata4cs.Net.Requests;
using JsonExSerializer;
using System.Collections;

namespace Livepvrdata4cs
{
    /// <summary>
    /// Create instances of Client to connect to a Live PVR Data web service server.  Instances of this class are used to communicate with
    /// the web service to query for information about events or to request modifications to the override map.
    /// </summary>
    public class Client
    {
        static private Uri DEFAULT_URI = new Uri("http://www.livepvrdata.com");
        /// <summary>
        /// The default URI instances of this class will connect to if not specified; always points to the latest production version of the web service.
        /// </summary>
        static public Uri DefaultUri { get { return DEFAULT_URI; } }

        private const string DEFAULT_USER_AGENT = "livepvrdata4cs/1.0.0.0";
        /// <summary>
        /// The default user agent instances of this class will use if not specified; all consumers of this API are encouraged to NOT use this value!
        /// </summary>
        static public string DefaultUserAgent { get { return DEFAULT_USER_AGENT; } }

        private Uri uri;
        private string userAgent;

        /// <summary>
        /// Constructor; most users of this API SHOULD NOT use this constructor.  It's mainly used for testing and debugging against test versions of the web service.
        /// </summary>
        /// <param name="baseUri">The base URI of the web service to connect to.</param>
        /// <param name="userAgent">The custom user agent header to send on all requests; this should uniquely identify the application using this API.</param>
        public Client(Uri baseUri, string userAgent)
        {
            this.uri = baseUri;
            this.userAgent = userAgent;
        }

        /// <summary>
        /// Constructor; most users of this API SHOULD NOT use this constructor.  It's made available for testing of the API.
        /// </summary>
        /// <param name="baseUri">The base URI of the web service to connect to.</param>
        public Client(Uri baseUri) : this(baseUri, DEFAULT_USER_AGENT) { }

        /// <summary>
        /// Constructor; this is the constructor that most users SHOULD use.  Allows setting of the User-Agent header sent with each request.
        /// </summary>
        /// <param name="userAgent">The unique app identifier of the program using this API (e.g. "MyApp/1.0.0")</param>
        public Client(string userAgent) : this(DEFAULT_URI, userAgent) { }

        /// <summary>
        /// Default constructor; connects to the production version of the web service using the default agent header; NOT recommended, please specifiy a unique agent header when using this API.
        /// </summary>
        public Client() : this(DEFAULT_URI, DEFAULT_USER_AGENT) { }

        /// <summary>
        /// Query the status of an event
        /// </summary>
        /// <param name="type">The type of event you're querying; this is usually the program title ("NFL Football", "MLB Baseball", etc.)</param>
        /// <param name="teams">An array of the teams in the event being queried; this array must contain EXACTLY two unique elements, neither of which can be null or the empty string</param>
        /// <param name="start">This is the start time of the event being monitored; usually you'll provide the start time of the recording; this value should never change while monitoring an event (i.e. do NOT simply provide the current time on each call)</param>
        /// <returns>On success, returns a StatusResponse object if the event is one that can be monitored by the web service or null if the event is not a monitored event.  If an error occured during the query then an ErrorResponse object is returned; use the <code>isError()</code> method to determine which type was received and then cast to the appropriate subclass.</returns>
        /// <exception cref="System.IO.IOException">Thrown if there is a fatal error when attempting to communicate with the web service (i.e. Internet connection is down, web service is down, firewall is blocking access to web service, etc.)</exception>
        public Response GetStatus(string type, string[] teams, DateTime start)
        {
            return GetStatus(new StatusRequest(type, null, teams, Utils.UnixTimestamp(start)));
        }

        /// <summary>
        /// Query the status of an event
        /// </summary>
        /// <param name="type">The type of event you're querying; this is usually the program title ("NFL Football", "MLB Baseball", etc.)</param>
        /// <param name="desc">A description of the event (i.e. "Miami Dolphins at Buffalo Bills"); the web service will parse out the team info, but it has to follow the expected format.  Zap2It program descriptions follow the expected format so just pass those along.</param>
        /// <param name="start">This is the start time of the event being monitored; usually you'll provide the start time of the recording; this value should never change while monitoring an event (i.e. do NOT simply provide the current time on each call)</param>
        /// <returns>On success, returns a StatusResponse object if the event is one that can be monitored by the web service or null if the event is not a monitored event.  If an error occured during the query then an ErrorResponse object is returned; use the <code>isError()</code> method to determine which type was received and then cast to the appropriate subclass.</returns>
        /// <exception cref="System.IO.IOException">Thrown if there is a fatal error when attempting to communicate with the web service (i.e. Internet connection is down, web service is down, firewall is blocking access to web service, etc.)</exception>
        public Response GetStatus(string type, string desc, DateTime start)
        {
            return GetStatus(new StatusRequest(type, desc, null, Utils.UnixTimestamp(start)));
        }

        /// <summary>
        /// Query the status of an event
        /// </summary>
        /// <param name="req">A StatusRequest object describing the event being queried</param>
        /// <returns>On success, returns a StatusResponse object if the event is one that can be monitored by the web service or null if the event is not a monitored event.  If an error occured during the query then an ErrorResponse object is returned; use the <code>isError()</code> method to determine which type was received and then cast to the appropriate subclass.</returns>
        /// <exception cref="System.IO.IOException">Thrown if there is a fatal error when attempting to communicate with the web service (i.e. Internet connection is down, web service is down, firewall is blocking access to web service, etc.)</exception>
        public Response GetStatus(StatusRequest req)
        {
            Serializer serializer = new Serializer(typeof(StatusRequest));
            try
            {
                string payload = GetWebClient().UploadString(uri + "query", "q=" + HttpUtility.UrlEncode(serializer.Serialize(req)));
                serializer = new Serializer(typeof(Object));
                Hashtable jobj = (Hashtable)serializer.Deserialize(payload);
                if (jobj == null)
                    return null;
                else if (!(bool)jobj["isError"])
                {
                    serializer = new Serializer(typeof(StatusResponse));
                    return (StatusResponse)serializer.Deserialize(payload);
                }
                else
                {
                    serializer = new Serializer(typeof(ErrorResponse));
                    return (ErrorResponse)serializer.Deserialize(payload);
                }
            }
            catch (System.Net.WebException e)
            {
                throw new System.IO.IOException("IOError!", e);
            }
        }

        /// <summary>
        /// Submit an update to the web service override map.  When successful, an email is sent to the provided address, which contains a link that must be clicked on in order to commit the change to the online data store.
        /// </summary>
        /// <param name="epgName">The name of the team as seen in the EPG feed (i.e. "Oklahoma State")</param>
        /// <param name="feedName">The name of the team as seen at http://www.livepvrdata.com/events.jsp (i.e. "Oklahoma St")</param>
        /// <param name="email">A VALID email address; a confirmation link is sent to this address by the web service and the submission is not committed to the data store until the link in the email is clicked.</param>
        /// <returns>A SimpleResponse on success or an ErrorResponse on failure; cast the return value based on the <code>isError()</code> value of the returned object</returns>
        /// <exception cref="System.IO.IOException">Thrown if there is a fatal error when attempting to communicate with the web service (i.e. Internet connection is down, web service is down, firewall is blocking access to web service, etc.)</exception>
        public Response SubmitOverrideRequest(string epgName, string feedName, string email)
        {
            return SubmitOverrideRequest(new OverrideRequest(epgName, feedName, email));
        }

        /// <summary>
        /// Submit an update to the web service override map.  When successful, an email is sent to the provided address, which contains a link that must be clicked on in order to commit the change to the online data store.
        /// </summary>
        /// <param name="req">An OverrideRequest object describing the details of the override edit request to be submitted to the server.</param>
        /// <returns>A SimpleResponse on success or an ErrorResponse on failure; cast the return value based on the <code>isError()</code> value of the returned object</returns>
        /// <exception cref="System.IO.IOException">Thrown if there is a fatal error when attempting to communicate with the web service (i.e. Internet connection is down, web service is down, firewall is blocking access to web service, etc.)</exception>
        public Response SubmitOverrideRequest(OverrideRequest req)
        {
            Serializer serializer = new Serializer(typeof(OverrideRequest));
            try
            {
                string payload = GetWebClient().UploadString(uri + "goedit", "q=" + HttpUtility.UrlEncode(serializer.Serialize(req)));
                serializer = new Serializer(typeof(Object));
                Hashtable jobj = (Hashtable)serializer.Deserialize(payload);
                if (!(bool)jobj["isError"])
                {
                    serializer = new Serializer(typeof(SimpleResponse));
                    return (SimpleResponse)serializer.Deserialize(payload);
                }
                else
                {
                    serializer = new Serializer(typeof(ErrorResponse));
                    return (ErrorResponse)serializer.Deserialize(payload);
                }
            }
            catch (System.Net.WebException e)
            {
                throw new System.IO.IOException("IOError!", e);
            }
        }

        /// <summary>
        /// Return an instance of WebClient suitable for making the request to the web service.  The instance returned is configured with the proper user agent header, etc.
        /// </summary>
        /// <returns>The WebClient to be used for web requests</returns>
        protected WebClient GetWebClient()
        {
            WebClient clnt = new WebClient();
            clnt.Headers.Add("User-Agent:" + userAgent);
            clnt.Headers.Add("Content-Type: application/x-www-form-urlencoded");
            return clnt;
        }
    }
}