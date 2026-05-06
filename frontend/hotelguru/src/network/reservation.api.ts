import axios from "axios";

const API_BASE_URL = 'https://localhost:7133/api/Reservation'



export type Reservation = {
    id: number
    roomId: number
    userId: number
    fromDate: Date
    toDate: Date
    checkInDate: Date
    checkOutDate: Date
    status: string
  reservationBenefits?: { benefitId: number, quantity: number }[]
}

export type ReservationBenefit = {
    benefitId: number
    quantity: number
}


export const getAllReservations = async () => {
    const res = await axios.get(`${API_BASE_URL}/GetAll`)
    return res.data
}


export const getReservationByUserId = async (userId: number) => {
    const res = await axios.get(`${API_BASE_URL}/ListByUserID/${userId}`)
    return res.data
}

export const getReservationById = async (reservationId: number) => {
    const res = await axios.get(`${API_BASE_URL}/InfoByID/${reservationId}`)
    return res.data
}

export const createReservation = async (payload: { reservation: Reservation }) => {
  const raw = { ...(payload.reservation || {}) } as Reservation

  raw.reservationBenefits = raw.reservationBenefits ?? []

  const body = raw.id === 0 ? (({ id, ...rest }) => rest)(raw) : raw

  const res = await axios.post(`${API_BASE_URL}/Create`, body)
  return res.data
}



export const acceptReservation = async (userId: number, reservationId: number) => {
    const res = await axios.post(`${API_BASE_URL}/RequestAccept/${userId}/${reservationId}`)
    return res.data
}

export const rejectReservation = async (userId: number, reservationId: number) => {
    const res = await axios.post(`${API_BASE_URL}/RequestDeny/${userId}/${reservationId}`)
    return res.data
}


export const cancelReservation = async (userId: number, reservationId: number) => {
    const res = await axios.put(`${API_BASE_URL}/Cancel`)
    return res.data
}

export const generateInvoice = async (reservationId: number, issuedBy: number) => {
  const res = await axios.post(
    `${API_BASE_URL}/GenerateInvoice/${reservationId}`, 
    issuedBy, 
    {
      headers: {
        'Content-Type': 'application/json' 
      }
    }
  )
  return res.data
}

export const checkInReservation = async (reservationId: number) => {
  
  const data = {
    reservationId: reservationId 
  };

  const res = await axios.put(`${API_BASE_URL}/CheckIn/check-in`, data)
  return res.data
}


export const checkOutReservation = async (reservationId: number) => {
  
  const data = {
    reservationId: reservationId 
  };

  const res = await axios.put(`${API_BASE_URL}/CheckOut/check-out`, data)
  return res.data
}


export const addServiceToReservation = async (reservationId: number, benefitId: number, quantity: number) => {
  const data = {
    reservationId,
    benefitId,
    quantity
  };
  const res = await axios.put(`${API_BASE_URL}/AddService`, data)
  return res.data
}
