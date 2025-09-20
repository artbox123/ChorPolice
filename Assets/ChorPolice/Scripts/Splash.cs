using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ArtboxGames
{
    public class Splash : MonoBehaviour
    {
        private float waitTime = 2f;

        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(LoadAsyncScene());
        }

        public IEnumerator LoadAsyncScene()
        {
            // The Application loads the Scene in the background as the current Scene runs.
            // This is particularly good for creating loading screens.
            // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
            // a sceneBuildIndex of 1 as shown in Build Settings.
            yield return new WaitForSeconds(waitTime);
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Game");
        }
    }
}