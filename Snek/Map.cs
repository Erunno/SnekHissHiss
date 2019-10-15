using System;
using System.Collections.Generic;
using System.Text;

namespace Snek
{
    public interface IMap
    {
        MapItem this[int x, int y] { get; set; }

        void EmplaceNewFood();

        int Height { get; }
        int Width { get; }
    }


    public interface IMapView
    {
        MapItem this[int x, int y] { get; }

        int Height { get; }
        int Width { get; }
    }

    public enum MapItem { none, snek, food }

    class Map : IMap, IMapView
    {
        public Map(int width, int height)
        {
            this.Height = height;
            this.Width = width;

            map = new MapItem[width, height];
        }

        private MapItem[,] map;

        public MapItem this[int x, int y] {
            get => map[x, y];
            set => map[x, y] = value;
        }

        public int Height { get; }

        public int Width { get; }

        private Random rnd = new Random();

        public void EmplaceNewFood()
        {
            int numOfFreeCells = GetNumOfFreeCells();

            if (numOfFreeCells == 0)
                return;

            int rndIndex = rnd.Next(0, numOfFreeCells);

            SetNewFoodOnIndex(rndIndex);
        }

        private int GetNumOfFreeCells()
        {
            int freeCells = 0;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    freeCells += map[x, y] == MapItem.none ? 1 : 0;

            return freeCells;
        }

        private void SetNewFoodOnIndex(int index)
        {
            int currIndex = 0;

            for (int x = 0; x < Width; x++)
                for (int y = 0; y < Height; y++)
                    if (map[x, y] == MapItem.none)
                        if (currIndex == index)
                        {
                            map[x, y] = MapItem.food;
                            return;
                        }
                        else
                            currIndex++;
        }
    }
}
