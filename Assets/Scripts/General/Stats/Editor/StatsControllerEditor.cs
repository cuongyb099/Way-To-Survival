using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomEditor(typeof(StatsController))]
public class StatsControllerEditor : Editor
{
	private VisualElement _Root;
	private VisualElement _Head;
	private VisualElement _Body;

	private const string _errorMessage = "Select Stats Holder";
	private const string _allAttributes = "All Attribute: ";
	private const string _allStats = "All Stats: ";
	

	private SerializedProperty _PropertyStatHolder;

	public override VisualElement CreateInspectorGUI()
    {
		_Root = new VisualElement();
		_Head = new VisualElement();
		_Body = new VisualElement();


		_Root.Add(_Head);
		_Root.Add(_Body);


		_PropertyStatHolder = serializedObject.FindProperty("_statsHolder");
		PropertyField fieldClass = new PropertyField(_PropertyStatHolder, string.Empty);
		
		RefreshBody();

		if (!EditorApplication.isPlayingOrWillChangePlaymode)
		{
			_Head.Add(fieldClass);
		}

		fieldClass.RegisterValueChangeCallback(_ => RefreshBody());
		return _Root;
    }

	private void RefreshBody()
	{
		serializedObject.Update();
		_Body.Clear();

		if (_PropertyStatHolder.objectReferenceValue == null)
		{
			_Body.Add(new HelpBox(_errorMessage, HelpBoxMessageType.Error));
			return;
		}

		StatsHolderSO statsHolder = _PropertyStatHolder.objectReferenceValue as StatsHolderSO;

		
		if(!target) return;
		bool playMode = EditorApplication.isPlayingOrWillChangePlaymode &&
						  !PrefabUtility.IsPartOfPrefabAsset(target);

		StatsController controller = target as StatsController;

		if (playMode)
		{
			_Body.Add(new M_AttributeView(controller));
			_Body.Add(new M_StatView(controller));
			_Body.Add(new M_StatusEffectView(controller));
			return;
		}

		InitBodyInEditor(statsHolder, controller);
	}

	private void InitBodyInEditor(StatsHolderSO statsHolder, StatsController controller)
	{
		var attributesView = new M_AttributeView();
		var statsView = new M_StatView();


		var labelAttribites = new Label(_allAttributes);
		labelAttribites.style.fontSize = 12;
		labelAttribites.style.unityFontStyleAndWeight = FontStyle.Bold;
		attributesView.Body.Add(labelAttribites);

		var labelStats = new Label(_allStats);
		labelStats.style.fontSize = 12;
		labelStats.style.unityFontStyleAndWeight = FontStyle.Bold;
		statsView.Body.Add(labelStats);

		foreach (AttributeType key in statsHolder.AttributeItems.Keys)
		{
			labelAttribites.text += key.ToString() + " | ";
		}

		foreach (StatType key in statsHolder.StatItems.Keys)
		{
			labelStats.text += key.ToString() + " | ";
		}

		_Body.Add(attributesView);
		_Body.Add(statsView);
	}
}