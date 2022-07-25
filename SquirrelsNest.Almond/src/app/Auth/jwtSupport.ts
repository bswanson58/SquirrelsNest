import {authenticationResponse, claim} from './auth.models'

const tokenKey = 'token'
const expirationKey = 'token-expiration'

export function saveAuthenticationToken( authData: authenticationResponse ) {
  localStorage.setItem( tokenKey, authData.token )
  localStorage.setItem( expirationKey, authData.expiration.toString() )
}

export function getAuthenticationToken() {
  return localStorage.getItem( tokenKey )
}

export function getAuthenticationExpiration() {
  return localStorage.getItem( expirationKey )
}

export function getAuthenticationClaims(): claim[] {
  const token = localStorage.getItem( tokenKey )

  if( !token ) {
    return []
  }

  const expiration = localStorage.getItem( expirationKey )!
  const expirationDate = new Date( expiration )

  if( expirationDate <= new Date() ) {
    logout()
    return [] // the token has expired
  }

  const dataToken = JSON.parse( atob( token.split( '.' )[1] ) )
  const response: claim[] = []
  for( const property in dataToken ) {
    response.push( { name: property, value: dataToken[property] } )
  }

  return response
}

/*
export function hasRoleClaim(role: string, claims: claim[]): boolean {
  return (
    role === 'none' ||
    claims.findIndex((claim) => claim.name === 'role' && claim.value === role) > -1 )
}
*/
export function logout() {
  localStorage.removeItem( tokenKey )
  localStorage.removeItem( expirationKey )
}

/*
export function getAuthenticationToken() {
  return localStorage.getItem(tokenKey)
}
*/
