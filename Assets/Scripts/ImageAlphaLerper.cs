using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ImageAlphaLerper : MonoBehaviour
{
    [SerializeField] private Image targetImage;
    [SerializeField] private float duration = 1f; // 変化にかける時間

    private void Start()
    {
        targetImage.color = new Color(0f, 0f, 0f, 1f);
        FadeTo(0f);
    }

    public void FadeTo(float targetAlpha)
    {
        this.targetImage.enabled = true;
        StartCoroutine(LerpAlpha(targetAlpha));
    }

    IEnumerator LerpAlpha(float targetAlpha)
    {
        Color startColor = targetImage.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, targetAlpha);

        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            targetImage.color = Color.Lerp(startColor, endColor, time / duration);
            yield return null;
        }

        targetImage.color = endColor; // 最終値を保証
        this.targetImage.enabled = false;
    }
}
