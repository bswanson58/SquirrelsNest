import { claim } from './authenticationModels'

export class User {
  mClaims: claim[]

  constructor(claims: claim[]) {
    this.mClaims = claims
  }

  isLoggedIn(): boolean {
      return this.mClaims.length > 0
  }

  findValue( name: string ) : claim | undefined {
      return this.mClaims.find( claim => claim.name === name )
  }

  hasRoleClaim(role: string): boolean {
    if( role === 'none')
      return true

    var roleClaim = this.findValue('role')

    if( Array.isArray( roleClaim?.value )) {
        let roles = roleClaim?.value as string[]

        return roles.findIndex( r => r === role ) > -1
    }

    return roleClaim?.value === role
  }

  emailAddress(): string {
    let emailClaim = this.findValue( 'email' );

    return emailClaim == null ? '' : emailClaim.value
  }

  name(): string {
    let name = this.findValue( 'name' );

    return name == null ? '' : name.value
  }
}

let noUser = new User([])
let adminUser = new User([
  { name: 'name', value: 'Bill Swanson' },
  { name: 'email', value: 'bswanson58@gmail.com' },
  { name: 'role', value: 'admin' },
])
let normalUser = new User([
  { name: 'name', value: 'Bill Swanson' },
  { name: 'email', value: 'bswanson58@gmail.com' },
  { name: 'role', value: 'user' },
])

export { noUser, adminUser, normalUser }
