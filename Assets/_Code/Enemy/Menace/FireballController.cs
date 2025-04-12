using System;
using System.Collections;
using Globals;
using MathsAndSome;
using Unity.VisualScripting;
using UnityEngine;


// Fireball move type
public enum FBMT{
    forward,
    homing
}


// Fireballs can stack ontop of each other, thats a problem that can be fixed later
// If the fireball is directly above the player it moves incredibly slowly

// Im not sure if i want fireballs to home or not, but i think i want both so ill make both
[RequireComponent(typeof(Rigidbody))]
public class FireballController : MonoBehaviour
{

    [Header("Parameters")]
    public float speed = 10f;
    public float maxHomingRange = 10f;
    public FBMT mt = FBMT.forward;
    Vector3 direction;


    Rigidbody rb;
    Vector3 playerDistance;
    GameObject player;
    [HideInInspector] public Collider parent;

    public IEnumerator MoveForward(Vector3 r /* Rotation */){
        rb.linearVelocity = r * speed;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(MoveForward(r));
    }

    public IEnumerator HomeToPlayer(){
        playerDistance = mas.PlayerDistance(player, gameObject);
        float distance = mas.AddVectorComponents(playerDistance);
    
        if(distance > maxHomingRange){
            // Idk what the numbers in rotate towards are but i do know what a radian is
            // transform.rotation = mas.VectorsToQuaternion(transform.position, player.transform.position);
            // transform.rotation = mas.v3q(Vector3.RotateTowards(transform.position, player.transform.position, 2*Mathf.PI, 1f));
            direction = -(transform.position - player.transform.position).normalized;
        }

        rb.linearVelocity = direction * speed;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(HomeToPlayer());
        
    }

    void Awake(){
        rb = GetComponent<Rigidbody>();
        player = mas.GetPlayer().gameObject;

        direction = Vector3.zero;

        if(mt == FBMT.forward){
            StartCoroutine(MoveForward(-(transform.position - player.transform.position).normalized));
        }
        else if(mt == FBMT.homing){
            StartCoroutine(HomeToPlayer());
        }

        // RespawnManager.Ins.entities.Add(gameObject);

        DestroyObj(15f);
    }

    IEnumerator DestroyObj(float delay){

        yield return new WaitForSeconds(delay);
        //* Insert any animations or particles or player collsion etc here

        // Save on processing power by just destroying the object if its far enough away
        if(mas.AddVectorComponents(mas.PlayerDistance(player, gameObject)) > 500f){
            Destroy(gameObject);
        }
        else{
            Destroy(gameObject);    
        }
/*
        try{
            RespawnManager.Ins.entities.Remove(gameObject);
        }

        catch(Exception e){
            Debug.LogError($"Caught exception {e} while attempting to remove {gameObject.name} from the Entities List");
        }
*/
        parent.GetComponent<Menace>().fireballs.Remove(gameObject);

    }

    void OnTriggerEnter(Collider other){
        
        if(other.tag == glob.playerTag){
            
            CombatController cc = other.GetComponent<CombatController>();
            // No reason it should be null other than i got a bit silly
            if(cc!=null){
                cc.TakeDamage(10);
            }

            StartCoroutine(DestroyObj(0f));
        }

        // Check to see if the other is a fireball
        if(other.GetComponent<FireballController>() == null){
            if(other != parent){
                StartCoroutine(DestroyObj(0f));
            }
        }
        
        
    }

    public FireballController(FBMT moveType, Collider p){
        mt = moveType;
        parent = p;
    }
}
