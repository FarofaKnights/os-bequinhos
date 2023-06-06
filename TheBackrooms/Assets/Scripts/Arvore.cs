using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Arvore {
    public Nodo raiz;
    public Inimigo inimigo;

    public Arvore(Inimigo inimigo) {
        raiz = null;
        this.inimigo = inimigo;
    }

    public void Inserir(Nodo novo) {
        novo.esquerdo = null;
        novo.direito = null;
        raiz = Inserir(raiz, novo);
    }

    protected Nodo Inserir(Nodo nodo, Nodo novo) {
        if (nodo == null) {
            return novo;
        }
        
        if (nodo.id > novo.id) nodo.esquerdo = Inserir(nodo.esquerdo, novo);
        else if (nodo.id < novo.id) nodo.direito = Inserir(nodo.direito, novo);
        else throw new Exception("id já inserido!");

        return nodo;
    }

    public void Remover(int id) {
        Remover(raiz, id);
    }

    protected Nodo Remover(Nodo nodo, int id) {
        if (nodo == null) return null;
        if (id > nodo.id) nodo.direito = Remover(nodo.direito, id);
        else if (id < nodo.id) nodo.esquerdo = Remover(nodo.esquerdo, id);
        else {
            if (nodo.esquerdo == null && nodo.direito == null) return null;
            if (nodo.esquerdo == null && nodo.direito != null) return nodo.direito;
            if (nodo.esquerdo != null && nodo.direito == null) return nodo.esquerdo;
            Nodo menor = Menor(nodo.direito);
            nodo.id = menor.id;
            nodo.direito = Remover(nodo.direito, menor.id);
        }
        return nodo;
    }

    public bool Pesquisar(int id) {
        return Pesquisar(raiz, id) != null;
    }

    protected Nodo Pesquisar(Nodo nodo, int id) {
        if (nodo == null) return null;
        inimigo.AddWaypoint(nodo.waypoint.transform);
        if (nodo.id > id) return Pesquisar(nodo.esquerdo, id);
        if (nodo.id < id) return Pesquisar(nodo.direito, id);
        return nodo;
    }

    protected Nodo Menor(Nodo nodo) {
        if (nodo.esquerdo != null) return Menor(nodo.esquerdo);
        return nodo;
    }
}