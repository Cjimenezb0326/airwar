using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class avion : MonoBehaviour
{
    public GameObject avionPrefab; 
    public int cantidadAviones = 5;
    public float velocidadMovimiento = 2f; 
    public float rangoY = 5f; 
    public Vector2 rangoX = new Vector2(-10f, 10f); 

    private List<GameObject> avionesActivos = new List<GameObject>();

    private void Start()
    {
        StartCoroutine(CrearAvionesConEspera());
    }

    private void Update()
    {
        foreach (var avion in avionesActivos)
        {
            if (avion != null)
            {
                avion.transform.Translate(Vector3.up * velocidadMovimiento * Time.deltaTime);

                if (avion.transform.position.y > rangoY)
                {
                    Destroy(avion);
                    avionesActivos.Remove(avion);
                    CrearAvion();
                    break;
                }
            }
        }
    }

    private IEnumerator CrearAvionesConEspera()
    {
        for (int i = 0; i < cantidadAviones; i++)
        {
            CrearAvion();
            yield return new WaitForSeconds(2f); // Espera de 2 segundos antes de crear el siguiente avión
        }
    }

    private void CrearAvion()
    {
        float posX = Random.Range(rangoX.x, rangoX.y);
        float posY = -rangoY; 
        Vector3 posicionInicial = new Vector3(posX, posY, 0f);

        GameObject nuevoAvion = Instantiate(avionPrefab, posicionInicial, Quaternion.identity);
        avionesActivos.Add(nuevoAvion);
    }

    public void DestruirAvion(GameObject avion)
    {
        Destroy(avion);
        avionesActivos.Remove(avion);
        CrearAvion();
    }
}
