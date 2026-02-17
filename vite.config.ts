import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import tailwindcss from '@tailwindcss/vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react(), tailwindcss()],
  server: {
    port: 4770,
    strictPort: true,
    host: true,
    origin: "http://0.0.0.0:4770",
    proxy: {
      '/backend': {
        target: 'https://solarwatch-api-csharp.onrender.com',
        changeOrigin: true,
        rewrite: (path) => path.replace(/^\/backend/, '')
      }
    }
  },
  preview: {
    port: 4770,
    strictPort: true,
  },
})
