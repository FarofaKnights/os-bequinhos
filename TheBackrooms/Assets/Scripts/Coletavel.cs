using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coletavel : MonoBehaviour {
    void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            GameManager.instance.Coletar(this);
            Destroy(gameObject);
        }
    }
}
