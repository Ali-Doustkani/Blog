module.exports = api => {
   api.cache(true)
   const presets = ['@babel/preset-react', '@babel/preset-env']
   return { presets }
}
