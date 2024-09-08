using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Platformer.Gameplay;
using static Platformer.Core.Simulation;
using Platformer.Model;
using Platformer.Core;

namespace Platformer.Mechanics
{
    public class PlayerController : KinematicObject
    {
        public AudioClip jumpAudio;
        public AudioClip respawnAudio;
        public AudioClip ouchAudio;

        public float maxSpeed = 7;
        public float jumpTakeOffSpeed = 7;

        public JumpState jumpState = JumpState.Grounded;
        private bool stopJump;
        public Collider2D collider2d;
        public AudioSource audioSource;
        public Health health;
        public bool controlEnabled = true;

        bool jump;
        Vector2 move;
        SpriteRenderer spriteRenderer;
        internal Animator animator;
        readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

        public Bounds Bounds => collider2d.bounds;

        public GameObject bulletPrefab;
        public Transform firePoint;

        public GameObject bulletPrefab2;
        public Transform firePoint2;

        public GameObject bulletPrefab3;
        public Transform firePoint3;

        public float fireRate = 1f;
        public float timer = 0f;

        Vector3 newPosition;

        void Awake()
        {
            health = GetComponent<Health>();
            audioSource = GetComponent<AudioSource>();
            collider2d = GetComponent<Collider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
        }

        // Use the 'new' keyword to hide the inherited Start() method
        new void Start()
        {
            newPosition = firePoint.localPosition;
        }

        protected override void Update()
        {
            if (controlEnabled)
            {
                move.x = Input.GetAxis("Horizontal");

                if (move.x > 0.01f)
                {
                    firePoint.localEulerAngles = new Vector3(firePoint.localEulerAngles.x, 0, firePoint.localEulerAngles.z);
                    newPosition.x = .5f;
                    firePoint.localPosition = newPosition;
                }

                if (move.x < -0.01f)
                {
                    firePoint.localEulerAngles = new Vector3(firePoint.localEulerAngles.x, 180, firePoint.localEulerAngles.z);
                    newPosition.x = -.5f;
                    firePoint.localPosition = newPosition;
                }

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    animator.SetTrigger("atk");
                    ShootBullet();
                } 
                else if (Input.GetKeyDown(KeyCode.X) && timer >= fireRate)
                {
                    animator.SetTrigger("atk");
                    ShootBullet2();
                    timer = 0f;
                } 
                else if (Input.GetKeyDown(KeyCode.C))
                {
                    animator.SetTrigger("atk");
                    ShootBullet3();
                }

                if (jumpState == JumpState.Grounded && Input.GetButtonDown("Jump"))
                    jumpState = JumpState.PrepareToJump;
                else if (Input.GetButtonUp("Jump"))
                {
                    stopJump = true;
                    Schedule<PlayerStopJump>().player = this;
                }
            }
            else
            {
                move.x = 0;
            }

            timer += Time.deltaTime;

            UpdateJumpState();
            base.Update();
        }

        void UpdateJumpState()
        {
            jump = false;
            switch (jumpState)
            {
                case JumpState.PrepareToJump:
                    jumpState = JumpState.Jumping;
                    jump = true;
                    stopJump = false;
                    break;
                case JumpState.Jumping:
                    if (!IsGrounded)
                    {
                        Schedule<PlayerJumped>().player = this;
                        jumpState = JumpState.InFlight;
                    }
                    break;
                case JumpState.InFlight:
                    if (IsGrounded)
                    {
                        Schedule<PlayerLanded>().player = this;
                        jumpState = JumpState.Landed;
                    }
                    break;
                case JumpState.Landed:
                    jumpState = JumpState.Grounded;
                    break;
            }
        }

        protected override void ComputeVelocity()
        {
            if (jump && IsGrounded)
            {
                velocity.y = jumpTakeOffSpeed * model.jumpModifier;
                jump = false;
            }
            else if (stopJump)
            {
                stopJump = false;
                if (velocity.y > 0)
                {
                    velocity.y = velocity.y * model.jumpDeceleration;
                }
            }

            if (move.x > 0.01f)
                spriteRenderer.flipX = false;
            else if (move.x < -0.01f)
                spriteRenderer.flipX = true;

            animator.SetBool("grounded", IsGrounded);
            animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

            targetVelocity = move * maxSpeed;
        }

        public enum JumpState
        {
            Grounded,
            PrepareToJump,
            Jumping,
            InFlight,
            Landed
        }

        void ShootBullet()
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.speed = 10f;
        }

        void ShootBullet2()
        {
            GameObject bullet = Instantiate(bulletPrefab2, firePoint.position, firePoint.rotation);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.speed = 10f;
        }

        void ShootBullet3()
        {
            GameObject bullet = Instantiate(bulletPrefab3, firePoint.position, firePoint.rotation);
            BulletController bulletController = bullet.GetComponent<BulletController>();
            bulletController.speed = 10f;
        }
    }
}
