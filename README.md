# Solar Watch

Full-stack application for finding sunrise and sunset times for cities worldwide.

## Quick Start

1. **Configure environment variables**:
   - Copy [example.env](example.env) to `.env` in the root directory
   - Configure database, admin and JWT secret passwords etc.
   - Get an [OpenWeatherMap API key](https://home.openweathermap.org/users/sign_in) (free):
     - Add it to `.env` as `ExternalApiKeys__OpenWeatherMap=your_key_here`

2. **Run with Docker**:

   ```bash
   docker compose up -d
   ```

3. **Access**:
   - Frontend: <http://localhost:4770>
   - Backend: <http://localhost:8080>

## Development Setup

**Backend** (`backend/`):

- Configure
- Configure database, JWT, and admin credentials in root `.env`

**Frontend** (`frontend/`):

- Copy `frontend/example.env` to `frontend/.env` for local development
- Set `APP_BACKEND_API` to your backend URL
