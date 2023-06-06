using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodo : MonoBehaviour {
    public Nodo esquerdo, direito;
    public int id;
    public GameObject waypoint;

    public Porta porta;

    // Start is called before the first frame update
    void Start() {
        if (transform.Find("Porta") != null)
            porta = transform.Find("Porta").GetComponent<Porta>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 GetPosition() {
        return waypoint.transform.position;
    }

    void OnDrawGizmosSelected() {
        Vector3 yOff = new Vector3(0, 1, 0);

        if (esquerdo != null) {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(transform.position + yOff, esquerdo.transform.position + yOff);
        }

        if (direito != null) {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + yOff, direito.transform.position + yOff);
        }
    }
}
