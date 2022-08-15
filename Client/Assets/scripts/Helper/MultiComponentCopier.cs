#if UNITY_EDITOR
#region Libraries
using UnityEditor;
using UnityEngine;
#endregion

#region Multi-Component Copier
public class MultiComponentCopierWindow : EditorWindow
{
    // Add menu item named "MultiPropertyCopier" to the 'Knights' Menu Section
    [MenuItem("Utility/Knights/Multi-Component Copier")]
    public static void ShowWindow() => GetWindow(typeof(MultiComponentCopier));
}

public class MultiComponentCopier : EditorWindow
{
    Variables_MultiComponentCopier variables_MultiComponentCopier;

    public void OnGUI()
    {
        // Just instantiate once
        if (variables_MultiComponentCopier == null) { variables_MultiComponentCopier = new Variables_MultiComponentCopier(); }

        // ~ Multi-Component Copier
        // Title
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        GUILayout.Label("Multi-Component Copier", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        // Tabs Fwd
        EditorGUI.indentLevel++;

        // Buttons
        EditorGUILayout.BeginHorizontal();
        // Instructions button
        if (GUILayout.Button("?", GUILayout.Width(16), GUILayout.Height(16))) GetWindow(typeof(Instructions));
        // Separated with space
        EditorGUILayout.Space();
        // Exit button
        if (GUILayout.Button("X", GUILayout.Width(16), GUILayout.Height(16))) Close();
        EditorGUILayout.EndHorizontal();

        // To separate the buttons from the progress bar
        EditorGUILayout.Space();

        // Progress Bar Rect
        variables_MultiComponentCopier.progressBarRect = EditorGUILayout.BeginVertical();

        // Just a dummy line, doesn't do anything special but keeps the structure together to display progress bar
        GUILayout.Label("Dummy Bar");
        GUILayout.Label("Disable this line if you want to make the progress bar thinner");

        // Progress Bar Color
        GUI.color = variables_MultiComponentCopier.progressBarColor;

        // Progress Bar
        if (variables_MultiComponentCopier.progressIndex != 0 && variables_MultiComponentCopier.destinationGameObjects.Length != 0 && !variables_MultiComponentCopier.progressError)
        {
            UpdateProgressBarCompletedText();
            EditorGUI.ProgressBar(variables_MultiComponentCopier.progressBarRect,
                variables_MultiComponentCopier.progressIndex / variables_MultiComponentCopier.destinationGameObjects.Length, variables_MultiComponentCopier.progressBarText);
        }
        else if (variables_MultiComponentCopier.progressError) EditorGUI.ProgressBar(variables_MultiComponentCopier.progressBarRect, 0f / 1f, variables_MultiComponentCopier.progressBarText);
        else { EditorGUI.ProgressBar(variables_MultiComponentCopier.progressBarRect, 0f / 1f, "DRAG & DROP AND PRESS COPY"); ColorProgressBar(variables_MultiComponentCopier.idleColor); }

        // Reset Color
        GUI.color = variables_MultiComponentCopier.idleColor;

        EditorGUILayout.EndVertical();

        // Scrollable
        EditorGUILayout.BeginVertical();
        variables_MultiComponentCopier.scrollPos = EditorGUILayout.BeginScrollView(variables_MultiComponentCopier.scrollPos, false, false);

        // Space
        EditorGUILayout.Space();

        // Serialized Variables_MultiComponentCopier
        SerializedObject serializedObject = new SerializedObject(variables_MultiComponentCopier);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("component"), true);

        // Script or Other Component Name
        if (variables_MultiComponentCopier.component == Variables_MultiComponentCopier.Components.Script)
            variables_MultiComponentCopier.componentName = EditorGUILayout.TextField("Script Name", variables_MultiComponentCopier.componentName);
        else if (variables_MultiComponentCopier.component == Variables_MultiComponentCopier.Components.Other)
            variables_MultiComponentCopier.componentName = EditorGUILayout.TextField("Component Name", variables_MultiComponentCopier.componentName);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Recursive");
        variables_MultiComponentCopier.recursive = EditorGUILayout.Toggle(variables_MultiComponentCopier.recursive);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("sourceGameObjects"), true);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("destinationGameObjects"), true);
        serializedObject.ApplyModifiedProperties();

        // Sequence String
        variables_MultiComponentCopier.sequence = EditorGUILayout.TextField("Sequence", variables_MultiComponentCopier.sequence);

        // More Space
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // Copy Button
        if (variables_MultiComponentCopier.stupidIndex < 5)
            if (GUILayout.Button("COPY", GUILayout.Height(50))) { variables_MultiComponentCopier.previousProperty = variables_MultiComponentCopier.component; CopyPaste(); }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();

        // Reset Enum
        if (variables_MultiComponentCopier.previousProperty != variables_MultiComponentCopier.component) { variables_MultiComponentCopier.stupidIndex = 0; ResetProgressBar(); ColorProgressBar(variables_MultiComponentCopier.idleColor); }

        // Keep track of recursive toggle state
        if (variables_MultiComponentCopier.recursive != variables_MultiComponentCopier.recursiveToggled) { ResetProgressBar(); ColorProgressBar(variables_MultiComponentCopier.idleColor); variables_MultiComponentCopier.recursiveToggled = variables_MultiComponentCopier.recursive; }
    }

    #region Component Functions
    public void CopyPaste()
    {
        ColorProgressBar(variables_MultiComponentCopier.loadingColor);
        ResetProgressBar();

        // Check if Array has empty values
        if (variables_MultiComponentCopier.sourceGameObjects != null && variables_MultiComponentCopier.sourceGameObjects.Length > 0)
        {
            for (int i = 0; i < variables_MultiComponentCopier.sourceGameObjects.Length; i++)
            {
                if (variables_MultiComponentCopier.sourceGameObjects[i] == null) variables_MultiComponentCopier.missingObjects = true;
            }
        }
        if (variables_MultiComponentCopier.destinationGameObjects != null && variables_MultiComponentCopier.destinationGameObjects.Length > 0)
        {
            for (int i = 0; i < variables_MultiComponentCopier.destinationGameObjects.Length; i++)
            {
                if (variables_MultiComponentCopier.destinationGameObjects[i] == null) variables_MultiComponentCopier.missingObjects = true;
            }
        }

        #region Main Source
        // Default Sequence
        if (string.IsNullOrEmpty(variables_MultiComponentCopier.sequence))
        {
            // If both 0
            if (variables_MultiComponentCopier.sourceGameObjects == null || variables_MultiComponentCopier.destinationGameObjects == null
                || variables_MultiComponentCopier.sourceGameObjects.Length == 0 || variables_MultiComponentCopier.destinationGameObjects.Length == 0
                || variables_MultiComponentCopier.missingObjects)
            {
                switch (variables_MultiComponentCopier.stupidIndex)
                {
                    case 0:
                        variables_MultiComponentCopier.progressBarText = "ADD SOMETHING BEFORE YOU COPY";
                        ColorProgressBar(variables_MultiComponentCopier.errorColor);
                        break;
                    case 1:
                        variables_MultiComponentCopier.progressBarText = "CAN'T YOU READ? ARE YOU DUMB, BRO? \n ADD SOMETHING BEFORE YOU COPY";
                        ColorProgressBar(variables_MultiComponentCopier.errorColor);
                        break;
                    case 2:
                        variables_MultiComponentCopier.progressBarText = "CONGRATULATIONS, YOU'RE A CERTIFIED IDIOT. \n ADD SOMETHING BEFORE YOU COPY!";
                        ColorProgressBar(variables_MultiComponentCopier.errorColor);
                        break;
                    case 3:
                        variables_MultiComponentCopier.progressBarText = "YOU REALLY LIKE PRESSING BUTTONS, \n DON'T YOU?";
                        ColorProgressBar(variables_MultiComponentCopier.errorColor);
                        break;
                    case 4:
                        variables_MultiComponentCopier.progressBarText = "HOW ABOUT NOW? \n NO MORE BUTTONS FOR YOU :(";
                        ColorProgressBar(variables_MultiComponentCopier.errorColor);
                        break;
                }

                if (variables_MultiComponentCopier.stupidIndex < 5) variables_MultiComponentCopier.stupidIndex++; // Give up if too stupid
            }
            // If both are of the same length
            else if (variables_MultiComponentCopier.sourceGameObjects.Length == variables_MultiComponentCopier.destinationGameObjects.Length &&
                // But it's not empty
                !variables_MultiComponentCopier.missingObjects)
            {
                for (int i = 0; i < variables_MultiComponentCopier.destinationGameObjects.Length; i++)
                {
                    switch (variables_MultiComponentCopier.component)
                    {
                        case Variables_MultiComponentCopier.Components.Animator:
                            // Animator
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<Animator>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<Animator>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Animator>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Animator>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<Animator>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<Animator>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<Animator>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Animator>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Animator>());
                            }
                            break;
                        case Variables_MultiComponentCopier.Components.Collider:
                            #region Colliders
                            // ~ 2D
                            // Box Collider 2D
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<BoxCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<BoxCollider2D>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<BoxCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<BoxCollider2D>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<BoxCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<BoxCollider2D>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<BoxCollider2D>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<BoxCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<BoxCollider2D>());
                            }

                            // Capsule Collider 2D
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<CapsuleCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<CapsuleCollider2D>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<CapsuleCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<CapsuleCollider2D>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<CapsuleCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<CapsuleCollider2D>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<CapsuleCollider2D>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<CapsuleCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<CapsuleCollider2D>());
                            }

                            // Circle Collider 2D
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<CircleCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<CircleCollider2D>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<CircleCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<CircleCollider2D>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<CircleCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<CircleCollider2D>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<CircleCollider2D>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<CircleCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<CircleCollider2D>());
                            }

                            // Composite Collider 2D
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<CompositeCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<CompositeCollider2D>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<CompositeCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<CompositeCollider2D>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<CompositeCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<CompositeCollider2D>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<CompositeCollider2D>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<CompositeCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<CompositeCollider2D>());
                            }

                            // Edge Collider 2D
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<EdgeCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<EdgeCollider2D>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<EdgeCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<EdgeCollider2D>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<EdgeCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<EdgeCollider2D>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<EdgeCollider2D>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<EdgeCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<EdgeCollider2D>());
                            }

                            // Polygon Collider 2D
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<PolygonCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<PolygonCollider2D>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<PolygonCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<PolygonCollider2D>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<PolygonCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<PolygonCollider2D>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<PolygonCollider2D>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<PolygonCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<PolygonCollider2D>());
                            }

                            // ~ 3D
                            // Box Collider 3D
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<BoxCollider>() != null
                               && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<BoxCollider>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<BoxCollider>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<BoxCollider>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<BoxCollider>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<BoxCollider>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<BoxCollider>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<BoxCollider>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<BoxCollider>());
                            }

                            // Capsule Collider 3D
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<CapsuleCollider2D>() != null
                              && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<CapsuleCollider2D>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<CapsuleCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<CapsuleCollider2D>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<CapsuleCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<CapsuleCollider2D>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<CapsuleCollider2D>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<CapsuleCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<CapsuleCollider2D>());
                            }

                            // Mesh Collider 3D
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<MeshCollider>() != null
                              && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<MeshCollider>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<MeshCollider>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<MeshCollider>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<MeshCollider>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<MeshCollider>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<MeshCollider>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<MeshCollider>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<MeshCollider>());
                            }

                            // Sphere Collider 3D
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<SphereCollider>() != null
                              && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<SphereCollider>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<SphereCollider>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<SphereCollider>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<SphereCollider>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<SphereCollider>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<SphereCollider>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<SphereCollider>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<SphereCollider>());
                            }

                            // Terrain Collider 3D
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<TerrainCollider>() != null
                              && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<TerrainCollider>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<TerrainCollider>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<TerrainCollider>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<TerrainCollider>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<TerrainCollider>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<TerrainCollider>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<TerrainCollider>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<TerrainCollider>());
                            }

                            // Wheel Collider 3D
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<WheelCollider>() != null
                              && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<WheelCollider>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<WheelCollider>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<WheelCollider>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<WheelCollider>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<WheelCollider>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<WheelCollider>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<WheelCollider>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<WheelCollider>());
                            }
                            #endregion
                            break;
                        case Variables_MultiComponentCopier.Components.Material:
                            // Mesh Renderers
                            // Usually for static meshes
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<MeshRenderer>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<MeshRenderer>() != null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<MeshRenderer>().sharedMaterial =
                                    variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<MeshRenderer>().sharedMaterial;
                            }

                            // Skinned Mesh Renderers
                            // Usually for rigged meshes with skinning
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<SkinnedMeshRenderer>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<SkinnedMeshRenderer>() != null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<SkinnedMeshRenderer>().sharedMaterial =
                                variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<SkinnedMeshRenderer>().sharedMaterial;
                            }
                            break;
                        case Variables_MultiComponentCopier.Components.MeshRenderer:
                            // Mesh Filter
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<MeshFilter>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<MeshFilter>() != null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<MeshFilter>().sharedMesh =
                                    variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<MeshFilter>().mesh;
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<MeshFilter>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<MeshFilter>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<MeshFilter>();
                                variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<MeshFilter>().sharedMesh =
                                    variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<MeshFilter>().mesh;
                            }

                            // Mesh Renderers
                            // Usually for static meshes
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<MeshRenderer>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<MeshRenderer>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<MeshRenderer>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<MeshRenderer>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<MeshRenderer>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<MeshRenderer>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<MeshRenderer>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<MeshRenderer>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<MeshRenderer>());
                            }

                            // Skinned Mesh Renderers
                            // Usually for rigged meshes with skinning
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<SkinnedMeshRenderer>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<SkinnedMeshRenderer>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<SkinnedMeshRenderer>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<SkinnedMeshRenderer>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<SkinnedMeshRenderer>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<SkinnedMeshRenderer>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<SkinnedMeshRenderer>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<SkinnedMeshRenderer>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<SkinnedMeshRenderer>());
                            }
                            break;
                        case Variables_MultiComponentCopier.Components.Rigidbody:
                            // Rigidbody 2D
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<Rigidbody2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<Rigidbody2D>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Rigidbody2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Rigidbody2D>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<Rigidbody2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<Rigidbody2D>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<Rigidbody2D>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Rigidbody2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Rigidbody2D>());
                            }

                            // Rigidbody 3D
                            // Update
                            if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<Rigidbody2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<Rigidbody2D>() != null)
                            {
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Rigidbody2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Rigidbody2D>());
                            }
                            // Add
                            else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<Rigidbody2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<Rigidbody2D>() == null)
                            {
                                variables_MultiComponentCopier.destinationGameObjects[i].AddComponent<Rigidbody2D>();
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Rigidbody2D>(), variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Rigidbody2D>());
                            }
                            break;
                        case Variables_MultiComponentCopier.Components.Script:
                            // Just a safety check to confirm if script is actually in the project
                            if (!string.IsNullOrEmpty(variables_MultiComponentCopier.componentName))
                            {
                                for (int k = 0; k < variables_MultiComponentCopier.sourceGameObjects.Length; k++)
                                {
                                    for (int comp = 0; comp < variables_MultiComponentCopier.sourceGameObjects[k].GetComponents<Component>().Length; comp++)
                                    {
                                        if (variables_MultiComponentCopier.sourceGameObjects[k].GetComponents<Component>()[comp].GetType().ToString().ToLower() == variables_MultiComponentCopier.componentName.ToLower())
                                        {
                                            Component scriptComponent = variables_MultiComponentCopier.sourceGameObjects[k].GetComponents<Component>()[comp];
                                            variables_MultiComponentCopier.destinationGameObjects[k].AddComponent(scriptComponent.GetType());
                                        }
                                    }
                                }
                            }
                            break;
                        case Variables_MultiComponentCopier.Components.Transform:
                            // Transform
                            // Update
                            EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[i].transform, variables_MultiComponentCopier.destinationGameObjects[i].transform);
                            break;
                        case Variables_MultiComponentCopier.Components.Other:
                            // Other Component Name
                            if (!string.IsNullOrEmpty(variables_MultiComponentCopier.componentName))
                            {
                                for (int k = 0; k < variables_MultiComponentCopier.sourceGameObjects.Length; k++)
                                {
                                    for (int comp = 0; comp < variables_MultiComponentCopier.sourceGameObjects[k].GetComponents<Component>().Length; comp++)
                                    {
                                        if (variables_MultiComponentCopier.sourceGameObjects[k].GetComponents<Component>()[comp].GetType().ToString().ToLower() == variables_MultiComponentCopier.componentName.ToLower())
                                        {
                                            Component scriptComponent = variables_MultiComponentCopier.sourceGameObjects[k].GetComponents<Component>()[comp];
                                            variables_MultiComponentCopier.destinationGameObjects[k].AddComponent(scriptComponent.GetType());
                                        }
                                    }
                                }
                            }
                            break;
                    }

                    // Recursively update children too
                    if (variables_MultiComponentCopier.recursive)
                    {
                        CopyPasteToChildren(variables_MultiComponentCopier.sourceGameObjects[i], variables_MultiComponentCopier.destinationGameObjects[i], variables_MultiComponentCopier.component);
                    }

                    variables_MultiComponentCopier.progressIndex++;
                }
            }
            // If both are not the same length
            else
            {
                variables_MultiComponentCopier.progressBarText = "SOURCE & DESTINATION OBJECT LENGTH MISMATCH. \n TRY USING A SEQUENCE?";
                ColorProgressBar(variables_MultiComponentCopier.errorColor);
            }
        }
        // Custom Sequence
        else
        {
            // Main sequence variables_MultiComponentCopier
            int numberOfSequences = variables_MultiComponentCopier.sequence.Split(',').Length;
            string[] splitsOfSequence = variables_MultiComponentCopier.sequence.Split(',');
            string[] sequences;
            int indexOfSequence;
            int repetitionOfSequence;
            // For-loop variables_MultiComponentCopier
            int previousRepetitionPosition = 0;

            // If both 0
            if ((variables_MultiComponentCopier.sourceGameObjects == null || variables_MultiComponentCopier.destinationGameObjects == null
                || variables_MultiComponentCopier.sourceGameObjects.Length == 0 || variables_MultiComponentCopier.destinationGameObjects.Length == 0) && numberOfSequences > 0)
            {
                switch (variables_MultiComponentCopier.stupidIndex)
                {
                    case 0:
                        variables_MultiComponentCopier.progressBarText = "ADD SOMETHING BEFORE YOU COPY";
                        ColorProgressBar(variables_MultiComponentCopier.errorColor);
                        break;
                    case 1:
                        variables_MultiComponentCopier.progressBarText = "CAN'T YOU READ? ARE YOU DUMB, BRO? \n ADD SOMETHING BEFORE YOU COPY";
                        ColorProgressBar(variables_MultiComponentCopier.errorColor);
                        break;
                    case 2:
                        variables_MultiComponentCopier.progressBarText = "CONGRATULATIONS, YOU'RE A CERTIFIED IDIOT. \n ADD SOMETHING BEFORE YOU COPY!";
                        ColorProgressBar(variables_MultiComponentCopier.errorColor);
                        break;
                    case 4:
                        variables_MultiComponentCopier.progressBarText = "HOW ABOUT NOW? \n NO MORE BUTTONS FOR YOU :(";
                        ColorProgressBar(variables_MultiComponentCopier.errorColor);
                        break;
                }

                if (variables_MultiComponentCopier.stupidIndex < 5) variables_MultiComponentCopier.stupidIndex++; // Give up if too stupid
            }
            // If both are not empty
            else if (variables_MultiComponentCopier.sourceGameObjects.Length == numberOfSequences)
            {
                for (int i = 0; i < numberOfSequences; i++)
                {
                    sequences = splitsOfSequence[i].Split('-');
                    indexOfSequence = int.Parse(sequences[0]); // Index (For example, the '0' in 0-5)
                    repetitionOfSequence = int.Parse(sequences[1]); // Repetition amount (For example, the '5' in 0-5)

                    for (int j = previousRepetitionPosition; j < repetitionOfSequence + previousRepetitionPosition; j++)
                    {
                        // Safety check if asked to repeat more than the provided amount
                        if (repetitionOfSequence > variables_MultiComponentCopier.destinationGameObjects.Length)
                        {
                            variables_MultiComponentCopier.progressBarText = "IMPROPER SEQUENCE. \n CAN'T FIND " + repetitionOfSequence + " DESTINATION OBJECTS.";
                            ColorProgressBar(variables_MultiComponentCopier.errorColor);
                            break;
                        }
                        // If sequence index > the number of source objects (For example, 0-1,2-3 for only 2 source objects)
                        else if (numberOfSequences > 0 && indexOfSequence + 1 > variables_MultiComponentCopier.sourceGameObjects.Length)
                        {
                            variables_MultiComponentCopier.progressError = true;
                            variables_MultiComponentCopier.progressBarText = "YOU'RE CALLING SOURCE OBJECT # " + (indexOfSequence + 1) + ". \n WE ONLY HAVE " + variables_MultiComponentCopier.sourceGameObjects.Length + ".";
                            ColorProgressBar(variables_MultiComponentCopier.errorColor);
                            break;
                        }
                        // If user enters to override an already used Object for example (0-1,1-4) for a total of 4 Destination Objects
                        else if (indexOfSequence != 0 && repetitionOfSequence >= variables_MultiComponentCopier.destinationGameObjects.Length)
                        {
                            variables_MultiComponentCopier.progressError = true;
                            variables_MultiComponentCopier.progressBarText = "PLEASE DON'T OVERRIDE EXISTING OBJECT IN SEQUENCE. \n FIX YOUR SEQUENCE AND TRY AGAIN.";
                            ColorProgressBar(variables_MultiComponentCopier.errorColor);
                            break;
                        }

                        switch (variables_MultiComponentCopier.component)
                        {
                            case Variables_MultiComponentCopier.Components.Animator:
                                // Animator
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<Animator>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<Animator>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Animator>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Animator>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<Animator>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<Animator>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<Animator>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Animator>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Animator>());
                                }
                                break;
                            case Variables_MultiComponentCopier.Components.Collider:
                                #region Colliders
                                // ~ 2D
                                // Box Collider 2D
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<BoxCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<BoxCollider2D>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<BoxCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<BoxCollider2D>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<BoxCollider2D>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<BoxCollider2D>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<BoxCollider2D>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<BoxCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<BoxCollider2D>());
                                }

                                // Capsule Collider 2D
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<CapsuleCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<CapsuleCollider2D>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<CapsuleCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<CapsuleCollider2D>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<CapsuleCollider2D>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<CapsuleCollider2D>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<CapsuleCollider2D>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<CapsuleCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<CapsuleCollider2D>());
                                }

                                // Circle Collider 2D
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<CircleCollider2D>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<CircleCollider2D>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<CircleCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<CircleCollider2D>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<CircleCollider2D>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<CircleCollider2D>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<CircleCollider2D>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<CircleCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<CircleCollider2D>());
                                }

                                // Composite Collider 2D
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<CompositeCollider2D>() != null
                                 && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<CompositeCollider2D>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<CompositeCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<CompositeCollider2D>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<CompositeCollider2D>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<CompositeCollider2D>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<CompositeCollider2D>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<CompositeCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<CompositeCollider2D>());
                                }

                                // Edge Collider 2D
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<EdgeCollider2D>() != null
                                 && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<EdgeCollider2D>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<EdgeCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<EdgeCollider2D>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<EdgeCollider2D>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<EdgeCollider2D>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<EdgeCollider2D>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<EdgeCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<EdgeCollider2D>());
                                }

                                // Polygon Collider 2D
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<PolygonCollider2D>() != null
                                 && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<PolygonCollider2D>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<PolygonCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<PolygonCollider2D>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<PolygonCollider2D>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<PolygonCollider2D>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<PolygonCollider2D>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<PolygonCollider2D>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<PolygonCollider2D>());
                                }

                                // ~ 3D
                                // Box Collider 3D
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<BoxCollider>() != null
                                 && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<BoxCollider>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<BoxCollider>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<BoxCollider>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<BoxCollider>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<BoxCollider>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<BoxCollider>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<BoxCollider>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<BoxCollider>());
                                }

                                // Capsule Collider 3D
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<CapsuleCollider>() != null
                                 && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<CapsuleCollider>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<CapsuleCollider>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<CapsuleCollider>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<CapsuleCollider>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<CapsuleCollider>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<CapsuleCollider>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<CapsuleCollider>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<CapsuleCollider>());
                                }

                                // Mesh Collider 3D
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<MeshCollider>() != null
                                 && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<MeshCollider>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<MeshCollider>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<MeshCollider>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<MeshCollider>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<MeshCollider>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<MeshCollider>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<MeshCollider>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<MeshCollider>());
                                }

                                // Sphere Collider 3D
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<SphereCollider>() != null
                                 && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<SphereCollider>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<SphereCollider>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<SphereCollider>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<SphereCollider>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<SphereCollider>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<SphereCollider>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<SphereCollider>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<SphereCollider>());
                                }

                                // Terrain Collider 3D
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<TerrainCollider>() != null
                                 && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<TerrainCollider>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<TerrainCollider>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<TerrainCollider>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<TerrainCollider>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<TerrainCollider>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<TerrainCollider>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<TerrainCollider>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<TerrainCollider>());
                                }

                                // Wheel Collider 3D
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<WheelCollider>() != null
                                 && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<WheelCollider>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<WheelCollider>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<WheelCollider>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<WheelCollider>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<WheelCollider>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<WheelCollider>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<WheelCollider>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<WheelCollider>());
                                }
                                #endregion
                                break;
                            case Variables_MultiComponentCopier.Components.Material:
                                // Mesh Renderers
                                // Usually for static meshes
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<MeshRenderer>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<MeshRenderer>() != null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<MeshRenderer>().sharedMaterial =
                                    variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<MeshRenderer>().sharedMaterial;
                                }

                                // Skinned Mesh Renderers
                                // Usually for rigged meshes with skinning
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<SkinnedMeshRenderer>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<SkinnedMeshRenderer>() != null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<SkinnedMeshRenderer>().sharedMaterial =
                                    variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<SkinnedMeshRenderer>().sharedMaterial;
                                }
                                break;
                            case Variables_MultiComponentCopier.Components.MeshRenderer:
                                // Mesh Filter
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<MeshFilter>() != null
                                && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<MeshFilter>() != null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<MeshFilter>().sharedMesh =
                                        variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<MeshFilter>().mesh;
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<MeshFilter>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<MeshFilter>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<MeshFilter>();
                                    variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<MeshFilter>().sharedMesh =
                                        variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<MeshFilter>().mesh;
                                }

                                // Mesh Renderers
                                // Usually for static meshes
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<MeshRenderer>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<MeshRenderer>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<MeshRenderer>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<MeshRenderer>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<MeshRenderer>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<MeshRenderer>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<MeshRenderer>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<MeshRenderer>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<MeshRenderer>());
                                }

                                // Skinned Mesh Renderers
                                // Usually for rigged meshes with skinning
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<SkinnedMeshRenderer>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<SkinnedMeshRenderer>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<SkinnedMeshRenderer>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<SkinnedMeshRenderer>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[i].GetComponent<Transform>().GetComponent<SkinnedMeshRenderer>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[i].GetComponent<Transform>().GetComponent<SkinnedMeshRenderer>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<SkinnedMeshRenderer>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<SkinnedMeshRenderer>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<SkinnedMeshRenderer>());
                                }
                                break;
                            case Variables_MultiComponentCopier.Components.Rigidbody:
                                // Rigidbody 2D
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<Rigidbody2D>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<Rigidbody2D>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Rigidbody2D>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Rigidbody2D>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<Rigidbody2D>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<Rigidbody2D>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<Rigidbody2D>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Rigidbody2D>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Rigidbody2D>());
                                }

                                // Rigidbody 3D
                                // Update
                                if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<Rigidbody>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<Rigidbody>() != null)
                                {
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Rigidbody>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Rigidbody>());
                                }
                                // Add
                                else if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Transform>().GetComponent<Rigidbody>() != null
                                    && variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Transform>().GetComponent<Rigidbody>() == null)
                                {
                                    variables_MultiComponentCopier.destinationGameObjects[j].AddComponent<Rigidbody>();
                                    EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponent<Rigidbody>(), variables_MultiComponentCopier.destinationGameObjects[j].GetComponent<Rigidbody>());
                                }
                                break;
                            case Variables_MultiComponentCopier.Components.Script:
                                // Script Name
                                // Just a safety check to confirm if script is actually in the project
                                if (!string.IsNullOrEmpty(variables_MultiComponentCopier.componentName))
                                {
                                    for (int k = 0; k < variables_MultiComponentCopier.sourceGameObjects.Length; k++)
                                    {
                                        for (int comp = 0; comp < variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponents<Component>().Length; comp++)
                                        {
                                            if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponents<Component>()[comp].GetType().ToString() == variables_MultiComponentCopier.componentName)
                                            {
                                                Component scriptComponent = variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponents<Component>()[comp];
                                                variables_MultiComponentCopier.destinationGameObjects[j].AddComponent(scriptComponent.GetType());
                                            }
                                        }
                                    }
                                }
                                break;
                            case Variables_MultiComponentCopier.Components.Transform:
                                EditorUtility.CopySerialized(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].transform, variables_MultiComponentCopier.destinationGameObjects[j].transform);
                                break;
                            case Variables_MultiComponentCopier.Components.Other:
                                // Other Component
                                if (!string.IsNullOrEmpty(variables_MultiComponentCopier.componentName))
                                {
                                    for (int k = 0; k < variables_MultiComponentCopier.sourceGameObjects.Length; k++)
                                    {
                                        for (int comp = 0; comp < variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponents<Component>().Length; comp++)
                                        {
                                            if (variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponents<Component>()[comp].GetType().ToString() == variables_MultiComponentCopier.componentName)
                                            {
                                                Component scriptComponent = variables_MultiComponentCopier.sourceGameObjects[indexOfSequence].GetComponents<Component>()[comp];
                                                variables_MultiComponentCopier.destinationGameObjects[j].AddComponent(scriptComponent.GetType());
                                            }
                                        }
                                    }
                                }
                                break;
                        }

                        // Recursively update children too
                        if (variables_MultiComponentCopier.recursive)
                        {
                            CopyPasteToChildren(variables_MultiComponentCopier.sourceGameObjects[indexOfSequence], variables_MultiComponentCopier.destinationGameObjects[j], variables_MultiComponentCopier.component);
                        }

                        variables_MultiComponentCopier.progressIndex++;
                    }

                    previousRepetitionPosition += repetitionOfSequence; // Add position of last sequence into new and so on
                }
            }
            // Improper Sequence
            else
            {
                variables_MultiComponentCopier.progressBarText = "IMPROPER SEQUENCE. MISSING DATA. \n RECHECK AND FIX YOUR SEQUENCE.";
                ColorProgressBar(variables_MultiComponentCopier.errorColor);
            }
        }
        #endregion
    }
    #endregion

    #region Recursive Functions for Components of Children
    // Traverse through children and their children and theirs
    public void CopyPasteToChildren(GameObject sourceParent, GameObject destinationParent, Variables_MultiComponentCopier.Components component)
    {
        switch (component)
        {
            case Variables_MultiComponentCopier.Components.Animator:
                // Animator
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Animator>().Length; i++)
                {
                    // If child already exists
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].childCount == destinationParent.GetComponentsInChildren<Transform>()[i].childCount)
                    {
                        // Find a work around to get null case
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Animator>() != null)
                        {
                            // Update
                            if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Animator>() != null &&
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<Animator>() != null)
                            {
                                EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Animator>(),
                                    destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<Animator>());
                            }
                            // Add
                            else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Animator>() != null &&
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<Animator>() == null)
                            {
                                destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<Animator>();
                                EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Animator>(),
                                    destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<Animator>());
                            }
                        }
                    }
                    // Create a new one here and then copy on it if you want here
                    else
                    {
                        variables_MultiComponentCopier.progressError = true;
                        variables_MultiComponentCopier.progressBarText = "SOURCE AND DESTINATION CHILD COUNT MISMATCH \n IF YOU WANT TO CREATE A NEW CHILD AND COPY, UPDATE CODE";
                        ColorProgressBar(variables_MultiComponentCopier.errorColor);
                    }
                }
                break;
            case Variables_MultiComponentCopier.Components.Collider:
                #region Colliders
                // ~ 2D
                // Box Collider 2D
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Find a work around to get null case
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider2D>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider2D>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider2D>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider2D>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider2D>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider2D>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider2D>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<BoxCollider2D>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider2D>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider2D>());
                        }
                    }
                }

                // Capsule Collider 2D
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Find a work around to get null case
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider2D>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider2D>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider2D>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider2D>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider2D>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider2D>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider2D>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<CapsuleCollider2D>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider2D>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider2D>());
                        }
                    }
                }

                // Circle Collider 2D
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Find a work around to get null case
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CircleCollider2D>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CircleCollider2D>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CircleCollider2D>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CircleCollider2D>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CircleCollider2D>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CircleCollider2D>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CircleCollider2D>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<CircleCollider2D>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CircleCollider2D>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CircleCollider2D>());
                        }
                    }
                }

                // Composite Collider 2D
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Find a work around to get null case
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CompositeCollider2D>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CompositeCollider2D>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CompositeCollider2D>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CompositeCollider2D>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CompositeCollider2D>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CompositeCollider2D>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CompositeCollider2D>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<CompositeCollider2D>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CompositeCollider2D>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CompositeCollider2D>());
                        }
                    }
                }

                // Edge Collider 2D
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Find a work around to get null case
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<EdgeCollider2D>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<EdgeCollider2D>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<EdgeCollider2D>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<EdgeCollider2D>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<EdgeCollider2D>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<EdgeCollider2D>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<EdgeCollider2D>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<EdgeCollider2D>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<EdgeCollider2D>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<EdgeCollider2D>());
                        }
                    }
                }

                // Polygon Collider 2D
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Find a work around to get null case
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<PolygonCollider2D>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<PolygonCollider2D>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<PolygonCollider2D>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<PolygonCollider2D>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<PolygonCollider2D>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<PolygonCollider2D>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<PolygonCollider2D>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<PolygonCollider2D>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<PolygonCollider2D>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<PolygonCollider2D>());
                        }
                    }
                }

                // ~ 3D
                // Box Collider 3D
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Find a work around to get null case
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<BoxCollider>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<BoxCollider>());
                        }
                    }
                }

                // Capsule Collider 3D
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Find a work around to get null case
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<CapsuleCollider>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<CapsuleCollider>());
                        }
                    }
                }

                // Mesh Collider 3D
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Find a work around to get null case
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshCollider>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshCollider>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshCollider>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshCollider>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshCollider>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshCollider>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshCollider>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<MeshCollider>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshCollider>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshCollider>());
                        }
                    }
                }

                // Sphere Collider 3D
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Find a work around to get null case
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<SphereCollider>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<SphereCollider>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<SphereCollider>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<SphereCollider>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<SphereCollider>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<SphereCollider>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<SphereCollider>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<SphereCollider>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<SphereCollider>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<SphereCollider>());
                        }
                    }
                }

                // Terrain Collider 3D
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Find a work around to get null case
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<TerrainCollider>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<TerrainCollider>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<TerrainCollider>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<TerrainCollider>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<TerrainCollider>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<TerrainCollider>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<TerrainCollider>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<TerrainCollider>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<TerrainCollider>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<TerrainCollider>());
                        }
                    }
                }

                // Wheel Collider 3D
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Find a work around to get null case
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<WheelCollider>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<WheelCollider>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<WheelCollider>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<WheelCollider>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<WheelCollider>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<WheelCollider>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<WheelCollider>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<WheelCollider>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<WheelCollider>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<WheelCollider>());
                        }
                    }
                }
                #endregion
                break;
            case Variables_MultiComponentCopier.Components.Material:
                // Mesh Renderer
                for (int i = 0; i < sourceParent.GetComponentsInChildren<MeshRenderer>().Length; i++)
                {
                    // If child already exists
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].childCount == destinationParent.GetComponentsInChildren<Transform>()[i].childCount)
                    {
                        // Find a work around to get null case
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>() != null)
                        {
                            // Update
                            if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>() != null &&
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>() != null)
                            {
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>().sharedMaterial =
                                    sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>().sharedMaterial;
                            }
                        }
                    }
                    // Create a new one here and then copy on it if you want here
                    else
                    {
                        variables_MultiComponentCopier.progressError = true;
                        variables_MultiComponentCopier.progressBarText = "SOURCE AND DESTINATION CHILD COUNT MISMATCH \n IF YOU WANT TO CREATE A NEW CHILD AND COPY, UPDATE CODE";
                        ColorProgressBar(variables_MultiComponentCopier.errorColor);
                    }
                }
                // Skinned Mesh Renderer
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Find a work around for if you get a null case
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<SkinnedMeshRenderer>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<SkinnedMeshRenderer>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<SkinnedMeshRenderer>() != null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<SkinnedMeshRenderer>().sharedMaterial =
                                sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<SkinnedMeshRenderer>().sharedMaterial;
                        }
                    }
                }
                break;
            case Variables_MultiComponentCopier.Components.MeshRenderer:
                // Mesh Filter
                for (int i = 0; i < sourceParent.GetComponentsInChildren<MeshFilter>().Length; i++)
                {
                    // If child already exists
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].childCount == destinationParent.GetComponentsInChildren<Transform>()[i].childCount)
                    {
                        // Find a work around to get null case
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshFilter>() != null)
                        {
                            // Update
                            if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshFilter>() != null &&
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshFilter>() != null)
                            {
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshFilter>().mesh =
                                    sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshFilter>().sharedMesh;
                            }
                            // Add
                            else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshFilter>() != null &&
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshFilter>() == null)
                            {
                                destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<MeshFilter>();
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshFilter>().mesh =
                                    sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshFilter>().sharedMesh;
                            }
                        }
                    }
                    // Create a new one here and then copy on it if you want here
                    else
                    {
                        variables_MultiComponentCopier.progressError = true;
                        variables_MultiComponentCopier.progressBarText = "SOURCE AND DESTINATION CHILD COUNT MISMATCH \n IF YOU WANT TO CREATE A NEW CHILD AND COPY, UPDATE CODE";
                        ColorProgressBar(variables_MultiComponentCopier.errorColor);
                    }
                }
                // Mesh Renderer
                for (int i = 0; i < sourceParent.GetComponentsInChildren<MeshRenderer>().Length; i++)
                {
                    // If child already exists
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].childCount == destinationParent.GetComponentsInChildren<Transform>()[i].childCount)
                    {
                        // Find a work around to get null case
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>() != null)
                        {
                            // Update
                            if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>() != null &&
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>() != null)
                            {
                                EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>(), destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>());
                            }
                            // Add
                            else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>() != null &&
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>() == null)
                            {
                                destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<MeshRenderer>();
                                EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>(), destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<MeshRenderer>());
                            }
                        }
                    }
                    // Create a new one here and then copy on it if you want here
                    else
                    {
                        variables_MultiComponentCopier.progressError = true;
                        variables_MultiComponentCopier.progressBarText = "SOURCE AND DESTINATION CHILD COUNT MISMATCH \n IF YOU WANT TO CREATE A NEW CHILD AND COPY, UPDATE CODE";
                        ColorProgressBar(variables_MultiComponentCopier.errorColor);
                    }
                }
                // Skinned Mesh Renderer
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Find a work around for if you get a null case
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<SkinnedMeshRenderer>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<SkinnedMeshRenderer>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<SkinnedMeshRenderer>() != null)
                        {
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<SkinnedMeshRenderer>(), destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<SkinnedMeshRenderer>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<SkinnedMeshRenderer>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<SkinnedMeshRenderer>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<SkinnedMeshRenderer>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<SkinnedMeshRenderer>(), destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<SkinnedMeshRenderer>());
                        }
                    }
                }
                break;
            case Variables_MultiComponentCopier.Components.Rigidbody:
                for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                {
                    // Rigidbody 2D
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody2D>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody2D>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody2D>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody2D>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody2D>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody2D>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody2D>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<Rigidbody2D>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody2D>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody2D>());
                        }
                    }
                    // Rigidbody 3D
                    if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody>() != null)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody>());
                        }
                        // Add
                        else if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody>() != null &&
                            destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody>() == null)
                        {
                            destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent<Rigidbody>();
                            EditorUtility.CopySerialized(sourceParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody>(),
                                destinationParent.GetComponentsInChildren<Transform>()[i].GetComponent<Rigidbody>());
                        }
                    }
                }
                break;
            case Variables_MultiComponentCopier.Components.Script:
                // Script Name
                // Just a safety check to confirm if script is actually in the project
                if (!string.IsNullOrEmpty(variables_MultiComponentCopier.componentName))
                {
                    for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                    {
                        for (int j = 0; j < sourceParent.GetComponentsInChildren<Transform>()[i].GetComponents<Component>().Length; j++)
                        {
                            if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponents<Component>()[j].GetType().ToString().ToLower() ==
                                variables_MultiComponentCopier.componentName.ToLower())
                            {
                                Component scriptComponent = sourceParent.GetComponentsInChildren<Transform>()[i].GetComponents<Component>()[j];
                                destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent(scriptComponent.GetType());
                            }
                        }
                    }
                }
                break;
            case Variables_MultiComponentCopier.Components.Transform:
                if (sourceParent.GetComponentsInChildren<Transform>().Length > 0)
                {
                    for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                    {
                        // Update
                        if (sourceParent.GetComponentsInChildren<Transform>()[i] != null &&
                            destinationParent.GetComponentsInChildren<Transform>() != null)
                        {
                            EditorUtility.CopySerializedIfDifferent(sourceParent.GetComponentsInChildren<Transform>()[i],
                                destinationParent.GetComponentsInChildren<Transform>()[i]);
                        }
                    }
                }
                break;
            case Variables_MultiComponentCopier.Components.Other:
                // Component Name
                // Just a safety check to confirm if script is actually in the project
                if (!string.IsNullOrEmpty(variables_MultiComponentCopier.componentName))
                {
                    for (int i = 0; i < sourceParent.GetComponentsInChildren<Transform>().Length; i++)
                    {
                        for (int j = 0; j < sourceParent.GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Component>().Length; j++)
                        {
                            if (sourceParent.GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Component>()[j].GetType().ToString().ToLower() ==
                                variables_MultiComponentCopier.componentName.ToLower())
                            {
                                Component scriptComponent = sourceParent.GetComponentsInChildren<Transform>()[i].GetComponentsInChildren<Component>()[j];
                                destinationParent.GetComponentsInChildren<Transform>()[i].gameObject.AddComponent(scriptComponent.GetType());
                            }
                        }
                    }
                }
                break;
        }
    }
    #endregion

    #region Progress Bar Functions
    // Resets progress bar stats to base
    public void ResetProgressBar()
    {
        variables_MultiComponentCopier.progressError = false; // Reset error texts
        variables_MultiComponentCopier.progressIndex = 0; // Reset progress bar's current progress index to base
        EditorGUI.ProgressBar(variables_MultiComponentCopier.progressBarRect, 0f / 1f, "DRAG & DROP AND PRESS COPY"); // Reset Text
    }

    // Updates the color of the progress bar during Idle, Loading & Error for glamour
    public void ColorProgressBar(Color color)
    {
        if (color == variables_MultiComponentCopier.errorColor) variables_MultiComponentCopier.progressError = true; // Everytime we get an error
        variables_MultiComponentCopier.progressBarColor = color;
    }

    // Show Completion Text
    public void UpdateProgressBarCompletedText()
    {
        if (variables_MultiComponentCopier.component.ToString().EndsWith("y"))
        {
            if (variables_MultiComponentCopier.recursive)
                variables_MultiComponentCopier.progressBarText = "PROGRESS: " + variables_MultiComponentCopier.progressIndex + "/" + variables_MultiComponentCopier.destinationGameObjects.Length + " \n " + variables_MultiComponentCopier.component.ToString().TrimEnd('y').ToUpper() + "IES" + " HAVE BEEN RECURSIVELY COPIED SUCCESSFULLY";
            else
                variables_MultiComponentCopier.progressBarText = "PROGRESS: " + variables_MultiComponentCopier.progressIndex + "/" + variables_MultiComponentCopier.destinationGameObjects.Length + " \n " + variables_MultiComponentCopier.component.ToString().TrimEnd('y').ToUpper() + "IES" + " HAVE BEEN COPIED SUCCESSFULLY";
        }
        else
        {
            if (variables_MultiComponentCopier.recursive)
                variables_MultiComponentCopier.progressBarText = "PROGRESS: " + variables_MultiComponentCopier.progressIndex + "/" + variables_MultiComponentCopier.destinationGameObjects.Length + " \n " + variables_MultiComponentCopier.component.ToString().ToUpper() + "S" + " HAVE BEEN RECURSIVELY COPIED SUCCESSFULLY";
            else
                variables_MultiComponentCopier.progressBarText = "PROGRESS: " + variables_MultiComponentCopier.progressIndex + "/" + variables_MultiComponentCopier.destinationGameObjects.Length + " \n " + variables_MultiComponentCopier.component.ToString().ToUpper() + "S" + " HAVE BEEN COPIED SUCCESSFULLY";
        }
    }
    #endregion
}

#region Help Window Class
public class Instructions : EditorWindow
{
    public void OnGUI()
    {
        GetWindow<Instructions>().position = new Rect(60, 60, 500, 380);
        GUILayout.Label("Multi-Component Copier Instructions", EditorStyles.boldLabel); GUILayout.Space(5f);
        GUILayout.Label("1. Drag and Drop 'Source Game Objects' to copy component from.");
        GUILayout.Label("2. Drag and Drop 'Destination Game Objects' to copy component to.");
        GUILayout.Label("3. Select a component among the list to copy.");
        GUILayout.Label("4. Select 'Other' and type the name if it's not in the drop-down menu.");
        GUILayout.Label("    Spellings are not important e.g. box collider 2d would mean 'Box Collider 2D'");
        GUILayout.Label("5. Enable 'Recursive' only if you want to copy components from the children.");
        GUILayout.Label("    of the 'Source' Objects down to the children of the 'Destination' Objects.");
        GUILayout.Label("6. Enter a sequence if you want to copy your objects in a specified way,");
        GUILayout.Label("    For example, '0-5,1-10,2-5' would mean '0' index would be copied 5 times,");
        GUILayout.Label("    '1' index would be copied 10 times, and '2' index would be copied 5 times.");
        GUILayout.Label("    This way you can update 20 Destination Objects with only 3 Source Objects.");
        GUILayout.Label("    Leave it empty if you want to copy everything sequentially 1:1.");
        GUILayout.Label("    Note: Sequence will not affect the children with 'Recursive' on.");
        GUILayout.Label("7. Press 'Copy'.");
        GUILayout.Label("8. Done.");
        GUILayout.Space(10f);

        GUIStyle gStyle = new GUIStyle();
        gStyle.fontSize = 8;
        gStyle.alignment = TextAnchor.MiddleCenter;
        gStyle.normal.textColor = Color.white;

        GUILayout.Label("pizza khila de koi mehnat ki hai :(", gStyle);

        if (GUILayout.Button("CLOSE", GUILayout.Height(50))) Close();
    }
}
#endregion

#region Global Variables
public class Variables_MultiComponentCopier : EditorWindow
{
    // Main
    public GameObject[] sourceGameObjects;
    public GameObject[] destinationGameObjects;
    public GameObject objectToClean;
    public Components component;
    public Property property;
    public bool recursive;

    // Dump
    public string componentName;
    public bool recursiveToggled;
    public bool progressError;
    public bool missingObjects;
    public int progressIndex;
    public int stupidIndex;
    public string sequence;
    public string progressBarText;
    public Components previousProperty;
    public Rect progressBarRect;
    public Vector2 scrollPos;
    public Color errorColor = new Color(1f, 0.15f, 0.15f);
    public Color loadingColor = new Color(0.15f, 1f, 0.35f);
    public Color idleColor = Color.white;
    public Color progressBarColor = Color.white;

    // List of Components you can copy, feel free to add more here
    // but be cautious about updating in the rest of the script ⚠️
    public enum Components { Material, Rigidbody, Collider, Script, Transform, MeshRenderer, Animator, Other }

    public enum Property { }
}
#endregion

#endregion

#region Multi-Component Cleaner

public class MultiComponentCleanerWindow : EditorWindow
{
    // Add menu item named "MultiPropertyCopier" to the 'Knights' Menu Section
    [MenuItem("Utility/Knights/Multi-Component Cleaner")]
    public static void ShowWindow() => GetWindow(typeof(MultiComponentCleaner));
}

public class MultiComponentCleaner : EditorWindow
{
    Variables_MultiComponentCleaner variables_MultiComponentCopier;

    public void OnGUI()
    {
        // Just instantiate once
        if (variables_MultiComponentCopier == null) { variables_MultiComponentCopier = new Variables_MultiComponentCleaner(); }

        // ~ Deep Cleaner
        // Title
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.Space();
        GUILayout.Label("Multi-Component Cleaner", EditorStyles.boldLabel);
        EditorGUILayout.EndHorizontal();

        // Tabs Fwd
        EditorGUI.indentLevel++;

        // Buttons
        EditorGUILayout.BeginHorizontal();
        // Instructions button
        // Separated with space
        EditorGUILayout.Space();
        // Exit button
        if (GUILayout.Button("X", GUILayout.Width(16), GUILayout.Height(16))) Close();
        EditorGUILayout.EndHorizontal();

        // To separate the buttons from the progress bar
        EditorGUILayout.Space();

        // Progress Bar Rect
        variables_MultiComponentCopier.progressBarRect = EditorGUILayout.BeginVertical();

        // Just a dummy line, doesn't do anything special but keeps the structure together to display progress bar
        GUILayout.Label("Dummy Bar");
        GUILayout.Label("Disable this line if you want to make the progress bar thinner");

        // Progress Bar Color
        GUI.color = variables_MultiComponentCopier.progressBarColor;

        EditorGUI.ProgressBar(variables_MultiComponentCopier.progressBarRect, variables_MultiComponentCopier.progressIndex / 100, variables_MultiComponentCopier.progressBarText);

        // Progress Bar
        if (variables_MultiComponentCopier.progressIndex != 0 && variables_MultiComponentCopier.objectsToClean.Length != 0 && !variables_MultiComponentCopier.progressError || variables_MultiComponentCopier.objectClean)
        {
            if (variables_MultiComponentCopier.objectClean)
            {
                variables_MultiComponentCopier.progressBarText = "OBJECT IS ALREADY CLEAN \n ZERO COMPONENTS FOUND";
                EditorGUI.ProgressBar(variables_MultiComponentCopier.progressBarRect,
                1, variables_MultiComponentCopier.progressBarText);
            }
            else
            {
                UpdateProgressBarCompletedText();
                EditorGUI.ProgressBar(variables_MultiComponentCopier.progressBarRect,
                    variables_MultiComponentCopier.progressIndex / variables_MultiComponentCopier.progressIndex, variables_MultiComponentCopier.progressBarText);
            }
        }
        else if (variables_MultiComponentCopier.progressError) EditorGUI.ProgressBar(variables_MultiComponentCopier.progressBarRect, 0f / 1f, variables_MultiComponentCopier.progressBarText);
        else { EditorGUI.ProgressBar(variables_MultiComponentCopier.progressBarRect, 0f / 1f, "DRAG & DROP AND PRESS CLEAN \n TOGGLE DEEP CLEAN TO REMOVE CHILDREN COMPONENTS"); ColorProgressBar(variables_MultiComponentCopier.idleColor); }

        // Reset Color
        GUI.color = variables_MultiComponentCopier.idleColor;

        EditorGUILayout.EndVertical();

        // Scrollable
        EditorGUILayout.BeginVertical();
        variables_MultiComponentCopier.scrollPos = EditorGUILayout.BeginScrollView(variables_MultiComponentCopier.scrollPos, false, false);

        // Space
        EditorGUILayout.Space();

        // Serialized Variables
        SerializedObject serializedObject = new SerializedObject(variables_MultiComponentCopier);

        // ~ Deep Clean

        // Space
        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("objectsToClean"), true);
        serializedObject.ApplyModifiedProperties();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Deep Clean");
        variables_MultiComponentCopier.deepClean = EditorGUILayout.Toggle(variables_MultiComponentCopier.deepClean);
        EditorGUILayout.EndHorizontal();

        // Space x 2
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (GUILayout.Button("CLEAN", GUILayout.Height(40))) { variables_MultiComponentCopier.objectClean = false; ColorProgressBar(variables_MultiComponentCopier.loadingColor); ResetProgressBar(); if (variables_MultiComponentCopier.deepClean) { DeepClean(); } else { Clean(); } }

        EditorGUILayout.EndVertical();
        EditorGUILayout.EndScrollView();

        // Keep track of deep clean toggle state
        if (variables_MultiComponentCopier.deepClean != variables_MultiComponentCopier.deepCleanToggled) { variables_MultiComponentCopier.objectClean = false; ResetProgressBar(); ColorProgressBar(variables_MultiComponentCopier.idleColor); variables_MultiComponentCopier.deepCleanToggled = variables_MultiComponentCopier.deepClean; }

        // Hotkeys, uncomment if you want to include keyboard shortcuts (I didn't)
        //Hotkeys();
    }

    #region Progress Bar Functions
    // Resets progress bar stats to base
    public void ResetProgressBar()
    {
        variables_MultiComponentCopier.progressError = false; // Reset error texts
        variables_MultiComponentCopier.progressIndex = 0; // Reset progress bar's current progress index to base
        EditorGUI.ProgressBar(variables_MultiComponentCopier.progressBarRect, 0f / 1f, "DRAG & DROP AND PRESS COPY"); // Reset Text
    }

    // Updates the color of the progress bar during Idle, Loading & Error for glamour
    public void ColorProgressBar(Color color)
    {
        if (color == variables_MultiComponentCopier.errorColor) variables_MultiComponentCopier.progressError = true; // Everytime we get an error
        variables_MultiComponentCopier.progressBarColor = color;
    }

    // Show Completion Text
    public void UpdateProgressBarCompletedText()
    {
        if (variables_MultiComponentCopier.deepClean)
            variables_MultiComponentCopier.progressBarText = "PROGRESS: " + variables_MultiComponentCopier.progressIndex + "/" + variables_MultiComponentCopier.progressIndex + " \n " + " COMPONENTS HAVE BEEN DEEP CLEANED";
        else
            variables_MultiComponentCopier.progressBarText = "PROGRESS: " + variables_MultiComponentCopier.progressIndex + "/" + variables_MultiComponentCopier.progressIndex + " \n " + " COMPONENTS HAVE BEEN CLEANED";

    }
    #endregion

    #region Hotkeys Function
    //void HotKeys()
    //{
    //    Check for Events here if you want these commands on Hotkeys
    //}
    #endregion

    #region Main Functions
    void DeepClean()
    {
        try
        {
            foreach (GameObject objectToClean in variables_MultiComponentCopier.objectsToClean)
            {
                foreach (Transform child in objectToClean.GetComponentsInChildren<Transform>())
                {
                    foreach (Transform childOfChild in child.GetComponentsInChildren<Transform>())
                    {
                        foreach (Component comp in childOfChild.GetComponents<Component>())
                        {
                            if (comp.GetType().ToString() != "UnityEngine.Transform")
                            {
                                DestroyImmediate(comp);
                                variables_MultiComponentCopier.progressIndex++;
                            }
                        }
                    }
                }
            }

            if (variables_MultiComponentCopier.progressIndex == 0) { variables_MultiComponentCopier.objectClean = true; }
        }
        catch (UnityException) { }
    }

    void Clean()
    {
        try
        {
            foreach (GameObject objectToClean in variables_MultiComponentCopier.objectsToClean)
            {
                foreach (Component comp in objectToClean.GetComponents<Component>())
                {
                    if (comp.GetType().ToString() != "UnityEngine.Transform")
                    {
                        DestroyImmediate(comp);
                        variables_MultiComponentCopier.progressIndex++;
                    }
                }
            }

            if (variables_MultiComponentCopier.progressIndex == 0) { variables_MultiComponentCopier.objectClean = true; }
        }
        catch (UnityException) { }
    }
    #endregion
}

#region Global Variables
public class Variables_MultiComponentCleaner : EditorWindow
{
    // Main
    public GameObject[] objectsToClean;
    public bool deepClean;

    // Dump
    public bool deepCleanToggled;
    public bool objectClean;
    public bool progressError;
    public int progressIndex;
    public string progressBarText;
    public Rect progressBarRect;
    public Vector2 scrollPos;
    public Color errorColor = new Color(1f, 0.15f, 0.15f);
    public Color loadingColor = new Color(0.15f, 1f, 0.35f);
    public Color idleColor = Color.white;
    public Color progressBarColor = Color.white;
}
#endregion
#endregion
#endif

