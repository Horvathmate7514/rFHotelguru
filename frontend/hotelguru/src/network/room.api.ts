    import axios from "axios";
import type { RoomType } from "./roomType.api";

    const API_BASE_URL = 'https://localhost:7133/api/Room'


    export type Room = {
        roomType: RoomType
    }





    export const getAllRooms = async () => {
        const res = await axios.get(`${API_BASE_URL}/GetAll`)
        return res.data
    }

    export const getRoomById = async (roomId: number) => {
        const res = await axios.get(`${API_BASE_URL}/GetById/${roomId}`)
        return res.data
    }

    export const createRoom = async (payload: { roomTypeId: number }) => {
        const res = await axios.post(`${API_BASE_URL}/Create`, payload)
        return res.data
    }

    export const updateRoom = async (roomId: number, payload: { roomTypeId: number }) => {
        const res = await axios.put(`${API_BASE_URL}/Update/${roomId}`, payload)
        return res.data
    }


    export const deleteRoom = async (roomId: number) => {
        const res = await axios.delete(`${API_BASE_URL}/Delete/${roomId}`)
        return res.data
    }



 export const addFacilityToRoom = async (roomId: number, facilityId: number) => {
  const data = {
    roomId: roomId,
    facilityId: facilityId
  };

  const res = await axios.put(`${API_BASE_URL}/AddFacility/${roomId}/${facilityId}`, data);
  return res.data;
}


    export const getAvailableRoomsInDateRange = async (startDate: string, endDate: string) => {
        const res = await axios.get(`${API_BASE_URL}/GetAvailableInDateRange/${startDate}/${endDate}`)
        return res.data
    }


