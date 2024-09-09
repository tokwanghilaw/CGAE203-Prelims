using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBulletController : MonoBehaviour
{
    public float speed = 10f;

    void Start()
    {
        Destroy(gameObject, 5f);
    }

    void Update()
    {
        // Move the bullet horizontally
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }

    /*
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Destroy the player on collision
        if (collision.gameObjects.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
        }

        if (collision.CompareTag("Untagged"))
        {
            Destroy(gameObject);
        }
    }
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
             Destroy(other.gameObject);
            

        }
    }
}
