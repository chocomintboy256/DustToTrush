using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UIElements;

public class StepUp : DragCharactor
{
    private GameObject box;
    public bool BonusFg { get; protected set; }
    private enum Costume { Bonus };
    public GameObject Target { get { return target; } set { target = value; } }

    // 生成直後
    void Awake()
    {
        BonusFg = false;

        HP = 0;
        MaxHP = 0;
        atk = 0;
    }
 
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Score = 0;
    }
    // Update is called once per frame
    // protected virtual void Update() { }

    // 生成
    public static DragCharactor Generate(Vector3 position) { 
        GameObject stepUp = Instantiate(GameMain.ins.MasterStepUp, position, Quaternion.identity, GameMain.ins.transform.parent);
        GameMain.ins.stepUp = stepUp.GetComponent<StepUp>();
        GameMain.ins.stepUp.Target = GameMain.ins.boxs[0];
        return stepUp.GetComponent<DragCharactor>();
    }
}
