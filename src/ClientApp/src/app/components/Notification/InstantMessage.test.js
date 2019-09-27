import React from 'react'
import { act } from 'react-dom/test-utils'
import { mount } from 'enzyme'
import { InstantMessage } from './InstantMessage'
import * as notification from './subscription'

jest.useFakeTimers()

beforeEach(() => jest.clearAllMocks())

it('displays the message', () => {
   const msg = mount(<InstantMessage />)

   act(() => notification.notify('the message', 'error'))
   msg.update()
   expect(msg.text().includes('the message')).toBe(true)

   act(() => notification.notify('second message', 'success'))
   msg.update()
   expect(msg.text().includes('second message')).toBe(true)
})

it('hides the message after 3 seconds', () => {
   const msg = mount(<InstantMessage />)

   act(() => notification.notify('the message', 'error'))
   msg.update()

   act(() => jest.advanceTimersByTime(3000))
   msg.update()

   expect(msg.isEmptyRender()).toBe(true)
})

it('clears the timer when hiding', () => {
   const msg = mount(<InstantMessage />)

   act(() => notification.notify('the message', 'error'))
   act(() => jest.runAllTimers())
   msg.update()

   expect(clearTimeout).toHaveBeenCalledTimes(1)
})

it('unsubscribe when unmounting', () => {
   jest.spyOn(notification, 'unsubscribe')

   const msg = mount(<InstantMessage />)
   msg.unmount()

   expect(notification.unsubscribe).toHaveBeenCalledTimes(1)
})
