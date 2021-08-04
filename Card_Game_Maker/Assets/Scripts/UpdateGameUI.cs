using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UpdateGameUI : MonoBehaviour
{
    static public void Update(Game game)
    {
        GameObject GamesObject = game.game;
        GamesObject.transform.Find("Canvas").Find("GameName").Find("Name").GetComponent<TMP_Text>().text = game.name;
        GamesObject.transform.Find("Canvas").Find("GameDescription").Find("Description").GetComponent<TMP_Text>().text = game.description;
        GamesObject.transform.Find("Canvas").Find("GameTotalTurns").Find("TotalTurns").GetComponent<TMP_Text>().text = game.total_turns.ToString();
        GamesObject.transform.Find("Canvas").Find("GameHealth").Find("Health").GetComponent<TMP_Text>().text = game.health_pool.ToString();
        GamesObject.transform.Find("Canvas").Find("GameTimeLimit").Find("TimeLimit").GetComponent<TMP_Text>().text = game.time_limit.ToString();
       
    }
}
