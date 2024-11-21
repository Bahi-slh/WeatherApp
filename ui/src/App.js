import React, { useState } from 'react';
import { getWeatherData } from './apiService';
import './App.css';

function App() {
  const [city, setCity] = useState('');
  const [country, setCountry] = useState('');
  const [weather, setWeather] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setWeather(null);

    try {
      const response = await getWeatherData(city, country);
      setWeather(response.description); // Access the description property
      console.log("Weather Data:", response);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="container">
      <div className="weather-card">
        <h1>Weather Description</h1>

        <form onSubmit={handleSubmit}>
          <div className="input-container">
            <input
              type="text"
              value={city}
              onChange={(e) => setCity(e.target.value)}
              placeholder="Enter city"
              required
            />
            <input
              type="text"
              value={country}
              onChange={(e) => setCountry(e.target.value)}
              placeholder="Enter country"
              required
            />
            <button type="submit" disabled={loading}>
              {loading ? 'Loading...' : 'Get Weather'}
            </button>
          </div>
        </form>

        {weather && (
          <div className="weather-result">
            <h2>Current Weather:</h2>
            <p className="description">{weather}</p>
          </div>
        )}

        {error && (
          <div className="error-message">
            {error}
          </div>
        )}
      </div>
    </div>
  );
}

export default App;