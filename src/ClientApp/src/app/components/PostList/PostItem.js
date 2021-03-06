import React, { useState } from 'react'

function PostItem({ title, date, published }) {
   const [showButtons, setShowButtons] = useState(false)
   return (
      <li
         style={{ direction: 'rtl' }}
         onMouseOver={() => setShowButtons(true)}
         onMouseOut={() => setShowButtons(false)}
      >
         <div className={'button-container' + (showButtons ? ' visible' : ' hidden')}>
            <button>Delete</button>
            <button>Edit</button>
            {published || <button data-testid="publish-button">Publish</button>}
            {published || <button data-testid="preview-button">Preview</button>}
            {published && <button data-testid="view-button">View</button>}
         </div>
         <div className="content-container">
            <div>{title}</div>
            <span className="date">{date}</span>
            {published || <span className="tag">Not Published</span>}
         </div>
      </li>
   )
}

export default PostItem
