import React, { useState } from "react";
import { Modal, Button } from "react-bootstrap";
import axios from "axios";

const ResetPassword = ({ show, handleClose }) => {
  const [otpCode, setOtpCode] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [message, setMessage] = useState("");

  // تحقق من أن كلمة المرور تتكون من 8 أحرف على الأقل وتحتوي على حرف كبير وحرف صغير ورقم
  const validatePassword = (password) => {
    const passwordRegex = /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d).{8,}$/;
    return passwordRegex.test(password);
  };

  const handleReset = async () => {
    if (!otpCode || !newPassword) {
      setMessage("Please fill in all fields.");
      return;
    }

    // التحقق من صحة كلمة المرور
    if (!validatePassword(newPassword)) {
      setMessage("Password must be at least 8 characters long, contain at least one uppercase letter, one lowercase letter, and one number.");
      return;
    }

    try {
      const response = await axios.post(
        "https://graduproj.runasp.net/api/Account/reset-password",
        { otpCode, newPassword }
      );
      if (response && response.status === 200) {
        setMessage("Password has been reset successfully!");
        setTimeout(() => {
          handleClose(); // إغلاق المودال بعد 2 ثانية
        }, 2000);
      } else {
        setMessage(`Error: ${response.data.message || "Could not reset the password."}`);
      }
    } catch (error) {
      // إضافة تسجيل للأخطاء
      if (error.response) {
        // إذا كان الخطأ ناجمًا عن رد من الخادم
        console.error("Error response data:", error.response.data);
        setMessage(`Error: ${error.response.data.message || "Could not reset the password."}`);
      } else if (error.request) {
        // إذا كان الطلب قد تم إرساله لكن لم يتم الرد عليه
        console.error("Error request:", error.request);
        setMessage("Error: No response from server.");
      } else {
        // إذا كان هناك خطأ آخر أثناء إعداد الطلب
        console.error("Error message:", error.message);
        setMessage(`Error: ${error.message}`);
      }
    }
  };

  return (
    <Modal show={show} onHide={handleClose}>
      <Modal.Header closeButton>
        <Modal.Title>Reset Password</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <input
          type="text"
          className="form-control"
          placeholder="Enter OTP Code received on email"
          value={otpCode}
          onChange={(e) => setOtpCode(e.target.value)}
        />
        <input
          type="password"
          className="form-control mt-2"
          placeholder="Create a New Password"
          value={newPassword}
          onChange={(e) => setNewPassword(e.target.value)}
        />
        {message && <p className="mt-2 text-info">{message}</p>}
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={handleClose}>Close</Button>
        <Button variant="primary" onClick={handleReset}>Reset Password</Button>
      </Modal.Footer>
    </Modal>
  );
};

export default ResetPassword;
