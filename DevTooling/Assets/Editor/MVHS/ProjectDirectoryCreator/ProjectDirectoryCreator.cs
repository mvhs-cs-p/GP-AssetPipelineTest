using System.IO;
using UnityEditor;
using UnityEngine;

namespace MVHS
{
    public static class ProjectDirectoryCreator
    {
        [MenuItem("Tools/MVHS/Project Directory Creator")]
        public static void CreateProjectDirectory()
        {
            const string projectDirectoryRoot = "Assets/Project";
            if (AssetDatabase.IsValidFolder(projectDirectoryRoot))
            {
                Debug.LogWarning("[ProjectDirectoryCreator] Unable to create project directory. Project directory already exists.");
                EditorUtility.DisplayDialog(
                    title: "Project Directory Creator",
                    message: "Unable to create project directory. Project directory already exists.",
                    ok: "OK"
                    );
                return;
            }

            string[] projectFolders =
            {
                "Assets/Project",

                "Assets/Project/Art",
                "Assets/Project/Art/Animations",
                "Assets/Project/Art/Animators",
                "Assets/Project/Art/Materials",
                "Assets/Project/Art/Models",
                "Assets/Project/Art/Shaders",
                "Assets/Project/Art/Sprites",
                "Assets/Project/Art/Textures",
                "Assets/Project/Art/VFX",

                "Assets/Project/Audio",
                "Assets/Project/Audio/Music",
                "Assets/Project/Audio/SFX",

                "Assets/Project/PhysicsMaterials",
                "Assets/Project/Prefabs",
                "Assets/Project/Scenes",
                "Assets/Project/ScriptableObjects",
                "Assets/Project/Scripts",
                "Assets/Project/UI",
            };

            foreach (string folder in projectFolders)
            {
                CreateFolderPath(folder);
            }
        }

        public static void CreateFolderPath(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
            {
                return;
            }
            string parentFolder = Path.GetDirectoryName(path);

            // Unity needs /, ensure this is inplace
            parentFolder = parentFolder.Replace('\\', '/');

            string newFolderName = Path.GetFileName(path);

            AssetDatabase.CreateFolder(parentFolder, newFolderName);
            AssetDatabase.Refresh();
        }
    }
}
