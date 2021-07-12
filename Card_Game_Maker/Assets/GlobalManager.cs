using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Reference: https://forum.unity.com/threads/game-initialization-script.721127/

public class GlobalManager{
 
    private static GlobalManager instance;
 
    public int userId;
 
    private GlobalManager(){
        userId = 0;
    }
 
    public static GlobalManager GetInstance(){
        if(instance == null){
            instance = new GlobalManager();
        }
        return instance;
    }
}
