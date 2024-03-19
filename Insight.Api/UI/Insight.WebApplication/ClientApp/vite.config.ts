/// <reference types="vitest" />
import react from "@vitejs/plugin-react";
import * as fs from "fs";
import { ServerOptions } from "https";
import { defineConfig, loadEnv } from "vite";
import checker from "vite-plugin-checker";

/**
 * Create the HTTPS server. If the local certificates are not found, fallback to default.
 * This also ensures that the test runner won't fail, because it's trying to find the certificates.
 * @returns HTTPS server options
 */
const getServerOptions = () => {
  let https: boolean | ServerOptions = true;
  if (process.env.SSL_KEY_FILE && process.env.SSL_CRT_FILE) {
    const key = fs.readFileSync(process.env.SSL_KEY_FILE);
    const cert = fs.readFileSync(process.env.SSL_CRT_FILE);
    if (key && cert) {
      https = {
        key,
        cert,
      };
    }
  }
  return https;
};

// https://vitejs.dev/config/ & https://vite-pwa-org.netlify.app/
export default ({ mode }) => {
  process.env = Object.assign(process.env, loadEnv(mode, process.cwd(), ""));
  return defineConfig({
    plugins: [react(), checker({ typescript: true })],
    optimizeDeps: {
      include: ["@mui/icons-material"],
    },
    build: {
      outDir: "../wwwroot",
    },
    server: {
      https: getServerOptions(),
      open: false,
      strictPort: true,
      port: 3000,
      proxy: {
        "/swagger": {
          target: "https://localhost:7084",
          secure: false,
          changeOrigin: true,
        },
        "/api": {
          target: "https://localhost:7084",
          secure: false,
          changeOrigin: true,
        },
        "/health": {
          target: "https://localhost:7084",
          secure: false,
          changeOrigin: true,
        },
      },
    },
  });
};
