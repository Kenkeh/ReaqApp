import { createTheme } from '@mui/material/styles';
import { blue, deepPurple } from '@mui/material/colors';

export const overTheme = createTheme({
    palette: {
      secondary: {
        main: blue[300],
        dark: blue[500],
        light: blue[100],
      },
      purple:{
        main: deepPurple[300],
        dark: deepPurple[500],
        light: deepPurple[100],
      },
      specialPurple:{
        main: 'rgba(94, 53, 177, 0.3)'
      }
    }
  });
