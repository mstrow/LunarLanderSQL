using System;
using System.Collections.Generic;
using System.Text;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System.Numerics;
using System.Diagnostics;

namespace LanderGame
{
    public class Game : GameLoop
    {
        public const int WIDTH = 640;
        public const int HEIGHT = 480;
        private LoginSession _session;
        private string status = "OK";
        public Game(LoginSession session) : base(WIDTH,HEIGHT,"Game",Color.Black)
        {
            _session = session;
        }
        public override void Initialize()
        {

        }

        public override void LoadContent()
        {
            GameDebug.LoadContent(System.Environment.GetFolderPath(Environment.SpecialFolder.Fonts) + @"\times.ttf");
            
        }

        public override void Render(GameTime gameTime)
        {
            
            Vector2f shipPos = new Vector2f(_session.Match.PosX, _session.Match.PosY);
            Vector2f shipOffset = new Vector2f(WIDTH / 2, HEIGHT / 2);
            float scale = 5;
            /*Vector2i mouse = Mouse.GetPosition(this.Window);
            RectangleShape shape = new RectangleShape(new Vector2f(40,40));
            shape.Position = new Vector2f(mouse.X,mouse.Y);
            this.Window.Draw(shape);*/


            GameDebug.PerformanceMetrics(this, Color.White);

            foreach (Tile tile in _session.Match.Map.Tiles)
            {
                if (Math.Floor(shipPos.X) == tile.X)
                {
                    float distance = Math.Abs(shipPos.Y - tile.Y);
                    if (distance >= 20)
                    {
                        scale = 5;
                    }else if(distance >= 10)
                    {
                        scale = 10;
                    }
                    else
                    {
                        scale = 15;
                    }
                }
            }

            foreach (Tile tile in _session.Match.Map.Tiles)
            {
                Vector2f tileOffset = new Vector2f(100,  100);
                RectangleShape tileShape = new RectangleShape(new Vector2f(scale, scale));
                tileShape.Position = new Vector2f((tile.X - shipPos.X) * scale + shipOffset.X, -((tile.Y - shipPos.Y) * scale + shipOffset.Y) +HEIGHT);
                this.Window.Draw(tileShape);
            }
            RectangleShape ship = new RectangleShape(new Vector2f(scale*2,scale*3));
            Texture tex;
            if (_session.Match.Thruster)
            {
                tex = new Texture("ship2.png");
            }
            else
            {
                tex = new Texture("ship.png");
            }
            //ship.Position = new Vector2f(shipPos.X * 5, -(shipPos.Y * 5) + HEIGHT);
            ship.Origin = new Vector2f(scale,scale*1.5f);
            ship.Position = new Vector2f(shipOffset.X, shipOffset.Y);
            ship.Rotation = -_session.Match.Rotation;
            ship.Texture = tex;
            this.Window.Draw(ship);
            GameDebug.DrawTxt(this, Color.White, status);
        }

        public override void Update(GameTime gameTime)
        {
            
            if (status == "OK")
            {
                status = _session.UpdateShip();
                if (Keyboard.IsKeyPressed(Keyboard.Key.Left))
                {
                    _session.ControlRocket(gameTime.DeltaTime*50, false);
                }

                if (Keyboard.IsKeyPressed(Keyboard.Key.Right))
                {
                    _session.ControlRocket(-(gameTime.DeltaTime * 50), false);
                }
                if (Keyboard.IsKeyPressed(Keyboard.Key.Up))
                {
                    _session.ControlRocket(0, true);
                }
                else
                {
                    _session.ControlRocket(0, false);
                }
            }
            else
            {
                if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
                {
                    Window.Close();
                    _session.EndMatch();
                }
            }
            
        }
    }
}

