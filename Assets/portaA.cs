using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portaA : MonoBehaviour
{
    public GameObject apPrefab; 
    public int cantidadAP = 5;

    public Vector2 rangoY = new Vector2(-2f, 9f);  
    public Vector2 rangoX = new Vector2(-9f, 9f); 

    private List<GameObject> aplist = new List<GameObject>(); 

    private void Start()
    {
        for (int i = 0; i < cantidadAP; i++)
        {
            CrearAeropuerto();
        }
    }

    private void CrearAeropuerto()
    {
        float posX = Random.Range(rangoX.x, rangoX.y);
        float posY = Random.Range(rangoY.x, rangoY.y);

        Vector3 posicionInicial = new Vector3(posX, posY, 0f);

        GameObject nuevoap = Instantiate(apPrefab, posicionInicial, Quaternion.identity);
        aplist.Add(nuevoap);
    }
}
