using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameMain : MonoBehaviour
{
    GameObject go;
    public GameObject box;
    public Text scoreText;
    int totalScore = 0;

    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "Score: " + totalScore.ToString();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TrushIn(int trushScore)
    {
        scoreText.text = "Score: " + (totalScore += trushScore).ToString();
    }
}
