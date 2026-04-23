import React from 'react';
import { 
  Box, 
  Card, 
  CardMedia, 
  CardContent, 
  CardActions, 
  Typography, 
  Button, 
  Chip 
} from '@mui/material';

const DUMMY_ROOMS = [
  {
    id: 1,
    title: 'Standard Franciaágyas Szoba',
    description: 'Kényelmes szoba pároknak, városra néző kilátással és ingyenes wifivel.',
    price: 25000,
    capacity: 2,
    imageUrl: 'https://images.unsplash.com/photo-1611892440504-42a792e24d32?auto=format&fit=crop&w=600&q=80'
  },
  {
    id: 2,
    title: 'Prémium Családi Lakosztály',
    description: 'Tágas lakosztály két külön hálótérrel, tengerre néző erkéllyel és minibárral.',
    price: 55000,
    capacity: 4,
    imageUrl: 'https://images.unsplash.com/photo-1582719478250-c89cae4dc85b?auto=format&fit=crop&w=600&q=80'
  },
  {
    id: 3,
    title: 'Elnöki Lakosztály',
    description: 'A legmagasabb luxus. Privát jakuzzi, tágas nappali és 24 órás szobaszerviz.',
    price: 120000,
    capacity: 2,
    imageUrl: 'https://images.unsplash.com/photo-1631049307264-da0ec9d70304?auto=format&fit=crop&w=600&q=80'
  },
  {
    id: 4,
    title: 'Egyágyas Business Szoba',
    description: 'Ideális üzleti utazóknak. Csendes környezet, nagyméretű íróasztal és kávéfőző.',
    price: 20000,
    capacity: 1,
    imageUrl: 'https://images.unsplash.com/photo-1505691938895-1758d7feb511?auto=format&fit=crop&w=600&q=80'
  }
];

const RoomSection: React.FC = () => {
  return (
    <Box sx={{ p: { xs: 2, md: 4 }, maxWidth: 1200, margin: '0 auto' }}>
      <Typography variant="h4" component="h2" fontWeight="bold" textAlign="center" sx={{ mb: 4 }} color="primary">
        Foglalható Szobáink
      </Typography>

   
      <Box 
        sx={{ 
          display: 'flex', 
          flexWrap: 'wrap', 
          gap: 3, 
          justifyContent: 'center' 
        }}
      >
        {DUMMY_ROOMS.map((room) => (
          <Card 
            key={room.id} 
            sx={{ 
              display: 'flex',
              flexDirection: 'column',
           
              width: { 
                xs: '100%', 
                sm: 'calc(50% - 24px)', 
                md: 'calc(33.333% - 24px)' 
              },
              boxShadow: 3,
              transition: 'transform 0.2s',
              '&:hover': {
                transform: 'translateY(-5px)',
                boxShadow: 6,
              }
            }}
          >
            <CardMedia
              component="img"
              height="200"
              image={room.imageUrl}
              alt={room.title}
            />
            
            {/* FlexGrow: 1 kitolja az Action részt az aljára, ha eltérő a szöveghossz */}
            <CardContent sx={{ flexGrow: 1 }}>
              <Box display="flex" justifyContent="space-between" alignItems="flex-start" mb={2}>
                <Typography variant="h6" component="div" fontWeight="bold">
                  {room.title}
                </Typography>
                <Chip label={`${room.capacity} Fő`} size="small" color="primary" variant="outlined" />
              </Box>
              
              <Typography variant="body2" color="text.secondary" mb={2}>
                {room.description}
              </Typography>
              
              <Typography variant="h6" color="primary" fontWeight="bold">
                {room.price.toLocaleString('hu-HU')} Ft <Typography component="span" variant="body2" color="text.secondary">/ éj</Typography>
              </Typography>
            </CardContent>

            <CardActions sx={{ p: 2, pt: 0 }}>
              <Button variant="contained" fullWidth size="large">
                Lefoglalom
              </Button>
            </CardActions>
          </Card>
        ))}
      </Box>
    </Box>
  );
};

export default RoomSection;