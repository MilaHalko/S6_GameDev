using System;

namespace Core.Services.Updater
{
    public interface IProjectUpdater
    {
        public event Action UpdateCalled;
        public event Action FixedUpdateCalled; 
        public event Action LateUpdateCalled; 
    }
}