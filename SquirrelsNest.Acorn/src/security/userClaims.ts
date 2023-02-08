import {claim} from './authenticationModels'

export function isLoggedIn( claims: claim[] ): boolean {
  return claims.length > 0
}

function findValue( claims: claim[], name: string ): claim | undefined {
  return claims.find( claim => claim.name === name )
}

export function hasRoleClaim( claims: claim[], role: string ): boolean {
  if( role === 'none' )
    return true

  const roleClaim = findValue( claims, 'role' )

  if( Array.isArray( roleClaim?.value ) ) {
    let roles = roleClaim?.value as string[]

    return roles.findIndex( r => r === role ) > -1
  }

  return roleClaim?.value === role
}

export function userEmail( claims: claim[] ): string {
  let emailClaim = findValue( claims, 'email' )

  return emailClaim == null ? '' : emailClaim.value
}

export function userName( claims: claim[] ): string {
  let name = findValue( claims, 'name' )

  return name == null ? '' : name.value
}
