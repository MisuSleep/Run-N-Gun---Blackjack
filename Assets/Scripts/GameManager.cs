using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // Import Script
    public DeckScript deckscript;
    
    // Buttons, Labels, usw.
    public UnityEngine.UIElements.Button Betbtn;
    public UnityEngine.UIElements.Button Hitbtn;
    public UnityEngine.UIElements.Button Standbtn;
    public UnityEngine.UIElements.Label Cardvalueplayer;
    public UnityEngine.UIElements.Label Cardvaluedealer;
    public UnityEngine.UIElements.Label Balancevalue;
    public UnityEngine.UIElements.Label Betsvalue;
    public DeckScript deck;
    public Transform Playerempty;
    public Transform Dealerempty;
    public Transform cubedealer;
    public Transform cubeplayer;
    
    // Funktionen (Buttons,...)

    public GameObject SpawnCard(Card c, Transform area, int index)
    {
        Debug.Log("SPAWN PREFAB: " + c.cardPrefab.name);
        
        GameObject go = Instantiate(c.cardPrefab);
        
        if (index == 1)
        {

            go.transform.position = area.position;
            go.transform.localScale = new Vector3(400.29f, 400.29f, 400.29f);
            
            
            Debug.Log("SPAWN POSITION: " + go.transform.position);
        }
        
        else
        {

            go.transform.position = area.position + new  Vector3(-38.8f * (index -1), 0, 0);
            go.transform.localScale = new Vector3(400.29f, 400.29f, 400.29f);
            
            
            Debug.Log("SPAWN POSITION: " + go.transform.position);
        }
        
        return go;
    }

    public void CheckPlayerState() // Sagt ob das Spiel schon entschieden ist!
    {

        if (PlayerHandValue > 21 && DealerHandValue <= 21)
        {
            Debug.Log("Ein Satz mit X, dass war wohl nix!!! Du hast verloren!!!");
            
        }
        
        else if (PlayerHandValue == DealerHandValue)
        {
            Debug.Log("Das war sooooo Knapp!!! Aber steckste net drin!!! Gleichstand!!!");
            
        }
        
        else if (PlayerHandValue > 21 && DealerHandValue > 21)
        {
            Debug.Log("Das war sooooo Knapp!!! Aber steckste net drin!!!");
        }
        
        else if (PlayerHandValue >= 21 && DealerHandValue < PlayerHandValue)
        {
            Debug.Log(
                "JJJJJJJJJaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa!!! DU GEWINNST!!!");
        }

        else if (PlayerHandValue >= 21 && DealerHandValue > 21)

        {
            Debug.Log(
                "JJJJJJJJJaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa!!! DU GEWINNST!!!");
        }
        
    }
    
    
    
    public void HitClicked()
    {
        GiveCardToPlayer();
        Debug.Log("Hit");
    }
    
    public void StandClicked()
    {
        
        
        if (DealerHand.Count == 2)
        {
            GameObject SecCard = GiveCardToDealer();
            
            SecCard.transform.rotation = Quaternion.Euler(180, 0, 0);
            GiveCardToDealer();
            CheckPlayerState();
        }
        
        else
        {
            GiveCardToDealer();
            CheckPlayerState();
        }
        Debug.Log("Stand");
        
    }

    public void BetClicked()
    {
        Debug.Log("OnBet");
    }

    //**********************************************************************************************
    
    // Dealer
    public List<Card> DealerHand = new List<Card>();
    public int DealerHandValue = 0;
    
    // Dealer Funktion
    public GameObject GiveCardToDealer()
    {
        
        Card c = deckscript.DrawCard();
        
        DealerHand.Add(c);
        
        GameObject go = SpawnCard(c, cubedealer, DealerHandValue);
        
        DealerHandValue += c.value;
        Cardvaluedealer.text = DealerHandValue.ToString();

        if (DealerHand.Count == 2)
        {
            go.transform.rotation = Quaternion.Euler(180, 0, 0);
        }
        
        return go;
    }

    
    //**********************************************************************************************
    
    // Player
    public List<Card> PlayerHand = new List<Card>();
    public int PlayerHandValue = 0;
    
    // Player Funktion
    public void GiveCardToPlayer()
    {
        
        Card c = deckscript.DrawCard();
        
        PlayerHand.Add(c);

        SpawnCard(c, cubeplayer, PlayerHand.Count);
        
        PlayerHandValue += c.value;
        Cardvalueplayer.text = PlayerHandValue.ToString();
    }

    //**********************************************************************************************
    
    
    // Game
        void Start()
    {
        var root = FindFirstObjectByType<UIDocument>().rootVisualElement;
        
        Hitbtn = root.Q<Button>("Hitbtn");
        Hitbtn.clicked += HitClicked;

        Betbtn = root.Q<Button>("Betbtn");
        Betbtn.clicked += BetClicked;
        
        Standbtn = root.Q<Button>("Standbtn");
        Standbtn.clicked += StandClicked;
        
        Cardvalueplayer = root.Q<Label>("Cardvalueplayer");
        
        Cardvaluedealer = root.Q<Label>("Cardvaluedealer");
        
        
        deck.ShuffleDeck();
        
        GiveCardToPlayer();
        GiveCardToDealer();
        GiveCardToPlayer();
        GiveCardToDealer();
        
    }


    
}


    
    

