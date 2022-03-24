import React, { PropsWithChildren } from 'react'
import Footer from './Footer'
import Header from './Header'
import appRoutes from '../config/appRoutes'

type Props = {}

function Layout(props: PropsWithChildren<Props>) {
  return (
      <>
        <Header menuItems={appRoutes} />
        {props.children}
        <Footer/>
      </>
  )
}

export default Layout