import { openRequest } from "./request";
import store from "@/store";

export function login(login, password) {
  return openRequest({
    url: "/auth/session",
    method: "post",
    data: {
      login,
      password,
      clientType: "console-web"
    }
  });
}

export function getUserInfo() {
  return openRequest({
    url: "/auth/me",
    method: "get",
    headers: {
      "X-Session-Token": store.getters.sessionToken
    }
  });
}
export function getPermission() {
  return openRequest({
    url: "/auth/me/modules",
    method: "get",
    headers: {
      "X-Session-Token": store.getters.sessionToken
    }
  });
}

export function logout() {
  return openRequest({
    url: "/auth/session",
    method: "delete",
    headers: {
      "X-Session-Token": store.getters.sessionToken
    }
  });
}

export function forgetPassword(email) {
  return openRequest({
    url: "/auth/request-password-reset",
    method: "post",
    data: {
      email
    }
  });
}

export function resetPassword(verifyCode, newPassword) {
  return openRequest({
    url: "/auth/do-password-reset",
    method: "post",
    data: {
      verifyCode,
      newPassword
    }
  });
}
