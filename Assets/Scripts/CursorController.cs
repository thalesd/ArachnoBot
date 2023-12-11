using UnityEngine;
using Assets.Scripts.Enums;
using System.Collections.Generic;
using System;

namespace Assets.Scripts
{
    public class CursorController : Singleton<CursorController>
    {
        [SerializeField]
        private List<CursorType> CursorTypes;
        [SerializeField]
        private List<Texture2D> CursorsForTypes;

        private Texture2D currentMouseTexture;

        public CursorController()
        {
            CursorTypes = new List<CursorType>();
            foreach (int type in Enum.GetValues(typeof(CursorType)))
            {
                this.CursorTypes.Add((CursorType)type);
            }
        }
        
        public void Start()
        {
            ChangeCursor(CursorType.Default);
        }

        public static void ChangeCursor(CursorType newCursor)
        {
            if(newCursor == CursorType.None) Cursor.visible = false;
            else
            {
                Instance.currentMouseTexture = Instance.CursorsForTypes[(int)newCursor];

                Cursor.SetCursor(Instance.currentMouseTexture, Vector2.zero, CursorMode.Auto);
            }
        }
    }
}
