import React from "react";
import { Col, Row, Table } from "react-bootstrap";
import WeatherForecast from "../entities/WeatherForecast"
import 'bootstrap/dist/css/bootstrap.css';

interface WeatherForecastTableProps {
  items: WeatherForecast[];
}

export const WeatherForecastTable: React.FC<WeatherForecastTableProps> = (props) => {
  return (
    <Table>
      <Row>
        <Col>Date</Col>
        <Col>TemperatureC</Col>
        <Col>TemperatureF</Col>
        <Col>Summary</Col>
      </Row>
      {props.items.map(forecast => (
        <Row>
          <Col>{forecast.date.toString()}</Col>
          <Col>{forecast.temperatureC}</Col>
          <Col>{forecast.temperatureF}</Col>
          <Col>{forecast.summary}</Col>
        </Row>
      ))}
    </Table>
  );
};