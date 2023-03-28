using Player;
using UnityEngine;

public class InputReader : MonoBehaviour
{
    [SerializeField] private PlayerEntity _playerEntity;
    
    private float _horizontalDirection;
    private float _verticalDirection;
    
    private void Update()
    {
        _horizontalDirection = Input.GetAxisRaw("Horizontal");
        _verticalDirection = Input.GetAxisRaw("Vertical");

        if (Input.GetButtonDown("Jump"))
        {
            _playerEntity.Jump();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift)) 
        { 
            _playerEntity.Slide();
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _playerEntity.StopSlide();
        }

        if (Input.GetButtonDown("Fire1"))
        {
            _playerEntity.Attack();
        }
        else if (Input.GetButtonUp("Fire1"))
        {
            _playerEntity.StopAttack();
        }
        
    }

    private void FixedUpdate()
    {
        _playerEntity.MoveHorizontally(_horizontalDirection);
        _playerEntity.MoveVertically(_verticalDirection);
    }
} 