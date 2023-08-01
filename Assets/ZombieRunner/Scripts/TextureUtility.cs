using System.Collections;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public class TextureUtility : MonoBehaviour
{
    public int index;
    public RenderTexture renderTexture;
    
    private static Texture2D RenderTextureToTexture2D(RenderTexture texture)
    {
        var oldRen = RenderTexture.active;
        RenderTexture.active = texture;
        var texture2D = new Texture2D(texture.width, texture.height, TextureFormat.RGBA64, false, false);
        texture2D.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
        texture2D.Apply();
        RenderTexture.active = oldRen;
        return texture2D;
    }

    private static void CreateImageFiles(Texture2D tex, int index)
    {
        Debug.Log("CreateImageFiles");
        var arrayData = tex.EncodeToPNG();
        if (arrayData .Length < 1)
        {
            return;
        }
        var path = @"D:\Code\ZombieComing\Assets\ZombieRunner\Materials\" + index + ".png";
        File.WriteAllBytes(path, arrayData );
        Debug.Log("Finish");
        //AssetDatabase.Refresh();
    }

    [Button]
    private void CreateImage()
    {
        CreateImageFiles(RenderTextureToTexture2D(renderTexture), index);
    }
}
