import { createTheme } from '@mui/material/styles';
import { blue, deepPurple } from '@mui/material/colors';

export const overTheme = createTheme({
    components: {
      MuiButtonBase:{
        defaultProps:{
          style:{
            width: '100%',
            height: '100%'
          }
        }
      },
      MuiSelect:{
        defaultProps:{
          style:{
            width: '100%',
            height: '100%'
          }
        }
      },
      MuiTextField:{
        defaultProps:{
          style:{
            width: '100%',
            height: '100%'
          }
        }
      },
      MuiInputBase:{
        defaultProps:{
          style:{
            width: '100%',
            height: '100%'
          }
        }
      }
    },
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
