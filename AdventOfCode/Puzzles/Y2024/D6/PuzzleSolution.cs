using AdventOfCode.DataStructures;
using AdventOfCode.Enums;
using AdventOfCode.Services;

namespace AdventOfCode.Puzzles.Y2024.D6;

public class PuzzleSolution : IPuzzleSolution
{
	private readonly string _input;
	private readonly Matrix<char> _matrix;
	private readonly int _boundary;
	private readonly Coordinate _start;
	private readonly Direction _startingDirection;

	public PuzzleSolution(string input)
	{
		_input = input;
		_matrix = Matrix<char>.ParseTextToMatrix(_input);
		_start = (91, 69);
		//_start = (4, 6); // sample input
		_startingDirection = Direction.Up;
		_boundary = _matrix.Columns - 1;
	}

	public async Task<object> PartOne()
	{
		var loc = _start;
		var dir = _startingDirection;
		var visited = new HashSet<int> { loc.GetHashCode() };

		try
		{
			while (loc.X != 0 && loc.X != _boundary && loc.Y != 0 && loc.Y != _boundary)
			{
				var next = loc.GetAdjacent(dir).Coordinate;
				if (_matrix[next] == '#')
				{
					dir = dir.RotateClockwise();
					next = loc.GetAdjacent(dir).Coordinate;
				}

				loc = next;
				visited.Add(loc.GetHashCode());
			}
		}
		catch (Exception ex)
		{
			throw new Exception("???", ex);
		}

		return await Task.FromResult(visited.Count);
	}

	public async Task<object> PartTwo()
	{
		return await Task.FromResult(0);
	}
}
