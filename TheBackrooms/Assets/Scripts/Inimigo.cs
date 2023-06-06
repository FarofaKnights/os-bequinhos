using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour {
    public static Inimigo instance;

    public List<Transform> view = new List<Transform>();

    public Queue<Transform> waypoints = new Queue<Transform>();
    Transform waypointAtual;

    public Arvore mapeamento;

    public enum Estado { IndoNodo, Atacando, Parado, IndoPorta };
    public Estado estado = Estado.Parado;

    public float velocidade = 5;

    Rigidbody rb;

    void Awake() {
        instance = this;
        mapeamento = new Arvore(this);
    }

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        switch (estado) {
            case Estado.Parado:
                SearchNodo();
                break;
            case Estado.IndoNodo:
                Walking();
                break;
            case Estado.IndoPorta:
                Walking();
                break;
        }
    }


    public void SearchNodo() {
        if (waypoints.Count == 0) {
            int random = GameManager.instance.GetDoor();
            if (random > -1 && mapeamento.Pesquisar(random)) {
                estado = Estado.IndoNodo;
                waypointAtual = waypoints.Peek();
            }
            else {
                waypoints.Clear();
                view.Clear();
                estado = Estado.Parado;
            }
        }
    }

    void Walking() {
        Vector3 direction = waypointAtual.position - transform.position;
        rb.velocity = direction.normalized * velocidade;

        if (Vector3.Distance(transform.position, waypointAtual.position) < 0.1f) {
            if (estado == Estado.IndoNodo) GotToNodo();
            else if (estado == Estado.IndoPorta) GotToPorta();

            if (waypoints.Count == 0) EndedMovement();
        }
    }

    void GotToNodo() {
        Nodo nodo = waypointAtual.parent.GetComponent<Nodo>();
        if (nodo == null) return;

        // Se houver uma porta no nodo atual, ir até ela
        if (nodo.porta != null && !nodo.porta.coletada) {
            waypointAtual = nodo.porta.waypoint;
            estado = Estado.IndoPorta;
        } else {
            RemoveWaypoint();
            if (waypoints.Count > 0) waypointAtual = waypoints.Peek();
        }
    }

    void GotToPorta() {
        waypointAtual = waypoints.Peek();

        // Se é o ultimo nodo
        if (waypoints.Count == 1) RemoveWaypoint();
        else estado = Estado.IndoNodo;
    }

    void EndedMovement() {
        transform.position = mapeamento.raiz.GetPosition();
        rb.velocity = Vector3.zero;
        estado = Estado.Parado;
    }

    public void AddWaypoint(Transform waypoint) {
        view.Add(waypoint);
        waypoints.Enqueue(waypoint);
    }

    protected Transform RemoveWaypoint() {
        Transform t = waypoints.Dequeue();
        view.Remove(t);
        return t;
    }
}
