using System;
using System.Collections.Generic;
using System.Text;

namespace LanderGame
{
    public class GameTime
    {
        private float _deltaTime = 0;
        private float _timeScale = 1;

        public float TimeScale { get => _timeScale; set => _timeScale = value; }
        public float DeltaTime { 
            get{ 
                return _deltaTime*_timeScale; 
            }
            set => _deltaTime = value; 
        }
        public float DeltaTimeUnscaled {
            get => _deltaTime;
        }
        public float TotalTimeElapsed
        {
            get;
            private set;
        }
        public GameTime()
        {

        }

        public void Update(float deltaTime, float totalTimeElapsed)
        {
            _deltaTime = deltaTime;
            TotalTimeElapsed = totalTimeElapsed;
        }
    }
}
