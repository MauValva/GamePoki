using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public interface IAudioService
    {
        SoundEmitter Play(string soundName);
        SoundEmitter Play(string soundName, Vector3 position);
        SoundEmitter Play(string soundName, Transform parent);
        void Stop(SoundEmitter emitter);
    }

    public class AudioService : IAudioService
    {
        private readonly IInteractorPoolContainer _pool;
        private readonly AudioLibrary _library;
        private readonly ILogService _logService;

        [Inject]
        public AudioService(
            IInteractorPoolContainer pool,
            AudioLibrary library,
            ILogService logService)
        {
            _pool = pool;
            _library = library;
            _logService = logService;
        }

        public SoundEmitter Play(string soundName)
        {
            var config = _library.GetConfig(soundName);
            if (config == null)
            {
                return null;
            }

            var emitter = _pool.GetPool<SoundEmitter>().Get();

            if (emitter == null)
            {
                _logService.LogWarning($"[AudioService] Failed to get SoundEmitter from pool for sound '{soundName}'");
                return null;
            }

            emitter.Play(config);

            return emitter;
        }

        public SoundEmitter Play(string soundName, Vector3 position)
        {
            var config = _library.GetConfig(soundName);
            if (config == null) return null;

            var emitter = _pool.GetPool<SoundEmitter>().Get();
            emitter.Play(config, position);
            return emitter;
        }

        public SoundEmitter Play(string soundName, Transform parent)
        {
            var config = _library.GetConfig(soundName);
            if (config == null) return null;

            var emitter = _pool.GetPool<SoundEmitter>().Get();
            emitter.Play(config, parent);
            return emitter;
        }

        public void Stop(SoundEmitter emitter) =>
            emitter?.Stop();
    }
}