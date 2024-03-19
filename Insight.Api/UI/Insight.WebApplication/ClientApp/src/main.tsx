import React from "react";
import ReactDOM from "react-dom/client";
import { App } from "./App.tsx";
import "./assets/fonts/Dinot.css";
import "./assets/fonts/OpenSans.css";
import "./assets/fonts/Montserrat.css";

async function configureMocks() {
  return new Promise<void>((resolve) => {
    if (
      import.meta.env.MODE === "development" &&
      localStorage.getItem("MOCK_NETWORK") === "true"
    ) {
      import("./mocks/browser").then((browser) => {
        browser.worker.start();
        resolve();
      });
    } else {
      resolve();
    }
  });
}

// TODO: Enable strict mode when able to ensure reusable state. See link:
// https://reactjs.org/docs/strict-mode.html#ensuring-reusable-state
configureMocks().then(() => {
  ReactDOM.createRoot(document.getElementById("root")!).render(
    <React.StrictMode>
      <App />
    </React.StrictMode>,
  );
});
