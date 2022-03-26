import styled from 'styled-components'

export const Container = styled.div`
  display: flex;
`

const BasePane = styled.div`
  flex: 1;
`

interface PaneWeight {
  weight: number
}

export const Pane = styled(BasePane)<PaneWeight>`
  flex: ${(props) => (props.weight ? props.weight : 1)};
`
