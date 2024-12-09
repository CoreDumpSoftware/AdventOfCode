using System.Numerics;
using AnyOfTypes;

namespace AdventOfCode.DataStructures;

public class TrinaryNumber
{
	public static TrinaryNumber operator +(TrinaryNumber left, AnyOf<BigInteger, TrinaryNumber> right)
	{
		var r = right.IsFirst ? right.First : right.Second._base10;

		return new TrinaryNumber(left._base10 + r);
	}

	public static TrinaryNumber operator -(TrinaryNumber left, AnyOf<BigInteger, TrinaryNumber> right)
	{
		var r = right.IsFirst ? right.First : right.Second._base10;

		return new TrinaryNumber(left._base10 - r);
	}

	public static TrinaryNumber operator *(TrinaryNumber left, AnyOf<BigInteger, TrinaryNumber> right)
	{
		var r = right.IsFirst ? right.First : right.Second._base10;

		return new TrinaryNumber(left._base10 * r);
	}

	public static TrinaryNumber operator /(TrinaryNumber left, AnyOf<BigInteger, TrinaryNumber> right)
	{
		var r = right.IsFirst ? right.First : right.Second._base10;

		return new TrinaryNumber(left._base10 / r);
	}

	public static TrinaryNumber operator %(TrinaryNumber left, AnyOf<BigInteger, TrinaryNumber> right)
	{
		var r = right.IsFirst ? right.First : right.Second._base10;

		return new TrinaryNumber(left._base10 % r);
	}

	public static TrinaryNumber operator ++(TrinaryNumber left)
	{
		var value = left._base10 + 1;
		return new TrinaryNumber(value);
	}

	public static TrinaryNumber operator --(TrinaryNumber left)
	{
		var value = left._base10 - 1;
		return new TrinaryNumber(value);
	}

	private const int Base = 3;

	private BigInteger _base10;
	public BigInteger Base10Value
	{
		get => _base10;
		set
		{
			_base10 = value;

		}
	}

	private List<byte> _trits;

	public TrinaryNumber(BigInteger number)
	{
		_base10 = number;
		_trits = Convert(number);
	}

	public TrinaryNumber(int numberOfTrits)
	{
		_base10 = 0;
		_trits = new(numberOfTrits);
	}

	public int GetNumberOfTrits() => _trits.Count;

	protected List<byte> Convert(BigInteger number)
	{
		var numTrits = GetNumberOfTrits(number);
		var result = new List<byte>(numTrits);
		while (number > 0)
		{
			_trits.Add((byte)(number % Base));
			number /= Base;
		}

		return result;
	}

	protected BigInteger ConvertToBase10()
	{
		BigInteger result = 0L;
		var power = 0;
		for (var i = _trits.Count - 1; i >= 0; i--)
		{
			var factor = BigInteger.Pow(Base, power++); //(long)Math.Pow(Base, power++);
			result += factor * _trits[i];
		}

		return result;
	}

	public byte GetTritAtPosition(int position)
	{
		if (position < 0)
			throw new ArgumentOutOfRangeException(nameof(position));

		if (position >= _trits.Count)
			return 0;

		return _trits[position];
	}

	/// <summary>
	/// Sets a trit from the provided <paramref name="position"/>. Values are enumerated from right to left.
	/// </summary>
	/// <exception cref="ArgumentOutOfRangeException"></exception>
	public void SetAtTritPosition(int position, byte value)
	{
		if (position < 0)
			throw new ArgumentOutOfRangeException(nameof(position));

		if (value >= Base)
			throw new ArgumentOutOfRangeException(nameof(value));

		if (position >= _trits.Count && value > 0)
		{
			var delta = (position - 1) - _trits.Count;
			_trits.InsertRange(0, new List<byte>(delta));
		}

		_trits[position] = value;
	}

	private static int GetNumberOfTrits(BigInteger value)
	{
		return (int)Math.Ceiling(BigInteger.Log(value, Base));
	}
}
