import { Route, Routes } from 'react-router'
import { SolarWatchPage } from './pages/SolarWatchPage'
import { LoginPage } from './pages/LoginPage'
import { RegistrationPage } from './pages/RegistrationPage'
import { ProtectedRoutes } from './utils/ProtectedRoutes'
import { HomePage } from './pages/HomePage'

function App() {

  return (
    <Routes>
      <Route element={<ProtectedRoutes />}>
        <Route path='/' element={<HomePage />}></Route>
        <Route path='/solar-watch' element={<SolarWatchPage />}></Route>
      </Route>
      <Route path='/login' element={<LoginPage />}></Route>
      <Route path='/register' element={<RegistrationPage />}></Route>
    </Routes>
  )
}

export default App
