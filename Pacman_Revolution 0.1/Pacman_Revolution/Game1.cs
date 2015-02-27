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
        //board = 0 -> nada/vazio
        //board = 1 -> pacman
        //board = 2 -> bloco
        //board = 3 -> pellet
        bool spacepressed = false;
        //40x20
        byte[,] board = new byte[40, 22]; //é preciso +2 no y (20 para 22)
        byte[,] ghostboard = new byte[40, 22];//é preciso +2 no y (20 para 22)
        //last direction faced: up-1, down-2,right-3,left-4
        int pX = 20, pY = 10, gpX=2, gpY=2, lastdirectionfaced=0;
        int pelletcont = 0, ghosttype=1;
        float lastHumanMove = 0f;
        float lastGhostMove = 0f;

        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
            //para chegar a estes valores temos de multiplicar por 30. Exemplo: 40(x)*30=1200
            graphics.PreferredBackBufferWidth = 1200; //x
            graphics.PreferredBackBufferHeight = 600; //y
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {

           //Codigo apenas corre quando o progama é inicializado
            base.Initialize();

            ghostboard[2, 2] = 1;
            board[20, 10] = 1;

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
            ghost = Content.Load<Texture2D>("purpleghost30");

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

            lastHumanMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastGhostMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            spacepressed = false;

            int cooldown = 0, random1=0;
            Random rnd = new Random();
            int randommov = rnd.Next(1, 5);
            ghosttype = 3; // apenas para testar as diferentes A.I. dos ghosts

            if (lastGhostMove >= 1f / 4f)
            {
                lastGhostMove = 0f;

                //movimento completamente random
                if (ghosttype == 1){
                    ghostboard[gpX, gpY] = 0;
                    if (randommov == 1)
                    {
                        if (gcanGoRight() == true)
                        {
                            gpX++;
                        }
                    }
                    if (randommov == 2)
                    {
                        if (gcanGoLeft() == true)
                        {
                            gpX--;
                        }
                    }
                    if (randommov == 3)
                    {
                        if (gcanGoDown() == true)
                        {
                            gpY++;
                        }
                    }
                    if (randommov == 4)
                    {
                        if (gcanGoUp() == true)
                        {
                            gpY--;
                        }
                    }
                    if (gpX == pX)
                    {

                    }

                    ghostboard[gpX, gpY] = 1;
                }//ghosttype==1


                //segue o jogador mas prioritiza o x
                if (ghosttype == 2)
                {
                    ghostboard[gpX, gpY] = 0;
                    cooldown = 0;

                    if (gpX != pX)
                    {

                        if (gpX > pX){
                            if (gcanGoLeft() == true)
                            {
                                gpX--;
                                cooldown = 1;
                            }
                        }else{
                            if (gcanGoRight() == true)
                            {
                                gpX++;
                                cooldown = 1;
                            }
                        }

                    }

                    if (gpY != pY)
                    {

                        if (cooldown == 0)
                        {
                            if (gpY > pY)
                            {
                                if (gcanGoUp() == true)
                                {
                                    gpY--;
                                }
                            }
                            else
                            {
                                if (gcanGoDown() == true)
                                {
                                    gpY++;
                                }
                            }
                        }

                    }
                    ghostboard[gpX, gpY] = 1;
                }//ghosttype==2


                //segue o jogador +/- random
                if (ghosttype == 3)
                {
                    ghostboard[gpX, gpY] = 0;
                    int flag1, cooldown2;

                    cooldown2 = 0;
                    random1 = 0;
                    flag1 = 0;

                    if ((gpX != pX) && (gpY != pY))
                    {
                        random1 = rnd.Next(1, 3);
                        flag1 = 1;
                    }

                    if ((flag1 == 0) || (flag1 == 1 && random1 == 1))
                    {
                        if (gpX != pX)
                        {
                                if (gpX > pX)
                                {
                                    if (gcanGoLeft() == true)
                                    {
                                        gpX--;
                                        cooldown2 = 1;
                                    }
                                }else{
                                    if (gcanGoRight() == true)
                                    {
                                        gpX++;
                                        cooldown2 = 1;
                                    }
                                }
                        }
                    }

                    if (cooldown2 == 0)
                    {
                        if ((flag1 == 0) || (flag1 == 1 && random1 == 2))
                        {
                            if (gpY != pY)
                            {
                                if (gpY > pY)
                                {
                                    if (gcanGoUp() == true)
                                    {
                                        gpY--;
                                    }
                                }
                                else
                                {
                                    if (gcanGoDown() == true)
                                    {
                                        gpY++;
                                    }
                                }

                            }
                        }
                    }
                    ghostboard[gpX, gpY] = 1;
                }//ghosttype==3




                ////segue o jogador deseparecendo as vezes
                //if (ghosttype == 10)
                //{
                //    ghostboard[gpX, gpY] = 0;
                //    int flag1;

                //    flag1 = 0;

                //    if ((gpX != pX) && (gpY != pY))
                //    {
                //        random1 = rnd.Next(1, 2);
                //        flag1 = 1;
                //    }

                //    if ((flag1 == 0) || (flag1 == 1 && random1 == 1))
                //    {
                //        if (gpX != pX)
                //        {
                //            if (flag1 == 1 && random1 == 1)
                //            {
                //                if (gpX > pX)
                //                {
                //                    if (gcanGoLeft() == true)
                //                    {
                //                        gpX--;

                //                    }
                //                }
                //                else
                //                {
                //                    if (gcanGoRight() == true)
                //                    {
                //                        gpX++;

                //                    }
                //                }

                //            }
                //        }
                //    }

                //        if ((flag1 == 0) || (flag1 == 1 && random1 == 2))
                //        {
                //            if (gpY != pY)
                //            {


                //                if (cooldown == 0)
                //                {
                //                    if (gpY > pY)
                //                    {
                //                        if (gcanGoUp() == true)
                //                        {
                //                            gpY--;
                //                        }
                //                    }
                //                    else
                //                    {
                //                        if (gcanGoDown() == true)
                //                        {
                //                            gpY++;
                //                        }
                //                    }
                //                }

                //            }

                //            if (gpX == pX)
                //            {

                //            }

                //            ghostboard[gpX, gpY] = 1;
                //        }

                //    }//ghosttype==10

                

                
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
                            lastdirectionfaced = 1;
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
                            lastdirectionfaced = 2;

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
                            lastdirectionfaced = 3;

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
                            lastdirectionfaced = 4;

                            pacman = Content.Load<Texture2D>("pacman30");
                        }
                    }
                }

                int cont1;
                if (spacepressed == false && Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    if (lastdirectionfaced == 1)
                    {
                        cont1 = 0;
                        while (canGoUp() == true && cont1 <5)
                        {
                            cont1++;
                            board[pX, pY] = 0;
                            pY--;
                            board[pX, pY] = 1;
                            spacepressed = true;
                        }
                    }
                    if (lastdirectionfaced == 2)
                    {
                        cont1 = 0;
                        while (canGoDown() == true && cont1 < 5)
                        {
                            cont1++;
                            board[pX, pY] = 0;
                            pY++;
                            board[pX, pY] = 1;
                            spacepressed = true;
                        }
                    }
                    if (lastdirectionfaced == 3)
                    {
                        cont1 = 0;
                        while (canGoRight() == true && cont1 < 5)
                        {
                            cont1++;
                            board[pX, pY] = 0;
                            pX++;
                            board[pX, pY] = 1;
                            spacepressed = true;
                        }
                    }
                    if (lastdirectionfaced == 4)
                    {
                        cont1 = 0;
                        while (canGoLeft() == true && cont1 < 5)
                        {
                            cont1++;
                            board[pX, pY] = 0;
                            pX--;
                            board[pX, pY] = 1;
                            spacepressed = true;
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

            board[25, 10] = 2;
            board[26, 10] = 2;
            board[27, 10] = 2;
            board[28, 10] = 2;
            board[29, 10] = 2;
            board[30, 10] = 2;


            board[21, 19] = 2;
            board[22, 19] = 2;  
            board[18, 19] = 2;
            board[19, 19] = 2;           
     


            //spriteBatch.Draw(pacman, new Vector2(300, 300), Color.Yellow);

            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 22; y++)
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


        private bool gcanGoUp()
        {

            if (gpY == 2 || board[gpX, gpY - 1] == 2)
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

            if (pY >= 21 || board[pX, pY + 1] == 2)
            {
                return false;
            }
            else
            {
               
                return true;
            }

        }

        private bool gcanGoDown()
        {

            if (gpY >= 21 || board[gpX, gpY + 1] == 2)
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

        private bool gcanGoLeft()
        {

            if (gpX == 0 || board[gpX - 1, gpY] == 2)
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

            if (pX == 39 || board[pX + 1, pY] == 2)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool gcanGoRight()
        {

            if (gpX == 39 || board[gpX + 1, gpY] == 2)
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
