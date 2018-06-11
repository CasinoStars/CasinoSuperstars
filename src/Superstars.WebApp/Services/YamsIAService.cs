﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Superstars.WebApp.Services
{
    public class YamsIAService
    {
        YamsService _yamsService;
        int[] _actualBestHand = new int[5] { 0, 0, 0, 0, 0 }; // on met à 0 comme ça le nombre de points de cette mmain est à 0 et la probabilité de cette main est la plus haute
		int[] _myHand = new int[5];
		int _enemyPoints;

		public YamsIAService(YamsService yamsService)
		{
            _yamsService = yamsService;
		}

        private int PointCount(int[] hand) => _yamsService.PointCount(hand);

        private void ChangeOneDice()
		{
			int[] testHand = new int[5];
			for (int index = 0; index < 5; index++)
			{
				for (int diceValue = 1; diceValue <= 6; diceValue++)
				{
					Array.Copy(_myHand, testHand, 5);
					testHand[index] = diceValue;
					if (HandProba(NumberRerollDice(testHand)) >= HandProba(NumberRerollDice(_actualBestHand)) && (PointCount(testHand) > _enemyPoints))
					{
						Array.Copy(testHand, _actualBestHand, 5);
					}
				}
			}
		}

		private void ChangeTwoDices()
		{
			int[] testHand = new int[5];

			for (int indexDice1 = 0; indexDice1 < 5; indexDice1++) // index of the first dice we change in the hand
			{
				for (int valueDiceOne = 1; valueDiceOne <= 6; valueDiceOne++) // value of the first dice we change in the hand
				{
					for (int indexDice2 = 0; (indexDice2 < 5) && (indexDice2 != indexDice1); indexDice2++) // index of the first dice we change in the hand
					{
						for (int valueDice2 = 1; valueDice2 <= 6; valueDice2++) // value of the first dice we change in the hand
						{
							Array.Copy(_myHand, testHand, 5);
							testHand[indexDice1] = valueDiceOne;
							testHand[indexDice2] = valueDice2;
							if (HandProba(NumberRerollDice(testHand)) >= HandProba(NumberRerollDice(_actualBestHand)))
							{
								if (PointCount(testHand) > _enemyPoints)
								{
									Array.Copy(testHand, _actualBestHand, 5);
								}
							}
						}
					}
				}
			}
		}

		private void ChangeThreeDices()
		{
			int[] testHand = new int[5];
			for (int indexDice1 = 0; indexDice1 < 5; indexDice1++)
			{
				for (int indexDice2 = 0; indexDice2 < 5; indexDice2++)
				{
					for (int indexDice3 = 0; indexDice3 < 5; indexDice3++)
					{
						for (int valueDice1 = 1; valueDice1 <= 6; valueDice1++)
						{
							for (int valueDice2 = 1; valueDice2 <= 6; valueDice2++)
							{
								for (int valueDice3 = 1; valueDice3 <= 6; valueDice3++)
								{
									Array.Copy(_myHand, testHand, 5);
									testHand[indexDice1] = valueDice1;
									testHand[indexDice2] = valueDice2;
									testHand[indexDice3] = valueDice3;
									if (HandProba(NumberRerollDice(testHand)) >= HandProba(NumberRerollDice(_actualBestHand)) && PointCount(testHand) > _enemyPoints)
									{
										Array.Copy(testHand, _actualBestHand, 5);
									}
								}
							}
						}
					}
				}
			}
		}

		private void ChangeFourDices()
		{
			int[] testHand = new int[5];

			for (int indexUnchangedDice = 0; indexUnchangedDice < 5; indexUnchangedDice++) // index of the unchanged dice in the hand
			{
				for (int valueDice1 = 1; valueDice1 <= 6; valueDice1++) // value of the first dice we change 
				{
					for (int valueDice2 = 1; valueDice2 <= 6; valueDice2++) // value of the second dice we change
					{
						for (int valueDice3 = 1; valueDice3 <= 6; valueDice3++) // value of the third dice we change
						{
							for (int valueDice4 = 1; valueDice4 <= 6; valueDice4++) // value of the forth dice we change
							{
								Array.Copy(_myHand, testHand, 5);
								List<int> values = new List<int> { valueDice1, valueDice2, valueDice3, valueDice4 };
								values.Insert(indexUnchangedDice, testHand[indexUnchangedDice]);
								for (int i = 0; i < 5; i++)
								{
									testHand[i] = values[i];
								}
								if (HandProba(NumberRerollDice(testHand)) >= HandProba(NumberRerollDice(_actualBestHand)) && PointCount(testHand) > _enemyPoints)
								{
									Array.Copy(testHand, _actualBestHand, 5);
								}
							}
						}
					}
				}
			}
		}

		public int[] GiveRerollHand(int[] hand, int enemypoints)
		{
			_myHand = hand;
			_enemyPoints = enemypoints;
			ChangeOneDice();
			if (HandNotNice(_actualBestHand))
			{
				ChangeTwoDices();
			}
			if (HandNotNice(_actualBestHand))
			{
				ChangeThreeDices();
			}
			if (HandNotNice(_actualBestHand))
			{
				ChangeFourDices();
			}
			int[] toRerollHand = new int[5];
			Array.Copy(_myHand, toRerollHand, 5);

			for (int i = 0; i < 5; i++)
			{
				if (_actualBestHand[i] != toRerollHand[i])
				{
					toRerollHand[i] = 0;
				}
			}
			return toRerollHand;
		}

		#region Tolls methodes

		private bool HandNotNice(int[] hand)
		{
			int k = 0;
			foreach (int i in hand)
			{
				if (i == 0) { k++; }
			}

			if (k == 5)
			{
				return true;
			}
			return false;
		}

		private int[] DicesValue(int[] hand)
		{
			int[] count = new int[6] { 0, 0, 0, 0, 0, 0 };
			for (int i = 0; i < 5; i++)
			{
				for (int z = 1; z <= 6; z++)
				{
					if (hand[i] == z) count[z - 1]++;
				}
			}
			return count;
		}

		private float HandProba(int diceRerollNumber)
		{
			float proba;
			switch (diceRerollNumber)
			{
				case 0:
					proba = 1;
					break;
				case 1:
					proba = ((float)1 / (float)6);
					break;
				case 2:
					proba = ((float)2 / (float)36);
					break;
				case 3:
					proba = ((float)6 / (float)(6 * 6 * 6));
					break;
				case 4:
					proba = ((float)24 / (float)(6 * 6 * 6 * 6));
					break;
				case 5:
					proba = ((float)120 / (float)(6 * 6 * 6 * 6 * 6));
					break;
				default:
					throw new NotImplementedException("bad number of dices");
			}
			return proba;
		} // give proba according to the number of dices we reroll in the same time

		private int NumberRerollDice(int[] handAfterReroll)
		{
			int numberOfDiceReroll = 0;
			for (int i = 0; i < _myHand.Length; i++)
			{
				if (_myHand[i] != handAfterReroll[i])
				{
					numberOfDiceReroll++;
				}
			}
			return numberOfDiceReroll;
		}
		#endregion
	}
}

