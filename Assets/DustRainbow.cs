using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustRainbow : Dust
{
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        Score *= 300;
        BonusFg = true;
    }

    // Update is called once per frame
    // protected override void Update() { base.Update(); }
}
