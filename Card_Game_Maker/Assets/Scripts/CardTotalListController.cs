using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class CardTotalListController : MonoBehaviour
{
	public GameObject ScrollPanel;
	public GameObject CardListItemPrefab;
	public GameObject LoadSpinner;
 
	ArrayList Cards;
 
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
        UnityWebRequest webRequest = UnityWebRequest.Get("https://osucapstone.herokuapp.com/cards/games/" + CurrentGame.GetInstance().id);
        yield return webRequest.SendWebRequest();

        if (webRequest.responseCode == 200)
        {
            Debug.Log(webRequest.responseCode);
            Card[] cards = JsonHelper.FromJson<Card>(fixJson(webRequest.downloadHandler.text));
			for(int i = 0; i < cards.Length; ++i)
			{
				GameObject newCard = Instantiate(CardListItemPrefab, ScrollPanel.transform) as GameObject;
				CardListController controller = newCard.GetComponent<CardListController>();
				
				newCard.transform.GetChild(0).gameObject.GetComponent<Text>().text = cards[i].name;
				controller.cardListName = cards[i].name;
				controller.cardListId = cards[i].id;
				
				newCard.transform.localScale = Vector3.one;
			}
        }
        else
        {
            Debug.Log("Response failed");
        }
        LoadSpinner.SetActive(false);
    }
}
