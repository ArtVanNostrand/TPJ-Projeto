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
        byte[] pacmanloc = {1}; //isto está mal [,](new byte[20, 20])
        int pX = 4, pY = 10;
        //float lastAutomaticMove = 0f;
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

   

            //if (lastHumanMove >= 1f / 15f)    //GAJO PÔS EM COMENTÁRIO
            //{

                lastHumanMove = 0f;

                if (Keyboard.GetState().IsKeyDown(Keys.Up))
                {
                    if (canGoUp()) pY--;    //PUS canGoUp
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Down))
                {
                    if (canGoDown()) pY++;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Right))
                {
                    if (canGoRight()) pX++;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Left))
                {
                    if (canGoLeft()) pX--;
                }
            //} EM COMENTÁRIO



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
  
            //spriteBatch.Draw(pacman, new Vector2(300,300), Color.Yellow); EM COMENT

            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 20; y++)
                {
                    if (board[x, y] != 0)
                    {
                        spriteBatch.Draw(pacman, new Vector2(x * 30, y * 30));
                    }
                    if (y >= pY && x >= pX && y < (pY + pacmanloc.GetLength(0)) && x < (pX + pacmanloc.GetLength(0)))
                    {
                        if (pacmanloc[x - pX] != 0 && pacmanloc[y - pY] != 0) //Troquei a ordem do x e do y
                        {
                            spriteBatch.Draw(pacman, new Vector2(x * 30, y * 30));
                        }
                    }
                }
            }


            spriteBatch.Draw(block, new Vector2(250, 230));
      
        
          
            spriteBatch.End();
            base.Draw(gameTime);
        }



        private bool canGoUp()
        {

            if (pY == 0)
            {
                return false;
            }
            else
            {
                return canGo(pX, pY - 1);
            }

        }

        private bool canGoDown()
        {

            if (pY >= 22)
            {
                return false;
            }
            else
            {
                return canGo(pX, pY + 1);
            }

        }

        private bool canGoLeft()
        {

            if (pX == 0)
            {
                return false;
            }
            else
            {
                return canGo(pX - 1, pY);
            }

        }

        private bool canGoRight()
        {

            if (pX == 10)
            {
                return false;
            }
            else
            {
                return canGo(pX + 1, pY);
            }

        }


        private bool canGo(int dX, int dY)
        {
            //Vamos supor que é possivel
            // e procurar um contra exemplo


            //for (int x = 0; x < piece.GetLength(1); x++)
            //{
            //    for (int y = 0; y < piece.GetLength(0); y++)
            //    {
            //        if (piece[y, x] != 0 && board[dX + x, dY + y] != 0)
            //        {
            //            return false;
            //        }
            //    }
            //}
            return true;
        }











    }
}
