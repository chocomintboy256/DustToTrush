using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustRainbowReguler : Dust
{
    public DustRainbowReguler()
    {
        BonusScore = 100;
    }
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Score *= BonusScore;
        BonusFg = true;
    }

    // Update is called once per frame
    // protected override void Update() { base.Update(); }
}
