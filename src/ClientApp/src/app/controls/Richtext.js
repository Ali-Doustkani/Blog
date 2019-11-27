import React, { useState, useRef, useMemo, useEffect } from 'react'
import PropTypes from 'prop-types'
import { ToolbarButton } from './ToolbarButton'
import { create } from '@alidoustkani/richtext'
import { ErrorList } from './ErrorList'
import DisabledContext from 'DisabledContext'

const OPTIONS = {
   defaultLink: '/',
   staySelected: false,
   decors: {
      important: 'strong',
      inlineCode: { tag: 'span', className: 'inline-code' },
      h1: { parent: true, tag: 'h1' }
   }
}

function Richtext(props) {
   const editorRef = useRef()
   const toolbarRef = useRef()
   const [rich, setRich] = useState(null)
   const [hasFocus, setFocus] = useState(false)
   const [hasStickyToolbar, setHasStickyToolbar] = useState(false)
   const [toolbarOffset, setToolbarOffset] = useState()
   const disabled = React.useContext(DisabledContext)
   const setStickyToolbar = () => setHasStickyToolbar(window.pageYOffset > toolbarOffset)

   useEffect(() => {
      setRich(create(editorRef.current, OPTIONS))
      if (props.autoFocus) {
         editorRef.current.firstChild.focus()
      }
   }, [])

   useEffect(() => {
      if (props.stickyToolbar) {
         setToolbarOffset(toolbarRef.current.offsetTop - 20)
         window.addEventListener('scroll', setStickyToolbar)
         return () => window.removeEventListener('scroll', setStickyToolbar)
      }
   }, [toolbarOffset])

   const article = useMemo(
      () => (
         <article
            ref={editorRef}
            className="entry"
            data-testid={props.name + '-richtext'}
            dangerouslySetInnerHTML={{ __html: props[props.name] }}
            onBlur={() =>
               props.onChange({
                  id: props.id,
                  [props.name]: editorRef.current.innerHTML
               })
            }
         />
      ),
      []
   )

   if (rich) {
      rich.setOptions({ disabled })
   }

   const errors = props[props.name + 'Errors']
   const classes = ['richtext__entry']
   if (errors && errors.length) {
      classes.push('incorrect')
   }
   if (hasFocus) {
      classes.push('hasFocus')
   }

   return (
      <div className={'richtext ' + props.className}>
         {props.label ? <label>{props.label}</label> : null}
         <div
            ref={toolbarRef}
            className={
               hasStickyToolbar
                  ? 'richtext__toolbar richtext__toolbar--sticky'
                  : 'richtext__toolbar'
            }
         >
            <ToolbarButton content="bold" tabIndex="-1" onClick={() => rich.style('important')} />
            <ToolbarButton content="code" tabIndex="-1" onClick={() => rich.style('inlineCode')} />
            <div className="richtext__separator" />
            <ToolbarButton tabIndex="-1" onClick={() => rich.apply('h1')}>
               <strong>H1</strong>
            </ToolbarButton>
            <ToolbarButton tabIndex="-1" onClick={() => rich.apply('h2')}>
               <strong>H2</strong>
            </ToolbarButton>
            <div className="richtext__separator" />
            {props.pro ? (
               <>
                  <ToolbarButton content="code" tabIndex="-1" onClick={() => rich.apply('code')} />
                  <ToolbarButton
                     content="terminal"
                     tabIndex="-1"
                     onClick={() => rich.apply('terminal')}
                  />
                  <div className="richtext__separator" />
                  <ToolbarButton
                     content="exclamation"
                     tabIndex="-1"
                     onClick={() => rich.apply('note')}
                  />
                  <ToolbarButton
                     content="times"
                     tabIndex="-1"
                     onClick={() => rich.apply('warning')}
                  />
                  <div className="richtext__separator" />
               </>
            ) : null}
            <ToolbarButton
               content="list-ul"
               tabIndex="-1"
               onClick={() => rich.applyUnorderedList()}
            />
            <ToolbarButton
               content="list-ol"
               tabIndex="-1"
               onClick={() => rich.applyOrderedList()}
            />
            <div className="richtext__separator" />
            <ToolbarButton content="link" tabIndex="-1" onClick={() => rich.styleLink()} />
            {props.pro ? (
               <ToolbarButton content="camera" tabIndex="-1" onClick={() => rich.selectImage()} />
            ) : null}
         </div>
         <div
            className={classes.join(' ')}
            onFocus={() => setFocus(true)}
            onBlur={() => setFocus(false)}
         >
            {article}
         </div>
         {props.showErrors ? <ErrorList errors={errors} /> : null}
      </div>
   )
}

Richtext.propTypes = {
   id: PropTypes.oneOfType([PropTypes.number, PropTypes.string]),
   name: PropTypes.string.isRequired,
   autoFocus: PropTypes.bool,
   onChange: PropTypes.func,
   label: PropTypes.string,
   showErrors: PropTypes.bool
}

Richtext.defaultProps = {
   showErrors: true
}

export { Richtext }
