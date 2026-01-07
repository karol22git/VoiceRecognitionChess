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
            OrganizeButtons();
            //bView.GameEnded += () => ResetGame();
            bView.GameEnded += (winner, reason) => EndGame(winner, reason);

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
                    gameTimeSeconds = dlg.SelectedTimeMinutes * 60; gameIncrement = dlg.SelectedIncrement;
                }
            }
            //  bView.IsWhiteOrientation = (playerColor == FormStartDialog.PlayerColor.White);
            //bView.IsWhiteOrientation = true; // czarne na dole bView.RebuildBoard();
            bView.Invalidate();
        }
        private void OnMoveMade(string notation, bool whiteMoved)
        {
            history.AddMove(notation, whiteMoved);
            if (!timer1.Enabled) timer1.Start();
            HandleClockAfterMove();
        }
        private void HandleClockAfterMove()
        {
            // Strona, która wykona³a ruch, dostaje inkrement
            if (whiteToMove)
                whiteClock.ApplyIncrement();
            else
                blackClock.ApplyIncrement();

            // Teraz ruch przeciwnika
            whiteToMove = !whiteToMove;

            // Prze³¹cz aktywny zegar
            if (whiteToMove)
            {
                whiteClock.Start();
                blackClock.Stop();
            }
            else
            {
                blackClock.Start();
                whiteClock.Stop();
            }
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


        public void OrganizeButtons()
        {
            button1.Text = "Quit";
            button1.Click += buttonQuit_Click;


            button2.Text = "Surrender";
            button2.Click += buttonSurrender_Click;


        }
        private void buttonSurrender_Click(object sender, EventArgs e)
        {
            var winner = whiteToMove ? GameWinner.Black : GameWinner.White;

            EndGame(winner, GameEndReason.Surrender);
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
            whiteClock = new ChessClock(gameTimeSeconds, gameIncrement);
            blackClock = new ChessClock(gameTimeSeconds, gameIncrement);


            myLabel = new Label();
            myLabel.Text = FormatTime(gameTimeSeconds);
            myLabel.Font = new Font("Consolas", 24);
            myLabel.AutoSize = true;
            myLabel.ForeColor = Color.White;
            myLabel.BackColor = Color.Black;
            myLabel.Location = myLabelPosition;


            oppLabel = new Label();
            oppLabel.Text = FormatTime(gameTimeSeconds);
            oppLabel.Font = new Font("Consolas", 24);
            oppLabel.AutoSize = true;
            oppLabel.ForeColor = Color.White;
            oppLabel.BackColor = Color.Black;
            oppLabel.Location = oppLabelPosition;


            whiteClock.TimeChanged += t => myLabel.Text = FormatTime(t);
            blackClock.TimeChanged += t => oppLabel.Text = FormatTime(t);
            whiteClock.TimeExpired += () => EndGame(GameWinner.Black, GameEndReason.TimeWhiteLost);
            blackClock.TimeExpired += () => EndGame(GameWinner.White, GameEndReason.TimeBlackLost);






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

        private int gameTimeSeconds = 300; // fallback
        private int gameIncrement = 2;

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

            whiteClock.Stop();
            blackClock.Stop();
            timer1.Stop();

            whiteClock = new ChessClock(gameTimeSeconds, gameIncrement);
            blackClock = new ChessClock(gameTimeSeconds, gameIncrement);


            //whiteClock.TimeChanged += t => myLabel.Text = FormatTime(t);
            //blackClock.TimeChanged += t => oppLabel.Text = FormatTime(t);
            whiteClock.TimeChanged += t => myLabel.Text = FormatTime(t);
            blackClock.TimeChanged += t => oppLabel.Text = FormatTime(t);

            whiteClock.TimeExpired += () => EndGame(GameWinner.Black, GameEndReason.TimeWhiteLost);
            blackClock.TimeExpired += () => EndGame(GameWinner.White, GameEndReason.TimeBlackLost);

            myLabel.Text = FormatTime(gameTimeSeconds);
            oppLabel.Text = FormatTime(gameTimeSeconds);


            whiteToMove = true;
            bView.IsGameOver = false;

        }
        private void EndGameOnTime(string message)
        {
            timer1.Stop();
            whiteClock.Stop();
            blackClock.Stop();

            MessageBox.Show(message);

            bView.DisableInteraction(); // jeœli masz tak¹ metodê
        }
       
        private void EndGame(GameWinner winner, GameEndReason reason)
        {
            timer1.Stop();
            whiteClock.Stop();
            blackClock.Stop();

            bView.DisableInteraction();

            string message = reason switch
            {
                GameEndReason.Checkmate =>
                    winner == GameWinner.White ? "White wins (checkmate)" : "Black wins (checkmate)",

                GameEndReason.Stalemate => "Stalemate — draw",
                GameEndReason.Draw => "Draw",
                GameEndReason.TimeWhiteLost => "White loses on time",
                GameEndReason.TimeBlackLost => "Black loses on time",
                GameEndReason.Surrender =>
                    winner == GameWinner.White ? "Black surrenders — White wins" : "White surrenders — Black wins",

                _ => "Game over"
            };

            MessageBox.Show(message, "Game over", MessageBoxButtons.OK, MessageBoxIcon.Information);
            ShowNewGameDialog();
        }

        private void buttonQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void ShowNewGameDialog()
        {
            using (var dlg = new FormStartDialog())
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    playerColor = dlg.SelectedColor;
                    gameTimeSeconds = dlg.SelectedTimeMinutes * 60;
                    gameIncrement = dlg.SelectedIncrement;

                    ResetGame();

                    bView.IsWhiteOrientation = (playerColor == FormStartDialog.PlayerColor.White);
                    InitializeClocks();
                    bView.Invalidate();
                }
            }
        }


    }
}
