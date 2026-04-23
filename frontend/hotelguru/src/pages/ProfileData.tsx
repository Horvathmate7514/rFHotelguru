import React, { useState } from 'react'
import { useQuery, useQueryClient } from '@tanstack/react-query'
import { getUserById, updateUser } from '../network/user.api'
import { getReservationByUserId, getReservationById, addServiceToReservation } from '../network/reservation.api'
import { getRoomById } from '../network/room.api'
import { getAllFacilities, getFacilityById } from '../network/Facility.api'
import { Box, Paper, Typography, CircularProgress, IconButton, Dialog, DialogContent, DialogActions, TextField, Button, List, ListItem, ListItemButton, Grid, Card, Divider, Avatar, Chip, Select, MenuItem, InputLabel, FormControl } from '@mui/material'
import EditIcon from '@mui/icons-material/Edit'
import PersonIcon from '@mui/icons-material/Person'
import EventIcon from '@mui/icons-material/Event'
import CalendarMonthIcon from '@mui/icons-material/CalendarMonth'
import { useNavigate } from 'react-router-dom'
import type { Reservation } from '../network/reservation.api'
import EmailIcon from '@mui/icons-material/Email';
import PhoneIcon from '@mui/icons-material/Phone';
import HomeIcon from '@mui/icons-material/Home';

function ReservationsList({ userId }: { userId: number }) {
  const { data: reservations, isLoading } = useQuery({
    queryKey: ['reservations', userId],
    queryFn: () => getReservationByUserId(userId),
  })
  const [selectedId, setSelectedId] = useState<number | null>(null)

  return (
    <Box>
      {isLoading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', p: 3 }}><CircularProgress size={30} /></Box>
      ) : reservations?.length === 0 ? (
        <Box sx={{ textAlign: 'center', py: 4, color: 'text.secondary' }}>
          <EventIcon sx={{ fontSize: 60, opacity: 0.2, mb: 2 }} />
          <Typography>Még nincsenek foglalásaid.</Typography>
        </Box>
      ) : (
        <List sx={{ pt: 0 }}>
          {reservations?.map((r: Reservation) => (
            <Card key={r.id} sx={{ mb: 2, boxShadow: '0 2px 8px rgba(0,0,0,0.05)', borderRadius: 3, border: '1px solid', borderColor: 'divider', transition: 'transform 0.2s', '&:hover': { transform: 'scale(1.01)', borderColor: 'primary.main' } }}>
              <ListItemButton onClick={() => setSelectedId(r.id)} sx={{ p: 2 }}>
                <Box sx={{ display: 'flex', width: '100%', justifyContent: 'space-between', alignItems: 'center' }}>
                  <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
                    <Avatar sx={{ bgcolor: 'primary.light', width: 40, height: 40 }}>
                      <CalendarMonthIcon fontSize="small" />
                    </Avatar>
                    <Box>
                      <Typography variant="subtitle1" fontWeight="bold">Foglalás #{r.id}</Typography>
                      <Typography variant="body2" color="text.secondary">
                        {new Date(r.fromDate).toLocaleDateString()} — {new Date(r.toDate).toLocaleDateString()}
                      </Typography>
                    </Box>
                  </Box>
                  <Chip 
                    label={r.status === 'Pending' ? 'Függőben' : r.status || 'Aktív'} 
                    color={r.status === 'Pending' ? 'warning' : 'success'} 
                    size="small" 
                    sx={{ fontWeight: 'bold' }} 
                  />
                </Box>
              </ListItemButton>
            </Card>
          ))}
        </List>
      )}

      <ReservationDetailsDialog reservationId={selectedId} onClose={() => setSelectedId(null)} />
    </Box>
  )
}

function ServiceListItem({ benefit }: { benefit: { benefitId: number, quantity: number } }) {
  const { data: service, isLoading } = useQuery({
    queryKey: ['service', benefit.benefitId],
    queryFn: () => getFacilityById(benefit.benefitId),
    enabled: !!benefit.benefitId, 
  })

  const serviceName = service?.facilityName || service?.name || `Szolgáltatás #${benefit.benefitId}`;

  return (
    <ListItem sx={{ px: 0, py: 0.5 }}>
      {isLoading ? (
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
          <Typography variant="body2">•</Typography>
          <CircularProgress size={14} thickness={5} />
        </Box>
      ) : (
        <Typography variant="body2">
          • {serviceName} - <Typography component="span" variant="body2" color="text.secondary">{benefit.quantity} db</Typography>
        </Typography>
      )}
    </ListItem>
  )
}

function ReservationDetailsDialog({ reservationId, onClose }: { reservationId: number | null; onClose: () => void }) {
  const queryClient = useQueryClient()
  const [selectedService, setSelectedService] = useState<number | ''>('')
  const [quantity, setQuantity] = useState<number>(1)
  const [addingService, setAddingService] = useState(false)

  const { data: res, isLoading } = useQuery({
    queryKey: ['reservation', reservationId],
    queryFn: () => reservationId ? getReservationById(reservationId) : Promise.resolve(null),
    enabled: reservationId != null,
  })

  const { data: roomData } = useQuery({
    queryKey: ['room', res?.roomId],
    queryFn: () => getRoomById(res!.roomId),
    enabled: !!res?.roomId,
  })

  const { data: facilities } = useQuery({
    queryKey: ['facilities'],
    queryFn: getAllFacilities,
  })

  const handleAddService = async () => {
    if (!reservationId || selectedService === '') return
    setAddingService(true)
    try {
      await addServiceToReservation(reservationId, Number(selectedService), quantity)
      await queryClient.invalidateQueries({ queryKey: ['reservation', reservationId] })
      setSelectedService('')
      setQuantity(1)
      alert("Szolgáltatás sikeresen hozzáadva!")
    } catch(err) {
      console.error(err)
      alert("Hiba történt a szolgáltatás rendelésekor.")
    } finally {
      setAddingService(false)
    }
  }

  const nights = res ? Math.max(1, Math.ceil((new Date(res.toDate).getTime() - new Date(res.fromDate).getTime()) / (1000 * 3600 * 24))) : 0
  const roomPrice = roomData?.pricePerNight ?? roomData?.basePrice ?? 0
  const totalRoomPrice = roomPrice * nights

  const totalServicesPrice = res?.reservationBenefits?.reduce((total: number, benefit: { benefitId: number, quantity: number }) => {
    const facility = facilities?.find((f: { id: number, facilityName: string, price: number }) => f.id === benefit.benefitId)
    return total + (facility?.price ? facility.price * benefit.quantity : 0)
  }, 0) || 0

  const grandTotal = totalRoomPrice + totalServicesPrice

  return (
    <Dialog
      open={Boolean(reservationId)}
      onClose={onClose}
      fullWidth
      maxWidth="sm"
      PaperProps={{ sx: { borderRadius: 4, width: { xs: '95%', sm: '84%', md: 640 }, maxWidth: '100%' } }}
    >
      <Box sx={{ bgcolor: 'primary.main', color: 'white', p: { xs: 2, sm: 3 }, textAlign: 'center' }}>
        <Typography variant="h6" fontWeight="bold">Foglalás részletei</Typography>
        <Typography variant="body2" sx={{ opacity: 0.85, mt: 0.5 }}>#{reservationId} számú foglalás</Typography>
      </Box>
      <DialogContent sx={{ p: { xs: 2, sm: 3 } }}>
        {isLoading ? (
          <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}><CircularProgress /></Box>
        ) : res ? (
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2.5 }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', p: 2, bgcolor: 'background.default', borderRadius: 2 }}>
              <Typography color="text.secondary" variant="body2">Státusz</Typography>
              <Chip label={res.status === 'Pending' ? 'Függőben' : res.status || 'Aktív'} color={res.status === 'Pending' ? 'warning' : 'success'} sx={{ fontWeight: 'bold' }} />
            </Box>
            <Divider />
            <Grid container spacing={2}>
              <Grid item xs={6}>
                <Typography color="text.secondary" variant="caption">Szoba azonosító</Typography>
                <Typography variant="body1" fontWeight="bold">Szoba #{res.roomId}</Typography>
              </Grid>
              <Grid item xs={6}>
                <Typography color="text.secondary" variant="caption">Várható végösszeg</Typography>
                <Typography variant="body1" fontWeight="bold" color="primary.main">
                  {grandTotal > 0 ? `${grandTotal.toLocaleString()} Ft` : 'N/A'}
                  {grandTotal > 0 && <Typography component="span" variant="caption" color="text.secondary"> (Szállás + Extrák)</Typography>}
                </Typography>
              </Grid>
              <Grid item xs={6}>
                <Typography color="text.secondary" variant="caption">Érkezés</Typography>
                <Typography variant="body1" fontWeight="bold">{new Date(res.fromDate).toLocaleDateString()}</Typography>
              </Grid>
              <Grid item xs={6}>
                <Typography color="text.secondary" variant="caption">Távozás</Typography>
                <Typography variant="body1" fontWeight="bold">{new Date(res.toDate).toLocaleDateString()}</Typography>
              </Grid>
            </Grid>

           {res.reservationBenefits && res.reservationBenefits.length > 0 && (
              <>
                <Divider sx={{ my: 1 }} />
                <Typography variant="subtitle2" fontWeight="bold">Megrendelt szolgáltatások</Typography>
                <List dense sx={{ pt: 0 }}>
                  {res.reservationBenefits.map((b: { benefitId: number, quantity: number }, idx: number) => (
                    <ServiceListItem key={idx} benefit={b} />
                  ))}
                </List>
              </>
            )}

            {['CheckedIn', 'Avalaible', 'Booked', 'Accepted'].includes(res.status) && (
              <Box sx={{ mt: 2, p: 2, bgcolor: 'background.paper', border: '1px solid', borderColor: 'divider', borderRadius: 2 }}>
                <Typography variant="subtitle2" fontWeight="bold" sx={{ mb: 1.5 }}>Kiegészítő szolgáltatás rendelése</Typography>
                <Grid container spacing={2} alignItems="flex-end">
                  <Grid item xs={7}>
                    <FormControl fullWidth size="small">
                      <InputLabel>Szolgáltatás</InputLabel>
                      <Select
                        value={selectedService}
                        label="Szolgáltatás"
                        onChange={(e) => setSelectedService(e.target.value as number)}
                        defaultValue=""
                      >
                        {facilities?.map((f: { id: number, facilityName: string, price: number }) => (
                          <MenuItem key={f.id} value={f.id}>{f.facilityName} ({f.price} Ft)</MenuItem>
                        ))}
                      </Select>
                    </FormControl>
                  </Grid>
                  <Grid item xs={3}>
                    <TextField 
                      label="Mennyiség" 
                      type="number" 
                      size="small" 
                      fullWidth 
                      value={quantity} 
                      onChange={(e) => setQuantity(Number(e.target.value))} 
                      inputProps={{ min: 1 }}
                    />
                  </Grid>
                  <Grid item xs={2}>
                    <Button 
                      variant="contained" 
                      fullWidth 
                      disabled={selectedService === '' || quantity < 1 || addingService}
                      onClick={handleAddService}
                    >
                      Kér
                    </Button>
                  </Grid>
                </Grid>
              </Box>
            )}
          </Box>
        ) : (
          <Typography textAlign="center" color="text.secondary">Nincs adat</Typography>
        )}
      </DialogContent>
      <DialogActions sx={{ p: { xs: 1.5, sm: 3 }, pt: 0, justifyContent: 'center' }}>
        <Button variant="contained" onClick={onClose} sx={{ px: 4, borderRadius: 2 }}>Bezárás</Button>
      </DialogActions>
    </Dialog>
  )
}

export default function ProfileData() {
  const rawId = localStorage.getItem('userId')
  const userId = rawId ? Number(rawId) : null

  const { data: user, isLoading, isError } = useQuery({
    queryKey: ['profile', userId],
    queryFn: () => getUserById(userId!),
    enabled: userId !== null,
  })
  const queryClient = useQueryClient()
  const navigate = useNavigate()

  const [openEdit, setOpenEdit] = useState(false)
  const [form, setForm] = useState<Partial<User>>({})

  const { refetch: submitUpdate, isFetching: isUpdateLoading } = useQuery({
    queryKey: ['updateProfile', userId, form],
    queryFn: () => updateUser(userId!, form),
    enabled: false,
  })

  const openEditDialog = () => {
    setForm({ fullName: user?.fullName, email: user?.email, mobile: user?.mobile, address: user?.address })
    setOpenEdit(true)
  }

  const handleSubmit = async () => {
    if (!userId) return
    try {
      await submitUpdate()
      await queryClient.invalidateQueries({ queryKey: ['profile', userId] })
      setOpenEdit(false)
    } catch (err) {
    }
  }

  if (!userId) {
    return (
      <Box sx={{ p: 4 }}>
        <Typography>Nincs bejelentkezett felhasználó.</Typography>
      </Box>
    )
  }

  if (isLoading)
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}>
        <CircularProgress />
      </Box>
    )

  if (isError || !user)
    return (
      <Box sx={{ p: 4 }}>
        <Typography>Nem található a profil.</Typography>
      </Box>
    )

  return (
    <Box sx={{ maxWidth: 1000, mx: 'auto', p: { xs: 2, md: 4 } }}>
      <Box sx={{ mb: 6, textAlign: 'center' }}>
        <Avatar sx={{ width: 100, height: 100, mx: 'auto', mb: 2, bgcolor: 'primary.main', fontSize: '3rem', fontWeight: 'bold' }}>
          {user.fullName ? user.fullName.charAt(0).toUpperCase() : <PersonIcon fontSize="large" />}
        </Avatar>
        <Typography variant="h3" fontWeight="800" sx={{ mb: 1, color: 'text.primary' }}>Üdvözlünk, {user.fullName}</Typography>
        <Typography variant="body1" color="text.secondary">Itt kezelheted személyes adataidat és megtekintheted foglalásaidat.</Typography>
      </Box>

      <Box sx={{ display: 'flex', flexDirection: { xs: 'column', md: 'row' }, gap: 4, alignItems: 'stretch', height: '100%' }}>
        <Box sx={{ flex: 1 }}>
          <Paper sx={{ p: 0, borderRadius: 4, overflow: 'hidden', boxShadow: '0 4px 20px rgba(0,0,0,0.08)',  }}>
            <Box sx={{ p: 3, bgcolor: 'primary.main', color: 'white', display: 'flex', width: '100%',    justifyContent: 'space-between', alignItems: 'center' }}>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1.5 }}>
                <PersonIcon />
                <Typography variant="h6" fontWeight="bold">Személyes Adatok</Typography>
              </Box>
              <IconButton onClick={openEditDialog} aria-label="edit" sx={{ color: 'white', bgcolor: 'rgba(255,255,255,0.2)', '&:hover': { bgcolor: 'rgba(255,255,255,0.3)' } }}>
                <EditIcon fontSize="small" />
              </IconButton>
            </Box>
            
            <Box sx={{ p: 3 }}>
              <List sx={{ p: 0 }}>
                <ListItem sx={{ px: 0, py: 2, borderBottom: '1px solid', borderColor: 'divider' }}>
                  <Box sx={{ display: 'flex', alignItems: 'flex-start', gap: 2, width: '100%' }}>
                    <Avatar sx={{ bgcolor: 'primary.light', width: 40, height: 40 }}><PersonIcon fontSize="small" /></Avatar>
                    <Box>
                      <Typography variant="caption" color="text.secondary" fontWeight="bold">Név</Typography>
                      <Typography variant="body1" fontWeight="medium">{user.fullName}</Typography>
                    </Box>
                  </Box>
                </ListItem>
                <ListItem sx={{ px: 0, py: 2, borderBottom: '1px solid', borderColor: 'divider' }}>
                  <Box sx={{ display: 'flex', alignItems: 'flex-start', gap: 2, width: '100%' }}>
                    <Avatar sx={{ bgcolor: 'secondary.main', width: 40, height: 40 }}><EmailIcon fontSize="small" /></Avatar>
                    <Box>
                      <Typography variant="caption" color="text.secondary" fontWeight="bold">E-mail cím</Typography>
                      <Typography variant="body1" fontWeight="medium">{user.email}</Typography>
                    </Box>
                  </Box>
                </ListItem>
                <ListItem sx={{ px: 0, py: 2, borderBottom: '1px solid', borderColor: 'divider' }}>
                  <Box sx={{ display: 'flex', alignItems: 'flex-start', gap: 2, width: '100%' }}>
                    <Avatar sx={{ bgcolor: 'success.main', width: 40, height: 40 }}><PhoneIcon fontSize="small" /></Avatar>
                    <Box>
                      <Typography variant="caption" color="text.secondary" fontWeight="bold">Telefonszám</Typography>
                      <Typography variant="body1" fontWeight="medium">{user.mobile || 'Nem megadott'}</Typography>
                    </Box>
                  </Box>
                </ListItem>
                <ListItem sx={{ px: 0, py: 2 }}>
                  <Box sx={{ display: 'flex', alignItems: 'flex-start', gap: 2, width: '100%' }}>
                    <Avatar sx={{ bgcolor: 'warning.main', width: 40, height: 40 }}><HomeIcon fontSize="small" /></Avatar>
                    <Box>
                      <Typography variant="caption" color="text.secondary" fontWeight="bold">Lakcím</Typography>
                      {user.address ? (
                        <Typography variant="body1" fontWeight="medium">
                          {user.address.country}, {user.address.city}<br />
                          {user.address.street} {user.address.houseNumber}<br />
                          {user.address.postCode}
                        </Typography>
                      ) : (
                        <Typography variant="body1" color="text.secondary">Nem megadott</Typography>
                      )}
                    </Box>
                  </Box>
                </ListItem>
              </List>
            </Box>
          </Paper>
        </Box>

        <Box sx={{ flex: 1 }}>
          <Paper sx={{ p: 0, borderRadius: 4, overflow: 'hidden', boxShadow: '0 4px 20px rgba(0,0,0,0.08)', height: '100%', display: 'flex', flexDirection: 'column' }}>
            <Box sx={{ p: 3, bgcolor: 'background.default', borderBottom: '1px solid', borderColor: 'divider', display: 'flex', alignItems: 'center', gap: 1.5 }}>
              <EventIcon color="primary" />
              <Typography variant="h6" fontWeight="bold" color="text.primary">Foglalásaim</Typography>
            </Box>
            <Box sx={{ p: 3, flexGrow: 1, bgcolor: 'background.paper' }}>
              {userId ? (
                <ReservationsList userId={userId} />
              ) : (
                <Typography color="text.secondary" textAlign="center" py={4}>Jelentkezz be a foglalások megtekintéséhez.</Typography>
              )}
            </Box>
          </Paper>
        </Box>
      </Box>
      <Dialog
        open={openEdit}
        onClose={() => setOpenEdit(false)}
        fullWidth
        maxWidth="sm"
        PaperProps={{ sx: { borderRadius: 4, width: { xs: '96%', sm: '86%', md: 700 }, maxWidth: '100%' } }}
      >
        <Box sx={{ bgcolor: 'primary.main', color: 'white', p: { xs: 2, sm: 3 }, textAlign: 'center' }}>
          <Typography variant="h6" fontWeight="bold">Profil szerkesztése</Typography>
          <Typography variant="body2" sx={{ opacity: 0.85, mt: 0.5 }}>Frissítsd a személyes adataidat</Typography>
        </Box>
        <DialogContent sx={{ p: { xs: 2, sm: 4 } }}>
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
            <TextField label="Teljes név" value={form.fullName || ''} onChange={(e) => setForm(f => ({ ...f, fullName: e.target.value }))} variant="outlined" fullWidth />
            <TextField label="E-mail" value={form.email || ''} onChange={(e) => setForm(f => ({ ...f, email: e.target.value }))} variant="outlined" fullWidth />
            <TextField label="Mobil" value={form.mobile || ''} onChange={(e) => setForm(f => ({ ...f, mobile: e.target.value }))} variant="outlined" fullWidth />
            
            <Typography variant="subtitle1" fontWeight="bold" sx={{ mt: 1, mb: -1, color: 'text.secondary' }}>Cím adatok</Typography>
            <Box sx={{ display: 'flex', gap: 2, flexDirection: { xs: 'column', sm: 'row' } }}>
              <TextField label="Ország" value={form.address?.country || ''} onChange={(e) => setForm(f => ({ ...f, address: { ...f.address, country: e.target.value } as string }))} variant="outlined" sx={{ flex: 1 }} />
              <TextField label="Város" value={form.address?.city || ''} onChange={(e) => setForm(f => ({ ...f, address: { ...f.address, city: e.target.value } as string }))} variant="outlined" sx={{ flex: 1 }} />
            </Box>
            <Box sx={{ display: 'flex', gap: 2, flexDirection: { xs: 'column', sm: 'row' } }}>
              <TextField label="Utca" value={form.address?.street || ''} onChange={(e) => setForm(f => ({ ...f, address: { ...f.address, street: e.target.value } as string }))} variant="outlined" sx={{ flex: 2 }} />
              <TextField label="Házszám" value={form.address?.houseNumber || ''} onChange={(e) => setForm(f => ({ ...f, address: { ...f.address, houseNumber: e.target.value } as string }))} variant="outlined" sx={{ flex: 1 }} />
            </Box>
          </Box>
        </DialogContent>
        <DialogActions sx={{ p: { xs: 1.5, sm: 3 }, pt: 0, justifyContent: 'center' }}>
          <Button onClick={() => setOpenEdit(false)} sx={{ color: 'text.secondary', mr: 2 }}>Mégse</Button>
          <Button variant="contained" onClick={handleSubmit} disabled={isUpdateLoading} sx={{ px: 4, py: 1.25, borderRadius: 2 }}>
            {isUpdateLoading ? 'Mentés...' : 'Mentés'}
          </Button>
          {/* Admin felulet kesobb auth alapján jelenik meg */}
          <Button variant="outlined" color="primary" onClick={() => navigate('/admin')} sx={{ px: 3, py: 1.25, borderRadius: 2, ml: 1 }}>Admin műveletek</Button>
        </DialogActions>
      </Dialog>
    </Box>
  )
}