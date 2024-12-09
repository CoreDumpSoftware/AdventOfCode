using AdventOfCode.Enums;

namespace AdventOfCode.DataStructures;

public class Matrix<T>
{
	public int Rows { get; set; }
	public int Columns { get; set; }

	private readonly T[] _data;

	public T this[int x, int y]
	{
		get => Get(x, y);
		set => Set(x, y, value);
	}

	public T this[Coordinate c]
	{
		get => Get(c.X, c.Y);
		set => Set(c.X, c.Y, value);
	}

	public Matrix(int rows, int columns)
	{
		Rows = rows;
		Columns = columns;

		_data = new T[Columns * Rows];
	}

	public bool IsValid(int x, int y) => x >= 0 && x < Columns && y >= 0 && y < Rows;
	public bool IsValid(Coordinate coordinate) => IsValid(coordinate.X, coordinate.Y);

	public IEnumerable<T> Row(int row)
	{
		ThrowIfInvalid(row, 0);

		for (var x = 0; x < Columns; x++)
		{
			yield return _data[Index(x, row)];
		}
	}

	public IEnumerable<T> Column(int column)
	{
		ThrowIfInvalid(0, column);

		for (var i = 0; i < Columns; i++)
		{
			yield return _data[Index(column, i)];
		}
	}

	public IEnumerable<Adjacent> GetAdjacents(Coordinate coordinate, AdjacentType type)
	{
		return type.GetDirections()
			.Select(coordinate.GetAdjacent)
			.Where(a => IsValid(a.Coordinate));
	}

	private T Get(int x, int y)
	{
		ThrowIfInvalid(x, y);

		return _data[Index(x, y)];
	}

	private void Set(int x, int y, T value)
	{
		ThrowIfInvalid(x, y);

		_data[Index(x, y)] = value;
	}

	private void ThrowIfInvalid(int x, int y)
	{
		var exceptions = new List<Exception>();

		if (x < 0 || x >= Columns)
			exceptions.Add(new ArgumentOutOfRangeException($"Column value out of bounds. Column = {x}, limits = (0 >= x < {Columns})"));

		if (y < 0 || y >= Rows)
			exceptions.Add(new ArgumentOutOfRangeException($"Row value out of bounds. Row = {y}, limits = (0 >= y < {Rows})"));

		if (exceptions.Any())
			throw exceptions.Count() > 1
				? new AggregateException(exceptions)
				: exceptions.First();
	}

	public static Matrix<char> ParseTextToMatrix(string text)
	{
		var lines = text.Split('\n');
		Matrix<char> matrix = null!;
		var lineLength = 0;
		var y = 0;

		foreach (var line in text.Split('\n'))
		{
			if (string.IsNullOrEmpty(line))
				continue;

			if (matrix == null)
			{
				lineLength = line.Length;
				matrix = new Matrix<char>(lines.Count() - 1, line.Length); // exclude the empty line at the end of all input
			}
			else if (line.Length != lineLength)
			{
				throw new ArgumentException($"Text input must have the same column length for all lines");
			}

			var x = 0;
			foreach (var c in line)
				matrix[x++, y] = c;

			y++;
		}

		return matrix;
	}

	private int Index(int x, int y) => y * Columns + x;
}

public static class CharMatrixExtensions
{
	/// <summary>
	/// Naïve print method to print the matrix out to the console (does no formatting to make it pretty)
	/// </summary>
	public static void Print(this Matrix<char> matrix, Action<char> printFn = null!)
	{
		printFn ??= Console.Write;
		for (var y = 0; y < matrix.Rows; y++)
		{
			foreach (var c in matrix.Row(y))
			{
				printFn(c);
			};

			Console.WriteLine();
		}

		Console.WriteLine();
	}
}