var glob = require("glob");
const fs = require("fs");
const _ = require("lodash");

let files = glob.sync("./*.cs");
let apiObj = {};
for (var file of files) {
    let str = fs.readFileSync(file, 'utf8');
    let reg = /\sHttpResponseMessage\s(.*?)\((.*?)\)/g;
    let arr = str.match(reg);
    // console.log(arr.length);
    let fileReg = /\.\/(.*?)Controller.cs/;
    console.log(fileReg.exec(file)[1]);
    let filePrefix = fileReg.exec(file)[1];
    let apiStr = `
    import { request } from "./request";
    class ${filePrefix}API{

    `;
    if (arr && arr.length) {

        for (let a of arr) {
            let re = /\sHttpResponseMessage\s(.*?)\((.*?)\)/;
            let str1 = re.exec(a)[1];
            let str2 = re.exec(a)[2];
            // console.log(str1);
            // console.log(str2);
            if (!apiObj[str1]) {
                apiObj[str1] = [];
            }
            let str2arr = str2.split(',');
            str2arr.map(s => {
                apiObj[str1].push(s.split(' ').pop())
            })
            // console.log(apiObj);

            apiStr += `
            ${str1}(params) {
                let objList = ${JSON.stringify(apiObj[str1])};
                params=_.pick(params,objList);
                const href = getFullUrl('/${filePrefix}/${str1}', params);
                return request({
                    url:href,
                    method:'get'
                })
            }
            
            `

        }

    }
    apiStr += `
    }
    export default ${filePrefix}API;
    function getFullUrl(url, params) {
        let query = "";
        for (let p in params) {
            let str = '&' + p + '=' + params[p]
            query += str;
        }
        url += "?" + query.substring(1);
        return url+'&token=00001iloveyouruo';
    }
    `;
    // console.log(apiStr)
    fs.writeFileSync(`../api/${filePrefix}API.js`, apiStr);
}

// fs.writeFileSync('./api.txt', JSON.stringify(apiObj));