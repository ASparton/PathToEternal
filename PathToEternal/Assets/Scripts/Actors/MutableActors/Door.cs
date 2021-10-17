using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MutableActor
{
    public override void ExecuteTriggerAction(Trigger trigger)
    {
        print("I have been triggered !");
    }

    public override void ExecuteUntriggerAction(Trigger trigger)
    {
        print("I have been UNUNUNNUtriggered !");
    }
}
