using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    // INSTANCE
    public static PlayerManager Instance;
    
    [Header("RIGIDBODY")] 
    [SerializeField] private Rigidbody _rb;
    public Rigidbody Rb => _rb;

    /* FOR JOYSTICK MOVEMENT
    [Header("JOYSTICK")] 
    [SerializeField] private Joystick _joystick;
    */
    
    [Header("MESH")] 
    [SerializeField] private SkinnedMeshRenderer _playerMesh;
    public SkinnedMeshRenderer PlayerMesh => _playerMesh;
    
    public Material PlayerMaterial
    {
        get => _playerMesh.material;
        set => _playerMesh.material = value;
    }
    [Header("COLLIDER")] 
    [SerializeField] private CapsuleCollider _playerCollider;
    public CapsuleCollider PlayerCollider => _playerCollider;

    [Header("SPEED VALUE")] 
    [SerializeField] private float _forwardSpeed;
    [SerializeField] private float _rotateSpeed;
    
    [Header("ANIMATOR")] 
    [SerializeField] private Animator _anim;
    public Animator Anim
    {
        get => _anim;
        set => _anim = value;
    }
    
    // FOR PLAYER SHOOTING
    public bool isAllowedToShoot = false;

    private void Awake()
    {
        Instance = this;
        Input.multiTouchEnabled = false; // MULTI TOUCH DISABL
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    private void FixedUpdate()
    {
        if (GameManager.IsGameStarted)
            Move();
        else
            ResetPlayerSpeed();
    }

    private void ResetPlayerSpeed()
    {
        _rb.velocity = Vector3.zero;
    }
    
    private void Move()
    {
        /* FOR JOYSTICK MOVEMENT
        if (_joystick.Vertical != 0 || _joystick.Horizontal != 0)
        {
            if (IKControl.TargetObj == null)
            {
                Quaternion targetRotation = Quaternion.LookRotation(new Vector3(
                    _joystick.Horizontal * _rotateSpeed * Time.deltaTime, 0,
                    _joystick.Vertical * _rotateSpeed * Time.deltaTime));
                
                transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, 0.1f);
            }
            else
            {
                if (IKControl.TargetObj != null)
                {
                    var targetVector = new Vector3(IKControl.TargetObj.transform.position.x, transform.position.y, IKControl.TargetObj.transform.position.z);
                    transform.DOLookAt(targetVector, 0.5f).Play();
                }
            }
            
        }
        _rb.velocity = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical) * _forwardSpeed;
        */
    }

    private void OnCollisionEnter(Collision col)
    {

    }

    private void OnTriggerEnter(Collider other)
    {

    }
}
