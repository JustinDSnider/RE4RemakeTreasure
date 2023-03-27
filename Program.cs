using System.Collections;
using System.Dynamic;
using System.Linq;

namespace RE4Treasure
{
	internal class Program
	{
		#region GemValues
		private static Dictionary<string, int> gemValues = new Dictionary<string, int>()
		{
			{ "Ruby" , 3000 },
			{ "Sapphire" , 4000 },
			{ "Diamond" , 7000 },
			{ "Emerald" , 5000 },
			{ "Alexandrite" , 6000 },
			{ "Beryl" , 9000 }
		};
		#endregion

		#region TreasureBase
		static int ElegantMask = 5000;
		static int Flagon = 4000;
		static int SpangledBangle = 4000;
		static int ElegantBangle = 5000;
		static int Crown = 19000;
		static int OrnateNecklace = 11000;
		static int ButterflyLamp = 6000;
		static int Chalice = 7000;
		static int Lynx = 15000;
		static int Clock = 9000;
		#endregion

		private static void GeneratePermutationsHelper(List<string> gemList, int maxSize, List<string> permutation, List<List<string>> permutations)
		{
			if (permutation.Count == maxSize)
			{
				permutation.Sort();
				if (permutations.Any(p => p.SequenceEqual(permutation)))
				{
					permutations.Add(permutation.ToList());
				}
				
			}
			else
			{
				foreach (string gem in gemList)
				{
					if (!permutation.Contains(gem))
					{
						permutation.Add(gem);
						GeneratePermutationsHelper(gemList, maxSize, permutation, permutations);
						permutation.RemoveAt(permutation.Count - 1);
					}
				}
			}
		}

		static List<List<string>> GeneratePermutations(List<string> gemList, int gemCount)
		{
			List<List<string>> permutations = new List<List<string>>();
			

			if (gemCount == 1)
			{
				foreach (string gem in gemList)
				{
					permutations.Add(new List<string>() { gem });
				}
				return permutations;
			}
			foreach (string gem in gemList)
			{
				List<string> remainingList = new List<string>(gemList);

				List<List<string>> subPermutations = GeneratePermutations(remainingList, gemCount - 1);

				foreach (List<string> subPermutation in subPermutations)
				{
					subPermutation.Insert(0, gem);
					subPermutation.Sort();
					if (!permutations.Any(existingPermutation => existingPermutation.SequenceEqual(subPermutation)))
					{
						permutations.Add(subPermutation);
					}
				}
			}
			return permutations;
		}

		static List<List<string>> GeneratePermutationOfPermutations(List<List<string>> roundPermutations, List<List<string>> squarePermutations)
		{
			List<List<string>> permutations = new List<List<string>>();

			foreach (List<string> round in roundPermutations)
			{
				foreach (List<string> square in squarePermutations)
				{
					List<string> permutation = new List<string>();
					permutation.AddRange(round);
					permutation.AddRange(square);
					permutation.Sort();
					if (!permutations.Any(existingPermutation => existingPermutation.SequenceEqual(permutation)))
					{
						permutations.Add(permutation);
					}
				}
			}
			
			return permutations;
		}

		static List<List<string>> GetPermutations(int roundGemCount, int squareGemCount)
		{
			#region Gems
			List<string> roundGems = new() { "Ruby", "Sapphire", "Diamond" };
			List<string> squareGems = new() { "Emerald", "Alexandrite", "Beryl" };
			#endregion

			List<List<string>> roundPermutations = new();
			List<List<string>> squarePermutations = new();
			List<List<string>> finalPermutations;

			if (roundGemCount > 0)
			{
				roundPermutations = GeneratePermutations(roundGems, roundGemCount);
			}
			if (squareGemCount > 0)
			{
				squarePermutations = GeneratePermutations(squareGems, squareGemCount);
			}
			if (squarePermutations.Count != 0 && squarePermutations.Count != 0)
			{
				finalPermutations = GeneratePermutationOfPermutations(roundPermutations, squarePermutations);
			}
			else
			{
				if (squarePermutations.Count != 0)
				{
					finalPermutations = squarePermutations;
				}
				else
				{
					finalPermutations = roundPermutations;
				}
			}
			return finalPermutations;
		}

		static float GetMultiplier(int totalGems, List<string> currPermutation)
		{
			HashSet<string> uniqueGems = new HashSet<string>();
			List<int> gemCounts = new List<int>();

			List<string> gemColors = new List<string>();

			foreach (string gem in currPermutation)
			{
				string color = string.Empty;

				if (gem == "Ruby" || gem == "Beryl")
				{
					gemColors.Add("Red");
				}
				else if (gem == "Sapphire")
				{
					gemColors.Add("Blue");
				}
				else if (gem == "Diamond")
				{
					gemColors.Add("Yellow");
				}
				else if (gem == "Emerald")
				{
					gemColors.Add("Green");
				}
				else if (gem == "Alexandrite")
				{
					gemColors.Add("Purple");
				}
			}

			foreach (string color in gemColors)
			{
				if (uniqueGems.Contains(color))
				{
					int index = new List<string> (uniqueGems).IndexOf(color);
					gemCounts[index]++;
				}
				else
				{
					uniqueGems.Add (color);
					gemCounts.Add(1);
				}
			}

			gemCounts.Sort();
			gemCounts.Reverse();

			int length = gemCounts.Count;

			if (length == 5)
			{
				return 2.0f;
			}

			else if (gemCounts[0] == 5)
			{
				return 1.9f;
			}

			else if ((length >= 2) && gemCounts[0] == 3 && gemCounts[1] == 2)
			{
				return 1.8f;
			}

			else if (gemCounts[0] == 4)
			{
				return 1.7f;
			}

			else if ((length >= 4) && gemCounts[0] >= 1 && gemCounts[1] == 1 && gemCounts[2] == 1 && gemCounts[3] == 1)
			{
				return 1.6f;
			}

			else if ((length >= 2) && gemCounts[0] == 2 && gemCounts[1] == 2)
			{
				return 1.5f;
			}

			else if (gemCounts[0] == 3)
			{
				return 1.4f;
			}

			else if ((length >= 3) && gemCounts[0] >= 1 && gemCounts[1] == 1 && gemCounts[2] == 1)
			{
				return 1.3f;
			}

			else if (gemCounts[0] == 2)
			{
				return 1.2f;
			}

			else if (gemCounts[0] == 1 && gemCounts[1] == 1)
			{
				return 1.1f;
			}

			else
			{
				return 1.0f;
			}
		}

		static void GenerateTreasure(string name, int baseValue, int roundGemCount, int squareGemCount)
		{
			List<List<string>> gemPermutations = GetPermutations(roundGemCount, squareGemCount);

			using (StreamWriter sw = File.CreateText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), $"{name}.csv")))
			{
				sw.WriteLine("Gem List, Base Value, Total Gem Value, Multiplier, Overall Value, Overall Minus Gem Value");
				foreach (var item in gemPermutations)
				{
					int totalGemValue = 0;
					string gemList = string.Join(" ", item);
					float multiplier = GetMultiplier((roundGemCount + squareGemCount), item);
					foreach (var item2 in item)
					{
						totalGemValue += gemValues[item2];
					}

					//Console.WriteLine($"{gemList}: (({baseValue} + {totalGemValue})) * {multiplier} = {Convert.ToSingle(baseValue + totalGemValue) * multiplier}");
					int totalValue = Convert.ToInt32(Convert.ToSingle(baseValue + totalGemValue) * multiplier);
					int totalValueAdjusted = totalValue - totalGemValue;
					sw.WriteLine($"{gemList}, {baseValue}, {totalGemValue}, {multiplier}, {totalValue}, {totalValueAdjusted}");
				}
			}
		}

		static void Main(string[] args)
		{
			//Elegant Mask
			GenerateTreasure("ElegantMask", ElegantMask, 3, 0);

			//Flagon
			GenerateTreasure("Flagon", Flagon, 2, 0);

			//Crown
			GenerateTreasure("Crown", Crown, 2, 3);

			//SpangledBangle
			GenerateTreasure("SpangledBangle", SpangledBangle, 0, 2);

			//ElegantBangle
			GenerateTreasure("ElegantBangle", ElegantBangle, 2, 0);

			//OrnateNecklace
			GenerateTreasure("OrnateNecklace", OrnateNecklace, 2, 2);

			//ButterflyLamp
			GenerateTreasure("ButterflyLamp", ButterflyLamp, 3, 0);

			//ChaliceOfAtonement
			GenerateTreasure("ChaliceOfAtonement", Chalice, 0, 3);

			//Lynx
			GenerateTreasure("GoldenLynx", Lynx, 2, 1);

			//Clock
			GenerateTreasure("ExtravagantClock", Clock, 1, 1);
		}
	}
};