using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace MVHS
{
    /// <summary>
    /// Runs automatically when Unity opens the project.
    ///
    /// Reads all manifest files to build a combined picture of what assets
    /// should exist, then compares against what's actually on disk.
    ///
    /// - Folder is empty + manifest says it should have files = WARN (needs import)
    /// - Folder is empty + no manifest mentions it = FINE (no one has added assets yet)
    ///
    /// This prevents students from accidentally saving scenes with broken
    /// references before importing the asset package.
    /// </summary>
    [InitializeOnLoad]
    public static class AssetPipelineStartupCheck
    {
        [MenuItem("Tools/MVHS/Asset Pipeline Startup Check")]
        public static void CreateProjectDirectory()
        {
            RunStartupCheck();
        }

        static AssetPipelineStartupCheck()
        {
            EditorApplication.delayCall += RunStartupCheck;
        }

        private static void RunStartupCheck()
        {
            //// Find the config.
            //string[] guids = AssetDatabase.FindAssets("t:AssetPackageConfig");
            //if (guids.Length == 0) return;

            //string configPath = AssetDatabase.GUIDToAssetPath(guids[0]);
            //var config = AssetDatabase.LoadAssetAtPath<AssetPackageConfig>(configPath);

            //if (config == null || config.assetFolders.Count == 0) return;

            // Load and combine all manifests.
            var expectedFiles = LoadCombinedManifest();

            // Check each configured folder.
            var foldersNeedingImport = new List<string>();
            int totalExpectedFiles = 0;

            foreach (string folder in SAssetPipelineConfig.projectFolders)
            {
                string projectRoot = Path.GetDirectoryName(Application.dataPath);
                string diskPath = Path.Combine(projectRoot, folder);
                diskPath = diskPath.Replace("\\", "/");

                bool existsOnDisk = Directory.Exists(diskPath);

                int diskCount = 0;
                if (existsOnDisk)
                {
                    diskCount = Directory.GetFiles(diskPath, "*.*", SearchOption.AllDirectories)
                        .Count(f => !f.EndsWith(".meta"));
                }

                // Only flag it if a manifest says files should be here.
                bool hasManifestEntry = expectedFiles.ContainsKey(folder);

                if (hasManifestEntry)
                {
                    int missingCount = 0;

                    foreach (string expectedFile in expectedFiles[folder])
                    {
                        string root = Path.GetDirectoryName(Application.dataPath);
                        string fullFilePath = Path.Combine(root, expectedFile);

                        if (!File.Exists(fullFilePath))
                        {
                            missingCount++;
                        }
                    }

                    if (missingCount > 0)
                    {
                        foldersNeedingImport.Add($"  ⬇  {folder}  ({missingCount} missing file(s))");
                        totalExpectedFiles += missingCount;
                    }
                }
            }

            // Nothing missing — all clear.
            if (foldersNeedingImport.Count == 0) return;

            // Build warning message.
            string folderList = string.Join("\n", foldersNeedingImport);

            string message =
                "This project has asset folders that should contain files " +
                "according to the export manifests, but they are currently empty.\n\n" +
                folderList + "\n\n" +
                $"{totalExpectedFiles} total file(s) need to be imported.\n\n" +
                "DO NOT save any scenes or prefabs until you have imported " +
                "the asset package. Saving now will break asset references.\n\n" +
                "Go to Tools > Asset Package Manager to import.";

            bool openManager = EditorUtility.DisplayDialog(
                "⚠ Asset Import Required",
                message,
                "Open Package Manager",
                "I Understand the Risk");

            if (openManager)
            {
                AssetPipelineManager.ShowWindow();
            }

            Debug.LogWarning(
                $"[AssetPackageStartupCheck] {foldersNeedingImport.Count} folder(s) need " +
                $"asset import ({totalExpectedFiles} files expected). " +
                "Do not save scenes until assets are imported.");
        }

        /// <summary>
        /// Reads every manifest JSON file in the manifest folder and merges
        /// them into a single dictionary of folder -> expected file list.
        /// </summary>
        private static Dictionary<string, List<string>> LoadCombinedManifest()
        {
            var combined = new Dictionary<string, List<string>>();

            string projectRoot = Path.GetDirectoryName(Application.dataPath);
            string diskManifestPath = Path.Combine(projectRoot, SAssetPipelineConfig.ManifestFolderPath);
            diskManifestPath = diskManifestPath.Replace("\\", "/");

            if (!Directory.Exists(diskManifestPath)) return combined;

            string[] manifestFiles = Directory.GetFiles(diskManifestPath, "*.json");

            foreach (string file in manifestFiles)
            {
                try
                {
                    string json = File.ReadAllText(file);
                    var manifest = JsonUtility.FromJson<AssetPipelineManifest>(json);

                    if (manifest?.folders == null) continue;

                    foreach (var folder in manifest.folders)
                    {
                        if (!combined.ContainsKey(folder.folderPath))
                        {
                            combined[folder.folderPath] = new List<string>();
                        }

                        foreach (string f in folder.files)
                        {
                            if (!combined[folder.folderPath].Contains(f))
                            {
                                combined[folder.folderPath].Add(f);
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogWarning(
                        $"[AssetPackageStartupCheck] Could not read manifest {Path.GetFileName(file)}: {e.Message}");
                }
            }

            return combined;
        }
    }
}