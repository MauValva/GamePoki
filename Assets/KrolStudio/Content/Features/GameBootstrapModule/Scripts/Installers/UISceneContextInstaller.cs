using UnityEngine;
using Zenject;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/GameBootstrap/" + nameof(UISceneContextInstaller),
    fileName = nameof(UISceneContextInstaller), order = 0)]
    public class UISceneContextInstaller : ScriptableObjectInstaller<UISceneContextInstaller>
    {
        public override void InstallBindings()
        {
            TutorialInstaller.Install(Container);

            UIModuleInstaller.Install(Container);
        }
    }
}
