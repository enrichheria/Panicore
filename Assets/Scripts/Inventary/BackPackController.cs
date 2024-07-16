using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class BackPackController : MonoBehaviour
{
    public Transform _backpack;
    public Transform _CameraTransform;
    public Transform _posToOpen,_posBackpack;

    public AudioClip openBP, closeBP;
    
    public void OpenInventary(bool v)
    {
        if (v)
        {
            _backpack.gameObject.SetActive(true);
            _backpack.DORotateQuaternion(Quaternion.Euler(_posBackpack.localEulerAngles.x,_posBackpack.localEulerAngles.y+transform.localEulerAngles.y,0), 0.5f);
            SFX.instance.PlaySFX(openBP);
            _CameraTransform.DOLocalMove(_posToOpen.localPosition, 0.5f);
            _CameraTransform.DORotateQuaternion(Quaternion.Euler(_posToOpen.localEulerAngles.x,transform.localEulerAngles.y,0), 0.5f).OnComplete(() =>
            {
                UIController.instance._Inventary.SetActive(true);
            });
        }
        else
        {
            SFX.instance.PlaySFX(closeBP);
            UIController.instance._Inventary.SetActive(false);
            _CameraTransform.DOLocalMove(Vector3.zero, 0.5f);
            _CameraTransform.DOLocalRotate(Vector3.zero, 0.5f);
            
            _backpack.DOLocalRotate(Vector3.zero, 0.5f).OnComplete(() =>
            {
                _backpack.gameObject.SetActive(false);
            });
        }
    }
}
