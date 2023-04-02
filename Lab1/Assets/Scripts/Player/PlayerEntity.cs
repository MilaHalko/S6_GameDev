using Core.Animation;
using Core.Tools;
using Movement.Controller;
using Movement.Data;
using StatsSystem;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Rigidbody2D))]

    public class PlayerEntity : MonoBehaviour
    {
        [SerializeField] private AnimatorController _animator;
        [SerializeField] private DirectionalMovementData _directionalMovementData;
        [SerializeField] private JumpData _jumpData;
        [SerializeField] private DirectionalCameraPair _cameras;

        private Rigidbody2D _rigidbody;
        private DirectionalMover _directionalMover;
        private Jumper _jumper;

        // Actions TODO: 
        private bool _isSliding;
        private bool _isAttacking;

        public void Initialize(IStatValueGiver statValueGiver)
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            _directionalMover = new DirectionalMover(_rigidbody, _directionalMovementData, statValueGiver);
            _jumper = new Jumper(_rigidbody, _jumpData, _directionalMovementData.MaxSize, statValueGiver);
        }

        private void Update()
        {
            if (_jumper.IsJumping)
                _jumper.UpdateJump();

            UpdateAnimations();
            UpdateCameras();
        }

        private void UpdateCameras()
        {
            foreach (var cameraPair in _cameras.DirectionalCameras)
                cameraPair.Value.enabled = cameraPair.Key == _directionalMover.Direction;
        }

        private void UpdateAnimations()
        {
            _animator.PlayAnimation(AnimationType.Idle, true);
            _animator.PlayAnimation(AnimationType.Walk, _directionalMover.IsMoving);
            _animator.PlayAnimation(AnimationType.Slide, _directionalMover.IsMoving && _isSliding);
            _animator.PlayAnimation(AnimationType.Jump, _jumper.IsJumping);
            _animator.PlayAnimation(AnimationType.Attack, _isAttacking);
        }

        public void MoveHorizontally(float direction) => _directionalMover.MoveHorizontally(direction);

        public void MoveVertically(float direction)
        {
            if (_jumper.IsJumping)
                return;
            _directionalMover.MoveVertically(direction);
        }

        public void Jump() => _jumper.Jump();

        /* TODO:
        public void StartAttack()
        {
            if (!_animator.PlayAnimation(AnimationType.Attack, true))
                return;

            _isAttacking = true;
            _speed = _speedWhileAttack;
            _animator.ActionRequested += Attack;
            _animator.AnimationEnded += EndAttack;
        }
        */

        /*private void Attack()
        {
            Debug.Log("Attack");
        }*/

        /*
        private void EndAttack()
        {
            _animator.ActionRequested -= Attack;
            _animator.AnimationEnded -= EndAttack;
            _animator.PlayAnimation(AnimationType.Attack, false);
            _speed = _horizontalSpeed;
            _isAttacking = false;
        }

        public void Slide()
        {
            if (!_isAttacking)
            {
                _speed = _slideSpeed;
                _isSliding = true;
            }
        }
        */

        /*
        public void ResetSlide()
        {
            if (_isSliding)
            {
                _isSliding = false;
                _speed = _horizontalSpeed;
            }
        }
    */
    }
}