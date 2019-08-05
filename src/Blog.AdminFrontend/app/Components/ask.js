function ask(fn) {
   return function(id) {
      if (confirm('Are you sure?')) {
         fn(id)
      }
   }
}

export { ask }
