const fetchMock = (() => {
   const obj = {}
   let code,
      data,
      dataExcMsg,
      fetchExcMsg,
      headers = []

   function makeMock() {
      const json = jest.fn()
      if (dataExcMsg) {
         json.mockImplementation(() => {
            throw { message: dataExcMsg }
         })
      } else {
         json.mockResolvedValue(data)
      }
      global.fetch = jest.fn()
      global.fetch.mockImplementation((url, options) => {
         obj.body = options.body
         if (fetchExcMsg) {
            throw { message: fetchExcMsg }
         }
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
      dataExcMsg = undefined
      fetchExcMsg = undefined
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
      dataExcMsg = val
      makeMock()
      return obj
   }

   obj.throw = msg => {
      fetchExcMsg = msg
      makeMock()
      return obj
   }

   return obj
})()

export default fetchMock
