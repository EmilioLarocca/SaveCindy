using UnityEngine;
using FMODUnity;

public class MoveCindy : MonoBehaviour
{
    public GameObject target;
    private Rigidbody cindyRb;
    [SerializeField] private GameManager gameManager;
    private GameObject wall;

    public ParticleSystem explosionParticle;
    private StudioEventEmitter bounceSound;
    private StudioEventEmitter deathSound;
    private StudioEventEmitter teleportSound;

    private float speed;
    [SerializeField] private float doubleSpeed = 9;
    [SerializeField] private float initialSpeed = 7;
    //[SerializeField] private float initialSpeed = 5;
    private float bounceForce = 3000f;
    public float spawnRangeX = 17;
    public float spawnRangeZ = 12;
    private float xBound = 30;
    private float yBound = 2;

    private void Start()
    {
        cindyRb = GetComponent<Rigidbody>();
        wall = GameObject.FindWithTag("Wall");

        bounceSound = GameObject.Find("BounceSound").GetComponent<StudioEventEmitter>();
        deathSound = GameObject.Find("DeathSound").GetComponent<StudioEventEmitter>();
        teleportSound = GameObject.Find("TeleportSound").GetComponent<StudioEventEmitter>();

        GenerateSpawnPosition();
        speed = initialSpeed;
    }

    private void FixedUpdate()
    {
        if (gameManager.levelTwo == true)
        {
            speed = doubleSpeed;
        }
        else if (gameManager.levelTwo == false)
        {
            speed = initialSpeed;
        }

        Vector3 lookDirection = (target.transform.position - transform.position).normalized;
        cindyRb.AddForce(-lookDirection * speed);

        DestroyOutOfBounds();
    }

    void Bounce()
    {
        Vector3 wallDirection = (wall.transform.position - transform.position).normalized;
        cindyRb.AddForce(-wallDirection * bounceForce);
        bounceSound.Play();
    }

    public void GenerateSpawnPosition()
    {
        float spawnPosX = Random.Range(-spawnRangeX, spawnRangeX);
        float spawnPosZ = Random.Range(-spawnRangeZ, spawnRangeZ);

        Vector3 randomPos = new Vector3(spawnPosX, yBound, spawnPosZ);

        transform.position = randomPos;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            Bounce();
        }

        if (other.gameObject.CompareTag("EmptyCorner"))
        {
            Bounce();
        }

        if (other.gameObject.CompareTag("Teleport"))
        {
            Instantiate(explosionParticle, transform.position, transform.rotation);
            GenerateSpawnPosition();
            teleportSound.Play();
        }

        if (other.gameObject.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            deathSound.Play();

            Instantiate(explosionParticle, transform.position, transform.rotation);
            gameManager.isCindyDead = true;
            gameManager.EndGameBehaviour();
        }
    }

    void DestroyOutOfBounds()
    {
        if (transform.position.x < -xBound || 
            transform.position.x > xBound)
        {
            gameObject.SetActive(false);
            gameManager.isCindyDead = true;
            gameManager.EndGameBehaviour();
        }
    }
}
