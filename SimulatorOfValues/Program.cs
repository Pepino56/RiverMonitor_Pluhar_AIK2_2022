﻿using Microsoft.Extensions.Configuration;
using SimulatorOfValues;
using SimulatorOfValues.Contracts;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        IConfiguration config = builder.Build();

        string connectionString = config["Sql:ConnectionString"];

        string apiBaseUri = config["Api:BaseUri"];
        string token = config["Api:AuthToken"];

        string logFolder = config["App:LogFolder"];
        string errorFolder = config["App:ErrorFolder"];

        while (true)
        {
            await SendValues(config);
            await Task.Delay(TimeSpan.FromSeconds(5)); // Wait 5 seconds before the next call
        }
    }
    public static async Task SendValues(IConfiguration config)
    {
        string apiBaseUri = config["Api:BaseUri"];
        string token = config["Api:AuthToken"];

        var message = GenerateFloodReport();

        using (var apiclient = new ApiClient())
        {
            await apiclient.SendMessage(message, apiBaseUri, token);
        }
    }
    public static FloodReport GenerateFloodReport()
    {
        Random random = new Random();
        int value = random.Next(1, 100);

        int[] originalStationIds = { 1, 2, 3, 5, 6 }; //  Array of stations

        int stationId = originalStationIds[random.Next(originalStationIds.Length)];

        DateTime now = DateTime.Now;
        DateTime oneYearAgo = now.AddYears(-1);

        DateTime dateTime = new DateTime(now.Year, random.Next(1, 13), random.Next(1, 29), random.Next(0, 24), random.Next(0, 60), random.Next(0, 60));

        if (dateTime > now)
        {
            dateTime = now;
        }
        else if (dateTime < oneYearAgo)
        {
            dateTime = oneYearAgo;
        }

        FloodReport floodReport = new FloodReport
        {
            StationId = stationId,
            TimeStamp = dateTime,
            Val = value
        };
        return floodReport;
    }
}