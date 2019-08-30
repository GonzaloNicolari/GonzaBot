using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ML.Data;
namespace MyEchoBot
{
    public class UserProfile
    {
        [LoadColumn(0)]
        public float ExpYears { get; set; }
        [LoadColumn(1)]
        public float Category { get; set; }
        [LoadColumn(2)]
        public float CategoryExpYears { get; set; }
        [LoadColumn(3)]
        public float LastEvalLetter { get; set; }

        [LoadColumn(4)]
        public float PredictionValue { get; set; }
    }
    public class UserProfilePrediction
    {
        [ColumnName("Label")]
        public float Prediction;
    }
}
