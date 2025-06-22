using Tech.Logger;
using UnityEngine;
using UnityEngine.UIElements;

public class M_AttributeView : M_ItemView
{
	private const string attributeTitle = "Attributes";
	private Color progressColor = Color.white;
	private Color backgroundColor = Color.black;

	public M_AttributeView(StatsController stats) : base(stats)
	{
		statsController.OnChange -= Rebuild;
		statsController.OnChange += Rebuild;
	}

	public M_AttributeView() : base()
	{

	}

	~M_AttributeView()
	{
		if (statsController == null) return;
		statsController.OnChange -= Rebuild;
	}

	protected override void SetTitle()
	{
		title = attributeTitle;
	}

	protected override void Rebuild()
	{
		if (statsController == null) return;

		Body.Clear();

		foreach (AttributeType key in statsController.Attributes.Keys)
		{
			var root = new VisualElement();
			root.style.flexDirection = FlexDirection.Row;

			statsController.TryGetAttribute(key, out Attribute attribute);

			var curValue = attribute.Value;
			var maxValue = attribute.MaxValue;

			ProgressBar progressBar = new ProgressBar();
			progressBar.style.alignSelf = Align.Center;

			var progress = progressBar.Q(className: "unity-progress-bar__progress");
			var background = progressBar.Q(className: "unity-progress-bar__background");

			progress.style.backgroundColor = progressColor;
			background.style.backgroundColor = backgroundColor;

			progressBar.style.width = 100f;
			progressBar.style.height = 10f;
			progressBar.value = curValue;
			progressBar.highValue = maxValue;
			root.Add(progressBar);

			Label label = new Label($"{key} : {curValue} / {maxValue}");
			label.style.fontSize = 12;
			label.style.unityFontStyleAndWeight = FontStyle.Bold;
			root.Add(label);

			Body.Add(root);
		}
	}
}
