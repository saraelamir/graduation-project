import React, { useState } from "react";
import { Modal, Button } from "react-bootstrap";
import axios from "axios";

const ForgotPassword = ({ show, handleClose, handleShowResetPassword }) => {
  const [email, setEmail] = useState("");
  const [message, setMessage] = useState("");
  const [otpSent, setOtpSent] = useState(false); // إضافة حالة للتحقق إذا تم إرسال الكود

  const handleSend = async () => {
    // التحقق من وجود البريد الإلكتروني
    if (!email) {
      setMessage("Please enter a valid email address.");
      return;
    }

    try {
      console.log("Email being sent:", email); // طباعة البريد للتأكد

      // إرسال الطلب وحفظ الريسبونس
      const response = await axios.post("https://graduproj.runasp.net/api/Account/forgot-password", { email });

      // فحص الرسالة من السيرفر
      if (response.data.message === "OTP sent to email") {
        setMessage("Check your email for reset link.");
        setOtpSent(true); // تحديد أنه تم إرسال الكود
      } else {
        setMessage("Something went wrong, please try again.");
      }
    } catch (error) {
      console.error("Error during request:", error);
      const errorMessage = error.response?.data?.message || "Error: Could not send reset link.";
      setMessage(errorMessage);
    }
  };

  const handleResetPassword = () => {
    handleShowResetPassword(); // عرض مودال Reset Password
    handleClose(); // إغلاق مودال Forgot Password
  };

  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>Forgot Password</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <input
          type="email"
          className="form-control"
          placeholder="Enter your email"
          value={email}
          onChange={(e) => setEmail(e.target.value)}
        />
        {message && <p className="mt-2 text-info">{message}</p>}
        
        {/* عرض زر Reset Password بعد إرسال OTP */}
        {otpSent && (
          <Button variant="primary" onClick={handleResetPassword}>Reset Password</Button>
        )}
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={handleClose}>Close</Button>
        {!otpSent && <Button variant="primary" onClick={handleSend}>Send OTP</Button>}
      </Modal.Footer>
    </Modal>
  );
};

export default ForgotPassword;
