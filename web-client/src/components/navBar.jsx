import React, { useState } from "react";
import {
  Collapse,
  Navbar,
  NavbarToggler,
  NavbarBrand,
  Nav,
  NavItem,
  NavLink,
  UncontrolledDropdown,
  DropdownToggle,
  DropdownMenu,
  DropdownItem,
  NavbarText,
} from "reactstrap";

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
