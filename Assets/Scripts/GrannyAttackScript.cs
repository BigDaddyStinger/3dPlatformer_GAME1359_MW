using System.Collections;
using UnityEngine;

public class GrannyAttackScript : MonoBehaviour
{
    [SerializeField] GrannyI_InputActions _actions;
    [SerializeField] GrannyController _controller;

    [Header("Bullet Variables")]
    public float bulletSpeed = 200f;
    public Rigidbody regularShot;
    public Rigidbody chargedShot;
    public Transform bulletSpawnPoint;

    [Header("Aim Variables")]
    public Transform _indicator;
    public LayerMask _aimColliderMask = new LayerMask();

    [Header("Charge Shot Variables")]
    [SerializeField] private float chargeGauge;
    public float chargeTimer = 1.5f;
    public bool isCharging;


    private void Awake()
    {
        _actions = new GrannyI_InputActions();
        _controller = GetComponent<GrannyController>();
    }

    private void OnEnable()
    {
        _actions.Enable();
    }

    private void OnDisable()
    {
        _actions.Disable();
    }

    private void Update()
    {
        if (_actions.Player.Attack.triggered)
        {
            if (_controller.zoomIn)
            {
                StartCoroutine(RegularShot());
            }
        }

        //========= Charging Actions ===========

        if( _actions.Player.Attack.IsPressed())
        {
            isCharging = true;  
        }
        else
        {
            isCharging= false;
        }

        if (isCharging) chargeGauge += Time.deltaTime;

        if (_actions.Player.Attack.WasReleasedThisFrame())
        {
            if (chargeGauge >= chargeTimer && _controller.zoomIn)
            {
                StartCoroutine(ChargeShot());
                chargeGauge = 0f;
            }
            else
            {
                chargeGauge = 0f;
            }
        }

        //======================================

        //=========== Aim Controls =================
        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit rayCastHit, 999f, _aimColliderMask))
        {
            _indicator.position = rayCastHit.point;
            bulletSpawnPoint.LookAt(rayCastHit.point);
        }

        //==========================================

    }

    IEnumerator RegularShot()
    {
        Rigidbody _shot;
        _shot = Instantiate(regularShot, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as Rigidbody;
        _shot.AddForce(bulletSpawnPoint.forward * bulletSpeed, ForceMode.Force);
        yield return null;
        RumbleManager.Instance.RumblePulse(0.25f, 0.25f, 0.2f);
    }

    IEnumerator ChargeShot()
    {
        Rigidbody _shot;
        _shot = Instantiate(chargedShot, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as Rigidbody;
        _shot.AddForce(bulletSpawnPoint.forward * bulletSpeed, ForceMode.Force);
        yield return null;
        RumbleManager.Instance.RumblePulse(0.5f, 0.5f, 0.25f);
    }

}
