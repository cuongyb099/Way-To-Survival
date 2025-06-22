using UnityEngine;
using UnityEngine.UIElements;

public class M_StatView : M_ItemView
{
	private const string statTitle = "Stats";
	private readonly Color backgroundColor = new Color(0.345098f, 0.345098f, 0.345098f);
	
	public M_StatView(StatsController stats) : base(stats)
	{
		statsController.OnChange -= Rebuild;
		statsController.OnChange += Rebuild;
	}

	public M_StatView()
	{

	}

	~M_StatView()
	{
		if (statsController == null) return;
		statsController.OnChange -= Rebuild;
	}

	protected override void SetTitle()
	{
		title = statTitle;
	}

	protected override void Rebuild()
	{
		if (statsController == null) return;

		Body.Clear();

		foreach (StatType key in statsController.Stats.Keys)
		{
			var root = new VisualElement();

			root.style.flexDirection = FlexDirection.Row;
			root.style.justifyContent = Justify.SpaceBetween;
			root.style.alignItems = Align.Center;

			root.style.backgroundColor = backgroundColor;
			root.style.marginBottom = 3;
			SetBorderColor(root, Color.black, 1);

			var stat = statsController.Stats[key];

			Label label = new Label($"{key} : {stat.Value}");
			label.style.paddingLeft = 8;
			label.style.fontSize = 12;
			label.style.unityFontStyleAndWeight = FontStyle.Bold;


			

			Label label2 = new Label();

			float modifier = stat.Value - stat.BaseValue;
			string stringModifier = modifier.ToString();

			if (modifier < 0)
			{
				label2.style.color = Color.red;
			}
			else if(modifier > 0)
			{
				stringModifier = $"+ {stringModifier}";
				label2.style.color = Color.green;
			}
			// == 0 Base Color

			label2.text = stringModifier;

			label2.style.paddingRight = 8;
			label2.style.fontSize = 12;
			label2.style.unityFontStyleAndWeight = FontStyle.Bold;

			root.Add(label);
			root.Add(label2);

			Body.Add(root);
		}
	}

	private void SetBorderColor(VisualElement root, Color color, float width)
	{
		root.style.borderTopColor = color;
		root.style.borderTopWidth = width;
		root.style.borderBottomColor = color;
		root.style.borderBottomWidth = width;
		root.style.borderLeftColor = color;
		root.style.borderLeftWidth = width;
		root.style.borderRightColor = color;
		root.style.borderRightWidth = width;
	}
}
