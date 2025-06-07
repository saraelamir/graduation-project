import React, { useState, useEffect } from "react";
import axios from "axios";
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

  useEffect(() => {
    const fetchLatestFeedback = async () => {
      try {
        const response = await axios.get("https://graduproj.runasp.net/api/Feedback/latest-feedback", {
          headers: {
            Accept: "*/*",
            Authorization: `Bearer ${token}`,
          },
        });

        const data = response.data;
        setNavigationEase(data.navigationEase);
        setBudgetHelpfulnessRating(data.budgetHelpfulnessRating);
        setConfusingFeatures(data.confusingFeatures);
        setDesiredFeatures(data.desiredFeatures);
        setRecommendationReason(data.recommendationReason);
        setImprovementSuggestion(data.improvementSuggestion);
        setOverallSatisfaction(data.overallSatisfaction);
      } catch (error) {
        console.error("Error fetching latest feedback:", error);
      }
    };

    if (token) {
      fetchLatestFeedback();
    }
  }, [token]);

  const handleSubmit = async () => {
    if (
      !navigationEase ||
      budgetHelpfulnessRating === 0 ||
      !confusingFeatures ||
      !desiredFeatures ||
      !recommendationReason ||
      !improvementSuggestion ||
      overallSatisfaction === 0
    ) {
      alert("Please fill in all fields!");
      return;
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
      await axios.post("https://graduproj.runasp.net/api/Feedback", body, {
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
      });
      alert("Feedback submitted successfully!");
    } catch (error) {
      console.error("Error submitting feedback:", error);
      alert("Failed to submit feedback.");
    }
  };

  return (
    <div className="feedback-container">
      <h2 className="text-primary">Feedback Page</h2>
      <p className="lead">Monthly feedback on your budget plan</p>

      <div className="card p-4 my-3">
        <h5>How easy was it to navigate and use the website?</h5>
        <div>
          <input
            type="radio"
            id="easy"
            name="navigate"
            value="easy"
            checked={navigationEase === "easy"}
            onChange={() => setNavigationEase("easy")}
          />
          <label htmlFor="easy" className="ms-2">
            Easy
          </label>
        </div>
        <div>
          <input
            type="radio"
            id="difficult"
            name="navigate"
            value="difficult"
            checked={navigationEase === "difficult"}
            onChange={() => setNavigationEase("difficult")}
          />
          <label htmlFor="difficult" className="ms-2">
            Difficult
          </label>
        </div>
      </div>

      <div className="card p-4 my-3">
        <h5>How helpful is the website in managing your budget and achieving your saving goals? (1 to 5)</h5>
        {[1, 2, 3, 4, 5].map((num) => (
          <label key={num} className="me-3">
            <input
              type="radio"
              name="helpfulness"
              value={num}
              checked={budgetHelpfulnessRating === num}
              onChange={() => setBudgetHelpfulnessRating(num)}
            />{" "}
            {num}
          </label>
        ))}
      </div>

      <div className="card p-4 my-3">
        <h5>Were there any features you found confusing or difficult to use?</h5>
        <textarea
          className="form-control"
          rows="3"
          value={confusingFeatures}
          onChange={(e) => setConfusingFeatures(e.target.value)}
        />
      </div>

      <div className="card p-4 my-3">
        <h5>Is there any feature you wish the website had to better support your saving goals?</h5>
        <textarea
          className="form-control"
          rows="3"
          value={desiredFeatures}
          onChange={(e) => setDesiredFeatures(e.target.value)}
        />
      </div>

      <div className="card p-4 my-3">
        <h5>Would you recommend this website to others? Why or why not?</h5>
        <textarea
          className="form-control"
          rows="3"
          value={recommendationReason}
          onChange={(e) => setRecommendationReason(e.target.value)}
        />
      </div>

      <div className="card p-4 my-3">
        <h5>What's one thing you wish this website did better?</h5>
        <textarea
          className="form-control"
          rows="3"
          value={improvementSuggestion}
          onChange={(e) => setImprovementSuggestion(e.target.value)}
        />
      </div>

      <div className="card p-4 my-3">
        <h5>How satisfied are you with the website overall? (1 to 5)</h5>
        {[1, 2, 3, 4, 5].map((num) => (
          <label key={num} className="me-3">
            <input
              type="radio"
              name="satisfaction"
              value={num}
              checked={overallSatisfaction === num}
              onChange={() => setOverallSatisfaction(num)}
            />{" "}
            {num}
          </label>
        ))}
      </div>

      <button className="btn btn-primary" onClick={handleSubmit}>
        Confirm
      </button>
    </div>
  );
};

export default Feedback;
