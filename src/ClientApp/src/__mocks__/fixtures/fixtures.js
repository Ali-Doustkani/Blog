import fetchMock from 'fetch-mock'
import drafts from './drafts'

fetchMock.get(
   '/api/drafts',
   drafts.map(x => ({ id: x.id, title: x.title, published: x.published }))
)

fetchMock.get(/\/api\/drafts\/\d/, url => {
   const id = url.match(/\d+/g)[0]
   return drafts.find(x => x.id == id)
})

fetchMock.post('/api/drafts', () => ({
   id: 4
}))

fetchMock.patch(/\/api\/drafts\/\d/, 200)
