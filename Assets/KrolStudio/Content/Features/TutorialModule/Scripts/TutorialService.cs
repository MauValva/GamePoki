using System;
using Zenject;

namespace KrolStudio
{
    public class TutorialService : IInitializable, IDisposable
    {
        private readonly TutorialConfig _config;
        private readonly SignalBus _signalBus;
        private readonly TutorialHandView _handView;
        private readonly TutorialStepHandler _stepHandler;
        private readonly IProgressService _progress;
        private readonly ILogService _logService;

        private int _currentStep;
        private bool _isActive;

        [Inject]
        public TutorialService(
            TutorialConfig config,
            SignalBus signalBus,
            TutorialHandView handView,
            IProgressService progress,
            ILogService logService,
            TutorialStepHandler stepHandler)
        {
            _config = config;
            _signalBus = signalBus;
            _handView = handView;
            _progress = progress;
            _logService = logService;
            _stepHandler = stepHandler;
        }

        public void Initialize()
        {
            _signalBus.Subscribe<TutorialStepCompletedSignal>(OnStepCompleted);
            _signalBus.Subscribe<GameplayStartedSignal>(OnGameplayStarted);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<TutorialStepCompletedSignal>(OnStepCompleted);
            _signalBus.Unsubscribe<GameplayStartedSignal>(OnGameplayStarted);
        }

        private void OnGameplayStarted()
        {
            if (ShouldPlay())
            {
                _signalBus.Fire(new ResetSaveDataSignal());
                StartTutorial();
            }
        }

        public bool ShouldPlay() =>
            !_progress.IsTutorialCompleted();

        public void StartTutorial()
        {
            if (!ShouldPlay()) return;

            _isActive = true;
            _currentStep = 0;
            _stepHandler.StartTutorial();
            ShowStep(_currentStep);
        }

        public void Skip()
        {
            if (!_config.CanSkip) return;
            Complete();
        }

        private void OnStepCompleted(TutorialStepCompletedSignal signal)
        {
            if (!_isActive) return;

            var current = _config.Steps[_currentStep];
            if (current.WaitSignal != signal.StepId) return;

            _currentStep++;

            if (_currentStep >= _config.Steps.Count)
            {
                Complete();
                return;
            }

            ShowStep(_currentStep);
        }

        private void ShowStep(int index)
        {
            var step = _config.Steps[index];
            _stepHandler.Show(step.Hint);
            _handView.Show(step.Hint);
            _logService.Log($"[Tutorial] Step {index}: {step.Id}");
        }

        private void Complete()
        {
            _isActive = false;
            _handView.Hide();
            _stepHandler.CompleteTutorial();
            _progress.SetTutorialCompleted(true);
            _signalBus.Fire(new TutorialCompletedSignal());
        }
    }
}