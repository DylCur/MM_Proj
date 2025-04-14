using System.Collections;
using System.Collections.Generic;
using MathsAndSome;
using UnityEngine;
using UnityEngine.AI;


// The menace flies away :(

[RequireComponent(typeof(Rigidbody))]
public class Archer : BaseEnemy
{
    Vector2 playerOffset;

    bool moving;

    Rigidbody rb;

    /*[HideInInspector]*/ public List<GameObject> arrows;

    public GameObject arrow;
    NavMeshAgent na = new NavMeshAgent();



    void ShootArrow(){
        if(arrows.Count < projLimit){
            GameObject ar = Instantiate(arrow, transform.position, Quaternion.identity);
            ar.GetComponent<ArrowController>().parent = GetComponent<CapsuleCollider>();
            arrows.Add(ar);
        }
    }

    public override IEnumerator Attack()
    {
        attacking = true;

        ShootArrow();
        

        yield return new WaitForSeconds(attackTime);

        attacking = false;
    }

    public override void EnemyStart()
    {
       rb = GetComponent<Rigidbody>();
       arrows = new List<GameObject>();
    }

    public override IEnumerator Hunt()
    {


        Debug.Log("Hunt");
        hunting = true;

        

        // If the player is frame
        // This is so they don just shoot at a wall
        if(mas.ShootPlayer(player, transform.position).collider == player.GetComponent<CapsuleCollider>()){
            if(inAttackRange()){
                na = GetComponent<NavMeshAgent>();

                if(na != null){
                    na.SetDestination(transform.position);
                }

                s = EState.attacking;
            }
        }

        else{
            if(GetComponent<NavMeshAgent>() == null){
                // Add NavMeshAgent if it doesnt already exist
                na = gameObject.AddComponent<NavMeshAgent>();
            }
            else{
                // Move to the player, but as soon as theyre in frame they will stop
                na.SetDestination(player.transform.position);
            }
        }

        yield return new WaitForSeconds(0.1f);

        hunting = false;
    } 


}
