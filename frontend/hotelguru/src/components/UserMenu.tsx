import React, { useState, useEffect, useCallback } from 'react'
import IconButton from '@mui/material/IconButton'
import Menu from '@mui/material/Menu'
import MenuItem from '@mui/material/MenuItem'
import Dialog from '@mui/material/Dialog'
import DialogContent from '@mui/material/DialogContent'
import SignupForm from './SignupForm'
import LoginForm from './LoginForm'
import { PersonPinCircleTwoTone } from '@mui/icons-material'
import MoreVertIcon from '@mui/icons-material/MoreVert'
import { Box, Typography } from '@mui/material'
import { useQuery } from '@tanstack/react-query'
import { getUserById } from '../network/user.api'
import { Button, CircularProgress } from '@mui/material'
import { useNavigate } from 'react-router-dom'

function UserMenu() {
  const [userId, setUserId] = useState<number | null>(() => {
    const stored = localStorage.getItem('userId')
    return stored ? Number(stored) : null
  })

  useEffect(() => {
    const handleAuthChange = () => {
      const stored = localStorage.getItem('userId')
      setUserId(stored ? Number(stored) : null)
    }
    window.addEventListener('auth-change', handleAuthChange)
    return () => window.removeEventListener('auth-change', handleAuthChange)
  }, [])

  const navigate = useNavigate()


  const handleLogout = () => {
    localStorage.removeItem('userId')
    window.dispatchEvent(new Event('auth-change'))
 
    navigate('/')
  }

  const { data: user, isLoading, isError } = useQuery({
    queryKey: ['user', userId],
    queryFn: () => getUserById(userId!),
    enabled: userId !== null,
  })

  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null)
  const [profileAnchorEl, setProfileAnchorEl] = useState<null | HTMLElement>(null)
  const [mobileAnchor, setMobileAnchor] = useState<null | HTMLElement>(null)
  const [openSignup, setOpenSignup] = useState(false)
  const [openLogin, setOpenLogin] = useState(false)

  const handleOpen = (e: React.MouseEvent<HTMLElement>) => setAnchorEl(e.currentTarget)
  const handleCloseMenu = () => setAnchorEl(null)
  const handleMobileOpen = (e: React.MouseEvent<HTMLElement>) => setMobileAnchor(e.currentTarget)
  const handleMobileClose = () => setMobileAnchor(null)

  return (
    <div style={{ display: 'flex', alignItems: 'center', gap: '16px' }}>
      {userId ? (
        isLoading ? (
          <div style={{ display: 'flex', alignItems: 'center', gap: '16px' }}>
            <CircularProgress size={24} />
          </div>
        ) : isError || !user ? (
          <div>
            <IconButton onClick={handleOpen} size="large" color="inherit" aria-label="user menu">
              <PersonPinCircleTwoTone />
            </IconButton>
          </div>
        ) : (
          <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
            <Box sx={{ display: { xs: 'none', sm: 'flex' }, alignItems: 'center' }}>
              <Typography sx={{ fontWeight: 'bold', color: 'text.primary', mr: 1 }}>Szia, {user.fullName || user.email}!</Typography>
            </Box>

            <Box sx={{ display: { xs: 'none', sm: 'flex' }, gap: 1 }}>
              <Button variant="contained" color="primary" onClick={(e) => setProfileAnchorEl(e.currentTarget)}>Profilom</Button>
              <Button variant="contained" color="secondary" onClick={() => navigate('/reception')}>Recepciós</Button>
              <Button variant="outlined" color="inherit" onClick={handleLogout}>Kijelentkezés</Button>
            </Box>

            <IconButton onClick={handleMobileOpen} size="large" color="inherit" aria-label="menu" sx={{ display: { xs: 'flex', sm: 'none' } }}>
              <MoreVertIcon />
            </IconButton>

            <Menu anchorEl={mobileAnchor} open={Boolean(mobileAnchor)} onClose={handleMobileClose}>
              <MenuItem onClick={() => { handleMobileClose(); setProfileAnchorEl(null); navigate('/profildata') }}>Profilom</MenuItem>
              <MenuItem onClick={() => { handleMobileClose(); navigate('/reception') }}>Recepciós</MenuItem>
              <MenuItem onClick={() => { handleMobileClose(); handleLogout(); }}>Kijelentkezés</MenuItem>
            </Menu>
          </Box>
        )
      ) : (
        <>
          <IconButton onClick={handleOpen} size="large" color="inherit" aria-label="user menu">
            <PersonPinCircleTwoTone />
          </IconButton>

          <Menu anchorEl={anchorEl} open={Boolean(anchorEl)} onClose={handleCloseMenu}>
            <MenuItem onClick={() => { handleCloseMenu(); setOpenSignup(true); }}>
              Regisztráció
            </MenuItem>
            <MenuItem onClick={() => { handleCloseMenu(); setOpenLogin(true); }}>
              Bejelentkezés
            </MenuItem>
          </Menu>
        </>
      )}

      <Menu anchorEl={profileAnchorEl} open={Boolean(profileAnchorEl)} onClose={() => setProfileAnchorEl(null)}>
        <MenuItem onClick={() => { setProfileAnchorEl(null); navigate('/profildata') }}>Profil adatok</MenuItem>
        <MenuItem onClick={() => { setProfileAnchorEl(null); navigate('/admin') }}>Admin</MenuItem>
      </Menu>

      <Dialog open={openSignup} onClose={() => setOpenSignup(false)}>

        <DialogContent>
          <SignupForm onClose={() => setOpenSignup(false)} />
        </DialogContent>
      </Dialog>

      <Dialog open={openLogin} onClose={() => setOpenLogin(false)}>
        <DialogContent>
          <LoginForm onClose={() => setOpenLogin(false)} />
        </DialogContent>
      </Dialog>
    </div>
  )
}
export default UserMenu
