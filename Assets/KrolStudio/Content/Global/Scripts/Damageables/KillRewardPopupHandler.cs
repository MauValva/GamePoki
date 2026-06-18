using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class KillRewardPopupHandler : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private bool isDoubleKillReward;

        private IDamageable _damageable;
        private IInteractorPoolContainer _poolContainer;
        private IWalletService _walletService;
        private EnemyStatsCalculator _statsModel;

        [Inject]
        private void Construct(
            IInteractorPoolContainer poolContainer,
            IWalletService walletService,
            EnemyStatsCalculator statsModel)
        {
            _poolContainer = poolContainer;
            _walletService = walletService;
            _statsModel = statsModel;
        }

        private void Awake() =>
            _damageable = GetComponent<IDamageable>();

        private void OnEnable() =>
            _damageable.OnKilled += OnKilled;

        private void OnDisable() =>
            _damageable.OnKilled -= OnKilled;

        private void OnKilled()
        {
            int killReward = _statsModel.GetEnemyKillReward(isDoubleKillReward);

            _poolContainer.GetPool<EnemyKillRewardText>()
                          .Get(spawnPoint.position)
                          .Play(NumberFormatter.Format(killReward));

            _walletService.AddAmount(CurrencyType.Coin, Mathf.RoundToInt(killReward * 10f));
        }
    }
}