using UnityEngine;
[CreateAssetMenu(fileName = "NewCard", menuName = "Card")]
public class Card : ScriptableObject
{
    
    public int CardID;
    public string rank; // Kartenwert 9,10,Bube,Dame,...
    public string suit; // Kartenfarbe Spates, Diamonds,...
    public int value; // Rang welche höher ist 1,2,3,...

    public GameObject cardPrefab;
    
    




}
