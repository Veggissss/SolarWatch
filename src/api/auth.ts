import { API_ENDPOINT, LOGIN_PATH, PROTECTED_PATH } from "../config";
import type { UserLogin } from "../types";

export const performLogin = async (login: UserLogin) => {
    const response = await fetch(API_ENDPOINT + LOGIN_PATH, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(login),
    });

    if (response.ok) {
        const { token } = await response.json();
        console.log(token);
        localStorage.setItem("authToken", token);
        return true;
    }
    return false;
};

export const checkAuthorized = async () => {
    const response = await fetch(API_ENDPOINT + PROTECTED_PATH);
    return response.ok;
}

export const handleLogout = () => {
    localStorage.removeItem("authToken");
}