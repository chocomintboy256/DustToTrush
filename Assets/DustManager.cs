using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DustManager : MonoBehaviour
{
    enum BONUS_TYPE { REGULER, BIG, GRAPE };
    const int REGULER_BONUS_APPEAR_RATE = 255;
    const int BIG_BONUS_APPEAR_RATE = 255;
    const int GRAPE_BONUS_APPEAR_RATE = 6;
    [SerializeField] GameObject normalDust;
    [SerializeField] GameObject dustCleaner;
    [SerializeField] GameObject rainbowDustReguler;
    [SerializeField] GameObject rainbowDustBig;
    [SerializeField] GameObject mainCamera;
    [SerializeField] GameObject box;
    public GameObject Box { get { return box; } private set {} }
    private List<DragCharactor> dusts = new List<DragCharactor>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            dusts.Add(child.GetComponent<DragCharactor>());
            ResetPosition(child); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TrashInDustDestory()
    {
        List<DragCharactor> trushInDusts = dusts.Where(x => x.IsTargetIn).ToList();
        trushInDusts.ForEach((DragCharactor x) => {
            var bx = box.GetComponent<Box>();

            x.HP = Mathf.Min(x.MaxHP, Mathf.Max(0, x.HP - bx.atk));
            x.hpGauge?.SetGauge((float)x.HP / (float)x.MaxHP);

            bx.HP = Mathf.Min(bx.MaxHP, Mathf.Max(0, bx.HP - x.atk));
            bx.hpGauge?.SetGauge((float)bx.HP / (float)bx.MaxHP);

            if (x.HP == 0) DustDestroy(x);
            if (bx.HP == 0) bx.Died();
         });
    }

    void DustDestroy(DragCharactor dust) {
        GameMain gameMain = mainCamera.GetComponent<GameMain>();
        gameMain.TrushIn(dust.Score, dust.transform.position);
        if (dusts.Count == 1) {
            GenerateDusts();
            GameMain.ins.AddTimeBonus();
        }
        dust.DestroyReference();
        dusts.Remove(dust);
        Destroy(dust.gameObject);
    }

    void GenerateDusts()
    {
        int count = Random.Range(3, 5);
        for (int i = 0; i < count; i++)
        {
            DragCharactor dust = CreateDust();
            dusts.Add(dust);
        }
    }
    DragCharactor CreateDust()
    {
        int nextBonusType = getNextDustBonus();
        GameObject nextGameObject = nextBonusType == (int)BONUS_TYPE.REGULER ? rainbowDustReguler :
                                    nextBonusType == (int)BONUS_TYPE.BIG     ? rainbowDustBig : 
                                    nextBonusType == (int)BONUS_TYPE.GRAPE   ? dustCleaner : normalDust;
        GameObject newDust = Instantiate(nextGameObject, GetRandomVector3(), Quaternion.identity, transform);
        HealthGauge.CreateGauge(newDust);
        return newDust.GetComponent<DragCharactor>();
    }
    int getNextDustBonus()
    {
        var abs = Mathf.Abs(REGULER_BONUS_APPEAR_RATE - BIG_BONUS_APPEAR_RATE);
        var lottery = Mathf.Min(REGULER_BONUS_APPEAR_RATE, BIG_BONUS_APPEAR_RATE) + (abs / 2);
        int result = Random.Range(0, lottery - 1);
        if (result >= (int)BONUS_TYPE.GRAPE)
        {
            result = result % GRAPE_BONUS_APPEAR_RATE == 0 ? (int)BONUS_TYPE.GRAPE : result;
        }
        return result;
    }

    Vector3 GetRandomVector3()
    {
        Vector3 vec = new Vector3(
            Random.Range(-2.3f, 2.3f),
            Random.Range(-2.8f, 2.8f),
            0
        );
        return vec;
    }

    public void ResetPosition(Transform tr)
    {
        tr.position = GetRandomVector3();
    }


}
