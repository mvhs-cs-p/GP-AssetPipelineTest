
namespace MVHS
{

    public static class SAssetPipelineConfig
    {
        public static string[] projectFolders =
        {
            "Assets/Project/",

            "Assets/Project/Art/",
            "Assets/Project/Art/Animations/",
            "Assets/Project/Art/Animators/",
            "Assets/Project/Art/Materials/",
            "Assets/Project/Art/Models/",
            "Assets/Project/Art/Shaders/",
            "Assets/Project/Art/Sprites/",
            "Assets/Project/Art/Textures/",
            "Assets/Project/Art/VFX/",

            "Assets/Project/Audio/",
            "Assets/Project/Audio/Music/",
            "Assets/Project/Audio/SFX/",

            "Assets/Project/PhysicsMaterials/",
            "Assets/Project/Prefabs/",
            "Assets/Project/Scenes/",
            "Assets/Project/ScriptableObjects/",
            "Assets/Project/Scripts/",
            "Assets/Project/UI/"
        };

        public static string ManifestFolderPath 
        { 
            get
            {
                return "Assets/Editor/MVHS/AssetPipelineManager/AssetManifests";
            }
        }
    }
}
