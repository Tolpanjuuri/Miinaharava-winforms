using System.Collections.Generic;
using System.Runtime.CompilerServices;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Miinaharava
{
    public partial class start : Form
    {
        public static bool firstTouch = true;
        public start()
        {
            InitializeComponent();
        }
        //Luodaan Playarea, jotta voidaan kayttaa muualla
        PlayArea playArea;

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }
       
        private void CreateDynamicPlayfield(int xSize, int ySize,int minePercent)
        {
            //Muutetaan Formin kokoa
            int pixelGap = Convert.ToInt32(nudMineSize.Value);
            int yPlus = 20;
            int xPlus = 50;
            this.Size = new Size(xSize*pixelGap+yPlus, ySize*pixelGap+xPlus);

            //Luodaan Pelikentta
            playArea = new PlayArea(xSize, ySize);
            playArea.CreatePlayArea(xSize, ySize, pixelGap, minePercent);
            DrawPlayField();
            MineButton.ChangePixelGap(pixelGap);
        }

        private void DrawPlayField()
        {
            //Piirret‰‰n jokainen nappi
            foreach (MineButton button in PlayArea.ReturnsButtons())
            {
                Controls.Add(button);
            }
        }

        private void RemoveUserInput() {
            //poistetaan kysymykset
            userInput.Dispose();
        }

        private void btnClick(int xSize, int ySize, int mineCount)
        {
            //vaihdetaan kursori
            Cursor.Current = Cursors.WaitCursor;
            //poistetaan kysymykset
            RemoveUserInput();
            //luodaan peli
            CreateDynamicPlayfield(xSize, ySize, mineCount);
            Cursor.Current = Cursors.Default;
        }

        //Eri pelitilanteet
        private void btnEasy_Click(object sender, EventArgs e)
        {
            btnClick(12,12,7);
        }

        private void btnNormai_Click(object sender, EventArgs e)
        {
           btnClick(14, 14, 15);
        }

        private void btnHard_Click(object sender, EventArgs e)
        {
            btnClick(16, 16, 25);
        }

        private void btnExtraHard_Click(object sender, EventArgs e)
        {
            btnClick(20, 20, 35);
        }

        private void btnStartCustom_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            RemoveUserInput();
            //tarkistetaan, ett‰ ruudukko ei ole liian pieni
            if (nudXWidth.Value < 3)
            {
                MessageBox.Show("Leveys ei voi olla pienempi kuin 3");
                Application.Restart();
                Environment.Exit(0);
                Cursor.Current = Cursors.WaitCursor;
            }
            else if(nudYLenght.Value < 3)
            {
                MessageBox.Show("Korkeus ei voi olla pienempi kuin 3");
                Application.Restart();
                Environment.Exit(0);
                Cursor.Current = Cursors.WaitCursor;
            }
            //varmistetaan, ett‰ miina prosentit eiv‰t men yli
            else if(nudMinePercent.Value <= 0 || nudMinePercent.Value >= 100)
            {
                MessageBox.Show("Miinojen m‰‰r‰ pit‰‰ olla v‰lilt‰ 1-99");
                Application.Restart();
                Environment.Exit(0);
                Cursor.Current = Cursors.WaitCursor;
            }
            else
            {
                //luodaan peli
                CreateDynamicPlayfield(Convert.ToInt32(nudXWidth.Value), Convert.ToInt32(nudYLenght.Value), Convert.ToInt32(nudMinePercent.Value));
                Cursor.Current = Cursors.Default;
            }

        }

        private void nudXWidth_ValueChanged(object sender, EventArgs e)
        {

        }
    }
}