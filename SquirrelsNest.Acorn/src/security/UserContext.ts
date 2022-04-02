import React from 'react'
import { User, noUser } from './user';

const UserContext = React.createContext<{
    user: User;
    updateUser(user: User): void
}>({user: noUser, updateUser: () => {}})

export default UserContext