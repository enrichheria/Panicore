using System;
using System.Collections;
using System.Collections.Generic;
using Animancer;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class AI_Controller : MonoBehaviour
{
    public static AI_Controller instance;
    [Serializable]
    public class DelayTime
    {
        public int id;
        public float time;
    }

    public List<DelayTime> DelayTimes = new List<DelayTime>();

    public AnimancerComponent _AnimancerComponent;

    public AnimationClip _idle, _walk, _run, _attak;

    public List<AnimationClip> _delayAnimations = new List<AnimationClip>();


    private string lastAnimationState;
    public GameObject deadCamera;
    public NavMeshAgent _Agent;
    public float walkSpeed, runSpeed;
    
    
    public Transform pointRay;
    public LayerMask doorLayer, playerLayer;
    
    public List<Transform> floorPoints = new List<Transform>();
    public int currentPointID,countPointsFind;
    public int CountFindPoints=4;
    public MonsterAudio _MonsterAudio;
    private Transform _player,_radio;
    public bool goToPlayer;
    private float timerGoing;
    private Coroutine waitCoroutine;
    public bool isFindPlayer, isMonsterDelay, isRadio;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        _player = PlayerController.instance.transform;
        StartCoroutine(FindPoint());
    }

    void FindPlayer()
    {
        isFindPlayer = true;
        _Agent.destination = transform.position;
        UIController.instance.SetActiveInputController(false);
        UIController.instance._Inventary.gameObject.SetActive(false);
        deadCamera.SetActive(true);
        PlayerController.instance.gameObject.SetActive(false);
        _AnimancerComponent.Play(_attak);
        _MonsterAudio.PlayAngry();
        UIController.instance.SetActiveDeadPanel();
    }

    void StopFindPlayer()
    {
        goToPlayer = false;
        timerGoing = 0;
        waitCoroutine = StartCoroutine(FindPoint());
    }

    void CheckGoToPlayer()
    {
        if (NoiseController.instance.currentFillValue >= 0.4f)
        {
            goToPlayer = true;
            _MonsterAudio.PlayAngry();
            if (waitCoroutine != null)
            {
                StopCoroutine(waitCoroutine);
                waitCoroutine = null;
            }

            PlayAnimation("run");
        }
    }
    void Update()
    {
        if (!isFindPlayer && !isMonsterDelay && !isRadio)
        {
            if (!goToPlayer)
            {
                CheckGoToPlayer();
            }
            else
            {
                if (timerGoing < 3f)
                {
                    timerGoing += 1f * Time.deltaTime;
                    _Agent.destination = _player.transform.position;
                    if (Vector3.Distance(new Vector3(transform.position.x,transform.position.y+1,transform.position.z), new Vector3(_player.position.x,_player.position.y,_player.position.z)) <= 3f)
                    {
                        FindPlayer();
                    }
                }
                else
                {
                    if (Vector3.Distance(transform.position, _player.transform.position) > 3f)
                    {
                        waitCoroutine = StartCoroutine(FindPoint());
                    }
                    else
                    {
                        FindPlayer();
                    }

                    timerGoing = 0f;
                    goToPlayer = false;
                }
            }

            if (lastAnimationState == "walk")
            {
                RaycastHit hit;
                if (Physics.Linecast(pointRay.position, _player.position+new Vector3(0,1f,0),out hit,playerLayer))
                {
                    if (hit.transform.tag == "Player")
                    {
                        if (NoiseController.instance.currentFillValue >= 0.25f)
                        {
                            goToPlayer = true;
                            _MonsterAudio.PlayAngry();
                            if (waitCoroutine != null)
                            {
                                StopCoroutine(waitCoroutine);
                                waitCoroutine = null;
                            }

                            PlayAnimation("run");
                        }
                        else if (Vector3.Distance(transform.position, _player.position) < 6f)
                        {
                            goToPlayer = true;
                            _MonsterAudio.PlayAngry();
                            if (waitCoroutine != null)
                            {
                                StopCoroutine(waitCoroutine);
                                waitCoroutine = null;
                            }

                            PlayAnimation("run");
                        }else if (PlayerController.instance._PhoneController.isLightActive)
                        {
                            goToPlayer = true;
                            _MonsterAudio.PlayAngry();
                            if (waitCoroutine != null)
                            {
                                StopCoroutine(waitCoroutine);
                                waitCoroutine = null;
                            }

                            PlayAnimation("run");
                        }
                        else
                        {
                            if (Vector3.Distance(transform.position, floorPoints[currentPointID].position) < 1f)
                            {
                                waitCoroutine = StartCoroutine(FindPoint());
                            }
                            
                        }
                    }
                    else
                    {
                        if (Vector3.Distance(transform.position, floorPoints[currentPointID].position) < 1f)
                        {
                            waitCoroutine = StartCoroutine(FindPoint());
                        }
                        
                    }
                }
                else
                {
                    if (goToPlayer)
                    {
                        StopFindPlayer();
                    }else
                    if (Vector3.Distance(transform.position, floorPoints[currentPointID].position) < 1f)
                    {
                        waitCoroutine = StartCoroutine(FindPoint());
                    }
                    
                }
            }
        }
        else if(isRadio && !isMonsterDelay)
        {
            if (_radio != null)
            {
                if (Vector3.Distance(transform.position, _radio.position) > 0.5f)
                {
                    _Agent.destination = _radio.position;
                }
                else
                {
                    _Agent.destination = transform.position;
                    isRadio = false;
                    isMonsterDelay = true;
                    StartCoroutine(AttakRadio());
                }
            }
        }
    }

    IEnumerator AttakRadio()
    {
        PlayAnimation("attak");
        yield return new WaitForSeconds(1f);
        isRadio = false;
        isMonsterDelay = false;
        waitCoroutine = StartCoroutine(FindPoint());
        if (_radio != null)
        {
            Destroy(_radio.gameObject);
        }
    }
    public void SetRadio(Transform radio)
    {
        _radio = radio;
        isRadio = true;
        PlayAnimation("run");
        _MonsterAudio.PlayAngry();
    }
    IEnumerator FindPoint()
    {
        PlayAnimation("idle");
        _Agent.destination = transform.position;
        if (countPointsFind < CountFindPoints)
        {
            countPointsFind++;
        }
        else
        {
            floorPoints = PathPoints.instance.GetPathPints();
            countPointsFind = 0;
        }

        currentPointID = Random.Range(0, floorPoints.Count);
        yield return new WaitForSeconds(2f);
        _Agent.destination = floorPoints[currentPointID].position;
        PlayAnimation("walk");
        waitCoroutine = null;
    }

    public void PlayAnimation(string state)
    {
        if (lastAnimationState != state)
        {
            if (state == "idle")
            {
                UIController.instance.titleRun.SetActive(false);
                _AnimancerComponent.Play(_idle, 0.25f);
            }
            if (state == "walk")
            {
                UIController.instance.titleRun.SetActive(false);
                _Agent.speed = walkSpeed;
                _AnimancerComponent.Play(_walk, 0.25f);
            }
            if (state == "run")
            {
                UIController.instance.titleRun.SetActive(true);
                _Agent.speed = runSpeed;
                _AnimancerComponent.Play(_run, 0.25f);
            }
            if (state == "attak")
            {
                _AnimancerComponent.Play(_attak, 0.25f);
            }
            lastAnimationState = state;
        }
    }

    public void SetMonsterEntered(int id, GameObject parent)
    {
        isMonsterDelay = true;
        timerGoing = 0f;
        goToPlayer = false;
        _Agent.destination = parent.transform.position;
        _MonsterAudio.PlayAngry();
        if (id == 6)
        {
            _AnimancerComponent.Play(_delayAnimations[5]);
        }
        if (id == 7)
        {
            _AnimancerComponent.Play(_delayAnimations[7]);
        }
        if (id == 8)
        {
            _AnimancerComponent.Play(_delayAnimations[6]);
        }
        if (id == 9)
        {
            _AnimancerComponent.Play(_delayAnimations[1]);
        }
        if (id == 10)
        {
            _AnimancerComponent.Play(_delayAnimations[2]);
        }
        if (id == 11)
        {
            _AnimancerComponent.Play(_delayAnimations[3]);
        }
        if (id == 14)
        {
            _AnimancerComponent.Play(_delayAnimations[4]);
        }

        for (int i = 0; i < DelayTimes.Count; i++)
        {
            if (DelayTimes[i].id == id)
            {
                StartCoroutine(WaitDelay(DelayTimes[i].time,parent));
                break;
            }
        }
    }

    IEnumerator WaitDelay(float time, GameObject parent)
    {

        yield return new WaitForSeconds(time);
        isMonsterDelay = false;
        if (!isRadio)
        {
            waitCoroutine = StartCoroutine(FindPoint());
        }
        else
        {
            PlayAnimation("run");
            _MonsterAudio.PlayAngry();
        }

        Destroy(parent);
    }
}
