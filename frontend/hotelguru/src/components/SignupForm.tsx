import { useState } from 'react';
import type { ChangeEvent, FormEvent } from 'react'; 
import { Box, Button, TextField, Typography, Paper, Alert } from '@mui/material';
import axios from 'axios';
import { signupUser } from '../network/user.api';
import type { SignupPayload, } from '../network/user.api'; 


type Props = {
  onClose?: () => void;
};

const SignupForm = ({ onClose }: Props) => {
  const [formData, setFormData] = useState<SignupPayload>({
    fullName: '',
    email: '',
    password: '',
    mobile: '',
    address: {
      postCode: '',
      country: '',
      city: '',
      street: '',
      houseNumber: ''
    }
  });

  const [error, setError] = useState<string | null>(null);

  const handleInputChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev: SignupPayload) => ({ ...prev, [name]: value }));
  };

  const handleAddressChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    setFormData((prev: SignupPayload) => ({
      ...prev,
      address: {
        postCode: prev.address?.postCode || '',
        country: prev.address?.country || '',
        city: prev.address?.city || '',
        street: prev.address?.street || '',
        houseNumber: prev.address?.houseNumber || '',
        [name]: value
      }
    }));
  };

  const handleSubmit = async (e: FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setError(null);

    try {
      await signupUser(formData);
      alert('Sikeres regisztráció!');
      if (onClose) onClose();
    } catch (err: unknown) {
      if (axios.isAxiosError(err)) {
        setError(err.response?.data?.message || 'Hiba a regisztráció során');
      } else if (err instanceof Error) {
        setError(err.message);
      } else {
        setError('Ismeretlen hiba történt');
      }
    }
  };

  return (
    <Box sx={{ display: 'flex', justifyContent: 'center', mt: 4 }}>
      <Paper sx={{ p: 4, maxWidth: 600, width: '100%' }}>
        <Typography variant="h5" sx={{ mb: 3, color: 'primary.main' }}>Regisztráció</Typography>

        {error && <Alert severity="error" sx={{ mb: 3 }}>{error}</Alert>}

        <form 
          onSubmit={handleSubmit} 
          noValidate 
          style={{ display: 'flex', flexDirection: 'column', gap: '16px' }}
        >
          <TextField fullWidth required label="Teljes név" name="fullName" value={formData.fullName} onChange={handleInputChange} />
          
          <Box sx={{ display: 'flex', gap: 2, flexDirection: { xs: 'column', sm: 'row' } }}>
            <TextField fullWidth required label="Email" name="email" type="email" value={formData.email} onChange={handleInputChange} />
            <TextField fullWidth required label="Jelszó" name="password" type="password" value={formData.password} onChange={handleInputChange} />
          </Box>
          
          <TextField fullWidth required label="Telefonszám" name="mobile" value={formData.mobile} onChange={handleInputChange} />

          <Typography variant="subtitle1" sx={{ mt: 1, mb: -1 }}>Cím adatok</Typography>
          
          <Box sx={{ display: 'flex', gap: 2, flexDirection: { xs: 'column', sm: 'row' } }}>
            <TextField fullWidth required label="Ország" name="country" value={formData.address?.country || ''} onChange={handleAddressChange} />
            <TextField fullWidth required label="Irányítószám" name="postCode" value={formData.address?.postCode || ''} onChange={handleAddressChange} />
          </Box>
          
          <Box sx={{ display: 'flex', gap: 2, flexDirection: { xs: 'column', sm: 'row' } }}>
            <TextField fullWidth required label="Város" name="city" value={formData.address?.city || ''} onChange={handleAddressChange} sx={{ flex: 1 }} />
            <TextField fullWidth required label="Utca" name="street" value={formData.address?.street || ''} onChange={handleAddressChange} sx={{ flex: 2 }} />
            <TextField fullWidth required label="Házszám" name="houseNumber" value={formData.address?.houseNumber || ''} onChange={handleAddressChange} sx={{ flex: 1 }} />
          </Box>

          <Button type="submit" fullWidth variant="contained" size="large" sx={{ mt: 2 }}>
            Regisztráció
          </Button>
        </form>
      </Paper>
    </Box>
  );
};

export default SignupForm;