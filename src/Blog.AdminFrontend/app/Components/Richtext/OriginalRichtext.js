import React from 'react'

class OriginalRichtextWithRef extends React.Component {
   constructor(props) {
      super(props)
   }
   shouldComponentUpdate() {
      return false
   }
   render() {
      return (
         <article
            data-testid={this.props['data-testid']}
            ref={this.props.innerRef}
            className="entry"
            dangerouslySetInnerHTML={{ __html: this.props.innerHtml }}
            onBlur={() =>
               this.props.onChange({
                  target: {
                     name: this.props.name,
                     value: this.props.innerRef.current.innerHTML
                  }
               })
            }
         />
      )
   }
}

const OriginalRichtext = React.forwardRef((props, ref) => {
   return <OriginalRichtextWithRef innerRef={ref} {...props} />
})

export default OriginalRichtext
