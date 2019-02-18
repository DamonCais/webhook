const editor = {
  state: {
    interfaceLanguage: localStorage.getItem("interfaceLanguage") || "en",
    eduForms: {},
    eduSteps: {}
  },
  mutations: {
    SET_INTERFACE_Language: (state, interfaceLanguage) => {
      state.interfaceLanguage = interfaceLanguage;
      localStorage.setItem("interfaceLanguage", interfaceLanguage);
    },
    SET_FORMS: (state, forms) => {
      state.eduForms = forms;
    },
    SET_STEPS: (state, steps) => {
      state.eduSteps = steps;
    }
  },
  actions: {
    setInterfaceLanguage({ commit }, interfaceLanguage) {
      commit("SET_INTERFACE_Language", interfaceLanguage);
    },
    setForms({ commit }, forms) {
      commit("SET_FORMS", forms);
    },
    setSteps({ commit }, steps) {
      commit("SET_STEPS", steps);
    }
  }
};

export default editor;
