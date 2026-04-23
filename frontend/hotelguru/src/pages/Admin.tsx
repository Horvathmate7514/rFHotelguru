import React, { useState } from 'react';
import { 
  Box, Typography, Paper, Tabs, Tab, Button, Table, TableBody, 
  TableCell, TableContainer, TableHead, TableRow, IconButton, 
  Dialog, DialogTitle, DialogContent, DialogActions, TextField, 
  CircularProgress, Select, MenuItem, InputLabel, FormControl, Chip 
} from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import AddCircleIcon from '@mui/icons-material/AddCircle';
import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';

import { getAllRooms, createRoom, updateRoom, deleteRoom, addFacilityToRoom } from '../network/room.api';
import { getAllFacilities, createFacility, updateFacility, deleteFacility } from '../network/Facility.api';

// Tab panel segédkomponens
interface TabPanelProps {
  children?: React.ReactNode;
  index: number;
  value: number;
}

function CustomTabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;
  return (
    <div role="tabpanel" hidden={value !== index} {...other}>
      {value === index && <Box sx={{ pt: 3 }}>{children}</Box>}
    </div>
  );
}

export default function Admin() {
  const queryClient = useQueryClient();
  const [tabValue, setTabValue] = useState(0);

  const [roomDialog, setRoomDialog] = useState<{ open: boolean, mode: 'add' | 'edit', id: number | null, roomTypeId: number | '' }>({ open: false, mode: 'add', id: null, roomTypeId: '' });
  const [facilityDialog, setFacilityDialog] = useState<{ open: boolean, mode: 'add' | 'edit', id: number | null, name: string, price: number | '' }>({ open: false, mode: 'add', id: null, name: '', price: '' });
  const [assignDialog, setAssignDialog] = useState<{ open: boolean, roomId: number | null, facilityId: number | '' }>({ open: false, roomId: null, facilityId: '' });

  // --- LEKÉRDEZÉSEK ---
  const { data: rooms, isLoading: loadingRooms } = useQuery({ queryKey: ['rooms'], queryFn: getAllRooms });
  const { data: facilities, isLoading: loadingFacilities } = useQuery({ queryKey: ['facilities'], queryFn: getAllFacilities });

  // --- MUTÁCIÓK (SZOBÁK) ---
  const createRoomMut = useMutation({
    mutationFn: (typeId: number) => createRoom({ roomTypeId: typeId }),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['rooms'] }); closeRoomDialog(); }
  });
  
  const updateRoomMut = useMutation({
    mutationFn: (props: { id: number, typeId: number }) => updateRoom(props.id, { roomTypeId: props.typeId }),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['rooms'] }); closeRoomDialog(); }
  });
  
  const deleteRoomMut = useMutation({
    mutationFn: deleteRoom,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['rooms'] })
  });
  
  const assignFacilityMut = useMutation({
    mutationFn: (props: { roomId: number, facilityId: number }) => addFacilityToRoom(props.roomId, props.facilityId),
    onSuccess: () => { 
      queryClient.invalidateQueries({ queryKey: ['rooms'] }); 
      closeAssignDialog(); 
      alert("Felszereltség sikeresen hozzáadva!"); 
    },
    onError: (err) => {
      console.error(err);
      alert("Hiba történt a felszerelés hozzáadásakor.");
    }
  });

  // --- MUTÁCIÓK (FELSZERELTSÉG) ---
  const createFacMut = useMutation({
    mutationFn: (payload: { facilityName: string, price: number }) => createFacility(payload),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['facilities'] }); closeFacilityDialog(); }
  });
  
  const updateFacMut = useMutation({
    mutationFn: (props: { id: number, payload: { facilityName: string, price: number } }) => updateFacility(props.id, props.payload),
    onSuccess: () => { queryClient.invalidateQueries({ queryKey: ['facilities'] }); closeFacilityDialog(); }
  });
  
  const deleteFacMut = useMutation({
    mutationFn: deleteFacility,
    onSuccess: () => queryClient.invalidateQueries({ queryKey: ['facilities'] })
  });

  const handleTabChange = (event: React.SyntheticEvent, newValue: number) => setTabValue(newValue);

  const closeRoomDialog = () => setRoomDialog({ open: false, mode: 'add', id: null, roomTypeId: '' });
  const saveRoom = () => {
    if (roomDialog.roomTypeId === '') return;
    if (roomDialog.mode === 'add') createRoomMut.mutate(Number(roomDialog.roomTypeId));
    else if (roomDialog.id) updateRoomMut.mutate({ id: roomDialog.id, typeId: Number(roomDialog.roomTypeId) });
  };

  const closeFacilityDialog = () => setFacilityDialog({ open: false, mode: 'add', id: null, name: '', price: '' });
  const saveFacility = () => {
    if (!facilityDialog.name || facilityDialog.price === '') return;
    if (facilityDialog.mode === 'add') createFacMut.mutate({ facilityName: facilityDialog.name, price: Number(facilityDialog.price) });
    else if (facilityDialog.id) updateFacMut.mutate({ id: facilityDialog.id, payload: { facilityName: facilityDialog.name, price: Number(facilityDialog.price) } });
  };

  const closeAssignDialog = () => setAssignDialog({ open: false, roomId: null, facilityId: '' });
  const saveAssign = () => {
    if (assignDialog.roomId && assignDialog.facilityId !== '') {
      assignFacilityMut.mutate({ roomId: assignDialog.roomId, facilityId: Number(assignDialog.facilityId) });
    }
  };

  return (
    <Box sx={{ maxWidth: 1200, mx: 'auto', p: { xs: 2, md: 4 } }}>
      <Typography variant="h4" fontWeight="bold" sx={{ mb: 4, color: 'primary.main' }}>
        Adminisztrációs Vezérlőpult
      </Typography>

      <Paper sx={{ width: '100%', mb: 2, borderRadius: 3, overflow: 'hidden', boxShadow: 3 }}>
        <Box sx={{ borderBottom: 1, borderColor: 'divider', bgcolor: 'background.default' }}>
          <Tabs value={tabValue} onChange={handleTabChange} centered>
            <Tab label="Szobák Kezelése" sx={{ fontWeight: 'bold', fontSize: '1.1rem', py: 2 }} />
            <Tab label="Szolgáltatások / Felszereltség" sx={{ fontWeight: 'bold', fontSize: '1.1rem', py: 2 }} />
          </Tabs>
        </Box>

        <CustomTabPanel value={tabValue} index={0}>
          <Box sx={{ p: 3 }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
              <Typography variant="h6">Nyilvántartott szobák</Typography>
              <Button variant="contained" startIcon={<AddCircleIcon />} onClick={() => setRoomDialog({ open: true, mode: 'add', id: null, roomTypeId: '' })}>
                Új szoba
              </Button>
            </Box>

            {loadingRooms ? <Box textAlign="center" p={4}><CircularProgress /></Box> : (
              <TableContainer component={Paper} variant="outlined">
                <Table>
                  <TableHead sx={{ bgcolor: 'background.default' }}>
                    <TableRow>
                      <TableCell sx={{ fontWeight: 'bold' }}>ID</TableCell>
                      <TableCell sx={{ fontWeight: 'bold' }}>Szoba Típus (ID)</TableCell>
                      <TableCell align="right" sx={{ fontWeight: 'bold' }}>Műveletek</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {rooms?.map((room: any) => (
                      <TableRow key={room.id} hover>
                        <TableCell>#{room.id}</TableCell>
                        <TableCell>
                          <Chip label={`Típus: ${room.roomTypeId || 'N/A'}`} color="primary" variant="outlined" size="small" />
                        </TableCell>
                        <TableCell align="right">
                          <Button size="small" variant="outlined" sx={{ mr: 1 }} onClick={() => setAssignDialog({ open: true, roomId: room.id, facilityId: '' })}>
                            + Felszerelés
                          </Button>
                          <IconButton color="primary" onClick={() => setRoomDialog({ open: true, mode: 'edit', id: room.id, roomTypeId: room.roomTypeId || '' })}>
                            <EditIcon />
                          </IconButton>
                          <IconButton  onClick={() => { if (window.confirm('Biztosan törlöd a szobát?')) deleteRoomMut.mutate(room.id) }}>
                            <DeleteIcon />
                          </IconButton>
                        </TableCell>
                      </TableRow>
                    ))}
                    {rooms?.length === 0 && <TableRow><TableCell colSpan={3} align="center">Nincsenek rögzített szobák.</TableCell></TableRow>}
                  </TableBody>
                </Table>
              </TableContainer>
            )}
          </Box>
        </CustomTabPanel>

        <CustomTabPanel value={tabValue} index={1}>
          <Box sx={{ p: 3 }}>
            <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 3 }}>
              <Typography variant="h6">Szolgáltatások és Felszerelések</Typography>
              <Button variant="contained" color="primary" startIcon={<AddCircleIcon />} onClick={() => setFacilityDialog({ open: true, mode: 'add', id: null, name: '', price: '' })}>
                Új szolgáltatás
              </Button>
            </Box>

            {loadingFacilities ? <Box textAlign="center" p={4}><CircularProgress /></Box> : (
              <TableContainer component={Paper} variant="outlined">
                <Table>
                  <TableHead sx={{ bgcolor: 'background.default' }}>
                    <TableRow>
                      <TableCell sx={{ fontWeight: 'bold' }}>ID</TableCell>
                      <TableCell sx={{ fontWeight: 'bold' }}>Megnevezés</TableCell>
                      <TableCell sx={{ fontWeight: 'bold' }}>Ár (Ft)</TableCell>
                      <TableCell align="right" sx={{ fontWeight: 'bold' }}>Műveletek</TableCell>
                    </TableRow>
                  </TableHead>
                  <TableBody>
                    {facilities?.map((fac: any) => (
                      <TableRow key={fac.id} hover>
                        <TableCell>#{fac.id}</TableCell>
                        <TableCell fontWeight="medium">{fac.facilityName}</TableCell>
                        <TableCell>{fac.price?.toLocaleString()} Ft</TableCell>
                        <TableCell align="right">
                          <IconButton color="primary" onClick={() => setFacilityDialog({ open: true, mode: 'edit', id: fac.id, name: fac.facilityName, price: fac.price })}>
                            <EditIcon />
                          </IconButton>
                          <IconButton  onClick={() => { if (window.confirm('Biztosan törlöd?')) deleteFacMut.mutate(fac.id) }}>
                            <DeleteIcon />
                          </IconButton>
                        </TableCell>
                      </TableRow>
                    ))}
                    {facilities?.length === 0 && <TableRow><TableCell colSpan={4} align="center">Nincs rögzített szolgáltatás.</TableCell></TableRow>}
                  </TableBody>
                </Table>
              </TableContainer>
            )}
          </Box>
        </CustomTabPanel>
      </Paper>

      {/* DIALOG: Szoba rögzítés / szerkesztés */}
      <Dialog open={roomDialog.open} onClose={closeRoomDialog} fullWidth maxWidth="xs">
        <DialogTitle sx={{ bgcolor: 'primary.main', color: 'white' }}>{roomDialog.mode === 'add' ? 'Új szoba rögzítése' : 'Szoba szerkesztése'}</DialogTitle>
        <DialogContent dividers>
          <TextField
            fullWidth
            label="Szoba Típus ID (roomTypeId)"
            type="number"
            value={roomDialog.roomTypeId}
            onChange={(e) => setRoomDialog({ ...roomDialog, roomTypeId: Number(e.target.value) })}
            sx={{ mt: 1 }}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={closeRoomDialog}>Mégse</Button>
          <Button variant="contained" onClick={saveRoom} disabled={createRoomMut.isLoading || updateRoomMut.isLoading}>
            Mentés
          </Button>
        </DialogActions>
      </Dialog>

      <Dialog open={facilityDialog.open} onClose={closeFacilityDialog} fullWidth maxWidth="xs">
        <DialogTitle sx={{ bgcolor: 'primary.main', color: 'white' }}>{facilityDialog.mode === 'add' ? 'Új szolgáltatás' : 'Szolgáltatás szerkesztése'}</DialogTitle>
        <DialogContent dividers sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
          <TextField
            fullWidth
            label="Megnevezés (pl. Klíma, Masszázs)"
            value={facilityDialog.name}
            onChange={(e) => setFacilityDialog({ ...facilityDialog, name: e.target.value })}
            sx={{ mt: 1 }}
          />
          <TextField
            fullWidth
            label="Ár (Ft)"
            type="number"
            value={facilityDialog.price}
            onChange={(e) => setFacilityDialog({ ...facilityDialog, price: Number(e.target.value) })}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={closeFacilityDialog}>Mégse</Button>
          <Button variant="contained" onClick={saveFacility} disabled={createFacMut.isLoading || updateFacMut.isLoading}>
            Mentés
          </Button>
        </DialogActions>
      </Dialog>

      {/* DIALOG: Felszereltség hozzáadása a szobához */}
      <Dialog open={assignDialog.open} onClose={closeAssignDialog} fullWidth maxWidth="xs">
        <DialogTitle sx={{ bgcolor: 'primary.main', color: 'white' }}>Felszerelés hozzáadása (Szoba #{assignDialog.roomId})</DialogTitle>
        <DialogContent dividers>
          <FormControl fullWidth sx={{ mt: 1 }}>
            <InputLabel>Válassz szolgáltatást...</InputLabel>
            <Select
              value={assignDialog.facilityId}
              label="Válassz szolgáltatást..."
              onChange={(e) => setAssignDialog({ ...assignDialog, facilityId: e.target.value as number })}
            >
              {facilities?.map((f: any) => (
                <MenuItem key={f.id} value={f.id}>{f.facilityName} (+{f.price} Ft)</MenuItem>
              ))}
            </Select>
          </FormControl>
        </DialogContent>
        <DialogActions>
          <Button onClick={closeAssignDialog}>Mégse</Button>
          <Button variant="contained" onClick={saveAssign} disabled={assignFacilityMut.isLoading || assignDialog.facilityId === ''}>
            Hozzáad
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
}