using System;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

/*
Unused

This was used before we started holdingall the information into a txt file
*/
[CreateAssetMenu(menuName = "Formatted Document")]
public class FormattedDocument : ScriptableObject
{
    public AudioClip audioClip;
    public List<ContentBlock> contentBlocks = new List<ContentBlock>();

    [Serializable]
    public class ContentBlock
    {
        public Sprite image;
        public AudioClip audioClip;
        public string text;
        public int fontSize;
        public FontStyles fontStyle;
        public TextAlignmentOptions alignment;

        public ContentBlock()
        {
            image = null;
            audioClip = null;
            text = "Sample text";
            fontSize = 10;
            fontStyle = FontStyles.Normal;
            alignment = TextAlignmentOptions.TopLeft;
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(FormattedDocument))]
    public class FormattedDocumentEditor : Editor
    {
        public void Awake()
        {
            EditorUtility.SetDirty(target);
        }

        public override void OnInspectorGUI()
        {
            FormattedDocument fd = (FormattedDocument) target;

            for(int i = 0; i < fd.contentBlocks.Count; i++)
            {
                //EditorGUILayout.LabelField("Content Block " + i, EditorGUILayout.);
                fd.contentBlocks[i].image = (Sprite) EditorGUILayout.ObjectField("Image", fd.contentBlocks[i].image, typeof(Sprite), false);

                //fd.contentBlocks[i].textScrollPosition = GUILayout.BeginScrollView(fd.contentBlocks[i].textScrollPosition);
                EditorStyles.textField.wordWrap = true;
                fd.contentBlocks[i].text = EditorGUILayout.TextArea(fd.contentBlocks[i].text, GUILayout.ExpandHeight(true));
                //GUILayout.EndScrollView();
                
                GUILayout.BeginHorizontal();
                fd.contentBlocks[i].fontSize = EditorGUILayout.IntField(fd.contentBlocks[i].fontSize);
                fd.contentBlocks[i].fontStyle = (FontStyles) EditorGUILayout.EnumPopup(fd.contentBlocks[i].fontStyle);
                fd.contentBlocks[i].alignment = (TextAlignmentOptions) EditorGUILayout.EnumPopup(fd.contentBlocks[i].alignment);
                GUILayout.EndHorizontal();
            }

            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            GUILayout.BeginHorizontal();
            if(GUILayout.Button("Add Content Block")) fd.contentBlocks.Add(new ContentBlock());
            else if(GUILayout.Button("Remove Content Block") && fd.contentBlocks.Count > 0) fd.contentBlocks.RemoveAt(fd.contentBlocks.Count - 1);
            GUILayout.EndHorizontal();
        }
    }
    #endif
}
