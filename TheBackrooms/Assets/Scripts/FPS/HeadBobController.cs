using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBobController : MonoBehaviour {
    public bool isEnabled = true;

    [SerializeField, Range(0f, 1f)]
    public float amplitude = 0.015f;
    [SerializeField, Range(0f, 10f)]
    public float frequency = 10f;
    [SerializeField, Range(1f, 15f)]
    public float focusMultiplier = 15f;

    Transform cam;
    public Transform cameraHolder;

    public float toggleSpeed = 3.0f;
    Vector3 startPosition;
    CharacterController cc;

    void Awake() {
        cam = cameraHolder.GetChild(0);
        startPosition = cameraHolder.localPosition;
        cc = GetComponent<CharacterController>();
    }

    void Update() {
        if (!isEnabled) return;
        CheckMotion();
        cam.LookAt(FocusTarget());
    }

    void CheckMotion() {
        float speed = new Vector3(cc.velocity.x, 0, cc.velocity.z).magnitude;
        ResetPosition();

        if (speed < toggleSpeed) return;
        if (!cc.isGrounded) return;

        PlayMotion(FootStepMotion());
    }

    void PlayMotion(Vector3 motion) {
        cameraHolder.localPosition += motion;
    }

    Vector3 FootStepMotion() {
        Vector3 pos = Vector3.zero;
        pos.x = Mathf.Sin(Time.time * frequency) * amplitude;
        pos.y = Mathf.Cos(Time.time * frequency / 2) * amplitude * 0.5f;
        return pos;
    }

    void ResetPosition() {
        if (cameraHolder.localPosition == startPosition) return;
        cameraHolder.localPosition = Vector3.Lerp(cameraHolder.localPosition, startPosition, Time.deltaTime * 1f);
    }

    Vector3 FocusTarget() {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + cameraHolder.localPosition.y, transform.position.z);
        pos += cameraHolder.forward * focusMultiplier;
        return pos;
    }
    
}
