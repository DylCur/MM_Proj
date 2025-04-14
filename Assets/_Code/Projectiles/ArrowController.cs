using System.Collections;
using UnityEngine;
using MathsAndSome;

public class ArrowController : ProjectileController
{

    // Delta y
    float dx;
    // Previous y
    float px;
    // Current y
    float cx;

    [SerializeField] Vector3 RotationalOffset;

    public override IEnumerator DestroyObj(float delay)
    {
        yield return new WaitForSeconds(delay);
        //* Insert any animations or particles or player collsion etc here

        // Save on processing power by just destroying the object if its far enough away
        if(mas.AddVectorComponents(mas.PlayerDistance(player, gameObject)) > 500f){
            Destroy(gameObject);
        }
        else{
            Destroy(gameObject);    
        }

        Archer a = parent.GetComponent<Archer>();
    
        if(a!=null){
            a.arrows.Remove(gameObject);
        }
    }

    void Update(){
        Vector2 t = new Vector2(transform.position.x, transform.position.z);
        Vector2 p = new Vector2(player.transform.position.x, player.transform.position.z);
        float theta = mas.FindDotProduct(t, p);
        transform.localRotation = Quaternion.Euler(transform.localRotation.x,transform.localRotation.y,theta);
    }

    public IEnumerator GetDeltaDistance()
    {
        px=cx;

        // Rotate on z to face player

        // Get the vectors
      


        yield return new WaitForSeconds(0.1f);
        cx = rb.linearVelocity.x;
        dx = cx-px;
        transform.localRotation = Quaternion.Euler(RotationalOffset.x+dx, RotationalOffset.y, transform.localRotation.z);
        StartCoroutine(GetDeltaDistance());
    }
}

    

