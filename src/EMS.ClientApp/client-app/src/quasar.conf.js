module.exports = function (ctx) {
    return {
        supportTS: true,
        framework: {
            plugins: ['Notify'],
            config: {
                notify: {
                    group: true,
                    timeout: 10000,
                    closeBtn: "Close",
                    multiLine: true,
                    progress: true,
                    classes: "user-notification",
                    progressClass: "user-notification-progress",
                },
            },
        },
        parserOptions: {
            parser: '@babel/eslint-parser'
        },
        extends: [
            'plugin:vue/vue3-essential'
        ]
    }
}