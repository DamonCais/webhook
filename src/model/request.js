import axios from "axios";
import {
  Message
} from "element-ui";
import store from "@/store";
// import relogin from "@/plugins/reLogin/function";

// 创建axios实例

const request = axios.create({
  baseURL: `http://jvstest.juvending.cn/api/`, // api的base_url
  // baseURL: `http://jvs.juvending.cn/api/`, // api的base_url
  timeout: 15000 // 请求超时时间
});

// request拦截器
request.interceptors.request.use(
  config => {
    // if (store.getters.sessionToken) {
    //   config.headers["X-Session-Token"] = store.getters.sessionToken;
    // }
    // config.headers["X-App-Id"] = store.getters.appId;
    return config;
  },
  error => {
    // Do something with request error
    Promise.reject(error);
  }
);

// respone拦截器
request.interceptors.response.use(
  response => {
    return response;
  },
  error => {
    console.log(error.response);
    if (error.response && error.response.status === 401) {
      return Promise.reject(error);
    } else {
      // store.commit("setIsError", true);
      let message =
        _.get(error, "response.data.message") || _.get(error, "message");
      Message({
        message: message,
        type: "error",
        duration: 2 * 1000
      });
      return Promise.reject(error);
    }
  }
);


export {
  request,

};