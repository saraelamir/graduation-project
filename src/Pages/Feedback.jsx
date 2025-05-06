import React, { useState } from "react";
import "bootstrap/dist/css/bootstrap.min.css";
import "./Feedback.css";

const Feedback = () => {
  const [navigationEase, setNavigationEase] = useState("");
  const [budgetHelpfulnessRating, setBudgetHelpfulnessRating] = useState(0);
  const [confusingFeatures, setConfusingFeatures] = useState("");
  const [desiredFeatures, setDesiredFeatures] = useState("");
  const [recommendationReason, setRecommendationReason] = useState("");
  const [improvementSuggestion, setImprovementSuggestion] = useState("");
  const [overallSatisfaction, setOverallSatisfaction] = useState(0);

  const token = localStorage.getItem("token");

  // const token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJqdGkiOiI2OWU2ZTJmMy03ZGI2LTQ5MDItYTEyNi0xYTdkYmFhZjIyYWYiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjMxIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZW1haWxhZGRyZXNzIjoiYWxpZWxhbWlyODk4QGdtYWlsLmNvbSIsImZpcnN0TmFtZSI6InNhcmEiLCJsYXN0TmFtZSI6ImVsYW1pciIsImV4cCI6MTc0NjQ0MTExMSwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDEzIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDozMDAwIn0.LJwMsceaHU4J-Jlkep15OB8RwJCw1U3ThNwzn0VrUiA";

  const handleSubmit = async () => {
    // التحقق من أن الحقول ليست فارغة
    if (!navigationEase || budgetHelpfulnessRating === 0 || !confusingFeatures || !desiredFeatures || !recommendationReason || !improvementSuggestion || overallSatisfaction === 0) {
      alert("Please fill in all fields!");
      return; // إيقاف التنفيذ إذا كانت هناك حقول فارغة
    }

    const body = {
      navigationEase,
      budgetHelpfulnessRating: Number(budgetHelpfulnessRating),
      confusingFeatures,
      desiredFeatures,
      recommendationReason,
      improvementSuggestion,
      overallSatisfaction: Number(overallSatisfaction),
    };

    try {
      const response = await fetch("https://graduproj.runasp.net/api/Feedback", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(body),
      });

      if (response.ok) {
        alert("Feedback submitted successfully!");
      } else {
        alert("Failed to submit feedback. Status: " + response.status);
      }
    } catch (error) {
      console.error("Error:", error);
      alert("Something went wrong!");
    }
  };

  return (
    <div className="feedback-container">
      <h2 className="text-primary">Feedback Page</h2>
      <p className="lead">Monthly feedback on your budget plan</p>

      <div className="card p-4 my-3">
        <h5>How easy was it to navigate and use the website?</h5>
        <div>
          <input type="radio" id="easy" name="navigate" value="easy" onChange={() => setNavigationEase("easy")} />
          <label htmlFor="easy" className="ms-2">Easy</label>
        </div>
        <div>
          <input type="radio" id="difficult" name="navigate" value="difficult" onChange={() => setNavigationEase("difficult")} />
          <label htmlFor="difficult" className="ms-2">Difficult</label>
        </div>
      </div>

      <div className="card p-4 my-3">
        <h5>How helpful is the website in managing your budget and achieving your saving goals? (1 to 5)</h5>
        {[1, 2, 3, 4, 5].map((num) => (
          <label key={num} className="me-3">
            <input type="radio" name="helpfulness" value={num} onChange={() => setBudgetHelpfulnessRating(num)} /> {num}
          </label>
        ))}
      </div>

      <div className="card p-4 my-3">
        <h5>Were there any features you found confusing or difficult to use?</h5>
        <textarea className="form-control" rows="3" onChange={(e) => setConfusingFeatures(e.target.value)} />
      </div>

      <div className="card p-4 my-3">
        <h5>Is there any feature you wish the website had to better support your saving goals?</h5>
        <textarea className="form-control" rows="3" onChange={(e) => setDesiredFeatures(e.target.value)} />
      </div>

      <div className="card p-4 my-3">
        <h5>Would you recommend this website to others? Why or why not?</h5>
        <textarea className="form-control" rows="3" onChange={(e) => setRecommendationReason(e.target.value)} />
      </div>

      <div className="card p-4 my-3">
        <h5>What's one thing you wish this website did better?</h5>
        <textarea className="form-control" rows="3" onChange={(e) => setImprovementSuggestion(e.target.value)} />
      </div>

      <div className="card p-4 my-3">
        <h5>How satisfied are you with the website overall? (1 to 5)</h5>
        {[1, 2, 3, 4, 5].map((num) => (
          <label key={num} className="me-3">
            <input type="radio" name="satisfaction" value={num} onChange={() => setOverallSatisfaction(num)} /> {num}
          </label>
        ))}
      </div>

      <button className="btn btn-primary" onClick={handleSubmit}>Confirm</button>
    </div>
  );
};

export default Feedback;
