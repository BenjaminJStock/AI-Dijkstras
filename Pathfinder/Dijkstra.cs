using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;
using System.IO;
namespace Pathfinder
{
    class Dijkstra
    {
        public bool[,] closed; //whether or not location is closed
        public float[,] cost; //cost value for each location
        public Coord2[,] link; //link for each location = coords of a neighbouring location
        public bool[,] inPath; //whether or not a location is in the final path
        public List<Coord2> path;        public Coord2 playerPos;        public Dijkstra(Level level)
        {
            closed = new bool[level.GridSize, level.GridSize];
            cost = new float[level.GridSize, level.GridSize];
            link = new Coord2[level.GridSize, level.GridSize];
            inPath = new bool[level.GridSize, level.GridSize];
            path = new List<Coord2>();
            
        }        public void Build(Level level, AiBotBase bot, Player plr)
        {
            for (int i = 0; i < level.GridSize; i++)
                        {
                            for (int j = 0; j < level.GridSize; j++)
                            {
                                cost[i, j] = int.MaxValue - 1;
                                closed[i, j] = false;
                                link[i, j] = new Coord2(-1, -1);
                                inPath[i, j] = false;
                            } 
                        }
            cost[bot.GridPosition.X, bot.GridPosition.Y] = 0;
            Coord2 currentPosition = bot.GridPosition;
            closed[bot.GridPosition.X, bot.GridPosition.Y] = true;
            while (!closed[plr.GridPosition.X, plr.GridPosition.Y])
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        if (level.ValidPosition(new Coord2(currentPosition.X + i, currentPosition.Y + j)) && !closed[currentPosition.X + i, currentPosition.Y + j]
                            && (i != 0 || j != 0) && (currentPosition.X + i < level.GridSize && currentPosition.Y + j < level.GridSize && currentPosition.X + i >= 0 && currentPosition.Y + j >= 0)
                            && (level.ValidPosition(new Coord2(currentPosition.X + i, currentPosition.Y)) && level.ValidPosition(new Coord2(currentPosition.X, currentPosition.Y + j))))
                        {
                            if (i == 0 || j == 0)
                                SetCost(currentPosition, level, i, j, false);
                            else
                                SetCost(currentPosition, level, i, j, true);
                        }
                    }
                }

                float lowestCost = int.MaxValue;
                Coord2 nextPosition = currentPosition;

                // Iterate through the list to check open cells for valid movements.
                for (int i = 0; i < level.GridSize; i++)
                {
                    for (int j = 0; j < level.GridSize; j++)
                    {
                        if (!closed[i, j] && level.ValidPosition(new Coord2(i, j)) && cost[i, j] < lowestCost)
                        {
                            lowestCost = cost[i, j];
                            nextPosition = new Coord2(i, j);
                        }
                    }
                }

                if (currentPosition == nextPosition)
                    break;
                closed[nextPosition.X, nextPosition.Y] = true;
                currentPosition = nextPosition;
            }

            bool done = false;
            Coord2 nextClose = plr.GridPosition;
            while (!done)
            {
                if (nextClose.X == -1)
                    break;
                if (nextClose == bot.GridPosition)
                {
                    playerPos = plr.GridPosition;
                    done = true;
                }
                inPath[nextClose.X, nextClose.Y] = true;
                path.Add(new Coord2(nextClose.X, nextClose.Y));
                nextClose = link[nextClose.X, nextClose.Y];

                }
            }
        // Set cost of cell (parent cost + movement cost).
        private void SetCost(Coord2 currentPosition, Level level, int x, int y, bool diag)
        {

            int moveCost;
            if (!diag)
                moveCost = 10;
            else
                moveCost = 14;

            if (cost[currentPosition.X + x, currentPosition.Y + y] > cost[currentPosition.X, currentPosition.Y] + moveCost)
            {
                cost[currentPosition.X + x, currentPosition.Y + y] = cost[currentPosition.X, currentPosition.Y] + moveCost;
                link[currentPosition.X + x, currentPosition.Y + y] = currentPosition;
            }
        }
    }
}  