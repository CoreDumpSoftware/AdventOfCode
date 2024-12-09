using AdventOfCode.DataStructures;
using AdventOfCode.Services;

namespace AdventOfCode.Puzzles.Y2024.D8;
public class PuzzleSolution : IPuzzleSolution
{
	private readonly string _input;
	private readonly Matrix<char> _matrix;
	private readonly Dictionary<char, List<Coordinate>> frequencyCoordinates = [];

	public PuzzleSolution(string input)
	{
		_input = input;
		_matrix = Matrix<char>.ParseTextToMatrix(_input, (coordinate, c) =>
		{
			if (c == '.')
				return;
			else if (frequencyCoordinates.TryGetValue(c, out var list))
				list.Add(coordinate);
			else
				frequencyCoordinates.Add(c, [coordinate]);

		});
	}

	public async Task<object> PartOne()
	{
		var antinodes = new HashSet<Coordinate>();

		foreach (var (frequency, coordinates) in frequencyCoordinates)
		{
			// for each coordinate, check the other recorded coordinates to find antinodes
			for (var i = 0; i < coordinates.Count; i++)
			{
				for (var j = i + 1; j < coordinates.Count; j++)
				{
					var (leftAntinode, rightAntinode) = CalculateAntinodes(coordinates[i], coordinates[j]);

					if (_matrix.IsValid(leftAntinode))
						antinodes.Add(leftAntinode);

					if (_matrix.IsValid(rightAntinode))
						antinodes.Add(rightAntinode);
				}
			}
		}

		return await Task.FromResult(antinodes.Count);
	}

	public async Task<object> PartTwo()
	{
		return await Task.FromResult(0);
	}

	private (Coordinate, Coordinate) CalculateAntinodes(Coordinate a, Coordinate b)
	{
		var leftDelta = a - b;
		var rightDelta = b - a;

		var leftAntinode = a + rightDelta + rightDelta;
		var rightAntinode = b + leftDelta + leftDelta;

		return (leftAntinode, rightAntinode);
	}

	//private void PrintNodesAndAntinodes(Coordinate coordinate, char c, )
	//{
	//	var defaultForeground = Console.ForegroundColor;

	//	if (c == frequency)
	//	{
	//		if (coordinate == coordinates[i])
	//			Console.ForegroundColor = ConsoleColor.Green;
	//		else if (coordinate == coordinates[j])
	//			Console.ForegroundColor = ConsoleColor.Red;
	//	}
	//	else if (c == '#')
	//	{
	//		if (coordinate == leftAntinode)
	//			Console.ForegroundColor = ConsoleColor.Green;
	//		if (coordinate == rightAntinode)
	//			Console.ForegroundColor = ConsoleColor.Red;
	//	}

	//	Console.Write(c);

	//	Console.ForegroundColor = defaultForeground;
	//}
}
