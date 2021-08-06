using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;


public class DecksRequests : MonoBehaviour
{
    public Transform DeckStartLoc;
    public RectTransform DeckFieldOfView;
    public GameObject DeckPrefab;
    public GameObject NameInput;
    public GameObject DescriptionInput;
    public GameObject CreateButton;
    public GameObject CreateFailText;
    public GameObject FailText;
    public GameObject EditMenu;
    public GameObject DeckMenu;
    public GameObject EditFailText;
    public GameObject EditButton;
    public TMP_InputField EditName;
    public TMP_InputField EditDescription;
    public TMP_Text Title;
    
    private string cookie;
    private Deck[] decks;
    private Deck editing;

    void Start()
    {
        StartCoroutine(GetDecks());
    }

    IEnumerator GetDecks()
    {
        UnityWebRequest webRequest = UnityWebRequest.Get("https://osucapstone.herokuapp.com/decks");
        yield return webRequest.SendWebRequest();

        if (webRequest.responseCode == 200)
        {
            Vector3 pos = DeckStartLoc.position;
            Vector3 viewPos = DeckFieldOfView.sizeDelta;
            GameObject currentDeck;
            decks = JsonHelper.FromJson<Deck>(fixJson(webRequest.downloadHandler.text));
            int i = 0;
            foreach (Deck d in decks)
            {
                currentDeck = Instantiate(DeckPrefab, new Vector3(pos.x + (i * 750), pos.y, pos.z), Quaternion.identity, DeckStartLoc);
                UpdateDeckUI(currentDeck, d);
                Button button = currentDeck.transform.Find("Canvas").Find("Button").GetComponent<Button>();
                button.onClick.AddListener(() => SetEditing(d.id));
                i++;
            }
            viewPos.x = 1920 + (750 * (decks.Length - 2));
            DeckFieldOfView.sizeDelta = viewPos;
            DeckFieldOfView.position = new Vector3(viewPos.x, DeckFieldOfView.position.y, DeckFieldOfView.position.z);
            FailText.GetComponent<TMP_Text>().text = "Success";
        }
        else
        {
            FailText.GetComponent<TMP_Text>().text = "Fail";
        }
    }

    string fixJson(string value)
    {
        value = "{\"Result\":" + value + "}";
        return value;
    }

    public void CreateDeck()
    {
        StartCoroutine(CreateDeckPost());
    }

    IEnumerator CreateDeckPost()
    {
        string Name = NameInput.GetComponent<TMP_Text>().text;
        string Description = DescriptionInput.GetComponent<TMP_Text>().text;
        if(Name == "") {yield break;}

        List<IMultipartFormSection> inputForm = new List<IMultipartFormSection>();
        inputForm.Add(new MultipartFormDataSection("name", Name));
        inputForm.Add(new MultipartFormDataSection("description", Description));
        
        UnityWebRequest webRequest = UnityWebRequest.Post("https://osucapstone.herokuapp.com/decks", inputForm);
        yield return webRequest.SendWebRequest();
       
        if (webRequest.responseCode == 204)
        {
		    Debug.Log("Saved");
            SceneManager.LoadScene("Decks");
        }
        else 
        {
		    Debug.Log("Request Failed");
            CreateFailText.SetActive(true);
            CreateButton.GetComponent<Button>().interactable = true;
        }
    }

    public void UpdateDeck()
    {
        StartCoroutine(SendUpdate());
    }

    IEnumerator SendUpdate()
    {
        string Name = EditName.text;
        string Description = EditDescription.text;
        if(Name == "") {yield break;}

        string body = "?name=" + Name + "&description=" + Description;
        
        UnityWebRequest webRequest = UnityWebRequest.Put("https://osucapstone.herokuapp.com/decks/" + editing.id + body, "dummy");
        yield return webRequest.SendWebRequest();
       
        if (webRequest.responseCode == 204)
        {
		    Debug.Log("Saved");
            SceneManager.LoadScene("Decks");
        }
        else 
        {
		    Debug.Log("Request Failed");
            EditFailText.SetActive(true);
            EditButton.GetComponent<Button>().interactable = true;
        }
    }

    public void DeleteDeck()
    {
        StartCoroutine(SendDelete());
    }

    IEnumerator SendDelete()
    {
        UnityWebRequest webRequest = UnityWebRequest.Delete("https://osucapstone.herokuapp.com/decks/" + editing.id);
        yield return webRequest.SendWebRequest();
       
        if (webRequest.responseCode == 204)
        {
		    Debug.Log("Saved");
            SceneManager.LoadScene("Decks");
        }
        else 
        {
		    Debug.Log("Request Failed");
            EditFailText.SetActive(true);
            EditButton.GetComponent<Button>().interactable = true;
        }
    }

    public void SetEditing(int id)
    {
        foreach (Deck d in decks)
        {
            if(d.id == id)
            {
                editing = d;
                EditMenu.SetActive(true);
                DeckMenu.SetActive(false);
                EditName.SetTextWithoutNotify(d.name);
                EditDescription.SetTextWithoutNotify(d.description);
                Title.text = "";
                break;
            }
        }
    }

    private void UpdateDeckUI(GameObject deck, Deck d)
    {
        deck.transform.Find("Canvas").Find("DeckDescription").Find("DescriptionText").GetComponent<TMP_Text>().text = d.description;
        deck.transform.Find("Canvas").Find("Name").Find("NameText").GetComponent<TMP_Text>().text = d.name;
    }
}


