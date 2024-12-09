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
		//_start = (1, 3); // sample input 2
		_startingDirection = Direction.Up;
		_boundary = _matrix.Columns - 1;
	}

	public async Task<object> PartOne()
	{
		if (!_matrix.IsValid(_start) || _matrix[_start] != '^')
			throw new Exception("Wrong starting location!");

		var loc = _start;
		var dir = _startingDirection;
		HashSet<Coordinate> visited = [loc];

		while (!HeadingOutOfBounds(loc, dir))
		{
			var next = loc.GetAdjacent(dir).Coordinate;
			if (_matrix[next] == '#')
			{
				dir = dir.RotateClockwise();
				next = loc.GetAdjacent(dir).Coordinate;
			}

			loc = next;
			visited.Add(loc);
		}

		return await Task.FromResult(visited.Count);
	}

	public async Task<object> PartTwo()
	{
		if (!_matrix.IsValid(_start) || _matrix[_start] != '^')
			throw new Exception("Wrong starting location!");

		//_matrix.Print();

		var loc = _start;
		var dir = _startingDirection;
		var obstacles = 0;

		while (!HeadingOutOfBounds(loc, dir))
		{
			var original = _matrix[loc];
			//_matrix[loc] = 'X';
			_matrix.Print(WithColor);
			//_matrix[loc] = original;

			var next = loc.GetAdjacent(dir).Coordinate;
			var c = dir == Direction.Up || dir == Direction.Down ? '|' : '-';
			while (_matrix[next] == '#')
			{
				dir = dir.RotateClockwise();
				next = loc.GetAdjacent(dir).Coordinate;
				c = '+';
			}

			if (DetermineInfiniteLoop(loc, dir))
			{
				obstacles++;
				var nextOriginal = _matrix[next];

				//_matrix[next] = 'O';
				//_matrix[loc] = 'X';

				//_matrix.Print(WithColor);

				//_matrix[loc] = original;
				//_matrix[next] = nextOriginal;
			}

			if ((original == '-' && dir.IsVertical()) || (original == '|' && dir.IsHorizontal()))
			{
				_matrix[loc] = '+';
			}
			else if (_matrix[loc] != '^')
			{
				_matrix[loc] = c;
			}

			loc = next;

		}

		_matrix.Print(WithColor);

		return await Task.FromResult(obstacles);
	}

	private bool DetermineInfiniteLoop(Coordinate loc, Direction dir)
	{
		HashSet<(Coordinate, Direction)> visited = [(loc, dir)];
		var obstacle = loc.GetAdjacent(dir).Coordinate;
		var result = false;

		while (!result && !HeadingOutOfBounds(loc, dir))
		{
			var next = loc.GetAdjacent(dir).Coordinate;
			while (next == obstacle || _matrix[next] == '#')
			{
				dir = dir.RotateClockwise();
				next = loc.GetAdjacent(dir).Coordinate;
			}

			loc = next;
			if (visited.Contains((loc, dir)))
				result = true;
			else
				visited.Add((loc, dir));
		}

		//if (result)
		//	Console.WriteLine(obstacle);

		return result;
	}

	private bool HeadingOutOfBounds(Coordinate c, Direction d) =>
		(c.X == 0 && d == Direction.Left) ||
		(c.X == _matrix.Columns - 1 && d == Direction.Right) ||
		(c.Y == 0 && d == Direction.Up) ||
		(c.Y == _matrix.Rows - 1 && d == Direction.Down);

	private static void WithColor(char c)
	{
		var defaultForeground = Console.ForegroundColor;

		if (c == '^')
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
		}
		else if (c == '-' || c == '|' || c == '+')
		{
			Console.ForegroundColor = ConsoleColor.Green;
		}
		else if (c == 'X')
		{
			Console.ForegroundColor = ConsoleColor.Red;
		}
		else if (c == 'O')
		{
			Console.ForegroundColor = ConsoleColor.Magenta;
		}
		else if (c == '#')
		{
			Console.ForegroundColor = ConsoleColor.Gray;
		}

		Console.Write(c);
		Console.ForegroundColor = defaultForeground;
	}
}
