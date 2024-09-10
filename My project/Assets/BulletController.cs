using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed = 10f;

    void Update()
    {
        // Move the bullet horizontally
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        // Destroy the bullet if it goes off-screen
        //if (!GetComponent<Renderer>().isVisible)
        //{
          //  Destroy(gameObject);
        //}
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Terrain"))
        {
            Destroy(gameObject);
        }
    }
}