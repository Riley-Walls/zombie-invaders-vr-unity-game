using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieInvader
{
    public class PlayerController : MonoBehaviour
    {
        public ControlType controlType;

        public GameObject bulletPrefab;

        public CascadeColliderRelay collisionRelay;

        private float movementSpeed = .1f;

        private bool bulletWasFired = false;
        private float bulletFireCooldown = .9f;
        private float bulletFireCooldownCounter = 0;

        public GameObject gameOverCanvas;

        public GameObject shotgunParticle;

        public AudioClip shotgunBlastSFX;
        public AudioSource shotGunBlastSource;

        // Start is called before the first frame update
        void Start()
        {
            shotGunBlastSource = GetComponent<AudioSource>();
            shotGunBlastSource.clip = shotgunBlastSFX;
            collisionRelay.OnCollisionEnterAction += PlayerCollided;
        }

        void PlayerCollided(Collider2D collider)
        {
            if (collider.gameObject.GetComponent<BaseInvader>() != null)
            {
                gameOverCanvas.SetActive(true);
            }

        }

        // Update is called once per frame
        void Update()
        {
            //UPDATE BULLET FIRE COOLDOWN
            if (bulletWasFired == true)
            {
                if (bulletFireCooldownCounter >= bulletFireCooldown)
                {
                    bulletWasFired = false;
                    bulletFireCooldownCounter = 0;
                }
                else
                {
                    bulletFireCooldownCounter += Time.deltaTime;
                }
            }


            //CONTROLS
            if (controlType == ControlType.Keyboard)
            {
                if (Input.GetKey(KeyCode.A))
                {
                    transform.Translate(new Vector3(-1 * movementSpeed, 0, 0));
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    transform.Translate(new Vector3(1 * movementSpeed, 0, 0));
                }

                if (Input.GetKey(KeyCode.Space))
                {
                    shotGunBlastSource.PlayOneShot(shotgunBlastSFX, 1);

                    if (!bulletWasFired)
                    {
                        bulletWasFired = true;
                        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                        //newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1);

                    }
                }
            }
            if (controlType == ControlType.Joystick)
            {
                if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x < -.25f)
                {
                    transform.Translate(new Vector3(-1 * movementSpeed, 0, 0));
                }
                else if (OVRInput.Get(OVRInput.Axis2D.PrimaryThumbstick).x > .25f)
                {
                    transform.Translate(new Vector3(1 * movementSpeed, 0, 0));
                }

                if (OVRInput.GetDown(OVRInput.Button.One))
                {
                    shotGunBlastSource.PlayOneShot(shotgunBlastSFX, 1);

                    if (!bulletWasFired)
                    {
                        bulletWasFired = true;
                        GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
                        //newBullet.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 1);

                    }
                }
            }

            if (OVRInput.GetDown(OVRInput.Button.One))
            {
                Debug.Log("hey!");
            }


        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Player hit!!");
        }

    }

    public enum ControlType
    {
        Keyboard,
        Joystick,
        Hands
    }

}




