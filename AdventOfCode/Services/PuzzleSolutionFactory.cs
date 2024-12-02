using System.Reflection;
using AdventOfCode.Services.Abstract;

namespace AdventOfCode.Services;

public class PuzzleSolutionFactory(IInputProvider inputProvider) : IPuzzleSolutionFactory
{
	private readonly IInputProvider _inputProvider = inputProvider;

	public async Task<IPuzzleSolution> GetPuzzleSolution(int year, int day)
	{
		var typeNamespace = $"AdventOfCode.Puzzles.Y{year}.D{day}";
		var fullName = $"{typeNamespace}.PuzzleSolution";

		var assembly = Assembly.GetExecutingAssembly();
		var type = assembly.GetTypes().SingleOrDefault(t => t.FullName == fullName);

		if (type == null)
			throw new PuzzleSolutionFactoryException($"No puzzle solution for year {year}, day {day} was found.");

		var input = await _inputProvider.GetInput(year, day);

		return (IPuzzleSolution)Activator.CreateInstance(type, input)!;
	}
}
