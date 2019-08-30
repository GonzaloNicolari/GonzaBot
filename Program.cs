// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio EchoBot v4.3.0

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Web.Http;
using System.Web.Http.SelfHost;
using System.IO;
using Microsoft.ML;

namespace MyEchoBot
{
    public class Program
    {
        static readonly string _trainDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "TrainDataML.csv");
        static readonly string _testDataPath = Path.Combine(Environment.CurrentDirectory, "Data", "TrainDataML.csv");
        static readonly string _modelPath = Path.Combine(Environment.CurrentDirectory, "Data", "Model.zip");
        public static void Main(string[] args)
        {
            MLContext mlContext = new MLContext(seed: 0);

            var model = Train(mlContext, _trainDataPath);
            Evaluate(mlContext, model);
            TestSinglePrediction(mlContext, model);
            CreateWebHostBuilder(args).Build().Run();        
            
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
        private static void TestSinglePrediction(MLContext mlContext, ITransformer model)
        {
            var predictionFunction = mlContext.Model.CreatePredictionEngine<UserProfile, UserProfilePrediction>(model);
            UserProfile userSample = new UserProfile()
            {

                Category = 3,
                CategoryExpYears = 3,
                LastEvalLetter = 3,

            };
            var prediction = predictionFunction.Predict(userSample);

            Console.WriteLine($"**********************************************************************");
            Console.WriteLine($"Predicted number: {prediction.Prediction:0.####}");
            Console.WriteLine($"**********************************************************************");
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureLogging((logging) =>
                {
                    logging.AddDebug();
                    logging.AddConsole();
                })
                .UseStartup<Startup>();
    }
}
