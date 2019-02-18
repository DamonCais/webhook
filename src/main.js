import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'




import _ from 'lodash'
import ElementUI from 'element-ui';

import './assets/scss/global.scss'

import './assets/scss/element-variables.scss'
import 'flex.box'
import "@/mixin";

Vue.use(ElementUI);




import myTable from "@/components/myTable";
import myForm from "@/components/myForm";
import myFormItem from "@/components/myFormItem";

Vue.component("myTable", myTable);
Vue.component("myForm", myForm);
Vue.component("myFormItem", myFormItem);











Vue.config.productionTip = false

new Vue({
  router,
  store,
  render: h => h(App)
}).$mount('#app')
