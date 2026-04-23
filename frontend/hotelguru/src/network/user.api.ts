import axios from "axios";

const API_BASE_URL = 'https://localhost:7133/api/User'


export type Address = {
  postCode: string
  country: string
  city: string
  street: string
  houseNumber: string
}

export type SignupPayload = {
  fullName: string
  email: string
  password: string
  mobile?: string
  address?: Address
}

export type LoginPayload = {
  email: string
  password: string
}


export type User = {
  id: number
  fullName: string
  email: string
  mobile?: string
  address?: Address
}


export const signupUser = async (payload: SignupPayload) => {
  const res = await axios.post(`${API_BASE_URL}/Register`, payload)
  return res.data
}

export const loginUser = async (payload: LoginPayload) => {
  const res = await axios.post(`${API_BASE_URL}/Login`, payload)
  return res.data
}



export const getUserById = async (userId: number) => {
  const res = await axios.get(`${API_BASE_URL}/GetProfile/${userId}`)
  return res.data
}


export const updateUser = async (userId: number, payload: Partial<User>) => {
  const res = await axios.put(`${API_BASE_URL}/Update/${userId}`, payload)
  return res.data
}


export const getAllUsers = async () => {
  const res = await axios.get(`${API_BASE_URL}/GetAll`)
  return res.data
}

