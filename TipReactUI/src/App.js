import React, { useEffect, useState } from "react";
import "./App.css";

function App() {
  const [form, setForm] = useState({
    totalBill: "",
    size: "",
    smoker: "No",
    day: "Fri",
    time: "Dinner",
  });

  const [result, setResult] = useState(null);
  const [logs, setLogs] = useState([]);
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  const API = "https://localhost:7100/api";

  //load history
  async function loadLogs() {
    try {
      const res = await fetch(`${API}/Prediction/logs`);
      if (!res.ok) {
        throw new Error("Failed to load logs.");
      }
      const data = await res.json();
      setLogs(data);
    } catch (err) {
      setError(err.message);
    }
  }

  useEffect(() => {
    loadLogs();
  }, []);

  function handleChange(e) {
    const { name, value } = e.target;
    setForm((prev) => {
      let newValue = value;

      if (name === "totalBill" || name === "size") {
        if (value === "") {
          newValue = "";
        } else {
          const parsed = parseFloat(value);
          newValue = isNaN(parsed) ? "" : parsed;
        }
      }

      return {
        ...prev,
        [name]: newValue,
      };
    });
  }

  async function handleSubmit(e) {
    e.preventDefault();
    setError("");
    setResult(null);
    setLoading(true);

    try {
      const res = await fetch(`${API}/Prediction`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(form),
      });

      if (!res.ok) {
        const text = await res.text();
        throw new Error(text || "Prediction failed.");
      }

      const data = await res.json();
      setResult(data);
      await loadLogs();

      setForm({
        totalBill: "",
        size: "",
        smoker: "No",
        day: "Fri",
        time: "Dinner",
      });
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }

    console.log("Payload being sent:", form);
  }

  return (
    <div className="app">
      <div className="container">
        <header className="header">
          <h1>Tip Prediction App</h1>
          <p>Predict restaurant tips and track prediction history.</p>
        </header>

        {error && <div className="error-box">{error}</div>}

        <div className="card">
          <h2>Predict Tip</h2>

          <form className="prediction-form" onSubmit={handleSubmit}>
            <div className="form-grid">
              <div className="form-group">
                <label>Total Bill</label>
                <input
                  type="number"
                  name="totalBill"
                  value={form.totalBill}
                  onChange={handleChange}
                  placeholder="Enter total bill"
                  step="0.01"
                  min="0"
                  required
                />
              </div>

              <div className="form-group">
                <label>Size</label>
                <input
                  type="number"
                  name="size"
                  value={form.size}
                  onChange={handleChange}
                  placeholder="Enter party size"
                  min="1"
                  required
                />
              </div>

              <div className="form-group">
                <label>Smoker</label>
                <select
                  name="smoker"
                  value={form.smoker}
                  onChange={handleChange}
                >
                  <option value="No">No</option>
                  <option value="Yes">Yes</option>
                </select>
              </div>

              <div className="form-group">
                <label>Day</label>
                <select name="day" value={form.day} onChange={handleChange}>
                  <option value="Thur">Thur</option>
                  <option value="Fri">Fri</option>
                  <option value="Sat">Sat</option>
                  <option value="Sun">Sun</option>
                </select>
              </div>

              <div className="form-group">
                <label>Time</label>
                <select name="time" value={form.time} onChange={handleChange}>
                  <option value="Lunch">Lunch</option>
                  <option value="Dinner">Dinner</option>
                </select>
              </div>
            </div>

            <button className="predict-button" type="submit" disabled={loading}>
              {loading ? "Predicting..." : "Predict"}
            </button>
          </form>
        </div>

        {result && (
          <div className="card">
            <h2>Prediction Result</h2>
            <div className="result-grid">
              <div className="result-box ">
                <span className="result-label">Predicted Tip %</span>
                <span className="result-value">
                  {result.predictedTipPct.toFixed(2)}%
                </span>
              </div>

              <div className="result-box">
                <span className="result-label">Recommended Tip Amount</span>
                <span className="result-value">
                  ${result.adjustedPredictedTipAmount.toFixed(2)}
                </span>
              </div>
            </div>

            <div className="result-box raw-box">
              <span className="result-label">Raw ML Tip Amount</span>
              <span className="result-value">
                ${result.rawPredictedTipAmount.toFixed(2)}
              </span>
            </div>

            <p className="result-message">{result.message}</p>
          </div>
        )}

        <div className="card">
          <h2>Prediction History</h2>

          <div className="table-wrapper">
            <table>
              <thead>
                <tr>
                  <th>Total</th>
                  <th>Size</th>
                  <th>Smoker</th>
                  <th>Day</th>
                  <th>Time</th>
                  <th>Tip %</th>
                  <th>Amount</th>
                  <th>Created</th>
                </tr>
              </thead>
              <tbody>
                {logs.length > 0 ? (
                  logs.map((log) => (
                    <tr key={log.id}>
                      <td>${Number(log.totalBill).toFixed(2)}</td>
                      <td>{log.size}</td>
                      <td>{log.smoker}</td>
                      <td>{log.day}</td>
                      <td>{log.time}</td>
                      <td>{log.predictedTipPct.toFixed(2)}%</td>
                      <td>${Number(log.predictedTipAmount).toFixed(2)}</td>
                      <td>{new Date(log.createdAt).toDateString()}</td>
                    </tr>
                  ))
                ) : (
                  <tr>
                    <td colSpan="8" className="empty-row">
                      No prediction history yet.
                    </td>
                  </tr>
                )}
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  );
}

export default App;
