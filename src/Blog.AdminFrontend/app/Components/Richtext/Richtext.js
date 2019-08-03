import React from 'react'
import Button from './Button'
import { create } from '@alidoustkani/richtext'
import InnerRichtext from './OriginalRichtext'

class Richtext extends React.Component {
   constructor(props) {
      super(props)
      this.editorRef = React.createRef()
      this.change = this.change.bind(this)
      this.state = { hasFocus: false }
   }
   componentDidMount() {
      this.rich = create(this.editorRef.current, {
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
      })
   }
   change(e) {
      if (this.props.onChange) {
         this.props.onChange(e)
      }
   }
   render() {
      var classes = ['entry']
      if (this.props.error) {
         classes.push('incorrect')
      }
      if (this.state.hasFocus) {
         classes.push('hasFocus')
      }
      return (
         <div className="text-group richtext-group">
            <div className="toolbar">
               <Button content="bold" onClick={() => this.rich.style('important')} />
               <Button content="code" onClick={() => this.rich.style('inlineCode')} />
               <div className="separator" />
               <Button content={<strong>H1</strong>} onClick={() => this.rich.apply('h1')} />
               <Button content={<strong>H2</strong>} onClick={() => this.rich.apply('h2')} />
               <div className="separator" />
               <Button content="code" onClick={() => this.rich.apply('code')} />
               <Button content="terminal" onClick={() => this.rich.apply('terminal')} />
               <div className="separator" />
               <Button content="exclamation" onClick={() => this.rich.apply('note')} />
               <Button content="times" onClick={() => this.rich.apply('warning')} />
               <div className="separator" />
               <Button content="list-ul" onClick={() => this.rich.applyUnorderedList()} />
               <Button content="list-ol" onClick={() => this.rich.applyOrderedList()} />
               <div className="separator" />
               <Button content="camera" onClick={() => this.rich.selectImage()} />
               <Button content="link" onClick={() => this.rich.styleLink()} />
            </div>
            <div
               className={classes.join(' ')}
               onFocus={() => this.setState({ hasFocus: true })}
               onBlur={() => this.setState({ hasFocus: false })}
            >
               <InnerRichtext ref={this.editorRef} {...this.props} />
            </div>
         </div>
      )
   }
}

export { Richtext }
