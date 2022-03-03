using System;
using UnityEngine;
using UnityEngine.UI;

namespace Ukiyo.Menu
{
    [ExecuteInEditMode]
    public class Color2Shape : MonoBehaviour
    {
        private static readonly int Shape = Shader.PropertyToID("_MainTex");
        private static readonly int Color = Shader.PropertyToID("_Color");

        [SerializeField]
        private Shader shader;
        [SerializeField]
        private Image targetImage;
        [SerializeField]
        private Texture2D targetShape;
        [SerializeField]
        private Texture2D targetColor;
        [SerializeField] 
        private Material newMaterial;


        private void Start()
        {
            newMaterial = GenerateMaterial();
            targetImage.material = newMaterial;
        }

        private void Update()
        {
            if (Application.isEditor && targetImage.IsActive())
                targetImage.material = newMaterial;
        }

        private Material GenerateMaterial()
        {
            Material material = new Material(shader)
            {
                name = "Color2Shape_" + name
            };
            material.SetTexture(Shape, targetShape);
            material.SetTexture(Color, targetColor);
            return material;
        }
    }
}
