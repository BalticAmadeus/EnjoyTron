using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using GameLogic;

namespace Tron.WebService.TransportClasses
{
    [DataContract]
    public class EnMapData
    {
        [DataMember]
        public int Width;

        [DataMember]
        public int Height;

        [DataMember]
        public List<string> Rows;

        public EnMapData()
        {
            // default
        }

        public EnMapData(MapData md)
        {
            Width = md.Width;
            Height = md.Height;
            Rows = new List<string>();
            for (int row = 0; row < Height; row++)
            {
                Rows.Add(BuildRow(md, row));
            }
        }

        public MapData ToMapData()
        {
            MapData md = new MapData();
            md.Width = Width;
            md.Height = Height;
            if (md.Width > Settings.MapSizeLimit || md.Height > Settings.MapSizeLimit)
                throw new ApplicationException("Map too big");
            md.Tiles = new TileType[Height, Width];
            for (int row = 0; row < Height; row++)
            {
                parseRow(row, Rows[row], md);
            }
            return md;
        }

        private void parseRow(int row, string data, MapData md)
        {
            for (int col = 0; col < Width; col++)
            {
                md.Tiles[row, col] = parseTile(data[col]);
            }
        }

        private TileType parseTile(char code)
        {
            switch (code)
            {
                case '#':
                    return TileType.BLOCK;
                case ' ':
                case '.':
                    return TileType.EMPTY;
                case '0':
                    return TileType.HEAD0;
                case '1':
                    return TileType.HEAD1;
                case '2':
                    return TileType.HEAD2;
                case '3':
                    return TileType.HEAD3;
                case '4':
                    return TileType.HEAD4;
                case '5':
                    return TileType.HEAD5;
                case '6':
                    return TileType.HEAD6;
                case '7':
                    return TileType.HEAD7;
                default:
                    throw new ApplicationException(string.Format("Invalid map tile character '{0}'", code.ToString()));
            }
        }

        public static string BuildRow(MapData md, int row)
        {
            StringBuilder sb = new StringBuilder();
            for (int col = 0; col < md.Width; col++)
            {
                sb.Append(BuildTile(md.Tiles[row, col]));
            }
            return sb.ToString();
        }

        public static char BuildTile(TileType tile)
        {
            switch (tile)
            {
                case TileType.BLOCK:
                    return '#';
                case TileType.EMPTY:
                    return '.';
                case TileType.HEAD0:
                case TileType.HEAD1:
                case TileType.HEAD2:
                case TileType.HEAD3:
                case TileType.HEAD4:
                case TileType.HEAD5:
                case TileType.HEAD6:
                case TileType.HEAD7:
                    return (char)('0' + (tile - TileType.HEAD0));
                case TileType.BODY0:
                case TileType.BODY1:
                case TileType.BODY2:
                case TileType.BODY3:
                case TileType.BODY4:
                case TileType.BODY5:
                case TileType.BODY6:
                case TileType.BODY7:
                    return (char)('a' + (tile - TileType.BODY0));
                default:
                    return '#';
            }
        }
    }
}
