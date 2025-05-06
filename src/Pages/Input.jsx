import React, { useState } from 'react';
import { Container, Button } from 'react-bootstrap';
import styles from './Input.module.css';
import logo from "../assets/Logo Icon@2x.png";

const InputPage = () => {
  const inputs = [
    'Enter your age',
    'Enter your dependents',
    'Enter your occupation',
    'Enter your city_tier',
    'Enter your monthly income',
    'Enter your rent expense',
    'Enter your loan repayment expense',
    'Enter your insurance expense',
    'Enter your groceries expense',
    'Enter your transportation expense',
    'Enter your eating out expense',
    'Enter your entertainment expense',
    'Enter your utilities expense',
    'Enter your healthcare expense',
    'Enter your education expense',
    'Enter your miscellaneous expense',
  ];

  const [isEditingInput, setIsEditingInput] = useState(false);
  const [isEditingGoal, setIsEditingGoal] = useState(false);

  const toggleEditInput = () => setIsEditingInput(!isEditingInput);
  const toggleEditGoal = () => setIsEditingGoal(!isEditingGoal);

  return (
    <div className={styles.inputPage}>
      <Container className={styles.inputContainer}>
        {/* Header with logo */}
        <div className={styles.header}>
          <img src={logo} alt="Logo" className={styles.logoImg} />
          <h2>Wealth Wise</h2>
        </div>

        <h2 className={styles.title}>Enter Your Financial Details</h2>

        {/* Input Fields */}
        {inputs.map((label, index) => (
          <div className={styles.inputGroup} key={index}>
            <label>{label}</label>
            <input
              type="number"
              className={styles.inputField}
              disabled={!isEditingInput} // Disable input when not editing
            />
          </div>
        ))}

        {/* Add/Edit Button for Inputs */}
        <div className={styles.buttonWrapper}>
          <Button variant="outline-secondary" onClick={toggleEditInput} className={styles.inputButton}>
            {isEditingInput ? 'Save Input' : 'Edit Input'}
          </Button>
          <Button variant="outline-secondary" onClick={toggleEditInput} className={styles.inputButton}>
            {isEditingInput ? 'Save Input' : 'Add Input'}
          </Button>
        </div>

        {/* Saving Goal Section */}
        <h5 className={styles.savingTitle}>Saving Goal:</h5>

        {/* Goal Fields */}
        <div className={styles.inputGroup}>
          <label>Goal Name</label>
          <input type="text" className={styles.inputField} disabled={!isEditingGoal} />
        </div>

        <div className={styles.inputGroup}>
          <label>Goal Amount</label>
          <input type="number" className={styles.inputField} disabled={!isEditingGoal} />
        </div>

        {/* Add/Edit Button for Goals */}
        <div className={styles.buttonWrapper}>
          <Button variant="outline-secondary" onClick={toggleEditGoal} className={styles.inputButton}>
            {isEditingGoal ? 'Save Goal' : 'Edit Goal'}
          </Button>
          <Button variant="outline-secondary" onClick={toggleEditGoal} className={styles.inputButton}>
            {isEditingGoal ? 'Save Goal' : 'Add Goal'}
          </Button>
        </div>

        {/* <div className={styles.confirmWrapper}>
          <Button variant="success" size="lg" className={styles.confirmBtn}>
            Confirm
          </Button>
        </div> */}
      </Container>
    </div>
  );
};

export default InputPage;
