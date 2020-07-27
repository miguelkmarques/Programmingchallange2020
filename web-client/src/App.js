import React from "react";
import "./App.css";
import Movies from "./views/movies/movies";
import { Container } from "reactstrap";
import NavBar from "./components/navBar";

const App = () => {
  return (
    <React.Fragment>
      <NavBar></NavBar>
      <main className="container">
        <Movies></Movies>
      </main>
    </React.Fragment>
  );
};

export default App;
