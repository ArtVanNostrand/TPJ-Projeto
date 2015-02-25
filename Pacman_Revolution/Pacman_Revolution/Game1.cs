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
        Texture2D pacman, block, pellet, black, ghost;
        //board = 0 -> nada
        //board = 1 -> pacman
        //board = 2 -> bloco
        //board = 3 -> pellet
        //board = 4 -> ghost
        byte[,] board = new byte[20, 20];
        byte[,] ghostboard = new byte[20, 20];
        int pX = 10, pY = 10, gpX=2, gpY=2;
        int pelletcont = 0;
        float lastHumanMove = 0f;
        float lastGhostMove = 0f;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = 540;
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

            ghostboard[2, 2] = 1;

            board[5, 2] = 3;
            board[5, 3] = 3;
            board[5, 4] = 3;
            board[5, 5] = 3;
            board[5, 6] = 3;
            board[5, 7] = 3;

            board[15, 2] = 3;
            board[15, 3] = 3;
            board[15, 4] = 3;
            board[15, 5] = 3;
            board[15, 6] = 3;
            board[15, 7] = 3;
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
            pellet = Content.Load<Texture2D>("pellet15");
            black = Content.Load<Texture2D>("black");
            ghost = Content.Load<Texture2D>("placeholderghost30");

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
            lastGhostMove += (float)gameTime.ElapsedGameTime.TotalSeconds;


            if (lastGhostMove >= 1f / 3f)
            {
                lastGhostMove = 0f;
                if (gpX < 19)
                {
                    ghostboard[gpX, gpY] = 0;
                    gpX++;
                    ghostboard[gpX, gpY] = 1;
                }
            }

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
                            if (board[pX, pY] == 3)
                            {
                                pelletcont++;
                            }
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
                            if (board[pX, pY] == 3)
                            {
                                pelletcont++;
                            }
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
                            if (board[pX, pY] == 3)
                            {
                                pelletcont++;
                            }
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
                            if (board[pX, pY] == 3)
                            {
                                pelletcont++;
                            }
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

            board[1, 10] = 2;
            board[2, 10] = 2;
            board[3, 10] = 2;
            board[4, 10] = 2;
            board[5, 10] = 2;

            board[12, 10] = 2;
            board[13, 10] = 2;
            board[14, 10] = 2;
            board[15, 10] = 2;

            board[16, 10] = 2;
            board[17, 10] = 2;
            board[18, 10] = 2;
            board[19, 10] = 2;

            board[18, 19] = 2;
            board[19, 19] = 2;           
     


            //spriteBatch.Draw(pacman, new Vector2(300, 300), Color.Yellow);

            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {

                    if (board[x, y] == 0)
                    {
                        spriteBatch.Draw(black, new Vector2(x * 30, (y - 2) * 30));
                    }
                    if (board[x, y] == 1)
                    {
                        spriteBatch.Draw(pacman, new Vector2(x * 30, (y - 2) * 30));
                    }
                    if (board[x, y] == 2)
                    {
                        spriteBatch.Draw(block, new Vector2(x * 30, (y - 2) * 30));
                    }
                    if (board[x, y] == 3)
                    {
                        spriteBatch.Draw(pellet, new Vector2(x * 30, (y - 2) * 30));
                    }
                    //if (board[x, y] == 4)
                    //{
                    //    spriteBatch.Draw(ghost, new Vector2(x * 30, (y - 2) * 30));
                    //}
                    //if (ghostboard[x, y] == 0)
                    //{
                    //    spriteBatch.Draw(black, new Vector2(x * 30, (y - 2) * 30));
                    //}
                    if (ghostboard[x, y] == 1)
                    {
                        spriteBatch.Draw(ghost, new Vector2(x * 30, (y - 2) * 30));
                    }
                    
                }
            }




            spriteBatch.End();
            base.Draw(gameTime);
        }



        private bool canGoUp()
        {

            if (pY == 2 || board[pX,pY-1]==2)
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

            if (pY >= 19 || board[pX, pY + 1] == 2)
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

            if (pX == 0 || board[pX-1, pY] == 2)
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

            if (pX == 19 || board[pX + 1, pY] == 2)
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
