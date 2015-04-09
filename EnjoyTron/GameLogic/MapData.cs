using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class MapData : ICloneable
    {
        public int Width;
        public int Height;
        public TileType[,] Tiles;

        public object Clone()
        {
            return new MapData
            {
                Width = this.Width,
                Height = this.Height,
                Tiles = (TileType[,]) this.Tiles.Clone()
            };
        }
    }

    public enum TileType : byte
    {
        EMPTY, BLOCK,
        HEAD0, HEAD1, HEAD2, HEAD3, HEAD4, HEAD5, HEAD6, HEAD7,
        BODY0, BODY1, BODY2, BODY3, BODY4, BODY5, BODY6, BODY7
    }
}
