const toClientFormat = message => ({ type: 2, message })

const toContext = serverErrors => serverProp => ({
   path: serverProp + 'Errors',
   errors: serverErrors[serverProp].map(toClientFormat)
})

const splitPath = path => path.split(/\[|\]|\./).filter(x => x !== '')

const getProperty = state => path => path.reduce((prop, p) => prop[p], state)

const writeItem = state => item => {
   const path = splitPath(item.path)
   const array = getProperty(state)(path)
   item.errors.forEach(er => {
      if (!array) {
         throw 'error array does not exist in the state'
      }
      array.push(er)
   })
}

const writeErrors = (serverError, state) =>
   Object.getOwnPropertyNames(serverError)
      .map(toContext(serverError))
      .forEach(writeItem(state))

export { writeErrors }
