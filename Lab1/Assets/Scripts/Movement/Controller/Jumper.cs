using Core.StatsSystem.Enum;
using Movement.Data;
using StatsSystem;
using UnityEngine;

namespace Movement.Controller
{
    public class Jumper
    {
        private readonly JumpData _jumpData;
        private readonly Rigidbody2D _rigidbody;
        private readonly IStatValueGiver _statValueGiver;

        private readonly float _maxVerticalSize;
        private readonly Transform _transform;
        private readonly Transform _shadowTransform;
        private readonly Vector2 _shadowLocalPosition;

        private float _startJumpVerticalPosition;
        private float _shadowVerticalPosition;
        
        public bool IsJumping { get; private set; }

        public Jumper(Rigidbody2D rigidbody, JumpData jumpData, float maxVerticalSize, IStatValueGiver statValueGiver)
        {
            _rigidbody = rigidbody;
            _jumpData = jumpData;
            _statValueGiver = statValueGiver;
            _maxVerticalSize = maxVerticalSize;
            _transform = _rigidbody.transform;
            _shadowTransform = jumpData.Shadow.transform;
            _shadowLocalPosition = _shadowTransform.localPosition;
        }

        public void Jump()
        {
            if (IsJumping)
                return;

            IsJumping = true;
            _startJumpVerticalPosition = _rigidbody.position.y;
            var jumpModificator = _transform.localScale.y / _maxVerticalSize;
            var currentJumpForce = _statValueGiver.GetStatValue(StatType.JumpForce) * jumpModificator;
            _rigidbody.gravityScale = _jumpData.GravityScale * jumpModificator;
            _rigidbody.AddForce(Vector2.up * currentJumpForce);
            _shadowVerticalPosition = _shadowTransform.position.y;
        }

        public void UpdateJump()
        {
            if (_rigidbody.velocity.y < 0 && _rigidbody.position.y <= _startJumpVerticalPosition)
            {
                ResetJump();
                return;
            }

            var distance = _transform.position.y - _startJumpVerticalPosition;
            _shadowTransform.position = new Vector2(_shadowTransform.position.x, _shadowVerticalPosition);
            _shadowTransform.localScale = Vector2.one * (1 + _jumpData.ShadowSizeModificator * distance);
            _jumpData.Shadow.color = new Color(1, 1, 1, 1 - distance * _jumpData.ShadowAlphaModificator);
        }

        private void ResetJump()
        {
            _rigidbody.gravityScale = 0;
            _transform.position = new Vector2(_transform.position.x, _startJumpVerticalPosition);

            _shadowTransform.localScale = Vector2.one;
            _shadowTransform.localPosition = _shadowLocalPosition;
            _jumpData.Shadow.color = Color.white;
            
            IsJumping = false;
        }
    }
}