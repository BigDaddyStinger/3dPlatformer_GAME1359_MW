using UnityEngine;

public class GrannyController : MonoBehaviour
{
    GrannyI_InputActions _inputActions;
    [SerializeField] private Rigidbody _rigidbody;
    private Transform _cam;

    [Header("Movement Variables")]
    public float moveSpeed = 5f;
    public float jumpForce = 12f;
    public float turnSmoothing = 0.25f;
    private float turnSmoothingVelocity;
    [SerializeField] private Vector2 moveInput;

    [Header("Jumping Variables")]
    public Transform groundCheck;
    public LayerMask thisIsGround;
    [SerializeField] private Collider[] col;
    public bool isGrounded;


    private void Awake()
    {
        _inputActions = new GrannyI_InputActions();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
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

        moveInput = _inputActions.Player.Move.ReadValue<Vector2>();

        if(_inputActions.Player.Jump.triggered && isGrounded)
        {
            PlayerJump();
        }
    }

    private void FixedUpdate()
    {
        PlayerMove();
    }

    void PlayerMove()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y);

        if (moveDirection != Vector3.zero)
        {
            if(moveDirection.magnitude >= 0.1f)
            {
                float targetAngle = Mathf.Atan2(moveDirection.x, moveDirection.z) * Mathf.Rad2Deg + _cam.eulerAngles.y;

                float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothingVelocity, turnSmoothing);

                transform.rotation = Quaternion.Euler(0, _angle, 0);

                Vector3 moveDirCam = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

                _rigidbody.MovePosition(transform.position + moveDirCam * moveSpeed * Time.fixedDeltaTime);

            }


        }

    }

    void PlayerJump()
    {
        _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
