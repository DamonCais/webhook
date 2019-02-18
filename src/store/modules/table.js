import { parse } from "@/utils/util";

const table = {
  state: {
    tableSetting: parse(localStorage.getItem("tableSetting")) || {}
  },
  mutations: {
    SET_TABLE_SETTING: (state, tableSetting) => {
      state.tableSetting = _.assign(state.tableSetting, tableSetting);
      localStorage.setItem("tableSetting", JSON.stringify(state.tableSetting));
      // window.location.reload();
    }
  },
  actions: {
    setTableSetting({ commit }, tableSetting) {
      commit("SET_TABLE_SETTING", tableSetting);
    }
  }
};

export default table;
