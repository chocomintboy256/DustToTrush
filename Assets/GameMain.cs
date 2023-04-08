using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor.PackageManager.UI;

public class GameMain : MonoBehaviour
{
    GameObject go;
    public GameObject box;
    public Text scoreText;
    public Text timeText;
    int totalScore = 0;
    int timeRemining = 60;
    int timeBonus = 5;
    public static GameMain _ins = null;
    public static GameMain ins
    {
        get { return _ins; }
        set { if (_ins == null) _ins = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        ins = this;
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
        RefleshTimer();
        Invoke("TimerUpdate", 1.0f);
    }
    public void RefleshTimer()
    { 
        timeRemining--;
        timeText.text = "Time: " + timeRemining;
    }
    public void AddTimeBonus()
    {
        timeRemining += timeBonus;
        RefleshTimer();
    }
    public void TrushIn(int trushScore)
    {
        scoreText.text = "Score: " + (totalScore += trushScore).ToString();
    }
}
