using System.Collections;
using UnityEngine;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager Instance;

    public float hitStopDuration;
    public float pendingStopDuration;
    public bool isFrozen;

    private void Start()
    {
        isFrozen = false;
        if(Instance == null)
            Instance = this;
    }

    private void Update()
    {
        if(pendingStopDuration != 0 && !isFrozen)
        {
            StartCoroutine(HitStopTimer());
        }
    }

    public void DoHitStop(float duration)
    {
        hitStopDuration = duration;
        pendingStopDuration = hitStopDuration;
    }

    IEnumerator HitStopTimer()
    {
        isFrozen = true;
        var _original = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(hitStopDuration);
        Time.timeScale = _original;
        pendingStopDuration = 0;
        isFrozen = false;
    }

}
