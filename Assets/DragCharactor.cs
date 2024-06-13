using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DragCharactor : MonoBehaviour
{
    [HideInInspector] protected DustManager dustManager;
    public HealthGauge hpGauge { get; set; }
    protected GameObject target;
    public bool IsTargetIn { get; protected set; }
    [HideInInspector] public UnityEvent<Collider2D> eventTriggerEnter2D;
    [HideInInspector] public UnityEvent<Collider2D> eventTriggerExit2D;
    [SerializeField] public int Score { get; protected set; }
    [SerializeField] public int BonusScore { get; protected set; } = 0;
 
    public int HP = 30;
    public int MaxHP = 30;
    public int atk = 10;
    // Start is called before the first frame update
    void Start()
    {
        eventTriggerEnter2D.AddListener(OnTriggerEnter2D);
        eventTriggerExit2D.AddListener(OnTriggerExit2D);
        hpGauge?.SetGauge((float)MaxHP/(float)HP);
    }
    void OnDestory()
    {
        eventTriggerEnter2D.RemoveAllListeners();
        eventTriggerExit2D.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseUp()
    {
        dustManager.TrashInDustDestory();
    }
    public void DestroyReference()
    {
        if(hpGauge) Destroy(hpGauge.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (target != collision.gameObject) return;
        IsTargetIn = true;
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (target  != collision.gameObject) return;
        IsTargetIn = false;
    }
}
