import axios from "axios";

const API_BASE_URL = 'https://localhost:7133/api/RoomType'


export type RoomType = {
    id: number
    bedNumber: number
    capacity: number
    basePrice: number
}

export const createRoomType = async (payload: { roomType: RoomType }) => {
    const res = await axios.post(`${API_BASE_URL}/Create`, payload)
    return res.data
}

export const getAllRoomTypes = async () => {
    const res = await axios.get(`${API_BASE_URL}/GetAll`)
    return res.data
}



export const updateRoomType = async (roomTypeId: number, payload: Partial<RoomType>) => {
    const res = await axios.put(`${API_BASE_URL}/Update/${roomTypeId}`, payload)
    return res.data
}



export const deleteRoomType = async (roomTypeId: number) => {
    const res = await axios.delete(`${API_BASE_URL}/Delete/${roomTypeId}`)
    return res.data
}