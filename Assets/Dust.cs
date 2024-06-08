using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;

public class Dust : MonoBehaviour
{
    private GameObject box;
    [SerializeField] public int Score { get; protected set; }
    public bool IsTrushIn { get; protected set; }
    public bool BonusFg { get; protected set; }
    private enum Costume { Bonus };
    private Dusts dusts;


    // 生成直後
    void Awalk()
    {
        BonusFg = false;
    }
 
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Score = 15;
        dusts = transform.parent.GetComponent<Dusts>();
    }

    // Update is called once per frame
    // protected virtual void Update() { }

    void OnMouseUp()
    {
        dusts.TrashInDustDestory();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (dusts.Box != collision.gameObject) return;
        IsTrushIn = true;
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (dusts.Box != collision.gameObject) return;
        IsTrushIn = false;
    }
}
