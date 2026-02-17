import { useEffect } from "react"
import { useNavigate } from "react-router"

export function HomePage() {
    const navigate = useNavigate()

    useEffect(() => {
        navigate('/solar-watch', { replace: true })
    }, [navigate])

    return (
        <>
            { /* Redirect to solar-watch */}
        </>
    )
}
