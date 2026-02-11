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
        <>
            <form onSubmit={handleSubmit}>
                <label htmlFor="search">Search For City</label>
                <input type="search" id="search" onChange={(event) =>
                    setCityName(event.target.value)}
                />
                <button type="submit">Search</button>
            </form>

        </>
    )
}