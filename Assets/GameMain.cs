using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameMain : MonoBehaviour
{
    GameObject go;
    public GameObject box;
    public Text scoreText;
    public Text timeText;
    int totalScore = 0;
    int timeRemining = 60;
    
    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + totalScore.ToString();
        GameObject.FindWithTag("Respawn").GetComponent<Dust>().ResetPosition();

        // get hierarchy objects
        Scene scene = SceneManager.GetSceneByBuildIndex(0);
        GameObject[] rootObjects = scene.GetRootGameObjects();
        foreach (GameObject obj in rootObjects)
        {
            Debug.Log(obj.name);
        }
        TimerUpdate();
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void TimerUpdate()
    {
        timeRemining--;
        timeText.text = "Time: " + timeRemining;
        Invoke("TimerUpdate", 1.0f);
    }
    public void TrushIn(int trushScore)
    {
        scoreText.text = "Score: " + (totalScore += trushScore).ToString();
    }
}
