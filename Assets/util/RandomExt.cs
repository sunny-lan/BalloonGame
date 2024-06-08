using System;
using System.Linq;

static class RandomExtensions
{
	public static void Shuffle<T>(this Random rng, T[] array)
	{
		int n = array.Length;
		while (n > 1)
		{
			int k = rng.Next(n--);
			T temp = array[n];
			array[n] = array[k];
			array[k] = temp;
		}
	}

	public static T SelectByProbability<T>(this GenericDictionary<T, float> probability)
	{
		float total = probability.Values.Sum();
		float rng = UnityEngine.Random.Range(0, total);
		foreach (var (side, prob) in probability)
		{
			rng -= prob;
			if (rng < 0)
			{
				return side;
			}
		}

		throw new System.Exception("This shouldn't happen");
	}
}