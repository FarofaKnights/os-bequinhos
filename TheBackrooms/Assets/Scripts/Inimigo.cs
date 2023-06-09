using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour {
    public static Inimigo instance;

    public List<Transform> view = new List<Transform>();

    public Queue<Transform> waypoints = new Queue<Transform>();
    Transform waypointAtual;
    public Transform wayportaDesesperado;

    public Arvore mapeamento;

    public enum Estado { IndoNodo, Atacando, Parado, IndoPorta, Esperando, Teleportando };
    public Estado estado = Estado.Esperando;

    IsInCamera isInimigoInCamera;
    public IsInCamera isSpawnInCamera;

    Porta portaAtual;

    Animator anim;

    public float velocidade = 5;

    Rigidbody rb;

    IEnumerator Inicial() {
        yield return new WaitForSeconds(5);
        ChangeEstado(Estado.Teleportando);
    }

    public void ChangeEstado(Estado estado) {
        if (this.estado == estado) return;
        string oldAnim = GetEstadoAnimation(this.estado);

        this.estado = estado;
        if (oldAnim != GetEstadoAnimation(estado)) anim.SetTrigger(GetEstadoAnimation(estado));
    }

    string GetEstadoAnimation(Estado estado) {
        switch (estado) {
            case Estado.Parado:
            case Estado.Esperando:
            case Estado.Teleportando:
                return "Parar";
            case Estado.IndoNodo:
            case Estado.IndoPorta:
                return "Andar";
            case Estado.Atacando:
                return "Atacar";
            default:
                return "";
        }
    }

    void Awake() {
        instance = this;
        mapeamento = new Arvore(this);
    }

    void Start() {
        rb = GetComponent<Rigidbody>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        isInimigoInCamera = GetComponent<IsInCamera>();

        StartCoroutine(Inicial());
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
            case Estado.Teleportando:
                AttemptTeleport();
                break;
        }
    }

    bool PortaAindaDisponivel(Porta porta) {
        return porta != null && !porta.coletada && porta.disponivel;
    }

    public void SearchNodo() {
        if (waypoints.Count == 0) {
            Porta port = GameManager.instance.GetDoor();
            portaAtual = port;

            if (port != null && mapeamento.Pesquisar(port.nodo.id) && PortaAindaDisponivel(portaAtual)) {
                ChangeEstado(Estado.IndoNodo);
                waypointAtual = waypoints.Peek();
            }
            else {
                portaAtual = null;
                waypoints.Clear();
                view.Clear();
                ChangeEstado(Estado.Parado);
            }
        }
    }

    void Walking() {
        Vector3 waypointPos = waypointAtual.position;
        waypointPos.y = transform.position.y;

        Vector3 direction = waypointPos - transform.position;
        direction.y = 0;
        rb.velocity = direction.normalized * velocidade;

        LerpLookAt(waypointPos);

        if (Vector3.Distance(transform.position, waypointPos) < 0.5f) {
            if (estado == Estado.IndoNodo) GotToNodo();
            else if (estado == Estado.IndoPorta) {
                wayportaDesesperado = waypointAtual;
                StartCoroutine(GotToPorta());
            }

            if (waypoints.Count == 0) EndedMovement();
        }

        if (!PortaAindaDisponivel(portaAtual)) {
            waypoints.Clear();
            view.Clear();

            EndedMovement();
        }
    }

    void AttemptTeleport() {
        if (isInimigoInCamera.isInCamera) return;
        if (isSpawnInCamera.isInCamera) return;

        Vector3 pos = mapeamento.raiz.GetPosition();
        pos.y = transform.position.y;
        transform.position = pos;
        rb.velocity = Vector3.zero;

        ChangeEstado(Estado.Parado);
    }

    void GotToNodo() {
        Nodo nodo = waypointAtual.parent.GetComponent<Nodo>();
        if (nodo == null) return;

        // Se houver uma porta no nodo atual, ir até ela
        if (nodo.porta != null && !nodo.porta.coletada) {
            waypointAtual = nodo.porta.waypoint;
            ChangeEstado(Estado.IndoPorta);
        } else {
            RemoveWaypoint();
            if (waypoints.Count > 0) waypointAtual = waypoints.Peek();
        }
    }

    IEnumerator GotToPorta() {
        waypointAtual = waypoints.Peek();
        ChangeEstado(Estado.Esperando);
        rb.velocity = Vector3.zero;

        yield return new WaitForSeconds(3);

        Porta porta = wayportaDesesperado.parent.GetComponent<Porta>();
        if (PortaAindaDisponivel(porta))
            porta.Coletar();

        // Se é o ultimo nodo
        if (waypoints.Count == 1) {
            RemoveWaypoint();
            EndedMovement();
        }
        else ChangeEstado(Estado.IndoNodo);
    }

    void EndedMovement() {
        ChangeEstado(Estado.Teleportando);
    }

    void LerpLookAt(Vector3 focus) {
        Vector3 relativePos = focus - transform.position;
        Quaternion toRotation = Quaternion.LookRotation(relativePos);
        transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, velocidade * Time.fixedDeltaTime);
    }

    public void LevarUmTiro() {
        anim.SetTrigger("Empurrar");
    }

    #region WaypointRelated
    public void AddWaypoint(Transform waypoint) {
        view.Add(waypoint);
        waypoints.Enqueue(waypoint);
    }

    protected Transform RemoveWaypoint() {
        Transform t = waypoints.Dequeue();
        view.Remove(t);
        return t;
    }
    #endregion
}
