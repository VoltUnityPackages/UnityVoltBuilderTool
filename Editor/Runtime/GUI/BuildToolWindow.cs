﻿using UnityEditor;
using UnityEngine;
using VoltUnityBuilder.Actions;
using VoltUnityBuilder.Build;

namespace VoltUnityBuilder.GUI
{
	public sealed class BuildToolWindow : EditorWindow
	{
		[MenuItem("Tools/Volt Unity Builder/Volt Builder")]
		public static void ShowWindow()
		{
			GetWindow(typeof(BuildToolWindow), false, "Volt Builder");
		}

		[MenuItem("Tools/Volt Unity Builder/Report an issue")]
		public static void OpenIssues()
		{
			Application.OpenURL("https://github.com/VoltUnityPackages/UnityVoltBuilderTool/issues");
		}

		public void OnGUI()
		{
			//Main title
			EditorGUILayout.Space(2f);
			EditorGUILayout.LabelField("Volt Unity Builder", GUIStyles.TitleStyle);
			EditorGUILayout.Space(15f);

			BuildSettings.DrawOptions();
			BuildActions.DrawOptions();
			GameBuilder.DrawOptions();
		}
	}
}