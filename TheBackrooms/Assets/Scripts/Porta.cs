using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Porta : MonoBehaviour {
    [NonSerialized] public Nodo nodo;
    public Transform waypoint;
    public bool coletada = false;

    void Start() {
        nodo = transform.parent.GetComponent<Nodo>();
        waypoint = transform.Find("WaypointPorta");
        GameManager.instance.AddPorta(this);
    }
    
    public void Coletar() {
        coletada = true;
        GameManager.instance.ColetarPorta(this);
    }
}
