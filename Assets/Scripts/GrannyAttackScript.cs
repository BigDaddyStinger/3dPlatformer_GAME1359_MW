using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class GrannyAttackScript : MonoBehaviour
{
    [SerializeField] GrannyI_InputActions _actions;
    [SerializeField] GrannyController _controller;
    [SerializeField] private Animator _anim;

    [Header("Melee Variables")]
    public GameObject playerHitSphere;
    public int comboID = 1;
    public float meleeTimer = 0.5f;
    public float comboTimer = 0.75f;
    public bool canMelee;

    [Header("Bullet Variables")]
    public float bulletSpeed = 200f;
    public Rigidbody regularShot;
    public Rigidbody chargedShot;
    public Transform bulletSpawnPoint;

    [Header("Aim Variables")]
    public Transform _indicator;
    public LayerMask _aimColliderMask = new LayerMask();
    public MultiAimConstraint _bodyAim;

    [Header("Charge Shot Variables")]
    [SerializeField] private float chargeGauge;
    public float chargeTimer = 1.5f;
    public bool isCharging;


    private void Awake()
    {
        _actions = new GrannyI_InputActions();
        _controller = GetComponent<GrannyController>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        playerHitSphere.SetActive(false);
        canMelee = true;
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
            else if (canMelee)
            {
                StartCoroutine(MeleeAttack());
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
        if (_controller.zoomIn)
        {
            _bodyAim.weight = 0.8f;
        }
        else
        {
            _bodyAim.weight= 0f;
        }

        Vector2 screenCenterPoint = new Vector2(Screen.width / 2, Screen.height / 2);

        Ray ray = Camera.main.ScreenPointToRay(screenCenterPoint);
        if (Physics.Raycast(ray, out RaycastHit rayCastHit, 999f, _aimColliderMask))
        {
            _indicator.position = rayCastHit.point;
            bulletSpawnPoint.LookAt(rayCastHit.point);
        }

        //==========================================

        _anim.SetBool("Melee", canMelee);
    }

    IEnumerator MeleeAttack()
    {
        if (comboID == 1)
        {
            comboID = 2;
            _anim.SetTrigger("Swing01");
            comboTimer = 1f;
        }
        else if (comboID == 2 && comboTimer > 0)
        {
            comboID = 3;
            _anim.SetTrigger("Swing02");
            comboTimer = 1f;
        }
        else if (comboID == 3 && comboTimer > 0)
        {
            comboID = 1;
            _anim.SetTrigger("Swing03");
        }


        canMelee = false;
        yield return new WaitForSeconds(0.25f);
        playerHitSphere.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        playerHitSphere.SetActive(false);
        yield return new WaitForSeconds(0.4f);
        canMelee = true;
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
