using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Card
{
    public int Googled;
    public string Name;
    public string RuName;
    public string TurName;
    public Sprite image;
}

public class CardListScript : MonoBehaviour
{
    public List<Card> Cards = new List<Card>();
    public List<Card> NewCards = new List<Card>();

    private void Awake()
    {
        for(int i = 0; i < Cards.Count; i++)
        {
            NewCards.Add(Cards[i]);
        }
    }


}
