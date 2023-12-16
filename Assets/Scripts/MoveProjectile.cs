using System.Collections;
using UnityEngine;
using FMODUnity;

public class MoveProjectile : MonoBehaviour
{
    private GameManager gameManager;
    private Collider bulletCollider;
    public ParticleSystem enemyExplosion;
    public ParticleSystem cindyExplosion;
    private StudioEventEmitter enemyDeathSound;
    private StudioEventEmitter cindyDeathSound;

    [SerializeField] private float speed = 50;
    private float lowerBound = 3;
    private float collisionEnableDelay = 0.1f; 

    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();

        enemyDeathSound = GameObject.Find("EnemyDeath").GetComponent<StudioEventEmitter>();
        cindyDeathSound = GameObject.Find("CindyDeath").GetComponent<StudioEventEmitter>();

        bulletCollider = GetComponent<Collider>();
        bulletCollider.enabled = false; 

        StartCoroutine(EnableCollisionAfterDelay(collisionEnableDelay));
    }

    void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * speed);
        
        if (transform.position.y < -lowerBound)
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator EnableCollisionAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        bulletCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            gameObject.SetActive(false);
            Destroy(other.gameObject);

            gameManager.UpdateScore(1);
            Instantiate(enemyExplosion, other.transform.position, other.transform.rotation);
            enemyDeathSound.Play();

            if (gameManager.levelTwo == true)
            {
                gameManager.HiddenScoreLV2(1);
            }
        }

        if (other.CompareTag("Cindy"))
        {
            gameObject.SetActive(false);
            other.gameObject.SetActive(false);
            
            Instantiate(cindyExplosion, other.transform.position, other.transform.rotation);
            cindyDeathSound.Play();

            gameManager.isCindyDead = true;
            gameManager.EndGameBehaviour();
        }
        
        else
        {
            gameObject.SetActive(false);
        }

        //Debug.Log($"Collided with {other.gameObject.name} (Tag: {other.tag})");
        //Debug.Log("Collision at " + other.transform.position);
    }
}
