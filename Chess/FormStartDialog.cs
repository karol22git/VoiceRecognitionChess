
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Chess
{
    public class FormStartDialog : Form
    {
        public enum PlayerColor { White, Black }

        public PlayerColor SelectedColor { get; private set; }
        public int SelectedDifficulty { get; private set; }
        public int SelectedTimeMinutes { get; private set; }
        public int SelectedIncrement { get; private set; }

        private RadioButton radioWhite;
        private RadioButton radioBlack;
        private TrackBar difficultyBar;
        private Label difficultyLabel;
        private NumericUpDown timeControl;
        private NumericUpDown incrementControl;
        private Button btnOk;

        public FormStartDialog()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Game settings";
            StartPosition = FormStartPosition.CenterParent;
            ClientSize = new Size(320, 320); // Mniejsze okno
            Padding = new Padding(15);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            // Główny panel
            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 8,
                AutoSize = true,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 100F) }
            };

            // Funkcja pomocnicza do tworzenia wyśrodkowanych etykiet
            Label CreateCenteredLabel(string text, Font font = null, int topMargin = 5, int bottomMargin = 3)
            {
                return new Label
                {
                    Text = text,
                    Font = font ?? new Font("Segoe UI", 9),
                    AutoSize = true,
                    TextAlign = ContentAlignment.MiddleCenter,
                    Anchor = AnchorStyles.None,
                    Margin = new Padding(0, topMargin, 0, bottomMargin)
                };
            }

            // Funkcja pomocnicza dla kontrolek
            Control CreateCenteredControl(Control control, int topMargin = 3, int bottomMargin = 8)
            {
                control.Anchor = AnchorStyles.None;
                control.Margin = new Padding(0, topMargin, 0, bottomMargin);
                return control;
            }

            // -----------------------------
            // SELECT COLOR
            // -----------------------------
            var colorLabel = CreateCenteredLabel("Select color:", new Font("Segoe UI", 10, FontStyle.Bold), 0, 2);

            var colorPanel = new FlowLayoutPanel
            {
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                Anchor = AnchorStyles.None,
                Margin = new Padding(0, 0, 0, 8)
            };

            radioWhite = new RadioButton
            {
                Text = "White",
                Checked = true,
                AutoSize = true,
                Margin = new Padding(15, 0, 15, 0)
            };
            radioBlack = new RadioButton
            {
                Text = "Black",
                AutoSize = true,
                Margin = new Padding(15, 0, 15, 0)
            };

            colorPanel.Controls.Add(radioWhite);
            colorPanel.Controls.Add(radioBlack);

            // -----------------------------
            // ENGINE STRENGTH
            // -----------------------------
            difficultyLabel = CreateCenteredLabel("Engine strength: 1000", topMargin: 8, bottomMargin: 2);

            difficultyBar = new TrackBar
            {
                Minimum = 1000,
                Maximum = 2400,
                TickFrequency = 100,
                Value = 1000,
                Width = 220,
                Height = 45 // Mniejsza wysokość
            };
            CreateCenteredControl(difficultyBar, 0, 8);

            difficultyBar.Scroll += (s, e) =>
            {
                int rounded = (int)(Math.Round(difficultyBar.Value / 100.0) * 100);
                difficultyBar.Value = rounded;
                difficultyLabel.Text = $"Engine strength: {rounded}";
            };

            // -----------------------------
            // TIME CONTROL
            // -----------------------------
            var timeLabel = CreateCenteredLabel("Time (minutes):", topMargin: 8, bottomMargin: 2);

            timeControl = new NumericUpDown
            {
                Minimum = 1,
                Maximum = 60,
                Value = 5,
                Width = 70,
                Height = 23
            };
            CreateCenteredControl(timeControl, 0, 8);
            timeControl.TextAlign = HorizontalAlignment.Center;

            // -----------------------------
            // INCREMENT
            // -----------------------------
            var incrementLabel = CreateCenteredLabel("Increment (seconds):", topMargin: 8, bottomMargin: 2);

            incrementControl = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 30,
                Value = 2,
                Width = 70,
                Height = 23
            };
            CreateCenteredControl(incrementControl, 0, 10);
            incrementControl.TextAlign = HorizontalAlignment.Center;

            // -----------------------------
            // OK BUTTON
            // -----------------------------
            btnOk = new Button
            {
                Text = "OK",
                Size = new Size(90, 32),
                Font = new Font("Segoe UI", 9)
            };
            CreateCenteredControl(btnOk, 5, 0);
            btnOk.Click += BtnOk_Click;

            // -----------------------------
            // DODAWANIE DO PANELU
            // -----------------------------
            mainPanel.Controls.Add(colorLabel);
            mainPanel.Controls.Add(colorPanel);
            mainPanel.Controls.Add(difficultyLabel);
            mainPanel.Controls.Add(difficultyBar);
            mainPanel.Controls.Add(timeLabel);
            mainPanel.Controls.Add(timeControl);
            mainPanel.Controls.Add(incrementLabel);
            mainPanel.Controls.Add(incrementControl);
            mainPanel.Controls.Add(btnOk);

            // Ustawianie pozycji wierszy - wszystkie AutoSize
            for (int i = 0; i < mainPanel.RowCount; i++)
            {
                mainPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            }

            Controls.Add(mainPanel);
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            SelectedColor = radioWhite.Checked ? PlayerColor.White : PlayerColor.Black;
            SelectedDifficulty = difficultyBar.Value;
            SelectedTimeMinutes = (int)timeControl.Value;
            SelectedIncrement = (int)incrementControl.Value;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}