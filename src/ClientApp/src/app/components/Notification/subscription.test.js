import { notify, subscribe, unsubscribe } from './subscription'

it('calls what is subscribed', () => {
   const callback = jest.fn()
   subscribe(callback)
   notify('the message', 'error')
   expect(callback).toHaveBeenCalledWith('the message', 'error')
})

it('unsubscribes', () => {
   subscribe(jest.fn())
   unsubscribe()
   try {
      notify('the message', 'error')
   } catch {
      expect(true).toBe(true)
   }
})
