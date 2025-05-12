import React, { useState } from 'react';
import { Container, Button } from 'react-bootstrap';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

import styles from './Input.module.css';
import logo from "../assets/Logo Icon@2x.png";

const InputPage = () => {

  const navigate = useNavigate(); // استخدام التوجيه

  const occupationOptions = ['Student', 'Employee', 'Freelancer', 'Unemployed'];
  const cityTierOptions = ['1', '2', '3'];

  const [formData, setFormData] = useState({
    age: '',
    dependents: '',
    occupation: '',
    city_tier: '',
    income: '',
    rent: '',
    loanPayment: '',
    insurance: '',
    groceries: '',
    transport: '',
    eatingOut: '',
    entertainment: '',
    utilities: '',
    healthcare: '',
    education: '',
    otherMony: '',
  });

  const [isDataAdded, setIsDataAdded] = useState(false);
  const [goalData, setGoalData] = useState({
    goal_name: '',
    goal_amount: '',
  });
  const [isGoalAdded, setIsGoalAdded] = useState(false);

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setFormData(prev => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleGoalChange = (e) => {
    const { name, value } = e.target;
    setGoalData(prev => ({
      ...prev,
      [name]: value,
    }));
  };

  // التحقق من صحة البيانات (رسالة عامة)
  const validateFormData = () => {
    if (Object.values(formData).some(value => value === '')) {
      alert('Please fill out all fields');
      return false;
    }
    return true;
  };

  // التحقق من صحة بيانات الهدف
  const validateGoalData = () => {
    if (!goalData.goal_name || !goalData.goal_amount) {
      alert('Please fill out all goal fields');
      return false;
    }
    if (isNaN(goalData.goal_amount) || Number(goalData.goal_amount) <= 0) {
      alert('Please enter a valid goal amount.');
      return false;
    }
    return true;
  };

  const handleAddInput = async () => {
    if (!validateFormData()) return;
    try {
      const token = localStorage.getItem("token");
      const response = await axios.post(
        'https://graduproj.runasp.net/api/Input/add_inputs',
        formData,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      alert(response.data.message || 'Data successfully added!');
      setIsDataAdded(true); // تم إضافة البيانات
      setFormData({
        age: '', dependents: '', occupation: '', city_tier: '', income: '', rent: '', loanPayment: '', insurance: '',
        groceries: '', transport: '', eatingOut: '', entertainment: '', utilities: '', healthcare: '', education: '', otherMony: ''
      });
      navigate('/'); // التوجيه إلى الصفحة الرئيسية
    } catch (error) {
      handleError(error);
    }
  };

  const handleUpdateInput = async () => {
    if (!isDataAdded) {
      alert("Please add data first before updating.");
      return;
    }
    if (!validateFormData()) return;
    try {
      const token = localStorage.getItem("token");
      const response = await axios.put(
        'https://graduproj.runasp.net/api/Input/update_inputs',
        formData,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      alert(response.data.message || 'Data successfully updated!');
      setIsDataAdded(false); // إعادة تعيين حالة البيانات المضافة
      setFormData({
        age: '', dependents: '', occupation: '', city_tier: '', income: '', rent: '', loanPayment: '', insurance: '',
        groceries: '', transport: '', eatingOut: '', entertainment: '', utilities: '', healthcare: '', education: '', otherMony: ''
      });
      navigate('/home'); // التوجيه إلى الصفحة الرئيسية
    } catch (error) {
      handleError(error);
    }
  };

  const handleAddGoal = async () => {
    if (!validateGoalData()) return;
    try {
      const token = localStorage.getItem("token");
      const response = await axios.post(
        'https://graduproj.runasp.net/api/Goal/add_goal',
        goalData,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      alert(response.data.message || 'Goal successfully added!');
      setIsGoalAdded(true); // تم إضافة الهدف
      setGoalData({ goal_name: '', goal_amount: '' });
      navigate('/home'); // التوجيه إلى الصفحة الرئيسية
    } catch (error) {
      handleError(error);
    }
  };

  const handleUpdateGoal = async () => {
    if (!isGoalAdded) {
      alert("Please add a goal first before updating.");
      return;
    }
    if (!validateGoalData()) return;
    try {
      const token = localStorage.getItem("token");
      const response = await axios.put(
        'https://graduproj.runasp.net/api/Goal/update_goal',
        goalData,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      alert(response.data.message || 'Goal successfully updated!');
      setIsGoalAdded(false); // إعادة تعيين حالة الهدف المضاف
      setGoalData({ goal_name: '', goal_amount: '' });
      navigate('/home'); // التوجيه إلى الصفحة الرئيسية
    } catch (error) {
      handleError(error);
    }
  };

  const handleError = (error) => {
    if (error.response) {
      console.error('Error response:', error.response);
      alert(error.response.data.message || `Server error (${error.response.status})`);
    } else if (error.request) {
      console.error('Error request:', error.request);
      alert('No response from server. Please check your network.');
    } else {
      console.error('Error message:', error.message);
      alert('Unexpected error: ' + error.message);
    }
  };

  return (
    <div className={styles.inputPage}>
      <Container className={styles.inputContainer}>
        <div className={styles.header}>
          <img src={logo} alt="Logo" className={styles.logoImg} />
          <h2>Wealth Wise</h2>
        </div>

        <h2 className={styles.title}>Enter Your Financial Details</h2>

        {Object.entries(formData).map(([key, value], index) => {
          const label = key.replace(/_/g, ' ').replace(/\b\w/g, l => l.toUpperCase());
          const isOccupation = key === 'occupation';
          const isCityTier = key === 'city_tier';
          const displayLabel = `Enter your ${label}`;

          return (
            <div className={styles.inputGroup} key={index}>
              <label>{displayLabel}</label>

              {isOccupation ? (
                <select name={key} value={value} className={styles.inputField} onChange={handleInputChange}>
                  <option value=""></option>
                  {occupationOptions.map((opt, i) => (
                    <option key={i} value={opt}>{opt}</option>
                  ))}
                </select>
              ) : isCityTier ? (
                <select name={key} value={value} className={styles.inputField} onChange={handleInputChange}>
                  <option value=""></option>
                  {cityTierOptions.map((tier, i) => (
                    <option key={i} value={tier}>{tier}</option>
                  ))}
                </select>
              ) : (
                <input
                  type="number"
                  name={key}
                  value={value}
                  className={styles.inputField}
                  onChange={handleInputChange}
                />
              )}
            </div>
          );
        })}

        <div className={styles.buttonGroup}>
          <Button variant="success" onClick={handleAddInput} className="me-2">Add Input</Button>
          <Button variant="success" onClick={handleUpdateInput}>Update Input</Button>
        </div>

        <h2 className={styles.title}>Add Your Saving Goal</h2>

        <div className={styles.inputGroup}>
          <label>Goal Name</label>
          <input
            type="text"
            name="goal_name"
            value={goalData.goal_name}
            className={styles.inputField}
            onChange={handleGoalChange}
          />
        </div>

        <div className={styles.inputGroup}>
          <label>Goal Amount</label>
          <input
            type="number"
            name="goal_amount"
            value={goalData.goal_amount}
            className={styles.inputField}
            onChange={handleGoalChange}
          />
        </div>

        <div className={styles.buttonGroup}>
          <Button variant="success" onClick={handleAddGoal} className="me-2">Add Goal</Button>
          <Button variant="success" onClick={handleUpdateGoal}>Update Goal</Button>
        </div>

      </Container>
    </div>
  );
};

export default InputPage;
