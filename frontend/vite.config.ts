import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react-swc'
import tailwindcss from '@tailwindcss/vite'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
  const env = loadEnv(mode, process.cwd(), '');
  return {
    define: {
      // Provide an explicit app-level constant derived from an env var.
      __APP_ENV__: JSON.stringify(env.APP_ENV),
    },
    plugins: [react(), tailwindcss()],

    server: {
      port: env.APP_PORT ? Number(env.APP_PORT) : 4770,
      strictPort: true,
      host: true,
      origin: `http://0.0.0.0:${env.APP_PORT ? Number(env.APP_PORT) : 4770}`,

      proxy: {
        '/backend': {
          target: `${env.APP_BACKEND_API ? env.APP_BACKEND_API : "http://localhost:8080"}`,
          changeOrigin: true,
          rewrite: (path) => path.replace(/^\/backend/, '')
        }
      }
    },

    preview: {
      port: env.APP_PORT ? Number(env.APP_PORT) : 4770,
      strictPort: true,
      allowedHosts: env.APP_HOSTNAME ? [env.APP_HOSTNAME] : ["localhost"]
    },
    optimizeDeps: {
      exclude: ["msw", "@mswjs/interceptors"]
    }
  }
})
