using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViraPraTras : MonoBehaviour {
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            transform.Rotate(0, 180, 0);
        }
    }
}
