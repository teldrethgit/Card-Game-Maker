using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class GameRequests : MonoBehaviour
{
    public GameObject gameCreateFail;
    public GameObject GameCreateSuccess;
    public GameObject ChooseGameMenu;
    public GameObject CreateGameMenu;
  


    public void SendPost()
    {

        StartCoroutine(gamePost());
    }

    IEnumerator gamePost()
    {

        string GameName = GameObject.Find("GameNameInputSU").GetComponent<TMP_InputField>().text;
        string PlayerHealth = GameObject.Find("PlayerHealthInputSU").GetComponent<TMP_InputField>().text;
        string TotalHand = GameObject.Find("TotalHandSizeInputSU").GetComponent<TMP_InputField>().text;
        string StartingHand = GameObject.Find("StartingHandSizeInputSU").GetComponent<TMP_InputField>().text;
        string Description = GameObject.Find("DescriptionInputSU").GetComponent<TMP_InputField>().text;

        if (GameName == "" || PlayerHealth == null || TotalHand == null || StartingHand == null || Description == "") { yield break; }

        List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
        inputForm.Add(new MultipartFormDataSection("name", GameName));
        inputForm.Add(new MultipartFormDataSection("health_pool", PlayerHealth));
        inputForm.Add(new MultipartFormDataSection("total_hand", TotalHand));
        inputForm.Add(new MultipartFormDataSection("starting_hand", StartingHand));
        inputForm.Add(new MultipartFormDataSection("description", Description));


        UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1:8000/games", inputForm);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.responseCode);

        if (webRequest.responseCode == 204)
        {
            GameCreateSuccess.SetActive(true);
            ChooseGameMenu.SetActive(true);
            CreateGameMenu.SetActive(false);
        }
        else
        {
            gameCreateFail.SetActive(true);
            CreateGameMenu.SetActive(true);
        }
    }

}