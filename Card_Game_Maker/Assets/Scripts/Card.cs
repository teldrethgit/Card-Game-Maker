using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Card
{
    public int attack;
    public int cost;
    public int deck;
    public string description;
    public int id;
    public object image;
    public string name;
    public int health = 5;
    public GameObject card;
    public bool inField = false;
    public bool hasAttacked = true;
}
