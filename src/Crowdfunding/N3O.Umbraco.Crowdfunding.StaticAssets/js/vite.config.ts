import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  build: {
    outDir: "./build",
    emptyOutDir: true,
    assetsDir: './',
    rollupOptions: {
      input: {
        main: 'index.html',
      },
      output: {
        entryFileNames: 'crowdfunding-editor.js',
        assetFileNames: 'crowdfunding-editor.css'
      },
    },
  }
})
