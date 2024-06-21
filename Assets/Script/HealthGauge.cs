using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthGauge : MonoBehaviour
{
    [SerializeField] private Image healthImage;
    [SerializeField] private Image burnImage;
    [SerializeField] GameObject target;
    [SerializeField] Vector3 targetPosCache;
    [SerializeField] Vector3 posCache;
    RectTransform rectHpGauge;
    RectTransform rectHpCanvas;

    public float duration = 0.5f;
    public float strength = 20f;
    public int vibrate = 100;

    public float debugDamageRate = 0.1f;

    private float currentRate = 1f;
    private bool _AlwaysActiveFlag = false;
    public bool AlwaysActiveFlag { 
        get { return _AlwaysActiveFlag; } 
        set { if (value) gameObject.SetActive(true); _AlwaysActiveFlag = value; } 
    }

    private void Start()
    {
        rectHpCanvas = transform.parent.GetComponent<RectTransform>();
        rectHpGauge = GetComponent<RectTransform>();

        SetGauge(1f, false);
        targetChase();
    }

    public void SetGauge(float value, bool isShake = true)
    {
        if (!AlwaysActiveFlag) gameObject.SetActive(value == 1.0 ? false : true);

        // DoTweenを連結して動かす
        healthImage.DOFillAmount(value, duration)
            .OnComplete(() =>
            {
                burnImage
                    .DOFillAmount(value, duration / 2f)
                    .SetDelay(0.5f)
                    .SetLink(target);
            }).SetLink(target);
        if (currentRate <= value) isShake = false;
        if (isShake) transform.DOShakePosition(
            duration / 2f,
            strength, vibrate).SetLink(target);

        currentRate = value;
    }

    public void TakeDamage(float rate)
    {
        SetGauge(currentRate - rate);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(debugDamageRate);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            transform.DOShakePosition(
                duration / 2f,
                strength, vibrate);
        }

        targetChase();
    }
    public void targetPosReflesh()
    {
        targetPosCache = Vector3.positiveInfinity;
    }

    //---------------------------------------------------------------------------------------------------------------
    // ターゲットの頭上にライフゲージを移動させるプログラム
    //----------------------------------------------------------------------------------------------------------------
    // ターゲットのスクリーン座標を取得して頭上へ移動してキャンバスの座標にして設定
    // ※ターゲットの位置とHPゲージの位置どちらも前回と同じなら処理しません
    //----------------------------------------------------------------------------------------------------------------
    private void targetChase()
    {
        if (target
            && target.transform.position == targetPosCache
            && rectHpGauge.localPosition == posCache) return;

        targetPosCache = target.transform.position;

        Camera camera = Camera.main;
        Vector2 newPos = Vector2.zero;
        Vector2 overHeadPos = new Vector2(0.0f, -25.0f);
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, target.transform.position);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectHpCanvas, screenPos, camera, out newPos);

        posCache = rectHpGauge.localPosition;
        rectHpGauge.localPosition = newPos - overHeadPos;
    }
    static public GameObject CreateGauge(GameObject target)
    {
        GameObject newHpGauge = Instantiate(GameMain.ins.hpGauge, Vector3.zero, Quaternion.identity, GameMain.ins.hpCanvas.transform);
        HealthGauge healthGauge = newHpGauge.GetComponent<HealthGauge>();
        healthGauge.target = target;
        target.GetComponent<DragCharactor>().hpGauge = healthGauge;
        return newHpGauge;
    }
}
