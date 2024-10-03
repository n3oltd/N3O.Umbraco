import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'
import path from 'node:path';

// https://vitejs.dev/config/
export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      '@': path.resolve(__dirname, 'src'),  // Map @ to src folder
    },
  },
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
