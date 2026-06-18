using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class PlayerDeadState : IState
    {
        private readonly IUIScreenNavigationService _navigation;
        private readonly IPlayerEffects _playerEffects;

        [Inject]
        public PlayerDeadState(
            IUIScreenNavigationService navigation,
            IPlayerEffects playerEffects)
        {
            _navigation = navigation;
            _playerEffects = playerEffects;
        }

        public async UniTask Enter()
        {
            //_audioManager.PlaySound(GameConstants.Sounds.CarCrash);
            _playerEffects.PlayNukeExplosion(true);

            Physics.gravity = new Vector3(0f, -35f, 0f);

            await UniTask.Delay(TimeSpan.FromSeconds(2.5), ignoreTimeScale: false);
            _navigation.Push<FailView>().Forget();
        }

        public UniTask Exit()
        {
            Physics.gravity = new Vector3(0f, -9.81f, 0f);
            return default;
        }
    }
}