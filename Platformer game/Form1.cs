using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Platformer_game
{
    public partial class frmPlatfomer : Form
    {
        bool goLeft, goRight, jump, IsGameOver = true;
        int jumpSpeed= 10;
        int force=0;
        int playerspd = 12;

        int score = 0;

        int horizontalspd = 1;
        int verticalspd = 2;
        int counttick = 0;

        int bg1spd = 5;
        int bg2spd = 3;
        int bg3spd = 4;




        public frmPlatfomer()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void GameTickEvent(object sender, EventArgs e)
        {
            txtScore.Text = "Score: "+ score;
            txtScore.Text += Environment.NewLine + "Red bad, "
                + Environment.NewLine + "don't touch red";
            //movement
            player.Top += jumpSpeed;

            if (goLeft == true)
                player.Left -= playerspd;

            if(goRight == true)
                player.Left += playerspd;

            //limit movement by border
            if (player.Left < 0)
                player.Left = 0;

            if (player.Left > 700 - player.Width*2)
                player.Left = 700 - player.Width*2;

            if (player.Top > 800 - player.Height)
            {
                IsGameOver = true;
                GameOver();
            }

            //jump
            if (jump == true && force < 5)//jump ends, start falling
                jump = false;

            if(jump == true)//jumps up
            {
                jumpSpeed -= 8;
                force -= 1;
            }
            else
            {
                jumpSpeed = 10;
            }

            //colision
            foreach (Control obj in this.Controls)
            {

                if(obj is PictureBox)
                {
                    //platform interaction
                    if((string)obj.Tag == "platform")
                    {
                        if (player.Bounds.IntersectsWith(obj.Bounds))//player touches platform
                        {
                            force = 8;// fuel for jumping 8 ticks
                            player.Top = obj.Top - player.Height;
                            jumpSpeed = 0;

                            // move player with platform;
                            if ((string)obj.Name == "horizontalPlatform" && goLeft == false && goRight == false)
                                player.Left += horizontalspd*6;
                        }
                        obj.BringToFront();//avoid visual bugs
                    }

                    //coin interaction
                    if((string)obj.Tag == "coin")
                    {
                        if (player.Bounds.IntersectsWith(obj.Bounds))
                        {
                            if(obj.Visible == true)
                                score += 100;
                            obj.Visible = false;
                        }
                    }

                    //gate interaction
                    if (player.Bounds.IntersectsWith(gate.Bounds))
                    {
                        IsGameOver = true;
                        GameWon();

                    }

                    //badguy interaction
                    if((string)obj.Tag == "badguy")
                    {
                        if (player.Bounds.IntersectsWith(obj.Bounds))
                        {
                            IsGameOver = true;
                            GameOver();
                        }
                    }

                    //horizontal and vertical platform
                    ++counttick;
                    if(counttick%15 == 0)
                    {
                        horizontalPlatform.Left += horizontalspd;
                        if (horizontalPlatform.Left < 100 || horizontalPlatform.Left + horizontalPlatform.Width > 300)
                            horizontalspd = -horizontalspd;

                        verticalPlatform.Top += verticalspd;
                        if (verticalPlatform.Top < 163 || verticalPlatform.Top + verticalPlatform.Height > 450)
                            verticalspd = -verticalspd;
                    }

                    //moving badguys
                    if (counttick % 50 == 0)
                    {
                        if (bg1.Left < 540 || bg1.Left - bg1.Width > 640)
                            bg1spd = -bg1spd;
                        bg1.Left += bg1spd;

                        if (bg2.Left < 50 || bg2.Left - bg2.Width > 150)
                            bg2spd = -bg2spd;
                        bg2.Left += bg2spd;

                        if (bg3.Left < 340 || bg3.Left + bg3.Width > 660)
                            bg3spd = -bg3spd;
                        bg3.Left += bg3spd;
                    }
                }
            }
        }

        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
                goLeft = true;
            if(e.KeyCode == Keys.Right || e.KeyCode ==Keys.D)
                goRight = true;
            if ((e.KeyCode == Keys.Space || e.KeyCode == Keys.Up || e.KeyCode == Keys.W) && jump == false)
            {
                jump = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
                goLeft = false;
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
                goRight = false;
            if (jump == true)     
                jump = false;

            // enter = restart game
            if (e.KeyCode == Keys.Enter && IsGameOver == true)
                RestartGame();
        }

        private void RestartGame()
        {
            //set game state
            jump = false;
            goLeft = false;
            goRight = false;
            IsGameOver = false;
            score= 0;
            txtScore.Text = "Score: "+ score;

            //set all coins to visible
            foreach (Control ctr in this.Controls)
            {
                if (ctr is PictureBox && ctr.Visible == false)
                {
                    ctr.Visible = true;
                }
            }

            //set player position
            player.Left = 18;
            player.Top = 640;

            //set badguys position 
            bg1.Left = 592;
            bg1.Top = 535;

            bg2.Left = 110;
            bg2.Top = 445;

            bg3.Left = 348;
            bg3.Top = 135;

            //set moving platform base pposition 202, 163 12, 388
            horizontalPlatform.Left = 202;
            horizontalPlatform.Top = 163;

            verticalPlatform.Left = 12;
            verticalPlatform.Top = 388;

            //start game
            gameTick.Start();
        }

        void GameOver()
        {
            gameTick.Stop();
            MessageBox.Show("You were killed. Try again?");
        }
        
        void GameWon()
        {
            IsGameOver = true;
            gameTick.Stop();
            MessageBox.Show("You escaped!");
            RestartGame();
            
        }
    }
}
