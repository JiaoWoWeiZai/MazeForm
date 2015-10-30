using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace MazeForm
{
    public class ScriptPoint
    {
        public Point P { get; set; }
        public Boolean A { get; set; }

        public ScriptPoint(Point point, Boolean a)
        {
            P = point;
            A = a;
        }
    }

    public class LinkPoint
    {
        public Point Value { get; set; }
        public LinkPoint Father { get; set; }

        public LinkPoint(Point value, LinkPoint father)
        {
            Value = value;
            Father = father;
        }

        public LinkPoint(Point value)
        {
            Value = value;
            Father= null;
        }
        public static LinkPoint Contains(List<LinkPoint> list, LinkPoint lp)
        {
            foreach (var l in list)
            {
                if (l.Value==lp.Value)
                {
                    return l;
                }
            }
            return null;
        }
    }
}
