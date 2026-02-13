import { LOGIN_PATH, PROTECTED_PATH, REGISTER_PATH } from "../config";
import type { UserLogin } from "../types";

export const performLogin = async (login: UserLogin) => {
    const response = await fetch(LOGIN_PATH, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(login),
    });

    if (!response.ok) {
        throw Error(await response.text());
    }
    const token = await response.text();
    localStorage.setItem("authToken", token);
};

export const performRegister = async (register: UserLogin) => {
    const response = await fetch(REGISTER_PATH, {
        method: "POST",
        headers: {
            "Content-Type": "application/json",
        },
        body: JSON.stringify(register),
    });

    if (!response.ok) {
        throw Error(await response.text());
    }
};



export const checkAuthorized = async () => {
    const response = await fetch(PROTECTED_PATH, setRequestAuth({}));
    return response.ok;
}

export const handleLogout = () => {
    localStorage.removeItem("authToken");
}

export const setRequestAuth = (requestInit: RequestInit) => {
    const token: string | null = localStorage.getItem("authToken");
    if (!token) {
        throw Error("Token not in localstorage!")
    }
    requestInit.headers = {
        "Authorization": `Bearer ${token}`
    }

    return requestInit;
}