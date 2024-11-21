# Weather App

A weather service application built with .NET Core backend and React frontend that retrieves weather descriptions using the OpenWeatherMap API.

## Prerequisites

- .NET 6.0 SDK
- Node.js and npm
- Git

## Installation

1. Clone the repository:
```bash
git clone https://github.com/Bahi-slh/WeatherApp.git
cd WeatherApp
```

2. Backend Setup:
```bash
cd WeatherAPI
dotnet restore
dotnet build
```

3. Frontend Setup:
```bash
cd ../ui
npm install
```

## Running the Application

1. Start the Backend:
```bash
cd WeatherAPI
dotnet run
```
Backend will run on http://localhost:5169

2. Start the Frontend (in a new terminal):
```bash
cd ui
npm start
```
Frontend will run on http://localhost:3000

## Features
### Backend

- Rate limiting system with 5 API keys (5 requests per hour per key)
- REST endpoint accepting city and country parameters
- OpenWeatherMap API integration with key rotation
- Returns weather description field only
- Error handling for rate limits and invalid requests

### Frontend

- Simple form for city and country input
- Displays weather description results
- Shows appropriate error messages
- Loading state handling

## Testing
Test the following scenarios:

- Valid city/country combinations (for example: Melbourne, AU)
- Rate limiting by making more than 5 requests in an hour
- Invalid city/country combinations
- Error message display
