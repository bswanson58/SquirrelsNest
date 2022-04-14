export interface claim {
  name: string
  value: string
}

export interface userCredentials {
  name: string
  email: string
  password: string
}

export interface authenticationResponse {
  token: string
  expiration: Date
}
