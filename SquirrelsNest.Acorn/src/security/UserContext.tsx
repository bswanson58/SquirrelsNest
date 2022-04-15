import {ClientContext} from 'graphql-hooks'
import {createContext, useContext, useEffect, useState} from 'react'
import {User, noUser} from './user'
import {getAuthenticationClaims, getAuthenticationToken} from './jwtSupport'

const UserContext = createContext<{
  user: User
  updateUser( user: User ): void
}>( { user: noUser, updateUser: () => {} } )

function UserContextProvider( props: any ) {
  const [user, setUser] = useState<User>( noUser )
  const clientContext = useContext( ClientContext )

  useEffect( () => {
    setUser( new User( getAuthenticationClaims()))
    clientContext.setHeader( 'Authorization', `Bearer ${getAuthenticationToken()}` )
  }, [] )

  return (
    <UserContext.Provider value={{ user, updateUser: setUser }}>
      {props.children}
    </UserContext.Provider>
  )
}

const useUserContext = () => useContext( UserContext )

export {UserContextProvider, useUserContext}
