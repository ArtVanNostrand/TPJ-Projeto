#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
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
        Texture2D pacman, block, pellet, black, ghost, ghost2, ghost3, bullet, box;
        Texture2D afterimageright, afterimageleft, afterimageup, afterimagedown;
        SpriteFont font1;
        SoundEffect eatingpellet, music1;
        //board = 0 -> nada/vazio
        //board = 1 -> pacman
        //board = 2 -> bloco
        //board = 3 -> pellet
        bool spacepressed = false;
        //40x20
        byte[,] board = new byte[40, 22]; //é preciso +2 no y (20 para 22)
        byte[,] ghostboard = new byte[40, 22];//é preciso +2 no y (20 para 22)
        int[] boardBullet = new int[2];
        //last direction faced: up-1, down-2,right-3,left-4
        //gameover deteta se o pacman ja não tem mais vidas
        int[,] ghostcoords = new int[8, 2];   //guardar coordenadas dos vários fantasmas[número do fantasma, posição(0 = x; 1 = y)]
        int[] ghostLastDirection = new int[8]; //guardar a direção do último movimento dos vários fantasmas
        int pX = 0, pY = 12, auxgpX=20, auxgpY=10, gpX=13, gpY=10, spX=1, spY=1, lastdirectionfaced=0, gameover=0;
        int pelletcont = 0, ghosttype = 1, flag2 = 0, linha, ghostcount, score=0;
        int[] repeat = new int[5];
        float lastHumanMove = 0f;
        float ghostspeed=0f;
        float[] lastGhostMove = new float[8];
        float lastAfterimage = -9999f;
        float lastDashMove = 10f;
        float totalgametime = 0f;
        float lastBullet = 10f;

        float lastTeleport = 10f;
        int tpX = 1, tpY = 1, tmarker=0;

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


            music1.Play();

            for (linha = 0; linha < 8; linha++)
            {
                ghostcoords[linha, 0] = gpX + linha;
                ghostcoords[linha, 1] = gpY;
            }

            for (linha = 0; linha < 8; linha ++)
            {
                ghostLastDirection[linha] = 6;
            }

                //ghostboard[1,1] = 2;



                //começa a desenhar blocos e pellets
                //board[x,y]=2 -> blocos
                //board[x,y]=3 -> pellets
                //Blocos
                for (int y = 3; y < 7; y++)
                {
                    board[1, y] = 2;
                }
            for (int y = 8; y < 12; y++)
            {
                board[1, y] = 2;
            }
            for (int y = 13; y < 16; y++)
            {
                board[1, y] = 2;
            }
            for (int y = 17; y < 21; y++)
            {
                board[1, y] = 2;
            }

            board[2, 11] = 2;
            board[2, 13] = 2;

            for (int y = 3; y < 6; y++)
            {
                board[3, y] = 2;
            }
            for (int y = 7; y < 10; y++)
            {
                board[3, y] = 2;
            }
            for (int y = 15; y < 18; y++)
            {
                board[3, y] = 2;
            }
            for (int y = 19; y < 22; y++)
            {
                board[3, y] = 2;
            }

            board[4, 3] = 2;
            for (int y = 11; y < 14; y++)
            {
                board[4, y] = 2;
            }
            board[4, 17] = 2;

            for (int y = 5; y < 8; y++)
            {
                board[5, y] = 2;
            }
            board[5, 9] = 2;
            board[5, 11] = 0; // = 2
            board[5, 13] = 2;
            board[5, 15] = 2;
            board[5, 19] = 2;
            board[5, 20] = 2;

            board[6, 2] = 2;
            board[6, 3] = 2;

            for (int x = 8; x < 12; x++)
            {
                board[x, 3] = 2;
            }

            board[6, 9] = 2;
            board[6, 11] = 2;
            board[6, 13] = 2;
            for (int y = 15; y < 18; y++)
            {
                board[6, y] = 2;
            }
            board[6, 20] = 2;

            board[7, 5] = 2;
            board[7, 7] = 2;
            board[7, 17] = 2;
            board[7, 18] = 2;
            board[7, 20] = 2;

            board[8, 5] = 2;
            for (int y = 7; y < 10; y++)
            {
                board[8, y] = 2;
            }
            board[8, 15] = 2;

            board[9, 5] = 2;
            board[9, 9] = 2;
            board[9, 11] = 2;
            board[9, 12] = 2;
            board[9, 14] = 2;
            board[9, 15] = 2;
            for (int y = 17; y < 20; y++)
            {
                board[9, y] = 2;
            }
            board[9, 21] = 2;

            board[10, 7] = 2;
            board[10, 11] = 2;
            board[10, 19] = 2;

            for (int y = 5; y < 10; y++)
            {
                board[11, y] = 2;
            }
            board[11, 11] = 2;
            board[11, 13] = 2;
            board[11, 14] = 2;
            board[11, 16] = 2;
            board[11, 17] = 2;

            for (int x = 10; x < 16; x++)
            {
                board[x, 21] = 2;
            }

            board[12, 6] = 2;
            board[12, 11] = 2;
            board[12, 19] = 2;

            board[13, 2] = 2;
            board[13, 4] = 2;
            board[13, 6] = 2;
            board[13, 8] = 2;
            board[13, 9] = 2;
            for (int y = 16; y < 20; y++)
            {
                board[13, y] = 2;
            }

            board[14, 4] = 2;
            board[14, 8] = 2;

            for (int y = 3; y < 7; y++)
            {
                board[15, y] = 2;
            }
            board[15, 8] = 2;
            for (int y = 11; y < 14; y++)
            {
                board[15, y] = 2;
            }
            for (int y = 16; y < 19; y++)
            {
                board[15, y] = 2;
            }
            board[15, 20] = 2;

            board[16, 8] = 2;
            board[16, 13] = 2;

            board[17, 3] = 2;
            board[17, 5] = 2;
            board[17, 6] = 2;
            for (int y = 11; y < 14; y++)
            {
                board[17, y] = 2;
            }
            for (int y = 16; y < 19; y++)
            {
                board[17, y] = 2;
            }
            board[17, 20] = 2;

            board[18, 3] = 2;
            board[18, 8] = 2;
            board[18, 20] = 2;

            for (int y = 5; y < 10; y++)
            {
                board[19, y] = 2;
            }
            for (int y = 14; y < 19; y++)
            {
                board[19, y] = 2;
            }

            board[20, 2] = 2;
            board[20, 3] = 2;
            board[20, 7] = 2;
            board[20, 11] = 2;
            board[20, 12] = 2;
            board[20, 16] = 2;
            board[20, 20] = 2;
            board[20, 21] = 2;

            board[21, 5] = 2;
            board[21, 9] = 2;
            for (int y = 12; y < 15; y++)
            {
                board[21, y] = 2;
            }
            board[21, 16] = 2;
            board[21, 18] = 2;

            for (int y = 3; y < 8; y++)
            {
                board[22, y] = 2;
            }
            board[22, 9] = 2;
            board[22, 10] = 2;
            board[22, 12] = 2;
            for (int y = 18; y < 21; y++)
            {
                board[22, y] = 2;
            }

            for (int y = 14; y < 17; y++)
            {
                board[23, y] = 2;
            }
            board[23, 18] = 2;

            for (int y = 3; y < 6; y++)
            {
                board[24, y] = 2;
            }
            for (int y = 7; y < 11; y++)
            {
                board[24, y] = 2;
            }
            board[24, 12] = 2;
            board[24, 20] = 2;

            board[25, 3] = 2;
            board[25, 7] = 2;
            for (int y = 12; y < 15; y++)
            {
                board[25, y] = 2;
            }
            board[25, 16] = 2;
            for (int y = 18; y < 21; y++)
            {
                board[25, y] = 2;
            }

            board[26, 5] = 2;
            board[26, 9] = 2;
            board[26, 10] = 2;
            board[26, 16] = 2;

            for (int y = 2; y < 6; y++)
            {
                board[27, y] = 2;
            }
            for (int y = 7; y < 10; y++)
            {
                board[27, y] = 2;
            }
            for (int y = 16; y < 19; y++)
            {
                board[27, y] = 2;
            }
            board[27, 20] = 2;
            board[27, 21] = 2;

            board[28, 11] = 2;
            board[28, 13] = 2;

            board[29, 3] = 2;
            for (int y = 5; y < 8; y++)
            {
                board[29, y] = 2;
            }
            board[29, 9] = 2;
            board[29, 11] = 2;
            board[29, 13] = 2;
            for (int y = 15; y < 18; y++)
            {
                board[29, y] = 2;
            }
            for (int y = 19; y < 21; y++)
            {
                board[29, y] = 2;
            }

            board[30, 3] = 2;
            board[30, 9] = 2;
            for (int y = 11; y < 14; y++)
            {
                board[30, y] = 2;
            }

            for (int y = 5; y < 8; y++)
            {
                board[31, y] = 2;
            }
            for (int y = 16; y < 21; y++)
            {
                board[31, y] = 2;
            }

            board[32, 3] = 2;
            board[32, 7] = 2;
            board[32, 9] = 2;
            board[32, 10] = 2;
            for (int y = 12; y < 15; y++)
            {
                board[32, y] = 2;
            }
            board[32, 20] = 2;

            for (int y = 3; y < 6; y++)
            {
                board[33, y] = 2;
            }
            board[33, 9] = 2;
            board[33, 14] = 2;
            board[33, 16] = 2;
            board[33, 17] = 2;
            board[33, 18] = 2;
            board[33, 20] = 2;

            //board[34, 7] = 2;
            //board[34, 11] = 2;
            //board[34, 12] = 2;



            //por as pellets
            for (int x = 0; x < 35; x++)
            {
                for (int y = 2; y < 22; y++)
                {
                    if (board[x, y] != 2)
                    {
                        board[x, y] = 3;
                    }
                }
            }

            //zona fantasmas
            for (int c = 3; c < 9; c++)
            {
                for (int l = 10; l < 15; l++)
                {
                    board[c, l] = 0;
                }
            }

            for (int a = 13; a < 20; a++)
            {
                for (int b = 10; b < 16; b++)
                {
                    board[a, b] = 0;
                }
            }

            for (int d = 26; d < 32; d++)
            {
                for (int e = 10; e < 16; e++)
                {
                    board[d, e] = 0;
                }
            }

            //zona fantasma esquerda
            for (int y = 11; y < 14; y++)
            {
                board[4, y] = 2;
            }
            board[5, 11] = 2; 
            board[5, 13] = 2;
            board[6, 11] = 2;
            board[6, 13] = 2;

            //zona fantasma meio
            for (int y = 11; y < 14; y++)
            {
                board[15, y] = 2;
            }
            board[16, 13] = 2;
            for (int y = 11; y < 14; y++)
            {
                board[17, y] = 2;
            }

            //zona fantasma direita
            board[28, 11] = 2;
            board[28, 13] = 2;
            board[29, 11] = 2;
            board[29, 13] = 2;
            for (int y = 11; y < 14; y++)
            {
                board[30, y] = 2;
            }


            board[0, 12] = 1;

            //Zonas em que o fantasma fica preso agora abertas
            board[4, 12] = 0;
            board[16, 13] = 0;
            board[30, 12] = 0;

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            box = Content.Load<Texture2D>("bloco");
            pacman = Content.Load<Texture2D>("pacman v2 30x30");
            block = Content.Load<Texture2D>("block 30x30 v2");
            pellet = Content.Load<Texture2D>("white pellet v2 10x10");
            bullet = Content.Load<Texture2D>("yellow shot v2 30x14");
            black = Content.Load<Texture2D>("black");
            ghost = Content.Load<Texture2D>("blueghost30 v2");
            ghost2 = Content.Load<Texture2D>("greenghost30 v2");
            ghost3 = Content.Load<Texture2D>("purpleghost30 v2");
            afterimageright = Content.Load<Texture2D>("afterimage_pacman30right");
            afterimageup = Content.Load<Texture2D>("afterimage_pacman30up");
            afterimagedown = Content.Load<Texture2D>("afterimage_pacman30down");
            afterimageleft = Content.Load<Texture2D>("afterimage_pacman30left");

            eatingpellet = Content.Load<SoundEffect>("placeholder sound");
            music1 = Content.Load<SoundEffect>("placeholder music");

            font1 = Content.Load<SpriteFont>("SpriteFont1a");


            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            pacman.Dispose();
            block.Dispose();
            pellet.Dispose();
            black.Dispose();
            ghost.Dispose();
            ghost2.Dispose();
            afterimageright.Dispose();
            afterimageup.Dispose();
            afterimagedown.Dispose();
            afterimageleft.Dispose();
            eatingpellet.Dispose();
            bullet.Dispose();
            box.Dispose();
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
            for (int x = 0; x < 8f; x++)
            {
                lastGhostMove[x] += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            lastDashMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastBullet += (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastTeleport += (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastAfterimage += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (gameover == 0)
            {
                totalgametime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            spacepressed = false;


            ghostspeed=4f;

            if(totalgametime>60f){
                ghostspeed=5f;
            }
            if(totalgametime>120f){
                ghostspeed=6f;
            }


            int cooldown = 0, random1 = 0, bX, bY; 
            int[] flagBullet = new int[1];
            int[] flagFirstBullet = new int[1];
            Random rnd = new Random();
            int randommov = rnd.Next(1, 5);
            //ghosttype = 3; // apenas para testar as diferentes A.I. dos ghosts
            for (ghostcount = 0; ghostcount < 3; ghostcount++)
            {


                gpX = ghostcoords[ghostcount, 0];
                gpY = ghostcoords[ghostcount, 1];

                if (ghostcount == 0)
                {
                    ghosttype = 1;  //movimento random
                }
                if (ghostcount != 0)
                {
                    ghosttype = 3;  //segue o jogador
                }
                //detetar se um fantasma apanhou o pacman
                if (pX == gpX && pY == gpY)
                {
                    gameover = 1;
                }

                 
                                                     //ghostspeed
                if (lastGhostMove[ghostcount] >= 1f / ghostspeed)
                {
                    lastGhostMove[ghostcount] = 0f;

                    //movimento completamente random
                    if (ghosttype == 1)
                    {
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
                        if (ghostcount == 0) ghostboard[gpX, gpY] = 1;
                        else ghostboard[gpX, gpY] = 2;
                    }//ghosttype==1


                    //segue o jogador mas prioritiza o x
                    if (ghosttype == 2)
                    {
                        ghostboard[gpX, gpY] = 0;
                        cooldown = 0;

                        if (gpX != pX)
                        {

                            if (gpX > pX)
                            {
                                if (gcanGoLeft() == true)
                                {
                                    gpX--;
                                    cooldown = 1;
                                }
                            }
                            else
                            {
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
                        if (ghostcount == 0) ghostboard[gpX, gpY] = 1;
                        else ghostboard[gpX, gpY] = 2;
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
                                    if (gcanGoLeft() == true && ghostLastDirection[ghostcount] != 6) // 8 - cima // 4 - esquerda // 5 - baixo // 6 - direita
                                    {
                                        gpX--;
                                        ghostLastDirection[ghostcount] = 4;
                                        cooldown2 = 1;
                                    }
                                }
                                else
                                {
                                    if (gcanGoRight() == true && ghostLastDirection[ghostcount] != 4)
                                    {
                                        gpX++;
                                        ghostLastDirection[ghostcount] = 6;
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
                                        if (gcanGoUp() == true && ghostLastDirection[ghostcount] != 5)
                                        {
                                            gpY--;
                                            ghostLastDirection[ghostcount] = 8;
                                            cooldown2 = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (gcanGoDown() == true && ghostLastDirection[ghostcount] != 8)
                                        {
                                            gpY++;
                                            ghostLastDirection[ghostcount] = 5;
                                            cooldown2 = 1;
                                        }
                                    }

                                }
                            }
                        }





                        if (cooldown2 == 0)
                        {
                            if (repeat[1] + repeat[2] + repeat[3] + repeat[4] > 9)
                            {
                                for (int x = 0; x < 5; x++)
                                {
                                    repeat[x] = 0;
                                }
                            }

                            if (flag2 == 1)
                            {
                                if (gcanGoUp() == true && ghostLastDirection[ghostcount] != 5)
                                {
                                    flag2 = 0;
                                    //repeat[1]++;
                                    gpY--;
                                    ghostLastDirection[ghostcount] = 8;
                                    cooldown2 = 1;
                                }
                            }
                            if (flag2 == 2)
                            {
                                if (gcanGoLeft() == true && ghostLastDirection[ghostcount] != 6)
                                {
                                    flag2 = 0;
                                    //repeat[1]++;
                                    gpX--;
                                    ghostLastDirection[ghostcount] = 4;
                                    cooldown2 = 1;
                                }
                            }
                            if (flag2 == 3)
                            {
                                if (gcanGoDown() == true && ghostLastDirection[ghostcount] != 8)
                                {
                                    flag2 = 0;
                                    //repeat[1]++;
                                    gpY++;
                                    ghostLastDirection[ghostcount] = 5;
                                    cooldown2 = 1;
                                }
                            }
                            if (flag2 == 4)
                            {
                                if (gcanGoRight() == true && ghostLastDirection[ghostcount] != 4)
                                {
                                    flag2 = 0;
                                    //repeat[1]++;
                                    gpX++;
                                    ghostLastDirection[ghostcount] = 6;
                                    cooldown2 = 1;
                                }
                            }
                        }




                        if (cooldown2 == 0)
                        {
                            if (gcanGoUp() == true && ghostLastDirection[ghostcount] != 5)
                            {
                                //if (repeat[1] < 5)
                                //{
                                flag2 = 1;
                                repeat[1]++;
                                gpY--;
                                ghostLastDirection[ghostcount] = 8;
                                cooldown2 = 1;
                                //}
                            }
                        }

                        if (cooldown2 == 0)
                        {
                            if (gcanGoLeft() == true && ghostLastDirection[ghostcount] != 6)
                            {
                                //if (repeat[2] < 5)
                                //{
                                flag2 = 2;
                                repeat[2]++;
                                gpX--;
                                ghostLastDirection[ghostcount] = 4;
                                cooldown2 = 1;
                                //}
                            }
                        }

                        if (cooldown2 == 0)
                        {
                            if (gcanGoDown() == true && ghostLastDirection[ghostcount] != 8)
                            {
                                //if (repeat[3] < 5)
                                //{
                                flag2 = 3;
                                repeat[3]++;
                                gpY++;
                                ghostLastDirection[ghostcount] = 5;
                                cooldown2 = 1;
                                //}
                            }
                        }

                        if (cooldown2 == 0)
                        {
                            if (gcanGoRight() == true && ghostLastDirection[ghostcount] != 4)
                            {
                                //if (repeat[4] < 5)
                                //{
                                flag2 = 4;
                                repeat[4]++;
                                gpX++;
                                ghostLastDirection[ghostcount] = 6;
                                cooldown2 = 1;
                                //}
                            }
                        }




                        if (ghostcount == 0) ghostboard[gpX, gpY] = 1;
                        else ghostboard[gpX, gpY] = 2;
                    }//ghosttype==3







                    //segue o jogador +/- random
                    if (ghosttype == 4)
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
                                }
                                else
                                {
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
                                            cooldown2 = 1;
                                        }
                                    }
                                    else
                                    {
                                        if (gcanGoDown() == true)
                                        {
                                            gpY++;
                                            cooldown2 = 1;
                                        }
                                    }

                                }
                            }
                        }



                        if (cooldown2 == 0)
                        {
                            if (gcanGoUp() == true)
                            {

                                flag2 = 1;

                                gpY--;
                                cooldown2 = 1;

                            }
                        }

                        if (cooldown2 == 0)
                        {
                            if (gcanGoLeft() == true)
                            {

                                flag2 = 2;

                                gpX--;
                                cooldown2 = 1;

                            }
                        }

                        if (cooldown2 == 0)
                        {
                            if (gcanGoDown() == true)
                            {

                                flag2 = 3;

                                gpY++;
                                cooldown2 = 1;

                            }
                        }

                        if (cooldown2 == 0)
                        {
                            if (gcanGoRight() == true)
                            {

                                flag2 = 4;

                                gpX++;
                                cooldown2 = 1;

                            }
                        }




                        if (ghostcount == 0) ghostboard[gpX, gpY] = 1;
                        else ghostboard[gpX, gpY] = 2;
                        //auxgpX = gpX;
                        //auxgpY = gpY;

                    }//ghosttype==4






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


                    ghostcoords[ghostcount, 0] = gpX;
                    ghostcoords[ghostcount, 1] = gpY;
                    //ghostcount++;
                    //if (ghostcount == 3) ghostcount = 0;
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
                                    eatingpellet.Play();
                                    pelletcont++;
                                    score++;
                                }
                                //apaga a posição antiga do pacman, mexe-o para baixo e desenha a nova posição
                                board[pX, pY] = 0;
                                pY--;
                                if (board[pX, pY] == 3)
                                {
                                    eatingpellet.Play();
                                    pelletcont++;
                                    score++;
                                }
                                board[pX, pY] = 1;
                                //
                                lastdirectionfaced = 1;
                                pacman = Content.Load<Texture2D>("pacman v2 30x30 -  up");
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
                                if (board[pX, pY] == 3)
                                {
                                    eatingpellet.Play();
                                    pelletcont++;
                                    score++;
                                }
                                board[pX, pY] = 1;
                                lastdirectionfaced = 2;

                                pacman = Content.Load<Texture2D>("pacman v2 30x30 -  down");
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
                                if (board[pX, pY] == 3)
                                {
                                    eatingpellet.Play();
                                    pelletcont++;
                                    score++;
                                }
                                board[pX, pY] = 1;
                                lastdirectionfaced = 3;
                                pacman = Content.Load<Texture2D>("pacman v2 30x30 -  right");
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
                                if (board[pX, pY] == 3)
                                {
                                    eatingpellet.Play();
                                    pelletcont++;
                                    score++;
                                }
                                board[pX, pY] = 1;
                                lastdirectionfaced = 4;

                                pacman = Content.Load<Texture2D>("pacman v2 30x30");
                            }
                        }
                    }

                    int cont1 = 0;

                    if (pelletcont > 9)
                    {
                        if (lastDashMove >= 10f)
                        {
                            if (spacepressed == false && Keyboard.GetState().IsKeyDown(Keys.Space))
                            {

                                //dash - mexe-o 5 unidades para o sentido que o pacman esta virado mas gasta 10 pellets
                                if (lastdirectionfaced == 1)
                                {
                                    cont1 = 0;
                                    while (canGoUp() == true && cont1 < 5)
                                    {
                                        lastAfterimage = 0f;
                                        lastDashMove = 0f;

                                        board[pX, pY] = 5;
                                        cont1++;

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
                                        lastAfterimage = 0f;
                                        lastDashMove = 0f;

                                        board[pX, pY] = 6;
                                        cont1++;

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
                                        lastAfterimage = 0f;
                                        lastDashMove = 0f;

                                        board[pX, pY] = 7;
                                        cont1++;
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
                                        lastAfterimage = 0f;
                                        lastDashMove = 0f;

                                        board[pX, pY] = 8;
                                        cont1++;

                                        pX--;
                                        board[pX, pY] = 1;
                                        spacepressed = true;
                                    }
                                }
                                if (lastDashMove < 0.1f)
                                {
                                    pelletcont = pelletcont - 10;
                                }

                            }

                        }
                    }


                    //disparar
                    if (pelletcont > 0)
                    {
                        if (lastBullet >= 0.8f)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.Z))
                            {
                                if (lastdirectionfaced == 1)
                                {
                                    cont1 = 0;
                                    if (scanGoUp() == true && cont1 < 5)
                                    {
                                        flagBullet[0] = 1;
                                        flagFirstBullet[0] = 0;
                                    }
                                }
                                if (lastdirectionfaced == 2)
                                {
                                    cont1 = 0;
                                    bX = pX;
                                    bY = pY;
                                    while (scanGoDown() == true && cont1 < 5)
                                    {
                                        lastBullet = 0f;

                                        board[pX, pY] = 9;
                                        cont1++;

                                        pY++;
                                        board[pX, pY] = 1;
                                        spacepressed = true;
                                    }
                                }
                                if (lastdirectionfaced == 3)
                                {
                                    cont1 = 0;
                                    bX = pX;
                                    bY = pY;
                                    while (scanGoRight() == true && cont1 < 5)
                                    {
                                        lastBullet = 0f;

                                        board[pX, pY] = 9;
                                        cont1++;
                                        pX++;
                                        board[pX, pY] = 1;
                                        spacepressed = true;
                                    }
                                }
                                if (lastdirectionfaced == 4)
                                {
                                    cont1 = 0;
                                    bX = pX;
                                    bY = pY;
                                    while (scanGoLeft() == true && cont1 < 5)
                                    {
                                        lastBullet = 0f;

                                        board[pX, pY] = 9;
                                        cont1++;

                                        pX--;
                                        board[pX, pY] = 1;
                                        spacepressed = true;
                                    }
                                }

                                if (lastBullet < 0.1f)
                                {
                                    pelletcont = pelletcont - 3;
                                }

                            }

                        }
                    }
                    //disparar

                    if (flagBullet[0] == 1 && cont1 < 5)
                    {
                        //if (lastBullet >= 1f / 6f)
                        lastBullet = 0f;
                        if (flagFirstBullet[0] == 0)
                        {
                            board[pX, pY - 1] = 9;
                            boardBullet[0] = pX;
                            boardBullet[1] = pY - 1;
                            flagFirstBullet[0] = 1;
                        }
                        else
                        {
                            board[boardBullet[0], boardBullet[1]] = 0;
                            boardBullet[1] = boardBullet[1] - 1;
                            board[boardBullet[0], boardBullet[1]] = 9;
                        }
                        cont1++;
                        board[pX, pY] = 1;
                        spacepressed = true;
                    }

                    //teleport
                    if (pelletcont > 24)
                    {
                        if (lastTeleport >= 1.9f)
                        {
                            if (Keyboard.GetState().IsKeyDown(Keys.X))
                            {
                                pelletcont = pelletcont - 25;
                                lastTeleport = 0f;

                                if (tmarker == 0)
                                {
                                    board[pX, pY] = 5;
                                    tpX = pX;
                                    tpY = pY;
                                    tmarker = 1;
                                }else{

                                
                                    for (int x = 0; x < 40; x++)
                                    {
                                        for (int y = 0; y < 22; y++)
                                        {
                                            if (board[x,y] == 1)
                                            {
                                                board[x, y] = 0;
                                                board[tpX, tpY] = 1;
                                                pX = tpX;
                                                pY = tpY;
                                            }
                                        }
                                    }
                                    tmarker = 0;
                                }

                            }
                        }
                    }//teleport







                }



                //detetar se um fantasma apanhou o pacman
                if (pX == gpX && pY == gpY)
                {
                    gameover = 1;
                }

                if (gameover == 1)
                {
                    for (int x = 0; x < 40; x++)
                    {
                        for (int y = 0; y < 22; y++)
                        {
                            board[x, y] = 2;
                            ghostboard[x, y] = 0;
                        }
                    }
                }



                if (lastAfterimage > 0.5f)
                {
                    for (int x = 0; x < 40; x++)
                    {
                        for (int y = 0; y < 22; y++)
                        {
                            if ((board[x, y] > 4) && (board[x, y] < 9))
                            {
                                board[x, y] = 0;
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

         

            //spriteBatch.Draw(pacman, new Vector2(300, 300), Color.Yellow);

            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 22; y++)
                {

                    //vai desenhando todos os elementos do jogo a medida que são precisos
                    //board[x, y] == 0 -> nada/vazio
                    //board[x, y] == 1 -> pacman
                    //board[x, y] == 2 -> bloco
                    //board[x, y] == 3 -> pellet
                    //board[x, y] == 4 -> bullet
                    //board[x, y] == 5-8-> afterimages
                    //ghostboard[x, y] == 1 -> fantasma
                   
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
                        spriteBatch.Draw(block, new Vector2(x * 30, (y-2) * 30));
                    }
                    if (board[x, y] == 3)
                    {
                        spriteBatch.Draw(pellet, new Vector2(x * 30+10, (y-2) * 30+10));
                    }
                    if (board[x, y] == 4)
                    {
                        spriteBatch.Draw(bullet, new Vector2(x * 30, (y - 2) * 30));
                    }
                    if (board[x, y] == 5)
                    {
                        spriteBatch.Draw(afterimageup, new Vector2(x * 30, (y - 2) * 30));
                    }
                    if (board[x, y] == 6)
                    {
                        spriteBatch.Draw(afterimagedown, new Vector2(x * 30, (y - 2) * 30));
                    }
                    if (board[x, y] == 7)
                    {
                        spriteBatch.Draw(afterimageright, new Vector2(x * 30, (y - 2) * 30));
                    }
                    if (board[x, y] == 8)
                    {
                        spriteBatch.Draw(afterimageleft, new Vector2(x * 30, (y - 2) * 30));
                    }
                    if (board[x,y] == 9)
                    {
                        spriteBatch.Draw(bullet, new Vector2(x * 30, (y - 2) * 30));
                    }

                    //if (board[x, y] == 4)
                    //{
                    //    spriteBatch.Draw(ghost, new Vector2(x * 30, (y - 2) * 30));
                    //}
                    //if (ghostboard[x, y] == 0)
                    //{
                    //    spriteBatch.Draw(black, new Vector2(x * 30, (y - 2) * 30));
                    //}


                    //if (ghostboard[x, y] == 1)
                    //{
                    //    spriteBatch.Draw(ghost, new Vector2(x * 30, (y - 2) * 30));
                    //}
                    //if (ghostboard[x, y] == 2)
                    //{
                    //    spriteBatch.Draw(ghost2, new Vector2(x * 30, (y - 2) * 30));
                    //}

                    spriteBatch.Draw(ghost, new Vector2(ghostcoords[0, 0] * 30, (ghostcoords[0, 1] - 2) * 30));
                    spriteBatch.Draw(ghost2, new Vector2(ghostcoords[1, 0] * 30, (ghostcoords[1, 1] - 2) * 30));
                    spriteBatch.Draw(ghost3, new Vector2(ghostcoords[2, 0] * 30, (ghostcoords[2, 1] - 2) * 30));

                        

                }
            }

            spriteBatch.DrawString(font1, "Time:" + totalgametime, new Vector2(1052, 10), Color.White);

            spriteBatch.DrawString(font1, "Pellets:" + pelletcont, new Vector2(1052, 50), Color.White);

            spriteBatch.DrawString(font1, "Score:" + score, new Vector2(1052, 90), Color.White);


            //spriteBatch.DrawString(font1, "*Abilities*", new Vector2(1050, 130), Color.White);

            spriteBatch.DrawString(font1, "Shoot:3P", new Vector2(1080, 150), Color.White);
            if (lastBullet > 0.8f)
            {
                spriteBatch.DrawString(font1, "(READY!)", new Vector2(1090, 175), Color.White);
            }
            else
            {
                spriteBatch.DrawString(font1, "(Wait...)", new Vector2(1090, 175), Color.White);
            }


            spriteBatch.DrawString(font1, "Dash:10P", new Vector2(1080, 225), Color.White);
            if (lastDashMove > 10f)
            {
                spriteBatch.DrawString(font1, "(READY!)", new Vector2(1090, 250), Color.White);
            }
            else
            {
                spriteBatch.DrawString(font1, "(Wait...)", new Vector2(1090, 250), Color.White);
            }

            spriteBatch.DrawString(font1, "Teleport:25P", new Vector2(1060, 300), Color.White);
            if (lastTeleport > 1.9f)
            {
                spriteBatch.DrawString(font1, "(READY!)", new Vector2(1090, 325), Color.White);
            }
            else
            {
                spriteBatch.DrawString(font1, "(Wait...)", new Vector2(1090, 325), Color.White);
            }



            DrawRectangle(new Rectangle(1050, 0, 148, 600), Color.DarkBlue);


            //teleport marker
            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 22; y++)
                {
                    if (tmarker == 1)
                    {
                        board[tpX, tpY] = 5;
                    }
                }
            }
            //teleport marker

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawRectangle(Rectangle r, Color c)
        {

            spriteBatch.Draw(box, new Rectangle(r.X, r.Y, r.Width, 1), c);
            spriteBatch.Draw(box, new Rectangle(r.X, r.Y, 1, r.Height), c);
            spriteBatch.Draw(box, new Rectangle(r.X, r.Y + r.Height - 1, r.Width, 1), c);
            spriteBatch.Draw(box, new Rectangle(r.X + r.Width - 1, r.Y, 1, r.Height), c);

        }



        private bool canGoUp()
        {

            //verifica se o pacman pode ir para cima, ou seja, não o deixa sair fora do ecra nem ir para a mesma posição que um bloco
            if (pY == 2 || board[pX,pY-1]==2)
            {
                return false;
            }
            else
            {
                
                return true;
            }

        }

        private bool scanGoUp()
        {

            if (spY == 2 || board[spX, spY - 1] == 2 || board[spX, spY - 1] == 1 || ghostboard[spX, spY - 1] != 0)
            {
                board[spX, spY] = 0;
                return false;
            }
            else
            {
                return true;
            }

        }


        private bool gcanGoUp()
        {

            //mesma coisa que a função de cima exceto que é para o fantasma
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

        private bool scanGoDown()
        {

            if (spY >= 21 || board[spX, spY+1] == 2 || board[spX, spY+1] == 1 || ghostboard[spX, spY+1] != 0)
            {
                board[spX, spY] = 0;
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

        private bool scanGoLeft()
        {

            if (spX == 0|| board[spX - 1, spY] == 2 || board[spX - 1, spY] == 1 || ghostboard[spX - 1, spY] != 0)
            {
                board[spX, spY] = 0;
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

            if (pX == 34 || board[pX + 1, pY] == 2)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        private bool scanGoRight()
        {

            if (spX == 34 || board[spX + 1, spY] == 2 || board[spX + 1, spY] == 1 || ghostboard[spX + 1, spY] !=0)
            {
                board[spX, spY] = 0;
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
