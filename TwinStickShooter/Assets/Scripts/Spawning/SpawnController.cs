using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpawnController : MonoBehaviour
{
    //[SerializeField]
    //private ScriptableObject mission;             Will be implemented later

    [SerializeField]
    private Spawner[] spawners;                     // This will probably be hidden later when I don't have to hard code waves
    [SerializeField]
    private int numWaves;
    private int currentWave;
    [SerializeField]
    private int[] waveSizes;
    [SerializeField]
    private float[] waveCountdowns;
    [SerializeField]
    private int enemiesLeft;

    [SerializeField]
    private Text countdownText;
    private float countdown;

    // Start is called before the first frame update
    void Start()
    {
        currentWave = -1;
        enemiesLeft = 0;
        countdown = 0f;
        HandleEnemyDeath();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public delegate void NewRoundAction();
    public event NewRoundAction StartNewRound;

    public void SetupWaves()
    {
        // Will be implemented later. Gives the waves to the spawners
    }

    public void HandleEnemyDeath()
    {
        enemiesLeft--;
        if(enemiesLeft <= 0)
        {
            currentWave++;
            if(currentWave < waveCountdowns.Length)
            {
                enemiesLeft = waveSizes[currentWave];
                countdown = waveCountdowns[currentWave];
                countdownText.text = "Next Wave in:\n" + countdown.ToString();
                Color color = countdownText.color;
                color.a = 1.0f;
                countdownText.color = color;
                StartCoroutine("NewRoundCountdown");
            }
            else
            {
                // End level
                Debug.Log("Player Wins");
            }
        }
    }

    IEnumerator NewRoundCountdown()
    {
        while(countdown > 0f)
        {
            countdown -= Time.deltaTime;
            countdownText.text = "Next Wave in:\n" + countdown.ToString();
            yield return null;
        }
        Color color = countdownText.color;
        color.a = 0.0f;
        countdownText.color = color;
        StartNewRound();
    }
}
