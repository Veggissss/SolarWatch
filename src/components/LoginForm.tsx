import { useState } from "react";
import type { UserLogin } from "../types";

interface LoginProps {
    onRegistrered(user: UserLogin): void;
}

export function LoginForm({ onRegistrered }: LoginProps) {
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
                alert(validation.message);
                return;
            }
        }
        onRegistrered({ username, password } as UserLogin);
    }

    return (
        <form onSubmit={handleSubmit}>
            <input
                type="text"
                placeholder="Username"
                value={username}
                onChange={(e) => setUsername(e.target.value)}
            />
            <input
                type="password"
                placeholder="Password"
                value={password}
                onChange={(e) => setPassword(e.target.value)}
            />
            <button type="submit">Login</button>
        </form>
    )
}