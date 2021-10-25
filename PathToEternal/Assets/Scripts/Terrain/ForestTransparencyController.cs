using UnityEngine;

public class ForestTransparencyController : MonoBehaviour
{   
    private bool _isForestTransparent;

    // Set the forest transparency depending on the camera
    private void Start() => SetForestTransparent(true);

    // Set the forest transparency depending on the camera
    private void Update()
    {
        if (CameraController.Instance.IsLevelCameraActive() && !_isForestTransparent)
            SetForestTransparent(true);
        else if (!CameraController.Instance.IsLevelCameraActive() && _isForestTransparent)
            SetForestTransparent(false);
    }

    private void SetForestTransparent(bool transparent)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform currentTree = transform.GetChild(i).GetChild(1);
            MeshRenderer currentMR = (MeshRenderer)currentTree.GetComponent("MeshRenderer");

            float alpha = 1f;
            if (transparent)
                alpha = 0.2f;

            currentMR.material.color = new Color(currentMR.material.color.r, currentMR.material.color.g, currentMR.material.color.b, alpha);
        }

        _isForestTransparent = transparent;
    }
}
