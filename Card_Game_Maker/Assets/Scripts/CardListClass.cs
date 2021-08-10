using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardListClass : MonoBehaviour
{
    public class CardList 
	{
		public string cardListName;
		public int cardListId;
	 
		public CardList(string name, int id)
		{
			cardListName = name;
			cardListId = id;
		}
	}
}
