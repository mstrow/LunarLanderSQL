using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;


namespace LanderGame
{
    public static class GameDebug
    {
        public static Font consoleFont;
        
        public static void LoadContent(string FONTPATH)
        {
            consoleFont = new Font(FONTPATH);
        }

        public static void PerformanceMetrics(GameLoop gameLoop, Color fontColor)
        {
            string totalTimeElapsed = gameLoop.GameTime.TotalTimeElapsed.ToString("0.000");
            string deltaTime = gameLoop.GameTime.DeltaTime.ToString("0.000");
            float fps = 1f / gameLoop.GameTime.DeltaTime;
            string  fpsString = fps.ToString("0.000");

            Text tex = new Text(totalTimeElapsed, consoleFont, 14);
            tex.Position = new Vector2f(4f, 8f);
            tex.FillColor = fontColor;

            Text tex2 = new Text(deltaTime, consoleFont, 14);
            tex2.Position = new Vector2f(4f, 28f);
            tex2.FillColor = fontColor;

            Text tex3 = new Text(fpsString, consoleFont, 14);
            tex3.Position = new Vector2f(4f, 48f);
            tex3.FillColor = fontColor;

            gameLoop.Window.Draw(tex);
            gameLoop.Window.Draw(tex2);
            gameLoop.Window.Draw(tex3);
        }

        public static void DrawTxt(GameLoop gameLoop, Color fill, string text)
        {
            Text tex = new Text(text, consoleFont, 14);
            tex.Position = new Vector2f(4f, 68f);
            tex.FillColor = fill;
            gameLoop.Window.Draw(tex);
        }
    }
}
