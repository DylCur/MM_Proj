using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using MathsAndSome;
using UnityEngine;

public enum state{
    walking,
    sliding,
    slamming,
    wall_run
}

[RequireComponent(typeof(Rigidbody))]           // For movement
[RequireComponent(typeof(CapsuleCollider))]     // For Collision
[RequireComponent(typeof(BoxCollider))]         // To Stop the player being launched
public class PlayerController : MonoBehaviour
{
    // State controls state so movement modes innit

    #region Variables
    public state s = state.walking;

    [Header("Camera")]
    [Range(1, 4)] public float mouseSensitivityX = 2.0f;
    [Range(1, 4)] public float mouseSensitivityY = 2.0f;

    [Range(-90, -60)] public float minY = -60f;
    [Range(60, 90)] public float maxY = 60f;

    [Header("Movement")]
    [Header("Walking")]
    [Range(3, 20)] public float moveSpeed = 5f;
    float hInp;
    float vInp;

    Rigidbody rb;
    [HideInInspector] public Camera playerCamera;

    float currentXRotation = 0f;

    Vector3 moveDirection;
    Vector3 slideDirection;

    [Header("Stamina")]
    public int stamina = 100;
    [SerializeField] float staminaRechargeCD;
    [SerializeField] int staminaPerRecharge;
    [SerializeField] int maxStamina = 100;
    [SerializeField] int staminaPerDash = 33;
    [SerializeField] bool rechargeWhileSliding;
    
    [Header("Jumping")]
    [SerializeField] float checkHeight;
    [SerializeField] bool canJump = true;
    [SerializeField] float jumpForce = 100f;
    public KeyCode jumpKey = KeyCode.Space;
    const float checkWidth = 0.3f;


    [Header("Sliding")]
    public float slideSpeed = 100f;
    [HideInInspector] public KeyCode slideKey = KeyCode.LeftControl;
    Vector3 dScale; // Default Scale
    Vector3 hScale; // Half scale

    [Header("Slamming")]
    [SerializeField] float slamForce = 90f;
    [HideInInspector] public KeyCode slamKey = KeyCode.LeftControl;
    [SerializeField] float slamJumpForce = 10f;
    [SerializeField] float slamJumpTime;
    float fallTime;
    bool timing;

    [Header("Dashing")]
    [HideInInspector] public KeyCode dashKey = KeyCode.LeftShift;
    [Range(5, 20)] public float dashForce = 100f;
    bool canDash = true;
    bool dashing;

    [Header("Wall Running")]
    // [SerializeField] float gravityScale=0.8f;
    // bool canWallRun = true;

    [Header("Wall Jumping")]
    [SerializeField] bool canWallJump = true;
    bool wallJumping;
    [SerializeField] [Range(1, 50)] float wallJumpForce = 100f;
    public float wjt = 0.5f;
    
    /*
    bool shouldWallRun(){
        // Gets all of the colliders in wall running range
        Collider[] cols = Physics.OverlapBox
        (
            transform.position - (transform.localScale/2),
            new Vector3(transform.localScale.x * 1.1f,transform.localScale.y, transform.localScale.z * 1.1f)
        );


        // If there is more than one (Player will always be included), loop through them
        if(cols.Length > 1){
            foreach(Collider col in cols){
                // If its a wall
                if(col.tag == "wall"){
                    // If the player is able to wall run
                    if(canWallRun && s == state.walking && !grounded()){return true;}
                }
            }
        }

        return false;

    }
    */

    // Takes in an array of colliders are returns the combined normal of those colliders
    //* Testing = Successful
    Vector3 GetWallNormals(){
        List<Collider> cols = mas.GetCollidersInArea(transform).ToList();
        List<Collider> wallCols = cols;
        
        for(int i = 0; i < cols.Count; i++){
            if(cols[i].tag != "wall"){
                wallCols.RemoveAt(i);
            }
        }

        Debug.Log(mas.GetNormalFromListOfColliders(wallCols));
        return -mas.GetNormalFromListOfColliders(wallCols);
    }


    bool shouldWallJump(){
        // Gets all of the colliders in wall running range
        Collider[] cols = Physics.OverlapBox
        (
            transform.position - (transform.localScale/2),
            new Vector3(transform.localScale.x * 1.1f,transform.localScale.y, transform.localScale.z * 1.1f)
        );


        // If there is more than one (Player will always be included), loop through them
        if(cols.Length > 2){
            foreach(Collider col in cols){
                // If its a wall
                if(col.tag == "wall"){
                    // If the player is able to wall run
                    if(canWallJump && s == state.walking && !grounded() && Input.GetKeyDown(jumpKey)){                       
                        return true;
                    }
                }
            }
        }
        return false;
    }

    float fov(){
        Vector3 v = rb.linearVelocity;
        v = new Vector3(v.x,0,v.z);
        float f = Mathf.Pow(Mathf.Atan(v.x+v.z),2) * 40 + 10;
        f = Mathf.Clamp(f, 90, 9999);
        return f;
    }

    
        

    [Header("Misc")]
    [SerializeField] GameObject forwardObject;


    bool shouldJump => canJump && Input.GetKeyDown(jumpKey) && grounded();
    bool shouldDash => canDash && Input.GetKeyDown(dashKey) && !dashing && stamina-staminaPerDash >= 0;

    bool grounded(){

        Vector3 scale = gameObject.transform.localScale;
        Vector3 pos = gameObject.transform.position;
        List<Collider> colliders = Physics.OverlapBox(new Vector3(pos.x, pos.y - scale.y/2, pos.z),
                                   new Vector3(scale.x *checkWidth, checkHeight, scale.z*checkWidth)).ToList();

        colliders.Remove(GetComponent<CapsuleCollider>());
        colliders.Remove(GetComponent<BoxCollider>());

        if(colliders.Count() > 0)
            return true;
        return false;

    }

    #endregion

    
    IEnumerator SlamJumpTimer(){
        float ogForce = jumpForce;
        jumpForce = slamJumpForce;
        yield return new WaitForSeconds(slamJumpTime);
        jumpForce = ogForce;
    }   
   

    #region Functions
    

    Vector3 DashForce(){
        Debug.Log("Dash");
        s = state.walking;
        Vector3 dashDirection;
        Vector3 forward = playerCamera.transform.forward;
        Vector3 right = playerCamera.transform.right;

        dashDirection = vInp == 0 && hInp == 0 ? (forward + right*hInp).normalized : slideDirection = (forward * vInp + right*hInp).normalized;
        return new Vector3(dashDirection.x * dashForce, 0,dashDirection.z * dashForce);
    }

    //TODO Fix and implement this
    /*
    void WallRun(){
        s = state.wall_run;
    }

    void UnWallRun(){
        s = state.walking;
    }
    */

    #region  Core Functions
    void Start()
    {

        if(GetComponent<HookshotController>() == null){
            Debug.LogError("Hookshot controller not found!");
        }


        if(forwardObject == null){
            Debug.LogError("Forward object is null!");
        }

        dScale=transform.localScale;
        hScale=new Vector3(dScale.x/2,dScale.y/2,dScale.z/2);


        rb = GetComponent<Rigidbody>();
        playerCamera = Camera.main;

       
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        StartCoroutine(HandleStamina());
    }


    IEnumerator DashCD(){
        dashing = true;
        yield return new WaitForSeconds(0.1f);
        dashing = false;
    }


    

    IEnumerator WallJump(){
        wallJumping = true;
        yield return new WaitForSeconds(wjt);
        wallJumping = false;
    }

    

    void Update()
    {

        forwardObject.transform.localEulerAngles = new Vector3(0,playerCamera.transform.localEulerAngles.y,playerCamera.transform.localEulerAngles.z);
        
        // playerCamera.fieldOfView = fov();

        if(shouldDash){
            stamina-=staminaPerDash;
            StartCoroutine(DashCD());
        }


        HandleMouse();
        Movement();
        
        if(shouldJump){
            Jump();
        }

        if(Input.GetKeyDown(slideKey) && s != state.sliding && grounded()){
            Slide();
        }

        if(Input.GetKeyUp(slideKey) && s == state.sliding){
           Unslide();
        }

        if(Input.GetKeyDown(slamKey) && !grounded() && s != state.slamming){
            Slam();
        }

        if(grounded() && s == state.slamming){
            Debug.Log("Slammed on ground!");
            s = state.walking;
            StartCoroutine(SlamJumpTimer());
        }

        if(shouldWallJump()){
            StartCoroutine(WallJump());
        }

        

        

        //TODO Implement this later 
        // if(shouldWallRun()){
        //     WallRun();
        // }
        // else{
        //     UnWallRun();
        // }


    }

    void FixedUpdate()
    {
        MovePlayer();
    }
    #endregion
    
    
    #region  Movement
    void Jump(){
        rb.linearVelocity=new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    

    // Handles Mouse Movement
    void HandleMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivityX;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivityY;

        transform.Rotate(Vector3.up * mouseX);

        currentXRotation -= mouseY;
        currentXRotation = Mathf.Clamp(currentXRotation, minY, maxY);

        playerCamera.transform.localRotation = Quaternion.Euler(currentXRotation, 0f, 0f);
    }

    // Sets Movement Direction
    void Movement()
    {
        hInp = Input.GetAxisRaw("Horizontal"); 
        vInp = Input.GetAxisRaw("Vertical");  

        // if(Input.GetAxisRaw("Horizontal") != 0){
        //     NZHinp = Input.GetAxisRaw("Horizontal"); 
        // }
        
        // if(Input.GetAxisRaw("Vertical") != 0){
        //     NZVinp = Input.GetAxisRaw("Vertical"); 
        // }


        Vector3 forward = forwardObject.transform.forward;
        Vector3 right = forwardObject.transform.right;

        // Might make this not normalized bc cool
        moveDirection = (forward * vInp + right * hInp).normalized;
    }




    // Handles movement and velocity based on state

    Vector3 CalculateDashVelocity(Vector3 velocity){
        return new Vector3(velocity.x + DashForce().x,velocity.y,velocity.z + DashForce().z);
    }

    Vector3 CalculateWallJumpVelocity(Vector3 velocity){
        Vector3 normals = GetWallNormals();

        return velocity + (normals * wallJumpForce) + (Vector3.up / 100) ;
    }

    // I have an idea, what about if there is a target velocity and that is set based on state, current velocity and what keys are pressed
    // i.e. if youre walking but was just sliding and youre still holding w, the target velocity should be a little lower than sliding, instead of just walking speed
    Vector3 newVector;

    IEnumerator LerpVelocity(float t, Vector3 currentVector, Vector3 TargetVector){
        Vector3 uneditedVector = currentVector;
        if(t < 1){
            newVector = mas.LerpVectors(currentVector, TargetVector, t);
        }

        rb.linearVelocity = newVector;
        
        yield return new WaitForSeconds(0.1f);
        if(t < 1){
            StartCoroutine(LerpVelocity(t+0.1f, uneditedVector, TargetVector));
        }
        
    }

    void MovePlayer()
    {
        Vector3 velocity = Vector3.zero;

        switch(s){
            case state.walking:
                velocity = moveDirection * moveSpeed;
                velocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
                
                //The player can only dash while they are walking (Currently)
                
                
                if(wallJumping){
                    //TODO Implement this 😭
                    // velocity=CalculateWallJumpVelocity(velocity);
                }
                break;

            case state.sliding: // Change this || Its been like 2 weeks and i have no idea what there is to change?
                velocity = slideDirection * slideSpeed;
                velocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);
                break;

            case state.slamming:
                velocity = new Vector3(0, rb.linearVelocity.y, 0);
                if(dashing){
                    velocity = new Vector3(0, 0, 0);
                }
                break;
            case state.wall_run:
                velocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y / 2, rb.linearVelocity.z);
                break;
            


            default:
                // This should never happen so ive added a massive impulse to punish me for bad code
                velocity = new Vector3(10000000, 10000000, 10000000); 
                Debug.LogError("Default case chosen in MovePlayer() in PlayerController");
                break;
            
        }

        if(dashing){
            if(s == state.sliding){
                Unslide();
            }
            s = state.walking;
            velocity=CalculateDashVelocity(velocity);
           
        }
        
        rb.linearVelocity = velocity;
        // StartCoroutine(LerpVelocity(0.5f, rb.linearVelocity, velocity));

       
    }


    #region Sliding
    void Unslide(){
        s = state.walking;
        transform.position = new Vector3(transform.position.x, transform.position.y+transform.localScale.y, transform.position.z); 
        transform.localScale = dScale;
    }

    void Slide(){
        s = state.sliding;

        transform.localScale = hScale;
        transform.position = new Vector3(transform.position.x, transform.position.y-transform.localScale.y, transform.position.z);
        Vector3 forward = forwardObject.transform.forward;
        Vector3 right = forwardObject.transform.right;

        slideDirection = vInp == 0 && hInp == 0 ? slideDirection = (forward + right*hInp).normalized : slideDirection = (forward * vInp + right*hInp).normalized;
        
    }
    #endregion
    void Slam(){
        s = state.slamming;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, -slamForce, rb.linearVelocity.z);
    }
    #endregion

    #region Stamina

    void IncreaseStamina(){
        if(!grounded()){
            stamina+=staminaPerRecharge;
        }
        else{
            stamina+=Mathf.CeilToInt(staminaPerRecharge * 1.5f);
        }

        if(stamina > maxStamina){
            stamina = maxStamina;
        }
    }

    IEnumerator HandleStamina(){
        
        if(s!=state.sliding || rechargeWhileSliding){
            IncreaseStamina();
        }

        yield return new WaitForSeconds(staminaRechargeCD);
        StartCoroutine(HandleStamina());
    }

    #endregion
   
   
   
   
    #endregion
}
