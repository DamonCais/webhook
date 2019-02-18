import config from '@/config.js';
let baseUrl = config.baseURL;
let apiSetting = localStorage.getItem('__env');
baseUrl = apiSetting === 'dev' ? '' : baseUrl;
export default baseUrl