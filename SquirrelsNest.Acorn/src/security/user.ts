import { claim } from './authenticationModels'

export class User {
  mClaims: claim[]

  constructor(claims: claim[]) {
    this.mClaims = claims
  }

  isLoggedIn(): boolean {
      return this.mClaims.length > 0
  }

  hasRoleClaim(role: string): boolean {
    return (
      role === 'none' ||
      this.mClaims.findIndex((claim) => claim.name === 'role' && claim.value === role ) > -1
    )
  }

  findValue( name: string ) : claim | undefined {
      return this.mClaims.find( claim => claim.name === name )
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
export { noUser }
