const isWhitespace = input => input.match(/^\s*$/) !== null

const isEmpty = input => !input || isWhitespace(input)

const isRichtextEmpty = input => {
   const result = /<p.*?>(?<inner>.*?)<\/p>/.exec(input)
   return result ? isEmpty(result.groups.inner) : isEmpty(input)
}

const getDuplicate = config => {
   for (const item of config.values) {
      return config.values.find(isDuplicate(item, config))
   }
}

const isDuplicate = (item1, config) => item2 =>
   propsNotEmpty(item1, item2, config) &&
   compareProps(item1, item2, config) &&
   item1.id !== item2.id

const propsNotEmpty = (item1, item2, config) =>
   Array.isArray(config.prop)
      ? config.prop.every(p => !isEmpty(item1[p]) && !isEmpty(item2[p]))
      : !isEmpty(item1[config.prop]) && !isEmpty(item2[config.prop])

const compareProps = (item1, item2, config) =>
   Array.isArray(config.prop)
      ? config.prop.every(p => item1[p] === item2[p])
      : item1[config.prop] === item2[config.prop]

const hasOverlap = (obj1, obj2) => {
   const startA = new Date(obj1.startDate)
   const endA = new Date(obj1.endDate)
   const startB = new Date(obj2.startDate)
   const endB = new Date(obj2.endDate)

   return startA < endB && endA > startB
}

const getOverlap = objs => {
   for (const item1 of objs) {
      for (const item2 of objs) {
         if (item1.id !== item2.id && hasOverlap(item1, item2)) {
            return [item2, item1]
         }
      }
   }
   return [undefined, undefined]
}

const isArrayEmpty = array => !array && array.length

export { isEmpty, isRichtextEmpty, isArrayEmpty, getDuplicate, hasOverlap, getOverlap }
