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
        string TimeLimit = GameObject.Find("TimeLimitInputSU").GetComponent<TMP_InputField>().text;
        string TotalTurn = GameObject.Find("TotalTurnInputSU").GetComponent<TMP_InputField>().text;
        string Description = GameObject.Find("DescriptionInputSU").GetComponent<TMP_InputField>().text;

        if (GameName == "" || PlayerHealth == null || TimeLimit == null || TotalTurn == null || Description == "") { yield break; }

        List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
        inputForm.Add(new MultipartFormDataSection("name", GameName));
        inputForm.Add(new MultipartFormDataSection("health_pool", PlayerHealth));
        inputForm.Add(new MultipartFormDataSection("time_limit", TimeLimit));
        inputForm.Add(new MultipartFormDataSection("total_turns", TotalTurn));
        inputForm.Add(new MultipartFormDataSection("description", Description));


        UnityWebRequest webRequest = UnityWebRequest.Post("https://osucapstone.herokuapp.com/games", inputForm);
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