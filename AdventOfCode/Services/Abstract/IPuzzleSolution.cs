namespace AdventOfCode.Services;

public interface IPuzzleSolution
{
	Task<object> PartOne();
	Task<object> PartTwo();
}
