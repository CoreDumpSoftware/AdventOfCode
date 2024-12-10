using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventOfCode.DataStructures;
using AdventOfCode.Services;

namespace AdventOfCode.Puzzles.Y2024.D10;
public class PuzzleSolution : IPuzzleSolution
{
	private readonly string _input;
	private readonly Matrix<char> _matrix;
	private List<Coordinate> _trailHeads = [];

	public PuzzleSolution(string input)
	{
		_input = input;
		_matrix = Matrix<char>.ParseTextToMatrix(_input, (coordinate, c) =>
		{
			if (c == '0')
				_trailHeads.Add(coordinate);
		});
	}

	public async Task<object> PartOne()
	{
		var totalScore = 0;
		foreach (var trailHead in _trailHeads)
		{
			var endsFound = new HashSet<Coordinate>();
			NavigatePartOne(trailHead, 1, endsFound);

			totalScore += endsFound.Count;
		}

		return await Task.FromResult(totalScore);
	}

	private void NavigatePartOne(Coordinate coordinate, int toFind, HashSet<Coordinate> endsFound)
	{
		char charToFind = (char)(toFind + '0');
		var adjacents = _matrix.GetAdjacents(coordinate, Enums.AdjacentType.Orthogonal)
			.Where(a => _matrix[a.Coordinate] == charToFind)
			.ToArray();

		foreach (var adjacent in adjacents)
		{
			if (toFind == 9)
				endsFound.Add(adjacent.Coordinate);
			else
				NavigatePartOne(adjacent.Coordinate, toFind + 1, endsFound);
		}
	}

	public async Task<object> PartTwo()
	{
		var totalScore = 0;
		foreach (var trailHead in _trailHeads)
		{
			List<List<Coordinate>> knownPaths = [];
			NavigatePartTwo(trailHead, 1, knownPaths, []);
			totalScore += knownPaths.Count;
		}

		return await Task.FromResult(totalScore);
	}

	private void NavigatePartTwo(Coordinate coordinate, int toFind, List<List<Coordinate>> knownPaths, List<Coordinate> currentPath)
	{
		currentPath.Add(coordinate);
		char charToFind = (char)(toFind + '0');
		var adjacents = _matrix.GetAdjacents(coordinate, Enums.AdjacentType.Orthogonal)
			.Where(a => _matrix[a.Coordinate] == charToFind)
			.ToArray();

		foreach (var adjacent in adjacents)
		{
			if (toFind == 9)
			{
				knownPaths.Add(currentPath);
			}
			else
			{
				NavigatePartTwo(adjacent.Coordinate, toFind + 1, knownPaths, new List<Coordinate>(currentPath));
			}
		}
	}
}
