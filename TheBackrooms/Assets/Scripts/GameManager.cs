using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public enum Estado { Jogando, Pausado };
    Estado estado = Estado.Jogando;

    public Nodo raiz;
    public List<Porta> portas = new List<Porta>();
    List<Porta> portasNaoColetadas = new List<Porta>();

    void Awake() {
        instance = this;
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;

        // Gera arvore de mapeamento do inimigo
        GeraPosicao(raiz);
        ValorParaArvore(raiz, Inimigo.instance.mapeamento);
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
    }

    public int GetDoor() {
        if (portasNaoColetadas.Count == 0) return -1;

        int random = Random.Range(0, portasNaoColetadas.Count);
        return portasNaoColetadas[random].nodo.id;
    }
}
