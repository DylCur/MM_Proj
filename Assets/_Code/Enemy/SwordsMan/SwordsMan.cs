using System.Collections;
using System.Collections.Generic;
using Globals;
using MathsAndSome;
using UnityEngine;

// No [RequireComponent(typeof(NavMeshAgent))] because i might need to add 
// and remove it for the ai to work properly

//! The ai is STUPID so ALL objects MUST be curved!!!!!!!!!

[RequireComponent(typeof(Rigidbody))]
public class SwordsMan : BaseEnemy
{

    public Rigidbody rb;
    public float moveSpeed = 12f;
    [SerializeField] float pushForce = 2f;
   
    public Vector3 Direction(Vector3 targetPos){
        Vector3 td = (targetPos-transform.position).normalized;
        return new Vector3(td.x,0,td.z);
    }

    public override void EnemyStart()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Push(){
        GameObject[] g = GameObject.FindGameObjectsWithTag(glob.swordsmanTag);
        List<SwordsMan> swordsMen = new List<SwordsMan>();

        foreach(GameObject obj in g){
            if(obj != gameObject){
                Vector3 distance = mas.AbsVector(mas.zeroY(transform.position-obj.transform.position));
                float dF = distance.x+distance.z;
                
                if(dF < 2){
                        rb.linearVelocity = rb.linearVelocity + distance.normalized * pushForce;
                }
            }
           
        }
    }

    void Update(){
        Push();
    }


    public override IEnumerator Hunt()
    {
        Debug.Log("Hunt");
        hunting = true;

        rb.linearVelocity = Direction(player.transform.position) * moveSpeed;

        if(inAttackRange()){
            s = EState.attacking;
        }

        yield return new WaitForSeconds(0.1f);

        hunting = false;
    }
    
    public override IEnumerator Attack()
    {
        Debug.Log("Attack");
        attacking = true;

        rb.linearVelocity = Vector3.zero;
        
        Vector3 pd = mas.PlayerDistance(player, gameObject);

        bool playerInRange = pd.x <= attackRange &&
        pd.z <= attackRange &&
        pd.y <= maxYRange;

        if(!playerInRange){
            s = EState.hunting;
        }

        yield return new WaitForSeconds(attackTime);

        attacking = false;
    }
}
