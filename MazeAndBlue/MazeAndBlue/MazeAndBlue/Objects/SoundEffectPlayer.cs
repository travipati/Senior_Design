﻿using Microsoft.Xna.Framework.Audio;

namespace MazeAndBlue
{
    public class SoundEffectPlayer
    {
        enum SoundType{ BUTTON, WALL, DOOR, GOAL };
        const int numSoundTypes = 4;

        SoundEffect[] sounds;

        public bool soundsOn { get; set; }

        public SoundEffectPlayer()
        {
            soundsOn = true;
            sounds = new SoundEffect[numSoundTypes];
            sounds[(int)SoundType.BUTTON] = Program.game.Content.Load<SoundEffect>("Sounds/button");
            sounds[(int)SoundType.WALL] = Program.game.Content.Load<SoundEffect>("Sounds/wall");
            sounds[(int)SoundType.DOOR] = Program.game.Content.Load<SoundEffect>("Sounds/door");
            sounds[(int)SoundType.GOAL] = Program.game.Content.Load<SoundEffect>("Sounds/goal");
        }

        private void playSound(int sound)
        {
            if(soundsOn)
                sounds[sound].Play();
        }

        public void playButton()
        {
            playSound((int)SoundType.BUTTON);
        }

        public void playWall()
        {
            playSound((int)SoundType.WALL);
        }

        public void playDoor()
        {
            playSound((int)SoundType.DOOR);
        }

        public void playGoal()
        {
            playSound((int)SoundType.GOAL);
        }

    }
}
