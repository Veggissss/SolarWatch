import { useEffect, useState } from "react";
import { useNavigate } from "react-router";
import type { UserLogin } from "../types";
import { LoginForm } from "../components/LoginForm";
import { performLogin } from "../api/auth";
import { DisplayError } from "../components/DisplayError";
import { getRandomMoji } from "../service/kaomoji";

export function LoginPage() {
    const [userLogin, setUserLogin] = useState<UserLogin | null>(null);
    const [error, setError] = useState("");
    const navitation = useNavigate();

    useEffect(() => {
        if (!userLogin) {
            return
        }
        performLogin(userLogin).then(() => {
            navitation("/solar-watch");
        }).catch((err: Error) => {
            setError(`${err.message} ${getRandomMoji()}`);
        });
    }, [userLogin, navitation])

    return (
        <div className="min-h-screen flex items-center justify-center bg-gray-50 dark:bg-gray-900 py-12 px-4 sm:px-6 lg:px-8">
            <div className="max-w-md w-full space-y-8 bg-white dark:bg-gray-800 p-8 rounded-xl shadow-lg">
                <div className="text-center">
                    <h1 className="text-3xl font-extrabold text-gray-900 dark:text-white mb-2">Login</h1>
                    <p className="text-sm text-gray-600 dark:text-gray-400">Sign in to your account</p>
                </div>

                <LoginForm onRegistrered={(user) => setUserLogin(user)} onError={(message) => setError(message)} />

                <div className="text-center mt-4">
                    <button
                        onClick={() => navitation("/register")}
                        className="text-indigo-600 hover:text-indigo-500 font-medium text-sm cursor-pointer bg-transparent border-none p-0 inline-block hover:underline"
                    >
                        Don't have an account? Sign up
                    </button>
                </div>
                <DisplayError error={error} />
            </div>
        </div>
    )
}