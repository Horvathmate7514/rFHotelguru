import React, { createContext, useContext, useState, useCallback } from 'react'
import type { ReactNode } from 'react'

type UserContextValue = {
  userId: number | null
  setUserId: (id: number | null) => void
}

const UserContext = createContext<UserContextValue | undefined>(undefined)

export function useUser() {
  const c = useContext(UserContext)
  if (!c) throw new Error('useUser must be used within UserProvider')
  return c
}

export function UserProvider({ children }: { children: ReactNode }) {
  const [userId, setUserId] = useState<number | null>(null)

  const value = {
    userId,
    setUserId: useCallback((id: number | null) => setUserId(id), []),
  }

  return <UserContext.Provider value={value}>{children}</UserContext.Provider>
}
