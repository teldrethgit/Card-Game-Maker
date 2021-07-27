using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class CardPostRequests : MonoBehaviour
{
    public GameObject cardFailText;
    public GameObject cardSuccessText;
    public GameObject EditCardsMenu;
   

    public void SendPostRequest()
    {

        StartCoroutine(cardPostRequest());
    }

    IEnumerator cardPostRequest()
    {
     
        string CardName = GameObject.Find("NameInput").GetComponent<TMP_InputField>().text;
        string Health = GameObject.Find("HealthInput").GetComponent<TMP_InputField>().text;
        string Attack = GameObject.Find("AttackInput").GetComponent<TMP_InputField>().text;
        string Cost = GameObject.Find("CostInput").GetComponent<TMP_InputField>().text;
        string Description = GameObject.Find("DescriptionInput").GetComponent<TMP_InputField>().text;
        //string Image = GameObject.Find("UploadImageButton").GetComponent<TMP_InputField>().text;
        string Deck = GameObject.Find("DeckInput").GetComponent<TMP_InputField>().text;

        if (CardName == "" || Health == null || Attack == null || Cost == null) { yield break; }

        List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
        inputForm.Add(new MultipartFormDataSection("name", CardName));
        inputForm.Add(new MultipartFormDataSection("health", Health));
        inputForm.Add(new MultipartFormDataSection("attack", Attack));
        inputForm.Add(new MultipartFormDataSection("cost", Cost));
        inputForm.Add(new MultipartFormDataSection("description", Description));
        //inputForm.Add(new MultipartFormDataSection("image", null));
        inputForm.Add(new MultipartFormDataSection("deck", Deck));


        UnityWebRequest webRequest = UnityWebRequest.Post("https://osucapstone.herokuapp.com/cards", inputForm);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.responseCode);

        if (webRequest.responseCode == 204)
        {   
            //EditCardsMenu.SetActive(true);
            cardSuccessText.SetActive(true);
            cardFailText.SetActive(false);
            //title.GetComponent<TMP_Text>().text = "Choose a Game";
        }
        else
        {
            cardFailText.SetActive(true);
        }
    }
}


/*

public void SendSignUp()
{
    StartCoroutine(SignUp());
}

IEnumerator SignUp()
{
    string UserName = GameObject.Find("UserNameInputSU").GetComponent<TMP_InputField>().text;
    string Password = GameObject.Find("PasswordInputSU").GetComponent<TMP_InputField>().text;
    if (UserName == "" || Password == "") { yield break; }

    List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
    inputForm.Add(new MultipartFormDataSection("name", UserName));
    inputForm.Add(new MultipartFormDataSection("password", Password));


    UnityWebRequest webRequest = UnityWebRequest.Post("https://osucapstone.herokuapp.com/signup", inputForm);
    yield return webRequest.SendWebRequest();

    if (webRequest.responseCode == 201)
    {
        signUp.SetActive(false);
        chooseGameMenu.SetActive(true);
        logOutButton.SetActive(true);
        title.GetComponent<TMP_Text>().text = "Choose a Game";
    }
    else
    {
        signUpFailedText.SetActive(true);
    }
}
}
*/