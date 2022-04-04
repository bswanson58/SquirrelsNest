import { createContext, useContext, useEffect, useState } from 'react'
import { User, noUser, adminUser, normalUser } from '../security/user'
import { getAuthenticationClaims } from '../security/jwtSupport'

const UserContext = createContext<{
  user: User
  updateUser(user: User): void
}>({ user: noUser, updateUser: () => {} })

function UserContextProvider(props: any) {
  const [user, setUser] = useState<User>(noUser)

  useEffect(() => {
    setUser(new User(getAuthenticationClaims()))
  }, [])

  return (
    <UserContext.Provider value={{ user, updateUser: setUser }}>
      {props.children}
    </UserContext.Provider>
  )
}

const useUserContext = () => useContext(UserContext)

export { UserContextProvider, useUserContext }