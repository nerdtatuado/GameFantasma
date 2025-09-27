using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaiuMorteTrigger : MonoBehaviour
{
    private PlayerSelector selector;

    void Start()
    {
        selector = FindObjectOfType<PlayerSelector>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (selector == null) return;

        if (other.CompareTag("PlayerAzul") || other.CompareTag("PlayerAmarelo") || other.CompareTag("PlayerVermelho"))
        {
            selector.PerderVida(other.gameObject);
        }
    }





}
