using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class aeropuertos : MonoBehaviour
{
    public GameObject apPrefab; // Prefab de aeropuertos
    public int cantidadAP = 5; // Número de aeropuertos

    public Vector2 rangoY = new Vector2(-2f, 9f);  
    public Vector2 rangoX = new Vector2(-9f, 9f); 

    private List<GameObject> aplist = new List<GameObject>(); // Lista de aeropuertos generados
    private List<Ruta> rutas = new List<Ruta>(); // Lista de rutas generadas

    private void Start()
    {
        // Crear aeropuertos
        for (int i = 0; i < cantidadAP; i++)
        {
            CrearAeropuerto();
        }

        // Generar rutas entre aeropuertos y portaaviones
        GenerarRutas();
    }

    private void CrearAeropuerto()
    {
        float posX = Random.Range(rangoX.x, rangoX.y);
        float posY = Random.Range(rangoY.x, rangoY.y);

        Vector3 posicionInicial = new Vector3(posX, posY, 0f);

        GameObject nuevoap = Instantiate(apPrefab, posicionInicial, Quaternion.identity);
        aplist.Add(nuevoap);
    }

    private void GenerarRutas()
    {
        // Obtener portaaviones existentes en la escena usando su tag
        GameObject[] portaaviones = GameObject.FindGameObjectsWithTag("PortaAviones");

        // Crear rutas entre todos los aeropuertos y portaaviones
        foreach (GameObject aeropuerto in aplist)
        {
            foreach (GameObject portaavion in portaaviones)
            {
                float distancia = Vector3.Distance(aeropuerto.transform.position, portaavion.transform.position);
                float peso = CalcularPeso(distancia, portaavion);

                Ruta nuevaRuta = new Ruta(aeropuerto, portaavion, peso);
                rutas.Add(nuevaRuta);

                Debug.Log($"Ruta generada: {aeropuerto.name} -> {portaavion.name} | Peso: {peso}");
            }
        }
    }

    private float CalcularPeso(float distancia, GameObject destino)
    {
        float pesoBase = distancia;
        float costoDestino = destino.CompareTag("PortaAviones") ? 50f : 20f; // Más caro si es portaaviones
        float costoInteroceánico = EsRutaInteroceánica(destino.transform.position) ? 30f : 0f;

        return pesoBase + costoDestino + costoInteroceánico;
    }

    private bool EsRutaInteroceánica(Vector3 posicionDestino)
    {
        // Considera una lógica básica para determinar si la ruta cruza el océano
        return posicionDestino.y > 5f; // Ejemplo: cualquier punto al norte se considera "interoceánico"
    }
}

// Clase para manejar rutas
public class Ruta
{
    public GameObject origen;
    public GameObject destino;
    public float peso;

    public Ruta(GameObject origen, GameObject destino, float peso)
    {
        this.origen = origen;
        this.destino = destino;
        this.peso = peso;
    }
}
