using System.Collections.Generic;
using UnityEngine;

namespace MVHS
{
    /// <summary>
    /// Configuration asset that defines which folders in the project may contain
    /// binary assets (models, textures, audio, etc.) distributed via .unitypackage.
    ///
    /// This is set up once by the instructor and rarely changes. It lists ALL
    /// possible asset folders, even ones that are currently empty. The manifest
    /// system (not this config) tracks which folders actually have content.
    ///
    /// Place this in a git-tracked folder (e.g., Assets/Editor/).
    /// The folders it references should be listed in .gitignore.
    ///
    /// Usage: Assets > Create > Asset Package Config
    /// </summary>
    [CreateAssetMenu(fileName = "AssetPipelineConfig", menuName = "MVHS/Asset Pipeline Config")]
    public class AssetPipelineConfig : ScriptableObject
    {
        [Header("Folder Configuration")]
        [Tooltip("ALL folder paths (relative to Assets/) that may contain binary assets.\n" +
                 "List every possible folder, even if currently empty.\n" +
                 "These folders should also be listed in your .gitignore.\n\n" +
                 "Examples: Art/Models, Art/Textures, Audio/Music, Audio/SFX")]
        public List<string> assetFolders = new List<string>();

        [Header("Package Settings")]
        [Tooltip("Name used for the exported .unitypackage file.\n" +
                 "The final filename will be: {packageName}_Assets.unitypackage")]
        public string packageName = "MyProject";

        [Header("Manifest Settings")]
        [Tooltip("Folder inside Assets/ where export manifests are stored.\n" +
                 "This folder MUST be tracked in git.\n" +
                 "Each export creates a new manifest file here.")]
        public string manifestFolder = "Editor/MVHS/AssetPipelineManager/AssetManifests";

        /// <summary>
        /// Returns the full Unity asset path for a configured folder.
        /// </summary>
        public string GetFullPath(string folder)
        {
            return "Assets/" + folder.TrimStart('/');
        }

        /// <summary>
        /// Returns the full Unity asset path for the manifest folder.
        /// </summary>
        public string GetManifestFolderPath()
        {
            return "Assets/" + manifestFolder.TrimStart('/');
        }

        /// <summary>
        /// Returns the generated filename for the package.
        /// </summary>
        public string GetPackageFileName()
        {
            string safeName = string.IsNullOrWhiteSpace(packageName) ? "Project" : packageName;
            return safeName + "_Assets.unitypackage";
        }
    }
}