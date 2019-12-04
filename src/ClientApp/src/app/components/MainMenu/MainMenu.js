import React from 'react'

function MenuItem({ text, page, onClick }) {
   return (
      <li className="menu__item">
         <a
            href="#"
            className="menu__link"
            onClick={() => {
               if (page) {
                  window.location.assign(page)
               } else {
                  onClick()
               }
            }}
         >
            {text}
         </a>
      </li>
   )
}

function MainMenu({ open, onOpenClick, onLogout }) {
   const style = open ? { display: 'inline-block' } : { display: 'none' }

   return (
      <nav className={open ? 'menu menu--open' : 'menu'}>
         <button
            className="menu__button"
            onClick={e => {
               e.preventDefault()
               onOpenClick(e)
            }}
         >
            <i className="fas fa-ellipsis-h"></i>
         </button>
         <ul className="menu__itemContainer" style={style}>
            <MenuItem text="Posts" page="/admin" />
            <MenuItem text="New Post" page="/admin/post" />
            <MenuItem text="Developer" page="/admin/developer" />
            <MenuItem text="Logout" onClick={onLogout} />
         </ul>
      </nav>
   )
}

export default MainMenu
