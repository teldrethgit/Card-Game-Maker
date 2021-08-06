using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
//draggable methods/variables reference: https://github.com/omnirift/drag-and-drop-unity/blob/1/Assets/Scripts/UIElementDragger.cs


public class GameRunner : MonoBehaviour
{
    public GameObject MainCanvas;
    public GameObject PlayerInfo;
    public GameObject EnemyInfo;
    public GameObject CardPrefab;
    public GameObject LoadScreen;
    public GameObject InfoScreen;
    public GameObject EndTurnButton;
    public GameObject EnemyCardBack;
    public GameObject EnemyIcon;
    public GameObject PlayerDeckInfo;
    public GameObject EnemyDeckInfo;
    public GameObject PlayerHealthOverlay;
    public GameObject[] CardSlotGhosts;
    public Transform[] PlayerFieldSlots;
    public Transform[] PlayerHandSlots;
    public Transform[] EnemyHandSlots;
    public Transform[] EnemyFieldSlots;
    public Transform TempCards;
    
    private Card[] PCards; 
    private Card[] ECards; 
    private Vector2 originalPosition;
    private Transform objectToDrag, attacker, defender;
    private Image objectToDragImage;
    private GameObject originalParent;
    private bool messageDisplayed = false, attacking = false, dragging = false;
    private string currentTurn;
    private int playerHandCount, playerDeckRemaining, enemyDeckRemaining, playerDeckLocation = 0, enemyDeckLocation = 0, playerHealth = 30, playerMana = 1, enemyHealth = 30, round = 1, enemyMana = 1;
    private List<Card> enemyHand;

    private Color COLOR_RED = new Color(1f, 0.086f, 0.028f, 0.2823f);
    private Color COLOR_GREEN = new Color(0.023f, 1f, 0.0203f, 0.2823f);
    private Color COLOR_OFF = new Color(0f, 0f, 0f, 0f);

    List<RaycastResult> hitObjects = new List<RaycastResult>();

    void Start()
    {
        StartCoroutine(WaitForRequests());
    }

    void Update()
    {
        UpdateGameUI();
        if (enemyHealth == 0)
        {
            StartCoroutine(EndGame(true));
            return;
        } 
        else if (playerHealth == 0)
        {
            StartCoroutine(EndGame(false));
            return;
        }

        switch(currentTurn)
        {
            case "StartPlayerTurn":
                StartCoroutine(PlayerTurn());
                currentTurn = "PlayerTurn";
                break;
            case "EndPlayerTurn": 
                StartCoroutine(EnemyTurn());
                currentTurn = "EnemyTurn";
                EndTurnButton.SetActive(false);
                RefreshField();
                break;
        }

        if (Input.GetMouseButtonDown(0) && !messageDisplayed)
        {
            StartDrag();
            if(!attacking)
            {
                StartAttack();
            } else 
            {
                EndAttack();
            }
        }

        if (dragging)
        {
            objectToDrag.position = Input.mousePosition;
        }

        if (attacking)
        {
            CheckHighlight();
        }
          
        if (Input.GetMouseButtonUp(0))
        {
           EndDrag();
        }
    }

    string fixJson(string value)
    {
        value = "{\"Result\":" + value + "}";
        return value;
    }

    IEnumerator WaitForRequests()
    {
        Coroutine Player = StartCoroutine(RequestDeck(0));
        Coroutine Enemy = StartCoroutine(RequestDeck(1));
        
        yield return new WaitForSeconds(3f);
        yield return Player;
        yield return Enemy;
        
        LoadScreen.SetActive(false); 
    }

    IEnumerator EndGame(bool won)
    {
        if (won)
        {
            yield return StartCoroutine(DisplayMessage("You Won!!!!", true));
        }
        else
        {
            yield return StartCoroutine(DisplayMessage("Defeat...", true));
        }
    }

    IEnumerator RequestDeck(int player)
    {
        UnityWebRequest webRequest = UnityWebRequest.Get("https://osucapstone.herokuapp.com/decks/1/cards");
        yield return webRequest.SendWebRequest();
       
        if (webRequest.responseCode == 200)
        {
            Card[] cards = JsonHelper.FromJson<Card>(fixJson(webRequest.downloadHandler.text));

            if (player == 0)
            {
                PCards = cards;
            } else
            {
                ECards = cards;
            }

            foreach (Card card in cards) 
            {
                card.card = Instantiate(CardPrefab, TempCards);
                CardHelpers.UpdateUI(card, player);
            }
        }
        else 
        {
            Debug.Log("response failed");
        }
    }

    IEnumerator DisplayMessage(string msg, bool permanent = false)
    {
        messageDisplayed = true;
        InfoScreen.transform.Find("InfoText").GetComponent<TMP_Text>().text = msg;
        InfoScreen.SetActive(true);
        yield return new WaitForSeconds(1f);
        
        messageDisplayed = permanent;
        InfoScreen.SetActive(permanent); 
    }

    IEnumerator EnemyTurn()
    {
        yield return StartCoroutine(DisplayMessage("Opponent Thinking"));

        yield return StartCoroutine(EnemyPlayCards());
        yield return StartCoroutine(EnemyAttack());
        yield return new WaitForSeconds(1f);

        currentTurn = "StartPlayerTurn";
        round++;
    }

    IEnumerator PlayerTurn()
    {
        yield return StartCoroutine(DisplayMessage("It's your turn!"));
        
        EndTurnButton.SetActive(true);
        playerMana = Math.Min(round, 10);
        if (playerHandCount < 5 && PCards.Length - playerDeckLocation > 0)
        {
            PCards[playerDeckLocation].card.transform.SetParent(PlayerHandSlots[playerHandCount], false);
            playerHandCount++;
            playerDeckLocation++;
        }
    }

    public void SetCard(int id, Transform pos)
    {
        foreach(Card card in PCards) 
        {
            if (card.id == id)
            {
                card.card.transform.SetParent(pos, false);
                break;
            }
        }
    }

    public void EndTurn()
    {
        currentTurn = "EndPlayerTurn";
    }

    public void Deal()
    {
        int index = 0;
        foreach (Card c in PCards)
        {
            SetCard(c.id, PlayerHandSlots[index]);
            index++;
            if (index > 4) {break;}
        }

        enemyHand = new List<Card>();
        for(int i = 0; i < 5; i++)
        {
            Instantiate(EnemyCardBack, EnemyHandSlots[i]);
            enemyHand.Add(ECards[i]);
            enemyDeckLocation++;
        }
        playerHandCount = 5;
        playerDeckLocation = 5;
        currentTurn = "StartPlayerTurn";
        PlayerDeckInfo.SetActive(true);
        EnemyDeckInfo.SetActive(true);
    }

    private GameObject GetObjectUnderMouse()
    {
        var pointer = new PointerEventData(EventSystem.current);

        pointer.position = Input.mousePosition;

        EventSystem.current.RaycastAll(pointer, hitObjects);

        if (hitObjects.Count <= 0) return null;
            
        return hitObjects.First().gameObject;
    }

    private Transform GetObject(string tag = "PlayerCard")
    {
        var clickedObject = GetObjectUnderMouse();
        if (clickedObject?.tag != "CardRaycast") return null;
        clickedObject = clickedObject.transform.parent.parent.gameObject;
        
        if (clickedObject != null && clickedObject.tag == tag)
        {
            return clickedObject.transform;
        }

        return null;
    }

    private Transform GetObjectIcon()
    {
        var clickedObject = GetObjectUnderMouse();
        if (clickedObject?.tag == "EnemyIcon")
        {
            return clickedObject.transform;
        }

        return null;
    }

    private Transform GetPlaceable()
    {
        var clickedObject = GetObjectUnderMouse();

        if (clickedObject != null && clickedObject.tag == "PlayerField")
        {
            return clickedObject.transform;
        }

        return null;
    }

    private void ToggleDrag(bool enabled)
    {
        foreach (GameObject ghost in CardSlotGhosts)
        {
            ghost.SetActive(enabled);
        }
        dragging = enabled; 
    }

    private void StartDrag()
    {
        if (currentTurn != "PlayerTurn"){return;}
        objectToDrag = GetObject();
        Card card = CardHelpers.FindCard(PCards, objectToDrag?.gameObject);

            if (objectToDrag != null && !card.inField)
            {
                ToggleDrag(true);

                originalParent = objectToDrag.parent.gameObject;
                objectToDrag.SetParent(MainCanvas.transform);
                objectToDrag.SetAsLastSibling();

                originalPosition = objectToDrag.position;
                objectToDragImage = objectToDrag.Find("Canvas").Find("RaycastTarget").GetComponent<Image>();
                objectToDragImage.raycastTarget = false;
            } else 
            {
                objectToDrag = null;
            }
    }

    private void StartAttack()
    {
        if (currentTurn != "PlayerTurn"){return;}
        attacker = GetObject();
        Card card = CardHelpers.FindCard(PCards, attacker?.gameObject);

            if (attacker != null && card.inField && !card.hasAttacked)
            {
                Highlight(attacker.gameObject, "Green", true);
                attacking = true;
            }
    }

    private void CheckHighlight()
    {
        if (currentTurn != "PlayerTurn"){return;}
        Transform check = GetObject("EnemyCard");
        Transform checkIcon = GetObjectIcon();

        if (check == null && defender != null && defender?.gameObject?.tag == "EnemyCard")
        {
            defender = null;
        }
        else if(checkIcon == null && defender != null && defender?.gameObject?.tag == "EnemyIcon")
        {
            defender = null;
        }
        else if(check != null || checkIcon != null)
        {
            Transform target = check == null ? checkIcon : check;
            defender = target;
        }

        foreach (Transform slot in EnemyFieldSlots)
        {
            if(slot.Find("CardFront(Clone)") != null)
            {
                Highlight(slot.Find("CardFront(Clone)").gameObject, "off", false);
            }
        }
        Highlight(EnemyIcon, "off", false);
        if (defender != null)
        {
            Highlight(defender?.gameObject, "Red", true);
        }
    }

    private void EndDrag()
    {
        if (objectToDrag != null)
        {
            var location = GetPlaceable();

            if (location != null)
            {
                Card card = CardHelpers.FindCard(PCards, objectToDrag.gameObject);
                if (playerMana - card.cost < 0)
                {
                    objectToDrag.SetParent(originalParent.transform);
                    objectToDrag.position = originalPosition;
                }
                else
                {
                    objectToDrag.SetParent(location.transform.parent.transform);
                    objectToDrag.position = location.position;
                    card.inField = true;
                    playerHandCount--;
                    playerMana -= card.cost;
                }              
            }
            else
            {
                objectToDrag.SetParent(originalParent.transform);
                objectToDrag.position = originalPosition;
            }
            objectToDragImage.raycastTarget = true;
            objectToDrag = null;
            ToggleDrag(false);
            UpdateHand();
        }
    }

    private void EndAttack()
    {
        if (defender != null && defender?.gameObject?.tag == "EnemyCard")
        {
            Card attackCard = CardHelpers.FindCard(PCards, attacker.gameObject);
            Card defendCard = CardHelpers.FindCard(ECards, defender.gameObject);

            attackCard.health -= defendCard.attack;
            defendCard.health -= attackCard.attack;
            if (attackCard.health <= 0) {attackCard.card.transform.SetParent(TempCards,false);}
            if (defendCard.health <= 0) {defendCard.card.transform.SetParent(TempCards,false);}
            CardHelpers.UpdateUI(attackCard,0);
            CardHelpers.UpdateUI(defendCard,1);
            Highlight(defender.gameObject, "off", false);
            attackCard.hasAttacked = true;
        }
        else if (defender != null && defender?.gameObject?.tag == "EnemyIcon")
        {
            Card attackCard = CardHelpers.FindCard(PCards, attacker.gameObject);
            enemyHealth = Math.Max(enemyHealth - attackCard.attack, 0);
            Highlight(defender.gameObject, "off", false);
            attackCard.hasAttacked = true;
        }
        Highlight(attacker.gameObject, "off", false);
        attacking = false;
    }

    private void UpdateGameUI()
    {
        PlayerInfo.transform.Find("HPText").GetComponent<TMP_Text>().text = playerHealth.ToString();
        PlayerInfo.transform.Find("ManaText").GetComponent<TMP_Text>().text = playerMana.ToString();
        EnemyInfo.transform.Find("HPText").GetComponent<TMP_Text>().text = enemyHealth.ToString();
        PlayerDeckInfo.GetComponent<TMP_Text>().text = PCards == null ? "0" : (PCards.Length - playerDeckLocation).ToString();
        EnemyDeckInfo.GetComponent<TMP_Text>().text = ECards == null ? "0" : (ECards.Length - enemyDeckLocation).ToString();
    }

    private void UpdateHand()
    {
        List<GameObject> hand = new List<GameObject>();
        foreach (Transform slot in PlayerHandSlots)
        {
            if(slot.Find("CardFront(Clone)") != null)
            {
                hand.Add(slot.Find("CardFront(Clone)").gameObject);
            }
        }

        int i = 0;
        foreach (GameObject c in hand)
        {
            c.transform.SetParent(PlayerHandSlots[i], false);
            i++;
        }
    }

    private void Highlight(GameObject card, string color, bool enable)
    {
        Image overlay = card.tag == "EnemyIcon" ? 
            card.GetComponent<Image>() :
            card.transform.Find("Canvas").Find("Overlay").GetComponent<Image>();

        if (!enable)
        {
            overlay.color = COLOR_OFF;
        }
        else if (color == "Red")
        {
            overlay.color = COLOR_RED;
        }
        else
        {
            overlay.color = COLOR_GREEN;
        }
    }

    private void RefreshField()
    {
        foreach (Transform slot in PlayerFieldSlots)
        {
            if(slot.Find("CardFront(Clone)") != null)
            {
                CardHelpers.FindCard(PCards, slot.Find("CardFront(Clone)").gameObject).hasAttacked = false;
            }
        }
    }

    private void UpdateEnemyHandUI()
    {
        int i = 0;
        foreach (Transform slot in EnemyHandSlots)
        {
            if (i < enemyHand.Count && slot.Find("CardBackPurple(Clone)") == null)
            {
                Instantiate(EnemyCardBack, slot);
            }
            else if(i >= enemyHand.Count && slot.Find("CardBackPurple(Clone)") != null)
            {
                Destroy(slot.Find("CardBackPurple(Clone)").gameObject);
            }
            i++;
        }
    }

    IEnumerator EnemyPlayCards()
    {
        List<Card> played = new List<Card>();
        enemyMana = Math.Min(round, 10);
        if(ECards.Length - enemyDeckLocation > 0 && enemyHand.Count < 5)
        {
            enemyHand.Add(ECards[enemyDeckLocation]);
            enemyDeckLocation++;
            UpdateEnemyHandUI();
            yield return new WaitForSeconds(1f);
        }
        

        bool filled = false;
        foreach (Transform slot in EnemyFieldSlots)
        {
            if(slot.Find("CardFront(Clone)") == null)
            {   
                filled = false;
                foreach (Card c in enemyHand)
                {
                    if (c.cost <= enemyMana && !c.inField && !filled)
                    {
                        c.card.transform.SetParent(slot, false);
                        played.Add(c);
                        enemyMana -= c.cost;
                        c.inField = true;
                        filled = true;
                        Highlight(c.card, "Green", true);
                        enemyHand.Remove(c);
                        UpdateEnemyHandUI();
                        yield return new WaitForSeconds(1f);
                        Highlight(c.card, "off", false);
                        break;
                    }
                }
            }
        }
        
        /*List<Card> filteredList = new List<Card>();
        foreach (Card c in enemyHand)
        {
            if (!c.inField)
            {
                filteredList.Add(c);
            }
        }
        enemyHand = filteredList;
        
        UpdateEnemyHandUI();
        if(played.Count > 0)
        {
            yield return new WaitForSeconds(1f);
        }
        foreach (Card c in played)
        {
            Highlight(c.card,"off",false);
        }*/
    }

    IEnumerator EnemyAttack()
    {
        List<Card> targets = GetTargetsForEnemy();

        foreach (Transform slot in EnemyFieldSlots)
        {
            if(slot.Find("CardFront(Clone)") != null)
            {   
                Card c = CardHelpers.FindCard(ECards, slot.Find("CardFront(Clone)").gameObject);
                if (!c.hasAttacked)
                {
                    EnemyCardAttack(c, targets);
                    yield return new WaitForSeconds(1f);
    
                    Highlight(c.card, "off", false);
                    if (c.health <= 0){c.card.transform.SetParent(TempCards,false);}
                    
                    if (targets.Count > 0)
                    {
                        Highlight(targets[0].card, "off", false);
                        if (targets[0].health <= 0)
                        {
                            targets[0].card.transform.SetParent(TempCards,false);
                            targets.Remove(targets[0]);
                        }
                    } else
                    {
                        Highlight(PlayerHealthOverlay, "off", false);
                    }
                    if (playerHealth == 0){yield break;}
                }
                else
                {
                    c.hasAttacked = false;
                }
            }
        }
        yield return null;
    }

    private List<Card> GetTargetsForEnemy()
    {
        List<Card> list = new List<Card>();
        foreach (Transform slot in PlayerFieldSlots)
        {
            if(slot.Find("CardFront(Clone)") != null)
            {
                list.Add(CardHelpers.FindCard(PCards, slot.Find("CardFront(Clone)").gameObject));
            }
        }
        return list;
    }

    private void EnemyCardAttack(Card attacker,  List<Card> targets)
    {
        if (targets.Count == 0)
        {
            playerHealth = Math.Max(playerHealth - attacker.attack, 0);
            Highlight(PlayerHealthOverlay, "Red", true);
        } else
        {
            Card defender = targets[0];
            attacker.health = Math.Max(attacker.health - defender.attack, 0);
            defender.health = Math.Max(defender.health - attacker.attack, 0);

            CardHelpers.UpdateUI(attacker,1);
            CardHelpers.UpdateUI(defender,0);
            Highlight(defender.card, "Red", true);
        }
        Highlight(attacker.card, "Green", true);
    }
}
