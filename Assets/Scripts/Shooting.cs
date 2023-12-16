using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using UnityEngine.Pool;

public class Shooting : MonoBehaviour
{
    public GameObject projectilePrefab;
    private GameObject shootingPoint;
    private GameObject mainCam;
    private GameManager gameManager;
    private LookAtCrosshair followCrosshair;
    private StudioEventEmitter shootingSound;

    void Start()
    {
        shootingPoint = GameObject.Find("ShootingPoint");
        mainCam = GameObject.Find("Main Camera");
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        followCrosshair = mainCam.GetComponent<LookAtCrosshair>();
        shootingSound = GetComponentInChildren<StudioEventEmitter>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && !gameManager.stopGame)
        {
            FireBullet();
            shootingSound.Play();
        }
    }

    void FireBullet()
    {
        GameObject pooledProjectile = ObjectPooler.SharedInstance.GetPooledObject();
        if (pooledProjectile != null)
        {
            // Get the target point at the moment of shooting
            Vector3 targetPoint = followCrosshair.GetTargetPoint();

            // Set the position of the projectile to the target point
            pooledProjectile.transform.position = shootingPoint.transform.position;

            // Calculate the direction from the shooting point to the target point
            Vector3 direction = (targetPoint - shootingPoint.transform.position).normalized;

            // Set the rotation of the projectile towards the target point
            pooledProjectile.transform.rotation = Quaternion.LookRotation(direction);

            // Activate the projectile
            pooledProjectile.SetActive(true);
        }
    }
}