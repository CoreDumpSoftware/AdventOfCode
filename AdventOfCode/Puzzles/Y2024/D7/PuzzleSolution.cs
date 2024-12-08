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
			if (HasSolution(value.Expected, value.Values))
				result += value.Expected;
		}

		//using var semaphore = new SemaphoreSlim(1);
		//await Parallel.ForEachAsync(_values, async (x, _) =>
		//{
		//	if (HasSolution(x.Expected, x.values))
		//	{
		//		await semaphore.WaitAsync();
		//		result += x.Expected;
		//		semaphore.Release();
		//	}
		//});

		return await Task.FromResult(result);
	}

	public async Task<object> PartTwo()
	{
		return await Task.FromResult(0);
	}

	private bool HasSolution(long expected, List<long> values)
	{
		var numberOfOperators = values.Count - 1;
		var stop = Math.Pow(2, numberOfOperators);
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
}
