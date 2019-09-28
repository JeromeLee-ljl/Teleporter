using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class RenderTex2Tex2d: MonoBehaviour
{
    public RenderTexture renderTex;
    public MeshRenderer renderTarget;
    private Texture2D _texture;

    void Start()
    {
        _texture = new Texture2D(renderTex.width, renderTex.height);

        // renderTarget.material.mainTexture = _texture;
        renderTarget.material.mainTexture = renderTex;
    }

    private void OnPostRender()
    {
        // Debug.Log("render");
        //设定当前RenderTexture为快照相机的targetTexture
        // RenderTexture.active = renderTex;

        // //读取缓冲区像素信息
        // _texture.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        // _texture.Apply();
        // renderTarget.
    }
}
