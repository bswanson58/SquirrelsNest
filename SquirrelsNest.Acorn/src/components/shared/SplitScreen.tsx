import React, { PropsWithChildren } from 'react'
import { Container, Pane } from './Container'

type Props = {
  leftWeight?: number
  rightWeight?: number
}

function SplitScreen(props: PropsWithChildren<Props>) {
  const [left, right] = props.children as Array<React.ReactNode>

  return (
    <Container>
      <Pane weight={props.leftWeight || 1}>{left}</Pane>
      <Pane weight={props.rightWeight || 1}>{right}</Pane>
    </Container>
  )
}

export default SplitScreen
