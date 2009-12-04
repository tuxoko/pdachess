using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using pdachess.Properties;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace pdachess
{
    public partial class Form1 : Form
    {

        //private Token pawn = null;
        Graphics g = null;
        private Bitmap chessboard = null;
   /*     private Image pawn_p = null;
        private Image bishop_p = null;
        private Image knight_p = null;
        private Image queen_p = null;
        private Image king_p = null;
        private Image castle_p = null;
        private Image pawn_p_w = null;
        private Image bishop_p_w = null;
        private Image knight_p_w = null;
        private Image queen_p_w = null;
        private Image king_p_w = null;
        private Image castle_p_w = null;*/
        private Image move_l = null;
        private Image move_d = null;
  //      private Image select = null;
        private Bitmap picshow = new Bitmap(480, 480);


        //ways of move
        private Point[] Bishop_move = new Point[28];
        private Point[] Knight_move = new Point[8];
        private Point[] Castle_move = new Point[28];
        private Point[] King_move = new Point[8];

        private Token[] t_map = new Token[64];
        private ArrayList possiblemove = new ArrayList();
        private Token tokenChosed = null;
        private Token enPassant = null;
        private string whosturn = "w";

        private TcpClient _tcpl = null;
        private string gamemode = null;
        private Thread listenthr = null;
        private string username = null;
        private string opponent = null;
        private string myside = null;

        private delegate void InvokeFunction(string []cmd);

        public Form1(string gamemode,string text)
        {
            InitializeComponent();
            this.gamemode = gamemode;
            this.username = text;
            if (gamemode == "Network")
            {
                try
                {
                    IPAddress serverip = IPAddress.Parse("140.112.18.203");
                    IPEndPoint serverhost = new IPEndPoint(serverip, 800);
                    _tcpl = new TcpClient();
                    _tcpl.Connect(serverhost);
                    NetworkStream nets = _tcpl.GetStream();

                    string msg = string.Format("l "+username);
                    byte[] buffer = Encoding.Unicode.GetBytes(msg);
                    nets.Write(buffer,0,buffer.Length);

                    
                    listenthr = new Thread(new ThreadStart(listenThread));
                    listenthr.Start();
                }
                catch
                {
                    MessageBox.Show("連線錯誤");
                    //this.Owner.Show();
                    this.Close();
                }
            }
        }

        private void listenThread()
        {
            while (true)
            {
                byte[] data = new byte[1024];
                NetworkStream nets=_tcpl.GetStream();
                nets.Read(data,0,1024);
                string cmddata = Encoding.Unicode.GetString(data, 0, data.Length).TrimEnd('\0');
                label2.Text = cmddata;
                string []cmd=cmddata.Split();
                switch (cmd[0])
                {
                    case("e"):
                        MessageBox.Show("使用者名稱重複");
                        this.Owner.Show();
                        this.Close();
                        break;
                    case("p"):
                        if (cmd[1] == "1")
                        {
                            myside = "w";
                        }
                        else
                        {
                            myside = "b";
                        }
                        opponent = cmd[2];
                        break;
                    case("play"):
                        this.Invoke(new InvokeFunction(opponentmove), new object[] { cmd });
                        break;
                    case("draw"):
                        if (cmd.Length == 1)
                        {
                            drawgameReply();
                        }
                        else if (cmd[1] == "y")
                        {
                            drawgame();
                        }
                        break;
                    case("gg"):
                        MessageBox.Show("對方棄權了!");
                        win();
                        break;
                    default:
                        break;
                }
            }
        }

        public Form1()
        {
            InitializeComponent(); 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //loading all image files
            setTokenmoves();
            loadImage();
            initTokens();
            drawScreen();

        }
        private void loadImage()
        {
            //resize chessboard to proper size
//            Size picsize = new Size(480, 480);
            chessboard = Properties.Resources.board;
//            chessboard = new Bitmap(chessboard_ori, picsize);
//            chessboard = new Bitmap(chessboard_ori);

            //load all token images

            move_l = Resources.l_p;
            move_d = Resources.d_p;
            

        }
        private void initTokens()
        {
            for (int i = 0; i < 64; i++)
            {
                t_map[i] = null;
            }
            //castle
            t_map[getIndex(0, 0)] = new Castle(0, 0, "b", Resources.Castle_b_l, Resources.Castle_b_d, Resources.Castle_b_l_p, Resources.Castle_b_d_p, Resources.Castle_b_l_s, Resources.Castle_b_d_s);
            t_map[getIndex(7, 0)] = new Castle(7, 0, "b", Resources.Castle_b_l, Resources.Castle_b_d, Resources.Castle_b_l_p, Resources.Castle_b_d_p, Resources.Castle_b_l_s, Resources.Castle_b_d_s);

            t_map[getIndex(0, 7)] = new Castle(0, 7, "w", Resources.Castle_w_l, Resources.Castle_w_d, Resources.Castle_w_l_p, Resources.Castle_w_d_p, Resources.Castle_w_l_s, Resources.Castle_w_d_s);
            t_map[getIndex(7, 7)] = new Castle(7, 7, "w", Resources.Castle_w_l, Resources.Castle_w_d, Resources.Castle_w_l_p, Resources.Castle_w_d_p, Resources.Castle_w_l_s, Resources.Castle_w_d_s);

            //Knight
            t_map[getIndex(1, 0)] = new Knight(1, 0, "b", Resources.Knight_b_l, Resources.Knight_b_d, Resources.Knight_b_l_p, Resources.Knight_b_d_p, Resources.Knight_b_l_s, Resources.Knight_b_d_s);
            t_map[getIndex(6, 0)] = new Knight(6, 0, "b", Resources.Knight_b_l, Resources.Knight_b_d, Resources.Knight_b_l_p, Resources.Knight_b_d_p, Resources.Knight_b_l_s, Resources.Knight_b_d_s);

            t_map[getIndex(1, 7)] = new Knight(1, 7, "w", Resources.Knight_w_l, Resources.Knight_w_d, Resources.Knight_w_l_p, Resources.Knight_w_d_p, Resources.Knight_w_l_s, Resources.Knight_w_d_s);
            t_map[getIndex(6, 7)] = new Knight(6, 7, "w", Resources.Knight_w_l, Resources.Knight_w_d, Resources.Knight_w_l_p, Resources.Knight_w_d_p, Resources.Knight_w_l_s, Resources.Knight_w_d_s);
            //Bishop
            t_map[getIndex(2, 0)] = new Bishop(2, 0, "b", Resources.Bishop_b_l, Resources.Bishop_b_d, Resources.Bishop_b_l_p, Resources.Bishop_b_d_p, Resources.Bishop_b_l_s, Resources.Bishop_b_d_s);
            t_map[getIndex(5, 0)] = new Bishop(5, 0, "b", Resources.Bishop_b_l, Resources.Bishop_b_d, Resources.Bishop_b_l_p, Resources.Bishop_b_d_p, Resources.Bishop_b_l_s, Resources.Bishop_b_d_s);

            t_map[getIndex(2, 7)] = new Bishop(2, 7, "w", Resources.Bishop_w_l, Resources.Bishop_w_d, Resources.Bishop_w_l_p, Resources.Bishop_w_d_p, Resources.Bishop_w_l_s, Resources.Bishop_w_d_s);
            t_map[getIndex(5, 7)] = new Bishop(5, 7, "w", Resources.Bishop_w_l, Resources.Bishop_w_d, Resources.Bishop_w_l_p, Resources.Bishop_w_d_p, Resources.Bishop_w_l_s, Resources.Bishop_w_d_s);

            //Queen
            t_map[getIndex(3, 0)] = new Queen(3, 0, "b", Resources.Queen_b_l, Resources.Queen_b_d, Resources.Queen_b_l_p, Resources.Queen_b_d_p, Resources.Queen_b_l_s, Resources.Queen_b_d_s);

            t_map[getIndex(3, 7)] = new Queen(3, 7, "w", Resources.Queen_w_l, Resources.Queen_w_d, Resources.Queen_w_l_p, Resources.Queen_w_d_p, Resources.Queen_w_l_s, Resources.Queen_w_d_s);

            //King
            t_map[getIndex(4, 0)] = new King(4, 0, "b", Resources.King_b_l, Resources.King_b_d, Resources.King_b_l_p, Resources.King_b_d_p, Resources.King_b_l_s, Resources.King_b_d_s);

            t_map[getIndex(4, 7)] = new King(4, 7, "w", Resources.King_w_l, Resources.King_w_d, Resources.King_w_l_p, Resources.King_w_d_p, Resources.King_w_l_s, Resources.King_w_d_s);

            //Pawn
            for (int i = 0; i < 8; i++)
            {
                t_map[getIndex(i, 1)] = new Pawn(i, 1, "b", Resources.Pawn_b_l, Resources.Pawn_b_d, Resources.Pawn_b_l_p, Resources.Pawn_b_d_p, Resources.Pawn_b_l_s, Resources.Pawn_b_d_s);
                t_map[getIndex(i, 6)] = new Pawn(i, 6, "w", Resources.Pawn_w_l, Resources.Pawn_w_d, Resources.Pawn_w_l_p, Resources.Pawn_w_d_p, Resources.Pawn_w_l_s, Resources.Pawn_w_d_s);
            }

        }

        private int getIndex(int x, int y)
        {
            return x + y * 8;
        }

        private void drawScreen()
        {
         //   try
            {
                //picshow = chessboard.Clone() as Bitmap;
                g = Graphics.FromImage(picshow);
                g.DrawImage(chessboard, 0, 0);

                for (int i = 0; i < 64; i++)
                {
                    try
                    {
                        t_map[i].draw(g);
                    }
                    catch { }
                }

                if (tokenChosed != null)
                {
                    //                g.FillRectangle(Brushes.Cyan, tokenChosed.getX() * 30, tokenChosed.getY() * 30, 30, 30);
                    //g.DrawImage(select, tokenChosed.getX() * 30, tokenChosed.getY() * 30, 30, 30);
                    // g.DrawImage(select, new Rectangle(tokenChosed.getX() * 30, tokenChosed.getY() * 30, 30, 30), new Rectangle(0, 0, 30, 30), GraphicsUnit.Pixel);
                    //g.DrawImage(select, tokenChosed.getX() * 30, tokenChosed.getY() * 30);
                    tokenChosed.draw(g, "s");
                }

                foreach (Point p in possiblemove)
                {
                    //                g.FillRectangle(Brushes.Yellow, p.X * 30, p.Y * 30, 30, 30);
                    //               g.DrawImage(move, p.X * 30, p.Y * 30, 30, 30);
                    //                g.DrawImage(move, new Rectangle(p.X * 30, p.Y * 30, 30, 30), new Rectangle(0, 0, 30, 30), GraphicsUnit.Pixel);
                    if (p.X < 0 || p.X > 7 || p.Y < 0 || p.Y > 7)
                        continue;
                    if (t_map[getIndex(p.X, p.Y)] == null)
                        g.DrawImage(((p.X + p.Y) % 2 == 0) ? move_d : move_l, p.X * 60, p.Y * 60);
                    else
                        t_map[getIndex(p.X, p.Y)].draw(g, "p");
                }



                gamebox.Image = picshow;
                gamebox.Refresh();
            }
        //    catch { }
        }


        private void gamebox_MouseDown(object sender, MouseEventArgs e)
        {
            if(gamemode=="Local" || myside==whosturn)
            {
                int po_x = e.X / (Screen.PrimaryScreen.WorkingArea.Width / 8);
                int po_y = e.Y / (Screen.PrimaryScreen.WorkingArea.Width / 8);
                Point p = new Point(po_x, po_y);
                if (possiblemove.Count != 0 && possiblemove.Contains(p))
                {
                    
                    
                    enPassant = null;
                    string promote=move(t_map, po_x, po_y);

                    if (gamemode == "Network")
                    {
                        int chose_x = tokenChosed.getX();
                        int chose_y = tokenChosed.getY();
                        NetworkStream nets = _tcpl.GetStream();
                        string msg = string.Format("play " + opponent + " " + chose_x.ToString() + " " + chose_y.ToString() + " " + po_x.ToString() + " " + po_y.ToString() + " " + promote);
                        byte[] buffer = new byte[1024];
                        buffer = Encoding.Unicode.GetBytes(msg.ToCharArray());
                        nets.Write(buffer, 0, buffer.Length);
                    }

                    tokenChosed = null;
                    possiblemove.Clear();
                    drawScreen();
                    whosturn = (whosturn == "w") ? "b" : "w";
                    label1.Text = (whosturn == "w") ? "White's turn" : "Black's turn";
                    if(ischeck(t_map,whosturn))
                    {
                        if (ismate(whosturn))
                        {
                            MessageBox.Show("Checkmate!");
                            win();
                        }
                        else
                        {
                            MessageBox.Show("Check!");
                        }
                    }
                    else if (ismate(whosturn))
                    {
                        MessageBox.Show("Stalemate!");
                        drawgame();
                    }
                }
                else if (t_map[getIndex(po_x, po_y)]!=null && t_map[getIndex(po_x, po_y)].isHere(po_x, po_y) && t_map[getIndex(po_x, po_y)].getSide() == whosturn)
                {
                    tokenChosed = t_map[getIndex(po_x, po_y)] as Token;
                    displaymove(t_map[getIndex(po_x, po_y)]);
                    drawScreen();
                }
                else
                {
                    tokenChosed = null;
                    possiblemove.Clear();
                    drawScreen();
                }
            }
        }
        private string move(Token[] map, int po_x, int po_y)
        {
            return move(map, po_x, po_y, "");
        }
        private string move(Token[] map,int po_x, int po_y,string promote)
        {
            enPassant = null;
            Point p = new Point(po_x, po_y);
            //if (possiblemove.Count != 0 && possiblemove.Contains(p))
            {
                //MessageBox.Show("choosed");
                //castling
                if (tokenChosed.getType() == "King" && Math.Abs(p.X - tokenChosed.getX()) == 2)
                {
                    if (p.X < tokenChosed.getX())
                    {
                        map[getIndex(tokenChosed.getX() - 1, tokenChosed.getY())] = map[getIndex(0, tokenChosed.getY())];
                        map[getIndex(0, tokenChosed.getY())] = null;
                        map[getIndex(tokenChosed.getX() - 1, tokenChosed.getY())].setmove(tokenChosed.getX() - 1, tokenChosed.getY());
                    }
                    else
                    {
                        map[getIndex(tokenChosed.getX() + 1, tokenChosed.getY())] = map[getIndex(7, tokenChosed.getY())];
                        map[getIndex(7, tokenChosed.getY())] = null;
                        map[getIndex(tokenChosed.getX() + 1, tokenChosed.getY())].setmove(tokenChosed.getX() + 1, tokenChosed.getY());
                    }
                }

                if (tokenChosed.getType() == "Pawn")
                {
                    //log for en passant
                    if (Math.Abs(tokenChosed.getY() - p.Y) == 2)
                    {
                        enPassant = tokenChosed;
                    }
                    //capture en passant
                    if (tokenChosed.getX() != p.X && map[getIndex(p.X, p.Y)] == null)
                    {
                        map[getIndex(p.X, tokenChosed.getY())] = null;
                    }
                }

                map[getIndex(tokenChosed.getX(), tokenChosed.getY())] = null as Token;
                tokenChosed.setmove(po_x, po_y);
                map[getIndex(tokenChosed.getX(), tokenChosed.getY())] = tokenChosed;

                //promote
                if ((map == t_map) && tokenChosed.getType() == "Pawn" && (tokenChosed.getY() == 0 || tokenChosed.getY() == 7))
                {
                    DialogResult res=new DialogResult();
                    if (promote == "")
                    {
                        res = MessageBox.Show("You can promote your pawn.\nIf you prefer a queen, press \"Yes\".\nIf you prefer a knight, press\"No\".", "Promotion", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                        promote=(res==DialogResult.Yes)?"Queen":"Knight";
                    }
                    if (tokenChosed.getSide() == "b")
                        if (promote=="Queen")
                            map[getIndex(tokenChosed.getX(), tokenChosed.getY())] = new Queen(tokenChosed.getX(), tokenChosed.getY(),
                                                                      tokenChosed.getSide(), Resources.Queen_b_l, Resources.Queen_b_d, Resources.Queen_b_l_p, Resources.Queen_b_d_p, Resources.Queen_b_l_s, Resources.Queen_b_d_s);
                        else
                            map[getIndex(tokenChosed.getX(), tokenChosed.getY())] = new Knight(tokenChosed.getX(), tokenChosed.getY(),
                                                                      tokenChosed.getSide(), Resources.Knight_b_l, Resources.Knight_b_d, Resources.Knight_b_l_p, Resources.Knight_b_d_p, Resources.Knight_b_l_s, Resources.Knight_b_d_s);
                    else
                        if (promote=="Queen")
                            map[getIndex(tokenChosed.getX(), tokenChosed.getY())] = new Queen(tokenChosed.getX(), tokenChosed.getY(), tokenChosed.getSide(), Resources.Queen_w_l, Resources.Queen_w_d, Resources.Queen_w_l_p, Resources.Queen_w_d_p, Resources.Queen_w_l_s, Resources.Queen_w_d_s);
                        else
                            map[getIndex(tokenChosed.getX(), tokenChosed.getY())] = new Knight(tokenChosed.getX(), tokenChosed.getY(), tokenChosed.getSide(), Resources.Knight_w_l, Resources.Knight_w_d, Resources.Knight_w_l_p, Resources.Knight_w_d_p, Resources.Knight_w_l_s, Resources.Knight_w_d_s);
                }
                

            }
            return promote;
        }

        private void displaymove(Token t)
        {
            switch (t.getType())
            {
                case ("Pawn"):
                    movecontrol_Pawn((Pawn)t);
                    break;
                case ("Bishop"):
                    movecontrol_Bishop((Bishop)t);
                    break;
                case ("King"):
                    movecontrol_King((King)t);
                    break;
                case ("Queen"):
                    movecontrol_Queen((Queen)t);
                    break;
                case ("Castle"):
                    movecontrol_Castle((Castle)t);
                    break;
                case ("Knight"):
                    movecontrol_Knight((Knight)t);
                    break;
                default:
                    break;
            }
            test_check();
        }
        private void quit()
        {
            if (gamemode == "Network")
            {
                
                NetworkStream nets = _tcpl.GetStream();
                string msg = string.Format("q ");
                byte[] buffer = new byte[1024];
                buffer = Encoding.Unicode.GetBytes(msg.ToCharArray());
                nets.Write(buffer, 0, buffer.Length);
            }
            if (listenthr != null) listenthr.Abort();
            this.Owner.Show();
            this.Close();
        }
        private void menuItem2_Click(object sender, EventArgs e)
        {
            menuItem4_Click(sender, e);
        }
        private void win()
        {
            MessageBox.Show("您贏了!", "恭喜");
            quit();
        }
        private void lose()
        {
            MessageBox.Show("您輸了!", "真可惜...");
            quit();
        }
        private void drawgame()
        {
            MessageBox.Show("這局不分勝負!", "和局");
            quit();
        }

        private void menuItem4_Click(object sender, EventArgs e)
        {
            DialogResult res=MessageBox.Show("確定要棄權嗎?", "棄權", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (res == DialogResult.Yes)
            {
                if (gamemode == "Network")
                {

                    NetworkStream nets = _tcpl.GetStream();
                    string msg = string.Format("gg " + opponent);
                    byte[] buffer = new byte[1024];
                    buffer = Encoding.Unicode.GetBytes(msg.ToCharArray());
                    nets.Write(buffer, 0, buffer.Length);
                }
                quit();
            }
        }

        private void menuItem3_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show("確定要和局嗎?", "和局", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (res == DialogResult.Yes)
            {
                if (gamemode == "Network")
                {

                    NetworkStream nets = _tcpl.GetStream();
                    string msg = string.Format("draw " + opponent);
                    byte[] buffer = new byte[1024];
                    buffer = Encoding.Unicode.GetBytes(msg.ToCharArray());
                    nets.Write(buffer, 0, buffer.Length);
                }
                else
                {
                    drawgameReply();
                }
            }
        }
        private void drawgameReply()
        {
            DialogResult res = MessageBox.Show("對方要求和局，確定要和局嗎?", "和局", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2);
            if (res == DialogResult.Yes)
            {
                if (gamemode == "Network")
                {

                    NetworkStream nets = _tcpl.GetStream();
                    string msg = string.Format("draw " + opponent + " y");
                    byte[] buffer = new byte[1024];
                    buffer = Encoding.Unicode.GetBytes(msg.ToCharArray());
                    nets.Write(buffer, 0, buffer.Length);
                }
                drawgame();
            }
            else
            {
                if (gamemode == "Network")
                {

                    NetworkStream nets = _tcpl.GetStream();
                    string msg = string.Format("draw " + opponent+" n");
                    byte[] buffer = new byte[1024];
                    buffer = Encoding.Unicode.GetBytes(msg.ToCharArray());
                    nets.Write(buffer, 0, buffer.Length);
                }
            }
        }
    }
}
