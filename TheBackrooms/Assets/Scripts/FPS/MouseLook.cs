using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public float xRotation = 0f;

    public Transform playerBody;

    // Update is called once per frame
    void Update()
    {
        if (GameManager.instance.finalizando) {
            return;
        }
        float x = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float y = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= y;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * x);
    }

    void FixedUpdate() {
        if (GameManager.instance.finalizando) {
            GameObject inimigo = Inimigo.instance.gameObject.transform.Find("Cara").gameObject;
            LerpLookAt(inimigo.transform.position);
        }
    }

    void LerpLookAt(Vector3 focus) {
        float velocidade = 1f;
        Vector3 relativePos = focus - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, velocidade * Time.fixedDeltaTime);
    }
}
