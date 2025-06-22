using System;
using System.Collections;
using System.Collections.Generic;
using Tech.Pooling;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DamagePopUpController : MonoBehaviour,IPoolObject
{
    public AnimationCurve ScaleCurve;
    public AnimationCurve FadeCurve;
	public AnimationCurve HeightCurve;
	public Action StopAnimation;
	public TextMeshProUGUI Text;
    private float time = 0f;
    private Vector3 initPos;

    void Awake()
    {
        Text = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
	}
	private void Start()
	{
		Initialize();
	}
	void Update()
    {
		Text.color = new Color(Text.color.r, Text.color.g, Text.color.b ,FadeCurve.Evaluate(time));
        transform.localScale = Vector3.one * ScaleCurve.Evaluate(time);
        transform.position = initPos + new Vector3(0,1 + HeightCurve.Evaluate(time),0);
        time += Time.deltaTime;
		if(time >= 1f)
			ObjectPool.Instance.ReturnObjectToPool(gameObject);
    }

	public void Initialize()
	{
		time = 0f;
		initPos = transform.position;
	}

	public void OnReturnToPool()
	{
	}
}
