import { compose } from './fn'

const isWhitespace = input => input.match(/^\s*$/) !== null

const isEmpty = input => !input || isWhitespace(input)

const isRichtextEmpty = input => {
   const result = /<p.*?>(?<inner>.*?)<\/p>/.exec(input)
   return result ? isEmpty(result.groups.inner) : isEmpty(input)
}

const message = msg => result => (result ? msg : undefined)

const requiredMessage = field => message(`${field} is required`)

const errorListGenerator = errors => message => {
   const result = !errors || !errors.length ? [] : errors.filter(x => x.type === 2)
   if (message) {
      result.push({ type: 1, message })
   }
   return result
}

const emptyValidator = (field, errors) =>
   compose(
      errorListGenerator(errors),
      requiredMessage(field),
      isEmpty
   )

const richtextEmptyValidator = (field, errors) =>
   compose(
      errorListGenerator(errors),
      requiredMessage(field),
      isRichtextEmpty
   )

export { emptyValidator, richtextEmptyValidator }
