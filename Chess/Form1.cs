using System.Drawing.Drawing2D;
using UiPiece = Chess.Piece;
namespace Chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.AutoScaleMode = AutoScaleMode.None;
            InitializeComponent();
          //  using (var dlg = new FormStartDialog()) { if (dlg.ShowDialog() != DialogResult.OK) { Close(); return; } playerColor = dlg.SelectedColor; difficulty = dlg.SelectedDifficulty; }
            InitializeBoardView();
            InitializeGraveyards();
            InitializeClocks();
            InitializeHistory();
            InitializeRightContainer();
            InitializeContainerForButtons();
            InitializeMainContainer();
            FitComponents();
            bView.GameEnded += () => ResetGame();

            bView.MoveMade += OnMoveMade;
            bView.PieceCaptured += OnPieceCaptured;
            timer1.Interval = 1000;
            timer1.Tick += TimerTick;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = true; // opcjonalnie
          


        }

        public void InitializeMainContainer()
        {
            mainContainer = new TableLayoutPanel();
            mainContainer.Dock = DockStyle.Fill;
            mainContainer.ColumnCount = 2;

            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            mainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
            Controls.Add(mainContainer);
        }

        public void FitComponents()
        {

            rightContainer.Controls.Add(oppGraveyardPanel);
            rightContainer.Controls.Add(oppLabel);
            rightContainer.Controls.Add(history);
            rightContainer.Controls.Add(myLabel);
            rightContainer.Controls.Add(myGraveyardPanel);
            rightContainer.Controls.Add(containerForButtons);

            mainContainer.Controls.Add(bView, 0, 0);
            mainContainer.Controls.Add(rightContainer, 1, 0);

        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
        //public void InitializeBoardView()
        //{
        //
        //    bView = new BoardView(boardViewPosition);
        //}
        public void InitializeBoardView()
        {
            board = new Board();
            bView = new BoardView(boardViewPosition);
            bView.Board = board;
            using (var dlg = new FormStartDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    playerColor = dlg.SelectedColor;
                    bView.IsWhiteOrientation = (playerColor == FormStartDialog.PlayerColor.White);
                }
            }
            //  bView.IsWhiteOrientation = (playerColor == FormStartDialog.PlayerColor.White);
            //bView.IsWhiteOrientation = true; // czarne na dole bView.RebuildBoard();
            bView.Invalidate();
        }
        private void OnMoveMade(string notation, bool whiteMoved)
        {
            history.AddMove(notation, whiteMoved);
        }

        public void InitializeRightContainer()
        {
            rightContainer = new FlowLayoutPanel();
            rightContainer.FlowDirection = FlowDirection.TopDown;
            rightContainer.WrapContents = false;
            rightContainer.AutoScroll = true;
            rightContainer.AutoSize = true;
            rightContainer.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            rightContainer.Margin = new Padding(
                left: 40,     // odleg³oœæ od lewej krawêdzi
                top: 85,      // odleg³oœæ od góry
                right: 5,    // odleg³oœæ od prawej krawêdzi
                bottom: 105    // odleg³oœæ od do³u
            );

        }



        public void InitializeContainerForButtons()
        {
            containerForButtons = new FlowLayoutPanel();
            containerForButtons.FlowDirection = FlowDirection.LeftToRight;
            containerForButtons.WrapContents = false;
            containerForButtons.AutoScroll = true;

            containerForButtons.AutoSize = true;
            containerForButtons.AutoSizeMode = AutoSizeMode.GrowAndShrink;

            containerForButtons.MaximumSize = new Size(280, 0);
            containerForButtons.MinimumSize = new Size(280, 40);

            containerForButtons.Controls.Add(button1);
            containerForButtons.Controls.Add(button2);
        }

        public void InitializeGraveyards()
        {
            oppGraveyardPanel = new GraveyardPanel(oppGraveyardPanelPosition);
            myGraveyardPanel = new GraveyardPanel(myGraveyardPanelPosition);
        }

        public void InitializeClocks()
        {
            whiteClock = new ChessClock(300, 2); // 5 min + 2 sek
            blackClock = new ChessClock(300, 2);

            myLabel = new Label();
            myLabel.Text = "05:00";
            myLabel.Font = new Font("Consolas", 24);
            myLabel.AutoSize = true;
            myLabel.ForeColor = Color.White;
            myLabel.BackColor = Color.Black;
            myLabel.Location = myLabelPosition;


            oppLabel = new Label();
            oppLabel.Text = "05:00";
            oppLabel.Font = new Font("Consolas", 24);
            oppLabel.AutoSize = true;
            oppLabel.ForeColor = Color.White;
            oppLabel.BackColor = Color.Black;
            oppLabel.Location = oppLabelPosition;


            whiteClock.TimeChanged += t => myLabel.Text = FormatTime(t);
            blackClock.TimeChanged += t => oppLabel.Text = FormatTime(t);
            whiteClock.TimeExpired += () => MessageBox.Show("Bia³y przegra³ na czas");
            blackClock.TimeExpired += () => MessageBox.Show("Czarny przegra³ na czas");



        }
        public void InitializeHistory()
        {
            history = new MoveHistoryView(moveHistoryViewPosition);
        }
        private void TimerTick(object? sender, EventArgs e)
        {
            if (whiteToMove)
                whiteClock.Tick();
            else
                blackClock.Tick();
        }

        private Point boardViewPosition = new Point(20, 20);
        private Point oppGraveyardPanelPosition = new Point(780,20);
        private Point myGraveyardPanelPosition = new Point(780, 540);
        private Point oppLabelPosition = new Point(800, 160);
        private Point myLabelPosition = new Point(800, 460);
        private Point moveHistoryViewPosition = new Point(800, 220);


        private BoardView bView;
        private GraveyardPanel myGraveyardPanel;
        private GraveyardPanel oppGraveyardPanel;
        private FlowLayoutPanel rightContainer;
        private FlowLayoutPanel containerForButtons;
        private MoveHistoryView history;
        private ChessClock whiteClock;
        private ChessClock blackClock;

        private Label oppLabel;
        private Label myLabel;

        private Panel rightWrapper;
        private TableLayoutPanel mainContainer;

        private Board board;

        private bool whiteToMove = true;

        private FormStartDialog.PlayerColor playerColor;
        private int difficulty;

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }
        private string FormatTime(int seconds)
        {
            int m = seconds / 60;
            int s = seconds % 60;
            return $"{m:00}:{s:00}";
        }

        private void OnPieceCaptured(UiPiece piece)
        {
            if (IsWhitePiece(piece))
                oppGraveyardPanel.AddPiece(piece); // czarne zbily bia³¹ figurê
            else
                myGraveyardPanel.AddPiece(piece); // bia³e zbily czarn¹ figurê
        }
        private bool IsWhitePiece(UiPiece p)
        {
            return p == UiPiece.WhitePawn ||
                   p == UiPiece.WhiteKnight ||
                   p == UiPiece.WhiteBishop ||
                   p == UiPiece.WhiteRook ||
                   p == UiPiece.WhiteQueen ||
                   p == UiPiece.WhiteKing;
        }
        private void ResetGame()
        {
            // 1. Reset logiki gry
            bView.RestartGame();

            // 3. Reset historii ruchów
            history.Clear();

            // 4. Reset cmentarzy
            myGraveyardPanel.Clear();
            oppGraveyardPanel.Clear();

            // 5. Odœwie¿ widok
            bView.Invalidate();
        }

    }
}
