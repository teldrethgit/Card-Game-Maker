using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public void GamesToDecks()
	{
		SceneManager.LoadScene("Decks");
	}
	
	public void DecksToGames()
	{
		SceneManager.LoadScene("Games");
	}
	
	public void DecksToCards()
	{
		SceneManager.LoadScene("Cards");
	}
	
	public void CardsToDecks()
	{
		SceneManager.LoadScene("Decks");
	}
	
	public void GamesToCards()
	{
		SceneManager.LoadScene("Cards");
	}
	
	public void CardsToGames()
	{
		SceneManager.LoadScene("Games");
	}
	
	public void GamesToPlay()
	{
		SceneManager.LoadScene("Play");
	}
	
	public void PlayToGames()
	{
		SceneManager.LoadScene("Games");
	}
}
