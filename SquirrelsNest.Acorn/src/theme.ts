import { createTheme } from '@mui/material/styles'

declare module '@mui/material/styles/createTheme' {
}

const colors = {
  accent: "#EA6500"
};

const theme = createTheme({
  palette: {
//    primary: {
      // light: will be calculated from palette.primary.main,
//      main: colors.primary,
      // dark: will be calculated from palette.primary.main,
//    },
  },
  components: {
    MuiListItemButton: {
      styleOverrides: {
        root: {
          '&.Mui-selected': {
            borderLeft: `5px solid ${colors.accent}`,
          },
        },
      },
    },
  },
})

export default theme
