import React, { useState } from 'react'
import { Button, TextField, Box, InputAdornment } from '@mui/material'
import EmailIcon from '@mui/icons-material/Email'
import LockIcon from '@mui/icons-material/Lock'
import { loginUser } from '../network/user.api'

type Props = {
  onClose?: () => void
}

export default function LoginForm({ onClose }: Props) {
  const [email, setEmail] = useState('')
  const [password, setPassword] = useState('')
  const [loading, setLoading] = useState(false)

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault()
    setLoading(true)
    try {
      const res = await loginUser({ email, password })
      console.log('Logged in', res)
      
      if (res?.userId || res?.id) {
        localStorage.setItem('userId', String(res.userId ?? res.id))
        window.dispatchEvent(new Event('auth-change'))
        
        // ITT VAN A TRÜKK: Azonnali oldalfrissítés a sikeres belépés után!
        window.location.reload()
      }
      
      if (onClose) onClose()
    } catch (err) {
      console.error('Login error', err)
      alert('Hibás e-mail cím vagy jelszó!')
    } finally {
      setLoading(false)
    }
  }

  return (
    <Box 
      component="form" 
      onSubmit={handleSubmit} 
      // A fix 360-as width helyett 100%-ot használunk, a gap-et pedig 3-ra növeltük, hogy levegősebb legyen
      sx={{ display: 'flex', flexDirection: 'column', gap: 3, width: '100%' }}
    >
      <TextField 
        label="E-mail cím" 
        type="email" 
        variant="outlined"
        value={email} 
        onChange={(e) => setEmail(e.target.value)} 
        required 
        fullWidth
        InputProps={{
          startAdornment: (
            <InputAdornment position="start">
              <EmailIcon color="action" />
            </InputAdornment>
          ),
        }}
      />
      <TextField 
        label="Jelszó" 
        type="password" 
        variant="outlined"
        value={password} 
        onChange={(e) => setPassword(e.target.value)} 
        required 
        fullWidth
        InputProps={{
          startAdornment: (
            <InputAdornment position="start">
              <LockIcon color="action" />
            </InputAdornment>
          ),
        }}
      />
      <Button 
        type="submit" 
        variant="contained" 
        size="large"
        disabled={loading || !email || !password}
        sx={{ 
          mt: 1, 
          py: 1.5, 
          fontSize: '1.1rem', 
          borderRadius: 2,
          fontWeight: 'bold'
        }}
      >
        {loading ? 'Bejelentkezés folyamatban...' : 'Bejelentkezés'}
      </Button>
    </Box>
  )
}