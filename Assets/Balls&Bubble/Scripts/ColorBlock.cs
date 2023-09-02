using System.Collections.Generic;
using UnityEngine;

namespace BallsAndBubble
{
    public class ColorBlock : MonoBehaviour
    {
        [System.Serializable]
        public enum ColorType
        {
            CYAN,
            RED, 
            ORANGE,
            CHARTREUSE,
        }

        [System.Serializable]
        public struct ColorSprite
        {
            public ColorType color;
            public Sprite sprite;
        }

        private SpriteRenderer sr;
        public ColorSprite[] colorSprites;
        private ColorType color;
        private Dictionary<ColorType, Sprite> colorSpriteDict;


        #region Properties
        public ColorType Color
        {
            get { return color; }
            set { SetColor(value); }
        }
        public int NumColors { get => colorSprites.Length; }

     
        #endregion
        private void Awake()
        {
            sr = GetComponent<SpriteRenderer>();
            colorSpriteDict = new Dictionary<ColorType, Sprite>();
            for(int i = 0; i < colorSprites.Length; i++)
            {
                if (colorSpriteDict.ContainsKey(colorSprites[i].color) == false)
                {
                    colorSpriteDict.Add(colorSprites[i].color, colorSprites[i].sprite);
                }
            }
        }

        public void SetColor(ColorType newColor)
        {
            this.color = newColor;

            if (colorSpriteDict.ContainsKey(newColor))
            {
                sr.sprite = colorSpriteDict[newColor];
            }
        }
           

    }
}

