using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System;

/// <summary>
/// Tries to parse a json spritesheet file for a spritesheet exportet with texturepacker
/// </summary>
public class SpriteImporter : AssetPostprocessor
{
    const float PIXELS_PER_UNIT = 32.0f;

    void OnPreprocessTexture()
    {
        if (assetPath.EndsWith(".png", System.StringComparison.OrdinalIgnoreCase))
        {
            TextureImporter textureImporter = assetImporter as TextureImporter;

            //only process sprites:
            if (textureImporter.textureType == TextureImporterType.Sprite)
            {
                //textureImporter.spritePixelsPerUnit = PIXELS_PER_UNIT;
                textureImporter.mipmapEnabled = false;
                textureImporter.filterMode = FilterMode.Point;
                textureImporter.textureFormat = TextureImporterFormat.RGBA32;
            }
        }
    }

}

