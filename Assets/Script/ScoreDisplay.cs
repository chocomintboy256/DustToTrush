using DG.Tweening;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    private float movedAmount = 10.0f;
    private float duration = 0.5f;
    private float duration_l = 0.3f;
    private float duration_r { get { return duration - duration_l; } }
        
    public float strength = 20f;
    public int vibrate = 100;

    public int currentScore;

    public void SetScore(int value)
    {
        string sign =  value >= 0 ? "+" : "-";
        string abs = Mathf.Abs(value).ToString();
        TMP_Text tmp = GetComponent<TMP_Text>();
        tmp.text = $"{sign} {abs}";

        // DoTween‚ð˜AŒ‹‚µ‚Ä“®‚©‚·
        RectTransform rt = GetComponent<RectTransform>();
        rt.DOLocalMoveY(movedAmount, duration)
            .SetRelative()
            .SetEase(Ease.OutSine)
            .SetDelay(duration_l).OnStart(() => {
                tmp.DOFade(0, duration_r).SetLink(gameObject)
                .SetEase(Ease.OutSine).OnComplete(() => { Destroy(gameObject); });
            }).SetLink(gameObject); 
        currentScore = value;
    }
    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    // void Update() { }

    public void SetPos(Vector3 targetPos)
    {
        RectTransform rectScoreDisp = GetComponent<RectTransform>();
        RectTransform rectMainCanvas = GameMain.ins.MainCanvas.GetComponent<RectTransform>();
        Camera camera = Camera.main;

        Vector2 newPos = Vector2.zero;
        Vector2 overHeadPos = new Vector2(0.0f, -25.0f);
        Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(camera, targetPos);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(rectMainCanvas, screenPos, camera, out newPos);
        rectScoreDisp.localPosition = newPos - overHeadPos;
    }
}
