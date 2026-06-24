using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    // Import Script
    public DeckScript deckscript;
    
    // Buttons, Labels, usw.
    public UnityEngine.UI.Button Betbtn;
    public UnityEngine.UI.Button Hitbtn;
    public UnityEngine.UI.Button Standbtn;
    public UnityEngine.UI.Button Startbtn;
    public UnityEngine.UI.Button Dealbtn;
    public TMPro.TextMeshProUGUI Cardvalueplayer;
    public TMPro.TextMeshProUGUI Cardvaluedealer;
    public TMPro.TextMeshProUGUI Balancevalue;
    public TMPro.TextMeshProUGUI Betsvalue;
    public TMPro.TextMeshProUGUI WinAmountText;
    public TMPro.TextMeshProUGUI LoseAmountText;
    public TMPro.TextMeshProUGUI DrawAmountText;
    public TMPro.TextMeshProUGUI BalanceMenu;

    public DeckScript deck;
    public Transform Playerempty;
    public Transform Dealerempty;
    public Transform cubedealer;
    public Transform cubeplayer;
    public GameObject dealerhiddencard;
    public GameObject WinPanel;
    public GameObject LosePanel;
    public GameObject DrawPanel;
    public GameObject Menu;
    public GameObject GameUI;
    public int roundToGo = 5;
    public TMPro.TextMeshProUGUI Rounds;

    public int Points = 300;
    int balance;
    public float Bonus;
    private int pot = 0;

    
    // Funktionen (Buttons,...)

    public int ConvertPointsToMoney(int points)
    {
        return points /10;
    }


    private IEnumerator DealStartCards()
    {
        GiveCardToPlayer();
        yield return new WaitForSeconds(1f);

        GiveCardToDealer();
        yield return new WaitForSeconds(1f);

        GiveCardToPlayer();
        yield return new WaitForSeconds(1f);

        GiveCardToDealer(true); // verdeckt
    }
    

    IEnumerator ShowResultThenMenu()
    {
        yield return new WaitForSeconds(3f);

        WinPanel.SetActive(false);
        LosePanel.SetActive(false);
        DrawPanel.SetActive(false);
        GameUI.SetActive(false);

        Menu.SetActive(true);
    }

    public void ResetRound()
    {
        PlayerHandValue = 0;
        DealerHandValue = 0;

        PlayerHand.Clear();
        DealerHand.Clear();

        deckscript.ResetDeck();
        
        if (dealerhiddencard != null)
        {
            Destroy(dealerhiddencard);
            dealerhiddencard = null;
        }
        
        foreach (Transform t in cubeplayer)
        {
            Destroy(t.gameObject);
        }
        
        foreach (Transform t in cubedealer)
        {
            Destroy(t.gameObject);
        }
        
        Cardvalueplayer.text = "0";
        Cardvaluedealer.text = "0";
    }

    
    public void ShowWin()
    {
        roundToGo--;
        Rounds.text = roundToGo.ToString();
        
        int winAmount = pot * 2;
        balance += winAmount;
        
        WinAmountText.text = "+" + winAmount.ToString();
        Balancevalue.text = balance.ToString();
        
        pot = 0;
        Betsvalue.text = "0";

        BalanceMenu.text = balance.ToString();
        
        WinPanel.SetActive(true);
        StartCoroutine(ShowResultThenMenu());
    }

    public void ShowLose()
    {
        roundToGo--;
        Rounds.text = roundToGo.ToString();
        
        LoseAmountText.text = "-" + pot.ToString();
        
        pot = 0;
        Betsvalue.text = "0";
        
        BalanceMenu.text = balance.ToString();
        
        LosePanel.SetActive(true);
        StartCoroutine(ShowResultThenMenu());
    }

    public void ShowDraw()
    {
        roundToGo--;
        Rounds.text = roundToGo.ToString();
        
        balance += pot;
        DrawAmountText.text = "+" + pot.ToString();
        Balancevalue.text = balance.ToString();
        
        pot = 0;
        Betsvalue.text = "0";
        
        BalanceMenu.text = balance.ToString();
        
        DrawPanel.SetActive(true);
        StartCoroutine(ShowResultThenMenu());
    }

    private int FixAce(int total, List<Card> hand)
    {
        foreach (Card c in hand)
        {
            if (c.rank == "Ass" && total > 21)
            {
                total -= 10; 
            }
        }

        return total;
    }

    
    public GameObject SpawnCard(Card c, Transform area, int index)
    {
        Debug.Log("SPAWN PREFAB: " + c.cardPrefab.name);
        
        GameObject go = Instantiate(c.cardPrefab, area);
        
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

    public void CheckPlayerState()
    {
        if (PlayerHandValue > 21)
        {
            Debug.Log("Ein Satz mit X, dass war wohl nix!!! Du hast verloren!!!");
            ShowLose();
            return;
        }
        
        if (DealerHandValue > 21)
        {
            Debug.Log("JJJJJJJJJaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa!!! DU GEWINNST!!!");
            ShowWin();
            return;
        }
        
        if (PlayerHandValue > DealerHandValue)
        {
            Debug.Log("JJJJJJJJJaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa!!! DU GEWINNST!!!");
            ShowWin();
            return;
        }
        
        if (DealerHandValue > PlayerHandValue)
        {
            Debug.Log("Ein Satz mit X, dass war wohl nix!!! Du hast verloren!!!");
            ShowLose();
            return;
        }
        
        Debug.Log("Das war sooooo Knapp!!! Aber steckste net drin!!! Gleichstand!!!");
        ShowDraw();
        
    }
    
    // Buttons usw.
    
    public void HitClicked()
    {
        GiveCardToPlayer();
        Debug.Log("Hit");
    }
    
    public void StandClicked()
    {
        
        
        if (dealerhiddencard != null)
        {
            dealerhiddencard.transform.rotation = Quaternion.Euler(0, 0, 0);
            Cardvaluedealer.text = DealerHandValue.ToString();

            dealerhiddencard = null;

        }

        StartCoroutine(DealerPlay());
        
        Debug.Log("Stand");
        
    }

    public void BetClicked()
    {
        if (balance <= 0)
            return;

        balance -= 1;
        pot += 1;

        Balancevalue.text = balance.ToString();
        Betsvalue.text = pot.ToString();
    }
    
    public void StartClicked()
    {
        Menu.SetActive(false);
        GameUI.SetActive(true);

        ResetRound();

        pot = 0;
        Betsvalue.text = "0";

        Betbtn.interactable = true;   
        Dealbtn.interactable = true;  

    }

    public void DealClicked()
    {
        Betbtn.interactable = false; 
        StartCoroutine(DealStartCards());
    }
    
    //**********************************************************************************************
    
    // Dealer
    public List<Card> DealerHand = new List<Card>();
    public int DealerHandValue = 0;
    
    // Dealer Funktion
    public GameObject GiveCardToDealer(bool facedown = false)
    {
        
        Card c = deckscript.DrawCard();
        
        DealerHand.Add(c);
        
        GameObject go = SpawnCard(c, cubedealer, DealerHand.Count);
        
        DealerHandValue += c.value;
        DealerHandValue = FixAce(DealerHandValue, DealerHand);

        if (facedown)
        {
            dealerhiddencard = go;
            go.transform.rotation = Quaternion.Euler(180, 0, 0);
        }

        else
        {
            Cardvaluedealer.text = DealerHandValue.ToString();
        }
        
        
        return go;
    }

    private IEnumerator DealerPlay()
    {
        // Dealer zieht solange er unter 17 ist
        while (DealerHandValue < 17)
        {
            yield return new WaitForSeconds(1f); // 1 Sekunde warten
            GiveCardToDealer();                  // Karte ziehen
        }

        yield return new WaitForSeconds(1f);     // kleine Pause
        CheckPlayerState();                      // Gewinner bestimmen
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
        PlayerHandValue = FixAce(PlayerHandValue, PlayerHand);
        Cardvalueplayer.text = PlayerHandValue.ToString();
    }

    //**********************************************************************************************
    
    
    // Game
        void Start()
    {
        Startbtn.onClick.AddListener(StartClicked);
        Hitbtn.onClick.AddListener(HitClicked);
        Betbtn.onClick.AddListener(BetClicked);
        Standbtn.onClick.AddListener(StandClicked);
        Dealbtn.onClick.AddListener(DealClicked);

        balance = ConvertPointsToMoney(Points);
        Balancevalue.text = balance.ToString();
        BalanceMenu.text = balance.ToString();
        
        Cardvalueplayer.text = "0";
        Cardvaluedealer.text = "0";
        
        Rounds.text = roundToGo.ToString();
        
        deck.ShuffleDeck();
        
        Menu.SetActive(true);

    }


    
}


    
    

