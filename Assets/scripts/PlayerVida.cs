using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVida : MonoBehaviour
{

    [HideInInspector]
    public Vector3 posicaoInicial;

    void Start()
    {
        posicaoInicial = transform.position;
    }

}
