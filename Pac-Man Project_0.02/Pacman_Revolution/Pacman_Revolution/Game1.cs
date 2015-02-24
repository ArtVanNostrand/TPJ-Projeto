#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
#endregion

namespace Pacman_Revolution
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D pacman, block;
        byte[,] board = new byte[20, 20];
        byte[,] pacmanloc = new byte[20, 20];
        int pX = 10, pY = 10;
        float lastHumanMove = 0f;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 600;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            pacman = Content.Load<Texture2D>("pacman30");
            block = Content.Load<Texture2D>("placeholder block");

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here


            lastHumanMove += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (lastHumanMove >= 1f / 10f)
            {
                int flag = 0;

                lastHumanMove = 0f;



                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    flag++;
                    if (canGoUp() == true)
                    {
                        if (flag == 1)
                        {
                            board[pX, pY] = 0;
                            pY--;
                            board[pX, pY] = 1;
                            pacman = Content.Load<Texture2D>("pacman30up");
                        }
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    flag++;
                    if (canGoDown() == true)
                    {
                        if (flag == 1)
                        {
                            board[pX, pY] = 0;
                            pY++;
                            board[pX, pY] = 1;

                            pacman = Content.Load<Texture2D>("pacman30down");
                        }
                    }
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    flag++;
                    if (canGoRight() == true)
                    {
                        if (flag == 1)
                        {
                            board[pX, pY] = 0;
                            pX++;
                            board[pX, pY] = 1;

                            pacman = Content.Load<Texture2D>("pacman30right");
                        }
                    }
                
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    if (canGoLeft() == true)
                    {
                        flag++;
                        if (flag == 1)
                        {
                            board[pX, pY] = 0;
                            pX--;
                            board[pX, pY] = 1;

                            pacman = Content.Load<Texture2D>("pacman30");
                        }
                    }
                }
            }



            base.Update(gameTime);
        }




        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            spriteBatch.Draw(block, new Vector2(250, 200));
            spriteBatch.Draw(block, new Vector2(250, 210));
            spriteBatch.Draw(block, new Vector2(250, 220));
            spriteBatch.Draw(block, new Vector2(250, 230));
            spriteBatch.Draw(block, new Vector2(250, 240));


            //spriteBatch.Draw(pacman, new Vector2(300, 300), Color.Yellow);

            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    if (board[x, y] != 0)
                    {
                        spriteBatch.Draw(pacman, new Vector2(x * 30, (y - 2) * 30));
                    }
                    
                }
            }


            spriteBatch.End();
            base.Draw(gameTime);
        }



        private bool canGoUp()
        {

            if (pY == 2)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool canGoDown()
        {

            if (pY >= 19)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool canGoLeft()
        {

            if (pX == 1)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool canGoRight()
        {

            if (pX == 19)
            {
                return false;
            }
            else
            {
                return true;
            }

        }


       






    }
}
