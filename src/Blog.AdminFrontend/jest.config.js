module.exports = {
   setupFilesAfterEnv: ['<rootDir>src/__tests__/setupTests.js'],
   transformIgnorePatterns: ['node_modules/(?!(@alidoustkani/richtext)/)'],
   testPathIgnorePatterns: ['__mocks__', 'setupTest'],
   moduleNameMapper: {
      '\\.(svg|jpg|png)$': '<rootDir>src/__tests__/__mocks__/empty-module.js',
      DisabledContext: '<rootDir>src/app/components/DisabledContext'
   }
}
