import { CITY_PATH, SUNDATA_PATH } from "../config"
import type { City, SunData } from "../types";
import { setRequestAuth } from "./auth";

export const fetchSunData = async (cityName: string) => {
    const response = await fetch(SUNDATA_PATH + `?city=${cityName}`, setRequestAuth({}))
    if (!response.ok) {
        if (response.status === 404) {
            throw Error("City not found!");
        }
        if (response.status === 401) {
            throw Error("Unauthorized!");
        }
        throw Error(await response.text());
    }

    return await response.json() as SunData;
}

export const fetchCities = async () => {
    const response = await fetch(CITY_PATH, setRequestAuth({}))
    if (!response.ok) {
        throw Error(await response.text())
    }
    return await response.json() as City[];
}