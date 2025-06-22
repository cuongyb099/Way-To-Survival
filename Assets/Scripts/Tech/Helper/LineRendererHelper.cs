using UnityEngine;

public class LineRendererHelper : MonoBehaviour
{
    public LineRenderer LR { get; private set; }
	private void Awake()
	{
		LR = GetComponent<LineRenderer>();
	}
	public void SetLineRenderer(Transform start, float aimAmount,Vector3 forward)
	{
		LR.SetPosition(0, start.transform.position);
		LR.SetPosition(1, start.transform.position + forward * aimAmount);
	}
}
