using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public enum Estado { Jogando, Pausado };
    Estado estado = Estado.Jogando;

    public Nodo raiz;
    public List<Porta> portas = new List<Porta>();
    List<Porta> portasNaoColetadas = new List<Porta>();

    public List<Coletavel> coletados = new List<Coletavel>();

    public GameObject door;

    public Transform inimigoLastPose;

    public bool finalizando = false;

    void Awake() {
        instance = this;
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;

        // Gera arvore de mapeamento do inimigo
        GeraPosicao(raiz);
        ValorParaArvore(raiz, Inimigo.instance.mapeamento);

        Despausar();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (estado == Estado.Jogando) {
                Pausar();
            } else {
                Despausar();
            }
        }
    }

    public void Pausar() {
        estado = Estado.Pausado;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Time.timeScale = 0;
    }

    public void Despausar() {
        estado = Estado.Jogando;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Time.timeScale = 1;
    }

    int GeraPosicao(Nodo atual, int cont = 0) {
        if (atual == null) return cont;
        cont = GeraPosicao(atual.esquerdo, cont);
        cont++;
        atual.id = cont;
        cont = GeraPosicao(atual.direito, cont);
        return cont;
    }

    void ValorParaArvore(Nodo atual, Arvore arvore) {
        if (atual == null) return;
        Nodo esq = atual.esquerdo;
        Nodo dir = atual.direito;

        arvore.Inserir(atual);
        ValorParaArvore(esq, arvore);
        ValorParaArvore(dir, arvore);
    }

    public void AddPorta(Porta porta) {
        portas.Add(porta);

        if (!porta.coletada)
            portasNaoColetadas.Add(porta);
    }

    public void ColetarPorta(Porta porta) {
        porta.coletada = true;
        portasNaoColetadas.Remove(porta);

        if (portasNaoColetadas.Count == 0 && !finalizando) {
            Acabar();
        }
    }

    public Porta GetDoor() {
        if (portasNaoColetadas.Count == 0) return null;

        int random = Random.Range(0, portasNaoColetadas.Count);
        return portasNaoColetadas[random];
    }

    public void Coletar(Coletavel coletavel) {
        if (coletados.Contains(coletavel)) return;

        coletados.Add(coletavel);

        if (coletados.Count == 3) {
            door.SetActive(false);
        }
    }

    public void Acabar() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void Finalizar() {
        Inimigo inimigo = Inimigo.instance;
        inimigo.gameObject.transform.SetParent(inimigoLastPose);
        inimigo.gameObject.transform.localPosition = Vector3.zero;
        inimigo.gameObject.transform.localRotation = Quaternion.identity;
        inimigo.GetComponent<Rigidbody>().isKinematic = true;

        inimigo.ChangeEstado(Inimigo.Estado.Esperando);

        finalizando = true;

        StartCoroutine(FinalizarCoroutine());
    }

    IEnumerator FinalizarCoroutine() {
        yield return new WaitForSeconds(3);

        Inimigo.instance.LevarUmTiro();

        yield return new WaitForSeconds(1);

        SceneManager.LoadScene("Menu");
    }
}
