using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; 

public class UpdateCardUI : MonoBehaviour
{
    static public void Update(Card card) 
    {
        GameObject CardObject = card.card;
        CardObject.transform.Find("Canvas").Find("Nameplate").Find("name").GetComponent<TMP_Text>().text = card.name;
        CardObject.transform.Find("Canvas").Find("Cost").Find("CostNum").GetComponent<TMP_Text>().text = card.cost.ToString();
        CardObject.transform.Find("Canvas").Find("Attack").Find("AttackNum").GetComponent<TMP_Text>().text = card.attack.ToString();
        // still need to add health to the api/db
        //CardObject.transform.Find("Canvas").Find("Health").Find("HealthNum").GetComponent<TMP_Text>().text = card.health.ToString();
    }
}
