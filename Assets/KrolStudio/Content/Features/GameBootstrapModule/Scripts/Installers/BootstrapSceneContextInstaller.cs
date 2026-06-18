using UnityEngine;
using Zenject;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/GameBootstrap/" + nameof(BootstrapSceneContextInstaller),
    fileName = nameof(BootstrapSceneContextInstaller), order = 0)]
    public class BootstrapSceneContextInstaller : ScriptableObjectInstaller<BootstrapSceneContextInstaller>
    {
        public override void InstallBindings()
        {
        }
    }
}