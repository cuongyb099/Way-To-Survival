using System;
using System.Collections;
using System.Collections.Generic;
using Tech.Singleton;
using UnityEngine;

public class EntitiesJobManager : Singleton<EntitiesJobManager>
{
    [SerializeField] private int _overlapCheckPerSecond = 10;
    [SerializeField] private JobRuntime[] _jobRuntime;
    private WaitForSeconds _waitForSeconds;
    
    public void Add(JobData jobData)
    {
        for(int i = 0; i < _jobRuntime.Length; i++)
        {
            if(_jobRuntime[i].JobLogic != jobData.EntitiesJob) continue;
            
            _jobRuntime[i].Datas.Add(jobData);
            return;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        _waitForSeconds = new WaitForSeconds((float)1 / _overlapCheckPerSecond);
    }
    
    private void Start()
    {
        StartCoroutine(JobCorotine());
    }

    private IEnumerator JobCorotine()
    {
        while (true)
        {
            yield return _waitForSeconds;
            foreach (JobRuntime job in _jobRuntime)
            {
                job.JobLogic.DoJob(job.Datas);
            }
        }
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}

[Serializable]
public class JobRuntime
{
    public EntitiesJobSO JobLogic;
    public List<JobData> Datas = new ();
}