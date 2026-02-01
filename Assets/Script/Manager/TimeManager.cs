using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
//using UnityEngine.Rendering.PostProcessing;
using Utils.Singleton;

public class TimeManager : Singleton<TimeManager>
{

    [SerializeField] private float timeChangeSpeed;
    [SerializeField] private float timeSpeedWall;
    [SerializeField] private float timeSpeedMoving;
    [SerializeField] private float timeSpeedQTE;
    [SerializeField] private float timeSpeedBin;

    [SerializeField][Range(1f, 5f)] private float vignetteAttenuationRatio;

    private float InitiaFixedDeltaTime;

    [SerializeField] private Tween lastChangeTime;
    [SerializeField] private Tween lastChangeFixed;

    [SerializeField] private Queue<NewTimeType> newTimeQueue;
    [SerializeField] private Queue<float> qualityQTEQueue;
    [SerializeField] public enum NewTimeType { Wall, Moving, QTE, Bin,Cop,Pause };
    [SerializeField] public NewTimeType newTimeType{
         get; private set;
    }

    private QteBehaviour qteBehaviour;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (newTimeQueue == null)
            newTimeQueue = new Queue<NewTimeType>();
        qualityQTEQueue = new Queue<float>();
        qteBehaviour = QteBehaviour.Instance;
        qteBehaviour.OnDone += OnQteDone;
        InitiaFixedDeltaTime = Time.fixedDeltaTime;
        newTimeType = NewTimeType.Pause;
        //Time.timeScale = 0;
        ComputeTimeSpeed(newTimeType);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetNewTimeSpeed(NewTimeType type){
        if(newTimeQueue==null)
            newTimeQueue= new Queue<NewTimeType>();
        switch (type) {
            case NewTimeType.Bin :
            case NewTimeType.QTE :
            case NewTimeType.Cop :
                newTimeQueue.Enqueue(type);
                break;
            case NewTimeType.Moving :
            case NewTimeType.Pause :
            case NewTimeType.Wall :
                newTimeQueue.Clear();
                //newTimeType = type;
                break;

        }
        ComputeTimeSpeed(type);

    }

    public void AddQualityQTE(float qualt){
        qualityQTEQueue.Enqueue(qualt);
    }

    public void PopTypeSpeed(NewTimeType type){
        if (newTimeQueue == null)
            newTimeQueue = new Queue<NewTimeType>();
        if (type == NewTimeType.QTE || type == NewTimeType.Cop){
            if( qualityQTEQueue.Count > 0 )
                qualityQTEQueue.Dequeue();
        }
        if( newTimeQueue.Count > 0 )
            newTimeQueue.Dequeue();
        ComputeTimeSpeed(null);
    }

    private void ComputeTimeSpeed(NewTimeType ?type){
        NewTimeType localType;
        if( newTimeQueue.Count > 0 ){
            localType = newTimeQueue.Peek();

        }
        else{
            if(type.HasValue)
                localType = type.Value;
            else
                if (newTimeType == NewTimeType.Wall)
                    localType = NewTimeType.Wall;
                else
                    localType = NewTimeType.Moving;
        }
        newTimeType = localType;

        float timeSpeed = localType switch
        {
            NewTimeType.Wall => timeSpeedWall,
            NewTimeType.Moving => timeSpeedMoving,
            NewTimeType.QTE => timeSpeedQTE,
            NewTimeType.Cop => timeSpeedQTE,
            NewTimeType.Bin => timeSpeedBin,
            NewTimeType.Pause => 0,
            _ => Time.timeScale
        };
        
        ChangeTimeSpeed(timeSpeed);
        if(localType == NewTimeType.QTE){
            float dir = Random.Range(0,2)==0?-1f:1f;
            qteBehaviour.Show(dir,qualityQTEQueue.Peek());
        }
    }


    void OnQteDone(int score){
        Debug.Log("J'ai pop");
        GameManager.Instance.ChangeGameState(GameState.Moving);
        PopTypeSpeed(NewTimeType.QTE);
    }

    void ChangeTimeSpeed(float newTimeSpeed){
        if(newTimeSpeed==0f){
            Time.timeScale = 0;
            return;
        }
        lastChangeTime.Kill();
        lastChangeTime = DOTween.To(x => Time.timeScale = x,Time.timeScale,newTimeSpeed,timeChangeSpeed).SetUpdate(true);
        lastChangeFixed.Kill();
        lastChangeFixed = DOTween.To(x => Time.fixedDeltaTime = x, Time.fixedDeltaTime, InitiaFixedDeltaTime * newTimeSpeed, timeChangeSpeed).SetUpdate(true);
        //VolumeManager.Instance.LerpVignette(newTimeSpeed/ vignetteAttenuationRatio, 
        //                                    Time.timeScale/ vignetteAttenuationRatio, timeChangeSpeed);
    }

}
