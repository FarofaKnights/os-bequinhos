using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTextureSetup : MonoBehaviour {
    public Camera cameraB;
    public Material matCameraB;

    public Camera cameraA;
    public Material matCameraA;

    void Start() {
        if (cameraB != null && matCameraB != null)
            ChangeCamera(cameraB, matCameraB);

        if (cameraA != null && matCameraA != null)
            ChangeCamera(cameraA, matCameraA);
    }

    void ChangeCamera(Camera cam, Material mat) {
        if (cam.targetTexture != null) {
            cam.targetTexture.Release();
        }

        cam.targetTexture = new RenderTexture(Screen.width, Screen.height, 24);
        mat.mainTexture = cam.targetTexture;
    }
}
