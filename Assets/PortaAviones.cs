using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortaAviones : MonoBehaviour
{
    public GameObject apPrefab; // Prefab de portaaviones
    public int cantidadPortaaviones = 5; // Número de portaaviones

    public Vector2 rangoY = new Vector2(-2f, 9f);  
    public Vector2 rangoX = new Vector2(-9f, 9f); 

    private List<GameObject> portaavionesList = new List<GameObject>(); // Lista de portaaviones generados
    private List<Ruta> rutas = new List<Ruta>(); // Lista de rutas generadas

    private void Start()
    {
        // Crear portaaviones
        for (int i = 0; i < cantidadPortaaviones; i++)
        {
            CrearPortaaviones();
        }

        // Generar rutas entre portaaviones y aeropuertos
        GenerarRutas();
    }

    private void CrearPortaaviones()
    {
        float posX = Random.Range(rangoX.x, rangoX.y);
        float posY = Random.Range(rangoY.x, rangoY.y);

        Vector3 posicionInicial = new Vector3(posX, posY, 0f);

        GameObject nuevoPortaavion = Instantiate(apPrefab, posicionInicial, Quaternion.identity);
        portaavionesList.Add(nuevoPortaavion);
    }

    private void GenerarRutas()
    {
        // Obtener aeropuertos existentes en la escena usando su tag
        GameObject[] aeropuertos = GameObject.FindGameObjectsWithTag("Aeropuertos");

        // Crear rutas entre todos los portaaviones y aeropuertos
        foreach (GameObject portaavion in portaavionesList)
        {
            foreach (GameObject aeropuerto in aeropuertos)
            {
                float distancia = Vector3.Distance(portaavion.transform.position, aeropuerto.transform.position);
                float peso = CalcularPeso(distancia, aeropuerto);

                Ruta nuevaRuta = new Ruta(portaavion, aeropuerto, peso);
                rutas.Add(nuevaRuta);

                Debug.Log($"Ruta generada: {portaavion.name} -> {aeropuerto.name} | Peso: {peso}");
            }
        }
    }

    private float CalcularPeso(float distancia, GameObject destino)
    {
        float pesoBase = distancia;
        float costoDestino = destino.CompareTag("Aeropuertos") ? 20f : 50f; // Menor costo si es aeropuerto
        float costoInteroceánico = EsRutaInteroceánica(distancia) ? 30f : 0f;

        return pesoBase + costoDestino + costoInteroceánico;
    }

    private bool EsRutaInteroceánica(float distancia)
    {
        // Considera una lógica básica para determinar si la ruta cruza el océano
        return distancia > 10f; // Ejemplo: cualquier ruta mayor a 10 unidades se considera interoceánica
    }
}
