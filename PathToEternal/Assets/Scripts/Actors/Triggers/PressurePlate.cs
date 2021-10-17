using System.Collections;
using UnityEngine;

/// <summary>
/// A PressurePlate is a Trigger that change its material and position when triggered.
/// </summary>
public class PressurePlate : Trigger
{
    protected override void setAspect(bool triggeredAspect)
    {
        if (triggeredAspect)
            StartCoroutine(ChangeAspectTo(true));
        else
            StartCoroutine(ChangeAspectTo(false));
    }

    /// <summary>
    /// Interpolate the position of the pressure plate to the pressed or not pressed position.
    /// </summary>
    /// <param name="pressed">True to interpolate to the pressed position, false to the not pressed position.</param>
    /// <returns>Corountine</returns>
    private IEnumerator ChangeAspectTo(bool pressed)
    {
        Vector3 finalPosition;
        if (pressed)
            finalPosition = new Vector3(transform.position.x, transform.position.y - transform.localScale.y / 2, transform.position.z);
        else
            finalPosition = new Vector3(transform.position.x, transform.position.y + transform.localScale.y / 2, transform.position.z);

        while (Vector3.Distance(transform.position, finalPosition) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, finalPosition, Time.deltaTime * 10);
            yield return null;
        }

        transform.position = finalPosition;
    }
}
