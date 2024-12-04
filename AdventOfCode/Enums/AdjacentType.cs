namespace AdventOfCode.Enums;

public enum AdjacentType
{
	None = 0,
	Orthogonal = 1, // Only left, right, up, or down
	Diagonal = 2, // Only up left, up right, down left, down right
	All = 3
}

public static class AdjacentTypeExtensions
{
	public static Direction[] GetDirections(this AdjacentType type) => type switch
	{
		AdjacentType.None => [],

		AdjacentType.Orthogonal => [Direction.Up, Direction.Left, Direction.Right, Direction.Down],

		AdjacentType.Diagonal =>
			[Direction.Up | Direction.Left, Direction.Up | Direction.Right,
			 Direction.Down | Direction.Left, Direction.Down | Direction.Right],

		AdjacentType.All =>
			[Direction.Up | Direction.Left, Direction.Up, Direction.Up | Direction.Right,
			 Direction.Left, Direction.Right,
			 Direction.Down | Direction.Left, Direction.Down, Direction.Down | Direction.Right],

		_ => throw new ArgumentException($"Invalid adjacent type, type = {(int)type}")
	};
}