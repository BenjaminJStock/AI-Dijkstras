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
    class AiBotSimple : AiBotBase
    {
        public AiBotSimple(int x, int y) : base(x, y)
        {
        }
        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            Coord2 playerPos = plr.GridPosition;
            Coord2 pos = GridPosition;
            bool ok = true;

            if (playerPos.X > pos.X)
            {
                pos.X++;
                ok = SetNextGridPosition(pos, level);
                if (ok != false)
                    return;
                pos.X--;
            }

            if (playerPos.X < pos.X)
            {
                pos.X--;
                ok = SetNextGridPosition(pos, level);
                if (ok != false)
                    return;
                pos.X++;
            }

            if (playerPos.Y > pos.Y)
            {
                pos.Y++;
                ok = SetNextGridPosition(pos, level);
                if (ok != false)
                    return;
                pos.Y--;
            }

            if (playerPos.Y < pos.Y)
            {
                pos.Y--;
                ok = SetNextGridPosition(pos, level);
                if (ok != false)
                    return;
                pos.Y++;
            }
        }
    }
}