using UnityEngine;

/// <summary>
/// Make the camera rotate around its parent.
/// </summary>
public class CameraRotator : GenericCamera
{
    [SerializeField][Tooltip("The rotation speed around the parent.")]
    private float RotationSpeed = 1f;

    [SerializeField][Tooltip("The rotation acceleration around the parent.")]
    private float RotationAcceleration = 1f;

    private bool _isRotating;

    /// <summary>
    /// Start rotating the camera around its parent.
    /// </summary>
    public void StartRotate() => _isRotating = true;

    /// <summary>
    /// Stop rotating the camera around its parent.
    /// </summary>
    public void StopRotate() => _isRotating = false;

    // Make the camera rotate around the parent
    private void LateUpdate()
    {
        if (_isRotating)
        {
            transform.parent.Rotate(0, Time.deltaTime * RotationSpeed * RotationAcceleration, 0);
            RotationAcceleration += Time.deltaTime;
        }
    }
}
