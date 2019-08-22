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

const update = change => obj => (change.id === obj.id ? { ...obj, ...change } : obj)

export { compose, dict, ifElse, update }
