using System;
using System.Collections.Generic;

namespace Cards
{
    public sealed class Card
    {
        /// <summary>
        /// Town from where we are travelling
        /// </summary>
        public string From { get; private set; }

        /// <summary>
        /// Town where we are travelling
        /// </summary>
        public string To { get; private set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public Card(string from, string to)
        {
            From = from;
            To = to;
        }

        /// <summary>
        /// Card data in a readable form
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("{0} -> {1}", From, To);
        }

        /// <summary>
        /// Order cards using one of the possible indexing algorythm.
        /// I prefer not to put such functionality in a separate classes to keep structure
        /// simple.
        /// </summary>
        /// <param name="cards"></param>
        /// <returns></returns>
        public static IEnumerable<Card> OrderCards(IEnumerable<Card> cards)
        {
            SortedDictionary<string, IndexEntry> index = new SortedDictionary<string, IndexEntry>();

            foreach(var card in cards)
            {
                // Add index entries for From and To
                if (!index.ContainsKey(card.From))
                    index.Add(card.From, new IndexEntry());
                index[card.From].From = card;

                if (!index.ContainsKey(card.To))
                    index.Add(card.To, new IndexEntry());
                index[card.To].To = card;
            }

            // Intialize collection and search for first card
            List<Card> ordered = new List<Card>();
            foreach (var entry in index)
            {
                if (entry.Value.To == null)
                {
                    ordered.Add(entry.Value.From);
                    break;
                }
            }

            // Ordering cards using index, assuming there no issues
            int pos = 0;
            while (true)
            {
                IndexEntry entry = index[ordered[pos++].To];
                if (entry.From == null)
                    break;
                ordered.Add(entry.From);
            }

            return ordered;
        }

        /// <summary>
        /// Index entry, store cards pair (From is null for last, To is null for first)
        /// </summary>
        sealed class IndexEntry
        {
            public Card From;
            public Card To;

            public IndexEntry()
            {
                From = null;
                To = null;
            }
        }
    }

    public static class Tests
    {
        /// <summary>
        /// Returns true if all is ok, false otherwise
        /// </summary>
        /// <returns></returns>
        public static bool Test()
        {
            // Test data
            List<Card> data = new List<Card>() {
                new Card("Мельбурн", "Кельн"),
                new Card("Москва", "Париж"),
                new Card("Кельн", "Москва") };

            // Run ordering
            IEnumerable<Card> ordered = Card.OrderCards(data);

            // Check order of cards
            Card prev = null;
            foreach (var card in ordered)
            {
                if (prev != null)
                {
                    if (prev.To != card.From)
                        return false;
                }
                prev = card;
            }

            return true;
        }
    }
}