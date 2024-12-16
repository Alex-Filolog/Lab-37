using System;
using System.Drawing;
using System.Windows.Forms;

namespace ArkanoidGame
{
    public partial class MainForm : Form
    {
        private int ballSpeedX;
        private int ballSpeedY;
        private int lostBallsCount = 0; // 5.1.2: счетчик потер€нных м€чей
        private int score = 0; // 5.5.2: счет
        private bool isGamePaused = false; // 5.3.2: состо€ние игры (пауза)

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                CreateBricks(); // 5.1.3: создание кирпичей
                System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
                path.AddEllipse(ball.ClientRectangle);
                ball.Region = new Region(path); // 5.1.4: кругла€ форма м€ча
                Random random = new Random();
                ResetBall(random); // 5.1.5: начальное положение м€ча
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ќшибка при загрузке игры: {ex.Message}");
            }
        }

        private void ResetBall(Random random)
        {
            ball.Location = new Point(random.Next(0, this.ClientSize.Width - ball.Width), this.ClientSize.Height - ball.Height - 50);
            ballSpeedX = random.Next(8, 15) * (random.Next(0, 2) == 0 ? -1 : 1);
            ballSpeedY = -Math.Abs(random.Next(8, 15)); // 5.2.2: начальные значени€ скорости
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
                int racketCenterX = racket.Width / 2; // 5.2.1: центр ракетки
                if (e.X > racketCenterX && e.X < this.ClientSize.Width - racketCenterX)
                {
                    racket.Location = new Point(e.X - racketCenterX, racket.Top); // 5.2.1: движение ракетки
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ќшибка при перемещении ракетки: {ex.Message}");
            }
        }

        private bool hasWon = false; // 5.5.1: флаг победы

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                ball.Location = new Point(ball.Location.X + ballSpeedX, ball.Location.Y + ballSpeedY); // 5.2.2: движение м€ча

                // 5.4.1: отскок м€ча от стен
                if (ball.Left <= 0 || ball.Right >= this.ClientSize.Width)
                {
                    ballSpeedX = -ballSpeedX;
                }

                if (ball.Top <= 0)
                {
                    ballSpeedY = -ballSpeedY;
                }

                // 5.4.3: потер€ м€ча
                if (ball.Bottom >= this.ClientSize.Height)
                {
                    GameTimer.Stop();
                    lostBallsCount++; 
                    LostBallsLabel.Text = $"ѕотер€нные м€чи: {lostBallsCount}"; 
                    MessageBox.Show("ћ€ч потер€н!", "ќй!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    ResetBall(new Random()); 
                    GameTimer.Start();
                }

                // 5.4.2: отскок от ракетки
                if (ball.Bounds.IntersectsWith(racket.Bounds))
                {
                    if (ballSpeedY > 0)
                    {
                        ballSpeedY = -ballSpeedY;
                        ball.Top = racket.Top - ball.Height;
                    }
                }

                CheckBrickCollisions(); 

                if (!AreBricksLeft() && !hasWon) // 5.5.1: проверка на победу
                {
                    hasWon = true;
                    GameTimer.Stop();
                    MessageBox.Show("ѕоздравл€ем! ¬ы победили!", "»гра окончена", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"ќшибка во врем€ игры: {ex.Message}");
            }
        }

        private void CheckBrickCollisions()
        {
            try
            {
                Rectangle ballRectangle = ball.Bounds; // пр€моугольна€ область м€ча
                bool hasBricksCollided = false; 
                for (int i = this.Controls.Count - 1; i >= 0; i--)
                {
                    Control item = this.Controls[i];
                    if (item is Panel && item != ball && item != racket) // 5.5.1: проверка, что элемент - кирпич
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
                                ScoreLabel.Text = $"—чет: {score}"; 
                            }
                            hasBricksCollided = true;
                        }
                    }
                }
                if (hasBricksCollided)
                {
                    Console.WriteLine(" ирпич был разрушен!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"ќшибка при проверке столкновений с кирпичами: {ex.Message}");
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
                if (e.KeyCode == Keys.Space) // 5.3.2: проверка нажати€ пробела
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
                MessageBox.Show($"ќшибка при нажатии клавиши: {ex.Message}");
            }
        }
    }
}
