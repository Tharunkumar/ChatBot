"use strict";
const webpack = require('webpack');

module.exports = {
    entry: "./Scripts/AppJs/ChatBot.jsx",
    output: {
        filename: "./Scripts/bundle.js"
    },
    module: {
        loaders: [
            {
                test: /\.jsx?$/,
                loader: "babel-loader",
                exclude: /node_modules/,
                query: {
                  presets: ["es2015", "react"]
                }
            }
        ]
    },
    resolve: {
      extensions: ['', '.js', '.jsx'],
    },
    plugins: [
          new webpack.optimize.UglifyJsPlugin({
            compress: {
              warnings: false
            }
          })
    ]
};