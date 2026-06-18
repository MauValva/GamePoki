using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace KrolStudio
{
    public class PartInteractionHandler : MonoBehaviour
    {
        private PartLevelController _levelController;
        private PartVisualsController _visualsController;
        private PartDragHandler _dragHandler;

        private IBoardInteractionService _boardInteraction;
        private IPlayerInteractionService _playerInteraction;
        private IGarbageService _garbageService;
        private IBoardHighlightService _highlightService;
        private BoardService _boardService;
        private IAudioService _audioService;


        [Inject]
        private void Construct(
            IBoardInteractionService boardInteraction,
            IPlayerInteractionService playerInteraction,
            IGarbageService garbageService,
            IBoardHighlightService highlightService,
            BoardService boardService,
            IAudioService audioService)
        {
            _boardInteraction = boardInteraction;
            _playerInteraction = playerInteraction;
            _garbageService = garbageService;
            _highlightService = highlightService;
            _boardService = boardService;
            _audioService = audioService;
        }

        private PartLevelController LevelController
        {
            get
            {
                if(_levelController == null)
                    _levelController = GetComponent<PartLevelController>();
                return _levelController;
            }
        }

        private PartVisualsController VisualsController
        {
            get
            {
                if (_visualsController == null)
                    _visualsController = GetComponent<PartVisualsController>();
                return _visualsController;
            }
        }

        private PartDragHandler DragHandler
        {
            get
            {
                if (_dragHandler == null)
                    _dragHandler = GetComponent<PartDragHandler>();
                return _dragHandler;
            }
        }

        private void OnEnable()
        {
            DragHandler.PointerDown += OnPointerDown;
            DragHandler.PointerUp += OnPointerUp;
            DragHandler.PointerDrag += OnPointerDrag;
        }

        private void OnDisable()
        {
            DragHandler.PointerDown -= OnPointerDown;
            DragHandler.PointerUp -= OnPointerUp;
            DragHandler.PointerDrag -= OnPointerDrag;
        }

        public void Initialize(PartType partType, int level)
        {
            LevelController.Initialize(partType, level);

            VisualsController.Initialize(_levelController.FindCurrentPart(), level);
            VisualsController.UpdateLevelIndicator(_levelController.Level, _levelController.CanUpgrade());
            VisualsController.DisplayLevelIndicator(true);
        }

        private void OnPointerDown()
        {
            _garbageService.Show();
            _highlightService.HighlightBoard(_levelController.PartType, _levelController.Level, _levelController.CanUpgrade());
            _playerInteraction.ShowBacklight(_levelController.PartType, _levelController.Level);
        }

        private void OnPointerDrag()
        {
            _garbageService.UpdateHoverColor();
        }

        private void OnPointerUp()
        {
            _highlightService.ClearBoard(_levelController.PartType);
            _playerInteraction.ClearBacklight(_levelController.PartType);

            if (_garbageService.IsHovered)
            {
                _garbageService.Hide();
                DestroyWithAnimation();
                return;
            }

            if (_playerInteraction.TryUpgrade(_levelController.PartType, _levelController.Level))
            {
                _audioService.Play(GameConstants.Sounds.UpgradePart, gameObject.transform.position);
                _garbageService.Hide();
                _boardService.BoardChangedSignal().Forget();
                DestroySelf();
                return;
            }

            if (_boardInteraction.TryMerge(this, _levelController.PartType, _levelController.Level))
            {
                _audioService.Play(GameConstants.Sounds.UpgradePart, gameObject.transform.position);
                _garbageService.Hide();
                return;
            }

            _garbageService.Hide();
            _dragHandler.ReturnToNearestCell();
        }

        public void DestroySelf()
        {
            Destroy(gameObject);
        }

        private void DestroyWithAnimation()
        {
            _dragHandler.MoveToGarbage(() =>
            {
                DestroySelf();
                _boardService.BoardChangedSignal().Forget();
            });
        }

        public PartType PartType => _levelController.PartType;
        public int Level => _levelController.Level;
        public bool CanUpgrade => _levelController.CanUpgrade();

        public void EnableBacklight(bool isGreen) =>
            _visualsController.EnableBacklight(isGreen);

        public void DisableBacklight() =>
            _visualsController.DisableBacklight();
    }
}