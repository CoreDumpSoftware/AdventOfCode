namespace AdventOfCode.Services;

public class PuzzleSolutionFactoryException(string message, Exception? innerException = null) : Exception(message, innerException) { }