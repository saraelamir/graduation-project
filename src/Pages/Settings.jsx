import React, { useState } from "react";
import { Container, Form, Button, Row, Col, Card } from "react-bootstrap";
import "bootstrap/dist/css/bootstrap.min.css";
import Sidebar from "../components/Sidebar";
import "./Settings.css";

const Settings = () => {
  const [userData, setUserData] = useState({
    firstName: "",
    lastName: "",
    email: "",
    address: "",
    currentPassword: "",
    newPassword: "",
    confirmPassword: "",
    deleteAccount: false,
  });

  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setUserData({
      ...userData,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    console.log("Updated Settings:", userData);
    alert("Settings updated successfully!");
  };

  return (
<Container fluid className="settings-container p-0 m-0">
{/* <Sidebar /> */}

      <Card className="settings-card p-4">
        <Card.Title className="mb-4">Settings Page</Card.Title>

        {/* Personal Information Section */}
        <Card className="p-3 mb-3 personal-info">
          <Card.Title>Personal Information</Card.Title>
          <div className="d-flex justify-content-between align-items-center mb-3">
            <div className="profile-image" style={{ width: "50px", height: "50px", borderRadius: "50%", backgroundColor: "#007bff", color: "#fff", display: "flex", alignItems: "center", justifyContent: "center" }}>🔵 </div>
            <div>
              <Button variant="info" className="me-2">Upload Image</Button>
              <Button variant="danger">Delete</Button>
            </div>
          </div>
          
          <Row className="align-items-center mt-3">
            <Col md={5}>
              <Form.Group controlId="firstName">
                <Form.Label>First Name</Form.Label>
                <Form.Control
                  type="text"
                  name="firstName"
                  value={userData.firstName}
                  onChange={handleChange}
                  placeholder="Enter your first name"
                />
              </Form.Group>
            </Col>
            <Col md={5}>
              <Form.Group controlId="lastName">
                <Form.Label>Last Name</Form.Label>
                <Form.Control
                  type="text"
                  name="lastName"
                  value={userData.lastName}
                  onChange={handleChange}
                  placeholder="Enter your last name"
                />
              </Form.Group>
            </Col>
            <Col md={2} className="mt-4">
              <Button variant="info" className="small-btn w-100">Change Name</Button>
            </Col>
          </Row>

          <Form.Group controlId="email" className="mt-3">
            <Form.Label>Email</Form.Label>
            <Row>
              <Col md={10}>
                <Form.Control
                  type="email"
                  name="email"
                  value={userData.email}
                  onChange={handleChange}
                  placeholder="Enter your email"
                />
              </Col>
              <Col md={2}>
                <Button variant="info" className="small-btn w-100">Change Email</Button>
              </Col>
            </Row>
          </Form.Group>

          <Form.Group controlId="address" className="mt-3">
            <Form.Label>Address</Form.Label>
            <Row>
              <Col md={10}>
                <Form.Control
                  type="text"
                  name="address"
                  value={userData.address}
                  onChange={handleChange}
                  placeholder="Enter your address"
                />
              </Col>
              <Col md={2}>
                <Button variant="info" className="small-btn w-100">Change Address</Button>
              </Col>
            </Row>
          </Form.Group>
        </Card>

        {/* Password Section */}
        <Card className="p-3 mb-3 password-section">
          <Card.Title>Password</Card.Title>
          <Row>
            <Col md={6}>
              <Form.Group controlId="currentPassword">
                <Form.Label>Current Password</Form.Label>
                <Form.Control type="password" name="currentPassword" value={userData.currentPassword} onChange={handleChange} />
              </Form.Group>
            </Col>
            <Col md={6}>
              <Form.Group controlId="newPassword">
                <Form.Label>New Password</Form.Label>
                <Form.Control type="password" name="newPassword" value={userData.newPassword} onChange={handleChange} />
              </Form.Group>
            </Col>
          </Row>
          <Form.Group controlId="confirmPassword" className="mt-3">
            <Form.Label>Confirm New Password</Form.Label>
            <Form.Control type="password" name="confirmPassword" value={userData.confirmPassword} onChange={handleChange} />
          </Form.Group>
        </Card>

        {/* Privacy Policy */}
        <Card className="p-3 mb-3 privacy-policy">
          <Card.Title>Privacy Policy</Card.Title>
          <p>We never share your personal information without consent.</p>
          <p>Your data is securely encrypted and stored.</p>
          <p>You can request access to your data anytime.</p>
          <a href="#">View Full Privacy Policy</a>
        </Card>

        {/* Delete Account Section */}
        <Card className="p-3 mb-3 delete-account">
          <Card.Title>Delete Account</Card.Title>
          <Form.Check
            type="checkbox"
            label="I understand that deleting my account is permanent and cannot be undone"
            name="deleteAccount"
            checked={userData.deleteAccount}
            onChange={handleChange}
          />
          <div className="d-flex justify-content-start">
            <Button variant="danger" className="mt-3 custom-btn" style={{ width: "auto" }}>Delete my account</Button>
          </div>
        </Card>

        {/* Save Changes Button */}
        <Button variant="primary" type="submit" className="mt-3 custom-btn" onClick={handleSubmit}>Save Changes</Button>
      </Card>
    </Container>
  );
};

export default Settings;
