import { Route, Routes } from 'react-router'
import './App.css'
import { SolarWatchPage } from './pages/SolarWatchPage'
import { LoginPage } from './pages/LoginPage'
import { RegistrationPage } from './pages/RegistrationPage'

function App() {

  return (
    <Routes>
      <Route path='/' element={<SolarWatchPage />}></Route>
      <Route path='/solar-watch' element={<SolarWatchPage />}></Route>
      <Route path='/login' element={<LoginPage />}></Route>
      <Route path='/registration' element={<RegistrationPage />}></Route>
    </Routes>
  )
}

export default App
