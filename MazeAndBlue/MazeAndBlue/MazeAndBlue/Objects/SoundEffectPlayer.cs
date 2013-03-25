using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace MazeAndBlue
{
    public class SoundEffectPlayer
    {
        enum SoundType{ BUTTON, WALL, DOOR, GOAL };
        const int numSoundTypes = 4;

        SoundEffect[] sounds;

        public SoundEffectPlayer()
        {
            sounds = new SoundEffect[numSoundTypes];
            sounds[(int)SoundType.BUTTON] = Program.game.Content.Load<SoundEffect>("Sounds\\button");
            sounds[(int)SoundType.WALL] = Program.game.Content.Load<SoundEffect>("Sounds\\wall");
            sounds[(int)SoundType.DOOR] = Program.game.Content.Load<SoundEffect>("Sounds\\door");
            sounds[(int)SoundType.GOAL] = Program.game.Content.Load<SoundEffect>("Sounds\\goal");
        }

        public void playButton()
        {
            sounds[(int)SoundType.BUTTON].Play();
        }

        public void playWall()
        {
            sounds[(int)SoundType.WALL].Play();
        }

        public void playDoor()
        {
            sounds[(int)SoundType.DOOR].Play();
        }

        public void playGoal()
        {
            sounds[(int)SoundType.GOAL].Play();
        }

    }
}
