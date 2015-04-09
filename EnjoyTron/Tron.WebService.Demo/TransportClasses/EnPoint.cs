using System.Runtime.Serialization;
using GameLogic;

namespace Tron.WebService.TransportClasses
{
    [DataContract]
    public class EnPoint
    {
        [DataMember]
        public int Row;

        [DataMember]
        public int Col;

        public EnPoint()
        {
            // default
        }

        public EnPoint(Point p)
        {
            Row = p.Row;
            Col = p.Col;
        }

        public Point ToPoint()
        {
            return new Point(Row, Col);
        }
    }

}
