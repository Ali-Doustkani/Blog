const compose = (...functions) => args => functions.reduceRight((arg, fn) => fn(arg), args)

const dict = (...pairs) => key => {
   try {
      return pairs.find(pair => pair[0] === key)[1]
   } catch {
      throw new Error('Invalid key for pair')
   }
}

/**
 * changes the current state with steps.
 * a step could be a value (change) or a function that returns the change.
 */
const evolve = (...steps) => (currentState, action) => ({
   ...currentState,
   ...applyEffects(steps, currentState, action)
})

/**
 * changes state from an initial state.
 */
const evolveFrom = (init, ...steps) => (currentState, action) => ({
   ...init,
   ...applyEffects(steps, currentState, action)
})

/**
 * a property selector step.
 */
const take = selector => options => selector(options)

const ifElse = (predicate, onTrue, onFalse) => (state, action) =>
   predicate(action) ? onTrue(state, action) : onFalse(state, action)

function applyEffects(steps, state, action) {
   const effects = {}
   steps.forEach(step =>
      Object.assign(effects, typeof step === 'function' ? step({ state, action }) : step)
   )
   return effects
}

export { compose, dict, evolve, evolveFrom, take, ifElse }
