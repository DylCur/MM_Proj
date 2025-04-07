using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using MathsAndSome;


// [RequireComponent(typeof(NavMeshAgent))]
public abstract class BaseEnemy : MonoBehaviour
{

    public enum eState{
        walking,
        hunting,
        hooked,
    }
    
    [HideInInspector] public eState s = eState.walking;
    [HideInInspector] public  NavMeshAgent agent;


    public float detectionRadius;
    public float damage;
    public float atkTime;

    protected bool canSeekPlayer => inRange() && !seeking;
    protected bool shouldSeekPlayer => s == eState.walking && canSeekPlayer;

    protected bool canAttack = true;

    protected bool seeking;

    public GameObject player;
    protected bool shouldRedirect = true;
    protected float delay = 0.01f;
    [SerializeField] protected Vector3 attackRange;

    bool inAttackRange => PlayerDistance().x < attackRange.x && PlayerDistance().y < attackRange.y && PlayerDistance().z < attackRange.z;

    protected IEnumerator redirectDelay(){
        shouldRedirect = false;
        yield return new WaitForSeconds(delay);
        shouldRedirect = true;

        if(s == eState.hunting){
            if(inAttackRange){
                AttackPlayer();
            }
            else{
                SeekPlayer();
            }
        }
    }

    protected void Start(){
        attackRange = new Vector3(attackRange.x, attackRange.y, attackRange.x);
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // This is the default, this will be different for the menace
    protected bool inRange(){
        return Physics.Raycast(
            gameObject.transform.position,
            player.transform.position - gameObject.transform.position, 
            out RaycastHit hit,
            detectionRadius
        );

    }


    protected Vector3 PlayerDistance(){
        return mas.AbsVector(player.transform.position - gameObject.transform.position);
    }

    protected abstract void SeekPlayer();
    protected abstract void AttackPlayer();
    

    // SeekPlayerGeneric() will be the same accross all enemies, SeekPlayer() will be unique
    protected void SeekPlayerGeneric(){
        seeking = true;
        s = eState.hunting;
        SeekPlayer();
    }



    public void Update(){
        if(canSeekPlayer){
            SeekPlayerGeneric();
        }
    }

    public IEnumerator AttackCD(){
        canAttack = false;
        yield return new WaitForSeconds(atkTime);
        canAttack = true;
    }
    



    


}
