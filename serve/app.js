const Koa = require('koa');
const bodyParser = require('koa-bodyparser')
const exec = require('child_process').exec;

const app = new Koa()
app.use(bodyParser());


app.use(async (ctx, next) => {
    console.log(ctx.request);

    try {
        let res = await mkfile();
        ctx.body = res;

    } catch (error) {
        ctx.body = error;
    }


})
app.listen(8082);

function mkfile() {
    return new Promise((resolve, reject) => {
        const commands = 'sh ./update.sh';
        exec(commands, (err, out, code) => {
            if (err) {
                reject(err);
            }
            resolve('success');
        })
    })
}