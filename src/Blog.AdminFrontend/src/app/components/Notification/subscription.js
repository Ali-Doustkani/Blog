let showFunc
const subscribe = fn => (showFunc = fn)
const unsubscribe = () => (showFunc = undefined)
const notify = (message, appearance) => showFunc(message, appearance)

export { notify, subscribe, unsubscribe }
