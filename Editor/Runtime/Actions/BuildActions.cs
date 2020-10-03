﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Voltstro.UnityBuilder.Settings;

internal class BuildActions
{
	internal BuildActions()
	{
		activeBuildActions = new Dictionary<string, IBuildAction>();
		availableActions = AppDomain.CurrentDomain.GetAssemblies()
			.SelectMany(s => s.GetTypes())
			.Where(p => typeof(IBuildAction).IsAssignableFrom(p) && !p.IsInterface)
			.ToList();
		dropdownOptions = availableActions.Select(availableAction => availableAction.FullName).ToArray();

		foreach (string buildAction in SettingsManager.BuildActions)
		{
			AddBuildAction(buildAction);
		}
	}

	private static BuildActions instance;

	/// <summary>
	/// Active <see cref="BuildActions"/> instance
	/// </summary>
	internal static BuildActions Instance
	{
		get
		{
			if (instance == null)
				instance = new BuildActions();

			return instance;
		}
	}

	private List<Type> availableActions;
	private Dictionary<string, IBuildAction> activeBuildActions;
	private string[] dropdownOptions;

	private bool buildActions;

	internal void DrawOptions()
	{
		GUIStyles.DrawDropdownButton("Build Actions", ref buildActions);
		if (buildActions)
		{
			EditorGUILayout.BeginVertical(GUIStyles.DropdownContentStyle);

			if (availableActions.Count == 0)
			{
				EditorGUILayout.HelpBox("There are no build actions to add!", MessageType.Error, true);
				return;
			}

			//Drop down of available build actions
			int selectedIndex = 0;
			EditorGUILayout.BeginHorizontal();
			selectedIndex = EditorGUILayout.Popup(selectedIndex, dropdownOptions);

			//Add button
			if (GUILayout.Button("+"))
			{
				AddBuildAction(dropdownOptions[selectedIndex]);
			}

			EditorGUILayout.EndHorizontal();

			//Do OnGUI for each option
			foreach (KeyValuePair<string, IBuildAction> activeBuildAction in activeBuildActions)
			{
				EditorGUILayout.BeginVertical(GUIStyles.DropdownContentStyle);

				EditorGUILayout.LabelField(activeBuildAction.Key, GUIStyles.DropdownHeaderStyle);
				activeBuildAction.Value.OnGUI();

				EditorGUILayout.EndVertical();
			}

			EditorGUILayout.EndVertical();
		}
	}

	private void AddBuildAction(string action)
	{
		if(activeBuildActions.ContainsKey(action))
			return;

		//First, get if that action still exists
		Type actionType = availableActions.FirstOrDefault(x => x.FullName == action);
		if (actionType == null)
		{
			activeBuildActions.Add("", null);
			return;
		}

		//Now to create it
		IBuildAction buildAction = (IBuildAction)Activator.CreateInstance(actionType);
		activeBuildActions.Add(action, buildAction);

		List<string> currentBuildActions = SettingsManager.BuildActions;
		currentBuildActions.Add(action);
		SettingsManager.BuildActions = currentBuildActions;
	}
}