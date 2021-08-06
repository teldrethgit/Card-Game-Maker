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
        GamesObject.transform.Find("Canvas").Find("GameTotalHand").Find("TotalHand").GetComponent<TMP_Text>().text = game.total_hand.ToString();
        GamesObject.transform.Find("Canvas").Find("GameHealth").Find("Health").GetComponent<TMP_Text>().text = game.health_pool.ToString();
        GamesObject.transform.Find("Canvas").Find("GameStartingHand").Find("StartingHand").GetComponent<TMP_Text>().text = game.starting_hand.ToString();
        GamesObject.transform.Find("Canvas").Find("GameID").Find("ID").GetComponent<TMP_Text>().text = game.id.ToString();
       
    }
}
