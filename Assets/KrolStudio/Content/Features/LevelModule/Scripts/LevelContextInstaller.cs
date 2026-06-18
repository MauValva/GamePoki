using UnityEngine;
using Zenject;

namespace KrolStudio
{
    [CreateAssetMenu(menuName = "Configurations/Level/" + nameof(LevelContextInstaller),
       fileName = nameof(LevelContextInstaller), order = 0)]
    public class LevelContextInstaller : ScriptableObjectInstaller<LevelContextInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<EnemyMarker>().FromComponentsInHierarchy().AsSingle();

            Container.BindInterfacesAndSelfTo<EnemySpawnService>().AsSingle();
        }
    }
}
