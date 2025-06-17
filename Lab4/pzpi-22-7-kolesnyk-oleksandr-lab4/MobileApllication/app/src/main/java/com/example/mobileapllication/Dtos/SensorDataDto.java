package com.example.mobileapllication.Dtos;

import com.fasterxml.jackson.annotation.JsonFormat;
import com.fasterxml.jackson.annotation.JsonIgnoreProperties;

@JsonIgnoreProperties(ignoreUnknown = true) // Ігноруємо невідомі поля (наприклад, device)
public class SensorDataDto {
    public int dataId;
    public int deviceId;

    @JsonFormat(pattern = "yyyy-MM-dd'T'HH:mm:ss.SSSSSSS") // Формат для timestamp
    public String timestamp;

    public int heartRate;
    public double bloodOxygenLevel;
    public double bodyTemperature;
    public String activityLevel;
    public String sleepPhase;
}