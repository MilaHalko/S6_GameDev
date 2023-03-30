using UnityEngine;
using UnityEngine.UI;

namespace InputReader
{
    public class GameUIInputView : MonoBehaviour, IEntityInputSource
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private Button _jumpButton;
        [SerializeField] private Button _attackButton;
        //[SerializeField] private Button _slideButton;

        public float HorizontalDirection => _joystick.Horizontal;
        public float VerticalDirection => _joystick.Vertical;
    
        public bool Jump { get; private set; }
        public bool Slide { get; private set; }
        public bool Attack { get; private set; }

        private void Awake()
        {
            _jumpButton.onClick.AddListener(() => Jump = true);
            _attackButton.onClick.AddListener(() => Attack = true);
        }

        private void OnDestroy()
        {   
            _jumpButton.onClick.RemoveAllListeners();
            _attackButton.onClick.RemoveAllListeners();
        }
        
        public void ResetOneTimeActions()
        {
            Jump = false;
            Slide = false;
            Attack = false;
        }
    }
}