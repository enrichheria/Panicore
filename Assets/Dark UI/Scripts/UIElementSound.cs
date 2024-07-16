using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

namespace Michsky.UI.Dark
{
    public class UIElementSound : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler
    {
        [Header("RESOURCES")]
        
        public AudioClip hoverSound;
        public AudioClip clickSound;
        public AudioClip notificationSound;

        [Header("SETTINGS")]
        public bool enableHoverSound = true;
        public bool enableClickSound = true;

        void Start()
        {
            
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (enableHoverSound == true )SFX.instance.PlaySFX(hoverSound);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (enableClickSound == true) SFX.instance.PlaySFX(clickSound);
        }

        public void Notification()
        {
            if(notificationSound!=null)
                SFX.instance.PlaySFX(notificationSound);
        }
    }
}