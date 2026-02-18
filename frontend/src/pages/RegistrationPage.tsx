import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import type { UserLogin } from "../types";
import { AuthForm } from "../components/AuthForm";
import { performLogin, performRegister } from "../api/auth";
import { DisplayError } from "../components/DisplayError";
import { getRandomMoji } from "../services/kaoMoji";

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
                setError(`${err.message} ${getRandomMoji()}`);
            })
        }).catch((err: Error) => {
            setError(`${err.message} ${getRandomMoji()}`);
        })
    }, [userRegister, navitation])

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-900 py-12 px-4 sm:px-6 lg:px-8">
            <div className="max-w-md w-full space-y-8 bg-white dark:bg-gray-800 p-8 rounded-xl shadow-lg">
                <div className="text-center">
                    <h1 className="text-3xl font-extrabold text-gray-900 dark:text-white mb-2">Register</h1>
                    <p className="text-sm text-gray-600 dark:text-gray-400">Create a new account</p>
                </div>

                <div className="mt-8">
                    <AuthForm onRegistrered={(user) => setUserRegister(user)} onError={(message) => setError(message)} />
                </div>

                <div className="text-center mt-4">
                    <button
                        onClick={() => navitation("/login")}
                        className="text-indigo-600 hover:text-indigo-500 font-medium text-sm cursor-pointer bg-transparent border-none p-0 inline-block hover:underline"
                    >
                        Already have an account? Login
                    </button>
                </div>
                <DisplayError error={error} />
            </div>
        </div >
    )
}