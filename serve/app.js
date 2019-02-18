const Koa = require('koa');
const bodyParser = require('koa-bodyparser')
const app = new Koa()
app.use(bodyParser());

app.use(async (ctx, next) => {
    ctx.body = 'success';

})
app.listen(8080);