/*
 *      Copyright 2010-2011 livepvrdata.com
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
package com.google.code.livepvrdataclients.java;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import org.apache.http.NameValuePair;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.utils.URLEncodedUtils;
import org.apache.http.impl.client.BasicResponseHandler;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.params.HttpProtocolParams;

import com.google.gson.Gson;
import com.livepvrdata.data.net.req.OverrideSubmissionRequest;
import com.livepvrdata.data.net.req.StatusRequest;
import com.livepvrdata.data.net.resp.Response;

/**
 * @author dbattams
 *
 */
public final class Client {
	static private final String VERSION = "5.0.0.0";
	static private final String BASE_URI = "http://5.livepvrdata.appspot.com";
	static private final String DEFAULT_USER_AGENT = "livepvrdata4j/" + VERSION;
	static private final Gson GSON = new Gson();

	private final String userAgent;
	private final String baseUri;

	public Client() {
		this(DEFAULT_USER_AGENT, BASE_URI);
	}

	public Client(String userAgent, String baseUri) {
		this.userAgent = userAgent;
		if(baseUri != null)
			this.baseUri = baseUri;
		else
			this.baseUri = BASE_URI;
	}

	public Client(String baseUri) {
		this.baseUri = baseUri;
		userAgent = "TestEnv";
	}

	private final HttpClient getClient() {
		HttpClient clnt = new DefaultHttpClient();
		HttpProtocolParams.setUserAgent(clnt.getParams(), userAgent);
		return clnt;
	}

	public Response addGlobalOverride(String epgName, String feedName, String email) throws IOException {
		HttpClient clnt = getClient();
		List<NameValuePair> args = new ArrayList<NameValuePair>();
		OverrideSubmissionRequest req = new OverrideSubmissionRequest(epgName, feedName, email);
		args.add(new BasicNameValuePair("q", GSON.toJson(req)));
		HttpGet httpGet = new HttpGet(baseUri + String.format("/goedit?%s", URLEncodedUtils.format(args, "UTF-8")));
		return ResponseFactory.getResponse(clnt.execute(httpGet, new BasicResponseHandler()));
	}


	//	public Event[] getEventsForDate(String eventName, Date d) throws IOException, RestException {
	//		String uri = buildUri("/events/");
	//		com.livepvrdata.shared.Request req = new com.livepvrdata.shared.Request();
	//		req.setParam("eventId", eventName);
	//		req.setParam("date", new SimpleDateFormat("yyyyMMdd").format(d));
	//		com.livepvrdata.shared.Response r = submit(uri, GSON.toJson(req));
	//		return GSON.fromJson((String)r.getResponse(), Event[].class);
	//	}

	public Response getStatus(String eventName, String desc, Date start) throws IOException {
		StatusRequest req = new StatusRequest(eventName, desc, ((long)(start.getTime() / 1000)));
		HttpClient clnt = getClient();
		List<NameValuePair> args = new ArrayList<NameValuePair>();
		args.add(new BasicNameValuePair("q", GSON.toJson(req)));
		HttpGet httpGet = new HttpGet(baseUri + String.format("/query?%s", URLEncodedUtils.format(args, "UTF-8")));
		String resp = clnt.execute(httpGet, new BasicResponseHandler());
		return ResponseFactory.getStatusResponse(resp);
	}

	public Response getStatus(String eventName, String[] teams, Date start) throws IOException {
		StatusRequest req = new StatusRequest(eventName, teams, ((long)(start.getTime() / 1000)));
		HttpClient clnt = getClient();
		List<NameValuePair> args = new ArrayList<NameValuePair>();
		args.add(new BasicNameValuePair("q", GSON.toJson(req)));
		HttpGet httpGet = new HttpGet(baseUri + String.format("/query?%s", URLEncodedUtils.format(args, "UTF-8")));
		String resp = clnt.execute(httpGet, new BasicResponseHandler());
		return ResponseFactory.getStatusResponse(resp);
	}
}
