namespace AdventOfCode.Attributes;

public class PuzzleAttribute : Attribute
{
	public int Year { get; init; }
	public int Day { get; init; }

	public PuzzleAttribute(int year, int day)
	{
		Year = year;
		Day = day;
	}
}
