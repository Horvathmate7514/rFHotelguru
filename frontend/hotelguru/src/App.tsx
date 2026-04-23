
import './App.css'
import UserMenu from './components/UserMenu'
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom'
import ProfileData from './pages/ProfileData'
import Admin from './pages/Admin'
import RoomsList from './pages/RoomsList'
import RoomDetail from './pages/RoomDetail'
import Reception from './pages/Reception'
import { ThemeProvider, createTheme, CssBaseline, AppBar, Toolbar, Typography, Container, Box } from '@mui/material'

const theme = createTheme({
  palette: {
    primary: {
      main: '#0f172a',
    },
    secondary: {
      main: '#d4af37', 
    },
    background: {
      default: '#f8fafc',
      paper: '#ffffff',
    },
    text: {
      primary: '#1e293b',
      secondary: '#64748b',
    }
  },
  typography: {
    fontFamily: '"Montserrat", "Roboto", "Helvetica", "Arial", sans-serif',
    h1: { fontWeight: 600 },
    h2: { fontWeight: 600 },
    h3: { fontWeight: 600 },
    h4: { fontWeight: 600 },
    h5: { fontWeight: 600 },
    h6: { fontWeight: 600 },
    button: { textTransform: 'none', fontWeight: 600 },
  },
  shape: {
    borderRadius: 12,
  },
  components: {
    MuiButton: {
      styleOverrides: {
        root: {
          borderRadius: 8,
          padding: '8px 24px',
          boxShadow: 'none',
          '&:hover': {
            boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1)',
          }
        },
      },
    },
    MuiCard: {
      styleOverrides: {
        root: {
          borderRadius: 16,
          boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1), 0 2px 4px -2px rgb(0 0 0 / 0.1)',
        }
      }
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          backgroundImage: 'none',
        }
      }
    }
  }
})

function App() {
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <BrowserRouter>
        <AppBar position="sticky" elevation={0} sx={{ borderBottom: '1px solid', borderColor: 'divider', bgcolor: 'background.paper', color: 'text.primary' }}>
          <Toolbar sx={{ display: 'flex', justifyContent: 'space-between', px: 2, flexWrap: 'wrap', gap: 1 }}>
            <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
              <Link to="/" style={{ textDecoration: 'none', display: 'flex', alignItems: 'center', gap: '8px' }}>
                <Box sx={{ width: 32, height: 32, bgcolor: 'primary.main', borderRadius: '8px', display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
                  <Typography sx={{ color: 'white', fontWeight: 900, lineHeight: 1, fontSize: 14 }}>H</Typography>
                </Box>
                <Typography variant="h6" sx={{ color: 'primary.main', fontWeight: 800, letterSpacing: '-0.5px', display: { xs: 'none', sm: 'block' } }}>
                  HotelGuru
                </Typography>
              </Link>
            </Box>
            <Box sx={{ ml: 'auto' }}>
              <UserMenu />
            </Box>
          </Toolbar>
        </AppBar>

        <Container maxWidth="xl" sx={{ mt: 4, mb: 8, minHeight: '80vh' }}>
          <Routes>
            <Route path="/" element={<RoomsList />} />
            <Route path="/rooms/:id" element={<RoomDetail />} />
            <Route path="/profildata" element={<ProfileData />} />
            <Route path="/reception" element={<Reception />} />
            <Route path="/admin/*" element={<Admin />} />
          </Routes>
        </Container>
      </BrowserRouter>
    </ThemeProvider>
  )
}

export default App
