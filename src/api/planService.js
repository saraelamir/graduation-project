// src/api/planService.js
import axios from "axios";
import { API_BASE } from "./userProfileService";

// Create an axios instance for JSON requests, with auth header
const api = axios.create({
  baseURL: API_BASE,
  headers: { "Content-Type": "application/json" },
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

// Fetch the current saving goal & its plan
export const getCurrentPlan = () =>
  api.get("/api/Goal/current_goal_with_Current_Plan");