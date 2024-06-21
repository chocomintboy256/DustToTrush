using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections.Generic;

public class GameMain : MonoBehaviour
{
    GameObject go;
    public const int UNIT_SIZE = 32;
    public const float DUST_ENTER_FIELD_PADDING_UNIT = 1.0f;
    public List<GameObject> boxs;
    public GameObject MainCanvas;
    public GameObject hpGauge;
    public GameObject hpCanvas;
    public GameObject ScoreDisplayGo;
    public GameObject MasterStepUp;
    public GameObject MasterStepDown;
    [NonSerialized] public StepUp stepUp;
    [NonSerialized] public StepDown stepDown;
    public Text scoreText;
    public Text timeText;
    int totalScore = 0;
    int timeRemining = 60;
    int timeBonus = 5;
    [NonSerialized] public Party party;
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
        UnityEngine.Random.InitState(DateTime.Now.Millisecond);
        party = new Party();

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
    public void RefleshScore()
    {
        scoreText.text = "Score: " + totalScore.ToString();
    }
    public void AddTimeBonus()
    {
        timeRemining += timeBonus;
        RefleshTimer();
    }
    public void TrushIn(int trushScore, Vector3 pos)
    {
        totalScore += trushScore;
        GameObject sdgo = Instantiate(ScoreDisplayGo, pos, Quaternion.identity, MainCanvas.transform);
        ScoreDisplay sd = sdgo.GetComponent<ScoreDisplay>();
        sd.SetPos(pos);
        sd.SetScore(trushScore);

        RefleshScore();
    }
}
