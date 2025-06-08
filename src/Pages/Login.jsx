import React, { useState } from "react";
import styles from "./Login.module.css";
import { Link, useNavigate } from "react-router-dom";
import { Toast, ToastContainer } from "react-bootstrap";
import logo from "../assets/Logo Icon@2x.png";
import loginImage from "../assets/signup.png";
import { GoogleOAuthProvider, GoogleLogin } from "@react-oauth/google";
import axios from "axios";
import ForgotPassword from "./ForgotPassword"; 
import ResetPassword from "./ResetPassword";
const CLIENT_ID = "980949516842-p25ise9gq3nrp0e6defp164c96k6l8iv.apps.googleusercontent.com";

function Login() {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [toastMessage, setToastMessage] = useState({ type: "", text: "" });
  const [showToast, setShowToast] = useState(false);
  const [showForgotPassword, setShowForgotPassword] = useState(false); 
  const [showResetPassword, setShowResetPassword] = useState(false); 
  const navigate = useNavigate();

  const showToastMessage = (type, text) => {
    setToastMessage({ type, text });
    setShowToast(true);
  };

  const handleLogin = async (e) => {
    e.preventDefault();
    try {
      const loginData = { email, password };
      const response = await axios.post("https://graduproj.runasp.net/api/Account/login", loginData);

      // تخزين التوكن في localStorage بعد تسجيل الدخول بنجاح
      const token = response.data.token;
      localStorage.setItem("token", token);

      showToastMessage("success", "Login successful!");
      setTimeout(() => navigate("/welcome"), 1500); // عدل المسار حسب احتياجك
    } catch (error) {
      const msg = error.response?.data?.message || "Something went wrong.";
      showToastMessage("error", msg);
    }
  };

const handleGoogleLogin = async (response) => {
  try {
    if (response.credential) {
      const googleData = { idToken: response.credential };

      const res = await axios.post("https://graduproj.runasp.net/api/Account/google-login", googleData);

      // ✅ خزن التوكن في localStorage
      const token = res.data.token;
      localStorage.setItem("token", token);

      showToastMessage("success", "Google login successful!");

      // ✅ تنقلي المستخدم على نفس صفحة ال login العادية (ممكن تخليها /welcome بدل /home)
      setTimeout(() => navigate("/welcome"), 1500);
    }
  } catch (error) {
    showToastMessage("error", "Google login failed.");
  }
};
  const handleShowForgotPassword = () => setShowForgotPassword(true); // إظهار مودال "Forgot Password"
  const handleCloseForgotPassword = () => setShowForgotPassword(false); // إغلاق مودال "Forgot Password"
  
  const handleShowResetPassword = () => setShowResetPassword(true); // إظهار مودال "Reset Password"
  const handleCloseResetPassword = () => setShowResetPassword(false); // إغلاق مودال "Reset Password"

  return (
    <GoogleOAuthProvider clientId={CLIENT_ID}>
      <div className={styles["login-container"]}>
        <div className={styles["login-image"]}>
          <img src={loginImage} alt="Finance Illustration" />
        </div>

        <div className={styles["login-box"]}>
          <div className={styles["login-logo"]}>
            <img src={logo} alt="Wealth Wise Logo" className={styles["login-logo-img"]} />
            <h1 className={styles["login-brand-name"]}>WealthWise</h1>
          </div>

          <h2>Log In</h2>
          <form className={styles["login-form"]} onSubmit={handleLogin}>
            <label htmlFor="email" className={styles["login-label"]}>Email</label>
            <input
              type="email"
              id="email"
              placeholder="Enter Email"
              required
              className={styles["login-input"]}
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />

            <label htmlFor="password" className={styles["login-label"]}>Password</label>
            <input
              type="password"
              id="password"
              placeholder="Enter Password"
              required
              className={styles["login-input"]}
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              
            />

            <div className={styles["login-options"]}>
              <label htmlFor="rememberMe">
                <input type="checkbox" id="rememberMe" className={styles.checkbox} /> Remember me
              </label>
              <Link to="#" onClick={handleShowForgotPassword} className={styles["forgot-password"]}>
                Forgot password?
              </Link>
            </div>

            <button type="submit" className={styles["login-btn"]}>
              Login
            </button>
          </form>

          <div className={styles.divider}><span>or</span></div>

          <GoogleLogin
            onSuccess={handleGoogleLogin}
            onError={() => showToastMessage("error", "Google Login Failed")}
            useOneTap
            shape="pill"
            width="50%"
            theme="outline"
            text="continue_with"
            containerProps={{
              style: {
                width: "auto",
                margin: "auto",
                padding: "auto",
              },
            }}
          />
        </div>
      </div>

      {/* Toast */}
      <ToastContainer position="top-center" className="p-3">
        <Toast show={showToast} onClose={() => setShowToast(false)} delay={3000} autohide>
          <Toast.Body className={toastMessage.type === "success" ? "text-success" : "text-danger"}>
            {toastMessage.text}
          </Toast.Body>
        </Toast>
      </ToastContainer>

      {/* مودال "Forgot Password" */}
      <ForgotPassword
        show={showForgotPassword}
        handleClose={handleCloseForgotPassword}
        handleShowResetPassword={handleShowResetPassword}
      />

      {/* مودال "Reset Password" */}
      <ResetPassword show={showResetPassword} handleClose={handleCloseResetPassword} />
    </GoogleOAuthProvider>
  );
}

export default Login;
