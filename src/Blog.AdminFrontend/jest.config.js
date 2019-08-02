module.exports = {
   transformIgnorePatterns: ['node_modules/(?!(@alidoustkani/richtext)/)'],
   testPathIgnorePatterns: ['empty-module\\.js$'],
   moduleNameMapper: {
      '\\.(svg|jpg|png)$': '<rootDir>/__tests__/__mocks__/empty-module.js'
   }
}
