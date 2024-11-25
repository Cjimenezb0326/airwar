using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class avion : MonoBehaviour
{
    public GameObject avionPrefab;
    public int cantidadAviones = 5;
    public float velocidadMovimiento = 2f;
    public float rangoY = 5f;
    public Vector2 rangoX = new Vector2(-10f, 10f);

    private List<GameObject> avionesActivos = new List<GameObject>();
    private List<AvionAI> avionesDerribados = new List<AvionAI>(); // Lista para aviones derribados

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
                    // Cuando el avión se destruye, agregamos a la lista de derribados
                    AvionAI avionAI = avion.GetComponent<AvionAI>();
                    avionesDerribados.Add(avionAI);
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
        AvionAI avionAI = nuevoAvion.AddComponent<AvionAI>(); // Agregar módulo AI al avión
        avionesActivos.Add(nuevoAvion);

        // Iniciar la destrucción por tiempo
        float[] tiemposPermitidos = { 3f, 10f, 5f, 7f, 8f };
        float tiempoDestruccion = tiemposPermitidos[Random.Range(0, tiemposPermitidos.Length)];
        StartCoroutine(DestruirPorTiempo(nuevoAvion, tiempoDestruccion));
    }

    public void DestruirAvion(GameObject avion)
    {
        AvionAI avionAI = avion.GetComponent<AvionAI>();
        avionesDerribados.Add(avionAI);
        Destroy(avion);
        avionesActivos.Remove(avion);
        CrearAvion();
    }

    // Detectar colisión con objetos etiquetados como "bala"
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("bala"))
        {
            Debug.Log("Avión destruido por bala");
            avionesActivos.Remove(gameObject);
            Destroy(gameObject); // Destruir este avión
            Destroy(collision.gameObject); // Destruir la bala
            CrearAvion();
        }
    }

    private IEnumerator DestruirPorTiempo(GameObject avion, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);

        if (avion != null && avionesActivos.Contains(avion)) // Verifica que el avión sigue activo
        {
            Debug.Log("Avión destruido por cortina de tiempo");
            avionesActivos.Remove(avion);
            Destroy(avion);
            CrearAvion();
        }
    }

    public List<AvionAI> ObtenerAvionesDerribadosOrdenados(string criterio)
    {
        // Ordenar los aviones derribados con Selection Sort basado en el criterio
        SelectionSort(avionesDerribados, criterio);
        return avionesDerribados;
    }

    // Método de Selection Sort
    private void SelectionSort(List<AvionAI> lista, string criterio)
    {
        for (int i = 0; i < lista.Count - 1; i++)
        {
            int indiceMinimo = i;
            for (int j = i + 1; j < lista.Count; j++)
            {
                bool debeCambiar = false;

                switch (criterio.ToLower())
                {
                    case "id":
                        debeCambiar = string.Compare(lista[j].ID, lista[indiceMinimo].ID) < 0;
                        break;
                    case "rol":
                        debeCambiar = string.Compare(lista[j].Rol, lista[indiceMinimo].Rol) < 0;
                        break;
                    case "horas":
                        debeCambiar = lista[j].HorasDeVuelo < lista[indiceMinimo].HorasDeVuelo;
                        break;
                }

                if (debeCambiar)
                {
                    indiceMinimo = j;
                }
            }

            // Intercambiar
            AvionAI temp = lista[i];
            lista[i] = lista[indiceMinimo];
            lista[indiceMinimo] = temp;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Verificar si la colisión es con un avión
        AvionAI avionAI = collision.gameObject.GetComponent<AvionAI>();
        if (avionAI != null)
        {
            // Imprimir la información del avión en la consola
            Debug.Log("Información del avión que colisionó:");
            Debug.Log("ID: " + avionAI.ID);
            Debug.Log("Rol: " + avionAI.Rol);
            Debug.Log("Horas de Vuelo: " + avionAI.HorasDeVuelo);
        }
    }
}

public class AvionAI : MonoBehaviour
{
    public string ID;
    public string Rol;
    public int HorasDeVuelo;

    private Pilot pilot;
    private Copilot copilot;
    private Maintenance maintenance;
    private SpaceAwareness spaceAwareness;

    private void Start()
    {
        ID = GenerateRandomID();
        Rol = "Piloto"; // Asignamos el rol inicial como Piloto
        HorasDeVuelo = Random.Range(100, 1000); // Horas de vuelo aleatorias

        pilot = new Pilot();
        copilot = new Copilot();
        maintenance = new Maintenance();
        spaceAwareness = new SpaceAwareness();

        // Imprimir la información del avión cuando se crea
        Debug.Log("Información del avión creado: ID: " + ID + ", Rol: " + Rol + ", Horas de Vuelo: " + HorasDeVuelo);
    }

    private string GenerateRandomID()
    {
        char[] chars = new char[3];
        for (int i = 0; i < 3; i++)
        {
            chars[i] = (char)Random.Range(65, 91); // Genera letras aleatorias entre A y Z
        }
        return new string(chars);
    }
}

public class Pilot
{
    public void ControlarAvion() { }
}

public class Copilot
{
    public void TomarControl() { }
}

public class Maintenance
{
    public void RevisarAvion() { }
}

public class SpaceAwareness
{
    public void Monitorear() { }
}
