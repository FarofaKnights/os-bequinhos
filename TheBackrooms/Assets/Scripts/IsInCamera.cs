using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInCamera : MonoBehaviour {
    Camera cam;
    Plane[] cameraFrustum;
    Collider col;

    public bool isInCamera = false;

    void Start() {
        cam = Camera.main;
        col = GetComponent<Collider>();
    }

    void Update() {
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(cam);
        isInCamera = GeometryUtility.TestPlanesAABB(cameraFrustum, col.bounds);
    }
}
