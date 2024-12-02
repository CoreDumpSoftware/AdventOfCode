using AdventOfCode.Services;

namespace AdventOfCode.Puzzles.Y2024.D1;

public class PuzzleSolution(string input) : IPuzzleSolution
{
	private readonly string _input = input;

	public async Task<long> PartOne()
	{
		long sum = 0;
		var left = new List<int>();
		var right = new List<int>();

		foreach (var line in _input.Split('\n'))
		{
			if (string.IsNullOrEmpty(line))
				continue;

			var parts = line.Split("   ").Select(int.Parse).ToArray();
			left.Add(parts[0]);
			right.Add(parts[1]);
		}

		left.Sort();
		right.Sort();

		foreach (var (l, r) in left.Zip(right))
		{
			sum += Math.Abs(l - r);
		}

		return sum;
	}

	public async Task<long> PartTwo()
	{
		long sum = 0;
		var left = new List<int>();
		var right = new Dictionary<int, int>();

		foreach (var line in _input.Split('\n'))
		{
			if (string.IsNullOrEmpty(line))
				continue;

			var parts = line.Split("   ").Select(int.Parse).ToArray();
			left.Add(parts[0]);
			if (right.TryGetValue(parts[1], out var val))
			{
				right[parts[1]]++;
			}
			else
			{
				right.Add(parts[1], 1);
			}
		}

		foreach (var item in left)
		{
			var count = right.TryGetValue(item, out var x)
				? x
				: 0;

			sum += (item * count);
		}

		return sum;
	}
}
