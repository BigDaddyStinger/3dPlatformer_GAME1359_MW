using UnityEngine;

public class GrannyController : MonoBehaviour
{
    GrannyI_InputActions _inputActions;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Animator _anim;
    private Transform _cam;

    [Header("Movement Variables")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float turnSmoothing = 0.25f;
    private float turnSmoothingVelocity;
    [SerializeField] private Vector2 moveInput;
    [SerializeField] private Vector2 aimInput;

    [Header("Jumping Variables")]
    public Transform groundCheck;
    public LayerMask thisIsGround;
    [SerializeField] private Collider[] col;
    public bool isGrounded;


    [Header("Shooting Variables")]
    public bool zoomIn;
    public float rotationX;
    public float rotationY;
    public float playerRotationSpeed = 15f;
    public float playerLookSpeed = 50f;
    public Transform camTarget;
    public Unity.Cinemachine.InputAxis yAxis;


    private void Awake()
    {
        _inputActions = new GrannyI_InputActions();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        _cam = Camera.main.transform;
    }

    private void OnEnable()
    {
        _inputActions.Enable();
    }

    private void OnDisable()
    {
        _inputActions.Disable();
    }


    private void Update()
    {
        col = Physics.OverlapSphere(groundCheck.position, 0.2f, thisIsGround);

        if (col.Length > 0)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }

        if (_inputActions.Player.Aim.IsPressed())
        {
            zoomIn = true;
        }
        else
        {
            zoomIn = false;
        }

        moveInput = _inputActions.Player.Move.ReadValue<Vector2>();

        aimInput = _inputActions.Player.Camera.ReadValue<Vector2>();

        if(_inputActions.Player.Jump.triggered && isGrounded && !zoomIn)
        {
            PlayerJump();
        }


        //========= ANIMATION S===========\\

        _anim.SetFloat("XDirection", moveInput.x);
        _anim.SetFloat("YDirection", moveInput.y);
        _anim.SetBool("Zoomed", zoomIn);
        _anim.SetBool("Grounded", isGrounded);
        _anim.SetFloat("VSpeed", _rigidbody.linearVelocity.y);

        //================================\\
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    void PlayerMove()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        _anim.SetFloat("Speed", moveDirection.magnitude);

        if (moveDirection != Vector3.zero)
        {
            if(moveDirection.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + _cam.eulerAngles.y;

                float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothingVelocity, turnSmoothing);

                if (!zoomIn)
                {
                    transform.rotation = Quaternion.Euler(0, _angle, 0);
                }

                Vector3 moveDirCam = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

                _rigidbody.MovePosition(transform.position + moveDirCam * moveSpeed * Time.fixedDeltaTime);

            }

        }

        if(zoomIn)
        {
            yAxis.Value = aimInput.y * playerLookSpeed * Time.fixedDeltaTime;

            rotationY += yAxis.Value;

            rotationY = Mathf.Clamp(rotationY, -40, 40);

            camTarget.localEulerAngles = new Vector3(-rotationY, 0, 0);

            rotationX = aimInput.x * playerRotationSpeed * Time.fixedDeltaTime;

            transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y + rotationX, 0);
        }
    }

    void PlayerJump()
    {
        Debug.Log("JumpPressed");
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
