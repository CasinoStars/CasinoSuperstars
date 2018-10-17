﻿using Superstars.WebApp.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Superstars.WebApp
{
    public class Deck
    {
        public string[] Symbole = new string[] { "c", "h", "p", "t" };

        public int[] Valeur = new int[] { 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 };

        public List<Card> DeckCards { get; set; }

        public void CreateDeck()
        {
            List<Card> NewDeck = new List<Card>();

            for (int i = 0; i < Symbole.Length; i++)
            {
                for (int j = 0; j < Valeur.Length; j++)
                {
                    NewDeck.Add(new Card(Symbole[i], Valeur[j]));
                }
            }
            DeckCards = NewDeck;
        }

        public void Shuffle()
        {
            for (int i = 0; i < 52; i++)
            {
                DeckCards.Add(RandomDraw());
            }
        }

        public Card RandomDraw()
        {
            Random rnd = new Random();
            int random = rnd.Next(1, DeckCards.Count);
            var drawedCard = DeckCards.ElementAt(random);
            DeckCards.RemoveAt(random);
            return drawedCard;
        }

        public Card Draw()
        {
            int top = DeckCards.Count;
            var drawedcard = DeckCards.ElementAt(top - 1);
            DeckCards.RemoveAt(top - 1);
            return drawedcard;
        }
    }
}
