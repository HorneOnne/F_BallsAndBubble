using UnityEngine.SceneManagement;

namespace BallsAndBubble
{
    public static class Loader
    {
        public enum Scene
        {
            GameplayScene,
            MenuScene,
        }

        private static Scene targetScene;

        public static void Load(Scene targetScene, System.Action afterLoadScene = null)
        {
            Loader.targetScene = targetScene;
            SceneManager.LoadScene(Loader.targetScene.ToString());
        }
    }
}
