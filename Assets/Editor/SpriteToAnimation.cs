using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections.Generic;
using System.IO;

public class SpriteToAnimationWindow : EditorWindow {
    private string sourcePathRoot = "Assets/Sprites/Characters/";
    private string savePathRoot = "Assets/Animations/Characters/";
    private string sourceFolder = "";
    private string saveFolder = "";

    private string[] subfolders = { "idle", "walk", "slash", "islash", "thrust", "smash", "shoot", "hurt", "cast" };
    static private int ppu = 64;

    private ClipSettingsDatabase settingsDatabase;

    [MenuItem("Tools/Sprites to Animation")]
    static void Init() {
        SpriteToAnimationWindow window = (SpriteToAnimationWindow)GetWindow(typeof(SpriteToAnimationWindow));
        window.titleContent = new GUIContent("Animation Generator");
        window.Show();
    }

    private void OnEnable() {
        string assetPath = "Assets/Data/ClipSettingsDatabase.asset";
        settingsDatabase = AssetDatabase.LoadAssetAtPath<ClipSettingsDatabase>(assetPath);

        if (settingsDatabase == null) {
            Debug.LogError("ClipSettingsDatabase not found at path: " + assetPath);
        }
    }

    public static void ShowWindow() {
        GetWindow<SpriteToAnimationWindow>("Sprites to Animation");
    }

    [System.Obsolete]
    private void OnGUI() {
        GUILayout.Label("Sprite Animation Clip Generator", EditorStyles.boldLabel);

        sourceFolder = EditorGUILayout.TextField("Sprite Source", sourceFolder);
        saveFolder = EditorGUILayout.TextField("Save Folder", saveFolder);
        ppu = EditorGUILayout.IntField("PPU", ppu);

        GUILayout.Space(20);
        GUILayout.Label("Clip Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("", GUILayout.Width(60));
        GUILayout.Label("Columns", GUILayout.Width(55));
        GUILayout.Label("Rows", GUILayout.Width(40));
        GUILayout.Label("Height", GUILayout.Width(40));
        GUILayout.Label("Width", GUILayout.Width(40));
        GUILayout.Label("FPS", GUILayout.Width(30));
        GUILayout.Label("Loop", GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();


        foreach (var key in subfolders) {
            var settings = settingsDatabase.Get(key);

            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(key, GUILayout.Width(60));

            EditorGUI.BeginChangeCheck();
            settings.columns = EditorGUILayout.IntField(settings.columns, GUILayout.Width(55));
            settings.rows = EditorGUILayout.IntField(settings.rows, GUILayout.Width(40));
            settings.height = EditorGUILayout.IntField(settings.height, GUILayout.Width(40));
            settings.width = EditorGUILayout.IntField(settings.width, GUILayout.Width(40));
            settings.frameRate = EditorGUILayout.IntField(settings.frameRate, GUILayout.Width(30));
            settings.canLoop = EditorGUILayout.Toggle(settings.canLoop, GUILayout.Width(40));
            if (EditorGUI.EndChangeCheck()) settingsDatabase.Set(key, settings);

            EditorGUILayout.EndHorizontal();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Create Animations")) {
            CreateAllClips();
            // Debug.Log(settingsDatabase.Get("shoot").columns);
        }
    }

    [System.Obsolete]
    private void CreateAllClips() {
        string fullFolderPath = Path.Combine(savePathRoot, saveFolder).Replace("\\", "/");
        string[] folders = saveFolder.Split('/');

        string currentPath = savePathRoot;
        foreach (string folder in folders) {
            string newPath = Path.Combine(currentPath, folder).Replace("\\", "/");
            if (!AssetDatabase.IsValidFolder(newPath)) {
                AssetDatabase.CreateFolder(currentPath, folder);
            }
            currentPath = newPath;
        }



        // Create Animator Controller
        string controllerPath = Path.Combine(fullFolderPath, $"controller.controller");
        var controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
        if (controller.layers.Length == 0) controller.AddLayer("Base Layer");
        var stateMachine = controller.layers[0].stateMachine;

        foreach (string s in subfolders) {
            ClipSettings settings = settingsDatabase.Get(s);
            if (s != "hurt") {
                string spriteSheetPath = $"{sourcePathRoot}/{sourceFolder}/{s}.png";
                SliceSpriteSheet(spriteSheetPath, s, settings);

                List<Sprite> sprites = new List<Sprite>();
                Object[] assets = AssetDatabase.LoadAllAssetsAtPath(spriteSheetPath);
                foreach (Object obj in assets) {
                    if (obj is Sprite sprite) sprites.Add(sprite);
                }

                if (sprites.Count == 0) continue;
                if (sprites.Count != settings.columns * settings.rows) {
                    Debug.Log($"Could not generate animation, {s} has settings that don't match the sprite");
                    return;
                }

                AnimationClip clip_up = CreateAnimationClip($"{s}_up", sprites.GetRange(0 * settings.columns, settings.columns), settings);
                AnimationClip clip_left = CreateAnimationClip($"{s}_left", sprites.GetRange(1 * settings.columns, settings.columns), settings);
                AnimationClip clip_down = CreateAnimationClip($"{s}_down", sprites.GetRange(2 * settings.columns, settings.columns), settings);
                AnimationClip clip_right = CreateAnimationClip($"{s}_right", sprites.GetRange(3 * settings.columns, settings.columns), settings);

                AnimatorState state_up = stateMachine.AddState($"{s}_up");
                state_up.motion = clip_up;
                AnimatorState state_left = stateMachine.AddState($"{s}_left");
                state_left.motion = clip_left;
                AnimatorState state_down = stateMachine.AddState($"{s}_down");
                state_down.motion = clip_down;
                AnimatorState state_right = stateMachine.AddState($"{s}_right");
                state_right.motion = clip_right;

                if (s == "idle") stateMachine.defaultState = state_right;
            }
            else {
                string spriteSheetPath = $"{sourcePathRoot}/{sourceFolder}/{s}.png";
                SliceSpriteSheet(spriteSheetPath, s, settings);

                List<Sprite> sprites = new List<Sprite>();
                Object[] assets = AssetDatabase.LoadAllAssetsAtPath(spriteSheetPath);
                foreach (Object obj in assets) {
                    if (obj is Sprite sprite) sprites.Add(sprite);
                }

                if (sprites.Count == 0) continue;

                AnimationClip clip = CreateAnimationClip(s, sprites, settings);

                AnimatorState state = stateMachine.AddState(s);
                state.motion = clip;
            }
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("Assets created at " + fullFolderPath);
    }

    private AnimationClip CreateAnimationClip(string clipName, List<Sprite> sprites, ClipSettings clipSettings) {
        int frameRate = clipSettings.frameRate;
        bool canLoop = clipSettings.canLoop;

        sprites.Sort((a, b) => a.name.CompareTo(b.name));

        AnimationClip clip = new AnimationClip {
            frameRate = frameRate,
            wrapMode = canLoop ? WrapMode.Loop : WrapMode.Once,

        };

        EditorCurveBinding binding = new EditorCurveBinding {
            type = typeof(SpriteRenderer),
            path = "",
            propertyName = "m_Sprite"
        };

        ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[sprites.Count];
        for (int i = 0; i < sprites.Count; i++) {
            keyframes[i] = new ObjectReferenceKeyframe {
                time = (float)i / frameRate,
                value = sprites[i]
            };
        }

        AnimationUtility.SetObjectReferenceCurve(clip, binding, keyframes);

        var settings = AnimationUtility.GetAnimationClipSettings(clip);
        settings.loopTime = canLoop;
        AnimationUtility.SetAnimationClipSettings(clip, settings);

        string baseName = char.ToLower(saveFolder[0]) + saveFolder.Substring(1);
        string finalPath = Path.Combine(savePathRoot, $"{saveFolder}/{clipName}.anim");

        AssetDatabase.CreateAsset(clip, finalPath);

        return clip;
    }

    [System.Obsolete]
    public static void SliceSpriteSheet(string path, string name, ClipSettings settings) {
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer == null) return;

        Debug.Log("Slicing started");

        importer.spriteImportMode = SpriteImportMode.Single;
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

        importer.textureType = TextureImporterType.Sprite;
        importer.spriteImportMode = SpriteImportMode.Multiple;
        importer.filterMode = FilterMode.Point;
        importer.spritePixelsPerUnit = ppu;

        int columns = settings.columns, rows = settings.rows;
        int width = settings.width, height = settings.height;

        importer.spritesheet = new SpriteMetaData[0];
        SpriteMetaData[] metas = new SpriteMetaData[columns * rows];
        for (int y = 0; y < rows; y++) {
            for (int x = 0; x < columns; x++) {
                int index = y * columns + x;
                SpriteMetaData meta = new SpriteMetaData {
                    name = $"{name}_{index:D3}",
                    rect = new Rect(x * width, (rows - y - 1) * height, width, height),
                    alignment = (int)SpriteAlignment.Center
                };
                metas[index] = meta;
            }
        }
        importer.spritesheet = metas;

        EditorUtility.SetDirty(importer);
        AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceSynchronousImport | ImportAssetOptions.ForceUpdate);
    }
}
