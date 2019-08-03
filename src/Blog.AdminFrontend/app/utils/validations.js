const emptyString = input => input.match(/^\s*$/) !== null

const isEmpty = input => (!input ? true : emptyString(input))

const isRichtextEmtpy = input => {
   const regex = /<p.*?>(?<inner>.*?)<\/p>/
   const result = regex.exec(input)
   if (!result) {
      return isEmpty(input)
   }
   return isEmpty(result.groups.inner)
}

export { isEmpty, isRichtextEmtpy }
