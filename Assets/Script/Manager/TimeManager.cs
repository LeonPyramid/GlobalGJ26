using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Utils.Singleton;

public class TimeManager : Singleton<TimeManager>
{

    [SerializeField] private float timeChangeSpeed;
    [SerializeField] private float timeSpeedWall;
    [SerializeField] private float timeSpeedMoving;
    [SerializeField] private float timeSpeedQTE;
    [SerializeField] private float timeSpeedBin;

    [SerializeField] private Tween lastChange;

    [SerializeField] private Queue<NewTimeType> newTimeQueue;
    [SerializeField] public NewTimeType newTimeType{
         get; private set;
    }


    public enum NewTimeType { Wall, Moving, QTE, Bin };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newTimeQueue = new Queue<NewTimeType>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetNewTimeSpeed(NewTimeType type){

        switch (type) {
            case NewTimeType.Bin :
            case NewTimeType.QTE :
                newTimeQueue.Enqueue(type);
                break;
            case NewTimeType.Moving :
            case NewTimeType.Wall :
                newTimeQueue.Clear();
                //newTimeType = type;
                break;

        }
        ComputeTimeSpeed(type);

    }

    public void PopTypeSpeed(){
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
            NewTimeType.Bin => timeSpeedBin,
            _ => Time.timeScale
        };
        
        ChangeTimeSpeed(timeSpeed);
    }



    void ChangeTimeSpeed(float newTimeSpeed){
        lastChange.Kill();
        lastChange = DOTween.To(x => Time.timeScale = x,Time.timeScale,newTimeSpeed,timeChangeSpeed);
    }

}
