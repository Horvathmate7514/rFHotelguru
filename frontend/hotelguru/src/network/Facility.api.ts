import axios from "axios";

const API_BASE_URL = 'https://localhost:7133/api/Facility'


export type Facility = {
    id: number
    facilityName: string,
    price: number
}


export const getAllFacilities = async () => {
    const res = await axios.get(`${API_BASE_URL}/GetAll`)
    return res.data
}


export const getFacilityById = async (facilityId: number) => {
    const res = await axios.get(`${API_BASE_URL}/GetById/${facilityId}`)
    return res.data
}


export const createFacility = async (payload: { facilityName: string; price: number }) => {
    const res = await axios.post(`${API_BASE_URL}/Create`, payload)
    return res.data
}

export const updateFacility = async (facilityId: number, payload: Partial<{ facilityName: string; price: number }>) => {
    const res = await axios.put(`${API_BASE_URL}/Update/${facilityId}`, payload)
    return res.data
}

export const deleteFacility = async (facilityId: number) => {
    const res = await axios.delete(`${API_BASE_URL}/Delete/${facilityId}`)
    return res.data
}

