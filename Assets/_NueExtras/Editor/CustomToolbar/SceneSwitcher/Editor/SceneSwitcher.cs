using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityToolbarExtender;

namespace _NueExtras.Editor.CustomToolbar.SceneSwitcher.Editor
{
	static class ToolbarStyles
	{
		public static readonly GUIStyle commandButtonStyle;

		static ToolbarStyles()
		{
			commandButtonStyle = new GUIStyle("Command")
			{
				fontSize = 16,
				alignment = TextAnchor.MiddleCenter,
				imagePosition = ImagePosition.ImageAbove,
				fontStyle = FontStyle.Bold
			};
		}
	}

	[InitializeOnLoad]
	public class SceneSwitchLeftButton
	{
		static SceneSwitchLeftButton()
		{
			ToolbarExtender.LeftToolbarGUI.Add(OnToolbarGUI);
		}

		static void OnToolbarGUI()
		{
			GUILayout.FlexibleSpace();
			var sceneCount =SceneManager.sceneCountInBuildSettings;
			for (int i = 0; i < sceneCount; i++)
			{
				if(GUILayout.Button(new GUIContent($"{i}", $"SCENE: {i}"), ToolbarStyles.commandButtonStyle))
				{
					if(EditorApplication.isPlaying)
						EditorApplication.isPlaying = false;
					
					if (EditorApplication.isPlaying || EditorApplication.isPaused ||
					    EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
					{
						return;
					}
					
					if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
					{
						EditorSceneManager.OpenScene(SceneUtility.GetScenePathByBuildIndex(i));
						//EditorApplication.isPlaying = true;
					}
				}
			}
			
			if(GUILayout.Button(new GUIContent($"A", $"SCENE: ART"), ToolbarStyles.commandButtonStyle))
			{
				if(EditorApplication.isPlaying)
					EditorApplication.isPlaying = false;
					
				if (EditorApplication.isPlaying || EditorApplication.isPaused ||
				    EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
				{
					return;
				}
					
				if(EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
				{
					EditorSceneManager.OpenScene("Assets/__Project/Scenes/Art Scene.unity");
					//EditorApplication.isPlaying = true;
				}
			}
			
		}
	}
}