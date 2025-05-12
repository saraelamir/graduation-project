// src/api/userProfileService.js
import axios from "axios";

// Read API base URL from Vite env, fallback to production URL
export const API_BASE =
  import.meta.env.VITE_API_BASE || "https://graduproj.runasp.net";

// Axios instance for JSON requests
const api = axios.create({
  baseURL: API_BASE,
  headers: { "Content-Type": "application/json" },
});

// Axios instance for multipart/form-data (for file uploads)
const apiFile = axios.create({
  baseURL: API_BASE,
  headers: { "Content-Type": "multipart/form-data" },
});

// Attach JWT token from localStorage to each request
[api, apiFile].forEach((instance) => {
  instance.interceptors.request.use((config) => {
    const token = localStorage.getItem("token");
    if (token) {
    config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
  });
});

// === API methods ===
// Fetch current user profile
export const getUserProfile = () => api.get("/api/UserProfile/profile");

// Upload or replace profile picture
// hasPicture: true → replace (PUT), false → first-time upload (POST)
export const uploadProfilePicture = (file, hasPicture) => {
  const formData = new FormData();
  formData.append("file", file);
  if (hasPicture) {
    return apiFile.put("/api/UserProfile/update-profile-picture", formData);
  } else {
    return apiFile.post("/api/UserProfile/upload-profile-picture", formData);
  }
};

// Delete profile picture
export const deleteProfilePicture = () =>
  api.delete("/api/UserProfile/delete-profile-picture");

// Update first & last name
export const updateName = (firstName, lastName) =>
  api.put("/api/UserProfile/update-name", { firstName, lastName });

// Request email change (sends verification code)
export const updateEmail = (email) =>
  api.put("/api/UserProfile/update-email", { email });

// Update country
export const updateCountry = (country) =>
  api.put("/api/UserProfile/update-country", { country });

// Request password change (sends verification code)
export const changePassword = (
  currentPassword,
  newPassword,
  confirmNewPassword
) =>
  api.put("/api/UserProfile/change-password", {
    currentPassword,
    newPassword,
    confirmNewPassword,
  });

// Confirm email change with OTP code
export const verifyEmailChange = (code) =>
  api.post("/api/UserProfile/verify-email-change", { code });

// Confirm password change with OTP code
export const verifyPasswordChange = (code) =>
  api.post("/api/Account/verify-code", { code });

// Delete user account permanently
export const deleteAccount = () =>
  api.delete("/api/UserProfile/delete-account");