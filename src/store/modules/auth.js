import { login, logout, getUserInfo } from "@/api/auth";
import { parse } from "@/utils/util";

const auth = {
  state: {
    sessionToken: localStorage.getItem("session_token") || "",
    // appId: config.appId,
    userInfo: parse(localStorage.getItem("userInfo")) || {}
  },
  mutations: {
    SET_SESSION_TOKEN: (state, sessionToken) => {
      state.sessionToken = sessionToken;
      localStorage.setItem("session_token", sessionToken);
    },
    // SET_APP_ID: (state, appId) => {
    //   state.appId = appId;
    //   localStorage.setItem("app_id", appId);
    // },
    SET_USER_INFO: (state, userInfo) => {
      state.userInfo = userInfo;
      localStorage.setItem("userInfo", JSON.stringify(userInfo));
    }
  },
  actions: {
    // setAppId({ commit }, appId) {
    //   commit("SET_APP_ID", appId);
    // },

    LoginByEmail({ commit }, userInfo) {
      let { email, password } = userInfo;
      return new Promise((resolve, reject) => {
        login(email, password)
          .then(res => {
            const data = res.data;
            commit("SET_SESSION_TOKEN", data.sessionToken);
            resolve();
          })
          .catch(err => {
            reject(err);
          });
      });
    },
    Logout({ commit }) {
      return new Promise((resolve, reject) => {
        logout()
          .then(() => {
            commit("SET_SESSION_TOKEN", "");
            resolve();
          })
          .catch(err => {
            reject(err);
          });
      });
    },
    GetUserInfo({ commit }) {
      return new Promise((resolve, reject) => {
        getUserInfo()
          .then(res => {
            const data = res.data;
            commit("SET_USER_INFO", data);
            resolve();
          })
          .catch(err => {
            reject(err);
          });
      });
    }
  }
};

export default auth;
