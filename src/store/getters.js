const getters = {
  interfaceLanguage: state => state.editor.interfaceLanguage,
  eduForms: state => state.editor.eduForms,
  eduSteps: state => state.editor.eduSteps,

  sidebarOpen: state => state.app.sidebarOpen,
  layout: state => state.app.layout,
  navPos: state => state.app.layout.navPos,
  toolbar: state => state.app.layout.toolbar,
  footer: state => state.app.layout.footer,
  boxed: state => state.app.layout.boxed,
  roundedCorners: state => state.app.layout.roundedCorners,
  viewAnimation: state => state.app.layout.viewAnimation,
  isLogged: state => state.app.logged,
  splashScreen: state => state.app.splashScreen,
  version: state => state.app.version,
  commit: state => state.app.commit,
  eduYear: state => state.app.eduYear,
  eduDirectory: state => state.app.eduDirectory,
  currency: state => state.app.currency,
  schoolConfig: state => state.app.schoolConfig,
  appId: state => state.app.appId,

  tableSetting: state => state.table.tableSetting,

  sessionToken: state => state.auth.sessionToken,
  userInfo: state => state.auth.userInfo
};
export default getters;
