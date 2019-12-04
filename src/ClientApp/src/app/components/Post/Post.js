import React, { useReducer, useEffect } from 'react'
import { useParams } from 'react-router-dom'
import { Richtext, Loader } from 'Controls'
import { LanguageSelector } from './LanguageSelector'
import { getDraft, postDraft, patchDraft } from '../services'

const initial = {
   id: '',
   language: '',
   title: '',
   content: '',
   changed: false,
   showSaveMessage: false,
   loading: true
}

function reducer(state, action) {
   switch (action.type) {
      case 'TO_SAVE_FINISHED':
         return { ...state, changed: false, showSaveMessage: true }
      case 'TO_SAVE_INFORMED':
         return { ...state, showSaveMessage: false }
      case 'CHANGE_ID':
         return { ...state, id: action.id }
      case 'CHANGE_LANG':
         return { ...state, language: action.language }
      case 'CHANGE_TITLE':
         return { ...state, changed: true, title: action.title }
      case 'CHANGE_CONTENT':
         return { ...state, changed: true, content: action.content }
      case 'CHANGE_DRAFT':
         return { ...state, ...action.draft, loading: false }
      default:
         throw Error('Not valid action type')
   }
}

function Post({ auth0 }) {
   const [state, dispatch] = useReducer(reducer, initial)
   const directionProps =
      state.language === 'farsi'
         ? {
              placeholder: 'موضوع را اینجا وارد کنید',
              formClass: 'postForm postForm--rtl',
              messageClass: 'saveMessage saveMessage--rtl'
           }
         : {
              placeholder: 'Enter title here',
              formClass: 'postForm',
              messageClass: 'saveMessage'
           }
   const { id } = useParams()

   if (!state.id && id) {
      dispatch({ type: 'CHANGE_ID', id })
   }

   useEffect(() => {
      const load = async () => {
         if (state.loading) {
            const result = await getDraft(state.id, auth0)
            if (result.status === 'ok') {
               dispatch({ type: 'CHANGE_DRAFT', draft: result.data })
            }
         }
      }
      load()
   }, [state.id])

   useEffect(() => {
      const save = async () => {
         if (state.id) {
            const result = await patchDraft(
               {
                  id: state.id,
                  title: state.title,
                  content: state.content
               },
               auth0
            )
            if (result.status !== 'ok') {
               console.error('Saving Error', result)
            }
         } else {
            const result = await postDraft(
               {
                  language: state.language,
                  title: state.title,
                  content: state.content
               },
               auth0
            )
            if (result.status === 'ok') {
               dispatch({ type: 'CHANGE_ID', id: result.data.id })
            } else {
               console.error('Saving Error', result)
            }
         }
      }

      const timer = setInterval(() => {
         if (state.changed) {
            save()
            dispatch({ type: 'TO_SAVE_FINISHED' })
         }
      }, 2000)
      return () => clearInterval(timer)
   }, [state])

   useEffect(() => {
      if (state.showSaveMessage) {
         const timer = setTimeout(() => {
            dispatch({ type: 'TO_SAVE_INFORMED' })
         }, 2000)
         return () => clearInterval(timer)
      }
   }, [state.showSaveMessage])

   if (!state.id && !state.language) {
      return (
         <LanguageSelector onSelection={language => dispatch({ type: 'CHANGE_LANG', language })} />
      )
   }

   if (state.loading) {
      return <Loader text="Loading draft..." />
   }

   return (
      <div className={directionProps.formClass}>
         {state.showSaveMessage && (
            <div className={directionProps.messageClass}>Draft Automatically Saved</div>
         )}
         <input
            placeholder={directionProps.placeholder}
            autoFocus
            className="postForm__title"
            value={state.title}
            onChange={e => dispatch({ type: 'CHANGE_TITLE', title: e.target.value })}
         />
         <Richtext
            name="content"
            stickyToolbar={true}
            pro={true}
            className="postForm__richtext"
            content={state.content}
            onChange={e => dispatch({ type: 'CHANGE_CONTENT', content: e.content })}
         />
      </div>
   )
}

export default Post
