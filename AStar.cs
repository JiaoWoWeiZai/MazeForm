using System;
using System.Collections.Generic;
using System.Drawing;

namespace MazeForm
{
    class AStar
    {
        Point Location { get; set; }
        int G { get; set; }
        int H { get; set; }

        int F
        {
            get { return G + H; }
        }
        AStar FatherAStar { get; set; }

        private AStar(Point location, int g, int h, AStar fatherAStar)
        {
            Location = location;
            G = g;
            H = h;
            FatherAStar = fatherAStar;
        }

        public static List<ScriptPoint> AstarScript { get; set; }

        public static List<Point> GetPath(Maze m, Point start, Point end)
        {
            //声明开启列表和关闭列表
            List<AStar> openList = new List<AStar>(), closeList = new List<AStar>();
            AstarScript=new List<ScriptPoint>();
            //声明起始点
            AStar sStar = new AStar(start, 0, 0, null);
            //把起始格添加到开启列表
            openList.Add(sStar);
            bool isFind = false;
            do
            {
                //寻找开启列表中F值最低的格子。我们称它为当前格。
                int min = 0;
                for (int i = 0; i < openList.Count - 1; i++)
                    if (openList[i].F < openList[min].F)
                        min = i;
                AStar now = openList[min];
                //把当前格切换到关闭列表
                closeList.Add(now);
                AstarScript.Add(new ScriptPoint(now.Location, false));
                openList.RemoveAt(min);
                //检测当前格周围的八个格子
                if (now.Location.X > 0 && now.Location.Y > 0)
                {
                    CheckPoint(new Point(now.Location.X - 1, now.Location.Y - 1), openList, closeList, now, end, m,14);
                }
                if (now.Location.Y > 0)
                {
                    CheckPoint(new Point(now.Location.X, now.Location.Y - 1), openList, closeList, now, end, m,10);
                }
                if (now.Location.X < m.MazeMap.GetUpperBound(0) && now.Location.Y > 0)
                {
                    CheckPoint(new Point(now.Location.X + 1, now.Location.Y - 1), openList, closeList, now, end, m,14);
                }
                if (now.Location.X < m.MazeMap.GetUpperBound(0))
                {
                    CheckPoint(new Point(now.Location.X + 1, now.Location.Y), openList, closeList, now, end, m,10);
                }
                if (now.Location.X < m.MazeMap.GetUpperBound(0) && now.Location.Y < m.MazeMap.GetUpperBound(1))
                {
                    CheckPoint(new Point(now.Location.X + 1, now.Location.Y + 1), openList, closeList, now, end, m,14);
                }
                if (now.Location.Y < m.MazeMap.GetUpperBound(1))
                {
                    CheckPoint(new Point(now.Location.X, now.Location.Y + 1), openList, closeList, now, end, m,10);
                }
                if (now.Location.X > 0 && now.Location.Y < m.MazeMap.GetUpperBound(1))
                {
                    CheckPoint(new Point(now.Location.X - 1, now.Location.Y + 1), openList, closeList, now, end, m,14);
                }
                if (now.Location.X > 0)
                {
                    CheckPoint(new Point(now.Location.X - 1, now.Location.Y), openList, closeList, now, end, m,10);
                }
                if (openList.Count == 0 || Contain(closeList, end))
                {
                    isFind = true;
                }
            } while (!isFind);
            if (Contain(closeList, end))
            {
                AStar star = FindPoint(closeList, end);
                List<Point> Path = new List<Point>();
                while (star != null)
                {
                    Path.Insert(0, star.Location);
                    AstarScript.Add(new ScriptPoint(star.Location, true));
                    star = star.FatherAStar;
                }
                return Path;
            }
            else
            {
                return null;
            }
        }

        private static void CheckPoint(Point p, List<AStar> openList, List<AStar> closeList, AStar now, Point end, Maze m,int g)
        {
            if (!Contain(closeList, p) && m.MazeMap[p.X, p.Y] == 1)
            {
                AStar star = new AStar(p, now.G + g, end.X - p.X + end.Y - p.Y, now);
                if (Contain(openList, star))
                {
                    ReplaceByG(openList, star);
                }
                else
                {
                    openList.Add(star);
                }
            }
        }

        private static Boolean Contain(List<AStar> stars, Point p)
        {
            foreach (var star in stars)
            {
                if (star.Location == p)
                {
                    return true;
                }
            }
            return false;
        }

        private static Boolean Contain(List<AStar> stars, AStar s)
        {
            foreach (var star in stars)
            {
                if (star.Location == s.Location)
                {
                    return true;
                }
            }
            return false;
        }

        private static void ReplaceByG(List<AStar> stars, AStar s)
        {
            for (int i = 0; i < stars.Count; i++)
            {
                if (stars[i].Location == s.Location)
                {
                    if (stars[i].G > s.G)
                    {
                        stars[i] = s;
                    }
                    return;
                }
            }
        }

        private static AStar FindPoint(List<AStar> stars, Point p)
        {
            foreach (var star in stars)
            {
                if (star.Location == p)
                {
                    return star;
                }
            }
            return null;
        }
    }
}
