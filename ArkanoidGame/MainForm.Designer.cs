namespace ArkanoidGame
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            LostBallsLabel = new Label();
            racket = new Panel();
            ball = new Panel();
            GameTimer = new System.Windows.Forms.Timer(components);
            PauseLabel = new Label();
            ScoreLabel = new Label();
            SuspendLayout();
            // 
            // LostBallsLabel
            // 
            LostBallsLabel.AutoSize = true;
            LostBallsLabel.Location = new Point(12, 29);
            LostBallsLabel.Name = "LostBallsLabel";
            LostBallsLabel.Size = new Size(162, 20);
            LostBallsLabel.TabIndex = 1;
            LostBallsLabel.Text = "Потерянные мячи: 0";
            // 
            // racket
            // 
            racket.BackColor = SystemColors.ControlDarkDark;
            racket.Location = new Point(350, 530);
            racket.Name = "racket";
            racket.Size = new Size(100, 20);
            racket.TabIndex = 2;
            // 
            // ball
            // 
            ball.BackColor = SystemColors.ActiveCaptionText;
            ball.Location = new Point(390, 500);
            ball.Name = "ball";
            ball.Size = new Size(20, 20);
            ball.TabIndex = 0;
            // 
            // GameTimer
            // 
            GameTimer.Enabled = true;
            GameTimer.Interval = 20;
            GameTimer.Tick += GameTimer_Tick;
            // 
            // PauseLabel
            // 
            PauseLabel.AutoSize = true;
            PauseLabel.Location = new Point(724, 530);
            PauseLabel.Name = "PauseLabel";
            PauseLabel.Size = new Size(54, 20);
            PauseLabel.TabIndex = 3;
            PauseLabel.Text = "Пауза";
            PauseLabel.Visible = false;
            // 
            // ScoreLabel
            // 
            ScoreLabel.AutoSize = true;
            ScoreLabel.Location = new Point(12, 9);
            ScoreLabel.Name = "ScoreLabel";
            ScoreLabel.Size = new Size(63, 20);
            ScoreLabel.TabIndex = 4;
            ScoreLabel.Text = "Счет: 0";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            ClientSize = new Size(784, 561);
            Controls.Add(ScoreLabel);
            Controls.Add(PauseLabel);
            Controls.Add(ball);
            Controls.Add(racket);
            Controls.Add(LostBallsLabel);
            Font = new Font("Tw Cen MT Condensed Extra Bold", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            KeyPreview = true;
            Margin = new Padding(3, 4, 3, 4);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Арканоид";
            Load += MainForm_Load;
            KeyDown += MainForm_KeyDown;
            MouseMove += MainForm_MouseMove;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label LostBallsLabel;
        private Panel racket;
        private Panel ball;
        private System.Windows.Forms.Timer GameTimer;
        private Label PauseLabel;
        private Label ScoreLabel;
    }
}
