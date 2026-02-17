
export interface UserLogin {
    username: string;
    password: string;
}

export interface SunData {
    sunrise: string;
    sunset: string;
}

export interface City {
    id: number;
    name: string;
    latitude: number;
    longitude: number;
    country: string;
    state: string;
    sunData: [] | SunData[];
}