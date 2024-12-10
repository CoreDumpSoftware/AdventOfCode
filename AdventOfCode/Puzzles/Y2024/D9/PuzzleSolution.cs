using AdventOfCode.Services;

namespace AdventOfCode.Puzzles.Y2024.D9;

public class PuzzleSolution : IPuzzleSolution
{
	private readonly string _input;

	private interface IStorage
	{
		int Size { get; }
	}

	private List<char> _disk;

	public PuzzleSolution(string input)
	{
		_input = input;
	}

	public async Task<object> PartOne()
	{
		var memory = new int[100000];
		var endIndex = 0;

		bool isFile = true;
		int id = 1;
		foreach (var c in _input.Trim())
		{
			var count = c - '0';

			var fill = isFile ? id++ : 0;

			Array.Fill(memory, fill, endIndex, count);

			endIndex += count;
			isFile = !isFile;
		}

		var index = 0;
		var revIndex = endIndex;
		var result = 0L;

		while (index < revIndex)
		{
			// scan forward until empty
			while (memory[index] != 0)
				index++;

			// scan reverse until non-empty
			while (memory[revIndex] == 0)
				revIndex--;

			if (index > revIndex)
				break;

			memory[index] = memory[revIndex];
			memory[revIndex] = 0;

			index++;
			revIndex--;
		}

		result = 0L;
		for (var i = 0; i < endIndex; i++)
		{
			if (memory[i] == 0)
				break;

			result += i * (memory[i] - 1);
		}

		return result;
	}

	private class EmptySpace(int index, int size)
	{
		public int Index { get; set; } = index;
		public int Size { get; set; } = size;
	}

	public async Task<object> PartTwo()
	{
		var memory = new int[100000];
		var endIndex = 0;

		bool isFile = true;
		int id = 1;
		var emptyGroups = new List<EmptySpace>();
		var files = new List<(int Id, int Index, int Size)>();
		foreach (var c in _input.Trim())
		{
			var count = c - '0';

			var fill = isFile ? id++ : 0;

			Array.Fill(memory, fill, endIndex, count);
			if (isFile)
				files.Add((fill, endIndex, count));
			else
				emptyGroups.Add(new(endIndex, count));

			endIndex += count;
			isFile = !isFile;
		}

		var result = 0L;

		foreach (var file in files.AsEnumerable().Reverse())
		{
			var group = emptyGroups.FirstOrDefault(g => g.Size >= file.Size);
			if (group == null || group.Index > file.Index)
				continue;

			Array.Fill(memory, file.Id, group.Index, file.Size);
			Array.Fill(memory, 0, file.Index, file.Size);
			group.Size -= file.Size;
			if (group.Size == 0)
				emptyGroups.Remove(group);
			else
				group.Index += file.Size;
		}

		result = 0L;
		for (var i = 0; i < endIndex; i++)
		{
			if (memory[i] > 0)
				result += i * (memory[i] - 1);
		}

		return result;
	}
}
