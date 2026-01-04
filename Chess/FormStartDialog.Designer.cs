namespace Chess
{
    partial class FormStartDialog
    {
        private System.ComponentModel.IContainer components = null;

        private RadioButton radioWhite;
        private RadioButton radioBlack;
        private ComboBox comboDifficulty;
        private Button btnOk;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.radioWhite = new RadioButton();
            this.radioBlack = new RadioButton();
            this.comboDifficulty = new ComboBox();
            this.btnOk = new Button();

            this.SuspendLayout();

            radioWhite.Text = "Białe";
            radioWhite.Location = new Point(20, 20);

            radioBlack.Text = "Czarne";
            radioBlack.Location = new Point(20, 50);

            comboDifficulty.Location = new Point(20, 90);
            comboDifficulty.Width = 150;

            btnOk.Text = "OK";
            btnOk.Location = new Point(20, 140);
            btnOk.Click += btnOk_Click;

            this.Controls.Add(radioWhite);
            this.Controls.Add(radioBlack);
            this.Controls.Add(comboDifficulty);
            this.Controls.Add(btnOk);

            this.ClientSize = new Size(220, 200);
            this.Text = "Ustawienia gry";

            this.ResumeLayout(false);
        }
    }
}
