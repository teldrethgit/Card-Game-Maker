using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Reference: https://forum.unity.com/threads/game-initialization-script.721127/

public class CurrentGame{
 
    private static CurrentGame instance;
   
    public int id;
 
    private CurrentGame(){
        id = 35;
    }
 
    public static CurrentGame GetInstance(){
        if(instance == null){
            instance = new CurrentGame();
        }
        return instance;
    }
}