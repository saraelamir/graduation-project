import React, { useState, useEffect } from 'react';
import { Container, Button, Toast } from 'react-bootstrap';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

import styles from './Input.module.css';
import logo from "../assets/Logo Icon@2x.png";

const InputPage = () => {
  const navigate = useNavigate();

  const occupationOptions = ['Student', 'Self_Employed', 'Retierd', 'Professional'];
  const cityTierOptions = ['Tier_1', 'Tier_2', 'Tier_3'];
  const [hasAddedInput, setHasAddedInput] = useState(false);
  const [hasAddedGoal, setHasAddedGoal] = useState(false);

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
    otherMoney: '',
  });

  const [goalData, setGoalData] = useState({
    goalName: '',
    goalAmount: '',
  });

  const [toast, setToast] = useState({ show: false, message: '', type: '' });

  useEffect(() => {
    const fetchLatestInput = async () => {
      try {
        const token = localStorage.getItem("token");
        const response = await axios.get(
          'https://graduproj.runasp.net/api/Input/latest-input',
          { headers: { Authorization: `Bearer ${token}` } }
        );
        if (response.data) {
          setFormData(response.data);
          setHasAddedInput(true);
        }
      } catch (error) {
        console.error('Failed to fetch latest input:', error);
      }
    };

const fetchLatestGoal = async () => {
  try {
    const token = localStorage.getItem("token");
    const response = await axios.get(
      'https://graduproj.runasp.net/api/Goal/last',
      { headers: { Authorization: `Bearer ${token}` } }
    );
    if (response.data) {
      setGoalData(response.data);
      setHasAddedGoal(true); 
    }
  } catch (error) {
    console.error('Failed to fetch latest goal:', error);
  }
};

    fetchLatestInput();
    fetchLatestGoal();
  }, []);

  const handleInputChange = (e) => {
    const { name, value} = e.target;
    setFormData(prev => ({
      ...prev,
      [name]:value,
    }));
  };

  const handleGoalChange = (e) => {
    const { name, value } = e.target;
    setGoalData(prev => ({
      ...prev,
      [name]: value,
    }));
  };

  const validateFormData = () => {
    if (Object.values(formData).some(value => value === '')) {
      showToast('Please fill out all fields', 'error');
      return false;
    }
    return true;
  };

  const validateGoalData = () => {
    if (!goalData.goalName || !goalData.goalAmount) {
      showToast('Please fill out all goal fields', 'error');
      return false;
    }
    if (isNaN(goalData.goalAmount) || Number(goalData.goalAmount) <= 0) {
      showToast('Please enter a valid goal amount.', 'error');
      return false;
    }
    return true;
  };

  const showToast = (message, type) => {
    setToast({ show: true, message, type });
  };

  const handleAddInput = async () => {
    if (hasAddedInput) {
      showToast("You already added your input. You can only update it.", 'error');
      return;
    }

    if (!validateFormData()) return;

    try {
      const token = localStorage.getItem("token");
      const response = await axios.post(
        'https://graduproj.runasp.net/api/Input/add_inputs',
        formData,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      showToast(response.data.message || 'Data successfully added!', 'success');
      setHasAddedInput(true);
    } catch (error) {
      handleError(error);
    }
  };

  const handleUpdateInput = async () => {
    if (!validateFormData()) return;
    try {
      const token = localStorage.getItem("token");
      const response = await axios.put(
        'https://graduproj.runasp.net/api/Input/update_inputs',
        formData,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      showToast(response.data.message || 'Data successfully updated!', 'success');
      navigate('/home');
    } catch (error) {
      handleError(error);
    }
  };

const handleAddGoal = async () => {
  if (hasAddedGoal) {
    showToast("You already added your goal. You can only update it.", 'error');
    return;
  }

  if (!validateGoalData()) return;

  try {
    const token = localStorage.getItem("token");
    const response = await axios.post(
      'https://graduproj.runasp.net/api/Goal/add_goal',
      goalData,
      { headers: { Authorization: `Bearer ${token}` } }
    );
    showToast(response.data.message || 'Goal successfully added!', 'success');
    setHasAddedGoal(true); 
    navigate('/home');
  } catch (error) {
    handleError(error);
  }
};

  const handleUpdateGoal = async () => {
    if (!validateGoalData()) return;
    try {
      const token = localStorage.getItem("token");
      const response = await axios.put(
        'https://graduproj.runasp.net/api/Goal/update_goal',
        goalData,
        { headers: { Authorization: `Bearer ${token}` } }
      );
      showToast(response.data.message || 'Goal successfully updated!', 'success');
      navigate('/home');
    } catch (error) {
      handleError(error);
    }
  };

  const handleError = (error) => {
    if (error.response) {
      console.error('Error response:', error.response);
      showToast(error.response.data.message || `Server error (${error.response.status})`, 'error');
    } else if (error.request) {
      console.error('Error request:', error.request);
      showToast('No response from server. Please check your network.', 'error');
    } else {
      console.error('Error message:', error.message);
      showToast('Unexpected error: ' + error.message, 'error');
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
          const label = key
            .replace(/_/g, ' ')
            .replace(/\b\w/g, l => l.toUpperCase())
            .replace('Mony', 'Money');
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
            name="goalName"
            value={goalData.goalName}
            className={styles.inputField}
            onChange={handleGoalChange}
          />
        </div>

        <div className={styles.inputGroup}>
          <label>Goal Amount</label>
          <input
            type="number"
            name="goalAmount"
            value={goalData.goalAmount}
            className={styles.inputField}
            onChange={handleGoalChange}
          />
        </div>

        <div className={styles.buttonGroup}>
          <Button variant="success" onClick={handleAddGoal} className="me-2">Add Goal</Button>
          <Button variant="success" onClick={handleUpdateGoal}>Update Goal</Button>
        </div>
      </Container>

      {/* Toast in center of screen */}
      {toast.show && (
        <div className={styles.toastCenter}>
          <Toast
            bg={toast.type === 'error' ? 'danger' : 'success'}
            onClose={() => setToast(prev => ({ ...prev, show: false }))}
            show={toast.show}
            delay={3000}
            autohide
          >
            <Toast.Body className="text-white">{toast.message}</Toast.Body>
          </Toast>
        </div>
      )}
    </div>
  );
};

export default InputPage;
