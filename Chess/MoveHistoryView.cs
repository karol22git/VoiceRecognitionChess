using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess
{
    internal class MoveHistoryView : UserControl
    {
            private ListView listView;
            private int moveNumber = 1;

            public MoveHistoryView(Point p)
            {
                Size = new Size(240, 300);
                Location = new Point(780, 250);
                InitializeUI();
            }

            private void InitializeUI()
            {
                listView = new ListView();
                listView.View = View.Details;
                listView.FullRowSelect = true;
                listView.GridLines = true;
                listView.Scrollable = true;

                listView.Columns.Add("#", 40);
                listView.Columns.Add("White", 100);
                listView.Columns.Add("Black", 100);

                listView.Dock = DockStyle.Fill;

                Controls.Add(listView);
            }

            public void AddMove(string notation, bool whiteToMove)
            {
                if (whiteToMove)
                {
                    // Biały wykonuje ruch → nowy wiersz
                    var item = new ListViewItem(moveNumber.ToString());
                    item.SubItems.Add(notation);
                    item.SubItems.Add(""); // czarny jeszcze nie ruszył
                    listView.Items.Add(item);
                }
                else
                {
                    // Czarny wykonuje ruch → uzupełniamy ostatni wiersz
                    var last = listView.Items[listView.Items.Count - 1];
                    last.SubItems[2].Text = notation;
                    moveNumber++;
                }

                // przewijanie do dołu
                listView.EnsureVisible(listView.Items.Count - 1);
            }
        public void Clear()
        {
            listView.Items.Clear();
            moveNumber = 1;
        }

    }

}

