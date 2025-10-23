using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{
    public static RumbleManager Instance;

    [SerializeField] private Gamepad _gamePad;
    public bool isRumbling = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void RumblePulse(float lowFreq, float highFreq, float _duration)
    {
        _gamePad = Gamepad.current;
        if(_gamePad != null)
        {
            if (!isRumbling)
            {
                isRumbling = true;
                _gamePad.SetMotorSpeeds(lowFreq, highFreq);
                StartCoroutine(StopRumble(_duration, _gamePad));
            }
        }
    }

    IEnumerator StopRumble(float _duration, Gamepad _pad)
    {
        float elapsedTime = 0;
        while (elapsedTime < _duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isRumbling = false ;
        _pad.SetMotorSpeeds(0,0);
    }
}
