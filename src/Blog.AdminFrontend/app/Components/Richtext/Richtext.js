import React, { useState, useRef, useMemo, useEffect } from 'react'
import PropTypes from 'prop-types'
import Button from './Button'
import { create } from '@alidoustkani/richtext'
import { ErrorList } from '../ErrorList'

const OPTIONS = {
   defaultLink: '/',
   staySelected: false,
   decors: {
      important: 'strong',
      inlineCode: { tag: 'span', className: 'inline-code' },
      h1: { parent: true, tag: 'h3' },
      h2: { parent: true, tag: 'h4' },
      code: { parent: true, tag: 'pre', className: 'code' },
      terminal: { parent: true, tag: 'pre', className: 'terminal' },
      note: { parent: true, tag: 'div', className: 'note' },
      warning: { parent: true, tag: 'div', className: 'warning' }
   }
}

function Richtext(props) {
   const editorRef = useRef()
   const [rich, setRich] = useState(null)
   const [hasFocus, setFocus] = useState(false)

   useEffect(() => {
      setRich(create(editorRef.current, OPTIONS))
      if (props.autofocus) {
         editorRef.current.firstChild.focus()
      }
   }, [])

   const article = useMemo(
      () => (
         <article
            data-testid={props['data-testid']}
            ref={editorRef}
            className="entry"
            dangerouslySetInnerHTML={{ __html: props.innerHtml }}
            onBlur={() =>
               props.onChange({
                  target: {
                     name: props.name,
                     value: editorRef.current.innerHTML
                  }
               })
            }
         />
      ),
      []
   )

   if (rich) {
      //rich.disabled(props.disabled)
   }

   const classes = ['entry']
   if (props.errors && props.errors.length) {
      classes.push('incorrect')
   }
   if (hasFocus) {
      classes.push('hasFocus')
   }

   return (
      <div className="text-group richtext-group">
         {props.label ? <label>{props.label}</label> : null}
         <div className="toolbar">
            <Button
               disabled={props.disabled}
               content="bold"
               onClick={() => rich.style('important')}
            />
            <Button
               disabled={props.disabled}
               content="code"
               onClick={() => rich.style('inlineCode')}
            />
            <div className="separator" />
            <Button
               disabled={props.disabled}
               content={<strong>H1</strong>}
               onClick={() => rich.apply('h1')}
            />
            <Button
               disabled={props.disabled}
               content={<strong>H2</strong>}
               onClick={() => rich.apply('h2')}
            />
            <div className="separator" />
            <Button disabled={props.disabled} content="code" onClick={() => rich.apply('code')} />
            <Button
               disabled={props.disabled}
               content="terminal"
               onClick={() => rich.apply('terminal')}
            />
            <div className="separator" />
            <Button
               disabled={props.disabled}
               content="exclamation"
               onClick={() => rich.apply('note')}
            />
            <Button
               disabled={props.disabled}
               content="times"
               onClick={() => rich.apply('warning')}
            />
            <div className="separator" />
            <Button
               disabled={props.disabled}
               content="list-ul"
               onClick={() => rich.applyUnorderedList()}
            />
            <Button
               disabled={props.disabled}
               content="list-ol"
               onClick={() => rich.applyOrderedList()}
            />
            <div className="separator" />
            <Button disabled={props.disabled} content="camera" onClick={() => rich.selectImage()} />
            <Button disabled={props.disabled} content="link" onClick={() => rich.styleLink()} />
         </div>
         <div
            className={classes.join(' ')}
            onFocus={() => setFocus(true)}
            onBlur={() => setFocus(false)}
         >
            {article}
         </div>
         <ErrorList errors={props.errors} />
      </div>
   )
}

Richtext.propTypes = {
   autofocus: PropTypes.bool,
   innerHtml: PropTypes.string.isRequired,
   onChange: PropTypes.func,
   name: PropTypes.string,
   errors: PropTypes.arrayOf(PropTypes.string),
   label: PropTypes.string,
   disabled: PropTypes.bool
}

export { Richtext }
