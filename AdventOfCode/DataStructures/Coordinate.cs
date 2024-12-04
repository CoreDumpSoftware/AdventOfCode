using AdventOfCode.Enums;

namespace AdventOfCode.DataStructures;

public record Coordinate(int X, int Y)
{
	public static implicit operator Coordinate((int X, int Y) tuple) => new(tuple.X, tuple.Y);

	public static Coordinate operator +(Coordinate left, Coordinate right) => new(left.X + right.X, left.Y + right.Y);

	public Direction GetDirectionTo(Coordinate coordinate)
	{
		Direction dir = Direction.Undefined;
		var deltaX = coordinate.X - X;
		var deltaY = coordinate.Y - Y;

		if (deltaX < 0)
			dir |= Direction.Left;
		else if (deltaX > 0)
			dir |= Direction.Right;

		if (deltaY < 0)
			dir |= Direction.Up;
		if (deltaY > 0)
			dir |= Direction.Down;

		return dir;
	}

	public Adjacent GetAdjacent(Direction dir)
	{
		if (!dir.IsValid())
			throw new ArgumentException($"Invalid direction, dir = {(int)dir}");

		var y = dir.HasFlag(Direction.Up)
			? -1
			: dir.HasFlag(Direction.Down)
				? 1
				: 0;

		var x = dir.HasFlag(Direction.Left)
			? -1
			: dir.HasFlag(Direction.Right)
				? 1
				: 0;

		return new(this + (x, y), dir);
	}
}
