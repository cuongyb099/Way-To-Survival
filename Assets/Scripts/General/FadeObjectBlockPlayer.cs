using System.Collections;
using System.Collections.Generic;
using Tech.Singleton;
using UnityEngine;

public class FadeObjectBlockPlayer : Singleton<FadeObjectBlockPlayer>
{
    public int UpdatePerSecond = 8;
    public float FadeDuration = 0.25f;
    public Vector3 Offset;
    public LayerMask Layer;
    public Transform Target { get; private set; }
    protected Coroutine checkCorotine;
    protected RaycastHit[] hits = new RaycastHit[10];
    protected List<FadingObject> fadeObjects = new List<FadingObject>();
    protected HashSet<FadingObject> fadeObjectInFrame = new HashSet<FadingObject>();
    protected float distanceBack = 30f;
    
    public void SetFadeObjet(Transform target)
    {
        if(!target) return;
        
        if (checkCorotine != null)
        {
            StopCoroutine(checkCorotine);    
        }
        
        Target = target;
        checkCorotine = StartCoroutine(StartChecking());
    }

    private IEnumerator StartChecking()
    {
        var wait = new WaitForSeconds(1f / UpdatePerSecond);

        while (true)
        {
            var rayDir = (Target.position + Offset) - transform.position;
            var originPos = transform.position - transform.forward * distanceBack;
            
            int hitCount = Physics.RaycastNonAlloc(originPos, rayDir.normalized, hits, rayDir.magnitude + distanceBack, Layer);

            fadeObjectInFrame.Clear();

            for (int i = 0; i < hitCount; i++)
            {
                var target = hits[i];
                
                if(!target.transform.TryGetComponent(out FadingObject fadeObject)) continue;

                fadeObjectInFrame.Add(fadeObject);

                if (fadeObjects.Contains(fadeObject)) continue;
                
                fadeObjects.Add(fadeObject);
                fadeObject.FadeOut(FadeDuration);
                break;
            }

            for (int i = fadeObjects.Count - 1; i >= 0; i--)
            {
                var fadeObject = fadeObjects[i];
                
                if(fadeObjectInFrame.Contains(fadeObject)) continue;
                
                fadeObject.FadeIn(FadeDuration);   
                fadeObjects.RemoveAt(i);
            }
            
            yield return wait;
        }
    }
    
    private void OnDrawGizmos()
    {
        if(!Target) return;
        
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position - transform.forward * distanceBack, Target.position + Offset);
    }
}
