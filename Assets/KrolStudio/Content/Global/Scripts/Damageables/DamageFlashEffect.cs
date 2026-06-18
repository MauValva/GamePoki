using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace KrolStudio
{
    public class DamageFlashEffect : MonoBehaviour
    {
        [SerializeField] private List<Renderer> _targetRenderers;
        [SerializeField] private Material _whiteFlashMaterial;
        [SerializeField] private float _flashDuration = 0.1f;
        [SerializeField] private int _flashCount = 2;

        private List<Material> _originalMaterials;
        private CtsHelper _ctsHelper = new();

        private MonoDamageable _damagable;

        public virtual void Awake()
        {
            _damagable = GetComponent<MonoDamageable>();
            _damagable.OnDamaged += FlashWhite;
            _damagable.OnKilled += OnKilled;
        }

        private void Start()
        {
            _originalMaterials = new List<Material>(_targetRenderers.Count);

            foreach (var renderer in _targetRenderers)
                _originalMaterials.Add(renderer.material);
        }

        private void OnDisable() =>
            _ctsHelper.Cancel();

        public void OnDestroy()
        {
            _damagable.OnKilled -= OnKilled;
            _damagable.OnDamaged -= FlashWhite;
            _ctsHelper.Dispose();
        }

        public void OnKilled()
        {
            _ctsHelper.Restart();
            ResetMaterial(_ctsHelper.Token);
        }

        public void FlashWhite(float value)
        {
            _ctsHelper.Restart();
            FlashAsync().Forget();
        }

        private async UniTask FlashAsync()
        {
            try
            {
                for (int i = 0; i < _flashCount; i++)
                {
                    SetMaterial(_whiteFlashMaterial, _ctsHelper.Token);

                    await UniTask.Delay(TimeSpan.FromSeconds(_flashDuration), cancellationToken: _ctsHelper.Token);

                    ResetMaterial(_ctsHelper.Token);

                    if (i < _flashCount - 1)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(_flashDuration), cancellationToken: _ctsHelper.Token);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                ResetMaterial(_ctsHelper.Token); // Reset the material if the flash is canceled.
            }
            catch (Exception ex) when (ex is MissingReferenceException || ex is NullReferenceException)
            {
                ResetMaterial(_ctsHelper.Token);
                Debug.LogWarning($"Object destroyed during flash: {ex.Message}", this);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Unexpected error during flash: {ex}", this);
            }
        }

        private void SetMaterial(Material mat, CancellationToken token)
        {
            if (token.IsCancellationRequested) return;

            for (int i = 0; i < _targetRenderers.Count; i++)
            {
                if (token.IsCancellationRequested) return;

                var renderer = _targetRenderers[i];
                if (renderer == null) continue;

                renderer.material = mat;
            }
        }

        private void ResetMaterial(CancellationToken token)
        {
            if (token.IsCancellationRequested) return;

            for (int i = 0; i < _targetRenderers.Count; i++)
            {
                if (token.IsCancellationRequested) return;

                var renderer = _targetRenderers[i];
                if (renderer == null) continue;
                if (i >= _originalMaterials.Count) continue;

                renderer.material = _originalMaterials[i];
            }
        }
    }
}