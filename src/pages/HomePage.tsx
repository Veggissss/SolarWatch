import { useNavigate } from "react-router"

export function HomePage() {
    const navigate = useNavigate()
    navigate('/solar-watch', { replace: true })

    return (
        <>
            <h1>Begone!</h1>
        </>
    )
}
