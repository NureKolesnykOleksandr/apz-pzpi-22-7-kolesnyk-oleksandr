﻿using ClientMM.Models;
using System.ComponentModel.DataAnnotations;

namespace ClientMM.Dtos
{
    public record AddSensorDataDto
    {
        public int? HeartRate { get; set; }
        public double? BloodOxygenLevel { get; set; }
        public double? BodyTemperature { get; set; }

        [MaxLength(10)]
        public string ActivityLevel { get; set; }

        [MaxLength(10)]
        public string SleepPhase { get; set; }
    }
}
