using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public class M_StatusEffectView : M_ItemView
{
	private const string _statusEffect = "Status Effect";
	private readonly Color backgroundColor = new Color(0.345098f, 0.345098f, 0.345098f);
	private List<BaseStatusEffect> m_statusEffects = new();
	
	public M_StatusEffectView(StatsController stats) : base(stats)
	{
        statsController.OnChange -= Rebuild;
		statsController.OnChange += Rebuild;
	}

	~M_StatusEffectView()
	{
		if (statsController == null) return;
		statsController.OnChange -= Rebuild;
	}
	
	protected override void SetTitle()
	{
		title = _statusEffect;
	}

	protected override void Rebuild()
	{
		if (statsController == null) return;

		Body.Clear();
		
		foreach (var m_effect in statsController.StatusEffects)
		{
			m_statusEffects.Add(m_effect);
			
			var root = new VisualElement
			{
				style =
				{
					flexDirection = FlexDirection.Row,
					justifyContent = Justify.SpaceBetween,
					alignItems = Align.Center,
					backgroundColor = backgroundColor,
					marginBottom = 3
				}
			};

			SetBorderColor(root, Color.black, 1);
	
			var tmp = m_effect.Data.Name.GetLocalizedString().Equals("") ? m_effect.Data.name : m_effect.Data.Name.GetLocalizedString();
			
			Label label = new Label($"{tmp}")
			{
				style =
				{
					paddingLeft = 8,
					fontSize = 12,
					unityFontStyleAndWeight = FontStyle.Bold
				}
			};
			string stack = m_effect.Data.Stackable ? $"Stack : {m_effect.CurrentStack}      " : string.Empty; 
			Label label2 = new Label($"{stack} Duration : {m_effect.Data.Duration}s")
			{
				style =
				{
					paddingRight = 8,
					fontSize = 12,
					unityFontStyleAndWeight = FontStyle.Bold
				}
			};
			
			root.Add(label);
			root.Add(label2);
			Body.Add(root);
		}
		
		/*foreach (StatType key in statsController.Stats.Keys)
		{
			var root = new VisualElement
			{
				style =
				{
					flexDirection = FlexDirection.Row,
					justifyContent = Justify.SpaceBetween,
					alignItems = Align.Center,
					backgroundColor = backgroundColor,
					marginBottom = 3
				}
			};

			SetBorderColor(root, Color.black, 1);

			var stats = statsController.Stats[key];

			Label label = new Label($"{key} : {stats.Value}");
			label.style.paddingLeft = 8;
			label.style.fontSize = 12;
			label.style.unityFontStyleAndWeight = FontStyle.Bold;


			Label label2 = new Label();

			float modifier = stats.Value - stats.BaseValue;
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
		}*/
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
