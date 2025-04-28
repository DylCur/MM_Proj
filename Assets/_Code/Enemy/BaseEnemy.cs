using System;
using System.Collections;
using System.Data.Common;
using Globals;
using MathsAndSome;
using UnityEngine;
using UnityEngine.AI;

public abstract class BaseEnemy : MonoBehaviour
{

    // Variables

    public EState s = EState.seeking;

    public bool seeking;
    public bool hunting;
    public bool attacking;

    public int health = 100;

    public float seekRange = 100f;
    public float attackRange = 10f;
    public float maxYRange = 10f;
    
    public int projLimit = 5;

    public int damage;


    [Header("Temp material stuff")]

    public Material hurtMat;
    public Material defaultMat;

    [SerializeField] float iframes = 0.1f;

    public enum EState{
        seeking = 0, // Idle, checking for the player
        hunting = 1, // Actively chasing the player
        attacking = 2, // In attack range and attacking
        hooked = 3, // Has been hooked
        dead = 4    // Health <= 0
    }

    // Was abstract but was the same over all enemies
    public IEnumerator Seek(){
        Debug.Log("Seek");
        seeking = true;

        if(inHuntRange()){
            s = EState.hunting;
        }

        yield return new WaitForSeconds(0.1f);

        seeking = false;
    }

    public bool canTakeDamage = true;

    public abstract IEnumerator Hunt();
    public abstract IEnumerator Attack();

    public bool inAttackRange(){
        Vector3 pd = mas.PlayerDistance(player, gameObject);
        
        return pd.x <= attackRange &&
        pd.z <= attackRange &&
        pd.y <= maxYRange;
    }

    public bool inHuntRange(){
        Vector3 pd = mas.PlayerDistance(player, gameObject);
        
        return pd.x <= seekRange &&
        pd.z <= seekRange &&
        pd.y <= maxYRange;
    }

    IEnumerator IFrames(){
        canTakeDamage = false;
        yield return new WaitForSeconds(iframes);
        canTakeDamage = true;
    }

    void Die(){
        s = EState.dead;
        Destroy(GetComponent<CapsuleCollider>());
        Destroy(GetComponent<MeshRenderer>());
        Destroy(GetComponent<MeshFilter>());
    }

    IEnumerator ChangeMaterial(){
        GetComponent<MeshRenderer>().material = hurtMat;
        yield return new WaitForSeconds(0.2f);
        GetComponent<MeshRenderer>().material = defaultMat;
    }

    public void TakeDamage(int damage){
        if(canTakeDamage){
            health -= damage;

            StartCoroutine(ChangeMaterial());
            
            if(health<=0){
                Die();
            }

            StartCoroutine(IFrames());
        }
    }

    

    void Awake(){
        Debug.Log(gameObject);
    }

    public float attackTime = 0.2f;

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

    public abstract void EnemyStart();

    void Start(){

    /*
        try{
            RespawnManager.Ins.entities.Add(gameObject);  
        }

        catch(Exception e){
            Debug.LogError($"Caught exception {e} while attempting to add {gameObject.name} from the Entities List");
        }
    */
        player=GameObject.FindGameObjectWithTag(glob.playerTag);
        EnemyStart();
        StartCoroutine(ActionLoop());
    }
}
