import React from "react";
import { useNavigate, Link, useLocation } from "react-router-dom";
import logo from "../assets/icon.png";
import "./Navbar.css";

const Navbar = () => {
  const navigate = useNavigate();
  const location = useLocation();
  const isWelcomePage = location.pathname === "/welcome";
  const isLandingPage = location.pathname === "/landing";


  return (
    <nav className="navbar">
      <div className="logo">
        <img src={logo} alt="Wealth Wise Logo" />
        <h2 className="mt-3">Wealth Wise</h2>
      </div>
      {!isLandingPage && (
        
      <div className="nav-links">
        <Link to="/home">Home</Link>
        <Link to="/help">Help</Link>
        <Link to="/history">History</Link>
        <Link to="/settings">Settings</Link>
        <Link to="/feedback">Feedback</Link>
      </div>
            )}

      {!isWelcomePage && (
        <div className="buttons">
          <button className="login-btn" onClick={() => navigate("/login")}>
            Login
          </button>
          <button className="signup-btn" onClick={() => navigate("/signup")}>
            Signup
          </button>
        </div>
      )}
    </nav>
  );
};

export default Navbar;
