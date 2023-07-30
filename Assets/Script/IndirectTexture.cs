using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class IndirectTexture : MonoBehaviour
{
    private Renderer renderer;

    private Material material;
  
    [SerializeField] private Texture tileset;
    [SerializeField] private Vector2Int tileSize;
    [SerializeField] private Vector2Int indirectTextureSize;
    private Texture2D indirectTexture;

    public int TileCount => (tileset.width / tileSize.x) * (tileset.height / tileSize.y);
    public int TileCountX => tileset.width / tileSize.x;
    public int TileCountY => tileset.height / tileSize.y;

    private void Awake()
    {
        renderer = GetComponentInChildren<Renderer>();
        material = new Material(Shader.Find("Unlit/IndirectTexture"));

        indirectTexture = new Texture2D(indirectTextureSize.x, indirectTextureSize.y, TextureFormat.Alpha8, false, true);
        indirectTexture.filterMode = FilterMode.Point;
        var pixels = new Color[indirectTextureSize.x * indirectTextureSize.y];
        for (var i=0;i<pixels.Length;++i)
        {
            pixels[i] = new Color(0, 0, 0, (float)Random.Range(0, TileCount) / TileCount);
        }
        indirectTexture.SetPixels(pixels);
        indirectTexture.Apply();

        material.SetTexture("_MainTex", indirectTexture);
        material.SetTexture("_TileTex", tileset);
        material.SetVector("_TileParams", new Vector4(TileCountX, TileCountY, indirectTextureSize.x, indirectTextureSize.y));

        renderer.material = material;
    }
}