using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    /// <summary> 
    /// This class controls the enemy behavior, making it constantly fire towards the player.
    /// </summary>
    public class EnemyC : KinematicObject
    {
        public AudioClip ouchAudio;
        public float maxSpeed = 3; // Adjust speed as needed
        public Collider2D collider2d;
        public AudioSource audioSource;
        public Health health;

        bool canMove = true;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public GameObject bulletPrefab;
        public Transform firePoint;

        public float fireRate = 1f; // Time between each shot
        private float nextFireTime = 0f; // Time tracking for next fire

        public Transform player; // Reference to the player object to target

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        // Use 'new' keyword to hide the base class method if needed
        protected new void Start()
        {
            // Make sure the player reference is assigned
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }

        // Ensure there's only one Update method
        protected override void Update()
        {
            if (canMove)
            {
                // Look at the player
                if (player != null)
                {
                    Vector2 direction = player.position - transform.position;
                    direction.Normalize();

                    // Update fire point direction
                    firePoint.right = direction;
                }

                // Fire constantly towards the player
                if (Time.time >= nextFireTime)
                {
                    animator.SetTrigger("atk");
                    ShootBullet();
                    nextFireTime = Time.time + fireRate;
                }
            }

            base.Update();
        }

        void ShootBullet()
        {
            // Instantiate a bullet at the fire point position and rotation
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            // Access the BulletController script and set its speed
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.speed = 10f;
        }

        protected override void ComputeVelocity()
        {
            targetVelocity = move * maxSpeed;
        }
    }
}
