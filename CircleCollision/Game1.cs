using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CircleCollision
{
    public class Game1 : Game
    {
        #region Variable Defenition

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        List<Circle> Circles = new List<Circle>();
        Circle SelectedCircle;

        Grid Grid;

        List<Keys> Keys_BeingPressed = new List<Keys>();
        bool Mouse_isClickingLeft;
        bool Mouse_isClickingRight;
        bool Mouse_isClickingMiddle;

        Texture2D Color_White;
        Texture2D Texture_Circle;

        #endregion

        #region Initialize

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1800;
            _graphics.PreferredBackBufferHeight = 1000;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            Circles.Add(new Circle()
            {
                X = 500,
                Y = 500,

                Radius = 50,
                Diameter = 100,

                MovementAngle = 0,
                MovementVelocity = 0,
                VelocityDecayMultiplier = 0.98F,

                Color = Color.Black,
                Selected = false,
                isColliding = false
            });
            Circles.Add(new Circle()
            {
                X = 1000,
                Y = 800,

                Radius = 50,
                Diameter = 100,

                MovementAngle = 0,
                MovementVelocity = 0,
                VelocityDecayMultiplier = 0.98F,

                Color = Color.Black,
                Selected = false,
                isColliding = false
            });
            SelectedCircle = Circles[0];
            Circles[0].Selected = true;


            Grid = new Grid();
            Grid.GenerateSlots(false);


            Mouse_isClickingLeft = false;
            Mouse_isClickingRight = false;
            Mouse_isClickingMiddle = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Texture_Circle = Content.Load<Texture2D>("WhiteCircle");
            Color_White = Content.Load<Texture2D>("Color_White");
        }

        #endregion

        /////////////////////////////////////////

        #region CircleMovement

        void CentreCircles()
        {
            if (Circles.Count > 0)
            {
                for (int i = 0; i < Circles.Count; i++)
                {
                    Circles[i].X = 100 + (i * 110);
                    Circles[i].Y = 500;
                }
            }
        }
        void EnactCircleVelocity()
        {
            foreach (Circle Circle in Circles)
            {
                Circle.EnactVelocity();
            }
        }

        #endregion

        
        void CheckCircleCollisions()
        {
            List<Circle> areColliding = new List<Circle>();
            List<(Circle, Circle)> CollidingPairs = new List<(Circle, Circle)>();

            foreach (Circle Circle in Circles)
            {
                foreach (Circle OtherCircle in Circles)
                {
                    if (Circle != OtherCircle)
                    {
                        // Collision Detected
                        if (Circle.isCollidingWith(OtherCircle))
                        {
                            if (!areColliding.Contains(Circle))
                            {
                                areColliding.Add(Circle);
                            }
                            if (!areColliding.Contains(OtherCircle))
                            {
                                areColliding.Add(OtherCircle);
                            }

                            //Creating Pairs
                            if (!CollidingPairs.Contains((OtherCircle, Circle)))
                            {
                                CollidingPairs.Add((Circle, OtherCircle));
                            }
                        }
                    }
                }
            }

            //Updating Circle Collision Attributes
            foreach (Circle Circle in Circles)
            {
                if (areColliding.Contains(Circle))
                {
                    Circle.isColliding = true;
                }
                else
                {
                    Circle.isColliding = false;
                }
            }


            foreach ((Circle, Circle) Pair in CollidingPairs)
            {
                Pair.Item1.EnactCollision(Pair.Item2);
            }
        }


        /////////////////////////////////////////

        #region UserInput

        void Mouse_ClickHandler()
        {
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                //Move Circle
                if (SelectedCircle != null)
                {
                    SelectedCircle.X = Mouse.GetState().X;
                    SelectedCircle.Y = Mouse.GetState().Y;
                }

                Mouse_isClickingLeft = true;
            }
            else
            {
                Mouse_isClickingLeft = false;
            }
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                if (!Mouse_isClickingRight)
                {
                    //Create new circle
                    Circles.Add(new Circle()
                    {
                        X = Mouse.GetState().X,
                        Y = Mouse.GetState().Y,

                        Radius = 50,
                        Diameter = 100,

                        MovementAngle = 0,
                        MovementVelocity = 0,
                        VelocityDecayMultiplier = 0.98F,

                        Color = Color.Black,
                        Selected = false,
                        isColliding = false
                    });
                }

                Mouse_isClickingRight = true;
            }
            else
            {
                Mouse_isClickingRight = false;
            }

            if (Mouse.GetState().MiddleButton == ButtonState.Pressed)
            {
                Mouse_isClickingMiddle = true;
            }
            else
            {
                Mouse_isClickingMiddle = false;
            }
        }
        void KeybindHandler()
        {
            List<Keys> Keys_NewlyPressed = Keyboard.GetState().GetPressedKeys().ToList();



            //Reset Circles
            if (Keys_NewlyPressed.Contains(Keys.R))
            {
                CentreCircles();
            }
            //Clear Circles
            if (Keys_NewlyPressed.Contains(Keys.C))
            {
                Circles.Clear();
                SelectedCircle = null;
            }
            //Tab through circles
            if (Keys_NewlyPressed.Contains(Keys.Tab) && !Keys_BeingPressed.Contains(Keys.Tab))
            {
                ChangeSelectedCircle();
            }

            //Circle UserMovement
            if (SelectedCircle != null)
            {
                //Rotate Circle
                if (Keys_NewlyPressed.Contains(Keys.Left))
                {
                    SelectedCircle.ChangeAngle(-1f);
                }
                if (Keys_NewlyPressed.Contains(Keys.Right))
                {
                    SelectedCircle.ChangeAngle(1f);
                }
                //Change Speed of Circle
                if (Keys_NewlyPressed.Contains(Keys.Up))
                {
                    SelectedCircle.ChangeSpeed(1f, false);
                }
                if (Keys_NewlyPressed.Contains(Keys.Down))
                {
                    SelectedCircle.ChangeSpeed(-1f, false);
                }
            }


            if (Keys_NewlyPressed.Contains(Keys.F) && !Keys_BeingPressed.Contains(Keys.F))
            {
                _graphics.ToggleFullScreen();
            }


            Keys_BeingPressed = Keys_NewlyPressed;
        }
        void ChangeSelectedCircle()
        {
            if (Circles.Count > 0)
            {
                if (SelectedCircle != null)
                {
                    SelectedCircle.Selected = false;

                    if (Circles.IndexOf(SelectedCircle) + 1 == Circles.Count)
                    {
                        SelectedCircle = null;
                    }
                    else
                    {
                        SelectedCircle = Circles[Circles.IndexOf(SelectedCircle) + 1];
                        SelectedCircle.Selected = true;
                    }
                }
                else
                {
                    SelectedCircle = Circles[0];
                    SelectedCircle.Selected = true;
                }
            }
        }

        #endregion

        #region Fundamentals

        void DrawLine(Vector2 point, float Length, float Angle, Color Color, float Thickness = 1F)
        {
            var origin = new Vector2(0f, 0.5f);
            var scale = new Vector2(Length, Thickness);

            _spriteBatch.Draw(Color_White, point, null, Color, Angle, origin, scale, SpriteEffects.None, 0);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            KeybindHandler();
            Mouse_ClickHandler();

            CheckCircleCollisions();
            EnactCircleVelocity();


            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();



            //Grid
            for (int y = 0; y < Grid.Slots.Count; y++)
            {
                for (int x = 0; x < Grid.Slots[y].Count; x++)
                {
                    if (Grid.Slots[y][x] != null)
                    {
                        _spriteBatch.Draw(Color_White, new Rectangle(Grid.X + (x * Grid.SlotWidth),
                                                                     Grid.Y + (y * Grid.SlotHeight), Grid.SlotWidth, Grid.SlotHeight), Color.Black);
                    }
                }
            }

            //Circles
            foreach (Circle Circle in Circles)
            {
                int BorderWidth = 2;
                Color BaseColor = Circle.Color;
                if (Circle.Selected)
                {
                    BaseColor = Color.Gold;

                    if (Circle.isColliding)
                    {
                        BaseColor = Color.Goldenrod;
                    }
                }
                else if (Circle.isColliding)
                {
                    BaseColor = Color.Red;
                }

                _spriteBatch.Draw(Texture_Circle, new Rectangle((int)Circle.X - (int)Circle.Radius, (int)Circle.Y - (int)Circle.Radius,
                                                                (int)Circle.Diameter, (int)Circle.Diameter), Circle.Color);
                _spriteBatch.Draw(Texture_Circle, new Rectangle((int)Circle.X - ((int)Circle.Radius - BorderWidth), (int)Circle.Y - ((int)Circle.Radius - BorderWidth),
                                                                (int)Circle.Diameter - BorderWidth * 2, (int)Circle.Diameter - BorderWidth * 2), BaseColor);

                //CircleDirection
                DrawLine(new Vector2(Circle.X, Circle.Y), Circle.Diameter, Circle.MovementAngle * (float)(Math.PI / 180), Color.Red);
            }



            _spriteBatch.End();

            base.Draw(gameTime);
        }

        #endregion
    }
}