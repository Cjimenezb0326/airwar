using System.Collections;
using UnityEngine;
using UnityEngine.UI; // Para utilizar la UI

public class Barratiempo : MonoBehaviour
{
    public Image barraCombustible; // Asigna el Image del Slider desde el Inspector
    public float combustibleMaximo = 10f; // Duración total del combustible en segundos
    public float combustibleActual; // Combustible actual
    public GameObject tron; // Referencia al objeto Tron, asigna desde el Inspector

    private bool isFuelDepleting = true; // Controla si el combustible se está consumiendo

    private void Start()
    {
        // Inicializa el combustible al valor máximo
        combustibleActual = combustibleMaximo;

        // Inicia la corrutina que va consumiendo el combustible
        StartCoroutine(ConsumirCombustible());
    }

    private void Update()
    {
        // Actualiza la barra de combustible en función del combustible restante
        barraCombustible.fillAmount = combustibleActual / combustibleMaximo;
    }

    private IEnumerator ConsumirCombustible()
    {
        while (isFuelDepleting && combustibleActual > 0)
        {
            // Reduce el combustible cada segundo
            combustibleActual -= 1f;

            // Actualiza la barra de combustible
            barraCombustible.fillAmount = combustibleActual / combustibleMaximo;

            // Espera un segundo antes de seguir consumiendo
            yield return new WaitForSeconds(1f);
        }

        if (combustibleActual <= 0)
        {
            isFuelDepleting = false;
            Debug.Log("Combustible agotado.");
            // Destruir a Tron
            if (tron != null)
            {
                Destroy(tron);
                Debug.Log("Se acabo el tiempo");
            }
        }
    }

    public void ReiniciarCombustible()
    {
        // Reinicia el combustible al máximo y reinicia la barra de combustible
        isFuelDepleting = true;
        combustibleActual = combustibleMaximo;
        barraCombustible.fillAmount = combustibleActual / combustibleMaximo;

        // Reinicia la corrutina si es necesario
        StopCoroutine(ConsumirCombustible());
        StartCoroutine(ConsumirCombustible());

        Debug.Log("Combustible reiniciado.");
    }
}
