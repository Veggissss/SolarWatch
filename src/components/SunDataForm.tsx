import { useState } from "react"

interface SunDataFormProps {
    onSearch(cityName: string): void;
}

export function SunDataForm({ onSearch }: SunDataFormProps) {
    const [cityName, setCityName] = useState("");

    const handleSubmit = (event: React.SubmitEvent<HTMLFormElement>) => {
        event.preventDefault();
        onSearch(cityName);
    }

    return (
        <form onSubmit={handleSubmit} className="flex flex-col sm:flex-row gap-2 items-center justify-center max-w-lg mx-auto p-4">
            <label htmlFor="search" className="sr-only">Search For City</label>
            <input
                className="flex-1 w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none transition-all dark:bg-gray-700 dark:border-gray-600 dark:text-white"
                type="search"
                id="search"
                placeholder="Enter city name..."
                onChange={(event) =>
                    setCityName(event.target.value)}
            />
            <button
                type="submit"
                className="w-full sm:w-auto px-6 py-2 bg-indigo-600 hover:bg-indigo-700 text-white font-semibold rounded-lg shadow-md transition-colors cursor-pointer"
            >
                Search
            </button>
        </form>
    )
}