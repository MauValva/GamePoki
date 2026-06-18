using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

namespace KrolStudio
{
    public class SoundEmitter : MonoBehaviour, IPoolReturnable<SoundEmitter>
    {
        private AudioSource _source;
        private CtsHelper _ctsHelper = new();

        public event Action<SoundEmitter> OnReturned;

        private void Awake() =>
            _source = GetComponent<AudioSource>();

        // 2D
        public void Play(SoundConfig config)
        {
            ApplyConfig(config);
            _source.spatialBlend = 0f;
            _source.Play();

            if (!config.Loop)
                WaitAndReturnAsync().Forget();
        }

        // 3D — fixed position
        public void Play(SoundConfig config, Vector3 position)
        {
            ApplyConfig(config);
            transform.SetParent(null);
            transform.position = position;
            _source.spatialBlend = config.SpatialBlend;
            _source.Play();

            if (!config.Loop)
                WaitAndReturnAsync().Forget();
        }

        // 3D — Child object (follows the source)
        public void Play(SoundConfig config, Transform parent)
        {
            ApplyConfig(config);
            transform.SetParent(parent, worldPositionStays: false);
            transform.localPosition = Vector3.zero;
            _source.spatialBlend = config.SpatialBlend;
            _source.Play();

            if (!config.Loop)
                WaitAndReturnAsync().Forget();
        }

        public void Stop()
        {
            _ctsHelper.Cancel();
            _source.Stop();
            ReturnToPool();
        }

        private void ApplyConfig(SoundConfig config)
        {
            _source.outputAudioMixerGroup = config.MixerGroup;
            _source.clip = config.Clip;
            _source.volume = config.Volume;
            _source.pitch = config.Pitch;
            _source.loop = config.Loop;
            _source.minDistance = config.MinDistance;
            _source.maxDistance = config.MaxDistance;
        }

        private async UniTask WaitAndReturnAsync()
        {
            try
            {
                await UniTask.Delay(
                    TimeSpan.FromSeconds(_source.clip.length / _source.pitch),
                    cancellationToken: _ctsHelper.Token);

                ReturnToPool();
            }
            catch (OperationCanceledException) { }
        }

        private void OnEnable()
        {
            _source.Stop();
            _source.clip = null;
        }

        public void ReturnToPool() =>
            OnReturned?.Invoke(this);

        private void OnDestroy()
        {
            _ctsHelper.Dispose();
        }
    }
}