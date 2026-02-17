import { SunInfoCard } from "./SunInfoCard";
import type { SunData } from "../types";

interface SunDataDisplayProps {
    data: SunData;
}

export function SunDataDisplay({ data }: SunDataDisplayProps) {
    return (
        <div className="bg-white dark:bg-gray-800 rounded-xl shadow-lg p-8 mb-8 transform transition-all animate-fade-in-up">
            <h2 className="text-2xl font-bold text-center text-gray-900 dark:text-white mb-6">Results</h2>
            <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                <SunInfoCard
                    label="Sunrise"
                    value={data.sunrise}
                    colorTheme="orange"
                    icon={
                        <svg xmlns="http://www.w3.org/2000/svg" className="h-10 w-10" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 3v1m0 16v1m9-9h-1M4 12H3m15.364 6.364l-.707-.707M6.343 6.343l-.707-.707m12.728 0l-.707.707M6.343 17.657l-.707.707M16 12a4 4 0 11-8 0 4 4 0 018 0z" />
                        </svg>
                    }
                />
                <SunInfoCard
                    label="Sunset"
                    value={data.sunset}
                    colorTheme="indigo"
                    icon={
                        <svg xmlns="http://www.w3.org/2000/svg" className="h-10 w-10" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M20.354 15.354A9 9 0 018.646 3.646 9.003 9.003 0 0012 21a9.003 9.003 0 008.354-5.646z" />
                        </svg>
                    }
                />
            </div>
        </div>
    );
}
