using AdventOfCode.Configuration;
using AdventOfCode.Services.Abstract;
using AdventOfCode.Services.Exceptions;
using Flurl;
using Microsoft.Extensions.Options;

namespace AdventOfCode.Services;

public class InputProvider : IInputProvider
{
	private readonly IHttpClientFactory _httpClientFactory;
	private readonly PuzzleFetchingConfiguration _config;

	public InputProvider(IHttpClientFactory httpClientFactory, IOptions<PuzzleFetchingConfiguration> config)
	{
		_httpClientFactory = httpClientFactory;
		_config = config.Value;
	}

	public async Task<string> GetInput(int year, int day)
	{
		var dir = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}/AdventOfCode/inputs/{year}";
		if (!Directory.Exists(dir))
			Directory.CreateDirectory(dir);

		var filePath = $"{dir}/day-{day}.txt";
		if (File.Exists(filePath))
			return await File.ReadAllTextAsync(filePath);

		var input = await FetchInput(year, day);
		await File.WriteAllTextAsync(filePath, input);

		return input;
	}

	private async Task<string> FetchInput(int year, int day)
	{
		var url = Url.Combine(_config.BaseUrl, year.ToString(), "day", day.ToString(), "input");
		var request = new HttpRequestMessage(HttpMethod.Get, url);
		request.Headers.Add("Cookie", _config.Cookie);

		var response = await _httpClientFactory.CreateClient().SendAsync(request);
		if (!response.IsSuccessStatusCode)
		{
			var statusCode = response.StatusCode;
			var errorMessage = await response.Content.ReadAsStringAsync();

			throw new InputProviderException(
				$"Failed to fetch puzzle input for year {year}, day {day}. " +
				$"Reason: Status code - {(int)statusCode}, Error Message - {errorMessage}"
			);
		}

		return await response.Content.ReadAsStringAsync();
	}
}
