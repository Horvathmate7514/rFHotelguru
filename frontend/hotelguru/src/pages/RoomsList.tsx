import React, { useState } from 'react'
import { useQuery } from '@tanstack/react-query'
import { getAllRooms, getAvailableRoomsInDateRange, getRoomById } from '../network/room.api'
import { getAllRoomTypes } from '../network/roomType.api'
import { useQuery as useRQQuery } from '@tanstack/react-query'
import { Box, Card, CardContent, CardActions, Typography, Button, CircularProgress, Chip, Grid, TextField } from '@mui/material'
import heroImg from '../assets/hero.png'
import { useNavigate } from 'react-router-dom'
import HotelIcon from '@mui/icons-material/Hotel'
import type {RoomType} from '../network/roomType.api'

export default function RoomsList() {
  const navigate = useNavigate()
  const [fromDate, setFromDate] = useState<string>('')
  const [toDate, setToDate] = useState<string>('')

  const { data: rooms, isLoading, isError } = useQuery({
    queryKey: ['rooms'],
    queryFn: () => getAllRooms(),
  })

  const { data: roomTypes } = useRQQuery<RoomType[]>({
    queryKey: ['roomTypes'],
    queryFn: () => getAllRoomTypes()
  })

  const { data: roomDetails } = useRQQuery<Room[]>({
    queryKey: ['roomDetails', rooms?.map(r => r.id)],
    queryFn: async () => {
      if (!rooms) return []
      const details = await Promise.all(rooms.map(r => getRoomById(r.id)))
      return details
    },
    enabled: Boolean(rooms && rooms.length > 0)
  })

  const { data: availableRooms, isLoading: loadingAvailable, refetch: refetchAvailable } = useQuery({
    queryKey: ['availableRooms', fromDate, toDate],
    queryFn: () => getAvailableRoomsInDateRange(fromDate, toDate),
    enabled: false
  })

  if (isLoading) return (
    <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', minHeight: '60vh' }}>
      <CircularProgress size={60} thickness={4} />
    </Box>
  )

  if (isError) return (
    <Box sx={{ p: 4, textAlign: 'center' }}>
      <Typography variant="h6" color="error">Hiba a szobák betöltésekor.</Typography>
    </Box>
  )

  const listToRender: Room[] | undefined = availableRooms || rooms
  const detailsById = (roomDetails || []).reduce((acc: Record<number, Room>, d: Room) => {
    if (d?.id) acc[d.id] = d
    return acc
  }, {})

  return (
    <Box>
      <Box sx={{ mb: 6, textAlign: 'center', py: { xs: 4, md: 8 }, bgcolor: 'primary.main', color: 'primary.contrastText', borderRadius: 4, mx: { xs: 2, md: 0 }, backgroundImage: 'linear-gradient(to right bottom, #0f172a, #1e293b)' }}>
        <Typography variant="h3" component="h1" fontWeight="800" sx={{ mb: 2 }}>
          Fedezd fel a HotelGuru világát
        </Typography>
        <Typography variant="h6" fontWeight="regular" sx={{ opacity: 0.8, maxWidth: 600, mx: 'auto' }}>
          Gyors & Egyszerű
        </Typography>
      </Box>

    <Box sx={{ mb: 3, textAlign: 'center' }}>
        <Typography variant="subtitle1" sx={{ mb: 1 }}>Mikorra keres szobát?</Typography>
        <Box sx={{ display: 'flex', gap: 2, justifyContent: 'center', mb: 2, flexWrap: 'wrap', alignItems: 'center' }}>
          <TextField
            label="Érkezés"
            type="date"
            value={fromDate}
            onChange={(e) => setFromDate(e.target.value)}
            InputLabelProps={{ shrink: true }}
            size="small"
            sx={{
              '& input::-webkit-datetime-edit': {
                color: fromDate ? 'inherit' : 'transparent'
              },
              '& input:focus::-webkit-datetime-edit': {
                color: 'inherit'
              }
            }}
          />
          <TextField
            label="Távozás"
            type="date"
            value={toDate}
            onChange={(e) => setToDate(e.target.value)}
            InputLabelProps={{ shrink: true }}
            size="small"
            sx={{
              '& input::-webkit-datetime-edit': {
                color: toDate ? 'inherit' : 'transparent'
              },
              '& input:focus::-webkit-datetime-edit': {
                color: 'inherit'
              }
            }}
          />
          <Button
            variant="contained"
            size="medium"
            onClick={() => {
              if (!fromDate || !toDate) return
              refetchAvailable()
            }}
            disabled={!fromDate || !toDate || loadingAvailable}
          >
            {loadingAvailable ? 'Keresés...' : 'Keresés'}
          </Button>
        </Box>
        {loadingAvailable && <CircularProgress size={24} />}
      </Box>    
      <Typography variant="h4" component="h2" fontWeight="bold" sx={{ mb: 4, px: 2, color: 'text.primary', textAlign: 'center' }}>Kínálatunk</Typography>

      {/* ITT A JAVÍTÁS: Egy max szélességű középre zárt Box és justifyContent="center" a Grid-nek */}
      <Box sx={{ maxWidth: 1200, mx: 'auto' }}>
        <Grid container spacing={4} justifyContent="center" sx={{ px: 2 }}>
          {listToRender && listToRender.length === 0 && (
            <Box sx={{ width: '100%', textAlign: 'center', p: 4 }}>
              <Typography color="text.secondary">Nincsenek elérhető szobák a kiválasztott időszakra.</Typography>
            </Box>
          )}

          {listToRender?.map((room: Room) => {
            const d = detailsById[room.id]
            const roomType = roomTypes?.find(rt => rt.id === (d?.roomTypeId || room.roomType?.id))
            const price = d?.pricePerNight ?? d?.basePrice ?? roomType?.basePrice

            return (
              <Grid item xs={12} sm={6} md={4} key={room.id}>
                <Card sx={{ 
                  height: '100%', 
                  display: 'flex', 
                  flexDirection: 'column',
                  transition: 'all 0.3s cubic-bezier(0.4, 0, 0.2, 1)',
                  '&:hover': { transform: 'translateY(-8px)', boxShadow: 6 },
                  overflow: 'hidden'
                }}>
                  <Box sx={{ position: 'relative' }}>
                    <Box
                      component="img"
                      src={ `https://loremflickr.com/640/480/room` }
                      alt={room.title || `Room ${room.id}`}
                      onError={(e: React.SVGProps<SVGSVGElement>) => { e.currentTarget.src = heroImg }}
                      style={{ width: '100%', height: 260, objectFit: 'cover', display: 'block' }}
                    />
                    {room.roomType?.name && (
                      <Chip 
                        label={room.roomType.name} 
                        color="secondary" 
                        sx={{ position: 'absolute', top: 16, right: 16, fontWeight: 'bold' }} 
                      />
                    )}
                  </Box>
                  <CardContent sx={{ flexGrow: 1, p: 3 }}>
                    <Typography variant="h5" component="div" fontWeight="800" gutterBottom>
                      {room.title || room.roomType?.name || `Szoba ${room.id}`}
                    </Typography>
                    <Box sx={{ display: 'flex', gap: 1, mb: 2, alignItems: 'center', color: 'text.secondary' }}>
                      <HotelIcon fontSize="small" />
                      <Typography variant="body2" fontWeight="medium">
                        {price ? `${Number(price).toLocaleString()} Ft / éj` : `Kapacitás: ${roomType?.capacity ?? room.roomType?.capacity ?? 'Ismeretlen'} fő`}
                      </Typography>
                    </Box>
                    {room.description && (
                      <Typography variant="body2" color="text.secondary" sx={{ display: '-webkit-box', WebkitLineClamp: 3, WebkitBoxOrient: 'vertical', overflow: 'hidden' }}>
                        {room.description}
                      </Typography>
                    )}
                  </CardContent>
                  <CardActions sx={{ p: 3, pt: 0 }}>
                    <Button fullWidth variant="contained" size="large" onClick={() => navigate(`/rooms/${room.id}`)}>Részletek & Foglalás</Button>
                  </CardActions>
                </Card>
              </Grid>
            )
          })}
        </Grid>
      </Box>
    </Box>
  )
}