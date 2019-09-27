module.exports = {
   setupFilesAfterEnv: ['<rootDir>setupTests.js'],
   transformIgnorePatterns: ['node_modules/(?!(@alidoustkani/richtext)/)'],
   testPathIgnorePatterns: ['__mocks__', 'setupTest', './tests'],
   moduleNameMapper: {
      '\\.(svg|jpg|png)$': '<rootDir>src/__mocks__/empty-module.js',
      DisabledContext: '<rootDir>src/app/components/DisabledContext',
      Controls: '<rootDir>src/app/controls',
      fetchMock: '<rootDir>src/__mocks__/fetchMock'
   }
}
