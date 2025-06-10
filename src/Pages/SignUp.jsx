import React, { useState } from "react";
import { Button, Form } from "react-bootstrap";
import axios from "axios"; // استيراد axios
import styles from "./Signup.module.css";
import signupImage from "../assets/signup.png";
import googleIcon from "../assets/image.png";
import { FaEye, FaEyeSlash } from "react-icons/fa"; // استيراد أيقونات العين
import { useNavigate } from "react-router-dom";
import VerifyCode from './VerifyCode'; // استيراد المودال

export default function Signup() {
  const navigate = useNavigate(); // تعريف navigate هنا
  const [formData, setFormData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    password: "",
    confirmPassword: "",
    country: "",
  });

  const [isLoading, setIsLoading] = useState(false);
  const [showPassword, setShowPassword] = useState(false); // حالة لعرض كلمة المرور
  const [showVerifyModal, setShowVerifyModal] = useState(false); // حالة إظهار المودال

  // دالة للتحقق من صحة كلمة المرور (8 أحرف على الأقل)
  const validatePassword = (password) => {
    const minLength = 8;

    if (password.length < minLength) {
      return `Password must be at least ${minLength} characters long.`;
    }
    return null;
  };

  // دالة إرسال البيانات للنموذج
  const handleSubmit = async (e) => {
    e.preventDefault();

    // التحقق من تطابق كلمة المرور
    if (formData.password !== formData.confirmPassword) {
      alert("Passwords do not match!");
      return;
    }

    // التحقق من صحة كلمة المرور (الشرط هو 8 أحرف فقط)
    const passwordError = validatePassword(formData.password);
    if (passwordError) {
      alert(passwordError);
      return;
    }

    // التحقق من صحة البريد الإلكتروني
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(formData.email)) {
      alert("Please enter a valid email address!");
      return;
    }

    // البيانات التي سيتم إرسالها
    const dataToSend = {
      firstName: formData.firstName,
      lastName: formData.lastName,
      email: formData.email,
      password: formData.password,
      country: formData.country,
    };

    setIsLoading(true); // تفعيل حالة التحميل

    try {
      // إرسال البيانات إلى API باستخدام axios
      const response = await axios.post(
        "https://graduproj.runasp.net/api/Account/register",
        dataToSend,
        {
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      // التعامل مع الاستجابة من API
      console.log("User registered successfully:", response.data);
      alert("Registration successful! Please check your email for the verification code.");
      
      // إظهار المودال بعد التسجيل بنجاح
      setShowVerifyModal(true); 

      // إعادة تعيين البيانات بعد النجاح
      setFormData({
        firstName: "",
        lastName: "",
        email: "",
        password: "",
        confirmPassword: "",
        country: "",
      });
    } catch (error) {
      // التعامل مع الأخطاء في حالة فشل الاتصال
      console.error("Error registering user:", error);
      alert(error.response?.data?.message || "Registration failed!");
    } finally {
      setIsLoading(false); // إيقاف حالة التحميل
    }
  };

  return (
    <div className={styles.container}>
      <div className="row justify-content-center w-100 align-items-center">
        <div className="col-12 col-md-6 fade-in order-md-1 order-2 text-center">
          <img
            src={signupImage}
            alt="Signup Illustration"
            className={`img-fluid ${styles.image}`}
            style={{ maxWidth: "85%" }}
          />
        </div>

        <div className="col-12 col-md-6 col-lg-5 fade-in order-md-2 order-1">
          <div className={styles.formContainer}>
            <h2 className={styles.title}>Join Wealth Wise</h2>

            <Form onSubmit={handleSubmit}>
              <div className="row g-2">
                <div className="col-md-6">
                  <Form.Group controlId="firstName">
                    <Form.Label className={styles.label}>First Name</Form.Label>
                    <Form.Control
                      required
                      className={styles.input}
                      placeholder="First Name"
                      onChange={(e) =>
                        setFormData({ ...formData, firstName: e.target.value })
                      }
                    />
                  </Form.Group>
                </div>

                <div className="col-md-6">
                  <Form.Group controlId="lastName">
                    <Form.Label className={styles.label}>Last Name</Form.Label>
                    <Form.Control
                      required
                      className={styles.input}
                      placeholder="Last Name"
                      onChange={(e) =>
                        setFormData({ ...formData, lastName: e.target.value })
                      }
                    />
                  </Form.Group>
                </div>
              </div>

              <div className="row g-2">
                <div className="col-md-6">
                  <Form.Group controlId="password">
                    <Form.Label className={styles.label}>Password</Form.Label>
                    <div className="position-relative">
                      <Form.Control
                        required
                        type={showPassword ? "text" : "password"} // تغيير النوع بناءً على الحالة
                        className={styles.input}
                        placeholder="Enter password"
                        onChange={(e) =>
                          setFormData({ ...formData, password: e.target.value })
                        }
                      />
                      <span
                        className="position-absolute"
                        style={{ top: "50%", right: "10px", transform: "translateY(-50%)" }}
                        onClick={() => setShowPassword(!showPassword)} // تغيير حالة العين
                      >
                        {showPassword ? <FaEyeSlash /> : <FaEye />}
                      </span>
                    </div>
                  </Form.Group>
                </div>

                <div className="col-md-6">
                  <Form.Group controlId="confirmPassword">
                    <Form.Label className={styles.label}>Confirm Password</Form.Label>
                    <Form.Control
                      required
                      type={showPassword ? "text" : "password"} // تغيير النوع بناءً على الحالة
                      className={styles.input}
                      placeholder="Confirm password"
                      onChange={(e) =>
                        setFormData({
                          ...formData,
                          confirmPassword: e.target.value,
                        })
                      }
                    />
                  </Form.Group>
                </div>
              </div>

              <div className="row g-2">
                <div className="col-md-6">
                  <Form.Group controlId="email" className="mb-2">
                    <Form.Label className={styles.label}>Email Address</Form.Label>
                    <Form.Control
                      required
                      type="email"
                      className={styles.input}
                      placeholder="Enter email"
                      onChange={(e) =>
                        setFormData({ ...formData, email: e.target.value })
                      }
                    />
                  </Form.Group>
                </div>

                <div className="col-md-6">
                  <Form.Group controlId="country">
                    <Form.Label className={styles.label}>Country</Form.Label>
                    <Form.Control
                      required
                      type="text"
                      className={styles.input}
                      placeholder="Enter country"
                      value={formData.country}
                      onChange={(e) =>
                        setFormData({ ...formData, country: e.target.value })
                      }
                    />
                  </Form.Group>
                </div>
              </div>

              <Button type="submit" className={styles.button} disabled={isLoading}>
                {isLoading ? "Signing Up..." : "Sign Up"}
              </Button>

              {/* <div className="d-flex align-items-center my-3">
                <div className="flex-grow-1 border-top border-white"></div>
                <span className="mx-2 text-white" style={{ fontSize: "0.8rem" }}>
                  or
                </span>
                <div className="flex-grow-1 border-top border-white"></div>
              </div>

              <Button
                variant="outline-light"
                className={`${styles.googleButton} d-flex justify-content-center align-items-center mt-3`}
              >
                <img
                  src={googleIcon}
                  alt="Google"
                  style={{ width: "1rem", marginRight: "8px" }}
                />
                <span>Continue with Google</span>
              </Button> */}
            </Form>
          </div>
        </div>
      </div>

      {/* إضافة المودال هنا */}
      {showVerifyModal && (
        <VerifyCode
          show={showVerifyModal}
          onHide={() => setShowVerifyModal(false)}
        />
      )}
    </div>
  );
}
