using UnityEngine;
using Unity.Cinemachine;

public class CameraSwitchScript : MonoBehaviour
{
    [SerializeField] GrannyController _gCTRL;


    [Header("Cameras")]
    public CinemachineCamera _platformCam;
    public CinemachineCamera _shooterCam;

    private void Awake()
    {
        _gCTRL = GameObject.FindGameObjectWithTag("Player").GetComponent<GrannyController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_gCTRL.zoomIn)
        {
            _platformCam.Priority = 0;
            _shooterCam.Priority = 1;
        }
        else
        {
            _platformCam.Priority = 1;
            _shooterCam.Priority = 0;
        }
    }
}
