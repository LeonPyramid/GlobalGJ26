using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Utils.Singleton;

public class VolumeManager : Singleton<VolumeManager>
{
    [SerializeField] Volume volume;
    [SerializeField] ChromaticAberration _chroma;
    [SerializeField] Vignette _vignette;
    Coroutine chromaticAberrationCoroutine;
    Coroutine vignetteCoroutine;

    private void Start()
    {
        volume = GetComponent<Volume>();
        if (volume.profile.TryGet(out ChromaticAberration chroma))
        {
            _chroma = chroma;
        }
        if (volume.profile.TryGet(out Vignette vignette))
        {
            _vignette = vignette;
        }
    }

    public void LerpChromaticAberration(float origin, float value, float duration, bool back = false)
    {
        if (volume.profile.TryGet(out ChromaticAberration chroma))
        {
            if (chromaticAberrationCoroutine != null)
                StopCoroutine(chromaticAberrationCoroutine);
            chromaticAberrationCoroutine = StartCoroutine(LerpChromaticAberrationCoroutine(chroma, origin, value, duration, back));
        }
    }

    IEnumerator LerpChromaticAberrationCoroutine(ChromaticAberration chroma, float origin, float value, float duration, bool back = false)
    {
        float elapsedTime = 0;
        float startValue = chroma.intensity.value;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.fixedDeltaTime;
            chroma.intensity.value = Mathf.Lerp(startValue, value, elapsedTime / duration);
            yield return null;
        }

        if (!back) yield break;
        elapsedTime = 0;
        startValue = chroma.intensity.value;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.fixedDeltaTime;
            chroma.intensity.value = Mathf.Lerp(startValue, origin, elapsedTime / duration);
            yield return null;
        }
    }

    public void LerpVignette(float origin, float value, float duration, bool back = false)
    {
        if (volume.profile.TryGet(out Vignette vignette))
        {
            if (vignetteCoroutine != null)
                StopCoroutine(vignetteCoroutine);
            vignetteCoroutine = StartCoroutine(LerpVignetteCoroutine(vignette, origin, value, duration, back));
        }
    }

    IEnumerator LerpVignetteCoroutine(Vignette vignette, float origin, float value, float duration, bool back = false)
    {
        float elapsedTime = 0;
        float startValue = vignette.intensity.value;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.fixedDeltaTime;
            vignette.intensity.value = Mathf.Lerp(startValue, value, elapsedTime / duration);
            yield return null;
        }

        if (!back) yield break;
        elapsedTime = 0;
        startValue = vignette.intensity.value;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.fixedDeltaTime;
            vignette.intensity.value = Mathf.Lerp(startValue, origin, elapsedTime / duration);
            yield return null;
        }
    }
}
