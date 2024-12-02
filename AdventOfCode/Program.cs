﻿using System.Reflection;
using AdventOfCode.Configuration;
using AdventOfCode.Services;
using AdventOfCode.Services.Abstract;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdventOfCode;

public static class Program
{
	public static async Task Main(string[] args)
	{
		var builder = Host.CreateApplicationBuilder(args);

		SetupAppConfiguration(builder.Configuration);
		SetupServiceConfiguration(builder.Services, builder.Configuration);

		var app = builder.Build();

		var factory = app.Services.GetRequiredService<IPuzzleSolutionFactory>();
		var config = app.Services.GetRequiredService<IConfiguration>();

		if (!int.TryParse(config["Year"], out var year))
			throw new ArgumentException("Year must be provided via runtime configuration.");

		if (!int.TryParse(config["Day"], out var day))
			throw new ArgumentException("Day must be provided via runtime configuration.");

		var solution = await factory.GetPuzzleSolution(year, day);

		Console.WriteLine($"Year: {year}, Day: {day}");
		Console.WriteLine($"\tPart One: {await solution.PartOne()}");
		Console.WriteLine($"\tPart Two: {await solution.PartTwo()}");
	}

	private static void SetupAppConfiguration(IConfigurationBuilder configBuilder)
	{
		configBuilder.AddUserSecrets(Assembly.GetExecutingAssembly(), false);
	}

	private static void SetupServiceConfiguration(IServiceCollection services, IConfiguration config)
	{
		services.AddHttpClient();
		services.AddScoped<IInputProvider, InputProvider>();
		services.AddScoped<IPuzzleSolutionFactory, PuzzleSolutionFactory>();
		services.AddOptions<PuzzleFetchingConfiguration>().Bind(config);
	}
}