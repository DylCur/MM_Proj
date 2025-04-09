using UnityEngine;
using MathsAndSome;
using Globals;

public abstract class DefaultEnemy : MonoBehaviour
{
    public enum eState{
        walking,
        hunting,
        hooked,
    }   

    protected eState s = eState.walking;
    PlayerController player;

    void Start(){
        player = mas.GetPlayer();
       
    }

    void SeekPlayer(){
        RaycastHit hit = mas.ShootPlayer(player.gameObject, transform.position);
       if(hit.collider != null){

       } 
    }

}
