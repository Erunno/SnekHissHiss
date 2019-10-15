using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Snek
{
    struct Cell
    {
        public Cell(int x, int y)
        {
            this.X = x;
            this.Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }

    class Snek
    {
        public Snek(IMap map)
        {
            this.map = map;
            cells = new Queue<Cell>();

            Head = new Cell(2, 2);
            cells.Enqueue(Head);

            map.EmplaceNewFood();
        }

        private Queue<Cell> cells;

        IMap map;

        public Cell Head { get; private set; }

        public bool CellIsValid(Cell cell)
         => IsInRange(cell) &&
            !(from c in cells where c.X == cell.X && c.Y == cell.Y select c).Any();
        
        private bool IsInRange(Cell cell)
            =>  0 <= cell.X && cell.X < map.Width &&
                0 <= cell.Y && cell.Y < map.Height;

        public void MoveToCell(Cell cell)
        {
            if (!IsFoodOn(cell))
            {
                RemoveTail();
                AddCell(cell);
            }
            else
            {
                AddCell(cell);
                map.EmplaceNewFood();
            }

        }

        private void AddCell(Cell cell)
        {
            cells.Enqueue(cell);
            map[cell.X, cell.Y] = MapItem.snek;
            Head = cell;
        }

        private bool IsFoodOn(Cell cell) => map[cell.X, cell.Y] == MapItem.food;

        private void RemoveTail()
        {
            Cell cell = cells.Dequeue();
            map[cell.X, cell.Y] = MapItem.none;
        }
    }
}
