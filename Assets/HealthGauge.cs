using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthGauge : MonoBehaviour
{
    [SerializeField] private Image healthImage;
    [SerializeField] private Image burnImage;
    [SerializeField] GameObject target;
    Vector3 targetPosCache;

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
        SetGauge(1f, false);
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

    //---------------------------------------------------------------------------------------------------------------
    // ターゲットの頭上にライフゲージを移動させるプログラム
    //----------------------------------------------------------------------------------------------------------------
    // まずヘルスゲージの幅高さを取得します。
    // 次にターゲットのスクリーン座標を取得します。ヘルスゲージの幅高さと計算して中央位置にして頭上へ微調整します。
    // 最後にメインキャンバスの中央座標に変換してメインキャンバスの縮尺を適応しています。
    // （※ターゲットの座標をスクリーン座標に変換するとメインキャンパスの座標になりました。
    //   うまく動くのですがもうちょっと別のやり方がありそうです...）
    //----------------------------------------------------------------------------------------------------------------
    private void targetChase()
    {
        if (target && target.transform.position == targetPosCache) return;
        targetPosCache = target.transform.position;

        Vector2 size = GetComponent<RectTransform>().sizeDelta;
        Vector2 pos = RectTransformUtility.WorldToScreenPoint(Camera.main, target.transform.position);

        // var x = pos.x + (target.tag == "Respawn" ? 0.0f : -size.x / 2);
        pos = new Vector2(pos.x, pos.y + 25.0f);
        GameObject MainCanvas = GameMain.ins.MainCanvas;
        pos = (pos - MainCanvas.GetComponent<RectTransform>().sizeDelta/2) * MainCanvas.transform.localScale;
        GetComponent<RectTransform>().position = pos;
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
