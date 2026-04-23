import { useParams, useNavigate } from 'react-router-dom'
import { useQuery, useQueryClient } from '@tanstack/react-query'
import { getRoomById } from '../network/room.api'
import { getAllRoomTypes } from '../network/roomType.api'
import { getFacilityById } from '../network/Facility.api'
import { Box, Paper, Typography, CircularProgress, Button, Dialog, DialogContent, DialogActions, TextField, Grid, Divider, Chip } from '@mui/material'
import heroImg from '../assets/hero.png'
import ArrowBackIcon from '@mui/icons-material/ArrowBack'
import HotelIcon from '@mui/icons-material/Hotel'
import WeekendIcon from '@mui/icons-material/Weekend'
import CheckCircleIcon from '@mui/icons-material/CheckCircle'
import PaymentsIcon from '@mui/icons-material/Payments'
import DoneIcon from '@mui/icons-material/Done'
import { createReservation } from '../network/reservation.api'
import { useState } from 'react'

import LoginForm from '../components/LoginForm' 

export default function RoomDetail() {
  const { id } = useParams()
  const navigate = useNavigate()
  const roomId = id ? Number(id) : null

  const queryClient = useQueryClient()
  const [open, setOpen] = useState(false)
  const [fromDate, setFromDate] = useState('')
  const [toDate, setToDate] = useState('')
  const [loading, setLoading] = useState(false)
  
  // ÚJ ÁLLAPOT A LOGIN ABLAKNAK
  const [openLogin, setOpenLogin] = useState(false)

  const { data: fetchedRoom, isLoading, isError } = useQuery({
    queryKey: ['room', roomId],
    queryFn: () => getRoomById(roomId!),
    refetchOnWindowFocus: false,
    enabled: !!roomId,
  })

  const { data: roomTypes } = useQuery({
    queryKey: ['roomTypes'],
    queryFn: getAllRoomTypes
  })

  const { data: facilitiesData } = useQuery({
    queryKey: ['roomFacilities', roomId],
    queryFn: async () => {
      if (!fetchedRoom?.roomFacilities) return []
      const promises = fetchedRoom.roomFacilities.map((f: { facilityId: number }) => getFacilityById(f.facilityId))
      return await Promise.all(promises)
    },
    enabled: !!fetchedRoom?.roomFacilities,
  })

  if (!roomId) return <Box sx={{ p: 4, textAlign: 'center' }}><Typography variant="h6">Érvénytelen szoba azonosító.</Typography></Box>
  if (isLoading) return <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '60vh' }}><CircularProgress size={60} thickness={4} /></Box>
  if (isError || !fetchedRoom) return <Box sx={{ p: 4, textAlign: 'center' }}><Typography variant="h6">Szoba nem található.</Typography></Box>

  const roomType = roomTypes?.find((rt: { id: number }) => rt.id === fetchedRoom.roomTypeId)
  const price = fetchedRoom.pricePerNight ?? roomType?.basePrice ?? 0
  const roomTitle = `Szoba #${fetchedRoom.id}`

  const handleReserve = async () => {
    const rawId = localStorage.getItem('userId')
    const userId = rawId ? Number(rawId) : null
    if (!userId) {
      alert('Jelentkezz be először')
      return
    }
    if (!fromDate || !toDate) {
      alert('Adj meg kezdő és vég dátumot')
      return
    }
    setLoading(true)
    try {
      await createReservation({ reservation: { id: 0, roomId: fetchedRoom.id, userId, fromDate: new Date(fromDate), toDate: new Date(toDate), checkInDate: new Date(fromDate), checkOutDate: new Date(toDate), status: 'Pending' } })
      await queryClient.invalidateQueries({ queryKey: ['reservations', userId] })
      setOpen(false)
      alert('Sikeres foglalás! Részleteket a Profilom menüben találod.')
    } catch (err) {
      console.error(err)
      alert('Foglalás sikertelen. Kérjük próbáld újra.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <Box sx={{ maxWidth: 1000, mx: 'auto', p: { xs: 2, md: 4 } }}>
      <Button 
        startIcon={<ArrowBackIcon />} 
        onClick={() => navigate('/')} 
        sx={{ mb: 3, color: 'text.secondary', '&:hover': { color: 'primary.main', bgcolor: 'transparent' } }}
      >
        Vissza a szobákhoz
      </Button>

      <Paper sx={{ borderRadius: 4, overflow: 'hidden', mb: 4, boxShadow: '0 10px 15px -3px rgb(0 0 0 / 0.1), 0 4px 6px -4px rgb(0 0 0 / 0.1)' }}>
        <Box sx={{ position: 'relative' }}>
          <Box
            component="img"
            src={ `https://loremflickr.com/640/480/room,interior?random=${fetchedRoom.id}`}
            alt={roomTitle}
            onError={(e: React.SVGProps<SVGSVGElement>) => { e.currentTarget.src = heroImg }}
            sx={{ height: { xs: 300, sm: 400, md: 500 }, objectFit: 'cover', width: '100%', display: 'block' }}
          />
          <Box sx={{ position: 'absolute', top: 0, left: 0, right: 0, bottom: 0, background: 'linear-gradient(to top, rgba(0,0,0,0.7) 0%, rgba(0,0,0,0) 50%)' }} />
          <Box sx={{ position: 'absolute', bottom: 24, left: 32, right: 32, color: 'white' }}>
            <Typography variant="h3" fontWeight="800" sx={{ mb: 1, textShadow: '0 2px 4px rgba(0,0,0,0.5)' }}>
              {roomTitle}
            </Typography>
            {roomType && (
              <Chip label={`Típus: ${roomType.id}`} color="secondary" sx={{ fontWeight: 'bold', fontSize: '1rem', px: 1 }} />
            )}
          </Box>
        </Box>

        <Grid container>
          <Grid item xs={12} md={8} sx={{ p: { xs: 3, md: 5 } }}>
            <Typography variant="h5" fontWeight="bold" sx={{ mb: 3 }}>A szobáról</Typography>
            <Typography variant="body1" sx={{ color: 'text.secondary', lineHeight: 1.8, mb: 4, fontSize: '1.1rem' }}>
              Ez a szálloda egyedi kialakítású szobája minőségi pihenést nyújt vendégeinknek. 
              {fetchedRoom.status === 'Avalaible' ? ' Jelenleg foglalható állapotban van.' : ''}
              A pontos felszereltségekről alább tájékozódhatsz.
            </Typography>

            <Divider sx={{ mb: 4 }} />

            <Typography variant="h6" fontWeight="bold" sx={{ mb: 3 }}>Adatok & Jellemzők</Typography>
            <Grid container spacing={3}>
              <Grid item xs={12} sm={6}>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                  <Box sx={{ p: 1, bgcolor: 'primary.light', color: 'white', borderRadius: 2, display: 'flex' }}><HotelIcon /></Box>
                  <Box>
                    <Typography variant="body2" color="text.secondary">Kapacitás</Typography>
                    <Typography variant="subtitle1" fontWeight="bold">{roomType?.capacity ?? '-'} férőhely</Typography>
                  </Box>
                </Box>
              </Grid>
              <Grid item xs={12} sm={6}>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                  <Box sx={{ p: 1, bgcolor: 'primary.light', color: 'white', borderRadius: 2, display: 'flex' }}><WeekendIcon /></Box>
                  <Box>
                    <Typography variant="body2" color="text.secondary">Ágyak száma (Bed Number)</Typography>
                    <Typography variant="subtitle1" fontWeight="bold">{roomType?.bedNumber ?? '-'}</Typography>
                  </Box>
                </Box>
              </Grid>
              <Grid item xs={12} sm={6}>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                  <Box sx={{ p: 1, bgcolor: 'secondary.main', color: 'white', borderRadius: 2, display: 'flex' }}><PaymentsIcon /></Box>
                  <Box>
                    <Typography variant="body2" color="text.secondary">Alap Ár (Base Price)</Typography>
                    <Typography variant="subtitle1" fontWeight="bold">{roomType?.basePrice ? `${roomType.basePrice.toLocaleString()} Ft` : '-'}</Typography>
                  </Box>
                </Box>
              </Grid>
              <Grid item xs={12} sm={6}>
                <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                  <Box sx={{ p: 1, bgcolor: 'success.main', color: 'white', borderRadius: 2, display: 'flex' }}><CheckCircleIcon /></Box>
                  <Box>
                    <Typography variant="body2" color="text.secondary">Státusz (Status)</Typography>
                    <Typography variant="subtitle1" fontWeight="bold" color="success.main">{fetchedRoom.status || '-'}</Typography>
                  </Box>
                </Box>
              </Grid>
            </Grid>

            {/* Szolgáltatások / Facilities szekció */}
            <Typography variant="h6" fontWeight="bold" sx={{ mt: 6, mb: 3 }}>Amit a szoba kínál (Szolgáltatások)</Typography>
            <Grid container spacing={2}>
              {facilitiesData && facilitiesData.length > 0 ? (
                facilitiesData.map((facility: { facilityName: string; price: number }, index: number) => (
                  <Grid item xs={12} sm={6} md={4} key={index}>
                    <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                      <Box sx={{ color: 'secondary.main', display: 'flex' }}>
                        <DoneIcon color="primary" />
                      </Box>
                      <Box>
                        <Typography variant="body1" fontWeight="bold">{facility.facilityName || `Áru: ${facility.price} Ft`}</Typography>
                        {facility.price !== undefined && <Typography variant="caption" color="text.secondary">{facility.price} Ft</Typography>}
                      </Box>
                    </Box>
                  </Grid>
                ))
              ) : (
                <Grid item xs={12}>
                  <Typography variant="body2" color="text.secondary">Nincsenek rögzített extra szolgáltatások ehhez a szobához.</Typography>
                </Grid>
              )}
            </Grid>

          </Grid>

          <Grid item xs={12} md={4} sx={{ bgcolor: 'background.default', p: { xs: 3, md: 5 }, borderLeft: { md: '1px solid' }, borderColor: 'divider' }}>
            <Box sx={{ position: 'sticky', top: 100 }}>
              <Typography variant="h5" fontWeight="bold" sx={{ mb: 1 }}>Foglalás</Typography>
              <Typography variant="body2" color="text.secondary" sx={{ mb: 4 }}>
                Add meg a dátumokat a foglalás indításához.
              </Typography>
              
              <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3, mb: 4 }}>
                <Box sx={{ p: 3, bgcolor: 'background.paper', borderRadius: 3, boxShadow: '0 2px 4px rgba(0,0,0,0.05)' }}>
                  <Typography variant="overline" color="text.secondary" fontWeight="bold">Éjszakák ára</Typography>
                  <Typography variant="h4" fontWeight="900" color="primary.main">
                    {price > 0 ? `${Number(price).toLocaleString()} Ft` : '-'} 
                    <Typography component="span" variant="subtitle1" color="text.secondary"> / éj</Typography>
                  </Typography>
                </Box>
              </Box>

              {localStorage.getItem('userId') ? (
                <Button 
                  variant="contained" 
                  size="large"
                  fullWidth 
                  onClick={() => setOpen(true)}
                  sx={{ 
                    py: 2, 
                    fontSize: '1.1rem', 
                    borderRadius: 3,
                    boxShadow: '0 4px 14px 0 rgba(15, 23, 42, 0.3)',
                    '&:hover': { boxShadow: '0 6px 20px 0 rgba(15, 23, 42, 0.4)' }
                  }}
                >
                  Lefoglalom most
                </Button>
              ) : (
                <Button 
                  variant="outlined" 
                  size="large"
                  fullWidth 
                  onClick={() => setOpenLogin(true)} // JAVÍTÁS ITT
                  sx={{ 
                    py: 2, 
                    fontSize: '1.1rem', 
                    borderRadius: 3,
                    color: 'text.secondary',
                    borderColor: 'divider',
                  }}
                >
                  Jelentkezz be a foglaláshoz
                </Button>
              )}
            </Box>
          </Grid>
        </Grid>
      </Paper>

      {/* Foglalás Dialog */}
      <Dialog
        open={open}
        onClose={() => setOpen(false)}
        fullWidth
        maxWidth="xs"
        PaperProps={{ sx: { borderRadius: 4, width: { xs: '96%', sm: '420px' }, maxWidth: '100%' } }}
      >
        <Box sx={{ bgcolor: 'primary.main', color: 'white', p: 2, textAlign: 'center' }}>
          <Typography variant="h6" fontWeight="bold">Foglalás indítása</Typography>
          <Typography variant="body2" sx={{ opacity: 0.85, mt: 0.5 }}>{roomTitle}</Typography>
        </Box>
        <DialogContent sx={{ p: { xs: 2, sm: 3 } }}>
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
            <Box>
              <Typography variant="caption" color="text.secondary" sx={{ mb: 0.5, display: 'block' }}>Érkezés (Check-in)</Typography>
              <TextField
                type="date"
                value={fromDate}
                onChange={(e) => setFromDate(e.target.value)}
                fullWidth
                variant="outlined"
                inputProps={{ 'aria-label': 'érkezés' }}
              />
            </Box>

            <Box>
              <Typography variant="caption" color="text.secondary" sx={{ mb: 0.5, display: 'block' }}>Távozás (Check-out)</Typography>
              <TextField
                type="date"
                value={toDate}
                onChange={(e) => setToDate(e.target.value)}
                fullWidth
                variant="outlined"
                inputProps={{ 'aria-label': 'távozás' }}
              />
            </Box>
          </Box>
        </DialogContent>
        <DialogActions sx={{ p: { xs: 1.5, sm: 3 }, pt: 0, justifyContent: 'center' }}>
          <Button onClick={() => setOpen(false)} sx={{ color: 'text.secondary', mr: 2 }}>Mégse</Button>
          <Button 
            variant="contained" 
            onClick={handleReserve} 
            disabled={loading || !fromDate || !toDate} 
            sx={{ px: 4, py: 1.25, borderRadius: 2 }}
          >
            {loading ? 'Feldolgozás...' : 'Véglegesítés'}
          </Button>
        </DialogActions>
      </Dialog>

    {/* ÚJ: Login Dialog JAVÍTVA */}
      <Dialog 
        open={openLogin} 
        onClose={() => setOpenLogin(false)}
        maxWidth="xs"
        fullWidth
        PaperProps={{ sx: { borderRadius: 4, overflow: 'hidden' } }}
      >
        <Box sx={{ bgcolor: 'primary.main', color: 'white', p: 2, textAlign: 'center' }}>
          <Typography variant="h6" fontWeight="bold">Bejelentkezés</Typography>
          <Typography variant="body2" sx={{ opacity: 0.85, mt: 0.5 }}>A foglaláshoz kérjük, lépj be!</Typography>
        </Box>
        
        {/* Itt a p: 4 adja meg a belső margót, hogy ne lógjon ki a szélére! */}
        <DialogContent sx={{ p: { xs: 3, sm: 4 } }}>
          <LoginForm />
        </DialogContent>
        
        <DialogActions sx={{ p: { xs: 1.5, sm: 3 }, pt: 0, justifyContent: 'center' }}>
          <Button onClick={() => setOpenLogin(false)} sx={{ color: 'text.secondary' }}>
            Mégse / Bezárás
          </Button>
        </DialogActions>
      </Dialog>

    </Box>
  )
}