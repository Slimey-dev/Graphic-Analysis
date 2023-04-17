using System.Numerics;

namespace GraphicAnalysis;

public partial class Form1 : Form
{
    public enum SelectSelect
    {
        None,
        Polygon,
        Line
    }

    private readonly Button _button1;
    private readonly Label _debug;
    private readonly PictureBox _pictureBox1;
    private readonly TextBox _textBox1;
    private SelectSelect _state;
    private Point endPoint = new(-1, -1);
    private float meterLenght;
    private Point startPoint = new(-1, -1);


    public Form1()
    {
        InitializeComponent();

        _pictureBox1 = new PictureBox();
        _button1 = new Button();
        _textBox1 = new TextBox();
        _debug = new Label();


        _pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
        _pictureBox1.Location = new Point(20, 50);
        _pictureBox1.Size = new Size(600, 530);
        _pictureBox1.BackColor = Color.Gray;
        _pictureBox1.Paint += _pictureBox1_Paint;
        _pictureBox1.Click += _pictureBox1_Click;

        _button1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _button1.Location = new Point(670, 50);
        _button1.Size = new Size(100, 30);
        _button1.Text = "Vytvořit měřítko";
        _button1.Click += _button1_Click;

        _debug.Location = new Point(100, 30);
        _debug.AutoSize = true;
        _debug.Text = "Debug";

        ClientSize = new Size(800, 600);

        Controls.Add(_pictureBox1);
        Controls.Add(_button1);
        Controls.Add(_textBox1);
        Controls.Add(_debug);
    }

    private void _pictureBox1_Click(object? sender, EventArgs e)
    {
        if (_state == SelectSelect.Line && startPoint.X == -1)
        {
            var cursor = Cursor.Position;
            var pointClick = _pictureBox1.PointToClient(new Point(cursor.X, cursor.Y));
            startPoint = pointClick;
        }

        else if (_state == SelectSelect.Line && endPoint.X == -1 && startPoint.X != -1)
        {
            var cursor = Cursor.Position;
            var pointClick = _pictureBox1.PointToClient(new Point(cursor.X, cursor.Y));
            endPoint = pointClick;
        }

        var start = new Vector2(startPoint.X, startPoint.Y);
        var end = new Vector2(endPoint.X, endPoint.Y);

        if (endPoint.X != -1)
        {
            meterLenght = Vector2.Distance(start, end);
            _debug.Text = Convert.ToString(meterLenght);
        }

        _pictureBox1.Invalidate();
    }

    private void _pictureBox1_Paint(object? sender, PaintEventArgs e)
    {
        var graphics = e.Graphics;
        var pen = new Pen(Color.Aqua);
        if (_state == SelectSelect.Line && endPoint.X != -1) graphics.DrawLine(pen, startPoint, endPoint);
    }


    private void _button1_Click(object? sender, EventArgs e)
    {
        _state = SelectSelect.Line;
    }
}