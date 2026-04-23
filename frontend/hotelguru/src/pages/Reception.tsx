import React, { useState } from 'react'
import { Box, Paper, Typography, List, ListItem, ListItemButton, ListItemText, Chip, Button, Dialog, DialogTitle, DialogContent, DialogActions, CircularProgress, Divider } from '@mui/material'
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query'
import { getAllReservations, acceptReservation, rejectReservation, getReservationById, cancelReservation, generateInvoice, checkInReservation, checkOutReservation } from '../network/reservation.api'
import { getAllUsers } from '../network/user.api'

export default function Reception() {
  const [selectedResId, setSelectedResId] = useState<number | null>(null)
  const [openInvoice, setOpenInvoice] = useState(false)
  const [invoiceData, setInvoiceData] = useState<any>(null) // Új state a számla adatainak
  
  const queryClient = useQueryClient()

  const rawUserId = localStorage.getItem('userId')
  const currentUserId = rawUserId ? Number(rawUserId) : 1 

  const { data: reservations, isLoading: loadingRes } = useQuery<Reservation[]>({
    queryKey: ['reservations'],
    queryFn: getAllReservations
  })

  const { data: users, isLoading: loadingUsers } = useQuery<User[]>({
    queryKey: ['users'],
    queryFn: getAllUsers
  })

  const { data: selectedResDetails, isLoading: loadingDetails } = useQuery({
    queryKey: ['reservation', selectedResId],
    queryFn: () => getReservationById(selectedResId!),
    enabled: !!selectedResId
  })

  const acceptMut = useMutation({
    mutationFn: (props: { userId: number, resId: number }) => acceptReservation(props.userId, props.resId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['reservations'] })
      queryClient.invalidateQueries({ queryKey: ['reservation', selectedResId] })
      setSelectedResId(null)
    }
  })

  const rejectMut = useMutation({
    mutationFn: (props: { userId: number, resId: number }) => rejectReservation(props.userId, props.resId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['reservations'] })
      queryClient.invalidateQueries({ queryKey: ['reservation', selectedResId] })
      setSelectedResId(null)
    }
  })

  const checkinMut = useMutation({
    mutationFn: (reservationId: number) => checkInReservation(reservationId),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ['reservations'] })
      queryClient.invalidateQueries({ queryKey: ['reservation', selectedResId] })
      setSelectedResId(null)
    }
  })

  const checkoutMut = useMutation({
    mutationFn: (reservationId: number) => checkOutReservation(reservationId),
    onSuccess: () => {
      setOpenInvoice(false)
      setInvoiceData(null)
      setSelectedResId(null)
      queryClient.invalidateQueries({ queryKey: ['reservations'] })
    }
  })

  const genInvMut = useMutation({
    mutationFn: (props: { reservationId: number, issuedBy: number }) => generateInvoice(props.reservationId, props.issuedBy),
    onSuccess: (data) => {
      setInvoiceData(data) // Eltesszük a visszakapott számla adatokat
      setOpenInvoice(true)
      queryClient.invalidateQueries({ queryKey: ['reservations'] })
      queryClient.invalidateQueries({ queryKey: ['reservation', selectedResId] })
    }
  })

  const getStatusColor = (status: string) => {
    const s = status?.toLowerCase?.() || ''
    if (s === 'pending' || s === 'requested') return 'warning'
    if (s === 'approved' || s === 'accepted') return 'success'
    if (s === 'checkedin' || s === 'inprogress') return 'info'
    if (s === 'checkedout' || s === 'completed') return 'primary'
    if (s === 'rejected' || s === 'denied' || s === 'cancelled') return 'error'
    return 'default'
  }

  const getUser = (userId: number) => users?.find(u => u.id === userId)

  const handleCloseInvoice = () => {
    setOpenInvoice(false)
    setTimeout(() => setInvoiceData(null), 300) 
  }

  return (
    <Box sx={{ p: { xs: 2, md: 4 } }}>
      <Typography variant="h4" fontWeight="bold" sx={{ mb: 3, textAlign: 'center' }}>Recepciós felület</Typography>
      
      <Box sx={{ width: '100%', maxWidth: { xs: '100%', md: 800, lg: 900 }, mx: 'auto' }}>
        <Paper sx={{ p: 2, borderRadius: 3, display: 'flex', flexDirection: 'column', gap: 2, minHeight: 400 }}>
          <Typography variant="h6" sx={{ mb: 1, textAlign: 'center' }}>Beérkező igények</Typography>
          
          {loadingRes || loadingUsers ? (
            <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}>
              <CircularProgress />
            </Box>
          ) : (
            <List sx={{ maxHeight: 680, overflow: 'auto', pt: 0 }}>
              {reservations?.map((r) => {
                const u = getUser(r.userId)
                const name = u?.fullName || `User #${r.userId}`
                const email = u?.email || 'Nincs megadva email'

                return (
                  <ListItem key={r.id} disablePadding>
                    <ListItemButton onClick={() => setSelectedResId(r.id)} sx={{ py: 2 }}>
                      <ListItemText 
                        primary={<Typography variant="subtitle1" fontWeight="bold">{`${name} — Szoba #${r.roomId}`}</Typography>} 
                        secondary={
                          <>
                            <Typography component="span" display="block" variant="body2" color="text.secondary">{email}</Typography>
                            <Typography component="span" display="block" variant="body2" color="text.secondary">{`${new Date(r.fromDate).toLocaleDateString()} → ${new Date(r.toDate).toLocaleDateString()}`}</Typography>
                          </>
                        } 
                      />
                      <Chip label={r.status} color={getStatusColor(r.status)} size="small" sx={{ textTransform: 'capitalize' }} />
                    </ListItemButton>
                  </ListItem>
                )
              })}
              {reservations?.length === 0 && (
                <Typography sx={{ textAlign: 'center', color: 'text.secondary', p: 3 }}>
                  Nincsenek beérkező igények.
                </Typography>
              )}
            </List>
          )}
        </Paper>
      </Box>

      {/* Booking detail dialog */}
      <Dialog open={Boolean(selectedResId)} onClose={() => setSelectedResId(null)} fullWidth maxWidth="lg" PaperProps={{ sx: { width: { xs: '95%', sm: '80%', md: 900 }, m: 'auto' } }}>
        {selectedResId && (
          <>
            <DialogContent>
              {loadingDetails ? (
                <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}><CircularProgress /></Box>
              ) : selectedResDetails ? (
                <Box sx={{ textAlign: 'center' }}>
                  <Typography variant="h6" color="text.primary" sx={{ mt: 2 }} >Vendég</Typography>
                  <Typography variant="h5" sx={{ mb: 2 }}>{getUser(selectedResDetails.userId)?.fullName} ({getUser(selectedResDetails.userId)?.email})</Typography>
                  
                  <Typography variant="h6" color="text.secondary">Szoba</Typography>
                  <Typography variant="h5" sx={{ mb: 2 }}>#{selectedResDetails.roomId}</Typography>
                  
                  <Typography variant="h6" color="text.secondary">Időszak</Typography>
                  <Typography variant="h5" sx={{ mb: 2 }}>
                    {new Date(selectedResDetails.fromDate).toLocaleDateString()} — {new Date(selectedResDetails.toDate).toLocaleDateString()}
                  </Typography>
                  
                  <Typography variant="h6" color="text.secondary">Státusz</Typography>
                  <Chip label={selectedResDetails.status} color={getStatusColor(selectedResDetails.status)} size="medium" sx={{ mb: 2, fontSize: '1.1rem', textTransform: 'capitalize' }} />
                </Box>
              ) : (
                <Typography textAlign="center">Részletek betöltése sikertelen.</Typography>
              )}
            </DialogContent>
            <DialogActions sx={{ justifyContent: 'center', pb: 4 }}>
              {['pending', 'requested', 'request', 'new'].includes(selectedResDetails?.status?.toLowerCase?.() || '') && (
                <>
                  <Button
                    onClick={() => acceptMut.mutate({ userId: selectedResDetails.userId, resId: selectedResDetails.id })}
                    variant="contained"
                    color="success"
                    size="large"
                    sx={{ px: 4 }}
                    disabled={acceptMut.isLoading || rejectMut.isLoading}
                  >
                    {acceptMut.isLoading ? 'Feldolgozás...' : 'Jóváhagy'}
                  </Button>
                  <Button
                    onClick={() => rejectMut.mutate({ userId: selectedResDetails.userId, resId: selectedResDetails.id })}
                    variant="outlined"
                    color="error"
                    size="large"
                    sx={{ px: 4 }}
                    disabled={acceptMut.isLoading || rejectMut.isLoading}
                  >
                    {rejectMut.isLoading ? 'Feldolgozás...' : 'Elutasít'}
                  </Button>
                </>
              )}
              {(selectedResDetails?.status?.toLowerCase() === 'approved' || selectedResDetails?.status?.toLowerCase() === 'accepted') && (
                <>
                  <Button
                    onClick={() => checkinMut.mutate(selectedResDetails.id)}
                    variant="contained"
                    color="primary"
                    size="large"
                    sx={{ px: 4 }}
                    disabled={acceptMut.isLoading || rejectMut.isLoading || checkinMut.isLoading}
                  >
                    {checkinMut.isLoading ? 'Feldolgozás...' : 'Check-in'}
                  </Button>
                </>
              )}
              {selectedResDetails?.status?.toLowerCase() === 'checkedin' && (
                <>
                  <Button
                    onClick={() => genInvMut.mutate({ reservationId: selectedResDetails.id, issuedBy: currentUserId })}
                    variant="contained"
                    color="primary"
                    size="large"
                    disabled={genInvMut.isLoading}
                  >
                    {genInvMut.isLoading ? 'Feldolgozás...' : 'Számla / Check-out'}
                  </Button>
                </>
              )}
              <Button onClick={() => setSelectedResId(null)} size="large" color="inherit">Bezár</Button>
            </DialogActions>
          </>
        )}
      </Dialog>

      {/* Invoice & Checkout Dialog */}
      <Dialog open={openInvoice} onClose={handleCloseInvoice} fullWidth maxWidth="sm">
        {/* JAVÍTOTT TYPOGRAPHY TAG */}
        <Typography sx={{ color: 'text.primary', textAlign: 'center', fontWeight: 'bold', fontSize: '1.5rem', pb: 1, pt: 3 }}>
          Végszámla
        </Typography>
        <DialogContent dividers>
          {invoiceData ? (
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
              <Box>
                <Typography variant="body2" color="text.secondary">Számlaszám: #{invoiceData.id}</Typography>
                <Typography variant="body2" color="text.secondary">Foglalás azonosító: #{invoiceData.reservationId}</Typography>
                <Typography variant="body2" color="text.secondary">
                  Kiállítva: {new Date(invoiceData.issuedAt).toLocaleString('hu-HU')}
                </Typography>
              </Box>
              
              <Divider />
              
              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Typography variant="body1">Szállás díja:</Typography>
                <Typography variant="body1" fontWeight="medium">{invoiceData.roomTotal.toLocaleString('hu-HU')} Ft</Typography>
              </Box>
              
              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                <Typography variant="body1">Extra szolgáltatások:</Typography>
                <Typography variant="body1" fontWeight="medium">{invoiceData.serviceTotal.toLocaleString('hu-HU')} Ft</Typography>
              </Box>
              
              <Divider />
              
              <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mt: 1 }}>
                <Typography variant="h6" fontWeight="bold">Fizetendő összeg:</Typography>
                <Typography variant="h5" fontWeight="bold" color="primary.main">
                  {invoiceData.grandTotal.toLocaleString('hu-HU')} Ft
                </Typography>
              </Box>
            </Box>
          ) : (
            <Box sx={{ display: 'flex', justifyContent: 'center', p: 4 }}>
              <CircularProgress />
            </Box>
          )}
        </DialogContent>
        <DialogActions sx={{ p: 2, px: 3, justifyContent: 'space-between' }}>
          <Button onClick={handleCloseInvoice} color="inherit">
            Vissza
          </Button>
          <Button 
            variant="contained" 
            color="success" 
            size="large"
            disabled={checkoutMut.isLoading || !invoiceData}
            onClick={() => checkoutMut.mutate(invoiceData.reservationId)}
          >
            {checkoutMut.isLoading ? 'Kijelentkezés...' : 'Fizetve & Check-out'}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  )
}