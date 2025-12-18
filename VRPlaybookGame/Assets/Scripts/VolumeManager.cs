using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : MonoBehaviour
{
    public Volume globalVolume;
    
    public Vector4 targetLift = new Vector4(0.98f, 1.01f, 1f, 0f);
    public float lerpDuration = 2f;
    
    private LiftGammaGain liftGammaGain;
    private Vector4 initialLift;

    private void OnEnable()
    {
        EditableTimer.onTimerEnd += StartLiftLerp;
    }

    private void OnDisable()
    {
        EditableTimer.onTimerEnd -= StartLiftLerp;
    }

    private void Start()
    {
        if (globalVolume.profile.TryGet(out liftGammaGain))
        {
            // Cache initial lift value
            initialLift = liftGammaGain.lift.value;
        }
        else
        {
            Debug.LogError("No encuentra nada");
        }
    }
    
    public void StartLiftLerp()
    {
        StopAllCoroutines();
        StartCoroutine(LerpLift());
    }
    
    IEnumerator LerpLift()
    {
        float elapsed = 0f;

        while (elapsed < lerpDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / lerpDuration;

            liftGammaGain.lift.value =
                Vector4.Lerp(initialLift, targetLift, t);

            
            yield return null;
        }

        // Ensure final value is exact
        liftGammaGain.lift.value = targetLift;
    }
}
