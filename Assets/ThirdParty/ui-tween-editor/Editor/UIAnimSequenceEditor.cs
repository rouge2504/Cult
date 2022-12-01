using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(UIAnimSequence))]
[CanEditMultipleObjects]
public class UIAnimSequenceEditor : Editor
{
	ReorderableList m_list;
	float[] m_itemHeights;

	struct StepCreationParams
	{
		public UIAnimSequence.ActionType Type;
	}

	void OnEnable()
	{
		//so we get continuous previews
		EditorApplication.update += ForceUpdate;

		m_list = new ReorderableList(serializedObject,
			serializedObject.FindProperty("Elements"),
			draggable: true, displayHeader: true,
			displayAddButton: true, displayRemoveButton: true);

		m_itemHeights = new float[m_list.count];

		m_list.drawHeaderCallback = (Rect rect) =>
		{
			EditorGUI.LabelField(rect, "Steps");
		};
		m_list.onAddDropdownCallback = OnAddDropdown;
		m_list.drawElementCallback = OnDrawElement;
		m_list.elementHeightCallback = (int index) =>
		{
			if (m_itemHeights.Length == m_list.count)
				return m_itemHeights[index];
			else
				return EditorGUIUtility.singleLineHeight * 5;
		};
	}

	void OnDisable()
	{
		//Debug.Log("removed forceupdate");
		EditorApplication.update -= ForceUpdate;
	}

	private string MakeStepLabel(SerializedProperty step, SerializedProperty listOfActions)
	{
		string objName = null;
		var tgtProp = step.FindPropertyRelative("Target");
		if (tgtProp.objectReferenceValue != null)
		{
			objName = tgtProp.objectReferenceValue.name + ":";
		}

		if (listOfActions.arraySize > 0)
		{
			var action = listOfActions.GetArrayElementAtIndex(0);
			var actionType = (UIAnimSequence.ActionType)action.FindPropertyRelative("Type").enumValueIndex;
			objName += actionType.ToString();
		}

		return objName;
	}

	void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
	{
		float origY = rect.y;
		float maxFieldWidth = 200;

		var element = m_list.serializedProperty.GetArrayElementAtIndex(index);
		EditorGUIUtility.labelWidth = 60; // Replace this with any width

		var listOfActions = element.FindPropertyRelative("Actions");

		//var actionType = (UIAnimSequence.ActionType)action.FindPropertyRelative("Type").enumValueIndex;
		string stepLabel = MakeStepLabel(element, listOfActions);
		//EditorGUI.LabelField(rect, new GUIContent(stepLabel), EditorStyles.boldLabel);
		//rect.y += EditorGUIUtility.singleLineHeight;

		var modeProp = element.FindPropertyRelative("LinkToPrevious");
		if (modeProp.boolValue == true)
			rect.x += 10f;

		element.isExpanded = EditorGUI.Foldout(new Rect(rect.x + 10, rect.y + 2.5f, maxFieldWidth, EditorGUIUtility.singleLineHeight), element.isExpanded, stepLabel, EditorStyles.foldout);
		rect.y += EditorGUIUtility.singleLineHeight + 2.5f;

		if (element.isExpanded)
		{
			//Start with previous is not selectable for first element
			bool wasEnabled = GUI.enabled;
			if (index == 0)
			{
				GUI.enabled = false;
				modeProp.boolValue = false;
			}
			float prevLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = 120f;
			EditorGUI.PropertyField(
				new Rect(rect.x, rect.y, maxFieldWidth, EditorGUIUtility.singleLineHeight),
				modeProp, new GUIContent("Start with Previous"), true);
			rect.y += EditorGUIUtility.singleLineHeight;
			if (index == 0)
				GUI.enabled = wasEnabled;
			EditorGUIUtility.labelWidth = prevLabelWidth;

			EditorGUI.PropertyField(
				new Rect(rect.x, rect.y, maxFieldWidth, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("Target"), new GUIContent("Target"), true);
			rect.y += EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField(
				new Rect(rect.x, rect.y, maxFieldWidth, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("Delay"), new GUIContent("Delay"));
			rect.y += EditorGUIUtility.singleLineHeight;
			EditorGUI.PropertyField(
				new Rect(rect.x, rect.y, maxFieldWidth, EditorGUIUtility.singleLineHeight),
				element.FindPropertyRelative("Duration"), new GUIContent("Duration"));
			rect.y += EditorGUIUtility.singleLineHeight;

			rect.y += EditorGUIUtility.singleLineHeight;
			DrawActionProperties(listOfActions, ref rect);
		}

		m_itemHeights[index] = rect.y - origY + EditorGUIUtility.singleLineHeight;
	}

	void DrawActionProperties(SerializedProperty listOfActions, ref Rect rect)
	{
		if (listOfActions.arraySize < 1)
			return;

		//Default drawing for the whole list
		//EditorGUI.PropertyField(rect, listOfActions, true);

		var action = listOfActions.GetArrayElementAtIndex(0);

		var actionType = (UIAnimSequence.ActionType)action.FindPropertyRelative("Type").enumValueIndex;
		EditorGUI.LabelField(rect, new GUIContent(actionType.ToString()), EditorStyles.boldLabel);
		rect.y += EditorGUIUtility.singleLineHeight;

		var proprect = new Rect(rect.x, rect.y, 200f, EditorGUIUtility.singleLineHeight);
		switch (actionType)
		{
			case UIAnimSequence.ActionType.Move:
				DrawEasingProperty(action, ref proprect);
				DrawSingleLineProperty("Vector", action, ref proprect);
				break;
			case UIAnimSequence.ActionType.RotateZ:
				DrawEasingProperty(action, ref proprect);
				DrawSingleLineProperty("Direction", action, ref proprect);
				DrawSingleLineProperty("Float", "Angle", action, ref proprect);
				break;
			case UIAnimSequence.ActionType.Scale:
				DrawEasingProperty(action, ref proprect);
				DrawSingleLineProperty("Direction", action, ref proprect);
				DrawSingleLineProperty("Vector", action, ref proprect);
				break;
			case UIAnimSequence.ActionType.Fade:
				DrawEasingProperty(action, ref proprect);
				DrawSingleLineProperty("Direction", action, ref proprect);
				DrawAlphaProperty("Float", "Alpha", action, ref proprect);
				//DrawSingleLineProperty("Float", action, ref proprect);
				break;
			case UIAnimSequence.ActionType.Fly:
				DrawEasingProperty(action, ref proprect);
				DrawSingleLineProperty("Direction", action, ref proprect);
				DrawSingleLineProperty("TravelDirection", "Travel", action, ref proprect);
				DrawSingleLineProperty("Float", "Distance", action, ref proprect);
				break;
			case UIAnimSequence.ActionType.Event:
				DrawMultiLineProperty("Event", action, ref proprect);
				break;
			case UIAnimSequence.ActionType.Shake:
				DrawEasingProperty(action, ref proprect);
				DrawSingleLineProperty("Vector", "Magnitude", action, ref proprect);
				DrawSingleLineProperty("Float", "Frequency", action, ref proprect);
				break;
		}

		rect.y = proprect.y;

	}

	void DrawAlphaProperty(string name, string label, SerializedProperty action, ref Rect rect)
	{
		var prop = action.FindPropertyRelative(name);
		prop.floatValue = Mathf.Clamp01(EditorGUI.FloatField(rect, new GUIContent(label), prop.floatValue));
		rect.y += EditorGUIUtility.singleLineHeight;
	}

	void DrawSingleLineProperty(string name, SerializedProperty action, ref Rect rect)
	{
		EditorGUI.PropertyField(rect, action.FindPropertyRelative(name));
		rect.y += EditorGUIUtility.singleLineHeight;
	}

	void DrawSingleLineProperty(string name, string label, SerializedProperty action, ref Rect rect)
	{
		EditorGUI.PropertyField(rect, action.FindPropertyRelative(name), new GUIContent(label));
		rect.y += EditorGUIUtility.singleLineHeight;
	}

	void DrawMultiLineProperty(string name, SerializedProperty action, ref Rect rect)
	{
		var prop = action.FindPropertyRelative(name);
		EditorGUI.PropertyField(rect, prop);
		rect.y += EditorGUI.GetPropertyHeight(prop);
	}

	void DrawEasingProperty(SerializedProperty action, ref Rect rect)
	{
		var prop = action.FindPropertyRelative("Easing");
		EditorGUI.PropertyField(rect, prop);
		rect.y += EditorGUI.GetPropertyHeight(prop);
		if (prop.enumValueIndex == (int)LeanTweenType.animationCurve)
		{
			prop = action.FindPropertyRelative("Curve");
			EditorGUI.PropertyField(rect, prop);
			rect.y += EditorGUI.GetPropertyHeight(prop);
		}
	}

	void AddItem(GenericMenu menu, string name, UIAnimSequence.ActionType t)
	{
		menu.AddItem(new GUIContent(name), false, CreateMenuClickHandler, new StepCreationParams { Type = t });
	}

	//Shortcuts to create actions of each type
	void OnAddDropdown(Rect buttonRect, ReorderableList l)
	{
		var menu = new GenericMenu();
		//Move is hidden because Fly fits most cases better
		//AddItem(menu, "Move", UIAnimSequence.ActionType.Move);
		AddItem(menu, "Fly", UIAnimSequence.ActionType.Fly);
		AddItem(menu, "Rotate", UIAnimSequence.ActionType.RotateZ);
		AddItem(menu, "Scale", UIAnimSequence.ActionType.Scale);
		AddItem(menu, "Fade", UIAnimSequence.ActionType.Fade);
		AddItem(menu, "Show", UIAnimSequence.ActionType.Show);
		AddItem(menu, "Hide", UIAnimSequence.ActionType.Hide);
		AddItem(menu, "Event", UIAnimSequence.ActionType.Event);
		AddItem(menu, "Shake", UIAnimSequence.ActionType.Shake);
		menu.ShowAsContext();
	}

	private void CreateMenuClickHandler(object target)
	{
		var data = (StepCreationParams)target;
		var index = m_list.serializedProperty.arraySize;
		m_list.serializedProperty.arraySize++;
		m_list.index = index;
		var element = m_list.serializedProperty.GetArrayElementAtIndex(index);
		element.FindPropertyRelative("Duration").floatValue = 1f;
		element.FindPropertyRelative("LinkToPrevious").boolValue = false;
		element.isExpanded = true;

		//add a new action
		var actionList = element.FindPropertyRelative("Actions");
		actionList.arraySize = 1;
		var action = actionList.GetArrayElementAtIndex(0);

		//set action parameters based on type
		CreateActionForStep(data, action);

		serializedObject.ApplyModifiedProperties();
	}

	void CreateActionForStep(StepCreationParams data, SerializedProperty property)
	{
		property.FindPropertyRelative("Type").enumValueIndex = (int)data.Type;
		var fprop = property.FindPropertyRelative("Float");
		var vprop = property.FindPropertyRelative("Vector");
		switch (data.Type)
		{
			//Set some sensible initial values here
			case UIAnimSequence.ActionType.Shake:
				{
					fprop.floatValue = 8f;
					vprop.vector3Value = new Vector3(10, 10, 0);
					break;
				}
			default:
				{
					fprop.floatValue = 0f;
					vprop.vector3Value = Vector3.zero;
					break;
				}
		}
	}

	public override void OnInspectorGUI()
	{
		var tgt = (UIAnimSequence)target;

		//if (tgt.Animating)
		//{
		//	GUILayout.Label("Running...");
		//	GUI.enabled = false;
		//}
		//else
		//{
		//	GUILayout.Label("Idle");
		//	GUI.enabled = true;
		//}

		//if (GUILayout.Button("Preview"))
		//{
		//	tgt.EditorPreview();
		//}

		bool wasEnabled = GUI.enabled;
		GUI.enabled = true;

		//if (GUILayout.Button("Stop preview"))
		//{
		//	ResetAfterPreview();
		//}
		
		GUI.enabled = wasEnabled;

		if (m_itemHeights.Length != m_list.count)
		{
			m_itemHeights = new float[m_list.count];
			//to force relayout and set heights
			EditorUtility.SetDirty(tgt);
		}

		serializedObject.Update();
		EditorGUIUtility.labelWidth = 200;
		EditorGUILayout.PropertyField(serializedObject.FindProperty("playOnEnable"), true, new GUILayoutOption[0]);
		EditorGUILayout.PropertyField(serializedObject.FindProperty("resetOnDisable"), true, new GUILayoutOption[0]);
		//EditorGUILayout.PropertyField(serializedObject.FindProperty("hideOnComplete"), true, new GUILayoutOption[0]);
		m_list.DoLayoutList();
		serializedObject.ApplyModifiedProperties();
	}

	void ForceUpdate()
	{
		var tgt = (UIAnimSequence)target;
		if (tgt != null)
		{
			tgt.EditorForceUpdate();
			//Dirtying seems to be the only way to reliably do this
			if (tgt.Animating)
			{
				//	Repaint();
				EditorUtility.SetDirty(tgt);
			}
		}
	}

	//void ResetAfterPreview()
	//{
	//	LeanTween.reset();
	//	var tgt = (UIAnimSequence)target;
	//	tgt.ResetToPreActionState();
	//}
}
