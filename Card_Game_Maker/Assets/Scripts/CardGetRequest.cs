using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CardGetRequest : MonoBehaviour
{
    public GameObject CardPrefab;
    public GameObject PlacementTile;
    public GameObject Scroller;
    
    
    void Start()
    {
        StartCoroutine(getCards());
    }

    string fixJson(string value)
    {
        value = "{\"Result\":" + value + "}";
        return value;
    }

    
    IEnumerator getCards()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get("https://osucapstone.herokuapp.com/cards");
        yield return webRequest.SendWebRequest();

        if (webRequest.responseCode == 200)
        {
            Debug.Log(webRequest.responseCode);
            Card[] cards = JsonHelper.FromJson<Card>(fixJson(webRequest.downloadHandler.text));
            int index = 0;

            foreach (Card card in cards)
            {   print(card.name);
                card.card = Instantiate(CardPrefab,new Vector3(-1000+index,-150,0), Quaternion.identity);
                UpdateCardUI.Update(card);
                card.card.transform.SetParent(Scroller.transform, false);
                index += 750;
            }
        }
        else
        {
            Debug.Log("response failed");
        }
    }
}
