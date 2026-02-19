import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.tsx'
import { BrowserRouter } from 'react-router'


async function enableMocking() {
  const isMockingEnabled = (import.meta.env.VITE_MOCKING_ENABLED ?? '') === 'true';

  if (isMockingEnabled) {
    const { worker } = await import('./mocks/worker');
    return worker.start();
  }
}

enableMocking().then(() => {
  createRoot(document.getElementById('root')!).render(
    <StrictMode>
      <BrowserRouter basename={import.meta.env.BASE_URL}>

        <App />
      </BrowserRouter>
    </StrictMode>
  )
});