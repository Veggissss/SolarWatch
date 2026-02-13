import { SUNDATA_PATH } from "../config"
import type { SunData } from "../types";
import { setRequestAuth } from "./auth";

export const fetchSunData = async (cityName: string) => {
    const response = await fetch(SUNDATA_PATH + `?city=${cityName}`, setRequestAuth({}))
    if (!response.ok) {
        if (response.status === 404) {
            throw Error("City not found!");
        }

        throw Error(await response.text());
    }

    return await response.json() as SunData;
}

