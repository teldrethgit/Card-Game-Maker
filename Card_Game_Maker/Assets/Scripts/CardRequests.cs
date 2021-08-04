using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class CardRequests : MonoBehaviour
{
    public GameObject cardFailText;
    public GameObject cardSuccessText;
    public GameObject EditCardsMenu;
    public GameObject CreateNewCardMenu;

    public void SendPost()
    {

        StartCoroutine(cardPost());
    }

    IEnumerator cardPost()
    {
     
        string CardName = GameObject.Find("NameInputCreate").GetComponent<TMP_InputField>().text;
        string Health = GameObject.Find("HealthInputCreate").GetComponent<TMP_InputField>().text;
        string Attack = GameObject.Find("AttackInputCreate").GetComponent<TMP_InputField>().text;
        string Cost = GameObject.Find("CostInputCreate").GetComponent<TMP_InputField>().text;
        string Description = GameObject.Find("DescriptionInputCreate").GetComponent<TMP_InputField>().text;
        //string Image = GameObject.Find("UploadImageButton").GetComponent<TMP_InputField>().text;
        string Deck = GameObject.Find("DeckInputCreate").GetComponent<TMP_InputField>().text;

        if (CardName == "" || Health == null || Attack == null || Cost == null) { yield break; }
        
        List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
        inputForm.Add(new MultipartFormDataSection("name", CardName));
        inputForm.Add(new MultipartFormDataSection("health", Health));
        inputForm.Add(new MultipartFormDataSection("attack", Attack));
        inputForm.Add(new MultipartFormDataSection("cost", Cost));
        inputForm.Add(new MultipartFormDataSection("description", Description));
        if (Deck == "") {
            {   Deck = "Null";
                inputForm.Add(new MultipartFormDataSection("deck", Deck));
            };
        }
        else
        {   
            inputForm.Add(new MultipartFormDataSection("deck", Deck));
        }
        //inputForm.Add(new MultipartFormDataSection("image", null));
        

        UnityWebRequest webRequest = UnityWebRequest.Post("https://osucapstone.herokuapp.com/cards", inputForm);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.responseCode);

        if (webRequest.responseCode == 204)
        {   
            EditCardsMenu.SetActive(true);
            cardSuccessText.SetActive(true);
            cardFailText.SetActive(false);
            CreateNewCardMenu.SetActive(false);
           
        }
        else
        {
            cardFailText.SetActive(true);
        }
    }
}

