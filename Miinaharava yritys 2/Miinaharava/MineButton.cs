using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;
using Application = System.Windows.Forms.Application;

namespace Miinaharava
{
    internal class MineButton : Button
    {
        public static int dividePixels;

        public MineButton(int Cordx,int Cordy, string text, bool mine, int height, int width)
        {
            this.Height = height;
            this.Width = width;
            this.BackColor = Color.Green;
            this.ForeColor = Color.Green;
            this.Location = new Point(Cordx, Cordy);
            this.Text = "";
            this.Name = text;
            this.Font = new Font("Arial", 17);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MineClick);
            bool Mine = mine;
            bool Flagged = false;
            bool open = false;
            int minesNear = 0;
        }

        //Jotta voidaan kayttaa niita muualla
        public bool Mine { get; set; }
        public bool Flagged { get; set; }
        public bool open { get; set; }
        public int minesNear { get; set; }

        //Palautetaan onko miina ja onko se merkitty
        public bool ReturnMine()
        {
            return Mine;
        }

        public bool ReturnFlagged()
        {
            return Flagged;
        }
        public bool ReturnOpen()
        {
            return open;
        }

        public static void ChangePixelGap(int gap)
        {
            dividePixels = gap;
        }

        public void NumberReveal()
        {
            MineButton[,] Buttons = PlayArea.ReturnsButtons();
            //käydään jokainen miina läpi
            foreach(MineButton button in Buttons)
            {
                //saadaan koordinaatit
                int pos_x = button.Location.X/dividePixels;
                int pos_y = button.Location.Y/dividePixels; 
                if (button.ReturnMine())
                {
                    //Käydään ruudun ympärys läpi
                    NumberAdd(pos_x+1,pos_y);
                    NumberAdd(pos_x-1, pos_y);
                    NumberAdd(pos_x, pos_y+1);
                    NumberAdd(pos_x, pos_y-1);
                    NumberAdd(pos_x+1, pos_y+1);
                    NumberAdd(pos_x+1, pos_y-1);
                    NumberAdd(pos_x-1, pos_y+1);
                    NumberAdd(pos_x-1, pos_y-1);

                }
            }
        }

        public void NumberAdd(int pos_x, int pos_y)
        {
            MineButton[,] Buttons = PlayArea.ReturnsButtons();
            //tarkistetaan että ruutu on olemassa
            if (pos_x >= 0 && pos_x <= (Buttons.GetLength(0)-1) && pos_y >= 0 && pos_y <= (Buttons.GetLength(1)-1))
            {
                Buttons[pos_x,pos_y].minesNear++;
            }
            
        }
        
        public void MineClear(int pos_x,int pos_y)
        {
            MineButton[,] Buttons = PlayArea.ReturnsButtons();
            //tarkistetaan että ruutu on olemassa
            if (pos_x >= 0 && pos_x <= (Buttons.GetLength(0)-1) && pos_y >= 0 && pos_y <= (Buttons.GetLength(1)-1))
            {
                
                if (Buttons[pos_x, pos_y].ReturnMine())
                {
                    Buttons[pos_x, pos_y].Mine = false;
                }
            }
        }

        public void FirstTouch(MineButton click)
        {
            //Tyhjennetään 3x3 ruudukko

            MineClear(click.Location.X/dividePixels, click.Location.Y/dividePixels);
            MineClear(click.Location.X/dividePixels +1, click.Location.Y/dividePixels);
            MineClear(click.Location.X/dividePixels -1, click.Location.Y/dividePixels);
            MineClear(click.Location.X/dividePixels, click.Location.Y/dividePixels+1);
            MineClear(click.Location.X/dividePixels, click.Location.Y/dividePixels-1);
            MineClear(click.Location.X/dividePixels +1, click.Location.Y/dividePixels+1);
            MineClear(click.Location.X/dividePixels +1, click.Location.Y/dividePixels-1);
            MineClear(click.Location.X/dividePixels -1, click.Location.Y/dividePixels+1);
            MineClear(click.Location.X/dividePixels -1, click.Location.Y/dividePixels-1);
            click.NumberReveal();
            start.firstTouch = false;

        }

        public void LeftClick(MineButton click)
        {
            //Jos ei lippu
            if (!click.ReturnFlagged())
            {
                if (start.firstTouch)
                {
                    FirstTouch(click);
                }
                //Jos on miina
                if (click.ReturnMine())
                {
                    click.BackColor = Color.Black;

                    //Kysytään pelataanko uudelleen
                    DialogResult result = MessageBox.Show("Hävisit\nHaluatko hävitä uudelleen?", "Pommi!", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Application.Restart();
                        Environment.Exit(0);
                        Cursor.Current = Cursors.WaitCursor;
                    }
                    else { Environment.Exit(0); }

                }
                //Jos ei ole miina
                else if (!click.ReturnMine())
                {
                    
                    PlayArea.MineCheck(click.Location.X/dividePixels, click.Location.Y/dividePixels);
                    //Click.BackColor = Color.White;
                }
            }
        }

        public void VictoryCheck(MineButton click)
        {
            bool check = true;
            if (click.ReturnMine())
            {
                foreach(MineButton mine in PlayArea.ReturnsButtons())
                {
                    if (mine.ReturnMine())
                    {
                        if (!mine.ReturnFlagged())
                        {
                            check = false;
                        }
                    }
                         
                }
                if (check)
                {
                    DialogResult result = MessageBox.Show("Voitit\nHaluatko pelata uudelleen?", "Voitto!", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        Application.Restart();
                        Environment.Exit(0);
                        Cursor.Current = Cursors.WaitCursor;
                    }
                    else { Environment.Exit(0); }
                }
            }
        }

        public void RightClick(MineButton click)
        {
            //Jos lippu
            if (click.ReturnFlagged())
            {
                click.BackColor = Color.Green;
                click.Flagged = false;


            }
            //Jos ei lippu
            else
            {
                if (!click.ReturnOpen())
                {
                    click.BackColor = Color.Orange;
                    click.Flagged = true;
                    VictoryCheck(click);
                }
            }
        }

        public void MineClick(object sender, MouseEventArgs e)
        {
            //objectista minebutton
            MineButton click = (MineButton)sender;

            //jos hiiren vasen
            if (e.Button == MouseButtons.Left)
            {
                click.LeftClick(click);
                
            }
            //Hiirne oikea
            else if (e.Button == MouseButtons.Right)
            {
                click.RightClick(click);
                
            }

        }
    }
}
