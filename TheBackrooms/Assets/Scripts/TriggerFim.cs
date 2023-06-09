using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerFim : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            GameManager.instance.Finalizar();
        }
    }
}
