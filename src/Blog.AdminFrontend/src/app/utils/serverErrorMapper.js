const makeMessage = error => {
   const propName = error.path[error.path.length - 1]
   switch (error.error) {
      case 'isRequired':
         return `${propName} is required`
      case 'isEmpty':
         return `${propName} can not be empty`
      default:
         return `something is wrong with ${propName}`
   }
}

const read = error => ({ type: 2, message: makeMessage(error) })

const mapPath = (error, obj) =>
   error.path.reduce(
      (parent, prop, index) =>
         error.path.length - 1 === index ? parent[prop + 'Errors'] : parent[prop],
      obj
   )

const map = (errors, obj) =>
   errors.forEach(error => {
      mapPath(error, obj).push(read(error))
   })

export { map }
