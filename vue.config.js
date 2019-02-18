const CompressionWebpackPlugin = require('compression-webpack-plugin')
const productionGzipExtensions = ['js', 'css']
module.exports = {
    lintOnSave: false,
    runtimeCompiler: true,
    productionSourceMap: false,
    // productionGzip: true,
    // productionGzipExtensions: ['js', 'css'],
    configureWebpack: {
        plugins: [
            new CompressionWebpackPlugin({
                // asset: '[path].gz[query]',
                algorithm: 'gzip',
                test: new RegExp('\\.(' + productionGzipExtensions.join('|') + ')$'),
                threshold: 10240,
                minRatio: 0.8
            })
        ]
    }
}