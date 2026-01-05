
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chess
{
    public partial class FormStartDialog : Form
    {
        public enum PlayerColor { White, Black }

        public PlayerColor SelectedColor { get; private set; }
        public int SelectedDifficulty { get; private set; }

        private RadioButton radioWhite;
        private RadioButton radioBlack;
        private TrackBar trackBar1;
        private Label labelStrength;
        private Button btnOk;

        public FormStartDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            // === FORM ===
            this.Text = "Ustawienia";
            this.StartPosition = FormStartPosition.CenterParent;
            this.Padding = new Padding(12);
            // this.MinimumSize = new Size(260, 240);
            this.Size = new Size(260, 240);
            // === GŁÓWNY LAYOUT ===
            var layout = new TableLayoutPanel();
            layout.RowCount = 4;
            layout.ColumnCount = 1;
            layout.AutoSize = true;
            layout.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            layout.Dock = DockStyle.Fill;
            layout.Padding = new Padding(5);

            // WYŚRODKOWANIE KOLUMN
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            // === PANEL WYBORU KOLORU ===
            var colorPanel = new FlowLayoutPanel();
            colorPanel.FlowDirection = FlowDirection.LeftToRight;
            colorPanel.AutoSize = true;
            colorPanel.Anchor = AnchorStyles.None;

            radioWhite = new RadioButton()
            {
                Text = "Białe",
                Checked = true,
                AutoSize = true
            };

            radioBlack = new RadioButton()
            {
                Text = "Czarne",
                AutoSize = true
            };

            colorPanel.Controls.Add(radioWhite);
            colorPanel.Controls.Add(radioBlack);

            // === TRACKBAR ===
            trackBar1 = new TrackBar();
            trackBar1.Minimum = 1000;
            trackBar1.Maximum = 2400;
            trackBar1.Value = 1000;
            trackBar1.TickFrequency = 100;
            trackBar1.SmallChange = 100;
            trackBar1.LargeChange = 100;
            trackBar1.AutoSize = true;
            trackBar1.Anchor = AnchorStyles.None;
            trackBar1.Scroll += trackBar1_Scroll;

            // === LABEL SIŁY ===
            labelStrength = new Label();
            labelStrength.AutoSize = true;
            labelStrength.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            labelStrength.Anchor = AnchorStyles.None;

            // === PRZYCISK OK ===
            btnOk = new Button();
            btnOk.Text = "OK";
            btnOk.AutoSize = true;
            btnOk.Padding = new Padding(6);
            btnOk.Anchor = AnchorStyles.None;
            btnOk.Click += BtnOk_Click;

            // === DODAJEMY DO LAYOUTU ===
            layout.Controls.Add(colorPanel);
            layout.Controls.Add(trackBar1);
            layout.Controls.Add(labelStrength);
            layout.Controls.Add(btnOk);

            this.Controls.Add(layout);

            UpdateStrengthLabel();
        }

        private void UpdateStrengthLabel()
        {
            labelStrength.Text = $"Siła przeciwnika: {trackBar1.Value}";
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            SelectedColor = radioWhite.Checked ? PlayerColor.White : PlayerColor.Black;
            SelectedDifficulty = trackBar1.Value;

            DialogResult = DialogResult.OK;
            Close();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            int v = trackBar1.Value;

            // Zaokrąglenie do najbliższej setki
            int rounded = (int)(Math.Round(v / 100.0) * 100);
            rounded = Math.Max(trackBar1.Minimum, Math.Min(trackBar1.Maximum, rounded));

            trackBar1.Value = rounded;
            UpdateStrengthLabel();
        }
    }
}
