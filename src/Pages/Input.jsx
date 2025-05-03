import React from 'react';
import { Container, Button } from 'react-bootstrap';
import styles from './Input.module.css'; 
import logo from "../assets/Logo Icon@2x.png";

const InputPage = () => {
  const inputs = [
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

  return (
    <div className={styles.inputPage}>
      <Container className={styles.inputContainer}>
        {/* Header with logo */}
        <div className={styles.header}>
          <img src={logo} alt="Logo" className={styles.logoImg} />
          <h2>Wealth Wise</h2>
        </div>
        
        <h2 className={styles.title}>Enter Your Financial Details</h2>

        {inputs.map((label, index) => (
          <div className={styles.inputGroup} key={index}>
            <label>{label}</label>
            <input type="number" className={styles.inputField} />
            <Button variant="outline-secondary" className={styles.inputButton}>Edit</Button>
          </div>
        ))}

        {/* Saving Goal Section */}
        <h5 className={styles.savingTitle}>Saving Goal:</h5>

        <div className={styles.inputGroup}>
          <label>Goal Name</label>
          <input type="text" className={styles.inputField} />
          <Button variant="outline-secondary" className={styles.inputButton}>Edit</Button>
        </div>

        <div className={styles.inputGroup}>
          <label>Goal Amount</label>
          <input type="number" className={styles.inputField} />
          <Button variant="outline-secondary" className={styles.inputButton}>Edit</Button>
        </div>

        <div className={styles.confirmWrapper}>
          <Button variant="success" size="lg" className={styles.confirmBtn}>Confirm</Button>
        </div>
      </Container>
    </div>
  );
};

export default InputPage;
