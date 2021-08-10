using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
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

    static public void UpdateImage(Card card) 
    {
        Image imageArea = card.card.transform.Find("Canvas").Find("Monster").GetComponent<Image>();
        if (card.image != "")
        {
            Texture2D tex = new Texture2D(1, 1);
            string b64 = card.image.Substring(12);
            b64 = b64.Remove(b64.Length - 2, 2);
            Debug.Log(b64);
            byte[] pngData = System.Convert.FromBase64String(b64);
            tex.LoadImage(pngData);
            Sprite spr = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            imageArea.sprite = spr;
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
