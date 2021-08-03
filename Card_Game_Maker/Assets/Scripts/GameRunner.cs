using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class GameRunner : MonoBehaviour
{
    public GameObject CardPrefab;
    public GameObject CardSlot1;
    public GameObject CardSlot2;
    public GameObject CardSlot3;
    public GameObject CardSlot4;
    public GameObject CardSlot5;
    public GameObject CardSlot6;
    public GameObject CardSlot7;
    public GameObject tempCards;

    void Start()
    {
        StartCoroutine(RequestDecks());
    }

    string fixJson(string value)
    {
        value = "{\"Result\":" + value + "}";
        return value;
    }

    IEnumerator RequestDecks()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get("https://osucapstone.herokuapp.com/decks/1");
        yield return webRequest.SendWebRequest();
       
        if (webRequest.responseCode == 200)
        {
            Card[] cards = JsonHelper.FromJson<Card>(fixJson(webRequest.downloadHandler.text));
            int index = 0; 
            foreach (Card card in cards) 
            {
                card.card = Instantiate(CardPrefab, tempCards.GetComponent<Transform>());
                UpdateCardUI.Update(card);

                switch (index) 
                {
                    case 0:
                        card.card.transform.SetParent(CardSlot1.transform, false);
                        break;
                    case 1:
                        card.card.transform.SetParent(CardSlot2.transform, false);
                        break;
                    case 2:
                        card.card.transform.SetParent(CardSlot3.transform, false);
                        break;
                    case 3:
                        card.card.transform.SetParent(CardSlot4.transform, false);
                        break;
                    case 4:
                        card.card.transform.SetParent(CardSlot5.transform, false);
                        break;
                }
                index++;
            }
        }
        else 
        {
            Debug.Log("response failed");
        }
    }

}
