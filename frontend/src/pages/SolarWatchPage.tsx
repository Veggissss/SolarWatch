import { SunDataForm } from "../components/SunDataForm";
import { SunDataDisplay } from "../components/SunDataDisplay";
import { LogoutButton } from "../components/LogoutButton";
import { fetchSunData } from "../api/fetchData";
import { useState } from "react";
import type { SunData } from "../types";
import { DisplayError } from "../components/DisplayError";
import { getRandomMoji } from "../services/kaoMoji";

export function SolarWatchPage() {
    const [sunData, setSunData] = useState<SunData | null>(null)
    const [error, setError] = useState("");

    const handleSearch = (cityName: string) => {
        if (!cityName || !cityName.trim()) {
            setError(`At least give me a city bro! ${getRandomMoji()}`);
            setSunData(null);
            return;
        }

        fetchSunData(cityName).then(data => {
            setSunData(data);
        }
        ).catch((err: Error) => {
            setError(`${err.message} ${getRandomMoji()}`);
        })
    }

    const handleError = (message: string) => {
        setError(message);
    }

    return (
        <div className="min-h-screen bg-gray-50 dark:bg-gray-900 py-12 px-4 sm:px-6 lg:px-8">
            <div className="max-w-3xl mx-auto">
                <div className="text-center mb-10">
                    <h1 className="text-4xl font-extrabold text-gray-900 dark:text-white mb-4">Solar Watch</h1>
                    <p className="text-lg text-gray-600 dark:text-gray-300">Check sunrise and sunset times for any city</p>
                </div>

                <div className="bg-white dark:bg-gray-800 rounded-xl shadow-lg p-6 mb-8">
                    <SunDataForm onSearch={handleSearch} onError={handleError} />
                </div>

                {sunData && <SunDataDisplay data={sunData} />}

                <div className="flex justify-center mt-12">
                    <LogoutButton />
                </div>
                <DisplayError error={error} />
            </div>
        </div>
    )
}