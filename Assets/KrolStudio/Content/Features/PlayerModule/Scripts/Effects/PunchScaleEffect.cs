using DG.Tweening;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class PunchScaleEffect : MonoBehaviour
    {
        private PlayerConfig _config;
        private Tween _tween;
        private MonoDamageable _damagable;
        private Vector3 _originalScale;

        [Inject]
        private void Construct(PlayerConfig config) =>
            _config = config;

        private void Awake()
        {
            _damagable = GetComponent<MonoDamageable>();
            _damagable.OnDamaged += Play;
            _originalScale = transform.localScale;
        }

        public void Play(float value)
        {
            _tween?.Kill();
            _tween = transform.DOPunchScale(
                _config.punchSettings.GetRandomPunch(),
                _config.punchSettings.duration,
                _config.punchSettings.vibrato,
                _config.punchSettings.elasticity)
                .OnKill(() => transform.localScale = _originalScale);
        }

        public void Stop() =>
            _tween?.Kill();

        private void OnDisable() => Stop();

        private void OnDestroy()
        {
            _damagable.OnDamaged -= Play;
            _tween?.Kill();
        }
    }
}