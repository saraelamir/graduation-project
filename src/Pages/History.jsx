import React, { useEffect, useState } from "react";
import axios from "axios";
import styles from './History.module.css';

const CompletedGoals = () => {
  const [goals, setGoals] = useState([]);
  const [error, setError] = useState("");
  const [showPlan, setShowPlan] = useState({});

  // ✅ جلب التوكن من localStorage
  const token = localStorage.getItem("token");

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await axios.get(
          "https://graduproj.runasp.net/api/Goal/AllCompleted_goals",
          {
            headers: {
              Authorization: `Bearer ${token}`
            }
          }
        );
        setGoals(response.data);
      } catch (error) {
        console.error("Error fetching goals:", error);
        setError("Failed to load completed goals.");
      }
    };

    fetchData();
  }, [token]);

  const togglePlanVisibility = (index) => {
    setShowPlan((prevState) => ({
      ...prevState,
      [index]: !prevState[index],
    }));
  };

  return (
    <div className={styles.completedGoalsContainer}>
      <h2 className={styles.completedGoalsTitle}>Completed Goals</h2>
      {error && <p style={{ color: "red" }}>{error}</p>}
      {goals.map((goal, index) => (
        <div key={index} className={styles.goalItemCard}>
          <h3 className={styles.goalName}>{goal.goalName}</h3>
          <p><strong>Amount:</strong> {goal.goalAmount}</p>
          <p><strong>Start Date:</strong> {new Date(goal.startDate).toLocaleDateString()}</p>
          <p><strong>End Date:</strong> {new Date(goal.endDate).toLocaleDateString()}</p>
          <p><strong>Status:</strong> {goal.status}</p>
          <p><strong>Has Savings:</strong> {goal.has_Savings ? "Yes" : "No"}</p>

          <button className={styles.showPlanButton} onClick={() => togglePlanVisibility(index)}>
            {showPlan[index] ? "Hide Plan" : "Show Plan"}
          </button>

{showPlan[index] && goal.plans && (
  <div className={styles.planContainer}>
    <h6 className={styles.planTitle}>Plan:</h6>
    {goal.plans.map((plan, i) => (
      <div key={i}>
        <div className={styles.planItemsGrid}>
          <div className={styles.planItem}>🛒
          Groceries: {plan.groceries}L.E</div>
          <div className={styles.planItem}>🚗 Transport: {plan.transport}L.E</div>
          <div className={styles.planItem}>🍽 Eating Out: {plan.eatingOut}L.E</div>
          <div className={styles.planItem}>🎮 Entertainment: {plan.entertainment}L.E</div>
          <div className={styles.planItem}>💡 Utilities: {plan.utilities}L.E</div>
          <div className={styles.planItem}>🏥 Healthcare: {plan.healthcare}L.E</div>
          <div className={styles.planItem}>📚 Education: {plan.education}L.E</div>
          <div className={styles.planItem}>💼 Other Money: {plan.otherMoney}L.E</div>
          <div className={styles.planItem}>💰 Monthly Savings: {plan.monthlySavings}L.E</div>
        </div>

        <p>📅 Plan Start: {new Date(plan.startDate).toLocaleDateString()}</p>
        <p>📅 Plan End: {new Date(plan.endDate).toLocaleDateString()}</p>
        <p>📊 Plan Status: {plan.status}</p>
      </div>
    ))}
  </div>
)}
        </div>
      ))}
    </div>
  );
};

export default CompletedGoals;
