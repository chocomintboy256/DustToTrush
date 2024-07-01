using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DustManager : MonoBehaviour
{
    enum BONUS_TYPE { REGULER, BIG, GRAPE };
    enum NORMAL_TYPE { NORMAL, TYPE2 };
    const int REGULER_BONUS_APPEAR_RATE = 255;
    const int BIG_BONUS_APPEAR_RATE = 255;
    const int GRAPE_BONUS_APPEAR_RATE = 6;
    const int TYPE2_NORMAL_APPEAR_RATE = 6;
    [SerializeField] GameObject normalDust;
    [SerializeField] GameObject normalDustType2;
    [SerializeField] GameObject dustCleaner;
    [SerializeField] GameObject rainbowDustReguler;
    [SerializeField] GameObject rainbowDustBig;
    [SerializeField] GameObject mainCamera;
    private List<DragCharactor> dusts = new List<DragCharactor>();
    private bool isFirstCreanAll = true;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
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
    public void TrashInStepUp() {
        // 上の階へ移動
        GameMain.ins.Floor = GameMain.ins.Floor - 1;
        // 今のゴミを削除して終了
        DestroyAllDusts();
    }
    public void TrashInStepDown() {
        // 下の階へ移動
        GameMain.ins.Floor = GameMain.ins.Floor + 1;
        // 今のゴミを削除して次のゴミを出す
        DestroyAllDusts();
        GenerateDustsWithSteps();
    }
    // ゴミ箱に入ってるゴミがあれば削除する
    public async void TrashInDust()
    {
        await TrushInDustDestory();
        GenerateDustNext();
    }
    // 次のゴミを再生成
    void GenerateDustNext() {
        // NextDusts
        if (dusts.Count == 0) {
            if (isFirstCreanAll) {
                GenerateDustsWithSteps();
                isFirstCreanAll = false;
            }
            // GameMain.ins.AddFloorBonus();
        }
    }


    // 入ってるゴミを削除
    async UniTask TrushInDustDestory()
    {
        foreach (GameObject box in GameMain.ins.boxs.ToList())
        {
            Box bx = box.GetComponent<Box>();
            foreach (DragCharactor x in bx.InDusts.ToList())
            {
                BattleAnimation battleAnim = new BattleAnimation(x, bx);
                // ゴミとボックスを衝突: アニメーション
                if (x.atk >= 0) await battleAnim.ClashPlay();
                else bx.RecoverHP();

                // ゴミとボックスを衝突: 計算
                x.HP = Mathf.Min(x.MaxHP, Mathf.Max(0, x.HP - bx.atk));
                x.hpGauge?.SetGauge((float)x.HP / (float)x.MaxHP);

                bx.HP = Mathf.Min(bx.MaxHP, Mathf.Max(0, bx.HP - x.atk));
                bx.hpGauge?.SetGauge((float)bx.HP / (float)bx.MaxHP);

                if (bx.HP == 0) bx.Died();
                else if (battleAnim.IsClashPlay) _ = battleAnim.AddRebound(bx.transform, sign: true, boundHalf: (x.HP <= 0));

                if (x.HP == 0)
                {
                    GameMain.ins.boxs.ForEach((GameObject box2) => { box2.GetComponent<Box>().InDusts.Remove(x); });
                    bx.exp += x.exp;
                    DustDestroy(x);
                    bx.LevelUpProc();
                } else if (battleAnim.IsClashPlay) _ = battleAnim.AddRebound(x.transform, sign: false);

                // ゴミとボックス衝突後の跳ね返り: アニメーション
                await battleAnim.ReboundPlay();
            }
        }
    }

    void DustDestroy(DragCharactor dust, bool isTrushIn = true) {
        GameMain gameMain = mainCamera.GetComponent<GameMain>();
        if (isTrushIn) gameMain.TrushIn(dust.Score, dust.transform.position);
        dust.DestroyReference();
        dusts.Remove(dust);
        Destroy(dust.gameObject);
    }

    void DestroyAllDusts()
    {
        dusts.ToList().ForEach((DragCharactor x) => { DustDestroy(x, isTrushIn: false); });
        Destroy(GameMain.ins.stepUp.gameObject); 
        Destroy(GameMain.ins.stepDown.gameObject); 
    }

    void GenerateDusts()
    {
        int count = UnityEngine.Random.Range(3, 5);
        for (int i = 0; i < count; i++)
        {
            DragCharactor dust = CreateDust();
            dusts.Add(dust);
        }
    }
    List<DragCharactor> GenerateDustsWithSteps()
    {
        GenerateDusts();
        
        return new List<DragCharactor> {
            StepUp.Generate(position: GetRandomPositionInEnterField()),
            StepDown.Generate(position: GetRandomPositionInEnterField())
        };
    }
    DragCharactor CreateDust()
    {
        int nextBonusType = getNextDustBonus();
        int nextNormalType = getNextDustNormal();
        GameObject nextGameObject = nextBonusType == (int)BONUS_TYPE.REGULER ? rainbowDustReguler :
                                    nextBonusType == (int)BONUS_TYPE.BIG     ? rainbowDustBig : 
                                    nextBonusType == (int)BONUS_TYPE.GRAPE   ? dustCleaner : 
                                    nextNormalType == (int)NORMAL_TYPE.TYPE2  ? normalDustType2 : normalDust;
        GameObject newDust = Instantiate(nextGameObject, GetRandomPositionInEnterField(), Quaternion.identity, transform);
        HealthGauge.CreateGauge(newDust);
        return newDust.GetComponent<DragCharactor>();
    }
    int getNextDustNormal()
    {
        int result = UnityEngine.Random.Range(0, TYPE2_NORMAL_APPEAR_RATE-1);
        if (result != (int)NORMAL_TYPE.TYPE2) { result = (int)NORMAL_TYPE.NORMAL; }
        return result;
    }
    int getNextDustBonus()
    {
        var abs = Mathf.Abs(REGULER_BONUS_APPEAR_RATE - BIG_BONUS_APPEAR_RATE);
        var lottery = Mathf.Min(REGULER_BONUS_APPEAR_RATE, BIG_BONUS_APPEAR_RATE) + (abs / 2);
        int result = UnityEngine.Random.Range(0, lottery - 1);
        if (result >= (int)BONUS_TYPE.GRAPE)
        {
            result = result % GRAPE_BONUS_APPEAR_RATE == 0 ? (int)BONUS_TYPE.GRAPE : result;
        }
        return result;
    }

    Vector3 GetRandomPositionInEnterField()
    {
        Vector2 rectSize = GameMain.ins.MainCanvas.GetComponent<RectTransform>().sizeDelta;
        float w = (rectSize.x / GameMain.UNIT_SIZE) / 2.0f - GameMain.DUST_ENTER_FIELD_PADDING_UNIT;
        float h = (rectSize.y / GameMain.UNIT_SIZE) / 2.0f - GameMain.DUST_ENTER_FIELD_PADDING_UNIT;
        Vector3 vec = new Vector3(
            UnityEngine.Random.Range(-w,w),
            UnityEngine.Random.Range(-h,h),
            0
        );
        return vec;
    }

    public void ResetPosition(Transform tr)
    {
        tr.position = GetRandomPositionInEnterField();
    }


}
