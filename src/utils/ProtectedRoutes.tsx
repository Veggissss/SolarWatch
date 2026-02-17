import { Outlet, useNavigate } from 'react-router'
import { useEffect } from 'react'
import { checkAuthorized, handleLogout } from '../api/auth'

export const ProtectedRoutes = () => {
    const token = localStorage.getItem('authToken')
    const navigate = useNavigate()

    useEffect(() => {
        const verifyToken = async () => {
            if (!token) {
                navigate('/login', { replace: true })
                return
            }

            const isAuthorized = await checkAuthorized();
            if (!isAuthorized) {
                console.error('Token verification failed!')
                handleLogout()
                navigate('/login', { replace: true })
            }
        }
        verifyToken()
    }, [token, navigate])

    return token ? <Outlet /> : null
}
