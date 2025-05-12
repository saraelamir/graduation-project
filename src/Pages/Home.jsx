import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
  FiShoppingCart,
  FiTruck,
  FiCoffee,
  FiFilm,
  FiZap,
  FiHeart,
  FiBook,
  FiCpu,
  FiSave,
} from "react-icons/fi";
import { PieChart, Pie, Cell, Tooltip, ResponsiveContainer } from "recharts";
import { getCurrentPlan } from "../api/planService";
import styles from "./Home.module.css";

const COLORS = ["#4682B4", "#1E90FF", "#00CED1", "#5F9EA0", "#87CEFA"];

const ICONS = {
  Groceries: <FiShoppingCart />,
  Transport: <FiTruck />,
  "Eating Out": <FiCoffee />,
  Entertainment: <FiFilm />,
  Utilities: <FiZap />,
  Healthcare: <FiHeart />,
  Education: <FiBook />,
  "Other Money": <FiCpu />,
  "Monthly Savings": <FiSave />,
};

function Home() {
  const [expenses, setExpenses] = useState([]);
  const [goal, setGoal] = useState(null);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();

  useEffect(() => {
    getCurrentPlan()
      .then((res) => {
        const data = res.data;
        if (!data.plans?.length) return navigate("/welcome");

        const {
          goalName,
          goalAmount,
          startDate,
          endDate,
          status,
        } = data;

        setGoal({
          goalName,
          goalAmount,
          status,
          startDate: startDate.split("T")[0],
          endDate: endDate.split("T")[0],
        });

        const plan = data.plans[0];
        setExpenses([
          { category: "Groceries", amount: plan.groceries },
          { category: "Transport", amount: plan.transport },
          { category: "Eating Out", amount: plan.eatingOut },
          { category: "Entertainment", amount: plan.entertainment },
          { category: "Utilities", amount: plan.utilities },
          { category: "Healthcare", amount: plan.healthcare },
          { category: "Education", amount: plan.education },
          { category: "Other Money", amount: plan.otherMoney },
           { category: "Monthly Savings", amount: plan.monthlySavings },

        ]);
      })
      .catch((err) => {
        if (err.response?.status === 401) navigate("/login");
        else console.error(err);
      })
      .finally(() => setLoading(false));
  }, [navigate]);

  if (loading) return <div className="text-center mt-5">Loading...</div>;
  if (!goal)
    return <div className="text-center mt-5">No active saving goal found.</div>;

  const total = expenses.reduce((sum, e) => sum + e.amount, 0);
  const pieData = expenses
    .filter((e) => e.category !== "Monthly Savings")
    .map((e) => ({
      name: e.category,
      value: e.amount,
      percentage: ((e.amount / total) * 100).toFixed(1),
    }));

  return (
    <div className={`container-fluid py-4 px-5 ${styles.container}`}>
      <h2 className="mb-4">Home Dashboard</h2>

      <div className="row mb-4">
        <div className="col-12">
          <div className={styles.goalCard}>
            <h5>Saving Goal: {goal.goalName}</h5>
            <p>Target: {goal.goalAmount} L.E</p>
            <p>Status: {goal.status}</p>
            <p>
              Duration: {goal.startDate} – {goal.endDate}
            </p>
          </div>
        </div>
      </div>

      <div className="row mb-5">
        {expenses.map((exp, idx) => (
          <div className="col-sm-6 col-md-4 mb-4" key={idx}>
            <div
              className={`expenseCard p-3 text-white h-100 text-center ${
                exp.category === "Monthly Savings"
                  ? styles.bgDanger
                  : idx % 2 === 0
                  ? styles.bgPrimary
                  : styles.bgSecondary
              }`}
            >
              <div className="mb-2" style={{ fontSize: "1.5rem" }}>
                {ICONS[exp.category]}
              </div>
              <div>{exp.category}</div>
              <div style={{ fontWeight: "bold" }}>{exp.amount} L.E</div>
            </div>
          </div>
        ))}
      </div>

      <div style={{ height: "300px" }}>
        <ResponsiveContainer>
          <PieChart>
            <Pie
              data={pieData}
              dataKey="value"
              nameKey="name"
              cx="50%"
              cy="50%"
              outerRadius={100}
              label={({ name, percentage }) => `${name}: ${percentage}%`}
            >
              {pieData.map((entry, index) => (
                <Cell
                  key={`cell-${index}`}
                  fill={COLORS[index % COLORS.length]}
                />
              ))}
            </Pie>
            <Tooltip />
          </PieChart>
        </ResponsiveContainer>
      </div>
    </div>
  );
}

export default Home;
