import axios from "axios";

const API_BASE_URL = 'https://localhost:7133/api/Benefit'

export type Service = {
  id: number
  type: string
  price: number
}

export const getAllServices = async () => {
  const res = await axios.get(`${API_BASE_URL}/GetAll`)
  return res.data
}

export const getServiceById = async (serviceId: number) => {
  const res = await axios.get(`${API_BASE_URL}/GetById/${serviceId}`)
  return res.data
}


export const createService = async (payload: { type: string; price: number }) => {
  const res = await axios.post(`${API_BASE_URL}/Create`, payload)
  return res.data
}


export const updateService = async (serviceId: number, payload: Partial<{ type: string; price: number }>) => {
  const res = await axios.put(`${API_BASE_URL}/Update/${serviceId}`, payload)
  return res.data
}

export const deleteService = async (serviceId: number) => {
  const res = await axios.delete(`${API_BASE_URL}/Delete/${serviceId}`)
  return res.data
}