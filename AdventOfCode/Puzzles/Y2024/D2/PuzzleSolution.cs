using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.Services;

namespace AdventOfCode.Puzzles.Y2024.D2;

public class PuzzleSolution(string input) : IPuzzleSolution
{
	private readonly string _input = input;

	public async Task<long> PartOne()
	{
		var validCount = 0;

		foreach (var line in _input.Split('\n').Where(x => !string.IsNullOrEmpty(x)))
		{
			var row = line.Split(' ').Select(int.Parse).ToList();

			if (ValidateSequence(row))
				validCount++;
		}

		return validCount;
	}

	private bool ValidateSequence(List<int> values)
	{
		var isAscending = false;
		for (var i = 1; i < values.Count(); i++)
		{
			var prev = values[i - 1];
			var cur = values[i];
			if (i == 1)
				isAscending = cur > prev;

			var diff = cur - prev;
			if (diff == 0 || Math.Abs(diff) > 3)
				return false;

			if (isAscending && diff < 0)
				return false;

			if (!isAscending && diff > 0)
				return false;
		}

		return true;
	}

	public async Task<long> PartTwo()
	{
		var validCount = 0;

		foreach (var line in _input.Split('\n').Where(x => !string.IsNullOrEmpty(x)))
		{
			var row = line.Split(' ').Select(int.Parse).ToList();
			if (ValidateSequence(row))
				validCount++;
			else
			{
				for (var i = 0; i < row.Count; i++)
				{
					var copy = new List<int>(row);
					copy.RemoveAt(i);
					if (ValidateSequence(copy))
					{
						validCount++;
						break;
					}
				}
			}
		}

		return validCount;
	}
}
