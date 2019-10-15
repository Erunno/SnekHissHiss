using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Snek
{
    public enum Direction { none, up, down, left, right }

    class GameManager
    {
        public GameManager(IMonitor monitor)
        {
            map = new Map(monitor.Width, monitor.Height);
            snek = new Snek(map);

            this.monitor = monitor;
            monitor.SetMapView((IMapView)map);
        }

        private IMap map;

        Direction lastDirection = Direction.down;
        Snek snek;
        IMonitor monitor;

        public bool SnekIsAlive { get; private set; } = true;

        public void RunGame()
        {
            ButtonProvider buttonProvider = new ButtonProvider();

            while(SnekIsAlive)
            {
                Update(buttonProvider.GetAndRestartLastDirection());
                Thread.Sleep(200);
            }

            buttonProvider.KillThread();
        }

        class ButtonProvider
        {
            public ButtonProvider()
            {
                Thread thread = new Thread(RunReading);

                thread.Start();
            }

            private bool killThread = false;

            public void KillThread() => killThread = true;

            private ConsoleKey? lastClickedKey;

            private void RunReading()
            {
                while (!killThread)
                   lastClickedKey = Console.ReadKey().Key;
            }

            public Direction GetAndRestartLastDirection()
            {
                Direction dir = GetLastDirection();
                lastClickedKey = null;

                return dir;
            }

            private Direction GetLastDirection()
            {
                switch (lastClickedKey)
                {
                    case ConsoleKey.UpArrow:
                        return Direction.up;

                    case ConsoleKey.DownArrow:
                        return Direction.down;

                    case ConsoleKey.RightArrow:
                        return Direction.right;

                    case ConsoleKey.LeftArrow:
                        return Direction.left;

                    default:
                        return Direction.none;
                }
            }
        }

        private void Update(Direction direction)
        {
            if (!SnekIsAlive)
                throw new SnekIsDeadException();

            if (direction == Direction.none)
                direction = lastDirection;

            SnekIsAlive = MoveSnek(direction) == SnekMovementResult.live;

            if (SnekIsAlive)
                monitor.Refresh();
            else
                monitor.DrawGameOver();

            lastDirection = direction;
        }


        enum SnekMovementResult { live, dead }

        private SnekMovementResult MoveSnek(Direction direction)
        {
            Cell nextCell = GetNextCell(direction);

            if (snek.CellIsValid(nextCell))
            {
                snek.MoveToCell(nextCell);
                return SnekMovementResult.live;
            }
            else
                return SnekMovementResult.dead;
        }

        private Cell GetNextCell(Direction direction)
        {
            Cell head = snek.Head;

            switch (direction)
            {
                case Direction.up:
                    return new Cell(head.X, head.Y - 1);
                case Direction.down:
                    return new Cell(head.X, head.Y + 1);
                case Direction.left:
                    return new Cell(head.X - 1, head.Y);
                case Direction.right:
                    return new Cell(head.X + 1, head.Y);
                default:
                    throw new NotImplementedException();
            }
        }
    }

    class SnekIsDeadException : Exception { }
}
