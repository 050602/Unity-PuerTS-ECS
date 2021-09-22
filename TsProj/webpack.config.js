module.exports = {
    // other...
    /** 忽略编辑的第三方库 */
    externals: {
        csharp: "commonjs2 csharp",
        puerts: "commonjs2 puerts",
        path: "commonjs2 path",
        fs: "commonjs2 fs",
        // global:"commonjs2 global",
        // "protobuf":"commonjs2 protobuf"
    },

    devtool: 'source-map',
    entry: __dirname + "/Script/view/TSMain.ts",
    output: {
        path: __dirname + "/output",
        filename: "main.js"
    },
    mode: "development",
    module: {
        rules: [{
            test: /\.tsx?$/,
            use: 'awesome-typescript-loader',
            exclude: /node_modules/
        }]
    },
    resolve: {   // 需要打包的文件后缀
        extensions: [".tsx", ".ts", ".js"]
    }

}
