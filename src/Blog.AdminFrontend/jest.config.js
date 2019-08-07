module.exports = {
   setupFilesAfterEnv: ['<rootDir>__tests__/setupTests.js'],
   transformIgnorePatterns: ['node_modules/(?!(@alidoustkani/richtext)/)'],
   testPathIgnorePatterns: ['__mocks__', 'setupTest'],
   moduleNameMapper: {
      '\\.(svg|jpg|png)$': '<rootDir>/__tests__/__mocks__/empty-module.js'
   }
}
