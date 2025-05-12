// src/pages/Settings.jsx
import React, { useState, useEffect, useRef } from "react";
import {
  Container,
  Form,
  Button,
  Row,
  Col,
  Card,
  Alert,
  Image,
} from "react-bootstrap";
import VerifyCode from "./VerifyCode";
import * as userService from "../api/userProfileService";
import "./Settings.css";

const Settings = () => {
  const fileInputRef = useRef(null);

  const [profile, setProfile] = useState({
    firstName: "",
    lastName: "",
    email: "",
    country: "",
  });
  const [passwords, setPasswords] = useState({
    current: "",
    new: "",
    confirm: "",
  });
  const [previewUrl, setPreviewUrl] = useState("");
  const [errors, setErrors] = useState({});
  const [success, setSuccess] = useState("");
  const [showVerify, setShowVerify] = useState(false);
  const [verifyContext, setVerifyContext] = useState("");
  const [isSaving, setIsSaving] = useState(false);

  useEffect(() => {
    userService
      .getUserProfile()
      .then((res) => {
        const p = res.data;
        setProfile({
          firstName: p.firstName || "",
          lastName: p.lastName || "",
          email: p.email || "",
          country: p.country || "",
        });
        if (p.profilePictureUrl) setPreviewUrl(p.profilePictureUrl);
      })
      .catch(() => setErrors({ fetch: "Failed to load profile data." }));
  }, []);

  const handleProfileChange = (e) => {
    const { name, value } = e.target;
    setProfile((prev) => ({ ...prev, [name]: value }));
  };
  const handlePasswordChangeInput = (e) => {
    const { name, value } = e.target;
    setPasswords((prev) => ({ ...prev, [name]: value }));
  };
  const handleFileClick = () => fileInputRef.current.click();

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (!file) return;
    const objectUrl = URL.createObjectURL(file);
    setPreviewUrl(objectUrl);
    setIsSaving(true);

    userService
      .uploadProfilePicture(file, Boolean(previewUrl))
      .then(() => userService.getUserProfile())
      .then((res) => {
        const p = res.data;
        if (p.profilePictureUrl) {
          setPreviewUrl(p.profilePictureUrl);
          setSuccess("Profile picture updated.");
        }
      })
      .catch(() => setErrors({ picture: "Upload failed." }))
      .finally(() => setIsSaving(false));
  };

  const handlePictureDelete = () => {
    setIsSaving(true);
    userService
      .deleteProfilePicture()
      .then(() => {
        setPreviewUrl("");
        setSuccess("Picture removed.");
      })
      .catch(() => setErrors({ picture: "Deletion failed." }))
      .finally(() => setIsSaving(false));
  };

  const handleNameUpdate = () => {
    setIsSaving(true);
    userService
      .updateName(profile.firstName, profile.lastName)
      .then(() => setSuccess("Name updated."))
      .catch(() => setErrors({ name: "Update failed." }))
      .finally(() => setIsSaving(false));
  };
const handleEmailUpdate = () => {
  setIsSaving(true);
  userService
    .updateEmail(profile.email)
    .then(() => {
      setSuccess("Email updated.");
    })
    .catch(() => setErrors({ email: "Failed to send verification code." }))
    .finally(() => setIsSaving(false));
};
  const handleCountryUpdate = () => {
    setIsSaving(true);
    userService
      .updateCountry(profile.country)
      .then(() => setSuccess("Country updated."))
      .catch(() => setErrors({ country: "Update failed." }))
      .finally(() => setIsSaving(false));
  };
  const handlePasswordUpdate = () => {
    if (passwords.new !== passwords.confirm) {
      setErrors({ password: "Passwords do not match." });
      return;
    }
    setIsSaving(true);
    userService
      .changePassword(passwords.current, passwords.new, passwords.confirm)
      .then(() => {
        setVerifyContext("password-change");
        setShowVerify(true);
      })
      .catch(() => setErrors({ password: "Failed to send verification code." }))
      .finally(() => setIsSaving(false));
  };
  const handleAccountDeletion = () => {
    if (!window.confirm("This will permanently delete your account. Continue?"))
      return;
    userService
      .deleteAccount()
      .then(() => {
        localStorage.removeItem("token");
        window.location.href = "/";
      })
      .catch(() => setErrors({ delete: "Deletion failed." }));
  };
  const onVerified = (code) => {
    const fn =
      verifyContext === "email-change"
        ? userService.verifyEmailChange
        : userService.verifyPasswordChange;
    fn(code)
      .then(() => window.location.reload())
      .catch(() => alert("Verification failed."));
  };

  return (
    <Container fluid className="px-0">
      <Card className="settings-card p-5 mb-5">
        <Card.Title className="display-5 mb-4 text-white">Settings</Card.Title>

        {errors.fetch && <Alert variant="danger">{errors.fetch}</Alert>}
        {success && <Alert variant="success">{success}</Alert>}

        {/* Profile Picture */}
        <Card className="p-1 mb-4 personal-info">
          <h5 className="mb-3">Profile Picture</h5>
          <Row className="align-items-center">
            <Col xs={12} md={6} className="text-center text-md-left mb-3">
              {previewUrl ? (
                <Image
                  src={previewUrl}
                  roundedCircle
                  className="border border-light pp-image"
                />
              ) : (
                <div className="rounded-circle bg-secondary pp-image" />
              )}
              <input
                ref={fileInputRef}
                type="file"
                accept="image/*"
                onChange={handleFileChange}
                className="d-none"
              />
            </Col>
            <Col xs={12} md={6} className="text-center text-md-right">
              <div className="d-flex flex-wrap justify-content-center justify-content-md-end gap-2">
                <Button
                  variant="outline-info"
                  onClick={handleFileClick}
                  disabled={isSaving}
                  className="pp-btn"
                >
                  Upload
                </Button>
                <Button
                  variant="outline-danger"
                  onClick={handlePictureDelete}
                  disabled={isSaving || !previewUrl}
                  className="pp-btn"
                >
                  Delete
                </Button>
              </div>
            </Col>
          </Row>
        </Card>

        {/* Personal Information */}
        <Card className="p-4 mb-4 personal-info">
          <h5 className="mb-3">Personal Information</h5>
          <Form>
            <Row className="g-3 align-items-end">
              <Col md={5}>
                <Form.Group controlId="firstName">
                  <Form.Label>First Name</Form.Label>
                  <Form.Control
                    type="text"
                    name="firstName"
                    value={profile.firstName}
                    onChange={handleProfileChange}
                    required
                  />
                </Form.Group>
              </Col>
              <Col md={5}>
                <Form.Group controlId="lastName">
                  <Form.Label>Last Name</Form.Label>
                  <Form.Control
                    type="text"
                    name="lastName"
                    value={profile.lastName}
                    onChange={handleProfileChange}
                    required
                  />
                </Form.Group>
              </Col>
              <Col md={2} className="d-grid">
                <Button
                  variant="primary"
                  onClick={handleNameUpdate}
                  disabled={isSaving}
                >
                  Update
                </Button>
              </Col>
            </Row>

            <Row className="g-3 align-items-end mt-3">
              <Col md={10}>
                <Form.Group controlId="email">
                  <Form.Label>Email</Form.Label>
                  <Form.Control
                    type="email"
                    name="email"
                    value={profile.email}
                    onChange={handleProfileChange}
                    required
                  />
                </Form.Group>
              </Col>
              <Col md={2} className="d-grid">
                <Button
                  variant="primary"
                  onClick={handleEmailUpdate}
                  disabled={isSaving}
                >
                  Change
                </Button>
              </Col>
            </Row>

            <Row className="g-3 align-items-end mt-3">
              <Col md={10}>
                <Form.Group controlId="country">
                  <Form.Label>Country</Form.Label>
                  <Form.Control
                    type="text"
                    name="country"
                    value={profile.country}
                    onChange={handleProfileChange}
                    required
                  />
                </Form.Group>
              </Col>
              <Col md={2} className="d-grid">
                <Button
                  variant="primary"
                  onClick={handleCountryUpdate}
                  disabled={isSaving}
                >
                  Update
                </Button>
              </Col>
            </Row>
          </Form>
        </Card>

        {/* Change Password */}
        <Card className="p-4 mb-4 personal-info">
          <h5 className="mb-3">Change Password</h5>
          <Form>
            <Row className="g-3 align-items-end">
              <Col md={4}>
                <Form.Group controlId="currentPassword">
                  <Form.Label>Current Password</Form.Label>
                  <Form.Control
                    type="password"
                    name="current"
                    value={passwords.current}
                    onChange={handlePasswordChangeInput}
                    required
                  />
                </Form.Group>
              </Col>
              <Col md={4}>
                <Form.Group controlId="newPassword">
                  <Form.Label>New Password</Form.Label>
                  <Form.Control
                    type="password"
                    name="new"
                    value={passwords.new}
                    onChange={handlePasswordChangeInput}
                    required
                  />
                </Form.Group>
              </Col>
              <Col md={4}>
                <Form.Group controlId="confirmPassword">
                  <Form.Label>Confirm Password</Form.Label>
                  <Form.Control
                    type="password"
                    name="confirm"
                    value={passwords.confirm}
                    onChange={handlePasswordChangeInput}
                    required
                  />
                </Form.Group>
              </Col>
            </Row>
            <div className="d-grid mt-3">
              <Button
                variant="primary"
                onClick={handlePasswordUpdate}
                disabled={isSaving}
              >
                Change Password
              </Button>
            </div>
          </Form>
        </Card>

        {/* Danger Zone */}
        <Card className="p-4 personal-info">
          <h5 className="mb-3 text-danger">Danger Zone</h5>
          <div className="d-grid">
            <Button
              variant="danger"
              onClick={handleAccountDeletion}
              className="dz-btn"
            >
              Delete Account
            </Button>
          </div>
        </Card>
      </Card>

      {showVerify && (
        <VerifyCode
          show={showVerify}
          onHide={() => setShowVerify(false)}
          onVerified={onVerified}
        />
      )}
    </Container>
  );
};

export default Settings;