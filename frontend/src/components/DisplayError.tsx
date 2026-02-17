
export function DisplayError({ error }: { error: string }) {
    if (!error || !error.trim()) {
        return <></>
    }
    return (
        <div className="mt-4 p-3 bg-red-100 border border-red-400 text-red-700 rounded relative" role="alert">
            <span className="block sm:inline">{error}</span>
        </div>
    )
}