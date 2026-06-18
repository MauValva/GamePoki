using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Zenject;
using Core.InputModule;

namespace KrolStudio
{
    // IPlayerInteractionService — upgrade logic for the player.
    public class PlayerInteractionService : IPlayerInteractionService
    {
        private readonly PlayerTransformModel _playerTransform;
        private readonly UpgradeLevelController _upgradeLevelController;
        private readonly IInputListener _input;
        private readonly ILogService _logService;
        private readonly SignalBus _signalBus;
        private readonly PlayerService _playerService;

        [Inject]
        public PlayerInteractionService(
            PlayerTransformModel playerTransform,
            UpgradeLevelController upgradeLevelController,
            IInputListener input,
            ILogService logService,
            SignalBus signalBus,
            PlayerService playerService)
        {
            _playerTransform = playerTransform;
            _upgradeLevelController = upgradeLevelController;
            _input = input;
            _logService = logService;
            _signalBus = signalBus;
            _playerService = playerService;
        }

        public bool TryUpgrade(PartType type, int level)
        {
            var results = new List<RaycastResult>();
            if (!RaycastUtility.RaycastFromPosition(out results, _input.CurrentMousePosition, LayerMask.GetMask("Player")))
                return false;

            foreach (var result in results)
            {
                var controller = result.gameObject.GetComponentInParent<UpgradeLevelController>();
                if (controller == null) continue;

                int playerLevel = controller.GetLevel(type);
                if (level < playerLevel) return false;

                if (!controller.CanUpgradePlayerPart(type))
                {
                    _logService.LogWarning("Player part is at max level.");
                    return false;
                }

                int newLevel = level > playerLevel ? level : level + 1;
                controller.SetLevel(type, newLevel);
                controller.PlayUpgradeEffect(type);

                _playerService.FireChanged();
                _signalBus.Fire(new TutorialStepCompletedSignal { StepId = "Upgrade" });

                return true;
            }

            return false;
        }

        public void ShowBacklight(PartType type, int level)
        {
            _upgradeLevelController.SetBacklight(type, level, true);
        }

        public void ClearBacklight(PartType type)
        {
            _upgradeLevelController.SetBacklight(type, 0, false);
        }
    }
}