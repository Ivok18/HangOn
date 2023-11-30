using UnityEngine;
using UnityEngine.UI;


namespace HangOn.Audio
{

    public class UISlider_ChangeVolumeMusic : MonoBehaviour
    {
        private Slider slider;
        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void Start()
        {
            slider.onValueChanged.AddListener(TaskOnSlide);
        }

        public void TaskOnSlide(float newValue)
        {
            MusicManager.Instance.ChangeVolume(newValue);
        }
    }
}