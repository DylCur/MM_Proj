using UnityEngine;
using System.Collections;

public class FollowCursor : MonoBehaviour
{
    void Update(){
        Vector3 mousePos = Input.mousePosition;
        transform.position = new Vector3(mousePos.x,mousePos.y,0);  
    }
}
