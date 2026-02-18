import { useState } from "react";
import type { UserLogin } from "../types";

interface AuthFormProps {
    onRegistrered(user: UserLogin): void;
    onError(message: string): void;
}

export function AuthForm({ onRegistrered, onError }: AuthFormProps) {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const handleSubmit = (event: React.SubmitEvent<HTMLFormElement>) => {
        event.preventDefault();

        const validations = [
            { condition: !username.trim(), message: "Username is required" },
            { condition: !password || password.length < 6, message: "Password must be at least 6 characters" },
        ];

        for (const validation of validations) {
            if (validation.condition) {
                onError(validation.message);
                return;
            }
        }
        onRegistrered({ username, password } as UserLogin);
    }

    return (
        <form onSubmit={handleSubmit} className="flex flex-col gap-4 w-full max-w-sm mx-auto">
            <input
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none transition-all dark:bg-gray-700 dark:border-gray-600 dark:text-white"
                type="text"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
            />
            <input
                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-indigo-500 focus:border-indigo-500 outline-none transition-all dark:bg-gray-700 dark:border-gray-600 dark:text-white"
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <button
                type="submit"
                className="w-full py-2 px-4 bg-indigo-600 hover:bg-indigo-700 text-white font-semibold rounded-lg shadow-md focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-opacity-75 transition-colors cursor-pointer"
            >
                Login
            </button>
        </form>
    )
}