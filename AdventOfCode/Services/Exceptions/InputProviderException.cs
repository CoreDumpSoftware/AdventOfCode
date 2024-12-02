namespace AdventOfCode.Services.Exceptions;

public class InputProviderException(string message, Exception? innerException = null) : Exception(message, innerException) { }