using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;

public class BattleAnimation
{
    const float DURATION_BACK = 0.3f;
    const float DURATION_TO = 0.3f;
    const float DURATION_REBOUND = 0.2f;
    const float REBOUND_POWER = 2.0f;
    DragCharactor a;
    DragCharactor b;
    Vector3 goal;
    Vector3 addBackPos;
    Vector3 addBackPosA;
    Vector3 addBackPosB;
    public bool IsClashPlay { get; set; }
    Sequence reboundSeq;

    public BattleAnimation(DragCharactor a, DragCharactor b)
    {
        this.a = a;
        this.b = b;
        reboundSeq = DOTween.Sequence().Pause();
    }

    // Tween: ゴミとボックスを衝突する
    public async UniTask ClashPlay() {
        //Vector3 goal = Vector3.Lerp(a.transform.position, b.transform.position, 0.5f);
        goal = b.transform.position;
        addBackPos = (b.transform.position - a.transform.position).normalized * REBOUND_POWER;
        addBackPosA = a.transform.position - addBackPos;
        addBackPosB = b.transform.position + addBackPos;

        Sequence seqA = DOTween.Sequence()
            .Append(a.transform.DOLocalMove(addBackPosA, DURATION_BACK)
              .SetEase(Ease.OutSine).SetLink(a.gameObject))
            .Append(a.transform.DOMove(goal, DURATION_TO)
              .SetEase(Ease.InSine).SetLink(a.gameObject));

        Sequence seqB = DOTween.Sequence()
            .Append(b.transform.DOLocalMove(addBackPosB, DURATION_BACK)
              .SetEase(Ease.OutSine).SetLink(b.gameObject))
            .Append(b.transform.DOMove(goal, DURATION_TO)
              .SetEase(Ease.InSine).SetLink(b.gameObject));

        Sequence seq = DOTween.Sequence()
            .Join(seqA)
            .Join(seqB);

        IsClashPlay = true;
        await seq.Play();
    }

    // Tween: 衝突後のリバウンド
    public async UniTask ReboundPlay()
    {
        await reboundSeq.Play();
    }

    // Tween: リバウンドのシーケンスに登録
    public Sequence AddRebound(Transform tf, bool sign, bool boundHalf = false, float duration = DURATION_REBOUND)
    {
        TweenCallback comp = () => {
            tf.gameObject.GetComponent<DragCharactor>().hpGauge.targetPosReflesh(); 
        };
        Vector3 pos = tf.position + (sign ? 1: -1) * addBackPos / (boundHalf ? 2.0f : 1.0f);
        var s = tf.DOLocalMove(pos, duration).OnComplete(comp).SetEase(Ease.OutSine).SetLink(tf.gameObject);
        return reboundSeq.Join(s);
    }
}
