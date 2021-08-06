using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class GameGetRequest : MonoBehaviour
{
    public GameObject GamePrefab;
    public GameObject GameSlot1;
    public GameObject GameSlot2;
    public GameObject GameSlot3;
    public GameObject GameSlot4;
    public GameObject GameSlot5;
    public GameObject GameSlot6;
    public GameObject GameSlot7;
    public GameObject tempGames;
 
    
    void Start()
    {
        StartCoroutine(getGames());
    }

    string fixJson(string value)
    {
        value = "{\"Result\":" + value + "}";
        return value;
    }

    
    IEnumerator getGames()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get("http://127.0.0.1:8000/games");
        yield return webRequest.SendWebRequest();

        if (webRequest.responseCode == 200)
        {
            Debug.Log(webRequest.responseCode);
            Game[] games = JsonHelper.FromJson<Game>(fixJson(webRequest.downloadHandler.text));
            int index = 0;
            foreach (Game game in games)
<<<<<<< HEAD
<<<<<<< HEAD
<<<<<<< HEAD
            {   
                game.game = Instantiate(GamePrefab,new Vector3(-1000+index,-150,0), Quaternion.identity);
=======
            {
                game.game = Instantiate(GamePrefab, tempGames.GetComponent<Transform>());
>>>>>>> parent of 1019f0b (f)
=======
            {
                game.game = Instantiate(GamePrefab, tempGames.GetComponent<Transform>());
>>>>>>> parent of 1019f0b (f)
=======
            {
                game.game = Instantiate(GamePrefab, tempGames.GetComponent<Transform>());
>>>>>>> parent of 1019f0b (f)
                UpdateGameUI.Update(game);

                switch (index)
                {
                    case 0:
                        game.game.transform.SetParent(GameSlot1.transform, false);
                        break;
                    case 1:
                        game.game.transform.SetParent(GameSlot2.transform, false);
                        break;
                    case 2:
                        game.game.transform.SetParent(GameSlot3.transform, false);
                        break;
                    case 3:
                        game.game.transform.SetParent(GameSlot4.transform, false);
                        break;
                    case 4:
                        game.game.transform.SetParent(GameSlot5.transform, false);
                        break;
                }
                index++;
            }
        }
        else
        {
            Debug.Log("response failed");
        }
    }
}