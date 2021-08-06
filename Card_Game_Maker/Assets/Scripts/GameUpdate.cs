using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class GameUpdate : MonoBehaviour
{
    public GameObject GameID;
    public GameObject EditGameMenu;
    public GameObject EditGameRulesMenu;
  

    public void SendPut()
    {

        StartCoroutine(gamePut());
    }

    IEnumerator gamePut()
    {
        string gameID = GameID.GetComponent<TMPro.TextMeshProUGUI>().text;
        string GameName = GameObject.Find("GameNameInputSU").GetComponent<TMP_InputField>().text;
        string PlayerHealth= GameObject.Find("PlayerHealthInputSU").GetComponent<TMP_InputField>().text;
        string TotalHandSize = GameObject.Find("TotalHandSizeInputSU").GetComponent<TMP_InputField>().text;
        string StartingHandSize = GameObject.Find("StartingHandSizeInputSU").GetComponent<TMP_InputField>().text;
        string Description = GameObject.Find("DescriptionInputSU").GetComponent<TMP_InputField>().text;

        if (GameName == "" || PlayerHealth == null || TotalHandSize == null || StartingHandSize == null || Description == "") { yield break; }

        if (Int32.Parse(TotalHandSize) < Int32.Parse(StartingHandSize)) {
            yield break;
        }

        List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
        inputForm.Add(new MultipartFormDataSection("name", GameName));
        inputForm.Add(new MultipartFormDataSection("health_pool", PlayerHealth));
        inputForm.Add(new MultipartFormDataSection("total_hand", TotalHandSize));
        inputForm.Add(new MultipartFormDataSection("starting_hand", StartingHandSize));
        inputForm.Add(new MultipartFormDataSection("description", Description));

        print($"http://127.0.0.1:8000/{gameID}");
        UnityWebRequest webRequest = UnityWebRequest.Post($"http://127.0.0.1:8000/games/{gameID}", inputForm);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.responseCode);

        if (webRequest.responseCode == 204)
        {
            //GameCreateSuccess.SetActive(true);
            EditGameMenu.SetActive(true);
            EditGameRulesMenu.SetActive(false);
        }
        else
        {
            //gameCreateFail.SetActive(true);
            EditGameRulesMenu.SetActive(true);
        }
    }

}