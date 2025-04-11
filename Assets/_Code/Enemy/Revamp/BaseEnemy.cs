using System.Collections;
using Globals;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour
{

    // Variables

    public EState s = EState.seeking;
// This should really be on the swordsman but i cant be boterthered
    [HideInInspector] public NavMeshAgent agent;


    public bool seeking;
    public bool hunting;
    public bool attacking;

    public int health = 100;

    public float seekRange = 100f;
    public float attackRange = 10f;
    public float maxYRange = 10f;

    public enum EState{
        seeking = 0, // Idle, checking for the player
        hunting = 1, // Actively chasing the player
        attacking = 2, // In attack range and attacking
        hooked = 3 // Has been hooked
    }

    public abstract IEnumerator Seek();
    public abstract IEnumerator Hunt();
    public abstract IEnumerator Attack();

    [HideInInspector] public GameObject player;

    IEnumerator ActionLoop(){
        Debug.Log("ActionLoop");

        if(s == EState.seeking && !seeking){
            StartCoroutine(Seek());
        }
        
        if(s == EState.attacking && !attacking){
            StartCoroutine(Attack());
        }
        
        if(s == EState.hunting && !hunting){
            StartCoroutine(Hunt());
        }

        yield return new WaitForSeconds(0.01f);

        StartCoroutine(ActionLoop());
    }

    void Start(){
        player=GameObject.FindGameObjectWithTag(glob.playerTag);
        agent = GetComponent<NavMeshAgent>();
        StartCoroutine(ActionLoop());
    }
}
