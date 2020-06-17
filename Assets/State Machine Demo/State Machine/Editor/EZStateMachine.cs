using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class EZStateMachine : EditorWindow
{
    private string _animations_path = "Assets/Resources/Animations";
    private string _controller_path = "Assets/Resources/Controllers/MyEZController.controller";
    private bool _generate_animations;
    private GameObject _selected_game_object;
    private string _selection_status;
    private readonly List<string> _states = new List<string>();

    [MenuItem("Window/EZ State Machine")]
    public static void ShowWindow()
    {
        //show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(EZStateMachine));
    }

    public void OnSelectionChange()
    {
        if (Selection.gameObjects.Length == 1)
        {
            this._selected_game_object = Selection.gameObjects[0];
            this._selection_status = "Animation target: " + this._selected_game_object.name;
        }
        else
        {
            this._selected_game_object = null;
            this._selection_status = "Select a game object";
            this._states.Clear();
            this._generate_animations = true;
        }
        Repaint();
    }

    public void OnGUI()
    {
        GUILayout.Label(this._selection_status);
        if (this._selected_game_object != null)
        {
            this._controller_path = EditorGUILayout.TextField("Controller output path", this._controller_path);
            this._generate_animations = EditorGUILayout.Toggle("Create animation stubs", this._generate_animations);

            if (this._generate_animations)
                this._animations_path = EditorGUILayout.TextField("Animation output path", this._animations_path);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label("States:");
            if (GUILayout.Button("+"))
                this._states.Add("");
            if (GUILayout.Button("-"))
                this._states.RemoveAt(this._states.Count - 1);
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < this._states.Count; i++)
            {
                this._states[i] = EditorGUILayout.TextField(this._states[i]);
            }

            if (this._states.Count > 0)
                if (GUILayout.Button("Build State Machine"))
                {
                    //create the controller and state machine
                    AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(this._controller_path);
                    AnimatorStateMachine rootStateMachine = controller.layers[0].stateMachine;

                    //create states
                    foreach (string state in this._states)
                    {
                        //add state
                        AnimatorState animatorState = rootStateMachine.AddState(state.ToLower());

                        //add state trigger
                        controller.AddParameter(state.ToLower(), AnimatorControllerParameterType.Trigger);

                        //add transition from any state to this state
                        AnimatorStateTransition triggerTransition = rootStateMachine.AddAnyStateTransition(animatorState);
                        triggerTransition.AddCondition(AnimatorConditionMode.If, 0, state.ToLower());
                        triggerTransition.duration = 0;

                        //create clip and attach to state
                        if (this._generate_animations)
                        {
                            AnimationClip clip = new AnimationClip {name = state};
                            AnimationClipSettings settings = AnimationUtility.GetAnimationClipSettings(clip);
                            settings.loopTime = true;
                            AnimationUtility.SetAnimationClipSettings(clip, settings);
                            AssetDatabase.CreateAsset(clip, this._animations_path + "/" + state + ".anim");
                            animatorState.motion = clip;
                        }
                    }
                }
        }
    }
}