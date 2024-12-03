using System.Text.RegularExpressions;
using AdventOfCode.Services;

namespace AdventOfCode.Puzzles.Y2024.D3;

public class PuzzleSolution(string input) : IPuzzleSolution
{
	private readonly string _input = input;

	public async Task<object> PartOne()
	{
		var regex = new Regex(@"mul\((?'left'\d+),(?'right'\d+)\)");

		var matches = regex.Matches(_input);
		var answer = 0l;

		foreach (Match match in matches)
		{
			var left = int.Parse(match.Groups["left"].ToString());
			var right = int.Parse(match.Groups["right"].ToString());

			answer += left * right;
		}

		return await Task.FromResult(answer);
	}

	public async Task<object> PartTwo()
	{
		return await Task.FromResult(0);
	}
}
