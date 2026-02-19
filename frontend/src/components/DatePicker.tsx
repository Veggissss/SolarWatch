import { useRef } from "react"

interface DatePickerProps {
    onSubmitDate(date: string): void;
}

export function DatePicker({ onSubmitDate }: DatePickerProps) {
    const datePickerRef = useRef<null | HTMLInputElement>(null);
    const displayDateRef = useRef<null | HTMLInputElement>(null);

    const handleDateSubmit = (event: React.ChangeEvent<HTMLInputElement, HTMLInputElement>) => {
        const value = event.target.value;
        if (!value || !displayDateRef.current) {
            return;
        }
        const [year, month, day] = value.split("-");
        const date = `${year}-${month}-${day}`;
        displayDateRef.current.value = date;
        onSubmitDate(date);
    }

    const handleFakeClick = () => {
        if (!datePickerRef.current) {
            return;
        }
        datePickerRef.current.showPicker();
    }


    return (
        <>
            <div className="flex justify-center">
                <div className="relative">
                    <input
                        id="date-picker"
                        type="date"
                        defaultValue={new Date().toISOString().split('T')[0]}
                        onChange={handleDateSubmit}
                        ref={datePickerRef}
                        className="absolute inset-0 opacity-0 pointer-events-none"
                    />
                    <input
                        id="display-date"
                        type="text"
                        ref={displayDateRef}
                        placeholder="yyyy-mm-dd"
                        onClick={handleFakeClick}
                        readOnly
                        className="px-4 py-2 border border-gray-300 dark:border-gray-600 rounded-lg bg-white dark:bg-gray-700 text-gray-900 dark:text-white focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                    />
                </div>
            </div>
        </>
    )
}