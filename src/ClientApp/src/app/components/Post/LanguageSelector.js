import React from 'react'

function LanguageSelector({ onSelection }) {
   return (
      <div className="languageSelector">
         Please select your Language:
         <button className="languageSelector__button" onClick={() => onSelection('farsi')}>
            Farsi
         </button>
         <button className="languageSelector__button" onClick={() => onSelection('english')}>
            English
         </button>
      </div>
   )
}

export { LanguageSelector }
