using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSegue : MonoBehaviour
{
    public GameObject player;
    public float camVel = 0.25f;
    private bool segueHeroi;
    public Vector3 ultimaAlvoPos;
    public Vector3 velAtual;
    
    void Start()
    {
        segueHeroi = true;
        ultimaAlvoPos = player.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(segueHeroi)
        {
           
            Vector3 novaCamPos = Vector3.SmoothDamp(transform.position, player.transform.position, ref velAtual, camVel);

            transform.position = new Vector3(novaCamPos.x, novaCamPos.y, transform.position.z);

            ultimaAlvoPos = player.transform.position;
            
        }
    }
}
