using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArkanoidGame
{
    public partial class MainForm : Form
    {
        private int ballSpeedX;
        private int ballSpeedY;
        private int lostBallsCount = 0; // 5.1.2: ������� ���������� �����
        private int score = 0; // 5.5.2: ����
        private bool isGamePaused = false; // 5.3.2: ��������� ���� (�����)

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                CreateBricks(); // 5.1.3: �������� ��������
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(ball.ClientRectangle);
                ball.Region = new Region(path); // 5.1.4: ������� ����� ����
                Random random = new Random();
                ResetBall(random); // 5.1.5: ��������� ��������� ����
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ ��� �������� ����: {ex.Message}");
            }
        }

        private void ResetBall(Random random)
        {
            ball.Location = new Point(random.Next(0, this.ClientSize.Width - ball.Width), this.ClientSize.Height - ball.Height - 50);
            ballSpeedX = random.Next(8, 15) * (random.Next(0, 2) == 0 ? -1 : 1);
            ballSpeedY = -Math.Abs(random.Next(8, 15)); // 5.2.2: ��������� �������� ��������
        }

        private void CreateBricks()
        {
            int brickWidth = 70;
            int brickHeight = 25;
            Color[] colors = { Color.Red, Color.Green, Color.Blue, Color.Orange };
            int totalBricks = 10;
            int startX = (this.ClientSize.Width - (totalBricks * (brickWidth + 5))) / 2;
            int startY = 175;
            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < totalBricks; col++)
                {
                    Panel brick = new Panel
                    {
                        Size = new Size(brickWidth, brickHeight),
                        Location = new Point(startX + col * (brickWidth + 5), startY + row * (brickHeight + 5)),
                        BackColor = colors[row % colors.Length],
                        Tag = row
                    };
                    this.Controls.Add(brick);
                }
            }
        }

        private void MainForm_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                int racketCenterX = racket.Width / 2; // 5.2.1: ����� �������
                if (e.X > racketCenterX && e.X < this.ClientSize.Width - racketCenterX)
                {
                    racket.Location = new Point(e.X - racketCenterX, racket.Top); // 5.2.1: �������� �������
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ ��� ����������� �������: {ex.Message}");
            }
        }

        private bool hasWon = false; // 5.5.1: ���� ������

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                ball.Location = new Point(ball.Location.X + ballSpeedX, ball.Location.Y + ballSpeedY); // 5.2.2: �������� ����

                // 5.4.1: ������ ���� �� ����
                if (ball.Left <= 0 || ball.Right >= this.ClientSize.Width)
                {
                    ballSpeedX = -ballSpeedX;
                }

                if (ball.Top <= 0)
                {
                    ballSpeedY = -ballSpeedY;
                }

                // 5.4.3: ������ ����
                if (ball.Bottom >= this.ClientSize.Height)
                {
                    GameTimer.Stop();
                    lostBallsCount++; 
                    LostBallsLabel.Text = $"���������� ����: {lostBallsCount}"; 
                    MessageBox.Show("��� �������!", "��!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ResetBall(new Random()); 
                    GameTimer.Start();
                }

                // 5.4.2: ������ �� �������
                if (ball.Bounds.IntersectsWith(racket.Bounds))
                {
                    if (ballSpeedY > 0)
                    {
                        ballSpeedY = -ballSpeedY;
                        ball.Top = racket.Top - ball.Height;
                    }
                }

                CheckBrickCollisions(); 

                if (!AreBricksLeft() && !hasWon) // 5.5.1: �������� �� ������
                {
                    hasWon = true;
                    GameTimer.Stop();
                    MessageBox.Show("�����������! �� ��������!", "���� ��������", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ �� ����� ����: {ex.Message}");
            }
        }

        private void CheckBrickCollisions()
        {
            try
            {
                Rectangle ballRectangle = ball.Bounds; // ������������� ������� ����
                bool hasBricksCollided = false; 
                for (int i = this.Controls.Count - 1; i >= 0; i--)
                {
                    Control item = this.Controls[i];
                    if (item is Panel && item != ball && item != racket) // 5.5.1: ��������, ��� ������� - ������
                    {
                        if (ballRectangle.IntersectsWith(item.Bounds)) 
                        {
                            Rectangle intersection = Rectangle.Intersect(ballRectangle, item.Bounds);
                            this.Controls.Remove(item); 
                            if (intersection.Height < intersection.Width) 
                            {
                                ballSpeedY = -ballSpeedY; 
                            }
                            else
                            {
                                ballSpeedX = -ballSpeedX; 
                            }

                            if (item.Tag is int row)
                            {
                                score += (4 - row) * 10; 
                                ScoreLabel.Text = $"����: {score}"; 
                            }
                            hasBricksCollided = true;
                        }
                    }
                }
                if (hasBricksCollided)
                {
                    Console.WriteLine("������ ��� ��������!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ ��� �������� ������������ � ���������: {ex.Message}");
            }
        }

        private bool AreBricksLeft()
        {
            foreach (Control control in this.Controls)
            {
                if (control is Panel && control != ball && control != racket) 
                {
                    return true;
                }
            }
            return false;
        }

        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Space) // 5.3.2: �������� ������� �������
                {
                    if (!isGamePaused)
                    {
                        GameTimer.Stop(); 
                        isGamePaused = true; 
                        PauseLabel.Visible = true; 
                    }
                    else
                    {
                        GameTimer.Start(); 
                        isGamePaused = false; 
                        PauseLabel.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"������ ��� ������� �������: {ex.Message}");
            }
        }
    }
}
