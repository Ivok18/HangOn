using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace HangOn.LoadingScene
{
    public class LoadManager : MonoBehaviour
    {
        [SerializeField] private GameObject loaderCanvas;
        [SerializeField] private Image progressBar;
        [SerializeField] private float progressBarSpeed;
        [SerializeField] private int waitTimeMilliseconds;
        [SerializeField] private float progressBarTarget;

        private void Awake()
        {
            //DOTween.SetTweensCapacity(999999, 999999);
        }
        private void OnEnable()
        {
            SceneManager.sceneLoaded += TryLoading;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= TryLoading;
        }


        public void TryLoading(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (scene.name == "___Loading")
            {
                LoadScene("__MMenu");
            }
        }

        public async void LoadScene(string sceneName)
        {
            loaderCanvas.SetActive(true);
            progressBar.fillAmount = 0;

            await Task.Delay(waitTimeMilliseconds);
            /*
             * why Task.Delay(waitTimeMilliseconds) ?
             * because it seems that when don't wait, we can't see the loading process of the progress bar
             * variable "waitTimeMilliseconds is an arbitrary value used to calibrate the amount of time we wait for the progress bar
             * to be filled, before we actually load the data of scene "Accueil"
            */

            var scene = SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;
            do
            {
                await Task.Delay(100);
                if (progressBar.fillAmount < progressBarTarget)
                {
                    progressBar.fillAmount = scene.progress;
                }
                /* why we check if fill amount is less than progress bar target ?
                 * because when we don't, the progress bar becomes unstable (reason unknow)
                 * why we do this just after line of code "await Task.Delay(100)" ,
                 * honestly I don't know why myself, but it seems to work
                 * 
                 */
            } while (scene.progress < progressBarTarget);

            loaderCanvas.SetActive(false);
            scene.allowSceneActivation = true;

        }

        private void Update()
        {
            if (progressBar.fillAmount < progressBarTarget)
            {
                progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, progressBarTarget, progressBarSpeed * Time.deltaTime);
            }
        }
    }
}
