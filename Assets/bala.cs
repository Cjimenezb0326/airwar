using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bala : MonoBehaviour
{
    // Este script debe ir en el prefab de la bala

    private void OnCollisionEnter2D(Collision2D collision)
    {


        // Si la bala colisiona con un objeto con el tag "avion", destrúyelo
        
       Debug.Log("avion derribado" );

        Destroy(collision.gameObject);  // Destruye el objeto con tag "avion"
        Destroy(gameObject);           // Destruye la bala
        
    }
}
