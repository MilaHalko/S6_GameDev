using Core.Enums;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerEntity : MonoBehaviour
    {
        [Header("Animator")]
        [SerializeField] private Animator _animator;
        
        [Header("HorizontalMovement")]
        [SerializeField] private float _horizontalSpeed;        
        [SerializeField] private float _slideSpeed;
        [SerializeField] private float _speedWhileAttack;
        [SerializeField] private Direction _direction;

        [Header("VerticalMovement")] 
        [SerializeField] private float _verticalSpeed;
        [SerializeField] private float _minSize;
        [SerializeField] private float _maxSize;
        [SerializeField] private float _minVerticalPosition;
        [SerializeField] private float _maxVerticalPosition;

        [Header("JumpMovement")] 
        [SerializeField] private float _jumpForce;
        [SerializeField] private float _gravityScale;
        [SerializeField] private SpriteRenderer _shadow;
        [SerializeField] [Range(0, 1)] private float _shadowSizeModificator;
        [SerializeField] [Range(0, 1)] private float _shadowAlphaModificator;

        [Header("Camera")] 
        [SerializeField] private DirectionalCameraPair _cameras;

        private Rigidbody2D _rigidbody;
        
        // Jump
        private float _sizeModificator;
        private bool _isJumping;
        private float _startJumpVerticalPosition;
        
        // Slide
        private float _speed;
        private bool _isSliding;
        
        // Attack
        private bool _isAttacking;
        
        // Shadow
        private Vector2 _shadowLocalPosition;
        private float _shadowVerticalPosition;

        // Animation
        private Vector2 _movement;
        private AnimationType _currentAnimationType;

        void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();

            _shadowLocalPosition = _shadow.transform.localPosition;
            float positionDifference = _maxVerticalPosition - _minVerticalPosition;
            float sizeDifference = _maxSize - _minSize;
            _sizeModificator = sizeDifference / positionDifference;
            UpdateSize();

            _speed = _horizontalSpeed;
        }

        private void Update()
        {
            if (_isJumping)
            {
                UpdateJump();
            }

            UpdateAnimations();
        }

        private void UpdateAnimations()
        {
            PlayAnimation(AnimationType.Idle, true);
            PlayAnimation(AnimationType.Walk, _movement.magnitude > 0);
            PlayAnimation(AnimationType.Slide, _movement.magnitude > 0 && _isSliding);
            PlayAnimation(AnimationType.Jump, _isJumping);
            PlayAnimation(AnimationType.Attack, _isAttacking);
        }

        public void MoveHorizontally(float direction)
        {
            _movement.x = direction;
            SetDirection(direction);
            Vector2 velocity = _rigidbody.velocity;
            velocity.x = direction * _speed;
            _rigidbody.velocity = velocity;
        }

        public void MoveVertically(float direction, float speed = default)
        {
            if(_isJumping)
                return;
            _movement.y = direction;
            SetDirection(direction);
            Vector2 velocity = _rigidbody.velocity;
            velocity.y = direction * _verticalSpeed;
            _rigidbody.velocity = velocity;
            
            if (direction == 0)
            {
                return;
            }

            float verticalPosition = Mathf.Clamp(transform.position.y, _minVerticalPosition, _maxVerticalPosition);
            _rigidbody.position = new Vector2(_rigidbody.position.x, verticalPosition);
            UpdateSize();
        }
        
        public void Slide()
        {
            if (!_isAttacking)
            {
                _speed = _slideSpeed;
                _isSliding = true;
            }
        }

        public void Jump()
        {
            if (_isJumping)
                return;
            
            _isJumping = true;
            float jumpModificator = transform.localScale.y / _maxSize;
            _rigidbody.AddForce(Vector2.up * (_jumpForce * jumpModificator));
            _rigidbody.gravityScale = _gravityScale * jumpModificator;
            _startJumpVerticalPosition = transform.position.y;
            _shadowVerticalPosition = _shadow.transform.position.y;
        }
        
        public void Attack()
        {
            _speed = _speedWhileAttack;
            _isAttacking = true;
        }
        
        public void StopAttack()
        {
            _speed = _horizontalSpeed;
            _isAttacking = false;
        }

        private void UpdateSize()
        {
            float verticalDelta = _maxVerticalPosition - transform.position.y;
            float currentSizeModificator = _minSize + _sizeModificator * verticalDelta;
            transform.localScale = Vector2.one * currentSizeModificator;
        }

        private void SetDirection(float direction)
        {
            if ((_direction == Direction.Right && direction < 0) ||
                (_direction == Direction.Left && direction > 0))
            {
                Flip();
            }
        }

        private void Flip()
        {
            transform.Rotate(0, 180, 0);
            _direction = _direction == Direction.Right ? Direction.Left : Direction.Right;
            foreach (var cameraPair in _cameras.DirectionalCameras)
            {
                cameraPair.Value.enabled = cameraPair.Key == _direction;
            }
        }

        private void UpdateJump()
        {
            if (_rigidbody.velocity.y < 0 && _rigidbody.position.y <= _startJumpVerticalPosition)
            {
                ResetJump();
                return; 
            }

            _shadow.transform.position = new Vector2(_shadow.transform.position.x, _shadowVerticalPosition);
            float distance = transform.position.y - _startJumpVerticalPosition;
            _shadow.color = new Color(1, 1, 1, 1 - distance * _shadowAlphaModificator);
            _shadow.transform.localScale = Vector2.one * (1 + _shadowSizeModificator * distance);
        }

        private void ResetJump()
        {
            _isJumping = false;
            _shadow.transform.localPosition = _shadowLocalPosition;
            _shadow.color = Color.white;
            _rigidbody.position = new Vector2(_rigidbody.position.x, _startJumpVerticalPosition);
            _rigidbody.gravityScale = 0;
        }
        
        public void StopSlide()
        {
            if (_isSliding)
            {
                _isSliding = false;
                _speed = _horizontalSpeed;
            }
        }

        private void PlayAnimation(AnimationType animationType, bool active)
        {
            if (!active)
            {
                if (_currentAnimationType == AnimationType.Idle || _currentAnimationType != animationType)
                {
                    return;
                }

                _currentAnimationType = AnimationType.Idle;
                PlayAnimation(_currentAnimationType);
                return;
            }
            
            if (_currentAnimationType >= animationType)
            {
                return;
            }

            _currentAnimationType = animationType;
            PlayAnimation(_currentAnimationType);
        }

        private void PlayAnimation(AnimationType animationType)
        {
            _animator.SetInteger(nameof(AnimationType), (int) animationType);
        }
    }
}