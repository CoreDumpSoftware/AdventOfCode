using AdventOfCode.DataStructures;
using AdventOfCode.Enums;
using AdventOfCode.Services;

namespace AdventOfCode.Puzzles.Y2024.D4;
public class PuzzleSolution(string input) : IPuzzleSolution
{
	private readonly string _input = input;

	public async Task<object> PartOne()
	{
		var matrix = Matrix<char>.ParseTextToMatrix(_input);
		int count = 0;

		for (var y = 0; y < matrix.Rows; y++)
		{
			for (var x = 0; x < matrix.Columns; x++)
			{
				Coordinate origin = (x, y);
				if (matrix[origin] != 'X')
					continue;

				foreach (var adjacent in matrix.GetAdjacents(origin, AdjacentType.All).Where(a => matrix[a.Coordinate] == 'M'))
				{
					var third = adjacent.Coordinate.GetAdjacent(adjacent.Direction);
					if (!matrix.IsValid(third.Coordinate) || matrix[third.Coordinate] != 'A')
						continue;

					var fourth = third.Coordinate.GetAdjacent(adjacent.Direction);
					if (!matrix.IsValid(fourth.Coordinate) || matrix[fourth.Coordinate] != 'S')
						continue;

					count++;
				}
			}
		}

		return await Task.FromResult(count);
	}

	public async Task<object> PartTwo()
	{
		var matrix = Matrix<char>.ParseTextToMatrix(_input);
		int count = 0;

		for (var y = 0; y < matrix.Rows; y++)
		{
			for (var x = 0; x < matrix.Columns; x++)
			{
				Coordinate origin = (x, y);
				if (matrix[origin] != 'A')
					continue;

				var adjacents = matrix.GetAdjacents(origin, AdjacentType.Diagonal).ToArray();
				if (adjacents.Length != 4)
					continue;

				/// M . M
				/// . A .
				/// S . S
				if (matrix[adjacents[0].Coordinate] == 'M' && matrix[adjacents[1].Coordinate] == 'M' &&
					matrix[adjacents[2].Coordinate] == 'S' && matrix[adjacents[3].Coordinate] == 'S')
					count++;
				/// M . S
				/// . A .
				/// M . S
				else if (matrix[adjacents[0].Coordinate] == 'M' && matrix[adjacents[1].Coordinate] == 'S' &&
					matrix[adjacents[2].Coordinate] == 'M' && matrix[adjacents[3].Coordinate] == 'S')
					count++;
				/// S . S
				/// . A .
				/// M . M
				else if (matrix[adjacents[0].Coordinate] == 'S' && matrix[adjacents[1].Coordinate] == 'S' &&
					matrix[adjacents[2].Coordinate] == 'M' && matrix[adjacents[3].Coordinate] == 'M')
					count++;
				/// S . M
				/// . A .
				/// S . M
				else if (matrix[adjacents[0].Coordinate] == 'S' && matrix[adjacents[1].Coordinate] == 'M' &&
					matrix[adjacents[2].Coordinate] == 'S' && matrix[adjacents[3].Coordinate] == 'M')
					count++;
			}
		}

		return await Task.FromResult(count);
	}
}
