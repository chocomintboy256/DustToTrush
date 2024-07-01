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
    public int level = 1;
    public int LEVEL_MAX = 99;
    public int exp = 1;
    public List<int> requiredExp = new List<int>() //{ 0, 4, 9, 16, 25, 36, 49, 64, 81, 100, 121, 144, 169, 196, 225, 256, 289, 324, 361, 400, 441, 484, 529, 576, 625, 676, 729, 784, 841, 900, 961, 1024, 1089, 1156, 1225, 1296, 1369, 1444, 1521, 1600, 1681, 1764, 1849, 1936, 2025, 2116, 2209, 2304, 2401, 2500, 2601, 2704, 2809, 2916, 3025, 3136, 3249, 3364, 3481, 3600, 3721, 3844, 3969, 4096, 4225, 4356, 4489, 4624, 4761, 4900, 5041, 5184, 5329, 5476, 5625, 5776, 5929, 6084, 6241, 6400, 6561, 6724, 6889, 7056, 7225, 7396, 7569, 7744, 7921, 8100, 8281, 8464, 8649, 8836, 9025, 9216, 9409, 9604, 9801 };
        {0, 5, 13, 25, 41, 61, 85, 113, 145, 181, 221, 265, 313, 365, 421, 481, 545, 613, 685, 761, 841, 925, 1013, 1105, 1201, 1301, 1405, 1513, 1625, 1741, 1861, 1985, 2113, 2245, 2381, 2521, 2665, 2813, 2965, 3121, 3281, 3445, 3613, 3785, 3961, 4141, 4325, 4513, 4705, 4901, 5101, 5305, 5513, 5725, 5941, 6161, 6385, 6613, 6845, 7081, 7321, 7565, 7813, 8065, 8321, 8581, 8845, 9113, 9385, 9661, 9941, 10225, 10513, 10805, 11101, 11401, 11705, 12013, 12325, 12641, 12961, 13285, 13613, 13945, 14281, 14621, 14965, 15313, 15665, 16021, 16381, 16745, 17113, 17485, 17861, 18241, 18625, 19013, 19405};

    public bool isLevelUp { get { return exp >= requiredExp[level] && level < LEVEL_MAX; } }

    public void LevelUpProc()
    {
        while (isLevelUp) LevelUp();
    }
    void LevelUp()
    {
        level += 1;
        MaxHP += 3 + Random.Range(0, 6);
        atk += 1 + Random.Range(0, 3);
    }
    
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
    public virtual void RecoverHP()
    {
        GetComponent<SpriteEffector>().Flashing();
    }

    protected virtual void OnMouseUp()
    {
        if (GameMain.ins.stepUp?.IsTargetIn == true)        dustManager?.TrashInStepUp();
        else if (GameMain.ins.stepDown?.IsTargetIn == true) dustManager?.TrashInStepDown();
        else                                                dustManager?.TrashInDust();
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
