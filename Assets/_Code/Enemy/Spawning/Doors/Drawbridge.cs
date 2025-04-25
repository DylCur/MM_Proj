using UnityEngine;
using System.Collections;

public class Drawbridge : DoorController
{
    public override void OpenDoor()
    {
        throw new System.NotImplementedException();
    }

    // This will never be called
    public override IEnumerator CloseDoor()
    {
        throw new System.NotImplementedException();
    }
}
