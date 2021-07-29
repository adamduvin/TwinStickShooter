using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    private PlayerInput playerInput;

    private Vector3 forward;
    private Vector3 right;

    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float minShootValue = 0f;

    [SerializeField]
    private float bulletVelocity = 50f;

    /*[SerializeField]
    private int bulletsSize = 30;
    private GameObject[] bullets;
    private int bulletsIndex = 0;*/

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private GameObject[] projectileSpawnPoints;
    private int numSpawnPoints;
    private int currentSpawnPoint = 0;

    [SerializeField]
    private float timeBetweenShots = 0f;
    private float timer;

    //[SerializeField]
    private HapticSource gunfireHapticSource;
    private AudioSource gunfireAudioSource;

    [SerializeField]
    private float shootShakeAmplitude = 0.1f;
    [SerializeField]
    private float shootShakeFrequency = 0.1f;
    [SerializeField]
    private float shootShakeDuration = 0.1f;

    private void Awake()
    {
        playerInput = new PlayerInput();

        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;

        gunfireHapticSource = GetComponent<HapticSource>();
        gunfireAudioSource = GetComponent<AudioSource>();

        numSpawnPoints = projectileSpawnPoints.Length;
    }

    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    private void Start()
    {
        /*bullets = new GameObject[bulletsSize];
        for(int i = 0; i < bulletsSize; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab);
            bullet.GetComponent<Projectile>().Shooter = gameObject;
            bullets[i] = bullet;
            bullets[i].SetActive(false);
        }*/
        timer = timeBetweenShots;
    }

    void Update()
    {
        Vector2 lookInput = playerInput.Player.Look.ReadValue<Vector2>();
        Vector3 rightDirection = right * lookInput.x;
        Vector3 forwardDirection = forward * lookInput.y;
        Vector3 direction = rightDirection + forwardDirection;

        if(timer < timeBetweenShots)
        {
            timer += Time.deltaTime;
        }

        if(direction.magnitude > 0f)
        {
            transform.forward = direction;
            if(timer >= timeBetweenShots)
            {
                if (direction.magnitude > minShootValue)
                {
                    GameObject bullet = Instantiate(bulletPrefab);
                    bullet.GetComponent<Projectile>().Shooter = gameObject;
                    bullet.GetComponent<Projectile>().Shoot(projectileSpawnPoints[currentSpawnPoint], transform.forward, bulletVelocity);
                    gunfireHapticSource.Play();
                    gunfireAudioSource.Play();

                    currentSpawnPoint++;
                    if(currentSpawnPoint >= projectileSpawnPoints.Length)
                    {
                        currentSpawnPoint = 0;
                    }

                    if(CameraShake.Instance.GetCurrentShakeAmplitude() <= 0f)
                    {
                        CameraShake.Instance.ShakeCamera(shootShakeAmplitude, shootShakeFrequency, shootShakeDuration);
                    }


                    /*bullets[bulletsIndex].SetActive(true);
                    bullets[bulletsIndex].GetComponent<Projectile>().Shoot(projectileSpawnPoint, transform.forward, bulletVelocity);
                    bulletsIndex++;
                    if (bulletsIndex >= bulletsSize)
                    {
                        bulletsIndex = 0;
                    }*/
                    timer = 0f;
                }
            }
        }
    }
}
