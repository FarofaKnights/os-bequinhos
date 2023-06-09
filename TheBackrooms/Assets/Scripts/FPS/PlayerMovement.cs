using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    CharacterController cc;

    public float speed = 12f;
    public float runningSpeed = 20f;


    void Start() {
        cc = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (GameManager.instance.finalizando) {
            return;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float currentSpeed = speed;

        if (Input.GetKey(KeyCode.LeftControl)) {
            currentSpeed = runningSpeed;
        }
        

        if (x != 0 || z != 0){
            Vector3 move = transform.right * x + transform.forward * z;
            move = move * currentSpeed;
            move.y = cc.isGrounded ? 0 : Gravity().y;
            
            cc.Move(move * Time.fixedDeltaTime);
        } else if (!cc.isGrounded) {
            cc.Move(Gravity() * Time.fixedDeltaTime);
        }
    }

    Vector3 Gravity() {
        Vector3 pos = Vector3.zero;
        pos.y -= 9.81f;
        return pos;
    }
}
