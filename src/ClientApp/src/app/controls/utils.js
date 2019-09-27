const ask = fn => id => {
   if (confirm('Are you sure?')) {
      fn(id)
   }
}

const emitChange = props => args => {
   const changes = props.id ? { id: props.id } : {}
   changes[props.name] = args.target.value
   props.onChange(changes)
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
