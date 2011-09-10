package com.google.code.livepvrdata4j;

import org.json.JSONException;
import org.json.JSONObject;

import com.google.gson.Gson;
import com.livepvrdata.data.net.resp.ExceptionError;
import com.livepvrdata.data.net.resp.Response;
import com.livepvrdata.data.net.resp.SimpleResponse;
import com.livepvrdata.data.net.resp.StatusResponse;

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

	static private boolean isError(String input) {
		if(input.equals("null"))
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
