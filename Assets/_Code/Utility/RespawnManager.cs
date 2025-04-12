using System.Collections.Generic;
using UnityEngine;


public class RespawnManager : MonoBehaviour
{
    public static RespawnManager Ins { get; private set; }

    public List<GameObject> entities = new List<GameObject>();
    
    void Awake(){
        entities = new List<GameObject>();
        if(Ins != null && Ins != this){
            Destroy(gameObject);
        }
        else{
            Ins = this;
        }
    }
}
