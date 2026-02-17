import type { ReactNode } from "react";

interface SunInfoCardProps {
    label: string;
    value: string;
    icon: ReactNode;
    colorTheme: 'orange' | 'indigo';
}

export function SunInfoCard({ label, value, icon, colorTheme }: SunInfoCardProps) {
    const themeClasses = {
        orange: {
            container: "bg-orange-50 dark:bg-orange-900/20 border-orange-100 dark:border-orange-800",
            icon: "text-orange-500 dark:text-orange-400",
        },
        indigo: {
            container: "bg-indigo-50 dark:bg-indigo-900/20 border-indigo-100 dark:border-indigo-800",
            icon: "text-indigo-500 dark:text-indigo-400",
        }
    };

    const theme = themeClasses[colorTheme];

    return (
        <div className={`p-6 rounded-lg border flex flex-col items-center ${theme.container}`}>
            <span className={`mb-2 ${theme.icon}`}>
                {icon}
            </span>
            <span className="text-sm uppercase tracking-wide text-gray-500 dark:text-gray-400 font-semibold">{label}</span>
            <span className="text-2xl font-bold text-gray-800 dark:text-white mt-1">{value}</span>
        </div>
    );
}
