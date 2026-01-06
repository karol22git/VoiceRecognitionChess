using ChessDotNet;
public class PromotionDialog : Form
{
    public char SelectedPromotionChar { get; private set; }

    public PromotionDialog(Player owner)
    {
        Text = "Promocja pionka";
        Width = 300;
        Height = 120;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        StartPosition = FormStartPosition.CenterParent;

        var queen = new Button { Text = "Hetman", Left = 10, Width = 60, Top = 10 };
        var rook = new Button { Text = "Wieża", Left = 80, Width = 60, Top = 10 };
        var bishop = new Button { Text = "Goniec", Left = 150, Width = 60, Top = 10 };
        var knight = new Button { Text = "Skoczek", Left = 220, Width = 60, Top = 10 };

        queen.Click += (s, e) => { SelectedPromotionChar = 'q'; DialogResult = DialogResult.OK; };
        rook.Click += (s, e) => { SelectedPromotionChar = 'r'; DialogResult = DialogResult.OK; };
        bishop.Click += (s, e) => { SelectedPromotionChar = 'b'; DialogResult = DialogResult.OK; };
        knight.Click += (s, e) => { SelectedPromotionChar = 'n'; DialogResult = DialogResult.OK; };

        Controls.Add(queen);
        Controls.Add(rook);
        Controls.Add(bishop);
        Controls.Add(knight);
    }
}
