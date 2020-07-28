import React from "react";
import "./App.css";
import Movies from "./views/movies/movies";
import NavBar from "./components/navBar";
import { ToastContainer } from "react-toastify";

const App = () => {
  return (
    <React.Fragment>
      <ToastContainer hideProgressBar={true} />
      <NavBar></NavBar>
      <main className="container">
        <Movies></Movies>
      </main>
    </React.Fragment>
  );
};

export default App;
