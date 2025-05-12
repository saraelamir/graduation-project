import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],

  server: {
    port: 5173,  // البورت الذي تريده
    // strictPort: true  // إذا كان البورت غير متاح، Vite لن يغيره تلقائيًا
  }
})
