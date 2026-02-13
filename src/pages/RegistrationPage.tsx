import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import type { UserLogin } from "../types";
import { LoginForm } from "../components/LoginForm";
import { performLogin, performRegister } from "../api/auth";

export function RegistrationPage() {
    const [userRegister, setUserRegister] = useState<UserLogin | null>(null)
    const [error, setError] = useState("");
    const navitation = useNavigate();

    useEffect(() => {
        if (!userRegister) {
            return
        }
        performRegister(userRegister).then(() => {
            performLogin(userRegister).then(() => {
                navitation("/solar-watch");
            }).catch((err: Error) => {
                setError(err.message);
            })
        }).catch((err: Error) => {
            setError(err.message);
        })
    }, [userRegister, navitation])

    return (
        <>
            <h1>Register</h1>
            <LoginForm onRegistrered={(user) => setUserRegister(user)} />
            <a onClick={() => navitation("/login")}>Already have an account?</a>
            {error ? <p>{error}</p> : <></>}
        </>
    )
}