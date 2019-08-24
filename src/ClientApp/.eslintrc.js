module.exports = {
   env: {
      browser: true,
      es6: true,
      node: true
   },
   extends: [
      'eslint:recommended',
      'plugin:react/recommended',
      'plugin:jest/recommended',
      'prettier'
   ],
   parser: 'babel-eslint',
   rules: {
      indent: ['off', 3]
   },
   settings: {
      react: {
         version: 'detect'
      }
   }
}
