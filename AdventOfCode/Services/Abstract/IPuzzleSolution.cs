namespace AdventOfCode.Services;

public interface IPuzzleSolution
{
	Task<long> PartOne();
	Task<long> PartTwo();
}
