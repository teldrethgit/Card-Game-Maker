using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;


public class GameGetRequest : MonoBehaviour
{
    public GameObject GamePrefab;
    public GameObject PlacementTile;
    public GameObject Scroller;
    
    
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
            {   print(game.name);
                game.game = Instantiate(GamePrefab,new Vector3(-1000+index,-150,0), Quaternion.identity);
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
}