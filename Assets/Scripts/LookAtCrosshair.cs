using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCrosshair : MonoBehaviour
{    
    public Camera fpsCam;
    public GameObject bullet;
    private Vector3 targetPoint;
    
    [SerializeField] private GameObject shootingPoint;
    [SerializeField] private GameObject gun;
    [SerializeField] private Transform reticle;

    private float distance = 50;

    void FixedUpdate()
    {
        Ray rayOrigin = fpsCam.ScreenPointToRay(new Vector2(reticle.position.x, reticle.position.y));

        RaycastHit hit;

        if (Physics.Raycast(rayOrigin, out hit, distance))
        {
            targetPoint = hit.point;

            // Rotate the gun towards the target point
            Vector3 direction = (targetPoint - shootingPoint.transform.position).normalized;
            gun.transform.rotation = Quaternion.LookRotation(direction);

            //Debug.DrawRay(rayOrigin.origin, rayOrigin.direction * hit.distance, Color.red);
            //Debug.DrawRay(rayOrigin.origin, rayOrigin.direction * distance, Color.blue);
        }
    }

    public Vector3 GetTargetPoint()
    {
        return targetPoint;
    }
}