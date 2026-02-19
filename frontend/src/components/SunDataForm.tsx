import React, { useRef, useState } from "react"
import { fetchCities } from "../api/fetchData";
import type { City } from "../types";

interface SunDataFormProps {
    onSearch(cityName: string): void;
    onError(message: string): void;
}

export function SunDataForm({ onSearch, onError }: SunDataFormProps) {
    const searchInputElement = useRef(null);
    const [cityName, setCityName] = useState("");
    const [cities, setCities] = useState<City[]>([]);

    const handleSubmit = (event: React.SubmitEvent<HTMLFormElement>) => {
        event.preventDefault();
        onSearch(cityName);
    }

    const handleSuggestionClick = (event: React.MouseEvent<HTMLLIElement>) => {
        const cityName = (event.target as HTMLLIElement).textContent;
        setCityName(cityName);
        if (!searchInputElement.current) {
            return;
        }
        (searchInputElement.current as HTMLInputElement).value = cityName;
        setCities([]);
    }

    const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const newCityName = event.target.value;
        setCityName(newCityName);
        if (!newCityName || !newCityName.trim()) {
            setCities([]);
            return;
        }

        fetchCities().then(cities => {
            const matchedSearchCitites = cities.filter(city => city.name.toLowerCase().includes(newCityName.toLowerCase()));
            const uniqueCities = matchedSearchCitites.filter((city, index, self) => self.findIndex(c => c.name === city.name) === index);
            setCities(uniqueCities.splice(0, 5));
        }).catch((err: Error) => {
            onError(err.message);
        });
    }

    return (
        <form onSubmit={handleSubmit} className="flex flex-col sm:flex-row gap-2 items-center justify-center max-w-lg mx-auto p-4">
            <div className="flex gap-2 w-full">
                <div className="flex-1 relative">
                    <input
                        className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none transition-all dark:bg-gray-700 dark:border-gray-600 dark:text-white"
                        type="search"
                        id="search"
                        placeholder="Enter city name..."
                        onChange={handleInputChange}
                        ref={searchInputElement}
                    />

                    {cities.length > 0 && <ol className="absolute w-full border bg-white dark:bg-gray-700  dark:text-white z-10 px-4 py-2 border-gray-300 rounded-lg outline-none transition-all">
                        {cities.map(city => (<li onClick={(event) => handleSuggestionClick(event)} className="hover:text-indigo-500 cursor-pointer" key={city.name}>{city.name}</li>))}
                    </ol>}
                </div>
                <button
                    type="submit"
                    className="h-fit px-6 py-2 bg-indigo-600 hover:bg-indigo-700 text-white font-semibold rounded-lg shadow-md transition-colors cursor-pointer"
                >
                    Search
                </button>
            </div>
        </form>
    )
}