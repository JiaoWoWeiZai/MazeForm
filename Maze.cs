using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using MazeForm.Properties;

namespace MazeForm
{
    /// <summary>
    /// 生成迷宫类型枚举
    /// </summary>
    public enum CreateType
    {
        Prim,
        RecursiveBacktrack,
        RecursiveDivision,
        DfsGraph
    }

    /// <summary>
    /// 寻路类型枚举
    /// </summary>
    public enum FindType
    {
        DfsFind,
        BFSFind,
        AStarFind
    }


    /// <summary>
    /// 迷宫类
    /// </summary>
    public class Maze
    {
        #region 属性和全局变量区域

        /// <summary>
        /// 迷宫宽度
        /// </summary>

        int MazeWidth { get; set; }

        /// <summary>
        /// 迷宫高度
        /// </summary>
        int MazeHeight { get; set; }
        int[,] d = new int[,] { { 0, -1 }, { 1, 0 }, { 0, 1 }, { -1, 0 } };

        /// <summary>
        /// 迷宫数组
        /// </summary>
        public int[,] MazeMap { get; private set; }
        public int[,] PreviewMaze { get; set; }

        /// <summary>
        /// 用于显示迷宫的Bitmap对象
        /// </summary>
        public Bitmap Bmp { get; private set; }

        /// <summary>
        /// 所有经过的点
        /// </summary>
        public List<Point> AllPathList { get; set; }

        /// <summary>
        /// 迷宫通路坐标
        /// </summary>
        public Stack<Point> PathStack { get; set; }

        /// <summary>
        /// 全部迷宫路径
        /// </summary>
        public List<List<Point>> PathList { get; set; }

        public List<ScriptPoint> FindScript { get; set; }

        /// <summary>
        /// 迷宫生成剧本
        /// </summary>
        public List<ScriptPoint> CreateScript { get; set; }

        //单位格的宽度    单位格的高度
        private int BlockWidth, BlockHeight;

        //block:不可通行    unBlock:可通行
        private const int Block = 0, UnBlock = 1;

        //haveBorder:是否有外围边框
        readonly bool HaveBorder;

        // 全局随机数变量
        readonly Random _r = new Random();

        private int _minArea = 1;

        public TimeSpan CreateTime { get; set; }

        public TimeSpan FindTime { get; set; }
        #endregion

        #region 一般方法区域

        /// <summary>
        /// 初始化迷宫
        /// </summary>
        /// <param name="mazewidth">迷宫宽度</param>
        /// <param name="mazeheight">迷宫高度</param>
        /// <param name="border">是否有边框</param>
        public Maze(int mazewidth, int mazeheight, bool border)
        {
            this.MazeWidth = mazewidth;
            this.MazeHeight = mazeheight;
            HaveBorder = border;

        }

        /// <summary>
        /// 生成迷宫
        /// </summary>
        /// <param name = "type">生成迷宫算法</param>
        /// <param name="startX">起点X坐标</param>
        /// <param name="startY">起点Y坐标</param>
        private void CreateMaze(CreateType type, int startX = -1, int startY = -1)
        {
            Stopwatch createStopwatch=new Stopwatch();
            createStopwatch.Start();
            //迷宫尺寸合法化
            if (MazeWidth < 1)
                MazeWidth = 1;
            if (MazeHeight < 1)
                MazeHeight = 1;
            //迷宫起点合法化
            if (startX < 0 || startX >= MazeWidth)
                startX = _r.Next(0, MazeWidth);
            if (startY < 0 || startY >= MazeHeight)
                startY = _r.Next(0, MazeHeight);
            //减去边框所占的格子
            if (!HaveBorder)
            {
                MazeWidth--;
                MazeHeight--;
            }
            //迷宫尺寸换算成带墙尺寸
            MazeWidth *= 2;
            MazeHeight *= 2;
            //迷宫起点换算成带墙起点
            startX *= 2;
            startY *= 2;
            if (HaveBorder)
            {
                startX++;
                startY++;
            }
            //产生空白迷宫
            MazeMap = new int[MazeWidth + 1, MazeHeight + 1];
            for (int x = 0; x <= MazeWidth; x++)
            {
                //mazeMap.Add(new BitArray(mazeHeight + 1));
                for (int y = 0; y <= MazeHeight; y++)
                {
                    MazeMap[x, y] = Block;
                }
            }
            //初始化剧本
            CreateScript = new List<ScriptPoint>();
            //产生迷宫
            switch (type)
            {
                case CreateType.Prim:
                    PreviewMaze = CopyInts(MazeMap);
                    Prim(startX, startY, MazeWidth - 1, MazeHeight - 1);
                    break;
                case CreateType.RecursiveBacktrack:
                    PreviewMaze = CopyInts(MazeMap);
                    RecursiveBacktrack(startX, startY, MazeWidth - 1, MazeHeight - 1);
                    break;
                case CreateType.RecursiveDivision:
                    for (int x = 0; x <= MazeWidth; x++)
                    {
                        for (int y = 0; y <= MazeHeight; y++)
                        {
                            if (HaveBorder && (x == 0 || x == MazeWidth || y == 0 || y == MazeHeight))
                            {
                                MazeMap[x, y] = Block;
                            }
                            else
                            {
                                MazeMap[x, y] = UnBlock;
                            }
                        }
                    }
                    if (HaveBorder)
                    {
                        PreviewMaze = CopyInts(MazeMap);
                        RecursiveDivision(1, MazeWidth - 1, 1, MazeHeight - 1);
                    }
                    else
                    {
                        PreviewMaze = CopyInts(MazeMap);
                        RecursiveDivision(0, MazeWidth, 0, MazeHeight);
                    }
                    break;
                case CreateType.DfsGraph:
                    PreviewMaze = CopyInts(MazeMap);
                    DfsGraph(startX, startY, MazeWidth - 1, MazeHeight - 1);
                    break;
            }
            createStopwatch.Stop();
            CreateTime = createStopwatch.Elapsed;
        }

        /// <summary>
        /// 初始化迷宫数组
        /// </summary>
        /// <param name="type">生成所使用的算法</param>
        /// <param name="start">起点</param>
        public void CreateMaze(CreateType type, Point start)
        {
            CreateMaze(type, start.X, start.Y);
        }

        /// <summary>
        /// 寻找迷宫的通路
        /// </summary>
        /// <param name="type">寻路所使用的算法</param>
        /// <param name="start">寻路起点</param>
        /// <param name="end">寻路终点</param>
        public void FindPath(FindType type, Point start, Point end = new Point())
        {
            Stopwatch findStopwatch=new Stopwatch();
            findStopwatch.Start();
            if (end == new Point(0, 0))
            {
                end = new Point(MazeMap.GetUpperBound(0) - 1, MazeMap.GetUpperBound(1) - 1);
            }
            switch (type)
            {
                case FindType.DfsFind:
                    DfsFind(start, end);
                    break;
                case FindType.AStarFind:
                    AStarFind(start, end);
                    break;
                case FindType.BFSFind:
                    BFSFind(start, end);
                    break;
            }
            findStopwatch.Stop();
            FindTime = findStopwatch.Elapsed;
        }

        #endregion

        #region 迷宫生成区

        /// <summary>
        /// 普利姆迷宫生成法
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="widthLimit"></param>
        /// <param name="heightLimit"></param>
        private void Prim(int startX, int startY, int widthLimit, int heightLimit)
        {
            //邻墙列表
            List<int> blockPos = new List<int>();
            //随机墙的索引
            int blockIndex = 0;
            //将起点作为目标格
            int targetX = startX, targetY = startY;
            //将起点标记为通路
            MazeMap[targetX, targetY] = UnBlock;
            CreateScript.Add(new ScriptPoint(new Point(targetX, targetY), false));

            //记录邻墙
            if (targetY > 1)
            {
                blockPos.AddRange(new int[] { targetX, targetY - 1, 0 });
            }
            if (targetX < widthLimit)
            {
                blockPos.AddRange(new int[] { targetX + 1, targetY, 1 });
            }
            if (targetY < heightLimit)
            {
                blockPos.AddRange(new int[] { targetX, targetY + 1, 2 });
            }
            if (targetX > 1)
            {
                blockPos.AddRange(new int[] { targetX - 1, targetY, 3 });
            }
            while (blockPos.Count > 0)
            {
                //随机选一堵墙
                blockIndex = _r.Next(0, blockPos.Count / 3) * 3;
                //找到墙对面的墙
                switch (blockPos[blockIndex + 2])
                {
                    case 0:
                        targetX = blockPos[blockIndex];
                        targetY = blockPos[blockIndex + 1] - 1;
                        break;
                    case 1:
                        targetX = blockPos[blockIndex] + 1;
                        targetY = blockPos[blockIndex + 1];
                        break;
                    case 2:
                        targetX = blockPos[blockIndex];
                        targetY = blockPos[blockIndex + 1] + 1;
                        break;
                    case 3:
                        targetX = blockPos[blockIndex] - 1;
                        targetY = blockPos[blockIndex + 1];
                        break;
                }
                //如果目标格未连通
                if (MazeMap[targetX, targetY] == Block)
                {
                    //联通目标格
                    MazeMap[blockPos[blockIndex], blockPos[blockIndex + 1]] = UnBlock;
                    CreateScript.Add(new ScriptPoint(new Point(blockPos[blockIndex], blockPos[blockIndex + 1]), false));
                    MazeMap[targetX, targetY] = UnBlock;
                    CreateScript.Add(new ScriptPoint(new Point(targetX, targetY), false)); ;
                    //添加目标格相邻格
                    if (targetY > 1 && MazeMap[targetX, targetY - 1] == Block && MazeMap[targetX, targetY - 2] == Block)
                    {
                        blockPos.AddRange(new int[] { targetX, targetY - 1, 0 });
                    }
                    if (targetX < widthLimit && MazeMap[targetX + 1, targetY] == Block && MazeMap[targetX + 2, targetY] == Block)
                    {
                        blockPos.AddRange(new int[] { targetX + 1, targetY, 1 });
                    }
                    if (targetY < heightLimit && MazeMap[targetX, targetY + 1] == Block && MazeMap[targetX, targetY + 2] == Block)
                    {
                        blockPos.AddRange(new int[] { targetX, targetY + 1, 2 });
                    }
                    if (targetX > 1 && MazeMap[targetX - 1, targetY] == Block && MazeMap[targetX - 1, targetY] == Block)
                    {
                        blockPos.AddRange(new int[] { targetX - 1, targetY, 3 });
                    }
                }
                blockPos.RemoveRange(blockIndex, 3);
            }
        }

        /// <summary>
        /// 递归回溯法迷宫生成法
        /// </summary>
        /// <param name="startX"></param>
        /// <param name="startY"></param>
        /// <param name="widthLimit"></param>
        /// <param name="heightLimit"></param>
        private void RecursiveBacktrack(int startX, int startY, int widthLimit, int heightLimit)
        {
            PathStack = new Stack<Point>();
            //周围未连通格坐标
            int[] blockPos = new int[4];
            //周围未标记格的数量
            int blockNum = 0;

            //将起点作为当前格
            int currentX = startX;
            int currentY = startY;

            //标记起点
            MazeMap[currentX, currentY] = UnBlock;
            CreateScript.Add(new ScriptPoint(new Point(currentX, currentY), false));
            do
            {
                //检测周围有没有未连通的格子
                blockNum = 0;
                //检查上方
                if (currentY > 1 && MazeMap[currentX, currentY - 2] == Block)
                {
                    blockPos[blockNum] = 0;
                    blockNum++;
                }
                //检查右侧
                if (currentX < widthLimit && MazeMap[currentX + 2, currentY] == Block)
                {
                    blockPos[blockNum] = 1;
                    blockNum++;
                }
                //检查下方
                if (currentY < heightLimit && MazeMap[currentX, currentY + 2] == Block)
                {
                    blockPos[blockNum] = 2;
                    blockNum++;
                }
                //检查左侧
                if (currentX > 1 && MazeMap[currentX - 2, currentY] == Block)
                {
                    blockPos[blockNum] = 3;
                    blockNum++;
                }

                //选出下一个当前格
                if (blockNum > 0)
                {
                    //随机选择一个邻格
                    blockNum = _r.Next(0, blockNum);
                    //把当前格入栈
                    PathStack.Push(new Point(currentX, currentY));
                    //连通邻格，并将邻格指定为当前格
                    switch (blockPos[blockNum])
                    {
                        case 0:
                            MazeMap[currentX, currentY - 1] = UnBlock;
                            CreateScript.Add(new ScriptPoint(new Point(currentX, currentY - 1), false));
                            currentY -= 2;
                            break;
                        case 1:
                            MazeMap[currentX + 1, currentY] = UnBlock;
                            CreateScript.Add(new ScriptPoint(new Point(currentX + 1, currentY), false));
                            currentX += 2;
                            break;
                        case 2:
                            MazeMap[currentX, currentY + 1] = UnBlock;
                            CreateScript.Add(new ScriptPoint(new Point(currentX, currentY + 1), false));
                            currentY += 2;
                            break;
                        case 3:
                            MazeMap[currentX - 1, currentY] = UnBlock;
                            CreateScript.Add(new ScriptPoint(new Point(currentX - 1, currentY), false));
                            currentX -= 2;
                            break;

                    }
                    //标记当前格
                    MazeMap[currentX, currentY] = UnBlock;
                    CreateScript.Add(new ScriptPoint(new Point(currentX, currentY), false));
                }
                else if (PathStack.Count > 0)
                {
                    //将栈顶作为当前格
                    Point top = PathStack.Pop();
                    currentY = top.Y;
                    currentX = top.X;
                }
            } while (PathStack.Count > 0);
        }

        /// <summary>
        /// 递归分割法迷宫生成法
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="top"></param>
        /// <param name="bottom"></param>
        private void RecursiveDivision(int left, int right, int top, int bottom)
        {
            //要分隔区域的尺寸
            int areaWidth = right - left;
            int areaHeight = bottom - top;

            //检查是否达到了最小分割面积
            if (_minArea > 3)
            {
                if ((areaWidth + 1) * (areaHeight + 1) <= _minArea)
                {
                    return;
                }
            }

            //假设分割点不存在
            int dx = -1;
            int dy = -1;

            //产生随机分割点
            if (areaWidth > 1)
            {
                dx = left + 1 + _r.Next(0, areaWidth / 2) * 2;
            }
            if (areaHeight > 1)
            {
                dy = top + 1 + _r.Next(0, areaHeight / 2) * 2;
            }

            //没有继续分割的必要
            if (dx == -1 && dy == -1)
            {
                return;
            }

            //补上墙壁
            if (dx != -1)
            {
                for (int y = top; y <= bottom; y++)
                {
                    MazeMap[dx, y] = Block;
                    CreateScript.Add(new ScriptPoint(new Point(dx, y), true));
                }
            }
            if (dy != -1)
            {
                for (int x = left; x <= right; x++)
                {
                    MazeMap[x, dy] = Block;
                    CreateScript.Add(new ScriptPoint(new Point(x, dy), true));
                }
            }

            int rand;
            //为确保连通，随机打通墙壁且不产生环路，并递归分割子区域
            if (dx != -1 && dy != -1)
            {
                int side = _r.Next(0, 4);
                if (side != 0)
                {
                    rand = top + _r.Next(0, (dy - 1 - top) / 2 + 1) * 2;
                    MazeMap[dx, rand] = UnBlock;
                    CreateScript.Add(new ScriptPoint(new Point(dx, rand), false));
                }
                if (side != 1)
                {
                    rand = dx + 1 + _r.Next(0, (right - dx - 1) / 2 + 1) * 2;
                    MazeMap[rand, dy] = UnBlock;
                    CreateScript.Add(new ScriptPoint(new Point(rand, dy), false));
                }
                if (side != 2)
                {
                    rand = dy + 1 + _r.Next(0, (bottom - dy - 1) / 2 + 1) * 2;
                    MazeMap[dx, rand] = UnBlock;
                    CreateScript.Add(new ScriptPoint(new Point(dx, rand), false));
                }
                if (side != 3)
                {
                    rand = left + _r.Next(0, (dx - 1 - left) / 2 + 1) * 2;
                    MazeMap[rand, dy] = UnBlock;
                    CreateScript.Add(new ScriptPoint(new Point(rand, dy), false));
                }
                RecursiveDivision(left, dx - 1, top, dy - 1);
                RecursiveDivision(dx + 1, right, top, dy - 1);
                RecursiveDivision(dx + 1, right, dy + 1, bottom);
                RecursiveDivision(left, dx - 1, dy + 1, bottom);
            }
            else if (dx == -1)
            {
                rand = left + _r.Next(0, areaWidth / 2 + 1) * 2;
                MazeMap[rand, dy] = UnBlock;
                CreateScript.Add(new ScriptPoint(new Point(rand, dy), false));
                RecursiveDivision(left, right, top, dy - 1);
                RecursiveDivision(left, right, dy + 1, bottom);
            }
            else if (dy == -1)
            {
                rand = top + _r.Next(0, areaHeight / 2 + 1) * 2;
                MazeMap[dx, rand] = UnBlock;
                CreateScript.Add(new ScriptPoint(new Point(dx, rand), false));
                RecursiveDivision(left, dx - 1, top, bottom);
                RecursiveDivision(dx + 1, right, top, bottom);
            }
        }

        /// <summary>
        /// 图的深度遍历迷宫生成法
        /// </summary>
        private void DfsGraph(int startX, int startY, int widthLimit, int heightLimit)
        {

            PathStack = new Stack<Point>();// 用栈来存储路径所通过点的坐标
            Point start = new Point(startX, startY), end = new Point(MazeMap.GetUpperBound(0) - 1, MazeMap.GetUpperBound(1) - 1), now;
            List<int>[,] pointMap = new List<int>[MazeMap.GetUpperBound(0) + 1, MazeMap.GetUpperBound(1) + 1];
            for (int i = 0; i <= MazeMap.GetUpperBound(0); i++)
            {
                MazeMap[i, 0] = 2;
                MazeMap[i, MazeMap.GetUpperBound(1)] = 2;
            }
            for (int i = 0; i <= MazeMap.GetUpperBound(1); i++)
            {
                MazeMap[0, i] = 2;
                MazeMap[MazeMap.GetUpperBound(1), i] = 2;
            }
            for (int i = 0; i <= pointMap.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= pointMap.GetUpperBound(1); j++)
                {
                    pointMap[i, j] = new List<int>(new int[] { 0, 1, 2, 3 });
                }
            }
            now = new Point(start.X, start.Y);
            MazeMap[now.X, now.Y] = UnBlock;
            CreateScript.Add(new ScriptPoint(new Point(now.X, now.Y), false));
            PathStack.Push(new Point(now.X, now.Y));
            do
            {
                if (pointMap[now.X, now.Y].Count == 0)
                {
                    MazeMap[now.X, now.Y] = Block;
                    CreateScript.Add(new ScriptPoint(new Point(now.X, now.Y), true));
                    PathStack.Pop();
                    now = PathStack.Peek();
                    continue;
                }
                int n = _r.Next(0, pointMap[now.X, now.Y].Count), direction;
                direction = pointMap[now.X, now.Y][n];
                pointMap[now.X, now.Y].RemoveAt(n);
                if (MazeMap[now.X + d[direction, 0], now.Y + d[direction, 1]] == Block)
                {
                    now = new Point(now.X + d[direction, 0], now.Y + d[direction, 1]);
                    MazeMap[now.X, now.Y] = UnBlock;
                    CreateScript.Add(new ScriptPoint(new Point(now.X, now.Y), false));

                }
                if (now != PathStack.Peek())
                {
                    PathStack.Push(new Point(now.X, now.Y));
                }
                if (now == end)
                {
                    break;
                }

            } while (PathStack.Count > 0);
            for (int i = 0; i <= MazeMap.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= MazeMap.GetUpperBound(1); j++)
                {
                    int rand = _r.Next(0, 2);
                    if (MazeMap[i, j] == Block)
                    {
                        MazeMap[i, j] = rand;
                        CreateScript.Add(new ScriptPoint(new Point(i, j), !Convert.ToBoolean(rand)));
                    }
                    if (MazeMap[i, j] == 2)
                        MazeMap[i, j] = Block;
                }
            }
        }

        #endregion

        #region 绘制区

        /// <summary>
        /// 将迷宫显示到指定PictureBox
        /// </summary>
        /// <param name="p">要显示迷宫的PictureBox</param>
        public void DrawMaze(PictureBox p)
        {
            Image block = Resources.block;
            int XU = MazeMap.GetUpperBound(0) + 1, YU = MazeMap.GetUpperBound(1) + 1;
            BlockWidth = block.Width; BlockHeight = block.Height;
            Bmp = new Bitmap(BlockWidth * XU, BlockHeight * XU);
            Graphics g = Graphics.FromImage(Bmp);
            for (int i = 0; i < XU; i++)
            {
                for (int j = 0; j < YU; j++)
                {
                    if (MazeMap[i, j] == 0)
                    {
                        g.DrawImage(block, i * BlockWidth, j * BlockHeight, BlockWidth, BlockHeight);
                    }
                }
            }

            p.Image = Bmp;
        }

        /// <summary>
        /// 在指定的PictureBox演示寻路过程
        /// </summary>
        /// <param name="p">要演示寻路过程的PictureBox</param>
        public void DrawGoPath(PictureBox p)
        {
            //拷贝迷宫地图到新对象
            Bitmap newBmp = new Bitmap(Bmp);
            //创建绘制对象
            Graphics g = Graphics.FromImage(newBmp);
            foreach (var value in FindScript)
            {
                if (value.A)
                    g.DrawImage(Resources.unblock, value.P.X * BlockWidth, value.P.Y * BlockHeight, BlockWidth, BlockHeight);
                else
                    g.DrawImage(Resources.step, value.P.X * BlockWidth, value.P.Y * BlockHeight, BlockWidth, BlockHeight);
                p.Image = newBmp;
                p.Invalidate();
                Application.DoEvents();
            }
        }

        /// <summary>
        /// 显示路径到指定的PictureBox
        /// </summary>
        /// <param name="p">要显示路径的PictureBox</param>
        public void DrawPath(PictureBox p)
        {
            //拷贝迷宫地图到新对象
            Bitmap newBmp = new Bitmap(Bmp);
            //创建绘制对象
            Graphics g = Graphics.FromImage(newBmp);
            //将栈内记录的路线点绘制在迷宫图上
            Stack<Point> points = new Stack<Point>(PathStack);
            while (points.Count > 0)
            {
                Point point = points.Pop();
                g.DrawImage(Resources.unblock, point.X * BlockWidth, point.Y * BlockWidth, BlockWidth, BlockHeight);
            }
            p.Image = newBmp;
            p.Invalidate();
        }

        /// <summary>
        /// 在指定PictureBox演示生成过程
        /// </summary>
        /// <param name="p">要展示的PictureBox</param>
        public void DrawCreate(PictureBox p)
        {
            Bitmap newBitmap = new Bitmap(Bmp.Width, Bmp.Height);
            Graphics g = Graphics.FromImage(newBitmap);
            for (int i = 0; i <= PreviewMaze.GetUpperBound(0); i++)
                for (int j = 0; j <= PreviewMaze.GetUpperBound(1); j++)
                    if (PreviewMaze[i, j] == Block)
                        g.DrawImage(Resources.block, i * BlockWidth, j * BlockHeight, BlockWidth, BlockHeight);
            p.Image = newBitmap;
            p.Invalidate();
            Application.DoEvents();
            foreach (var value in CreateScript)
            {
                if (value.A)
                    g.DrawImage(Resources.block, value.P.X * BlockWidth, value.P.Y * BlockHeight, BlockWidth, BlockHeight);
                else
                    g.DrawImage(Resources.empty, value.P.X * BlockWidth, value.P.Y * BlockHeight, BlockWidth, BlockHeight);
                p.Image = newBitmap;
                p.Invalidate();
                Application.DoEvents();
            }
        }
        #endregion

        #region 迷宫寻路区

        /// <summary>
        /// 深度优先遍历寻找路径
        /// </summary>
        /// <param name="start">寻路起点</param>
        /// <param name="end">寻路重点</param>
        public void DfsFind(Point start, Point end)
        {
            PathList = new List<List<Point>>();
            FindScript = new List<ScriptPoint>();
            int[,] maze = CopyInts(MazeMap);
            int x = 1;// 初始点横坐标
            int y = 1;// 初始点纵坐标
            PathStack = new Stack<Point>();// 用栈来存储路径所通过点的坐标
            AllPathList = new List<Point>();
            PathStack.Push(start);// 将迷宫入口点的坐标压栈
            PathList.Add(new List<Point>(PathStack));
            FindScript.Add(new ScriptPoint(start, true));
            maze[x, y] = 2;
            do
            {
                bool flag = false;
                // 向右->下->左->上的顺序探测是否有通路
                if (1 == maze[x + 1, y])
                {
                    maze[x + 1, y] = 2;// 有通过的路径点 ，避免以后重复走
                    AllPathList.Add(new Point(x + 1, y));
                    PathStack.Push(new Point(x + 1, y));// 把该点保存
                    PathList.Add(new List<Point>(PathStack));
                    FindScript.Add(new ScriptPoint(new Point(x + 1, y), true));
                    flag = true;
                }
                // bottom
                else if (1 == maze[x, y + 1])
                {
                    maze[x, y + 1] = 2;
                    AllPathList.Add(new Point(x, y + 1));
                    PathStack.Push(new Point(x, y + 1));
                    PathList.Add(new List<Point>(PathStack));
                    FindScript.Add(new ScriptPoint(new Point(x, y + 1), true));
                    flag = true;
                }
                // left
                else if (1 == maze[x - 1, y])
                {
                    maze[x - 1, y] = 2;
                    AllPathList.Add(new Point(x - 1, y));
                    PathStack.Push(new Point(x - 1, y));
                    PathList.Add(new List<Point>(PathStack));
                    FindScript.Add(new ScriptPoint(new Point(x - 1, y), true));
                    flag = true;
                }
                // top
                else if (1 == maze[x, y - 1])
                {
                    maze[x, y - 1] = 2;
                    AllPathList.Add(new Point(x, y - 1));
                    PathStack.Push(new Point(x, y - 1));
                    PathList.Add(new List<Point>(PathStack));
                    FindScript.Add(new ScriptPoint(new Point(x, y - 1), true));
                    flag = true;
                }
                /*
                 如果四周都没有通路,  则将点从路径中删除，即弹出栈顶元素。
                 如果此时栈为空，则表明弹出的是入口点，即此迷宫无解。
                 否则后退一个点重新探测。
				 */
                if (!flag)
                {
                    Point popPoint = PathStack.Pop();
                    PathList.Add(new List<Point>(PathStack));
                    FindScript.Add(new ScriptPoint(popPoint, false));
                    if (0 == PathStack.Count)
                    {
                        Console.WriteLine(" 没有找到可以走出此迷宫的路径 ");
                        break;
                    }
                }
                // 读取栈顶元素并判断其是否是出口
                Point peek = PathStack.Peek();
                if (peek.X == end.X && peek.Y == end.Y)
                {
                    Console.WriteLine("已经找到出口");
                    break;
                }
                // 如果栈顶元素不是出口，则以该元素为起点继续进行探测
                start = peek;
                x = start.X;
                y = start.Y;
            } while (PathStack.Count > 0);
        }

        /// <summary>
        /// AStar启发式搜索寻找路径
        /// </summary>
        /// <param name="start">寻路起点</param>
        /// <param name="end">寻路重点</param>
        public void AStarFind(Point start, Point end)
        {
            PathStack = new Stack<Point>(AStar.GetPath(this, start, end));
            FindScript = AStar.AstarScript;
        }

        /// <summary>
        /// 广度优先搜索寻找路径
        /// </summary>
        /// <param name="start">寻路起点</param>
        /// <param name="end">寻路终点</param>
        public void BFSFind(Point start, Point end)
        {
            PathStack=new Stack<Point>();
            FindScript=new List<ScriptPoint>();
            List<LinkPoint> linkPoints = new List<LinkPoint>(), linkTemp = new List<LinkPoint>();
            linkTemp.Add(new LinkPoint(start));
            int[,] newMaze = CopyInts(MazeMap);
            while (linkTemp.Count > 0)
            {
                linkPoints.AddRange(linkTemp);
                linkTemp = new List<LinkPoint>();
                for (int i = 0; i < linkPoints.Count; i++)
                {
                    for (int j = 0; j < 4; j++)
                    {
                        if (newMaze[linkPoints[i].Value.X + d[j, 0], linkPoints[i].Value.Y + d[j, 1]] == UnBlock&& newMaze[linkPoints[i].Value.X + d[j, 0], linkPoints[i].Value.Y + d[j, 1]] != 2)
                        {
                            linkTemp.Add(new LinkPoint(new Point(linkPoints[i].Value.X + d[j, 0], linkPoints[i].Value.Y + d[j, 1]), linkPoints[i]));
                            FindScript.Add(new ScriptPoint(new Point(linkPoints[i].Value.X + d[j, 0], linkPoints[i].Value.Y + d[j, 1]), false));
                            newMaze[linkPoints[i].Value.X + d[j, 0], linkPoints[i].Value.Y + d[j, 1]] = 2;
                        }
                    }
                }
                LinkPoint lp = LinkPoint.Contains(linkTemp, new LinkPoint(end));
                if (lp != null)
                {
                    while (lp != null)
                    {
                        PathStack.Push(lp.Value);
                        FindScript.Add(new ScriptPoint(lp.Value, true));
                        lp = lp.Father;
                    }
                    return;
                }
            }
        }

        #endregion

        #region 辅助方法区域

        private int[,] CopyInts(int[,] ints)
        {
            int[,] newInts = new int[ints.GetUpperBound(0) + 1, ints.GetUpperBound(1) + 1];
            for (int i = 0; i <= ints.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= ints.GetUpperBound(1); j++)
                {
                    newInts[i, j] = ints[i, j];
                }
            }
            return newInts;
        }

        int Search(int x, int y, int[,] Map)
        {
            int zx = x * 2, zy = y * 2;
            Map[zx, zy] = UnBlock; //int turn = _r.Next(0, 2) == 1 ? 1 : 3;
            for (int i = 0, next = _r.Next(0, 4); i < 4; i++, next = (next + 1) % 4)
                if ((zx + 2 * d[next, 0] >= 0 && zx + 2 * d[next, 0] <= Map.GetUpperBound(0)) && (zy + 2 * d[next, 1] >= 0 && zy + 2 * d[next, 1] <= Map.GetUpperBound(1)) && (Map[zx + 2 * d[next, 0], zy + 2 * d[next, 1]] == Block))
                { //如果当前方向的拓展之后存在墙壁 则标记当前方向为路（就是拓展之后不破壁）
                    Map[zx + d[next, 0], zy + d[next, 1]] = UnBlock;
                    Search(x + d[next, 0], y + d[next, 1], Map);
                }
            return 0;
        }

        private void PrintMaze(int[,] mazeInts, string mazeName = "")
        {
            Console.WriteLine("------------------" + mazeName + "-----------------");
            for (int i = 0; i <= mazeInts.GetUpperBound(0); i++)
            {
                for (int j = 0; j <= mazeInts.GetUpperBound(1); j++)
                {
                    Console.Write(mazeInts[i, j] + " ");
                }
                Console.Write("\r\n");
            }
        }

        #endregion

    }

}