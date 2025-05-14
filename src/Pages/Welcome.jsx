import React from "react";
import { Button } from "react-bootstrap";
import { Link } from "react-router-dom";
import welcomeImage from "../assets/welcome.png";
import Navbar from "../components/Navbar";
import styles from "./Welcome.module.css";

export default function Welcome() {
  return (
    <div className={styles.wrapper}>
      <Navbar />
      <div className={`container-fluid ${styles.contentContainer}`}>
        <div className="row justify-content-center align-items-center w-100">
          {/* Text Column */}
          <div className={`col-12 col-md-6 col-lg-5 ${styles.textCol}`}>
            <h1 className={styles.heading}>Welcome</h1>
            <p className={styles.subtitle}>
              Your Journey To Smarter Spending And Bigger<br />Savings Starts Here
            </p>
            <Button
              as={Link}
              to="/input"
              className={styles.startBtn}
            >
              Get Started
            </Button>
          </div>

          {/* Image Column */}
          <div className={`col-12 col-md-6 col-lg-5 ${styles.imageCol}`}>
            <img
              src={welcomeImage}
              alt="Financial Planning"
              className={styles.image}
            />
          </div>
        </div>
      </div>
    </div>
  );
}
