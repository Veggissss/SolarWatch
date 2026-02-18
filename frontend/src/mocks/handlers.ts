import { http, HttpResponse } from 'msw'
import citiesData from "./data/cities.json"
import type { City } from '../types'

export const handlers = [
    http.get('*backend/api/city', () => {
        return HttpResponse.json(citiesData as City[])
    }),

    http.get('*backend/api/sundata', ({ request }) => {
        const url = new URL(request.url)
        const city = url.searchParams.get('city')

        if (city === 'London') {
            return HttpResponse.json({
                sunrise: '06:00:00',
                sunset: '18:00:00',
            })
        }

        if (city === 'Budapest') {
            return HttpResponse.json({
                sunrise: '05:30:00',
                sunset: '19:30:00',
            })
        }
        return new HttpResponse(null, { status: 404, statusText: 'City not found!' })
    }),

    http.post('*backend/api/auth/login', async () => {
        return HttpResponse.text('eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.fake-token')
    }),

    http.post('*backend/api/auth/register', async () => {
        return HttpResponse.text('User created')
    }),

    http.get('*backend/api/auth/protected', async () => {
        return HttpResponse.text('ok')
    }),
]