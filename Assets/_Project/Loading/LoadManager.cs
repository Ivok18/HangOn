using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace HangOn.LoadingScene
{
    public class LoadManager : MonoBehaviour
    {
        [SerializeField] private GameObject loadingUI;
        [SerializeField] private Image progressBar;
        [SerializeField] private float progressBarSpeed;
        [SerializeField] private int waitTimeMilliseconds;
        [SerializeField] private float progressBarTarget;
        [SerializeField] private float timeUntilMainMenu;

        private void Awake()
        {
            //DOTween.SetTweensCapacity(999999, 999999);
        }
        private void Start()
        {
            timeUntilMainMenu = waitTimeMilliseconds / 1000;
            progressBar.fillAmount = 0;
        }

        private void Update()
        {
            bool isLoadAlmostComplete = progressBar.fillAmount >= progressBarTarget;
            if (!isLoadAlmostComplete)
            {
                progressBar.fillAmount = Mathf.MoveTowards(progressBar.fillAmount, progressBarTarget, progressBarSpeed * Time.deltaTime);
            }
            else
            {
                bool hasShownMainMenu = loadingUI.gameObject.activeSelf == false ? true : false;
                if (!hasShownMainMenu)
                {
                    if(timeUntilMainMenu > 0)
                    {
                        timeUntilMainMenu -= Time.deltaTime;
                    }
                    else
                    {
                        // we indirectly show the main menu by disabling the loading ui ..
                        // because the loading ui is on top of the main menu in the hierarchy
                        loadingUI.SetActive(false);
                    }
                }
            }
            
        }
    }
}
