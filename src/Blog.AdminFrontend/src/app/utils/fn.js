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

export { compose, dict, ifElse }
