import React from "react";
import { useNavigate } from "react-router-dom";
import "./Landing.css";
import pic from "../assets/Landing.png"; 
import Navbar from "../components/Navbar"; 

const Landing = () => {
  const navigate = useNavigate(); 

  return (
    <>
      <Navbar />
      <div className="hero-container">
        <div className="hero">
          <h1>Take Charge Of Your Financial Journey</h1>
          <p>Plan Smarter, Save More, And Achieve Your Goals Effortlessly</p>
          <button className="hero-btn" onClick={() => navigate("/signup")}>
            Try It Now →
          </button>
        </div>

        <div className="hero-image">
          <img src={pic} alt="Charts Illustration" />
        </div>
      </div>
    </>
  );
};

export default Landing;
