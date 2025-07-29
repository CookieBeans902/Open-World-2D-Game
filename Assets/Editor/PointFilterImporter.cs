using UnityEngine;
using UnityEditor;

public class PointFilterImporter : AssetPostprocessor
{
    void OnPreprocessTexture()
    {
        TextureImporter importer = (TextureImporter)assetImporter;

        // Example condition: only set to point if the name contains "pixel" or any condition you want
        importer.filterMode = FilterMode.Point;

        // Optional: Disable compression for pixel-perfect clarity
        importer.textureCompression = TextureImporterCompression.Uncompressed;

        // Optional: Set NPOT scale to None (for precise pixel sizes)
        importer.npotScale = TextureImporterNPOTScale.None;
    }
}
