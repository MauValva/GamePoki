using Global.Scripts.Generated;
using UnityEngine;
using Zenject;

namespace KrolStudio
{ 
    [CreateAssetMenu(menuName = "Configurations/GameBootstrap/" + nameof(GameplaySceneContextInstaller),
        fileName = nameof(GameplaySceneContextInstaller), order = 0)]
    public class GameplaySceneContextInstaller : ScriptableObjectInstaller<GameplaySceneContextInstaller>
    {
        public override void InstallBindings()
        {
            PrefabSpawnerInstaller.Install(Container);
            GameplayPoolInstaller.Install(Container);
            LevelModuleInstaller.Install(Container);
        }
    }
}