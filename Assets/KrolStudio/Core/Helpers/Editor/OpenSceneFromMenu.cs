using UnityEditor;
using UnityEditor.SceneManagement;

namespace KrolStudio
{
    public class OpenSceneFromMenu
    {
        private const string scenePath = "Assets/KrolStudio/Content/Global/GameResources/Scenes/BootstrapScene.unity";
        private const string uIScenePath = "Assets/KrolStudio/Content/Global/GameResources/Scenes/UIScene.unity";
        private const string gameplayScenePath = "Assets/KrolStudio/Content/Global/GameResources/Scenes/GameplayScene.unity";
        private const string globalScenePath = "Assets/KrolStudio/Content/Global/GameResources/Scenes/GlobalScene.unity";

        [MenuItem("Tools/", false)]
        static void Separator() { }

        [MenuItem("Tools/🚀 Open BootstrapScene")]
        public static void OpenScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(scenePath);
            }
        }

        [MenuItem("Tools/🎨 Open UIScene")]
        public static void OpenUIScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(uIScenePath);
            }
        }

        [MenuItem("Tools/🌍 Open GlobalScene")]
        public static void OpenGlobalScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(globalScenePath);
            }
        }

        [MenuItem("Tools/🎮 Open GameplayScene")]
        public static void OpenGameplayScene()
        {
            if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            {
                EditorSceneManager.OpenScene(gameplayScenePath);
            }
        }
    }
}