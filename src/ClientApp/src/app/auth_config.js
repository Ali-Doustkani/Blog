const config =
   process.env.NODE_ENV === 'production'
      ? {
           info: {
              domain: 'ali-doustkani.auth0.com',
              client_id: 'wZmKb30P6lnFD4Olh2ryazHqBwX8KU8L',
              redirect_uri: 'https://alidoustkani.com/newadmin',
              audience: 'https://alidoustkani.com/api'
           },
           logout: {
              returnTo: 'https://alidoustkani.com'
           }
        }
      : {
           info: {
              domain: 'ali-doustkani.auth0.com',
              client_id: 'wZmKb30P6lnFD4Olh2ryazHqBwX8KU8L',
              redirect_uri: 'http://localhost:5000/newadmin',
              audience: 'http://localhost:5000/api/developer'
           },
           logout: {
              returnTo: 'http://localhost:5000'
           }
        }

export default config
