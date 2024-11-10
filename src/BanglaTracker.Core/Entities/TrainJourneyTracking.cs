﻿using BanglaTracker.Core.Enums;

namespace BanglaTracker.Core.Entities
{
    // Sensor-Journey relational model 
    public class TrainJourneyTracking
    {
        public int Id { get; set; }
        public int TrainJourneyId { get; set; }
        public string TrainNumber { get; set; }
        public Guid SensorNumber { get; set; } // Nullable if not always required
        public JourneyStatus Status { get; set; } = JourneyStatus.NotStarted;
        public DateTime? ModifyDateTime { get; set; }
    }
}
