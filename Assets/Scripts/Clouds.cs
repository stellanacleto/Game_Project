using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class clouds : MonoBehaviour
{
    public float velocidadeDaNuvem;

    void Update()
    {
        MovimentarNuvem();
    }

    private void MovimentarNuvem()
    {
        Vector2 deslocamento = new Vector2(Time.time * velocidadeDaNuvem, 0);
        GetComponent<Renderer>().material.mainTextureOffset = deslocamento;
    }
}
