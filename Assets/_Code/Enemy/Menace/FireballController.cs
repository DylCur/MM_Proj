using System.Collections;
using MathsAndSome;
using Unity.VisualScripting;
using UnityEngine;


// Fireball move type
public enum FBMT{
    forward,
    homing
}

// Im not sure if i want fireballs to home or not, but i think i want both so ill make both
[RequireComponent(typeof(Rigidbody))]
public class FireballController : MonoBehaviour
{

    [Header("Parameters")]
    public float speed = 10f;
    public float maxHomingRange = 10f;

    Rigidbody rb;
    Vector3 playerDistance;
    GameObject player;
    FBMT mt = FBMT.forward;

    public IEnumerator MoveForward(Vector3 r /* Rotation */){
        Vector3.RotateTowards(transform.position, r, 2*Mathf.PI, 3f);
        rb.linearVelocity = transform.forward * speed;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(MoveForward(r));
    }

    public IEnumerator HomeToPlayer(){
        playerDistance = mas.PlayerDistance(player, gameObject);
        float distance = mas.AddVectorComponents(playerDistance);

        if(distance > maxHomingRange){
            // Idk what the numbers in rotate towards are but i do know what a radian is
            transform.rotation = mas.v3q(Vector3.RotateTowards(transform.position, player.transform.position, 2*Mathf.PI, 3f));
        }

        rb.linearVelocity = transform.forward * speed;
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(HomeToPlayer());
        
    }

    void Awake(){
        rb = GetComponent<Rigidbody>();
        player = mas.GetPlayer().gameObject;

        if(mt == FBMT.forward){
            StartCoroutine(MoveForward(player.transform.position));
        }
        else if(mt == FBMT.homing){
            StartCoroutine(HomeToPlayer());
        }

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
    }

    void OnCollisionEnter(Collision other){
        StartCoroutine(DestroyObj(0f));
    }

    public FireballController(FBMT moveType){
        mt = moveType;
    }
}
