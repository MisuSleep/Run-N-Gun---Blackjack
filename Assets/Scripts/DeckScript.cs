using System.Collections.Generic;
using UnityEngine;

public class DeckScript : MonoBehaviour
{
    // CardPrefabs
    public List<GameObject> cardPrefabs = new List<GameObject>();
    public List<Card> deck = new List<Card>();

    void Start()
    {
        GenerateDeck();
        ShuffleDeck();
    }

    public void GenerateDeck()
    {
        foreach (GameObject prefab in cardPrefabs)
        {
            string prefabName = prefab.name; 

            string suit = prefabName.Substring(0, prefabName.Length - 2);

            string numberString = prefabName.Substring(prefabName.Length - 2);
            int number = int.Parse(numberString); 

            string rank = GetRankFromNumber(number);

            int value = GetValueFromRank(rank);

            Card c = ScriptableObject.CreateInstance<Card>();
            c.suit = suit;
            c.rank = rank;
            c.value = value;
            c.cardPrefab = prefab;
            c.CardID = deck.Count;

            deck.Add(c);
        }
    }

    string GetRankFromNumber(int number)
    {
        switch (number)
        {
            case 1: return "Ass";
            case 11: return "Bube";
            case 12: return "Dame";
            case 13: return "König";
            default: return number.ToString(); 
        }
    }

    int GetValueFromRank(string rank) 
    {
        switch (rank)
        {
            case "Ass": return 11; 
            case "Bube":
            case "Dame":
            case "König":
                return 10;
            default:
                return int.Parse(rank); 
        }
    }

    public void ShuffleDeck() //Ändert die Reihenfolge der Objekte in der Liste
    {
        for (int i = 0; i < 52; i++)
        {
            int randomIndex = Random.Range(i, deck.Count);
            Card temp = deck[i];
            deck[i] = deck[randomIndex];
            deck[randomIndex] = temp;
        }
    }
    
                
    public Card DrawCard() //Löscht die erste Karte vom Deck
    {
        Card c = deck[0];
        deck.RemoveAt(0);
        return c;
    }

    public void ResetDeck()
    {
        deck.Clear();
        GenerateDeck();
        ShuffleDeck();
    }
}