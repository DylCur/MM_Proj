using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
public class DefaultDoor : DoorController
{
    BoxCollider bc;

    void Start(){
        bc = GetComponent<BoxCollider>();
    }

    public override void OpenDoor()
    {
        bc.isTrigger = true;


    }

    public override IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(0.3f);
        bc.isTrigger = false;
    }
}
