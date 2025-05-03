import React from "react";
import { Button } from "react-bootstrap";
import { Link } from "react-router-dom";
import welcomeImage from "../assets/welcome.png";
import Navbar from "../components/Navbar";

export default function Welcome() {
  return (
    <div className="main-gradient-bg">
      <Navbar />
      <div className="container-fluid flex-grow-1 d-flex align-items-center">
        <div className="row justify-content-center w-100 align-items-center">
          {/* Text Content Column */}
          <div className="col-12 col-md-6 col-lg-5 text-left fade-in order-md-1 order-2 ps-0">
            <h1
              className="text-white mb-4 welcome-heading"
              style={{
                fontSize: "64px",
                fontWeight: "700",
                fontFamily: "Inter",
              }}
            >
              Welcome Mohammed,
            </h1>
            <p
              className="text-white mb-5 welcome-subtitle"
              style={{
                fontSize: "24px",
                fontWeight: "400",
                fontFamily: "Inter",
              }}
            >
              Your Journey To Smarter Spending And Bigger
              <br />
              Savings Starts Here
            </p>
            <Button
              as={Link}
              to="/home"
              className="get-started-btn px-5 py-3 rounded-pill ms-0"
              style={{
                backgroundColor: "#fff",
                color: "#0F5378",
                fontSize: "18px",
                fontWeight: "500",
                fontFamily: "Inter",
                transition: "all 0.3s ease",
              }}
              onMouseOver={(e) => (e.target.style.backgroundColor = "#e6e6e6")}
              onMouseOut={(e) => (e.target.style.backgroundColor = "#fff")}
            >
              Get Started
            </Button>
          </div>

          {/* Image Column */}
          <div className="col-12 col-md-6 col-lg-5 fade-in order-md-2 order-1 mb-4 mb-md-0">
            <img
              src={welcomeImage}
              alt="Financial Planning"
              className="img-fluid welcome-image"
              style={{ maxWidth: "600px" }}
            />
          </div>
        </div>
      </div>
    </div>
  );
}
