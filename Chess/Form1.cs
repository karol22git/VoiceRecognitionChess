namespace Chess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.AutoScaleMode = AutoScaleMode.None;
            InitializeComponent();
            InitializeBoardView();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
        public void InitializeBoardView()
        {
            bView = new BoardView(boardViewPosition);
            Controls.Add(bView);
        }
        private Point boardViewPosition = new Point(20, 20);
        private BoardView bView;

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
