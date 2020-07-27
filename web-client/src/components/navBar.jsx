import React from "react";
import { Navbar, NavbarBrand, NavbarText } from "reactstrap";

const NavBar = () => {
  return (
    <div>
      <Navbar color="dark" dark expand="md">
        <NavbarBrand href="/">Programming Challenge 2020</NavbarBrand>
        <NavbarText>Movie Lens</NavbarText>
      </Navbar>
    </div>
  );
};

export default NavBar;
