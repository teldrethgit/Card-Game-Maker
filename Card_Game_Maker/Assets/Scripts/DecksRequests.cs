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
    public GameObject GameID;
    public GameObject DeckPrefab;
    public GameObject NameInput;
    public GameObject DescriptionInput;
    public GameObject CreateButton;
    public GameObject CreateFailText;
    public GameObject Index;
    public GameObject EditMenu;
    public GameObject DeckMenu;
    public GameObject EditFailText;
    public GameObject EditButton;
    public TMP_InputField EditName;
    public TMP_InputField EditDescription;
    public TMP_Text Title;
    public TMP_Text DisplayName;
    public TMP_Text DisplayDescription;
    
    private string cookie;
    private Deck[] decks;
    private Deck editing;

    void Start()
    {
        StartCoroutine(GetDecks());
    }

    IEnumerator GetDecks()
    {
 
        UnityWebRequest webRequest = UnityWebRequest.Get("https://osucapstone.herokuapp.com/decks?game=" + CurrentGame.GetInstance().id);
        yield return webRequest.SendWebRequest();

        if (webRequest.responseCode == 200)
        {
            Vector3 pos = DeckStartLoc.position;
            Vector3 viewPos = DeckFieldOfView.sizeDelta;
            GameObject currentDeck;
            decks = JsonHelper.FromJson<Deck>(fixJson(webRequest.downloadHandler.text));
            Deck random = new Deck();
            random.name = "Create Random Deck";
            random.description = "A random deck will be created for you with 30 cards whose values are generally balanced so that health + attack = cost";
            currentDeck = Instantiate(DeckPrefab, new Vector3(pos.x, pos.y, pos.z), Quaternion.identity, DeckStartLoc);
            UpdateDeckUI(currentDeck, random);
            Button button = currentDeck.transform.Find("Canvas").Find("Button").GetComponent<Button>();
            button.onClick.AddListener(() => SetRandom());

            int i = 1;
            foreach (Deck d in decks)
            {
                currentDeck = Instantiate(DeckPrefab, new Vector3(pos.x + (i * 750), pos.y, pos.z), Quaternion.identity, DeckStartLoc);
                UpdateDeckUI(currentDeck, d);
                button = currentDeck.transform.Find("Canvas").Find("Button").GetComponent<Button>();
                button.onClick.AddListener(() => SetEditing(d.id));
                i++;
            }
            viewPos.x = 1920 + (750 * (Math.Max(0, decks.Length - 1)));
            DeckFieldOfView.sizeDelta = viewPos;
            DeckFieldOfView.position = new Vector3(viewPos.x, DeckFieldOfView.position.y, DeckFieldOfView.position.z);
        }
        else
        {
            Debug.Log("wee woo wee woo");
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
        inputForm.Add(new MultipartFormDataSection("game", CurrentGame.GetInstance().id.ToString()));
        
        UnityWebRequest webRequest = UnityWebRequest.Post("https://osucapstone.herokuapp.com/decks", inputForm);
        yield return webRequest.SendWebRequest();
       
        if (webRequest.responseCode == 204)
        {
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
            DeckMenu.SetActive(true);
            EditMenu.SetActive(false);
            DisplayName.text = Name;
            DisplayDescription.text = Description;
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
                DeckMenu.SetActive(true);
                Index.SetActive(false);
                EditName.SetTextWithoutNotify(d.name);
                EditDescription.SetTextWithoutNotify(d.description);
                Title.text = "";
                DisplayName.text = d.name;
                DisplayDescription.text = d.description;
                CurrentGame.GetInstance().deck = d.id;
                break;
            }
        }
    }

    private void SetRandom()
    {
        CurrentGame.GetInstance().deck = -1;
        SceneManager.LoadScene("Play");
    }

    public void ResetScene()
    {
        SceneManager.LoadScene("Decks");
    }

    private void UpdateDeckUI(GameObject deck, Deck d)
    {
        deck.transform.Find("Canvas").Find("DeckDescription").Find("DescriptionText").GetComponent<TMP_Text>().text = d.description;
        deck.transform.Find("Canvas").Find("Name").Find("NameText").GetComponent<TMP_Text>().text = d.name;
    }
}


