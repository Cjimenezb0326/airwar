using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movegun : MonoBehaviour
{
    public float velocidad = 0.02f;
    private Vector2 lastPosition;
    private bool movingRight = true;
    private bool balaDisparada = false;
    public GameObject balaPrefab;
    private float tiempoCargado = 0f;
    public float velocidadBaseBala = 5f;
    

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        float posX = transform.position.x;

        if (Input.GetKey(KeyCode.Space))
        {
            tiempoCargado += Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            balaDisparada = true;
        }

        if (posX >= 10)
        {
            movingRight = false;
        }
        else if (posX <= -10)
        {
            movingRight = true;
        }

        if (movingRight)
        {
            transform.position = new Vector2(transform.position.x + velocidad, transform.position.y);
        }
        else
        {
            transform.position = new Vector2(transform.position.x - velocidad, transform.position.y);
        }

        lastPosition = transform.position;

        if (balaDisparada)
        {
            DispararBala();
            balaDisparada = false;
        }
    }

    private void DispararBala()
    {
        if (balaPrefab != null)
        {
            GameObject bala = Instantiate(balaPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bala.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                float velocidadBala = velocidadBaseBala + tiempoCargado * 5f;
                rb.velocity = new Vector2(0, velocidadBala);
            }

            tiempoCargado = 0f;
            StartCoroutine(DestroyAfterTime(bala, 3f));
        }
    }

    private IEnumerator DestroyAfterTime(GameObject bala, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        Destroy(bala);
    }
}
