using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;
using System;

public class CreateCardRequest : MonoBehaviour
{
	
	public GameObject createCardFailedText;
	public GameObject createNewCardMenu;
	public GameObject editCardsMenu;
	public GameObject eventSystem;
	
    public void SendCreateRequest()
	{
		StartCoroutine(CreateCard());
	}
	
	IEnumerator CreateCard()
	{
		string CardName = GameObject.Find("NameInput").GetComponent<TMP_InputField>().text;
		int Health = -1;
		if (Int32.TryParse(GameObject.Find("HealthInput").GetComponent<TMP_InputField>().text, out int h))
		{
			if (h >= 0)
			{
				Health = h;
			}
		}
		int Attack = -1;
		if (Int32.TryParse(GameObject.Find("AttackInput").GetComponent<TMP_InputField>().text, out int a))
		{
			if (a >= 0)
			{
				Attack = a;
			}
		}
		int Cost = -1;
		if (Int32.TryParse(GameObject.Find("CostInput").GetComponent<TMP_InputField>().text, out int c))
		{
			if (c >= 0)
			{
				Cost = c;
			}
		}
		
		string imageString = eventSystem.GetComponent<UploadImage>().base64Data;
		if (CardName == "" || Health <= -1 || Attack <= -1 || Cost <= -1 || imageString.Length == 0)
		{
			yield break;
		}
		
		List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
		inputForm.Add(new MultipartFormDataSection("name", CardName));
		inputForm.Add(new MultipartFormDataSection("health", Convert.ToString(Health)));
		inputForm.Add(new MultipartFormDataSection("attack", Convert.ToString(Attack)));
		inputForm.Add(new MultipartFormDataSection("cost", Convert.ToString(Cost)));
		inputForm.Add(new MultipartFormDataSection("game", Convert.ToString(CurrentGame.GetInstance().id)));
		inputForm.Add(new MultipartFormDataSection("image", imageString));
		

		UnityWebRequest webRequest = UnityWebRequest.Post("https://osucapstone.herokuapp.com/cards", inputForm);
        yield return webRequest.SendWebRequest();
		
		if (webRequest.responseCode == 204)
        {
		    createNewCardMenu.SetActive(false);
			editCardsMenu.SetActive(true);
        }
        else 
        {
            createCardFailedText.SetActive(true);
        }
	}
}
