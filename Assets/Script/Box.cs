using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Box : DragCharactor
{
    [SerializeField] DustManager Dusts;
    [SerializeField] HealthGauge HpGauge;
    private Collider2D col;
    [NonSerialized] public List<DragCharactor> InDusts = new List<DragCharactor>();
    // Start is called before the first frame update
    void Start()
    {
        dustManager = Dusts;
        hpGauge = HpGauge;
        col = GetComponent<Collider2D>();

        HP = 100;
        MaxHP = 100;
        atk = 10;
        hpGauge.AlwaysActiveFlag = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Died()
    {
        gameObject.SetActive(false);
        hpGauge.gameObject.SetActive(false);
    }

    // 当たり判定
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Respawn") return;
        DragCharactor dc = collision.gameObject.GetComponent<DragCharactor>();
        dc.eventTriggerEnter2D.Invoke(col);
        InDusts.Add(dc);
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Respawn") return;
        DragCharactor dc = collision.gameObject.GetComponent<DragCharactor>();
        dc.eventTriggerExit2D.Invoke(col);
        InDusts.Remove(dc);
    }
}
