using AdventOfCode.Services;

namespace AdventOfCode.Puzzles.Y2024.D5;

public class PuzzleSolution : IPuzzleSolution
{
	private readonly string _input;
	private readonly List<List<int>> _knownBad = [];
	private readonly List<List<int>> _updates;
	private readonly Dictionary<int, HashSet<int>> _pageOrderingRules;

	public PuzzleSolution(string input)
	{
		_input = input;
		(_pageOrderingRules, _updates) = ParseInput();
	}

	public async Task<object> PartOne()
	{
		var sum = 0L;
		foreach (var update in _updates)
		{
			var invalid = false;
			for (var i = 1; i < update.Count; i++)
			{
				var current = update[i];
				if (!_pageOrderingRules.TryGetValue(current, out var pages))
					continue;

				for (var j = 0; j < i; j++)
				{
					var prev = update[j];
					invalid = pages.Contains(prev);

					if (invalid)
						break;
				}

				if (invalid)
					break;
			}

			if (!invalid)
			{
				sum += update[update.Count / 2];
			}
			else
			{
				_knownBad.Add(update);
			}
		}

		return await Task.FromResult(sum);
	}

	public async Task<object> PartTwo()
	{
		var sum = 0L;
		foreach (var update in _knownBad)
		{
			var hadError = false;
			for (var i = 1; i < update.Count; i++)
			{
				var current = update[i];
				if (!_pageOrderingRules.TryGetValue(current, out var pages))
					continue;

				for (var j = 0; j < i; j++)
				{
					var invalid = pages.Contains(update[j]);
					if (invalid)
					{
						hadError = true;

						// swap the values
						update[i] = update[j];
						update[j] = current;

						// recheck this index for more errors
						i--;
						break;
					}
				}
			}

			if (hadError)
				sum += update[update.Count / 2];
		}

		return await Task.FromResult(sum);
	}

	private (Dictionary<int, HashSet<int>> PageOrderRules, List<List<int>> Updates) ParseInput()
	{
		var pageOrderingRules = new Dictionary<int, HashSet<int>>();
		var updates = new List<List<int>>();

		var firstSection = true;
		foreach (var line in _input.Split("\n"))
		{
			if (firstSection)
			{
				if (string.IsNullOrEmpty(line))
				{
					firstSection = false;
					continue;
				}

				var parts = line.Split('|');
				var x = int.Parse(parts[0]);
				var y = int.Parse(parts[1]);

				if (pageOrderingRules.TryGetValue(x, out var list))
					list.Add(y);
				else
					pageOrderingRules[x] = [y];
			}
			else
			{
				if (string.IsNullOrEmpty(line))
					break;

				updates.Add(line.Split(',').Select(int.Parse).ToList());
			}
		}

		return (pageOrderingRules, updates);
	}
}
