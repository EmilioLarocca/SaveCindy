using UnityEngine;

public class EndingScore : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;

    public int Score()
    {
        int finalScore = gameManager.score + (gameManager.livesLeft * 10);
        return finalScore;
    }
}
