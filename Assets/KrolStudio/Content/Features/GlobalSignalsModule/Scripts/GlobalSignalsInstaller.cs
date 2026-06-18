using Zenject;

namespace KrolStudio
{
    public class GlobalSignalsInstaller : Installer<GlobalSignalsInstaller>
    {
        public override void InstallBindings()
        {
            Container.DeclareSignal<ShowScreenSignal>().OptionalSubscriber();
            Container.DeclareSignal<ClearNavigationStackSignal>().OptionalSubscriber();

            Container.DeclareSignal<PlayerHUDVisibilitySignal>().OptionalSubscriber();

            //
            Container.DeclareSignal<GameplayStartedSignal>().OptionalSubscriber();
            Container.DeclareSignal<GameplayFinishedSignal>().OptionalSubscriber();

            // 
            Container.DeclareSignal<GamePausedSignal>().OptionalSubscriber();
            Container.DeclareSignal<GameResumedSignal>().OptionalSubscriber();

            //
            Container.DeclareSignal<NextLevelSignal>().OptionalSubscriber();

            //
            Container.DeclareSignal<SettingsLoadedSignal>().OptionalSubscriber();
            Container.DeclareSignal<SettingsChangedSignal>().OptionalSubscriber();
            Container.DeclareSignal<RequestSettingsSignal>().OptionalSubscriber();

            Container.DeclareSignal<WalletLoadedSignal>().OptionalSubscriber();
            Container.DeclareSignal<WalletChangedSignal>().OptionalSubscriber();
            Container.DeclareSignal<RequestWalletSignal>().OptionalSubscriber();

            Container.DeclareSignal<PlayerDataLoadedSignal>().OptionalSubscriber();
            Container.DeclareSignal<PlayerDataChangedSignal>().OptionalSubscriber();
            Container.DeclareSignal<RequestPlayerDataSignal>().OptionalSubscriber();

            Container.DeclareSignal<ProgressDataLoadedSignal>().OptionalSubscriber();
            Container.DeclareSignal<ProgressDataChangedSignal>().OptionalSubscriber();
            Container.DeclareSignal<RequestProgressDataSignal>().OptionalSubscriber();

            Container.DeclareSignal<BoardDataLoadedSignal>().OptionalSubscriber();
            Container.DeclareSignal<BoardDataChangedSignal>().OptionalSubscriber();
            Container.DeclareSignal<RequestBoardDataSignal>().OptionalSubscriber();

            Container.DeclareSignal<ResetSaveDataSignal>().OptionalSubscriber();

            //
            Container.DeclareSignal<BoardChangedSignal>().OptionalSubscriber();

            //
            Container.DeclareSignal<TutorialCompletedSignal>().OptionalSubscriber();
            Container.DeclareSignal<TutorialStepCompletedSignal>().OptionalSubscriber();
        }
    }
}