import React from "react";
import "./SavingGoal.css";
const goals = [
  {
    title: "third goal",
    dateRange: "1/5/2025 to 1/9/2025",
    name: "Laptop",
    amount: "20000 LE",
    monthly: "4000 LE",
  },
  {
    title: "second goal",
    dateRange: "1/8/2023 to 1/1/2024",
    name: "college fees",
    amount: "30000 LE",
    monthly: "5000 LE",
  },
  {
    title: "first goal",
    dateRange: "1/11/2022 to 1/5/2023",
    name: "TV",
    amount: "7000 LE",
    monthly: "1000 LE",
  },
];

export default function SavingGoalsPage() {
  return (
    <div className="container-fluid bg-primary text-white min-vh-100 py-4">
      <div className="container">
        <h2 className="mb-4">Saving Goals Page</h2>

        <div className="d-flex justify-content-between align-items-center mb-4 flex-column flex-md-row">
          <div className="bg-lightt text-dark px-3 py-2 rounded-3">
            <strong>Total Savings:</strong> L.E20000
          </div>
          <div className="input-group mt-3 mt-md-0" style={{ maxWidth: "300px" }}>
            <input
              type="text"
              className="form-control pr-5"  // added padding to the right to make space for the icon
              placeholder="Search here..."
            />
            <i className="fa fa-search position-absolute" style={{ right: "15px", top: "50%", transform: "translateY(-50%)", color: "#6c757d" }}></i> {/* Font Awesome search icon inside the input */}
          </div>
        </div>

        {/* Goals display */}
        {goals.map((goal, index) => (
          <div className="row mb-3" key={index}>
            {/* Left Box */}
            <div className="col-12 col-md-3 mb-2 mb-md-0">
              <div className="bg-info text-white p-3 rounded text-center h-100 rounded-4">
                <h4 className="mb-2 ">{goal.title}</h4>
                <small>{goal.dateRange}</small>
              </div>
            </div>

            <div className="col-12 col-md-9">
              <div className="bg-lightt text-dark p-3 rounded h-100 rounded-4">
                <p><strong>Goal Name:</strong> {goal.name}</p>
                <p><strong>Goal Amount:</strong> {goal.amount}</p>
                <p><strong>Monthly savings:</strong> {goal.monthly}</p>
              </div>
            </div>
          </div>
        ))}

      </div>
    </div>
  );
}
