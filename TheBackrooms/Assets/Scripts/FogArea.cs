using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogArea : MonoBehaviour {
    public float desiredFog;
    float startedFog;

    public GameObject desapearWithFog;
    
    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            startedFog = RenderSettings.fogDensity;

            StopAllCoroutines();
            StartCoroutine(ChangeFog());
        }
    }

    IEnumerator ChangeFog() {
        float t = 0;
        while (t < 1) {
            t += Time.deltaTime;
            RenderSettings.fogDensity = Mathf.Lerp(startedFog, desiredFog, t);
            yield return null;
        }

        if (desapearWithFog != null) {
            desapearWithFog.SetActive(false);
        }
    }

    IEnumerator ResetFog() {
        float t = 0;

        if (desapearWithFog != null) {
            desapearWithFog.SetActive(true);
        }

        while (t < 1) {
            t += Time.deltaTime;
            RenderSettings.fogDensity = Mathf.Lerp(desiredFog, startedFog, t);
            yield return null;
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            StopAllCoroutines();
            StartCoroutine(ResetFog());
        }
    }
}
