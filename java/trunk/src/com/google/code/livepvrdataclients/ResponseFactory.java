/*
 *      Copyright 2011-2012 livepvrdata.com
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

import org.json.JSONException;
import org.json.JSONObject;

import com.google.gson.Gson;
import com.livepvrdata.common.data.net.resp.EventsResponse;
import com.livepvrdata.common.data.net.resp.ExceptionError;
import com.livepvrdata.common.data.net.resp.Response;
import com.livepvrdata.common.data.net.resp.SimpleResponse;
import com.livepvrdata.common.data.net.resp.StatusResponse;

public final class ResponseFactory {
	static private final Gson GSON = new Gson();

	static public Response getStatusResponse(String input) {
		if(!isError(input))
			return GSON.fromJson(input, StatusResponse.class);
		return GSON.fromJson(input, ExceptionError.class);
	}

	static public Response getResponse(String input) {
		if(!isError(input))
			return GSON.fromJson(input, SimpleResponse.class);
		return GSON.fromJson(input, ExceptionError.class);
	}
	
	static public Response getEventsResponse(String input) {
		if(!isError(input))
			return GSON.fromJson(input, EventsResponse.class);
		return GSON.fromJson(input, ExceptionError.class);
	}

	static private boolean isError(String input) {
		if("null".equals(input))
			return false;
		try {
			JSONObject jobj = new JSONObject(input);
			return jobj.getBoolean(Response.PROP_IS_ERR);
		} catch (JSONException e) {
			throw new RuntimeException(e);
		}
	}

	private ResponseFactory() {}
}
