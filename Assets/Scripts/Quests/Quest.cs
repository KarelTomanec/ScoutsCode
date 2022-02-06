using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest {
    protected bool finishedByUser = false;
    protected bool readyToDestroy = false;
    protected float elapsedTime = 0f;
    protected QuestPointer.Pointer pointer;

    public int points;
    public float timeCap;

    public void AddElapsedTime(float timeToAdd)
    {
        elapsedTime += timeToAdd;
    }

    public bool GetReadyToDestroy()
    {
        return readyToDestroy;
    }

    public bool GetFinishedByUserStatus()
    {
        return finishedByUser;
    }

    public float GetElapsedTime()
    {
        return elapsedTime;
    }

    public void SetFinishedByUser(bool status)
    {
        finishedByUser = status;
    }

    public void SetReadyToDestroy(bool status)
    {
        readyToDestroy = status;
    }

    public virtual void Success() { }

    public virtual void Fail() { }
}
