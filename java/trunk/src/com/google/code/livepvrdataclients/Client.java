/*
 *      Copyright 2010-2012 livepvrdata.com
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
package com.google.code.livepvrdataclients;

import java.io.IOException;
import java.util.ArrayList;
import java.util.Date;
import java.util.List;

import javax.crypto.Mac;
import javax.crypto.SecretKey;
import javax.crypto.spec.SecretKeySpec;

import org.apache.commons.codec.digest.DigestUtils;
import org.apache.http.NameValuePair;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.utils.URLEncodedUtils;
import org.apache.http.impl.client.BasicResponseHandler;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.message.AbstractHttpMessage;
import org.apache.http.message.BasicNameValuePair;
import org.apache.http.params.HttpProtocolParams;

import com.google.gson.Gson;
import com.livepvrdata.common.data.net.req.EventsRequest;
import com.livepvrdata.common.data.net.req.OverrideSubmissionRequest;
import com.livepvrdata.common.data.net.req.StatusRequest;
import com.livepvrdata.common.data.net.resp.Response;

/**
 * @author dbattams
 *
 */
public final class Client {
	static private final String VERSION = "10.0.0.0";
	static private final String BASE_URI = "http://www.livepvrdata.com";
	static private final String DEFAULT_USER_AGENT = "livepvrdata4j/" + VERSION;
	static private final Gson GSON = new Gson();
	static private final String calculateSignature(String secret, String payload) {
		try {
			SecretKey key = new SecretKeySpec(secret.getBytes("UTF-8"), "HmacSHA256");
			Mac mac = Mac.getInstance("HmacSHA256");
			mac.init(key);
			final byte[] hash = mac.doFinal(payload.getBytes("UTF-8"));
			return DigestUtils.md5Hex(hash);
		} catch(Exception e) {
			throw new RuntimeException(e);
		}
	}
	
	private final String userAgent;
	private final String baseUri;
	private final String email;
	private final String secret;

	public Client() {
		this(DEFAULT_USER_AGENT, BASE_URI, null, null);
	}

	public Client(String email, String secret) {
		this(DEFAULT_USER_AGENT, BASE_URI, email, secret);
	}

	public Client(String userAgent, String email, String secret) {
		this(userAgent, BASE_URI, email, secret);
	}
	
	public Client(String userAgent, String baseUri, String email, String secret) {
		this.userAgent = userAgent;
		if(baseUri != null)
			this.baseUri = baseUri;
		else
			this.baseUri = BASE_URI;
		this.email = email;
		this.secret = secret;
	}

	public Client(String baseUri) {
		this("TestEnv", baseUri, null, null);
	}
	
	private final void signRequest(AbstractHttpMessage req, String payload) {
		if(email != null && email.length() > 0 && secret != null && secret.length() > 0) {
			req.setHeader("livepvrdata-email", email);
			req.setHeader("livepvrdata-signature", calculateSignature(secret, payload));
		}
	}
	
	private final HttpClient getClient() {
		HttpClient clnt = new DefaultHttpClient();
		HttpProtocolParams.setUserAgent(clnt.getParams(), userAgent);
		return clnt;
	}

	public Response addGlobalOverride(String epgName, String feedName) throws IOException {
		HttpClient clnt = getClient();
		List<NameValuePair> args = new ArrayList<NameValuePair>();
		OverrideSubmissionRequest req = new OverrideSubmissionRequest(epgName, feedName);
		args.add(new BasicNameValuePair("q", GSON.toJson(req)));
		HttpGet httpGet = new HttpGet(baseUri + String.format("/api/override?%s", URLEncodedUtils.format(args, "UTF-8")));
		return ResponseFactory.getResponse(clnt.execute(httpGet, new BasicResponseHandler()));
	}

	public Response getEventsForDate(String eventName, Date d) throws IOException {
		HttpClient clnt = getClient();
		List<NameValuePair> args = new ArrayList<NameValuePair>();
		EventsRequest req = new EventsRequest(eventName, ((long)(d.getTime() / 1000)));
		String payload = GSON.toJson(req);
		args.add(new BasicNameValuePair("q", payload));
		HttpGet httpGet = new HttpGet(baseUri + String.format("/api/qryEvents?%s", URLEncodedUtils.format(args, "UTF-8")));
		signRequest(httpGet, payload);
		return ResponseFactory.getEventsResponse(clnt.execute(httpGet, new BasicResponseHandler()));
	}

	public Response getStatus(String eventName, String desc, Date start) throws IOException {
		StatusRequest req = new StatusRequest(eventName, desc, ((long)(start.getTime() / 1000)));
		HttpClient clnt = getClient();
		List<NameValuePair> args = new ArrayList<NameValuePair>();
		String payload = GSON.toJson(req);
		args.add(new BasicNameValuePair("q", payload));
		HttpGet httpGet = new HttpGet(baseUri + String.format("/api/qryStatus?%s", URLEncodedUtils.format(args, "UTF-8")));
		signRequest(httpGet, payload);
		String resp = clnt.execute(httpGet, new BasicResponseHandler());
		return ResponseFactory.getStatusResponse(resp);
	}

	public Response getStatus(String eventName, String[] teams, Date start) throws IOException {
		StatusRequest req = new StatusRequest(eventName, teams, ((long)(start.getTime() / 1000)));
		HttpClient clnt = getClient();
		List<NameValuePair> args = new ArrayList<NameValuePair>();
		String payload = GSON.toJson(req);
		args.add(new BasicNameValuePair("q", payload));
		HttpGet httpGet = new HttpGet(baseUri + String.format("/api/qryStatus?%s", URLEncodedUtils.format(args, "UTF-8")));
		signRequest(httpGet, payload);
		String resp = clnt.execute(httpGet, new BasicResponseHandler());
		return ResponseFactory.getStatusResponse(resp);
	}
}