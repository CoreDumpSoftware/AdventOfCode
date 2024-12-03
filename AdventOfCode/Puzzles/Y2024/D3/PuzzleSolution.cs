using System.Text.RegularExpressions;
using AdventOfCode.Services;

namespace AdventOfCode.Puzzles.Y2024.D3;

public class PuzzleSolution(string input) : IPuzzleSolution
{
	private readonly string _input = input;
	private static Regex mulRegex = new Regex(@"mul\((?'left'\d+),(?'right'\d+)\)");

	public async Task<object> PartOne()
	{
		return await Task.FromResult(Calculate(_input));
	}

	public async Task<object> PartTwo()
	{
		var i = 0;
		var enabled = true;
		var answer = 0l;

		// Find enabled sections, perform calculations when enabled
		var enableRegex = new Regex(@"do\(\)|don't\(\)");
		foreach (Match enableMatch in enableRegex.Matches(_input))
		{
			if (enabled)
			{
				var preceedingText = _input.Substring(i, enableMatch.Index - i);
				answer += Calculate(preceedingText);
			}

			if (enableMatch.Value == "don't()")
				enabled = false;
			else
				enabled = true;

			i = enableMatch.Index + enableMatch.Length;
		}

		return await Task.FromResult(answer);
	}

	private long Calculate(string input)
	{
		var matches = mulRegex.Matches(input);
		var answer = 0l;

		foreach (Match match in matches)
		{
			var left = int.Parse(match.Groups["left"].ToString());
			var right = int.Parse(match.Groups["right"].ToString());

			answer += left * right;
		}

		return answer;
	}
}
