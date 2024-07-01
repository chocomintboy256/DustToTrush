using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class SpriteEffector: MonoBehaviour
{
    [SerializeField] Shader shader;
    private Material mat;
    private float duration { get; set; } = 0.4f;
    private Sequence seq;

    void Start() {
        mat = GetComponent<SpriteRenderer>().material;
    }

    // void Update() { }

    // マテリアルのエミッションカラーで１回光らせる
    public void Flashing()
    {
        if (!mat.HasProperty("_Progress")) return;
        if (seq != null) seq.Kill();
        seq = DOTween.Sequence()
           .Append(DOTween.To(() => mat.GetFloat("_Progress"), value => mat.SetFloat("_Progress", value), 1.0f, duration))
           .Append(DOTween.To(() => mat.GetFloat("_Progress"), value => mat.SetFloat("_Progress", value), 0.0f, duration))
//           .OnUpdate(() => { material.SetColor("_EmissionColor", new Color(0f, FlashingValue, 0f)); })
           .SetLink(gameObject);
    }

    /*
    // カスタムシェーダで描画するとき
    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        // Graphics.Blit(src, dest, m_Material);
    }
    */
}
