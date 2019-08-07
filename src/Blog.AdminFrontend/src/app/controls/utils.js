const ask = fn => id => {
   if (confirm('Are you sure?')) {
      fn(id)
   }
}

const emitChange = props => args => {
   props.onChange({
      id: props.id,
      [props.name]: args.target.value
   })
}

const renderClassNames = props => {
   const errors = props[props.name + 'Errors']
   const className = props.className
   if (errors && errors.length) {
      return className ? className + ' incorrect' : 'incorrect'
   }
   return className
}

export { ask, emitChange, renderClassNames }
