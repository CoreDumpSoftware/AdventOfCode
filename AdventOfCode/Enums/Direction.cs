using System.Runtime.CompilerServices;

namespace AdventOfCode.Enums;

[Flags]
public enum Direction : byte
{
	Undefined = 0,
	Up = 1 << 0,
	Down = 1 << 1,
	Left = 1 << 2,
	Right = 1 << 3,

	UpLeft = Up | Left,
	UpRight = Up | Right,
	DownLeft = Down | Left,
	DownRight = Down | Right
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

	public static Direction RotateClockwise(this Direction dir) => dir switch
	{
		Direction.Up => Direction.Right,
		Direction.Right => Direction.Down,
		Direction.Down => Direction.Left,
		Direction.Left => Direction.Up,

		Direction.UpLeft => Direction.UpRight,
		Direction.UpRight => Direction.DownRight,
		Direction.DownRight => Direction.DownLeft,
		Direction.DownLeft => Direction.UpLeft,

		_ => throw new ArgumentException(nameof(dir))
	};

	public static Direction RotateCounterClockwise(this Direction dir) => dir switch
	{
		Direction.Up => Direction.Left,
		Direction.Left => Direction.Down,
		Direction.Down => Direction.Right,
		Direction.Right => Direction.Up,

		Direction.UpLeft => Direction.DownLeft,
		Direction.DownLeft => Direction.DownRight,
		Direction.DownRight => Direction.UpRight,
		Direction.UpRight => Direction.UpLeft,

		_ => throw new ArgumentException(nameof(dir))
	};

	public static bool IsVertical(this Direction dir) => dir == Direction.Up || dir == Direction.Down;

	public static bool IsHorizontal(this Direction dir) => dir == Direction.Left || dir == Direction.Right;
}