namespace AdventOfCode.Enums;

[Flags]
public enum Direction : byte
{
	Undefined = 0,
	Up = 1 << 0,
	Down = 1 << 1,
	Left = 1 << 2,
	Right = 1 << 3
}

public static class DirectionExtensions
{
	public static bool IsValid(this Direction direction)
	{
		if (direction == Direction.Undefined)
			return false;

		if (direction.HasFlag(Direction.Up) && direction.HasFlag(Direction.Down))
			return false;

		if (direction.HasFlag(Direction.Left) && direction.HasFlag(Direction.Right))
			return false;

		if (((byte)direction & 0xF0) != 0)
			return false;

		return true;
	}
}