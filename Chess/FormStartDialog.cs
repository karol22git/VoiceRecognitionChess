using System;
using System.Windows.Forms;

namespace Chess
{
    public partial class FormStartDialog : Form
    {
        public enum PlayerColor { White, Black }

        public PlayerColor SelectedColor { get; private set; }
        public int SelectedDifficulty { get; private set; }

        public FormStartDialog()
        {
            InitializeComponent();
            comboDifficulty.Items.AddRange(new string[] { "Łatwy", "Średni", "Trudny" });
            comboDifficulty.SelectedIndex = 0;
            radioWhite.Checked = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SelectedColor = radioWhite.Checked ? PlayerColor.White : PlayerColor.Black;
            SelectedDifficulty = comboDifficulty.SelectedIndex;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
