import { request, openRequest } from "./request";
// import axios from 'axios'

export function basePostFile(url, params) {
  return request({
    url: url,
    method: "post",
    data: params,
    headers: {
      "Content-Type": "multipart/form-data"
    }
  });
}

export function basePost(url, params) {
  return request({
    url: url,
    method: "post",
    data: params
  });
}

export function basePatch(url, params) {
  return request({
    url: url,
    method: "patch",
    data: params
  });
}
export function baseDel(url, params) {
  const href = getFullUrl(url, params);
  return request({
    url: href,
    method: "delete"
  });
}

export function baseGet(url, params) {
  const href = getFullUrl(url, params);
  return request({
    url: href,
    method: "get"
  });
}

function getFullUrl(url, params) {
  let query = "";
  for (let p in params) {
    if (typeof params[p] === "object") {
      query += `&${p}=${JSON.stringify(params[p])}`;
    } else {
      query += `&${p}=${params[p]}`;
    }
  }
  url += "?" + query.substring(1);
  return url;
}
