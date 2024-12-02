namespace AdventOfCode.Services.Abstract;

public interface IPuzzleSolutionFactory
{
	Task<IPuzzleSolution> GetPuzzleSolution(int year, int day);
}
