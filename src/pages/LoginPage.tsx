import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import type { UserLogin } from "../types";
import { LoginForm } from "../components/LoginForm";
import { performLogin } from "../api/auth";

export function LoginPage() {
    const [userLogin, setUserLogin] = useState<UserLogin | null>(null)
    const navitation = useNavigate();

    useEffect(() => {
        if (!userLogin) {
            return
        }
        performLogin(userLogin).then(() => {
            navitation("/solar-watch");
        })
    }, [userLogin, navitation])

    return (
        <>
            <h1>Login</h1>
            <LoginForm onRegistrered={(user) => setUserLogin(user)} />
        </>
    )
}