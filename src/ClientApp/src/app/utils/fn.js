Array.prototype.scalar = function() {
   return this.length === 1 ? this[0] : this
}

const compose = (...functions) => args => functions.reduceRight((arg, fn) => fn(arg), args)

const dict = (...pairs) => key => {
   try {
      return pairs.find(pair => pair[0] === key)[1]
   } catch {
      throw new Error('Invalid key for pair')
   }
}

const ifElse = (predicate, onTrue, onFalse) => (state, action) =>
   predicate(action) ? onTrue(state, action) : onFalse(state, action)

const addItem = (prop, getNew) => state => ({ ...state, [prop]: [...state[prop], getNew()] })

const updateItem = prop => (state, action) => ({
   ...state,
   [prop]: state[prop].map(obj =>
      action.change.id === obj.id ? { ...obj, ...action.change } : obj
   )
})

const deleteItem = prop => (state, action) => ({
   ...state,
   [prop]: state[prop].filter(x => x.id !== action.id)
})

function today() {
   const now = new Date()
   return `${now.getFullYear()}-${String(now.getMonth() + 1).padStart(2, 0)}-${String(
      now.getDate()
   ).padStart(2, 0)}`
}

export { compose, dict, ifElse, addItem, updateItem, deleteItem, today }
