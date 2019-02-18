import { parse } from "@/utils/util";
import config from "@/config.js";

const app = {
  state: {
    layout: {
      navPos: "left", //top, bottom, left, right, false
      toolbar: "top", //top, bottom, false
      footer: true, //above, below, false
      boxed: false, //true, false
      roundedCorners: false, //true, false
      viewAnimation: false // "fade-top", fade-left, fade-right, fade-top, fade-top-in-out, fade-bottom, fade-bottom-in-out, fade, false
    },
    sidebarOpen: true,
    splashScreen: true,
    logged: localStorage.getItem("session_token") ? true : false,
    eduYear: parse(localStorage.getItem("eduYear")) || {},
    eduDirectory: parse(localStorage.getItem("eduDirectory")),
    version: "",
    commit: "",
    currency: "HKD",
    schoolConfig: {},
    appId: config.appId
  },
  mutations: {
    setVersion(state, version) {
      state.version = version;
    },
    setCommit(state, commit) {
      state.commit = commit;
    },
    setSidebarOpen(state, open) {
      state.sidebarOpen = open;
    },
    setLayout(state, payload) {
      if (payload && payload.navPos !== undefined)
        state.layout.navPos = payload.navPos;

      if (payload && payload.toolbar !== undefined)
        state.layout.toolbar = payload.toolbar;

      if (payload && payload.footer !== undefined)
        state.layout.footer = payload.footer;

      if (payload && payload.boxed !== undefined)
        state.layout.boxed = payload.boxed;

      if (payload && payload.roundedCorners !== undefined)
        state.layout.roundedCorners = payload.roundedCorners;

      if (payload && payload.viewAnimation !== undefined)
        state.layout.viewAnimation = payload.viewAnimation;
    },
    setLogin(state, payload) {
      state.logged = true;
    },
    setLogout(state, payload) {
      state.layout.navPos = null;
      state.layout.toolbar = null;
      state.logged = false;
    },
    setSplashScreen(state, payload) {
      state.splashScreen = payload;
    },
    setYear(state, year) {
      state.eduYear = year;
      localStorage.setItem("eduYear", JSON.stringify(year));
    },
    setDirectory(state, directory) {
      state.eduDirectory = directory;
      localStorage.setItem("eduDirectory", JSON.stringify(directory));
    },
    SET_SCHOOL_CONFIG(state, schoolConfig) {
      state.schoolConfig = schoolConfig;
      state.appId = _.get(schoolConfig, "appId");
    }
  },
  actions: {
    setSchoolConfig({ commit }, schoolConfig) {
      commit("SET_SCHOOL_CONFIG", schoolConfig);
    }
  }
};

export default app;
