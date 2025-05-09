using UnityEngine;
using MathsAndSome;
using System.Collections;
using Magical;

/*
public class BezierCurve{

   

    public IEnumerator CalcualteCurve(){
        
    }

    public double bez(double t, double[] coefficients){
        double[] beta = coefficients;
        int n = beta.length;
        for (int i = 1; i < n; i++) {
            for (int j = 0; j < (n - i); j++) {
                beta[j] = beta[j] * (1 - t) + beta[j + 1] * t;
            }
        }
        return beta[0];
    }
}
*/

public class SlidePage : MonoBehaviour
{
    [SerializeField] Vector2 targetPosition;
    [SerializeField] float moveTime;

    
    void Update(){
        if(magic.key.down(keys.left)){
            
        }
    }

    public IEnumerator GoToTargetPosition(float t){

        transform.position = mas.LerpVectors(transform.position, targetPosition, t);
        t+=0.1f;
        yield return new WaitForSeconds(0.1f);
        
        if(t < 1){
            
            StartCoroutine(GoToTargetPosition(t));
        }

    }
} 
