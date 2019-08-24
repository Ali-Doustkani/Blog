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
   const [rich, setRich] = useState(null)
   const [hasFocus, setFocus] = useState(false)
   const disabled = React.useContext(DisabledContext)

   useEffect(() => {
      setRich(create(editorRef.current, OPTIONS))
      if (props.autoFocus) {
         editorRef.current.firstChild.focus()
      }
   }, [])

   const article = useMemo(
      () => (
         <article
            ref={editorRef}
            className="entry"
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
   const classes = ['entry']
   if (errors && errors.length) {
      classes.push('incorrect')
   }
   if (hasFocus) {
      classes.push('hasFocus')
   }

   return (
      <div className="text-group richtext-group">
         {props.label ? <label>{props.label}</label> : null}
         <div className="toolbar">
            <ToolbarButton content="bold" onClick={() => rich.style('important')} />
            <ToolbarButton content="code" onClick={() => rich.style('inlineCode')} />
            <div className="separator" />
            <ToolbarButton onClick={() => rich.apply('h1')}>
               <strong>H1</strong>
            </ToolbarButton>
            <ToolbarButton onClick={() => rich.apply('h2')}>
               <strong>H2</strong>
            </ToolbarButton>
            <div className="separator" />
            <ToolbarButton content="list-ul" onClick={() => rich.applyUnorderedList()} />
            <ToolbarButton content="list-ol" onClick={() => rich.applyOrderedList()} />
            <div className="separator" />
            <ToolbarButton content="link" onClick={() => rich.styleLink()} />
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
