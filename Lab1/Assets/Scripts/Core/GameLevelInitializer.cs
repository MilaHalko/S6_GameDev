using System;
using System.Collections.Generic;
using Core.Services.Updater;
using InputReader;
using Player;
using UnityEngine;

namespace Core
{
    public class GameLevelInitializer : MonoBehaviour
    {
        [SerializeField] private PlayerEntity _playerEntity;
        [SerializeField] private GameUIInputView _gameUIInputView;
        
        private ExternalDevicesInputReader _externalDevicesInput;
        private PlayerSystem _playerSystem;
        private ProjectUpdater _projectUpdater;

        private List<IDisposable> _disposables;
        
        private bool _onPause;

        private void Awake()
        {
            _disposables = new List<IDisposable>();
            if (ProjectUpdater.Instance == null)
            {
                _projectUpdater = new GameObject().AddComponent<ProjectUpdater>();
            }
            else
            {
                _projectUpdater = ProjectUpdater.Instance as ProjectUpdater;
            }
            
            _externalDevicesInput = new ExternalDevicesInputReader();
            _disposables.Add(_externalDevicesInput);
            _playerSystem = new PlayerSystem(_playerEntity, new List<IEntityInputSource>
            {
               _gameUIInputView,
               _externalDevicesInput
            });
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                _projectUpdater.IsPaused = !_projectUpdater.IsPaused;
            }
        }

        private void OnDestroy()
        {
            foreach (var disposable in _disposables)
            {
                disposable.Dispose();
            }
        }
    }
}