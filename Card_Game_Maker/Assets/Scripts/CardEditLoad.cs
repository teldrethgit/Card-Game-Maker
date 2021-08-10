using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using TMPro;

public class CardEditLoad : MonoBehaviour
{
    public int cardId;
	public GameObject editMenu;
	public GameObject chooseMenu;
	public Transform listEntry;
	public GameObject editCardFailedText;
	public GameObject eventSystem;
	
	
	public void FetchCardInfo()
	{
		StartCoroutine(CardInfo());
	}
	
	public void SubmitCardInfo()
	{
		StartCoroutine(SubmitEditCard());
	}
	
	string fixJson(string value)
    {
        value = "{\"Result\":" + value + "}";
        return value;
    }
	
	IEnumerator CardInfo()
	{
		//find inactive menu object
		UnityEngine.Object[] allObjs = Resources.FindObjectsOfTypeAll(typeof(GameObject));
		foreach(GameObject obj in allObjs)
		{
			if (obj.name == "EditSelectedCardMenu")
			{
				editMenu = obj;
				break;
			}
		}
		chooseMenu = GameObject.Find("EditCardsMenu");
		listEntry = gameObject.transform.parent;
		int cardId = listEntry.GetComponent<CardListController>().cardListId;
		UnityWebRequest webRequest = UnityWebRequest.Get("https://osucapstone.herokuapp.com/cards/" + Convert.ToString(cardId));
        yield return webRequest.SendWebRequest();
		
		if (webRequest.responseCode == 200)
        {
            Debug.Log(cardId);
			chooseMenu.SetActive(false);
			editMenu.SetActive(true);
			
			Card cardInfo = JsonUtility.FromJson<Card>(webRequest.downloadHandler.text);
			TextMeshProUGUI textArea = GameObject.Find("EditNameInput").transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
			textArea.text = Convert.ToString(cardInfo.name);
			textArea = GameObject.Find("EditHealthInput").transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
			textArea.text = Convert.ToString(cardInfo.health);
			textArea = GameObject.Find("EditAttackInput").transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
			textArea.text = Convert.ToString(cardInfo.attack);
			textArea = GameObject.Find("EditCostInput").transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>();
			textArea.text = Convert.ToString(cardInfo.cost);
			
			Image imageArea = GameObject.Find("EditImageButton").GetComponent<Image>();
			if (cardInfo.image != "")
			{
				Texture2D tex = new Texture2D(1, 1);
				string b64 = cardInfo.image.Substring(12);
				b64 = b64.Remove(b64.Length - 1, 1);
				byte[] pngData = System.Convert.FromBase64String(cardInfo.image);
				tex.LoadImage(pngData);
				Sprite spr = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
				imageArea.sprite = spr;
			}
			else
			{
				imageArea.sprite = null;
			}
        }
        else
        {
            Debug.Log("Response failed");
        }
	}
	
	IEnumerator SubmitEditCard()
	{
		string CardName = GameObject.Find("EditNameInput").GetComponent<TMP_InputField>().text;
		int Health = -1;
		if (Int32.TryParse(GameObject.Find("EditHealthInput").GetComponent<TMP_InputField>().text, out int h))
		{
			if (h >= 0)
			{
				Health = h;
			}
		}
		int Attack = -1;
		if (Int32.TryParse(GameObject.Find("EditAttackInput").GetComponent<TMP_InputField>().text, out int a))
		{
			if (a >= 0)
			{
				Attack = a;
			}
		}
		int Cost = -1;
		if (Int32.TryParse(GameObject.Find("EditCostInput").GetComponent<TMP_InputField>().text, out int c))
		{
			if (c >= 0)
			{
				Cost = c;
			}
		}
		
		string imageString = eventSystem.GetComponent<UploadImage>().base64Data;
		if (CardName == "" || Health <= -1 || Attack <= -1 || Cost <= -1 || imageString.Length == 0)
		{
			editCardFailedText.SetActive(true);
			yield break;
		}
		
		List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
		inputForm.Add(new MultipartFormDataSection("name", CardName));
        inputForm.Add(new MultipartFormDataSection("health", Convert.ToString(Health)));
        inputForm.Add(new MultipartFormDataSection("attack", Convert.ToString(Attack)));
		inputForm.Add(new MultipartFormDataSection("cost", Convert.ToString(Cost)));
		inputForm.Add(new MultipartFormDataSection("image", imageString));
		
		
		UnityWebRequest webRequest = UnityWebRequest.Post("https://osucapstone.herokuapp.com/cards/edit/" + Convert.ToString(cardId), inputForm);
        yield return webRequest.SendWebRequest();
		
		if (webRequest.responseCode == 204)
        {
		    chooseMenu.SetActive(true);
			editMenu.SetActive(false);
        }
        else 
        {
            editCardFailedText.SetActive(true);
        }
	}
}
