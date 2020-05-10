using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Dust : MonoBehaviour
{
    GameMain gameMain;
    Transform dusts;
    Collision box;
    int score = 15;
    bool isTrushIn;

    // Start is called before the first frame update
    void Start()
    {
        gameMain = GameObject.Find("Main Camera").GetComponent<GameMain>();
        dusts = GameObject.Find("dusts").transform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseUp()
    {
        if (isTrushIn)
        {
            gameMain.TrushIn(score);
            if (dusts.childCount == 1) {
                GenerateDusts();
            }
            Destroy(this.gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameMain.box != collision.gameObject) return;
        isTrushIn = true;
    }
    void OnTriggernExit2D(Collider2D collision)
    {
        if (gameMain.box != collision.gameObject) return;
        isTrushIn = false;
    }

    void GenerateDusts()
    {
        float x, y;
        string name;
        int count = Random.Range(3, 5);
        int index = int.Parse(gameObject.name.Replace("dust", ""));
        for (int i = 0; i < count; i++) {
            x = Random.Range(-2.3f, 2.3f);
            y = Random.Range(-2.8f, 2.8f);
            name = "dust" + (i + index).ToString();

            GameObject newDust = Instantiate(gameObject, new Vector3(x, y, 0), Quaternion.identity);
            newDust.name = name;
            newDust.transform.parent = dusts;
        }
    }
}
