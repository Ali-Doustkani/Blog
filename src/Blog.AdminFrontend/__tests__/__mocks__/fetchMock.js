const fetchMock = (() => {
   const obj = {}
   let code,
      data,
      exceptionMessage,
      headers = []

   function makeMock() {
      const json = jest.fn()
      if (exceptionMessage) {
         json.mockImplementation(() => {
            throw { message: exceptionMessage }
         })
      } else {
         json.mockResolvedValue(data)
      }
      global.fetch = jest.fn()
      global.fetch.mockImplementation((url, options) => {
         obj.body = options.body
         return {
            status: code,
            json,
            text: json
         }
      })
   }

   obj.reset = () => {
      code = undefined
      data = undefined
      exceptionMessage = undefined
      headers = []
   }

   obj.status = val => {
      code = val
      makeMock()
      return obj
   }

   obj.header = (name, value) => {
      headers.push({ name, value })
      makeMock()
      return obj
   }

   obj.data = val => {
      data = val
      makeMock()
      return obj
   }

   obj.throwOnData = val => {
      exceptionMessage = val
      makeMock()
      return obj
   }

   return obj
})()

export default fetchMock
