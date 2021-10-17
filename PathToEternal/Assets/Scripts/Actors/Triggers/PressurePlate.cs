using UnityEngine;

/// <summary>
/// A PressurePlate is a Trigger that change its material and position when triggered.
/// </summary>
public class PressurePlate : Trigger
{
    protected override void setAspect(bool triggeredAspect)
    {
        if (triggeredAspect)
        {
            print("Trigger");
            transform.position = new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2, transform.position.z);
        }
        else
        {
            print("Not trigger");
            transform.position = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, transform.position.z);
        }
    }
}
