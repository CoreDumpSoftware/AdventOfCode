using System;
using System.Numerics;
using AdventOfCode.DataStructures;
using AdventOfCode.Services;

namespace AdventOfCode.Puzzles.Y2024.D7;

public class PuzzleSolution : IPuzzleSolution
{
	private readonly string _input;
	private readonly List<(long Expected, List<long> Values)> _values;

	public PuzzleSolution(string input)
	{
		_input = input;

		_values = input.Split('\n', StringSplitOptions.RemoveEmptyEntries)
			.Select(l =>
			{
				var parts = l.Split(':');
				return (long.Parse(parts[0]), parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList());
			})
			.ToList();
	}

	public async Task<object> PartOne()
	{
		var result = 0L;

		foreach (var value in _values)
		{
			if (HasPartOneSolution(value.Expected, value.Values))
				result += value.Expected;
		}

		return await Task.FromResult(result);
	}

	public async Task<object> PartTwo()
	{
		var result = 0L;
		var semaphore = new SemaphoreSlim(1);

		await Parallel.ForEachAsync(_values, async (x, _) =>
		{
			if (HasPartTwoSolution(x.Expected, x.Values))
			{
				await semaphore.WaitAsync();
				result += x.Expected;
				semaphore.Release();
			}
		});

		return await Task.FromResult(result);
	}

	private bool HasPartOneSolution(long expected, List<long> values)
	{
		var numberOfOperators = values.Count - 1;
		var stop = (long)Math.Pow(2, numberOfOperators);
		for (var i = 0; i < stop; i++)
		{
			var result = values[0];

			for (var j = 0; j < numberOfOperators; j++)
			{
				result = (i & (1 << j)) == 0
					? result + values[j + 1]
					: result * values[j + 1];
			}

			if (result == expected)
				return true;
		}

		return false;
	}

	private bool HasPartTwoSolution(long expected, List<long> values)
	{
		var numberOfOperators = values.Count - 1;
		var stop = (long)Math.Pow(3, numberOfOperators);
		var permutationsState = new byte[numberOfOperators];

		for (var i = 0; i < stop; i++)
		{
			// Do the math
			var result = values[0];
			for (var j = 0; j < permutationsState.Length; j++)
			{
				var op = permutationsState[j];

				if (op == 0)
					result += values[j + 1];
				else if (op == 1)
					result *= values[j + 1];
				else
					result = long.Parse(result.ToString() + values[j + 1].ToString());
			}

			if (result == expected)
				return true;

			Increment(permutationsState);
		}

		return false;
	}

	private void Increment(byte[] operators)
	{
		operators[^1]++;
		for (var i = operators.Length - 1; i >= 0 && operators[i] > 2; i--)
		{
			operators[i] = 0;
			if (i > 0)
				operators[i - 1]++;
		}
	}

	private long ConcatNumbers(long left, long right)
	{
		return long.Parse(left.ToString() + right.ToString());
	}

}
