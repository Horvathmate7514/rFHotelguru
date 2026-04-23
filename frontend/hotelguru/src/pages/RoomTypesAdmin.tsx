import React, { useState } from 'react'
import { Box, Button, Dialog, DialogTitle, DialogContent, DialogActions, TextField, Table, TableHead, TableRow, TableCell, TableBody, IconButton, Typography, Paper, TableContainer } from '@mui/material'
import DeleteIcon from '@mui/icons-material/Delete'
import AddIcon from '@mui/icons-material/Add'
import HotelIcon from '@mui/icons-material/Hotel'
import { createRoomType } from '../network/roomType.api'

type RoomType = { id: number; name: string; capacity?: number }

const STORAGE_KEY = 'roomTypes'

function readTypes(): RoomType[] {
  try {
    return JSON.parse(localStorage.getItem(STORAGE_KEY) || '[]')
  } catch {
    return []
  }
}

function writeTypes(items: RoomType[]) {
  localStorage.setItem(STORAGE_KEY, JSON.stringify(items))
}

export default function RoomTypesAdmin() {
  const [items, setItems] = useState<RoomType[]>(() => readTypes())
  const [open, setOpen] = useState(false)
  const [name, setName] = useState('')
  const [capacity, setCapacity] = useState<number | ''>('')

  const handleCreate = async () => {
    const next: RoomType = { id: Date.now(), name, capacity: capacity === '' ? undefined : Number(capacity) }
    try {
      await createRoomType({ roomType: { id: 0, bedNumber: 0, capacity: next.capacity ?? 0, basePrice: 0 } })
    } catch (err) {
      console.error('createRoomType failed', err)
    }
    const updated = [...items, next]
    writeTypes(updated)
    setItems(updated)
    setOpen(false)
    setName('')
    setCapacity('')
  }

  const handleDelete = (id: number) => {
    if (!confirm('Törölni akarod?')) return
    const updated = items.filter((i) => i.id !== id)
    writeTypes(updated)
    setItems(updated)
  }

  return (
    <Box sx={{ p: { xs: 2, md: 4 } }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 4 }}>
        <Box sx={{ display: 'flex', alignItems: 'center', gap: 2 }}>
          <Box sx={{ p: 1.5, bgcolor: 'primary.main', borderRadius: 2, color: 'white', display: 'flex' }}>
            <HotelIcon />
          </Box>
          <Typography variant="h4" fontWeight="bold">Szobatípusok</Typography>
        </Box>
        <Button variant="contained" startIcon={<AddIcon />} onClick={() => setOpen(true)} sx={{ borderRadius: 2 }}>
          Új típus
        </Button>
      </Box>

      <TableContainer component={Paper} sx={{ borderRadius: 3, overflowX: 'auto', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)' }}>
        <Table sx={{ minWidth: 600 }}>
          <TableHead sx={{ bgcolor: 'background.default' }}>
            <TableRow>
              <TableCell sx={{ fontWeight: 'bold' }}>ID</TableCell>
              <TableCell sx={{ fontWeight: 'bold' }}>Név</TableCell>
              <TableCell sx={{ fontWeight: 'bold' }}>Kapacitás</TableCell>
              <TableCell align="right" sx={{ fontWeight: 'bold' }}>Műveletek</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {items.length === 0 ? (
              <TableRow>
                <TableCell colSpan={4} align="center" sx={{ py: 4, color: 'text.secondary' }}>Nincs megjeleníthető szobatípus</TableCell>
              </TableRow>
            ) : (
              items.map((t) => (
                <TableRow key={t.id} hover>
                  <TableCell>{t.id}</TableCell>
                  <TableCell sx={{ fontWeight: 'medium' }}>{t.name}</TableCell>
                  <TableCell>{t.capacity ? `${t.capacity} fő` : '-'}</TableCell>
                  <TableCell align="right">
                    <IconButton onClick={() => handleDelete(t.id)}  size="small" sx={{ bgcolor: 'error.light', color: 'error.main', '&:hover': { bgcolor: 'error.main', color: 'white' } }}>
                      <DeleteIcon fontSize="small" />
                    </IconButton>
                  </TableCell>
                </TableRow>
              ))
            )}
          </TableBody>
        </Table>
      </TableContainer>

      <Dialog
        open={open}
        onClose={() => setOpen(false)}
        fullWidth
        maxWidth="sm"
        PaperProps={{ sx: { borderRadius: 3, width: { xs: '96%', sm: '84%', md: 640 }, maxWidth: '100%' } }}
      >
        <DialogTitle sx={{ fontWeight: 'bold', pb: 1 }}>Új szobatípus hozzáadása</DialogTitle>
        <DialogContent sx={{ p: { xs: 2, sm: 3 } }}>
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
            <TextField label="Név (pl. Deluxe, Standard)" value={name} onChange={(e) => setName(e.target.value)} fullWidth variant="outlined" />
            <TextField label="Kapacitás (fő)" value={capacity} onChange={(e) => setCapacity(e.target.value === '' ? '' : Number(e.target.value))} type="number" fullWidth variant="outlined" />
          </Box>
        </DialogContent>
        <DialogActions sx={{ p: { xs: 2, sm: 3 }, pt: 1 }}>
          <Button onClick={() => setOpen(false)} sx={{ color: 'text.secondary' }}>Mégse</Button>
          <Button variant="contained" onClick={handleCreate} disabled={!name} sx={{ borderRadius: 2 }}>Hozzáadás</Button>
        </DialogActions>
      </Dialog>
    </Box>
  )
}
