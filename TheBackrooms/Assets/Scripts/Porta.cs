using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Porta : MonoBehaviour {
    [NonSerialized] public Nodo nodo;
    public Transform waypoint;
    public bool coletada = false;

    public GameObject[] ativar;
    public GameObject[] desativar;

    void Start() {
        nodo = transform.parent.GetComponent<Nodo>();
        waypoint = transform.Find("WaypointPorta");
        GameManager.instance.AddPorta(this);

        if (ativar != null) {
            foreach (GameObject obj in ativar) {
                obj.SetActive(false);
            }
        }

        if (desativar != null) {
            foreach (GameObject obj in desativar) {
                obj.SetActive(true);
            }
        }
    }
    
    public void Coletar() {
        coletada = true;
        GameManager.instance.ColetarPorta(this);

        if (ativar != null) {
            foreach (GameObject obj in ativar) {
                obj.SetActive(true);
            }
        }

        if (desativar != null) {
            foreach (GameObject obj in desativar) {
                obj.SetActive(false);
            }
        }
    }
}
