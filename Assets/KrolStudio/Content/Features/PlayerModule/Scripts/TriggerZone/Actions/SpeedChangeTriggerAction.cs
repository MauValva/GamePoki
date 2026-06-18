using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class SpeedChangeTriggerAction : MonoBehaviour, ITriggerAction
    {
        [SerializeField] private bool isReturningToBaseSpeed;
        private IMovementService _movement;
        private PlayerConfig _playerConfig;

        [Inject]
        private void Construct(
            IMovementService movement,
            PlayerConfig playerConfig)
        {
            _movement = movement;
            _playerConfig = playerConfig;
        }

        public void Execute()
        {
            _movement.SetSpeed(isReturningToBaseSpeed ? _playerConfig.baseMoveSpeed : _playerConfig.startSpeed);
        }
    }
}
