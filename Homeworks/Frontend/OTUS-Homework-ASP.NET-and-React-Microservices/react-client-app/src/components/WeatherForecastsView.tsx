import { useState } from "react"
import { Button } from 'react-bootstrap';
import apiClient from "../api/Client";
import WeatherForecast from "../entities/WeatherForecast";
import { WeatherForecastTable } from "./WeatherForecastTable";

export const WeatherForecastsView: React.FC = () => {
  const [forecasts, setForecasts] = useState<WeatherForecast[]>([]);

  const getForecastsAsync = async () => {
    try {
      const response = await apiClient.get<WeatherForecast[]>('WeatherForecast');
      if (response.status >= 200 && response.status < 300) {
        setForecasts(response.data);
      } 
      else {
        throw new Error("Возникла ошибка");
      }       
    } catch (error) {
      console.error(error);
    }
  };

  return (
    <div>      
      <WeatherForecastTable items={forecasts} />
      <Button onClick={getForecastsAsync}>Show Weather</Button>
    </div>
  );
};