using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class DustType2 : DragCharactor
{
    private GameObject box;
    public bool BonusFg { get; protected set; }
    private enum Costume { Bonus };

    // 生成直後
    void Awake()
    {
        dustManager = transform.parent.GetComponent<DustManager>();
        BonusFg = false;

        MaxHP = 30;
        HP = MaxHP;
        atk = 10;
    }
 
    // Start is called before the first frame update
    protected virtual void Start()
    {
        target = GameMain.ins.boxs[0];
        Score = 20;
        dustManager = transform.parent.GetComponent<DustManager>();
    }

    // Update is called once per frame
    // protected virtual void Update() { }
}
