namespace AdventOfCode.Services.Abstract;

public interface IInputProvider
{
	Task<string> GetInput(int year, int day);
}
