const API_BASE_URL = 'http://localhost:5169';

export const getWeatherData = async (city, country) => {
    try {
        const response = await fetch(`${API_BASE_URL}/Weather?city=${encodeURIComponent(city)}&country=${encodeURIComponent(country)}`);
        
        if (!response.ok) {
            const errorMessage = await response.text();
            throw new Error(errorMessage);
        }
        
        return await response.json();
    } catch (error) {
        throw error;
    }
};