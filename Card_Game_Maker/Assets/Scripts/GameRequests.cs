using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;


public class GameRequests : MonoBehaviour
{

    //create
    public GameObject gameFail;
    public GameObject GameSuccess;
    public GameObject ChooseGameMenu;
    public GameObject CreateGameMenu;

    //update
    public GameObject GameID;
    public GameObject GamePublish;
    public GameObject EditGameMenu;
    public GameObject EditGameRulesMenu;
  
    //get
    public GameObject GamePrefab;
    public GameObject UserGamePrefab;
    public GameObject Scroller;
    public GameObject ScrollerUser;

    void Start()
    {
        StartCoroutine(getGames());
        StartCoroutine(getUserGames());
    }

    string fixJson(string value)
    {
        value = "{\"Result\":" + value + "}";
        return value;
    }

    
    IEnumerator getGames()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get("https://osucapstone.herokuapp.com/games");
        yield return webRequest.SendWebRequest();

        if (webRequest.responseCode == 200)
        {
            Debug.Log(webRequest.responseCode);
            Game[] games = JsonHelper.FromJson<Game>(fixJson(webRequest.downloadHandler.text));
            int index = 0;

            foreach (Game game in games)
            { 
                game.game = Instantiate(GamePrefab,new Vector3(-1200+index,-150,0), Quaternion.identity);
                UpdateGameUI.Update(game);
                game.game.transform.SetParent(Scroller.transform, false);
                index += 750;
            }
        }
        else
        {
            Debug.Log("response failed");
        }
    }

    IEnumerator getUserGames()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get("https://osucapstone.herokuapp.com/games/user");
        yield return webRequest.SendWebRequest();

        if (webRequest.responseCode == 200)
        {
            Debug.Log(webRequest.responseCode);
            Game[] games = JsonHelper.FromJson<Game>(fixJson(webRequest.downloadHandler.text));
            int index = 0;

            foreach (Game game in games)
            { 
                game.game = Instantiate(UserGamePrefab,new Vector3(-1000+index,-150,0), Quaternion.identity);
                UpdateGameUI.Update(game);
                game.game.transform.SetParent(ScrollerUser.transform, false);
                index += 750;
            }
        }
        else
        {
            Debug.Log("response failed");
        }
    }




    public void CreateGame()
    {

        StartCoroutine(gameCreate());
    }

    IEnumerator gameCreate()
    {
        
        
        string PublishToggle;
        string GameName = GameObject.Find("GameNameInputSU").GetComponent<TMP_InputField>().text;
        string PlayerHealth = GameObject.Find("PlayerHealthInputSU").GetComponent<TMP_InputField>().text;
        string TotalHand = GameObject.Find("TotalHandSizeInputSU").GetComponent<TMP_InputField>().text;
        string StartingHand = GameObject.Find("StartingHandSizeInputSU").GetComponent<TMP_InputField>().text;
        string Description = GameObject.Find("DescriptionInputSU").GetComponent<TMP_InputField>().text;

        if (GameName == "" || PlayerHealth == null || TotalHand == null || StartingHand == null || Description == "") { yield break; }
        if (GameObject.Find("GamePublishToggle").GetComponent<Toggle>().isOn == true){
            PublishToggle = "true";
        } else{
            PublishToggle = "false";
        }
        List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
        inputForm.Add(new MultipartFormDataSection("name", GameName));
        inputForm.Add(new MultipartFormDataSection("health_pool", PlayerHealth));
        inputForm.Add(new MultipartFormDataSection("total_hand", TotalHand));
        inputForm.Add(new MultipartFormDataSection("starting_hand", StartingHand));
        inputForm.Add(new MultipartFormDataSection("description", Description));
        inputForm.Add(new MultipartFormDataSection("published", PublishToggle));


        UnityWebRequest webRequest = UnityWebRequest.Post("https://osucapstone.herokuapp.com/games", inputForm);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.responseCode);

        if (webRequest.responseCode == 204)
        {
            GameSuccess.SetActive(true);
            SceneManager.LoadScene("Games");
        }
        else
        {
            gameFail.SetActive(true);
            CreateGameMenu.SetActive(true);
        }
    }


 public void SendUpdate()
    {

        StartCoroutine(gamePut());
    }

    IEnumerator gamePut()
    {
        string PublishToggle;
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
        if (GameObject.Find("GamePublishToggle").GetComponent<Toggle>().isOn == true){
            PublishToggle = "true";
        } else{
            PublishToggle = "false";
        }
        List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
        inputForm.Add(new MultipartFormDataSection("name", GameName));
        inputForm.Add(new MultipartFormDataSection("health_pool", PlayerHealth));
        inputForm.Add(new MultipartFormDataSection("total_hand", TotalHandSize));
        inputForm.Add(new MultipartFormDataSection("starting_hand", StartingHandSize));
        inputForm.Add(new MultipartFormDataSection("description", Description));
        inputForm.Add(new MultipartFormDataSection("published", PublishToggle));


        UnityWebRequest webRequest = UnityWebRequest.Post($"https://osucapstone.herokuapp.com/games/{gameID}", inputForm);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.responseCode);

        if (webRequest.responseCode == 204)
        {
            GameSuccess.SetActive(true);
            SceneManager.LoadScene("Games");
            
        }
        else
        {
            
            EditGameRulesMenu.SetActive(true);
        }
    }

public void DeleteGame()
    {
        StartCoroutine(SendDelete());
    }

    IEnumerator SendDelete()
    {   string gameID = GameID.GetComponent<TMPro.TextMeshProUGUI>().text;
        UnityWebRequest webRequest = UnityWebRequest.Delete($"https://osucapstone.herokuapp.com/games/{gameID}");
        yield return webRequest.SendWebRequest();
       
        if (webRequest.responseCode == 204)
        {
            SceneManager.LoadScene("Games");
        }
        else 
        {
		    Debug.Log("Request Failed");
        }
    }

public void PublishGame()
    {

        StartCoroutine(gamePublish());
    }

    IEnumerator gamePublish()
    {
       
        string gameID = GameID.GetComponent<TMPro.TextMeshProUGUI>().text;
        string gamePublish = GamePublish.GetComponent<TMPro.TextMeshProUGUI>().text;

        List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
        
        if (gamePublish == "True"){ 
            inputForm.Add(new MultipartFormDataSection("published", "false"));
        } else {
            inputForm.Add(new MultipartFormDataSection("published", "true"));
        }

        UnityWebRequest webRequest = UnityWebRequest.Post($"https://osucapstone.herokuapp.com/games/{gameID}/publish", inputForm);
        yield return webRequest.SendWebRequest();
        Debug.Log(webRequest.responseCode);

        if (webRequest.responseCode == 204)
        {
            GameSuccess.SetActive(true);
            SceneManager.LoadScene("Games");
        }
        else
        {   
            EditGameRulesMenu.SetActive(true);
        }
    }

}


