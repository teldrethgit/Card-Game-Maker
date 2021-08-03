using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class CardHelpers : MonoBehaviour
{
    static public void UpdateUI(Card card, int player) 
    {
        GameObject CardObject = card.card;
        CardObject.transform.Find("Canvas").Find("Nameplate").Find("name").GetComponent<TMP_Text>().text = card.name;
        CardObject.transform.Find("Canvas").Find("Cost").Find("CostNum").GetComponent<TMP_Text>().text = card.cost.ToString();
        CardObject.transform.Find("Canvas").Find("Attack").Find("AttackNum").GetComponent<TMP_Text>().text = card.attack.ToString();
        CardObject.transform.Find("Canvas").Find("Health").Find("HealthNum").GetComponent<TMP_Text>().text = card.health.ToString();

        CardObject.transform.Find("Canvas").Find("RaycastTarget").tag = "CardRaycast";
        if (player == 0)
        {
            CardObject.tag = "PlayerCard";
        } else 
        {
            CardObject.tag = "EnemyCard";
        }
    }

    static public int[] GetStats(Card card)
    {
        GameObject CardObject = card.card;
        int attack = int.Parse(CardObject.transform.Find("Canvas").Find("Attack").Find("AttackNum").GetComponent<TMP_Text>().text);
        int cost = int.Parse(CardObject.transform.Find("Canvas").Find("Cost").Find("CostNum").GetComponent<TMP_Text>().text);
        int health = int.Parse(CardObject.transform.Find("Canvas").Find("Health").Find("HealthNum").GetComponent<TMP_Text>().text);

        int[] result = {cost, attack, health};
        return result;
    }

    static public Card FindCard(Card[] arr, GameObject card)
    {
        if (card == null) return null;
        foreach (Card c in arr)
        {
            if(c.card == card)
            {
                return c;
            }
        }
        return null;
    }
}
