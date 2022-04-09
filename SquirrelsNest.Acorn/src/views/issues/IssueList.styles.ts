import {Box, Stack, Typography} from '@mui/material'
import styled from 'styled-components'

export const RelativeBox = styled( Box )`
  position: relative;
`
export const TopRightStack = styled( Stack )`
  position: absolute;
  right: 0;
  top: 0;
  padding: 0;
  margin: 0;
`
export const SubTypography = styled( Typography )`
  opacity: 0.7;
  // transform: scale(1);
  // -webkit-transform-origin-x: 0; // align text left after scaling
`
