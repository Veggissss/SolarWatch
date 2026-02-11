import { useNavigate } from "react-router";
import { SunDataForm } from "../components/SunDataForm";
import { fetchSunData } from "../api/fetchData";
import { useState } from "react";
import type { SunData } from "../types";

export function SolarWatchPage() {
    const navigate = useNavigate();
    const [sunData, setSunData] = useState<SunData | null>(null)

    const handleSearch = (cityName: string) => {
        fetchSunData(cityName).then(data => {
            setSunData(data);
        }
        ).catch(() => {
            console.log("Could not get sundata");
        })
    }
    return (
        <>
            <h1>Solar Watch</h1>
            <SunDataForm onSearch={handleSearch} />
            <br />
            {sunData ? <>
                <p>Sunrise: {sunData.sunrise}</p>
                <p>Sunset: {sunData.sunset}</p>
            </> : <></>}

            <div style={{ paddingTop: 100 }}>
                <button onClick={() => {
                    sessionStorage.removeItem("authToken");
                    navigate('/login', { replace: true })
                }}>Log out!</button>
            </div>
        </>
    )
}