using UnityEngine;

public class ThirdPersonCamera : GenericCamera
{
    [SerializeField][Tooltip("The target that the camera has to follow.")]
    private Transform Target;

    [SerializeField][Tooltip("The position ofthe camera regarding the target.")]
    private Vector3 Offset;

    [SerializeField][Tooltip("The smoothing speed when following the target.")]
    private float SmoothingSpeed = 10f;

    private void Start()
    {
        if (Target == null)
        {
            print("ERROR: No target assigned. Stop camera.");
        }
    }

    /// <summary>
    /// Update the camera position
    /// </summary>
    private void LateUpdate()
    {
        if (Target != null)
        {   
            transform.localPosition = Vector3.Lerp(transform.localPosition, Offset, Time.deltaTime * SmoothingSpeed);
        }
    }
}
