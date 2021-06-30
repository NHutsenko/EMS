module.exports = function (ctx) {
    return {
        supportTS: true,
        parserOptions: {
            parser: '@babel/eslint-parser'
        },
        extends: [
            'plugin:vue/vue3-essential'
        ]
    }
}