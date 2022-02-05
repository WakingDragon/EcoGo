using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BP.Core
{
    public enum GameLayer
    {
        Default,
        UI,
        Environment,
        OverlayCamera,
        ParallaxBackground,
        Applications,
        Units,
        Senses,
        NonInteractable
    }

    public static class GameLayers
    {
        public static int layerInt(GameLayer layer)
        {
            switch (layer)
            {
                case GameLayer.UI:
                    return 5;
                case GameLayer.Environment:
                    return 8;
                case GameLayer.OverlayCamera:
                    return 9;
                case GameLayer.ParallaxBackground:
                    return 10;
                case GameLayer.Applications:
                    return 11;
                case GameLayer.Units:
                    return 12;
                case GameLayer.Senses:
                    return 13;
                case GameLayer.NonInteractable:
                    return 14;
                default:
                    return 0;
            }
        }

        public static int layerMask(GameLayer layer)
        {
            return 1 << layerInt(layer);
        }

    }
}

