using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Cleaner : DragCharactor
{
    private GameObject box;
    public bool BonusFg { get; protected set; }
    private enum Costume { Bonus };

    // 生成直後
    void Awake()
    {
        dustManager = transform.parent.GetComponent<DustManager>();
        BonusFg = false;
        target = dustManager.Box;

        HP = 10;
        MaxHP = 10;
        atk = -30;
    }
 
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Score = 30;
        dustManager = transform.parent.GetComponent<DustManager>();
    }

    // Update is called once per frame
    // protected virtual void Update() { }
}
