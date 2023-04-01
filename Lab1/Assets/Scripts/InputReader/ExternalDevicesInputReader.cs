using System;
using Core.Services.Updater;
using UnityEngine;
using UnityEngine.EventSystems;

namespace InputReader
{
    public class ExternalDevicesInputReader : IEntityInputSource, IDisposable
    {
        public float HorizontalDirection => Input.GetAxisRaw("Horizontal");
        public float VerticalDirection => Input.GetAxisRaw("Vertical");

        public bool Jump { get; private set; }
        public bool Slide { get; private set; }
        public bool Attack { get; private set; }

        public ExternalDevicesInputReader()
        {
            ProjectUpdater.Instance.UpdateCalled += OnUpdate;
        }

        public void ResetOneTimeActions()
        {
            Jump = false;
            Slide = false;
            Attack = false;
        }

        public void Dispose() => ProjectUpdater.Instance.UpdateCalled -= OnUpdate;

        private void OnUpdate()
        {
            if (Input.GetButtonDown("Jump"))
                Jump = true;

            if (Input.GetKeyDown(KeyCode.LeftShift))
                Slide = true;

            if (!IsPointerOverUI() && Input.GetButtonDown("Fire1"))
                Attack = true;
        }

        private bool IsPointerOverUI() => EventSystem.current.IsPointerOverGameObject();
    }
}
