using System;
using System.Collections.Generic;
using System.Text;

namespace Snek
{
    interface IMonitor
    {
        void SetMapView(IMapView map);

        void Refresh();

        void DrawGameOver();

        int Width { get; }

        int Height { get; }
    }

    class ConsoleMonitor : IMonitor
    {
        public int Width { get; }

        public int Height { get; }

        public ConsoleMonitor(int width, int height)
        {
            Width = width;
            Height = height;

            Console.CursorVisible = false;
            DrawBorder();
        }

        IMap lastMap;
        IMapView currMapView;

        public void DrawGameOver()
        {
            string msg = "GameOver";

            Console.SetCursorPosition(
                Math.Max(0,(Width - msg.Length) / 2),
                (Height + 1) / 2);
            Console.Write(msg);
        }

        public void Refresh()
        {
            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (currMapView[x, y] != lastMap[x, y])
                        RefreshCell(x, y);

            SaveMapView();
        }

        private void DrawBorder()
        {
            Console.Clear();

            StringBuilder firstLine = new StringBuilder("--");
            StringBuilder innerLine = new StringBuilder("|");

            for (int i = 0; i < Width; i++)
            {
                firstLine.Append("-");
                innerLine.Append(" ");
            }
            innerLine.Append("|");

            Console.WriteLine(firstLine);

            for (int i = 0; i < Height; i++)
                Console.WriteLine(innerLine);

            Console.WriteLine(firstLine);
        }

        private void RefreshCell(int x, int y)
        {
            Console.SetCursorPosition(x + 1, y + 1);

            switch (currMapView[x,y])
            {
                case MapItem.none:
                    Console.Write(" ");
                    break;
                case MapItem.snek:
                    Console.Write("#");
                    break;
                case MapItem.food:
                    Console.Write("O");
                    break;
                default:
                    break;
            }

            Console.SetCursorPosition(0, Height + 2);
        }

        public void SetMapView(IMapView map)
        {
            currMapView = map;
            lastMap = new Map(currMapView.Width, currMapView.Height);
        }

        private void SaveMapView()
        {
            for (int i = 0; i < currMapView.Width; i++)
                for (int j = 0; j < currMapView.Height; j++)
                    lastMap[i, j] = currMapView[i, j];
        }
    }
}
