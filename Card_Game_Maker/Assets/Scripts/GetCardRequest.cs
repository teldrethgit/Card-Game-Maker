using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GetCardRequest : MonoBehaviour
{
    public void GetImage()
	{
		StartCoroutine(PullDownImage());
	}
	
	string fixJson(string value)
    {
        value = "{\"Result\":" + value + "}";
        return value;
    }
	
	IEnumerator PullDownImage()
	{
		UnityWebRequest webRequest = UnityWebRequest.Get("https://osucapstone.herokuapp.com/cards");
        yield return webRequest.SendWebRequest();

        if (webRequest.responseCode == 200)
        {
            Debug.Log(webRequest.downloadHandler.text);
            Card[] cards = JsonHelper.FromJson<Card>(fixJson(webRequest.downloadHandler.text));

            foreach (Card card in cards)
            {   
				Debug.Log(card.name);
            }
        }
        else
        {
            Debug.Log("response failed");
        }
	}
}
