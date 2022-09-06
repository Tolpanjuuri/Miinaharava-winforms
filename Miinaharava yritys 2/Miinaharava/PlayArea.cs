using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Miinaharava
{
    internal class PlayArea
    {
        //Luodaan array taala, jotta sita voi kayttaa kaikkialla
        public static MineButton[,] Buttons;

        Random random = new Random();

        public static List<Point> checkedButtons = new List<Point>();

        public PlayArea(int x, int y)
        {
            //Maaritetaan arrayn kokoa
            Buttons = new MineButton[x, y];
        }

        public void CreatePlayArea(int xSide, int ySide,int pixelGap, int minePercent)
        {
            
            int yPosition = 0;
            int xPosition = 0;

            //Lasketaan nappien maara
            for (int i = 0; i < (xSide*ySide); i++)
            {
                //Jotta Random olisi Random
                //Thread.Sleep(10);

                //Tarkistetaan ollaanko rivin lopussa
                if (i % xSide == 0 && i != 0)
                {
                    yPosition++;
                    xPosition = 0;
                }

                //Randomoidaan onko nappi miina voi ei
                if (random.Next(0, 100) <= minePercent)
                {
                    //Luodaan nappi ja tehdään siita miina
                    Buttons[xPosition, yPosition] = new MineButton(xPosition*pixelGap, yPosition*pixelGap, i.ToString(), true,pixelGap,pixelGap);
                    Buttons[xPosition, yPosition].Mine = true;

                }
                else
                {
                    //Luodaan nappi
                    Buttons[xPosition, yPosition]=new MineButton(xPosition*pixelGap, yPosition*pixelGap, i.ToString(), false,pixelGap,pixelGap);
                }


                xPosition++;
            }
        
        }

        ////https://www.freecodecamp.org/news/flood-fill-algorithm-explained/
        public static void MineCheck(int pos_x, int pos_y)
        {
            // Jos yli listan tai jo kayty
            if (pos_x < 0 || pos_x > (Buttons.GetLength(0)-1) || pos_y < 0 || pos_y > (Buttons.GetLength(1)-1) || checkedButtons.Contains(new Point(pos_x,pos_y)))
                return;

            //Jos miina peruutetaan
            if (Buttons[pos_x, pos_y].ReturnMine())
            {
                return;
            }

            //Jos ruudun vieressä on miina, ei mennä pidemmälle ja tulostetaan miinojen määrä
            if (Buttons[pos_x, pos_y].minesNear != 0)
            {
                Buttons[pos_x, pos_y].ForeColor = Color.Black;
                Buttons[pos_x, pos_y].BackColor = Color.White;
                Buttons[pos_x, pos_y].Text = Buttons[pos_x, pos_y].minesNear.ToString();
                Buttons[pos_x, pos_y].open = true;
                return;
            }

            //merkataan lapikaydyt
            checkedButtons.Add(new Point(pos_x, pos_y));
            Buttons[pos_x, pos_y].BackColor = Color.White;
            Buttons[pos_x,pos_y].open = true;


            MineCheck(pos_x + 1, pos_y);  
            MineCheck(pos_x - 1, pos_y);  
            MineCheck(pos_x, pos_y + 1);  
            MineCheck(pos_x, pos_y - 1);
            MineCheck(pos_x+1, pos_y - 1);
            MineCheck(pos_x-1, pos_y - 1);
            MineCheck(pos_x+1, pos_y + 1);
            MineCheck(pos_x-1, pos_y + 1);

            return;
            
        }

        //Palautetaan napit
        public static MineButton[,] ReturnsButtons()
        {
            return Buttons;
        }
    }
}
