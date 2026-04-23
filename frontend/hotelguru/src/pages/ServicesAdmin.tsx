import React, { useState } from 'react'
import { Box, Button, Dialog, DialogTitle, DialogContent, DialogActions, TextField, IconButton, Table, TableHead, TableRow, TableCell, TableBody, CircularProgress, Typography, Paper, TableContainer } from '@mui/material'
import { useQuery, useQueryClient } from '@tanstack/react-query'
import { getAllServices, createService, updateService, deleteService } from '../network/benefit.api'
import EditIcon from '@mui/icons-material/Edit'
import DeleteIcon from '@mui/icons-material/Delete'

export default function ServicesAdmin() {
  const queryClient = useQueryClient()
  const { data: services, isLoading } = useQuery({
    queryKey: ['services'],
    queryFn: () => getAllServices(),
  })

  const [open, setOpen] = useState(false)   
  const [editing, setEditing] = useState<null | Service>(null)
  const [form, setForm] = useState({ type: '', price: 0 })

  const openCreate = () => {
    setEditing(null)
    setForm({ type: '', price: 0 })
    setOpen(true)
  }

  const openEdit = (svc: Service) => {
    setEditing(svc)
    setForm({ type: svc.type || '', price: svc.price || 0 })
    setOpen(true)
  }

  const handleSave = async () => {
    try {
      if (editing && editing.id) {
        await updateService(editing.id, { type: form.type, price: Number(form.price) })
      } else {
        await createService({ type: form.type, price: Number(form.price) })
      }
      await queryClient.invalidateQueries(['services'])
      setOpen(false)
    } catch (err) {
      console.error(err)
    }
  }

  const handleDelete = async (id?: number) => {
    if (!id) return
    if (!confirm('Törölni akarod ezt a service-t?')) return
    try {
      await deleteService(id)
      await queryClient.invalidateQueries(['services'])
    } catch (err) {
      console.error(err)
    }
  }

  return (
    <Box sx={{ p: 3 }}>
      <Box sx={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center', mb: 4, flexWrap: 'wrap', gap: 2 }}>
        <Typography variant="h4" fontWeight="bold">Servicek kezelése</Typography>
        <Button variant="contained" onClick={openCreate} sx={{ borderRadius: 2 }}>Új service</Button>
      </Box>

      {isLoading ? (
        <Box sx={{ display: 'flex', justifyContent: 'center', py: 8 }}><CircularProgress /></Box>
      ) : (
        <TableContainer component={Paper} sx={{ borderRadius: 3, overflowX: 'auto', boxShadow: '0 4px 6px -1px rgb(0 0 0 / 0.1)' }}>
          <Table sx={{ minWidth: 600 }}>
            <TableHead sx={{ bgcolor: 'background.default' }}>
              <TableRow>
                <TableCell sx={{ fontWeight: 'bold' }}>ID</TableCell>
                <TableCell sx={{ fontWeight: 'bold' }}>Típus</TableCell>
                <TableCell sx={{ fontWeight: 'bold' }}>Ár</TableCell>
                <TableCell align="right" sx={{ fontWeight: 'bold' }}>Műveletek</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {services?.map((s: Service) => (
                <TableRow key={s.id} hover>
                  <TableCell>{s.id}</TableCell>
                  <TableCell>{s.type}</TableCell>
                  <TableCell>{s.price ? `${s.price.toLocaleString()} Ft` : '-'}</TableCell>
                  <TableCell align="right">
                    <IconButton onClick={() => openEdit(s)} size="small" color="primary" sx={{ mr: 1, bgcolor: 'primary.light', color: 'primary.main', '&:hover': { bgcolor: 'primary.main', color: 'white' } }}><EditIcon fontSize="small" /></IconButton>
                    <IconButton onClick={() => handleDelete(s.id)} size="small"  sx={{ bgcolor: 'error.light', color: 'error.main', '&:hover': { bgcolor: 'error.main', color: 'white' } }}><DeleteIcon fontSize="small" /></IconButton>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      )}

      <Dialog
        open={open}
        onClose={() => setOpen(false)}
        fullWidth
        maxWidth="sm"
        PaperProps={{ sx: { borderRadius: 3, width: { xs: '95%', sm: '80%', md: 600 }, maxWidth: '100%' } }}
      >
        <DialogTitle sx={{ fontWeight: 'bold', pb: 1 }}>{editing ? 'Service szerkesztése' : 'Új service hozzáadása'}</DialogTitle>
        <DialogContent sx={{ p: { xs: 2, sm: 3 } }}>
          <Box sx={{ display: 'flex', flexDirection: 'column', gap: 2, mt: 1 }}>
            <TextField label="Típus (pl. Wellness, Reggeli)" value={form.type} onChange={(e) => setForm(f => ({ ...f, type: e.target.value }))} fullWidth variant="outlined" />
            <TextField label="Ár (Ft)" type="number" value={form.price || ''} onChange={(e) => setForm(f => ({ ...f, price: Number(e.target.value) }))} fullWidth variant="outlined" />
          </Box>
        </DialogContent>
        <DialogActions sx={{ p: { xs: 2, sm: 3 }, pt: 1, justifyContent: 'flex-end' }}>
          <Button onClick={() => setOpen(false)} sx={{ color: 'text.secondary' }}>Mégse</Button>
          <Button variant="contained" onClick={handleSave} sx={{ borderRadius: 2 }}>Mentés</Button>
        </DialogActions>
      </Dialog>
    </Box>
  )
}
