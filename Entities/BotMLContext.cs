using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using System.IO;
namespace MyEchoBot.Entities
{
    
    public class BotMLContext
    {
        public MLContext mlContext { get; set; }
        public ITransformer model { get; set; }
        public UserProfilePrediction prediction { get; set; }
        static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "TrainDataML.csv");
        static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "TrainDataML.csv");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");

        public BotMLContext()
        {
            this.mlContext = new MLContext(seed: 0);
            this.model = Train(mlContext, _trainDataPath);
            Evaluate(mlContext, model);
        }

        public static ITransformer Train(MLContext mlContext, string dataPath)
        {
            IDataView dataView = mlContext.Data.LoadFromTextFile<UserProfile>(dataPath, hasHeader: true, separatorChar: ',');
            var pipeline = mlContext.Transforms
                .CopyColumns(outputColumnName: "Label", inputColumnName: "ExpYears")
                .Append(mlContext.Transforms.Concatenate("Features", "ExpYears"))
                .Append(mlContext.Regression.Trainers.FastTree());
            var model = pipeline.Fit(dataView);
            
            return model;
        }
        private static void Evaluate(MLContext mlContext, ITransformer model)
        {
            IDataView dataView = mlContext.Data.LoadFromTextFile<UserProfile>(_testDataPath, hasHeader: true, separatorChar: ',');
            var predictions = model.Transform(dataView);
            var metrics = mlContext.Regression.Evaluate(predictions, "Label", "ExpYears");
            Console.WriteLine();
            Console.WriteLine($"*************************************************");
            Console.WriteLine($"*       Model quality metrics evaluation         ");
            Console.WriteLine($"*------------------------------------------------");
            Console.WriteLine($"*       RSquared Score:      {metrics.RSquared:0.##}");
            Console.WriteLine($"*       Root Mean Squared Error:      {metrics.RootMeanSquaredError:#.##}");
            Console.WriteLine($"*       LossFunction Score:      {metrics.LossFunction:0.##}");
            Console.WriteLine($"*       MeanSquaredError Score:      {metrics.MeanSquaredError:0.##}");
            Console.WriteLine($"*       MeanAbsoluteError Score:      {metrics.MeanAbsoluteError:0.##}");
        }
        
        public void TestSinglePrediction(MLContext mlContext, ITransformer model, UserProfile userSample)
        {
            var predictionFunction = mlContext
                 .Model.CreatePredictionEngine<UserProfile, UserProfilePrediction>(model);
            this.prediction = predictionFunction.Predict(userSample);

            //Console.WriteLine($"**********************************************************************");
            //Console.WriteLine($"Predicted number: {prediction.Prediction:0.####}");
            //Console.WriteLine($"**********************************************************************");
        }
    }
}
