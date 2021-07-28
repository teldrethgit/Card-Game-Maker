using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class DecksRequests : MonoBehaviour
{
    public GameObject cardFailText;
    public GameObject cardSuccessText;
    public GameObject EditCardsMenu;
    public GameObject ChoosingDeckCardsMenu;

    public void SendPostRequest()
    {

        StartCoroutine(DeckPostRequest());
    }

    IEnumerator DeckPostRequest()
    {

        string DeckName = GameObject.Find("DeckNameCreate").GetComponent<TMP_InputField>().text;
        
        

        if (DeckName == "") { yield break; }

        List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
        inputForm.Add(new MultipartFormDataSection("name", DeckName));
        

        UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1:8000/decks", inputForm);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.responseCode);

        if (webRequest.responseCode == 204)
        {
            ChoosingDeckCardsMenu.SetActive(true);
            //title.GetComponent<TMP_Text>().text = "Choose a Game";
        }
        else
        {
            cardFailText.SetActive(true);
        }
    }
}


//http://127.0.0.1:8000/