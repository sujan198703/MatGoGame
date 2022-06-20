#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class AnchorSetter : EditorWindow
{

    [MenuItem("Helper/Anchor Setter/Set Anchors to Corners for Selected GameObject #Z")]
    static void AnchorsToCorners_per_object()
    {
        if (Selection.activeGameObject.GetComponent<RectTransform>() == null)
            return;
        RectTransform t = Selection.activeTransform as RectTransform;
        RectTransform pt = Selection.activeTransform.parent as RectTransform;

        if (t == null || pt == null)
            return;

        Vector2 newAnchorsMin = new Vector2(t.anchorMin.x + t.offsetMin.x / pt.rect.width,
                                    t.anchorMin.y + t.offsetMin.y / pt.rect.height);
        Vector2 newAnchorsMax = new Vector2(t.anchorMax.x + t.offsetMax.x / pt.rect.width,
                                    t.anchorMax.y + t.offsetMax.y / pt.rect.height);

        t.anchorMin = newAnchorsMin;
        t.anchorMax = newAnchorsMax;
        t.offsetMin = t.offsetMax = new Vector2(0, 0);
    }

    [MenuItem("Helper/Anchor Setter/Set Anchors to Corners for Selected GameObjects #X")]
    static void AnchorsToCorners_per_multiple_selected_Objects()
    {
        if (Selection.activeGameObject.GetComponent<RectTransform>() == null)
            return;
        GameObject[] temp = Selection.gameObjects;
        RectTransform[] t = new RectTransform[temp.Length];
        for (int i = 0; i < temp.Length; i++)
        {
            t[i] = temp[i].GetComponent<RectTransform>();
        }

        if (t == null)
            return;

        for (int i = 0; i < t.Length; i++)
        {
            Vector2 newAnchorsMin = new Vector2(t[i].anchorMin.x + t[i].offsetMin.x / t[i].parent.GetComponent<RectTransform>().rect.width,
                                        t[i].anchorMin.y + t[i].offsetMin.y / t[i].parent.GetComponent<RectTransform>().rect.height);
            Vector2 newAnchorsMax = new Vector2(t[i].anchorMax.x + t[i].offsetMax.x / t[i].parent.GetComponent<RectTransform>().rect.width,
                                        t[i].anchorMax.y + t[i].offsetMax.y / t[i].parent.GetComponent<RectTransform>().rect.height);

            t[i].anchorMin = newAnchorsMin;
            t[i].anchorMax = newAnchorsMax;
            t[i].offsetMin = t[i].offsetMax = new Vector2(0, 0);
        }
    }
    //		}
    [MenuItem("Helper/Anchor Setter/Set Anchors to Corners for Whole Canvas % & C")]
    static void AnchorsToCorners_for_all_child_in_canvas()
    {
        if (Selection.activeGameObject.GetComponent<Canvas>())
        {
            if (EditorUtility.DisplayDialog("Set Anchors to Corners", "Set Anchors to Corners of every Child in Canvas. \n\nAre you sure you want to continue?", "OK", "Cancel"))
            {
                GameObject canvas = Selection.activeGameObject as GameObject;
                //		foreach(RectTransform t in canvas){
                //		RectTransform[] t = Selection.activeTransform as RectTransform;
                //		RectTransform[] pt = Selection.activeTransform.parent as RectTransform;

                RectTransform[] t = canvas.GetComponentsInChildren<RectTransform>(true);

                if (t == null)
                    return;

                for (int i = 1; i < t.Length; i++)
                {
                    Vector2 newAnchorsMin = new Vector2(t[i].anchorMin.x + t[i].offsetMin.x / t[i].parent.GetComponent<RectTransform>().rect.width,
                                                t[i].anchorMin.y + t[i].offsetMin.y / t[i].parent.GetComponent<RectTransform>().rect.height);
                    Vector2 newAnchorsMax = new Vector2(t[i].anchorMax.x + t[i].offsetMax.x / t[i].parent.GetComponent<RectTransform>().rect.width,
                                                t[i].anchorMax.y + t[i].offsetMax.y / t[i].parent.GetComponent<RectTransform>().rect.height);

                    t[i].anchorMin = newAnchorsMin;
                    t[i].anchorMax = newAnchorsMax;
                    t[i].offsetMin = t[i].offsetMax = new Vector2(0, 0);
                }
            }
        }
        else
        {
            EditorUtility.DisplayDialog("Canvas Not Selected", "only works if your current selected GameObject is Canvas!", "OK");
        }
    }
}
#endif