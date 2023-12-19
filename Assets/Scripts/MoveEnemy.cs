using System.Collections;
using FMODUnity;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    private GameObject cindy;
    private Transform target;
    private Animator enemyAnim;
    private Rigidbody enemyRb;
    private GameManager gameManager;

    private float speed;
    [SerializeField] private float doubleSpeed = 1350;
    [SerializeField] private float initialSpeed = 1000;
    //[SerializeField] private float initialSpeed = 750;
    float rotationSpeed = 20f;

    private GameObject footsteps;
    private StudioEventEmitter footstepsSounds;
    public ParticleSystem explosionParticle;
    private float yBound = 3;

    void Start()
    {
        enemyAnim = GetComponent<Animator>();
        enemyRb = GetComponent<Rigidbody>();

        footsteps = GameObject.Find("FootstepsSound");
        footstepsSounds = footsteps.GetComponent<StudioEventEmitter>();

        cindy = GameObject.FindWithTag("Cindy");
        target = cindy.transform;

        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        speed = initialSpeed;
    }

    void FixedUpdate()
    {
        if (cindy.activeSelf)
        {
            ChaseCindy();
        }
    }

    void Update()
    {
        if (cindy.activeSelf && !gameManager.paused)
        {
            PlayFootsteps();
        } 
        else
        {
            StopFootsteps();
        }

        DestroyOutOfBounds();
    }

    void ChaseCindy()
    {
        if (gameManager.levelTwo == true)
        {
            speed = doubleSpeed;
        }
        else if (gameManager.levelTwo == false)
        {
            speed = initialSpeed;
        }

        enemyAnim.SetBool("IsWalking", true);
        
        Vector3 lookDirection = (target.position - transform.position).normalized;
        enemyRb.AddForce(lookDirection * speed);

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void PlayFootsteps()
    {
        if (!gameManager.paused && !footstepsSounds.IsPlaying())
        {
            footstepsSounds.Play();
            enemyAnim.SetBool("IsWalking", true);
            enemyAnim.SetBool("GameOver", false);
        }
    }

    public void StopFootsteps()
    {
        if (footstepsSounds.IsPlaying())
        {
            footstepsSounds.Stop();
            enemyAnim.SetBool("IsWalking", false);
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Cindy"))
        {
            enemyAnim.SetBool("GameOver", true);
            enemyAnim.SetBool("IsWalking", false);

            if (footstepsSounds.IsPlaying())
            {
                footstepsSounds.Stop();
            }
        }
    }

    void DestroyOutOfBounds()
    {
        if (transform.position.y < -yBound)
        {
            Destroy(gameObject);
            if (footstepsSounds.IsPlaying())
            {
                footstepsSounds.Stop();
            }
        }
    }
}
