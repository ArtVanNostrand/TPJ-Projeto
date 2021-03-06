﻿#region Using Statements
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
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D pacman, block, pellet, black, ghost, ghost2, ghost3, ghost4, ghost5, ghost6, bullet, box, bulletH, explosion;
        Texture2D afterimageright, afterimageleft, afterimageup, afterimagedown, portal, superbullet, superbulletH;
        SpriteFont font1;
        SoundEffect eatingpellet, dashsound, soundteleport, soundtransform, soundpacmanhit, soundghostspeedup;
        SoundEffect soundboom, soundgameover, soundwrong, soundnice, soundbullet, soundghosthit, sound1up;
        Song music1;
        SoundEffect songsuper;
        //board = 0 -> nada/vazio
        //board = 1 -> pacman
        //board = 2 -> bloco
        //board = 3 -> pellet
        //board = 9 -> bullet
        //board = 5-8 -> afterimages
        //ghostboard = 1 -> fantasma
        bool spacepressed = false;
        bool Wpressed = false, QPressed = false, Epressed = false, Rpressed = false;
        int[,] board = new int[40, 22]; //é preciso +2 no y (20 para 22)
        int[,] ghostboard = new int[40, 22];//é preciso +2 no y (20 para 22)
        int[] boardBullet = new int[2];
        //last direction faced: up-1, down-2,right-3,left-4
        //gameover deteta se o pacman ja não tem mais vidas
        int[,] ghostcoords = new int[6, 2];   //guardar coordenadas dos vários fantasmas[número do fantasma, posição(0 = x; 1 = y)]
        int[] ghostHealth = new int[6];
        int[] ghostLastDirection = new int[6]; //guardar a direção do último movimento dos vários fantasmas
        int pX = 0, pY = 12, auxgpX=20, auxgpY=10, gpX=13, gpY=10, spX=1, spY=1, lastdirectionfaced=0, gameover=0;
        int pelletcont = 0, ghosttype = 1, flag2 = 0, linha, ghostcount, score = 0, vidaPacman = 3, pelletScore = 0, kills = 0;
        int[] repeat = new int[5];
        float lastHumanMove = 0f;
        int level = 1, flagSpeed = 0, flagExplosion = 0, auxExplosion, xExplosion, yExplosion, flagHide = 0, flagCollision = 0;
        float ghostspeed=0f, ghostspeedlevel=1;
        float[] lastGhostMove = new float[8];
        float lastBulletMove = 0f;
        float lastExplosion = 0f;
        float lastAfterimage = -9999f;
        float lastDashMove = 10f;
        float lastTimeHit = 0f;
        float lastSuper = 45f;
        float totalgametime = 0f;
        float lastBullet = 10f;
        float CooldownVida = 10f;
        float CooldownGhostHit = 10f;
        float bulletSpeed = 10f;
        float supertime = 0f;
        int flagBullet = 0, auxLastDirection = 0, flagCima = 0, flagBaixo = 0, flagEsquerda = 0, flagDireita = 0;
        int auxflagbullet = 0, auxSup;
        int pellettotal = 0;
        int contbullet = 0, super = 0, cont5=0, Qpressed=0;
        int cont3=0, cont4=0;
        int vidasganhas = 0;
        float bullettravelspeed = 1f;
        float pacmanspeed = 1f / 10f;
        int cooldown = 0, random1 = 0, bX, bY;
        int bulletcont = 0;
        int flagFirstBullet = 0;
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

        protected override void Initialize()
        {

           //Codigo apenas corre quando o progama é inicializado
            base.Initialize();

            MediaPlayer.Play(music1);

            for (linha = 0; linha < 6; linha++)
            {
                ghostcoords[linha, 0] = gpX + linha;
                ghostcoords[linha, 1] = gpY;
            }

            for (linha = 0; linha < 6; linha ++)
            {
                ghostLastDirection[linha] = 6;
            }

            for (linha = 0; linha < 6; linha++)
            {
                ghostHealth[linha] = 1;
            }
            ghostspeed = 3.5f;
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

            //pôr as pellets
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

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            box = Content.Load<Texture2D>("bloco");
            pacman = Content.Load<Texture2D>("pacman v2 30x30");
            block = Content.Load<Texture2D>("block 30x30 v2");
            pellet = Content.Load<Texture2D>("white pellet v2 10x10");
            bullet = Content.Load<Texture2D>("yellow shot v4 9x18");
            bulletH = Content.Load<Texture2D>("yellow shot v4 9x18Horizontal");
            superbullet = Content.Load<Texture2D>("superpellet");
            superbulletH = Content.Load<Texture2D>("superpelletHorizontal");
            black = Content.Load<Texture2D>("black");
            ghost = Content.Load<Texture2D>("blueghost30 v2");
            ghost2 = Content.Load<Texture2D>("brownghost30 v2");
            ghost3 = Content.Load<Texture2D>("greyghost30 v2");
            ghost4 = Content.Load<Texture2D>("purpleghost30 v2");
            ghost5 = Content.Load<Texture2D>("greenghost30 v2");
            ghost6 = Content.Load<Texture2D>("redghost30x30v5");
            portal = Content.Load<Texture2D>("portal19x30");
            afterimageright = Content.Load<Texture2D>("afterimage_pacman30right");
            afterimageup = Content.Load<Texture2D>("afterimage_pacman30up");
            afterimagedown = Content.Load<Texture2D>("afterimage_pacman30down");
            afterimageleft = Content.Load<Texture2D>("afterimage_pacman30left");
            explosion = Content.Load<Texture2D>("explosao30x30");
            dashsound = Content.Load<SoundEffect>("placeholder sound");
            eatingpellet = Content.Load<SoundEffect>("eating pellet sound v1");
            soundteleport = Content.Load<SoundEffect>("pacmanteleport");
            soundtransform = Content.Load<SoundEffect>("pacmantransform");
            soundpacmanhit = Content.Load<SoundEffect>("pacmanhit");
            soundboom = Content.Load<SoundEffect>("soundboom");
            soundgameover = Content.Load<SoundEffect>("soundgameover");
            soundwrong = Content.Load<SoundEffect>("wrong sound effect v2");
            soundnice = Content.Load<SoundEffect>("right sound effect");
            soundbullet = Content.Load<SoundEffect>("soundbullet2");
            soundghosthit = Content.Load<SoundEffect>("soundghosthit");
            sound1up = Content.Load<SoundEffect>("sound1up");
            soundghostspeedup = Content.Load<SoundEffect>("soundspeedup");
            music1 = Content.Load<Song>("music1pacman");
            songsuper = Content.Load<SoundEffect>("superpacman14s");
            font1 = Content.Load<SpriteFont>("QuartzMS13");
        }

        protected override void UnloadContent()
        {
            pacman.Dispose();
            block.Dispose();
            pellet.Dispose();
            black.Dispose();
            ghost.Dispose();
            ghost2.Dispose();
            ghost3.Dispose();
            ghost4.Dispose();
            ghost5.Dispose();
            ghost6.Dispose();
            afterimageright.Dispose();
            afterimageup.Dispose();
            afterimagedown.Dispose();
            afterimageleft.Dispose();
            bullet.Dispose();
            box.Dispose();
            eatingpellet.Dispose();
            dashsound.Dispose();
            music1.Dispose();
            songsuper.Dispose();
            bulletH.Dispose();
            superbullet.Dispose();
            superbulletH.Dispose();
            portal.Dispose();
            explosion.Dispose();
            soundteleport.Dispose();
            soundtransform.Dispose();
            soundpacmanhit.Dispose();
            soundboom.Dispose();
            soundgameover.Dispose();
            soundwrong.Dispose();
            soundnice.Dispose();
            soundbullet.Dispose();
            soundghosthit.Dispose();
            sound1up.Dispose();
            soundghostspeedup.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            lastHumanMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            for (int x = 0; x < 8f; x++)
            {
                lastGhostMove[x] += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            lastBulletMove += 100f;
            lastDashMove += (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastTimeHit += (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastBullet += (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastTeleport += (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastAfterimage += (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastExplosion += (float)gameTime.ElapsedGameTime.TotalSeconds;
            lastSuper += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (gameover == 0)
            {
                totalgametime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                CooldownVida += (float)gameTime.ElapsedGameTime.TotalSeconds;
                CooldownGhostHit += (float)gameTime.ElapsedGameTime.TotalSeconds;
                bulletSpeed += (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            spacepressed = false;

            if (flagExplosion == 1)
            {
                if (board[xExplosion, yExplosion] != 1) board[boardBullet[0], boardBullet[1]] = auxExplosion;
                board[xExplosion, yExplosion] = 14;
                flagExplosion = 0;
                lastExplosion = 0;
                flagHide = 1;
            }
            if (lastExplosion > 0.1f && flagHide == 1)
            {
                if (board[xExplosion, yExplosion] != 1) board[xExplosion, yExplosion] = auxExplosion;
                flagHide = 0;
            }

            ghostspeedlevel = 1;
            if(totalgametime>30f){
                ghostspeed=4f;
                ghostspeedlevel = 2;
                if (flagSpeed == 0)
                {
                    soundghostspeedup.Play();
                    flagSpeed = 1;
                }
            }
            if(totalgametime>60f){
                ghostspeed=4.5f;
                ghostspeedlevel = 3;
                if (flagSpeed == 1)
                {
                    soundghostspeedup.Play();
                    flagSpeed = 2;
                }
            }
            if (totalgametime > 90f)
            {
                ghostspeed = 5f;
                ghostspeedlevel = 4;
                if (flagSpeed == 2)
                {
                    soundghostspeedup.Play();
                    flagSpeed = 3;
                }
            }
            if (totalgametime > 120f)
            {
                ghostspeed = 5.5f;
                ghostspeedlevel = 5;
                if (flagSpeed == 3)
                {
                    soundghostspeedup.Play();
                    flagSpeed = 4;
                }
            }
            if (totalgametime > 150f)
            {
                ghostspeed = 6f;
                ghostspeedlevel = 6;
                if (flagSpeed == 4)
                {
                    soundghostspeedup.Play();
                    flagSpeed = 5;
                }
            }
            if (totalgametime > 200f)
            {
                ghostspeed = 6.5f;
                ghostspeedlevel = 7;
                if (flagSpeed == 5)
                {
                    soundghostspeedup.Play();
                    flagSpeed = 6;
                }
            }
            if (totalgametime > 250f)
            {
                ghostspeed = 7f;
                ghostspeedlevel = 8;
                if (flagSpeed == 6)
                {
                    soundghostspeedup.Play();
                    flagSpeed = 7;
                }
            }
            if (totalgametime > 300f)
            {
                ghostspeed = 8f;
                ghostspeedlevel = 9;
                if (flagSpeed == 7)
                {
                    soundghostspeedup.Play();
                    flagSpeed = 8;
                }
            }
            if (totalgametime > 600f)
            {
                ghostspeed = 10f;
                ghostspeedlevel = 10;
                if (flagSpeed == 8)
                {
                    soundghostspeedup.Play();
                    flagSpeed = 9;
                }
            }

            Random rnd = new Random();
            int randommov = rnd.Next(1, 5);
            //ghosttype = 3; // apenas para testar as diferentes A.I. dos ghosts
            //MOVIMENTO FANTASMAS
            for (ghostcount = 0; ghostcount < 6; ghostcount++)
            {
                gpX = ghostcoords[ghostcount, 0];
                gpY = ghostcoords[ghostcount, 1];

                if (board[gpX, gpY] == 1 && super == 1)
                {
                    soundboom.Play();//som de comer
                    kills++;
                    score = score + (5 * level);
                    ghostcoords[ghostcount, 0] = 13;
                    gpX = 13;
                    gpY = 10;
                    ghostHealth[ghostcount] = 1;
                    flagCollision = 1;
                }

                if (ghostcount == 0)
                {
                    ghosttype = 1;  //movimento random
                }
                if (ghostcount != 0)
                {
                    ghosttype = 3;  //segue o jogador
                }

                //detetar se um fantasma apanhou o pacman
                if (pacmanDead(vidaPacman, pX, pY, gpX, gpY, lastTimeHit) == true)
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

                        if (pX == gpX && pY == gpY && CooldownVida > 3f)
                        {
                            CooldownVida = 0f;
                            if (super == 0)
                            {
                                vidaPacman--;
                                if (vidaPacman > 0)
                                {
                                    soundpacmanhit.Play();
                                }
                            }
                        }

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
                        if (pX == gpX && pY == gpY && CooldownVida > 3f)
                        {
                            CooldownVida = 0f;
                            if (super == 0)
                            {
                                vidaPacman--;
                                if (vidaPacman > 0)
                                {
                                    soundpacmanhit.Play();
                                }
                            }
                        }
                    }//ghosttype==1

                    //segue o jogador mas prioritiza o x
                    if (ghosttype == 2)
                    {
                        ghostboard[gpX, gpY] = 0;
                        cooldown = 0;

                        if (pX == gpX && pY == gpY && CooldownVida > 3f)
                        {
                            CooldownVida = 0f;
                            if (super == 0)
                            {
                                vidaPacman--;
                                if (vidaPacman > 0)
                                {
                                    soundpacmanhit.Play();
                                }
                            }
                        }

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
                        if (pX == gpX && pY == gpY && CooldownVida > 3f)
                        {
                            CooldownVida = 0f;
                            if (super == 0)
                            {
                                vidaPacman--;
                                if (vidaPacman > 0)
                                {
                                    soundpacmanhit.Play();
                                }
                            }
                            else
                            {
                                ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1], ghostcount);
                            }
                        }
                    }//ghosttype==2

                    //segue o jogador +/- random
                    if (ghosttype == 3)
                    {
                        if (pX == gpX && pY == gpY && CooldownVida > 3f)
                        {
                            CooldownVida = 0f;
                            if (super == 0)
                            {
                                vidaPacman--;
                                if (vidaPacman > 0)
                                {
                                    soundpacmanhit.Play();
                                }
                            }
                            if (super == 1)
                            {
                                ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1], ghostcount);
                            }
                        }

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
                                flag2 = 1;
                                repeat[1]++;
                                gpY--;
                                ghostLastDirection[ghostcount] = 8;
                                cooldown2 = 1;
                            }
                        }

                        if (cooldown2 == 0)
                        {
                            if (gcanGoLeft() == true && ghostLastDirection[ghostcount] != 6)
                            {
                                flag2 = 2;
                                repeat[2]++;
                                gpX--;
                                ghostLastDirection[ghostcount] = 4;
                                cooldown2 = 1;
                            }
                        }

                        if (cooldown2 == 0)
                        {
                            if (gcanGoDown() == true && ghostLastDirection[ghostcount] != 8)
                            {
                                flag2 = 3;
                                repeat[3]++;
                                gpY++;
                                ghostLastDirection[ghostcount] = 5;
                                cooldown2 = 1;
                            }
                        }

                        if (cooldown2 == 0)
                        {
                            if (gcanGoRight() == true && ghostLastDirection[ghostcount] != 4)
                            {
                                flag2 = 4;
                                repeat[4]++;
                                gpX++;
                                ghostLastDirection[ghostcount] = 6;
                                cooldown2 = 1;
                            }
                        }

                        if (ghostcount == 0) ghostboard[gpX, gpY] = 1;
                        else ghostboard[gpX, gpY] = 2;
                        if (pX == gpX && pY == gpY && CooldownVida > 3f)
                        {
                            CooldownVida = 0f;
                            if (super == 0)
                            {
                                vidaPacman--;
                                if (vidaPacman > 0)
                                {
                                    soundpacmanhit.Play();
                                }
                            }
                            if (super == 1)
                            {
                                ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1], ghostcount);
                            }
                        }
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
                    }//ghosttype==4

                    if (boardBullet[0] == gpX && boardBullet[1] == gpY && flagBullet == 1)
                    {
                          ghostDead(ghostHealth[ghostcount], gpX, gpY, ghostcount);
                    }
                    if (board[gpX, gpY] == 1 && super == 1)
                    {
                        ghostDead(0, gpX, gpY, ghostcount);
                    }
                    if (board[gpX, gpY] != 1) auxSup = board[gpX, gpY];
                    ghostcoords[ghostcount, 0] = gpX;
                    ghostcoords[ghostcount, 1] = gpY;
                }
            }

                if (lastHumanMove >= pacmanspeed)
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
                                    pelletScore++;
                                    score = score + (1 * level);
                                }
                                //apaga a posição antiga do pacman, mexe-o para baixo e desenha a nova posição
                                board[pX, pY] = 0;
                                pY--;
                                if (ghostboard[pX, pY] == 1 && super == 1)
                                {
                                    for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                    {
                                        if (ghostboard[ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1]] == 1) ghostDead(0, gpX, gpY, ghostcount);
                                    }
                                }
                                if (board[pX, pY] == 3)
                                {
                                    eatingpellet.Play();
                                    pelletcont++;
                                    pelletScore++;
                                    score = score + (1 * level);
                                }
                                board[pX, pY] = 1;
                                lastdirectionfaced = 1;
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
                                if (ghostboard[pX, pY] == 1 && super == 1)
                                {
                                    for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                    {
                                        if (ghostboard[ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1]] == 1) ghostDead(0, gpX, gpY, ghostcount);
                                    }
                                }
                                if (board[pX, pY] == 3)
                                {
                                    eatingpellet.Play();
                                    pelletcont++;
                                    pelletScore++;
                                    score = score + (1 * level);
                                }
                                board[pX, pY] = 1;
                                lastdirectionfaced = 2;
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
                                if (ghostboard[pX, pY] == 1 && super == 1)
                                {
                                    for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                    {
                                        if (ghostboard[ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1]] == 1) ghostDead(0, gpX, gpY, ghostcount);
                                    }
                                }
                                if (board[pX, pY] == 3)
                                {
                                    eatingpellet.Play();
                                    pelletcont++;
                                    pelletScore++;
                                    score = score + (1 * level);
                                }
                                board[pX, pY] = 1;
                                lastdirectionfaced = 3;
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
                                if (ghostboard[pX, pY] == 1 && super == 1)
                                {
                                    for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                    {
                                        if (ghostboard[ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1]] == 1) ghostDead(0, gpX, gpY, ghostcount);
                                    }
                                }
                                if (board[pX, pY] == 3)
                                {
                                    eatingpellet.Play();
                                    pelletcont++;
                                    pelletScore++;
                                    score = score + (1 * level);
                                }
                                board[pX, pY] = 1;
                                lastdirectionfaced = 4;
                            }
                        }
                    }

                    //LASTDIRECTIONFACED »» 1 = cima // 2 = baixo // 3 = direita // 4 = esquerda
                    //Disparar OLD parte 1

                    if (Keyboard.GetState().IsKeyUp(Keys.Q)) QPressed = false;
                    if (QPressed == false && Keyboard.GetState().IsKeyDown(Keys.Q))
                    {
                        if (flagBullet == 0)
                        {
                            if (lastBullet > 0.5f)
                            {
                                if (pelletcont > 2 || super ==1)
                                {
                                    if (lastBullet >= 0.5f)
                                    {
                                        if (pY != 2 && lastdirectionfaced == 1 && (board[pX, pY - 1] == 0 || board[pX, pY - 1] == 3 || board[pX, pY - 1] == 10 || board[pX, pY - 1] == 14))
                                        {
                                            if (scanGoUp() == true)
                                            {
                                                lastBullet = 0f;
                                                if (super == 0)
                                                {
                                                    pelletcont = pelletcont - 3;
                                                }
                                                soundbullet.Play();
                                                auxLastDirection = 1;
                                                flagBullet = 1;
                                                flagFirstBullet = 0;
                                                if (ghostboard[pX, pY - 1] == 1)
                                                {
                                                    for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                                    {
                                                        if (ghostboard[ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1] - 1] == 1) ghostDead(0, ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1], ghostcount);
                                                    }
                                                }
                                            }
                                        }
                                        if (pY != 21 && lastdirectionfaced == 2 && (board[pX, pY + 1] == 0 || board[pX, pY + 1] == 3 || board[pX, pY + 1] == 10 || board[pX, pY + 1] == 14))
                                        {
                                            if (scanGoDown() == true)
                                            {
                                                lastBullet = 0f;
                                                if (super == 0)
                                                {
                                                    pelletcont = pelletcont - 3;
                                                }
                                                soundbullet.Play();
                                                auxLastDirection = 2;
                                                flagBullet = 1;
                                                flagFirstBullet = 0;
                                                if (ghostboard[pX, pY + 1] == 1)
                                                {
                                                    for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                                    {
                                                        if (ghostboard[ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1] + 1] == 1) ghostDead(0, ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1], ghostcount);
                                                    }
                                                }
                                            }
                                        }
                                        if (pX != 0 && lastdirectionfaced == 4 && (board[pX - 1, pY] == 0 || board[pX - 1, pY] == 3 || board[pX - 1, pY] == 10 || board[pX - 1, pY] == 14))
                                        {
                                            if (scanGoRight() == true)
                                            {
                                                lastBullet = 0f;
                                                if (super == 0)
                                                {
                                                    pelletcont = pelletcont - 3;
                                                }
                                                soundbullet.Play();
                                                auxLastDirection = 4;
                                                flagBullet = 1;
                                                flagFirstBullet = 0;
                                                if (ghostboard[pX + 1, pY] == 1)
                                                {
                                                    for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                                    {
                                                        if (ghostboard[ghostcoords[ghostcount, 0] + 1, ghostcoords[ghostcount, 1]] == 1) ghostDead(0, ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1], ghostcount);
                                                    }
                                                }
                                            }
                                        }
                                        if (pX != 34 && lastdirectionfaced == 3 && (board[pX + 1, pY] == 0 || board[pX + 1, pY] == 3 || board[pX + 1, pY] == 10 || board[pX + 1, pY] == 14))
                                        {
                                            if (scanGoLeft() == true)
                                            {
                                                lastBullet = 0f;
                                                if (super == 0)
                                                {
                                                    pelletcont = pelletcont - 3;
                                                }
                                                soundbullet.Play();
                                                auxLastDirection = 3;
                                                flagBullet = 1;
                                                flagFirstBullet = 0;
                                                if (ghostboard[pX - 1, pY] == 1)
                                                {
                                                    for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                                    {
                                                        if (ghostboard[ghostcoords[ghostcount, 0] - 1, ghostcoords[ghostcount, 1]] == 1) ghostDead(0, ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1], ghostcount);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        soundwrong.Play();
                                    }
                                }
                                else
                                {
                                    soundwrong.Play();
                                }
                            }
                            else
                            {
                                soundwrong.Play();
                            }
                        }
                        else
                        {
                            soundwrong.Play();
                        }
                        QPressed = true;
                    }

                    //Disparar OLD parte 2
                    if (lastBulletMove >= 1f / bullettravelspeed)
                    {
                        lastBulletMove = 0f;
                        if (flagBullet == 1)
                        {
                            if (auxLastDirection == 1)    //CIMA
                            {
                                if (flagFirstBullet == 0)
                                {
                                    if (ghostboard[pX, pY - 1] == 1)
                                    {
                                        for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                        {
                                            ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1] - 1, ghostcount);
                                        }
                                    }
                                    boardBullet[0] = pX;
                                    boardBullet[1] = pY - 1;
                                    flagFirstBullet = 1;
                                    auxflagbullet = board[boardBullet[0], boardBullet[1]];
                                    board[boardBullet[0], boardBullet[1]] = 9;
                                }
                                else if ((board[boardBullet[0], boardBullet[1] - 1] == 0 || board[boardBullet[0], boardBullet[1] - 1] == 3 || board[boardBullet[0], boardBullet[1] - 1] == 10 || board[boardBullet[0], boardBullet[1] - 1] == 14) && boardBullet[1] != 1)
                                {
                                    if (board[boardBullet[0], boardBullet[1]] == 1 && auxflagbullet == 3)
                                    {
                                        eatingpellet.Play();
                                        pelletcont++;
                                        pelletScore++;
                                        score = score + (1 * level);
                                    }
                                    if (board[boardBullet[0], boardBullet[1]] != 1) board[boardBullet[0], boardBullet[1]] = auxflagbullet;
                                    boardBullet[1] = boardBullet[1] - 1;
                                    auxflagbullet = board[boardBullet[0], boardBullet[1]];
                                    if (super == 0)
                                    {
                                        board[boardBullet[0], boardBullet[1]] = 9;
                                    }
                                    else
                                    {
                                        board[boardBullet[0], boardBullet[1]] = 12;
                                    }
                                }
                                if (boardBullet[1] == 1 || flagCima == 1)
                                {
                                    if (board[boardBullet[0], boardBullet[1]] != 1) board[boardBullet[0], boardBullet[1]] = auxflagbullet;
                                    flagBullet = 0;
                                }
                                if (board[boardBullet[0], boardBullet[1]] == 1 && auxflagbullet == 3)
                                {
                                    eatingpellet.Play();
                                    pelletcont++;
                                    pelletScore++;
                                    score = score + (1 * level);
                                }
                                if (board[boardBullet[0], boardBullet[1] - 1] == 2) flagCima = 1;
                                if (flagBullet == 0)
                                {
                                    flagCima = 0;
                                    auxLastDirection = 0;
                                }
                                if (ghostboard[boardBullet[0], boardBullet[1] - 1] == 1)
                                {
                                    for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                    {
                                        ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1] - 1, ghostcount);
                                    }
                                }
                                if (boardBullet[1] != 21 && ghostboard[boardBullet[0], boardBullet[1] + 1] == 1)
                                {
                                    for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                    {
                                        ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1] + 1, ghostcount);
                                    }
                                }
                            }
                            else if (auxLastDirection == 2)   //BAIXO
                            {
                                if (flagFirstBullet == 0)
                                {
                                    if (ghostboard[pX, pY + 1] == 1)
                                    {
                                        for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                        {
                                            ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1] + 1, ghostcount);
                                        }
                                    }
                                    boardBullet[0] = pX;
                                    boardBullet[1] = pY + 1;
                                    flagFirstBullet = 1;
                                    auxflagbullet = board[boardBullet[0], boardBullet[1]];
                                    board[boardBullet[0], boardBullet[1]] = 9;
                                }
                                else if (boardBullet[1] != 21 && (board[boardBullet[0], boardBullet[1] + 1] == 0 || board[boardBullet[0], boardBullet[1] + 1] == 3 || board[boardBullet[0], boardBullet[1] + 1] == 10 || board[boardBullet[0], boardBullet[1] + 1] == 14))
                                {
                                    if (board[boardBullet[0], boardBullet[1]] == 1 && auxflagbullet == 3)
                                    {
                                        eatingpellet.Play();
                                        pelletcont++;
                                        pelletScore++;
                                        score = score + (1 * level);
                                    }
                                    if (board[boardBullet[0], boardBullet[1]] != 1) board[boardBullet[0], boardBullet[1]] = auxflagbullet;
                                    boardBullet[1] = boardBullet[1] + 1;
                                    auxflagbullet = board[boardBullet[0], boardBullet[1]];
                                    if (super == 0)
                                    {
                                        board[boardBullet[0], boardBullet[1]] = 9;
                                    }
                                    else
                                    {
                                        board[boardBullet[0], boardBullet[1]] = 12;
                                    }
                                }
                                if (boardBullet[1] == 21 && flagBaixo == 1)
                                {
                                    if (board[boardBullet[0], boardBullet[1]] != 1) board[boardBullet[0], boardBullet[1]] = auxflagbullet;
                                    flagBullet = 0;
                                }
                                if (flagBaixo == 1)
                                {
                                    if (board[boardBullet[0], boardBullet[1]] != 1) board[boardBullet[0], boardBullet[1]] = auxflagbullet;
                                    flagBullet = 0;
                                }
                                if (board[boardBullet[0], boardBullet[1]] == 1 && auxflagbullet == 3)
                                {
                                    eatingpellet.Play();
                                    pelletcont++;
                                    pelletScore++;
                                    score = score + (1 * level);
                                }
                                if (boardBullet[1] == 21 && flagBaixo == 0) flagBaixo = 1;
                                if (boardBullet[1] != 21 && board[boardBullet[0], boardBullet[1] + 1] == 2) flagBaixo = 1;
                                if (flagBullet == 0)
                                { 
                                    flagBaixo = 0;
                                    auxLastDirection = 0;
                                }
                                if (boardBullet[1] != 21 && ghostboard[boardBullet[0], boardBullet[1] + 1] == 1)
                                {
                                    for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                    {
                                        ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1] + 1, ghostcount);
                                    }
                                }
                                if (ghostboard[boardBullet[0], boardBullet[1] - 1] == 1)
                                {
                                    for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                    {
                                        ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1] - 1, ghostcount);
                                    }
                                }
                            }
                            else if (auxLastDirection == 4)   //ESQUERDA
                            {
                                if (flagFirstBullet == 0)
                                {
                                    if (ghostboard[pX - 1, pY] == 1)
                                    {
                                        for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                        {
                                            ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0] - 1, ghostcoords[ghostcount, 1], ghostcount);
                                        }
                                    }
                                    boardBullet[0] = pX - 1;
                                    boardBullet[1] = pY;
                                    flagFirstBullet = 1;
                                    auxflagbullet = board[boardBullet[0], boardBullet[1]];
                                    board[boardBullet[0], boardBullet[1]] = 11;
                                }
                                else if (boardBullet[0] != 0 && (board[boardBullet[0] - 1, boardBullet[1]] == 0 || board[boardBullet[0] - 1, boardBullet[1]] == 3 || board[boardBullet[0] - 1, boardBullet[1]] == 10 || board[boardBullet[0] - 1, boardBullet[1]] == 14))
                                {
                                    if (board[boardBullet[0], boardBullet[1]] == 1 && auxflagbullet == 3)
                                    {
                                        eatingpellet.Play();
                                        pelletcont++;
                                        pelletScore++;
                                        score = score + (1 * level);
                                    }
                                    if (board[boardBullet[0], boardBullet[1]] != 1) board[boardBullet[0], boardBullet[1]] = auxflagbullet;
                                    boardBullet[0] = boardBullet[0] - 1;
                                    auxflagbullet = board[boardBullet[0], boardBullet[1]];
                                    if (super == 0)
                                    {
                                        board[boardBullet[0], boardBullet[1]] = 11;
                                    }
                                    else
                                    {
                                        board[boardBullet[0], boardBullet[1]] = 13;
                                    }
                                }
                                if (boardBullet[0] == 0 && flagEsquerda == 1)
                                {
                                    if (board[boardBullet[0], boardBullet[1]] != 1) board[boardBullet[0], boardBullet[1]] = auxflagbullet;
                                    flagBullet = 0;
                                }
                                if (flagEsquerda == 1)
                                {
                                    if (board[boardBullet[0], boardBullet[1]] != 1) board[boardBullet[0], boardBullet[1]] = auxflagbullet;
                                    flagBullet = 0;
                                }
                                if (board[boardBullet[0], boardBullet[1]] == 1 && auxflagbullet == 3)
                                {
                                    eatingpellet.Play();
                                    pelletcont++;
                                    pelletScore++;
                                    score = score + (1 * level);
                                }
                                if (boardBullet[0] == 0 && flagEsquerda == 0) flagEsquerda = 1;
                                if (boardBullet[0] != 0 && board[boardBullet[0] - 1, boardBullet[1]] == 2) flagEsquerda = 1;
                                if (flagBullet == 0)
                                {
                                    flagEsquerda = 0;
                                    auxLastDirection = 0;
                                }
                                if (boardBullet[0] != 0 && ghostboard[boardBullet[0] - 1, boardBullet[1]] == 1)
                                {
                                    for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                    {
                                        ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0] - 1, ghostcoords[ghostcount, 1], ghostcount);
                                    }
                                }
                                if (boardBullet[0] != 34 && ghostboard[boardBullet[0] + 1, boardBullet[1]] == 1)
                                {
                                    for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                    {
                                        ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0] + 1, ghostcoords[ghostcount, 1], ghostcount);
                                    }
                                }
                            }
                            else if (auxLastDirection == 3)   //DIREITA
                            {
                                if (flagFirstBullet == 0)
                                {
                                    if (ghostboard[pX + 1, pY] == 1)
                                    {
                                        for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                        {
                                            ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0] + 1, ghostcoords[ghostcount, 1], ghostcount);
                                        }
                                    }
                                    boardBullet[0] = pX + 1;
                                    boardBullet[1] = pY;
                                    flagFirstBullet = 1;
                                    auxflagbullet = board[boardBullet[0], boardBullet[1]];
                                    board[boardBullet[0], boardBullet[1]] = 11;
                                }
                                else if (boardBullet[0] != 34 && (board[boardBullet[0] + 1, boardBullet[1]] == 0 || board[boardBullet[0] + 1, boardBullet[1]] == 3 || board[boardBullet[0] + 1, boardBullet[1]] == 10 || board[boardBullet[0] + 1, boardBullet[1]] == 14))
                                {
                                    if (board[boardBullet[0], boardBullet[1]] == 1 && auxflagbullet == 3)
                                    {
                                        eatingpellet.Play();
                                        pelletcont++;
                                        pelletScore++;
                                        score = score + (1 * level);
                                    }
                                    if (board[boardBullet[0], boardBullet[1]] != 1) board[boardBullet[0], boardBullet[1]] = auxflagbullet;
                                    boardBullet[0] = boardBullet[0] + 1;
                                    auxflagbullet = board[boardBullet[0], boardBullet[1]];
                                    if (super == 0)
                                    {
                                        board[boardBullet[0], boardBullet[1]] = 11;
                                    }
                                    else
                                    {
                                        board[boardBullet[0], boardBullet[1]] = 13;
                                    }
                                }
                                if (boardBullet[0] == 34 && flagDireita == 1)
                                {
                                    if (board[boardBullet[0], boardBullet[1]] != 1) board[boardBullet[0], boardBullet[1]] = auxflagbullet;
                                    flagBullet = 0;
                                }
                                if (flagDireita == 1)
                                {
                                    if (board[boardBullet[0], boardBullet[1]] != 1) board[boardBullet[0], boardBullet[1]] = auxflagbullet;
                                    flagBullet = 0;
                                }
                                if (board[boardBullet[0], boardBullet[1]] == 1 && auxflagbullet == 3)
                                {
                                    eatingpellet.Play();
                                    pelletcont++;
                                    pelletScore++;
                                    score = score + (1 * level);
                                }
                                if (boardBullet[0] == 34 && flagDireita == 0) flagDireita = 1;
                                if (boardBullet[0] != 34 && board[boardBullet[0] + 1, boardBullet[1]] == 2) flagDireita = 1;
                                if (flagBullet == 0)
                                {
                                    flagDireita = 0;
                                    auxLastDirection = 0;
                                }
                            }
                            if (boardBullet[0] != 34 && ghostboard[boardBullet[0] + 1, boardBullet[1]] == 1)
                            {
                                for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                {
                                    ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0] + 1, ghostcoords[ghostcount, 1], ghostcount);
                                }
                            }
                            if (boardBullet[0] != 0 && ghostboard[boardBullet[0] - 1, boardBullet[1]] == 1)
                            {
                                for (ghostcount = 0; ghostcount < 6; ghostcount++)
                                {
                                    ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0] - 1, ghostcoords[ghostcount, 1], ghostcount);
                                }
                            }
                            for (ghostcount = 0; ghostcount < 6; ghostcount++)
                            {
                                ghostDead(ghostHealth[ghostcount], ghostcoords[ghostcount, 0], ghostcoords[ghostcount, 1], ghostcount);
                            }
                        }
                    }

                    int cont1 = 0;
                    if (Keyboard.GetState().IsKeyUp(Keys.W)) Wpressed = false;
                    if (Wpressed == false && Keyboard.GetState().IsKeyDown(Keys.W))
                        {
                            if (pelletcont > 9 || super==1)
                            {
                                if (lastDashMove >= 10f)
                                {
                                        //dash - mexe-o 5 unidades para o sentido que o pacman esta virado mas gasta 10 pellets
                                        if (lastdirectionfaced == 1)
                                        {
                                            cont1 = 0;
                                            while (canGoUp() == true && cont1 < 5)
                                            {
                                                Qpressed = 1;
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
                                            dashsound.Play();
                                            if (super == 0)
                                            {
                                                pelletcont = pelletcont - 10;
                                            }
                                        }    
                                }
                                else
                                {
                                    soundwrong.Play();
                                }
                            }
                            else
                            {
                                soundwrong.Play();
                            }
                            Wpressed = true;
                        }

                    //teleport
                    if (Keyboard.GetState().IsKeyUp(Keys.E)) Epressed = false;
                    if (Epressed == false && Keyboard.GetState().IsKeyDown(Keys.E))
                    {
                    if (pelletcont > 19 || super==1)
                    {
                        if (lastTeleport >= 1.9f)
                        {
                                if (super == 0)
                                {
                                    pelletcont = pelletcont - 20;
                                }
                                lastTeleport = 0f;

                                if (tmarker == 0)
                                {
                                    dashsound.Play();
                                    board[pX, pY] = 10;
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
                                    soundteleport.Play();
                                    tmarker = 0;
                                }
                            }
                        else
                        {
                            soundwrong.Play();
                        }
                        }
                    else
                    {
                        soundwrong.Play();
                    }
                    Epressed = true;
                    }//teleport
                }

                //detetar se um fantasma apanhou o pacman
                if (pacmanDead(vidaPacman, pX, pY, gpX, gpY, lastTimeHit) == true) gameover = 1;
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

                if (lastdirectionfaced == 1)
                {
                    if (super == 0)
                    {
                        if (CooldownVida < 3f)
                        {
                            pacman = Content.Load<Texture2D>("hurtpacmanup");
                        }
                        else
                        {
                            pacman = Content.Load<Texture2D>("pacman v2 30x30 -  up");
                        }
                    }
                    if (super == 1)
                    {
                        pacman = Content.Load<Texture2D>("superpacmanaltup");
                    }
                }
                if (lastdirectionfaced == 2)
                {
                    if (super == 0)
                    {
                        if (CooldownVida < 3f)
                        {
                            pacman = Content.Load<Texture2D>("newpacmanhurtdown");
                        }
                        else
                        {
                            pacman = Content.Load<Texture2D>("pacman v2 30x30 -  down");
                        }
                    }
                    if (super == 1)
                    {
                        pacman = Content.Load<Texture2D>("superpacmanaltdown");
                    }
                }
                if (lastdirectionfaced == 3)
                {
                    if (super == 0)
                    {
                        if (CooldownVida < 3f)
                        {
                            pacman = Content.Load<Texture2D>("newpacmanhurtright");
                        }
                        else
                        {
                            pacman = Content.Load<Texture2D>("pacman v2 30x30 -  right");
                        }
                    }
                    if (super == 1)
                    {
                        pacman = Content.Load<Texture2D>("superpacmanaltright");
                    }
                }
                if (lastdirectionfaced == 4)
                {
                    if (super == 0)
                    {
                        if (CooldownVida < 3f)
                        {
                            pacman = Content.Load<Texture2D>("newpacmanhurtleft");
                        }
                        else
                        {
                            pacman = Content.Load<Texture2D>("pacman v2 30x30");
                        }
                    }
                    if (super == 1)
                    {
                        pacman = Content.Load<Texture2D>("superpacmanalt");
                    }
                }

                //super
                if (Keyboard.GetState().IsKeyUp(Keys.R)) Rpressed = false;
                if (Rpressed == false && Keyboard.GetState().IsKeyDown(Keys.R))
                {
                if (super == 0)
                {
                    if (pelletcont > 99)
                    {
                        if (lastSuper >= 45f)
                        {
                                MediaPlayer.Stop();
                                bullettravelspeed = 12f;
                                soundtransform.Play();
                                pelletcont = pelletcont - 100;
                                lastSuper = 0f;
                                supertime = 0f;
                                super = 1;
                                cont4 = 1;
                                pacmanspeed = 1f / 20f;
                            }
                        else
                        {
                            soundwrong.Play();
                        }
                        }
                    else
                    {
                        soundwrong.Play();
                    }
                    }
                Rpressed = true;
                }
            
                if (super == 1)
                {
             
                    lastDashMove = 10f;
                    lastTeleport = 10f;
                    supertime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    if (supertime >= 0.5f)
                    {
                        if (cont4 == 1)
                        {
                            cont4 = 0;
                            songsuper.Play();
                        }
                    }
                    if (supertime >= 14.5f)
                    {
                        super = 0;
                        pacmanspeed = 1f / 10f;
                        bullettravelspeed = 10f;
                        MediaPlayer.Play(music1);
                    }
                }//super

            //respawn pellets
                pellettotal = 0;
                for (int x = 0; x < 40; x++)
                {
                    for (int y = 0; y < 22; y++)
                    {
                        if (board[x, y] == 3)
                        {
                            pellettotal++;
                        }
                    }
                }

                if (pellettotal == 0)
                {
                    if (gameover == 0)
                    {
                        soundnice.Play();
                        score = score + (50 * level);
                        level++;
                        for (int x = 0; x < 35; x++)
                        {
                            for (int y = 2; y < 22; y++)
                            {
                                if (board[x, y] != 2 && board[x, y] != 1)
                                {
                                    board[x, y] = 3;
                                }
                            }
                        }
                    }
                }
            //respawn pellets

            //ganhar vidas
                if (vidasganhas == 0)
                {
                    if (score > 500)
                    {
                        sound1up.Play();
                        vidaPacman++;
                        vidasganhas++;
                    }
                }
                if (vidasganhas == 1)
                {
                    if (score > 1250)
                    {
                        sound1up.Play();
                        vidaPacman++;
                        vidasganhas++;
                    }
                }
                if (vidasganhas == 2)
                {
                    if (score > 2200)
                    {
                        sound1up.Play();
                        vidaPacman++;
                        vidasganhas++;
                    }
                }
                if (vidasganhas == 3)
                {
                    if (score > 4000)
                    {
                        sound1up.Play();
                        vidaPacman++;
                        vidasganhas++;
                    }
                }
                if (vidasganhas == 4)
                {
                    if (score > 7000)
                    {
                        sound1up.Play();
                        vidaPacman++;
                        vidasganhas++;
                    }
                }
                //ganhar vidas
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();
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
                    if (board[x, y] == 2 && gameover == 0)
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
                        spriteBatch.Draw(bullet, new Vector2(x * 30+10, (y - 2) * 30));
                    }
                    if (board[x, y] == 10)
                    {
                        spriteBatch.Draw(portal, new Vector2(x * 30+5, (y - 2) * 30));
                    }
                    if (board[x, y] == 11)
                    {
                        spriteBatch.Draw(bulletH, new Vector2(x * 30 + 10, (y - 2) * 30+10));
                    }
                    if (board[x, y] == 12)
                    {
                        spriteBatch.Draw(superbullet, new Vector2(x * 30 + 10, (y - 2) * 30));
                    }
                    if (board[x, y] == 13)
                    {
                        spriteBatch.Draw(superbulletH, new Vector2(x * 30 + 10, (y - 2) * 30+10));
                    }
                    if (board[x, y] == 14)
                    {
                        spriteBatch.Draw(explosion, new Vector2(x * 30, (y - 2) * 30));
                    }
                    if (gameover == 0)
                    {
                        spriteBatch.Draw(ghost, new Vector2(ghostcoords[0, 0] * 30, (ghostcoords[0, 1] - 2) * 30));
                        spriteBatch.Draw(ghost2, new Vector2(ghostcoords[1, 0] * 30, (ghostcoords[1, 1] - 2) * 30));
                        spriteBatch.Draw(ghost3, new Vector2(ghostcoords[2, 0] * 30, (ghostcoords[2, 1] - 2) * 30));
                        spriteBatch.Draw(ghost4, new Vector2(ghostcoords[3, 0] * 30, (ghostcoords[3, 1] - 2) * 30));
                        spriteBatch.Draw(ghost5, new Vector2(ghostcoords[4, 0] * 30, (ghostcoords[4, 1] - 2) * 30));
                        spriteBatch.Draw(ghost6, new Vector2(ghostcoords[5, 0] * 30, (ghostcoords[5, 1] - 2) * 30));
                    }
                }
            }

            if (gameover == 0)
            {
                spriteBatch.DrawString(font1, "*Info*", new Vector2(1100, 5), Color.White);
                spriteBatch.DrawString(font1, "Life:" + vidaPacman, new Vector2(1052, 30), Color.White);
                spriteBatch.DrawString(font1, "Pellets:" + pelletcont, new Vector2(1052, 55), Color.White);
                spriteBatch.DrawString(font1, "Time:" + totalgametime, new Vector2(1052, 90), Color.White);
                spriteBatch.DrawString(font1, "Score:" + score, new Vector2(1052, 115), Color.White);
                spriteBatch.DrawString(font1, "Kills:" + kills, new Vector2(1052, 140), Color.White);
                spriteBatch.DrawString(font1, "Ghost Speed:" + ghostspeedlevel, new Vector2(1052, 165), Color.White);
                spriteBatch.DrawString(font1, "*Abilities*", new Vector2(1075, 225), Color.White);
                spriteBatch.DrawString(font1, "Shoot:3P(Q)", new Vector2(1065, 250), Color.White);
                if ((lastBullet > 0.5f && flagBullet==0) || (super==1 && flagBullet==0))
                {
                    spriteBatch.DrawString(font1, "(READY!)", new Vector2(1090, 275), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(font1, "(Wait...)", new Vector2(1090, 275), Color.White);
                }
                spriteBatch.DrawString(font1, "Dash:10P(W)", new Vector2(1065, 325), Color.White);
                if (lastDashMove > 10f || super== 1)
                {
                    spriteBatch.DrawString(font1, "(READY!)", new Vector2(1090, 350), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(font1, "(Wait...)", new Vector2(1090, 350), Color.White);
                }
                spriteBatch.DrawString(font1, "Tele:20P(E)", new Vector2(1065, 400), Color.White);
                if (lastTeleport > 1.9f || super==1)
                {
                    if (tmarker == 0)
                    {
                        spriteBatch.DrawString(font1, "(READY!)", new Vector2(1090, 425), Color.White);
                    }
                    if (tmarker == 1)
                    {
                        spriteBatch.DrawString(font1, "(SET!)", new Vector2(1090, 425), Color.White);
                    }
                }
                else
                {
                    spriteBatch.DrawString(font1, "(Wait...)", new Vector2(1090, 425), Color.White);
                }

                spriteBatch.DrawString(font1, "Super:100P(R)", new Vector2(1058, 475), Color.White);
                if (lastSuper > 45f)
                {
                    spriteBatch.DrawString(font1, "(READY!)", new Vector2(1090, 500), Color.White);
                }
                else
                {
                    spriteBatch.DrawString(font1, "(Wait...)", new Vector2(1090, 500), Color.White);
                }
            }

            //                                              x   y largura altura
            if (gameover == 0) DrawRectangle(new Rectangle(1050, 0, 148, 600), Color.DarkBlue);
            if (gameover == 1)
            {
                if (cont5 == 0)
                {
                    soundgameover.Play();
                    cont5 = 1;
                }
                pacman = Content.Load<Texture2D>("pacman v2 30x30 -  up");
                spriteBatch.Draw(pacman, new Vector2(420, 18));
                spriteBatch.Draw(pacman, new Vector2(450, 18));
                spriteBatch.Draw(pacman, new Vector2(480, 18));
                spriteBatch.Draw(pacman, new Vector2(510, 18));
                spriteBatch.Draw(pacman, new Vector2(540, 18));
                spriteBatch.Draw(pacman, new Vector2(570, 18));
                spriteBatch.Draw(pacman, new Vector2(600, 18));
                spriteBatch.Draw(pacman, new Vector2(630, 18));
                spriteBatch.Draw(pacman, new Vector2(660, 18));
                spriteBatch.Draw(pacman, new Vector2(690, 18));
                spriteBatch.Draw(pacman, new Vector2(720, 18));
                pacman = Content.Load<Texture2D>("pacman v2 30x30");
                spriteBatch.Draw(pacman, new Vector2(418, 50));
                spriteBatch.Draw(pacman, new Vector2(418, 80));
                spriteBatch.Draw(pacman, new Vector2(418, 110));
                spriteBatch.Draw(pacman, new Vector2(418, 140));
                spriteBatch.Draw(pacman, new Vector2(418, 170));
                spriteBatch.Draw(pacman, new Vector2(418, 200));
                spriteBatch.Draw(pacman, new Vector2(418, 230));
                spriteBatch.Draw(pacman, new Vector2(418, 260));
                pacman = Content.Load<Texture2D>("pacman v2 30x30 -  right");
                spriteBatch.Draw(pacman, new Vector2(720, 50));
                spriteBatch.Draw(pacman, new Vector2(720, 80));
                spriteBatch.Draw(pacman, new Vector2(720, 110));
                spriteBatch.Draw(pacman, new Vector2(720, 140));
                spriteBatch.Draw(pacman, new Vector2(720, 170));
                spriteBatch.Draw(pacman, new Vector2(720, 200));
                spriteBatch.Draw(pacman, new Vector2(720, 230));
                spriteBatch.Draw(pacman, new Vector2(720, 260));
                pacman = Content.Load<Texture2D>("pacman v2 30x30 -  down");
                spriteBatch.Draw(pacman, new Vector2(418, 290));
                spriteBatch.Draw(pacman, new Vector2(450, 290));
                spriteBatch.Draw(pacman, new Vector2(480, 290));
                spriteBatch.Draw(pacman, new Vector2(510, 290));
                spriteBatch.Draw(pacman, new Vector2(540, 290));
                spriteBatch.Draw(pacman, new Vector2(570, 290));
                spriteBatch.Draw(pacman, new Vector2(600, 290));
                spriteBatch.Draw(pacman, new Vector2(630, 290));
                spriteBatch.Draw(pacman, new Vector2(660, 290));
                spriteBatch.Draw(pacman, new Vector2(690, 290));
                spriteBatch.Draw(pacman, new Vector2(720, 290));
                DrawRectangle(new Rectangle(450, 50, 269, 239), Color.Red);
                spriteBatch.DrawString(font1, "GAME OVER", new Vector2(530, 70), Color.Yellow);
                spriteBatch.DrawString(font1, "Time: " + totalgametime, new Vector2(475, 150), Color.Yellow);
                spriteBatch.DrawString(font1, "Pellets Collected: " + pelletScore, new Vector2(475, 180), Color.Yellow);
                spriteBatch.DrawString(font1, "Kills:" + kills, new Vector2(475, 210), Color.Yellow);
                spriteBatch.DrawString(font1, "Score:" + score, new Vector2(475, 240), Color.Yellow);
            }

            //teleport marker
            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 22; y++)
                {
                    if (tmarker == 1)
                    {
                        board[tpX, tpY] = 10;
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
            if (spY == 2 || board[spX, spY - 1] == 2 || board[spX, spY - 1] == 1)
            {
                board[spX, spY] = 0;
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool bulletcanGoUp()
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
            if (spY >= 21 || board[spX, spY+1] == 2 || board[spX, spY+1] == 1)
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
            if (spX == 0|| board[spX - 1, spY] == 2 || board[spX - 1, spY] == 1)
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
            if (spX == 34 || board[spX + 1, spY] == 2 || board[spX + 1, spY] == 1)
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
            if (gpX == 34 || board[gpX + 1, gpY] == 2)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void ghostDead(int vida, int ghostX, int ghostY, int numGhost)
        {
            if ((board[ghostX, ghostY] == 9 || board[ghostX, ghostY] == 11 || board[ghostX, ghostY] == 12 || board[ghostX, ghostY] == 13 || (board[ghostX,ghostY]==1 && super==1)) && CooldownGhostHit > 0.1f)
            {
                vida--;
                CooldownGhostHit = 0f;
            }
            if (vida <= 0)
            {
                xExplosion = ghostcoords[numGhost, 0];
                yExplosion = ghostcoords[numGhost, 1];
                auxExplosion = auxflagbullet;
                soundboom.Play();
                kills++;
                score = score + (5 * level);
                ghostcoords[numGhost, 0] = 13;
                ghostcoords[numGhost, 1] = 10;
                gpX = 13;
                gpY = 10;
                vida = 1;
                if (flagCollision == 0)
                {
                    flagBullet = 0;
                    flagExplosion = 1;
                }
                else flagCollision = 0;
            }//imagem de fantasma morto
        }

        private bool pacmanDead(int vida, int pacmanX, int pacmanY, int ghostX, int ghostY, float lastHit)
        {
            if (vida <= 0) return true;
            else return false;
        }
    }
}
