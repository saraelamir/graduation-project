import React, { useState } from "react";
import { Modal, Button, Form } from "react-bootstrap";
import axios from "axios";
import { useNavigate } from "react-router-dom";

const VerifyCode = ({ show, onHide }) => {
  const [code, setCode] = useState(""); // تغيير الاسم هنا إلى "code"
  const navigate = useNavigate();

  const handleVerify = async (e) => {
    e.preventDefault();

    // التحقق من أن الكود ليس فارغًا
    if (code.trim() === "") {
      alert("Please enter the verification code.");
      return;
    }

    try {
      const response = await axios.post("https://graduproj.runasp.net/api/Account/verify-code", {
        code, // ارسال "code" بدلاً من "otp"
      });

      alert(response.data.message);
      onHide();  // غلق المودال بعد التحقق بنجاح
      navigate("/login");
    } catch (error) {
      // التحقق من الخطأ وعرض الرسالة المناسبة
      if (error.response && error.response.data) {
        alert(error.response.data.message);
      } else {
        alert("Verification failed. Please try again.");
      }
    }
  };

  return (
    <Modal show={show} onHide={onHide} centered>
      <Modal.Header closeButton>
        <Modal.Title>Enter Verification Code</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <Form onSubmit={handleVerify}>
          <Form.Group controlId="formCode">
            <Form.Label>Verification Code</Form.Label>
            <Form.Control
              type="text"
              placeholder="Enter the code from your email"
              value={code}  // تغيير القيمة هنا إلى "code"
              onChange={(e) => setCode(e.target.value)}  // تغيير الكود هنا إلى "code"
              required
            />
          </Form.Group>
          <div className="text-center mt-3">
            <Button variant="primary" type="submit">
              Verify
            </Button>
          </div>
        </Form>
      </Modal.Body>
    </Modal>
  );
};

export default VerifyCode;
