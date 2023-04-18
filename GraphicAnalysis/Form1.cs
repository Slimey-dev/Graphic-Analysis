using System.ComponentModel;
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

    private readonly Label _debug;
    private readonly FileDialog _fileDialog;
    private readonly Button _fileSelect;
    private readonly PictureBox _pictureBox1;
    private readonly Button _resetMeter;

    private readonly Button _setMeter;
    private readonly TextBox _textBox1;
    private SelectSelect _state;
    private float counter;
    private Point endPoint = new(-1, -1);
    private float koeficient;
    private float meterLenght;
    private Point startPoint = new(-1, -1);


    public Form1()
    {
        InitializeComponent();

        _pictureBox1 = new PictureBox();
        _setMeter = new Button();
        _resetMeter = new Button();
        _textBox1 = new TextBox();
        _fileDialog = new OpenFileDialog();
        _fileSelect = new Button();
        _debug = new Label();


        _pictureBox1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right;
        _pictureBox1.Location = new Point(20, 50);
        _pictureBox1.Size = new Size(600, 530);
        _pictureBox1.BackColor = Color.Gray;
        _pictureBox1.Paint += pictureBox1OnPaint;
        _pictureBox1.Click += pictureBox1OnClick;

        _setMeter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _setMeter.Location = new Point(670, 50);
        _setMeter.Size = new Size(100, 30);
        _setMeter.Text = "Vytvořit měřítko";
        _setMeter.Click += SetMeterClick;

        _resetMeter.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _resetMeter.Location = new Point(670, 80);
        _resetMeter.Size = new Size(100, 30);
        _resetMeter.Text = "Reset";
        _resetMeter.Click += ResetMeterClick;

        _fileSelect.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _fileSelect.Location = new Point(670, 120);
        _fileSelect.Size = new Size(100, 30);
        _fileSelect.Text = "Select Image";
        _fileSelect.Click += FileSelectOnClick;

        _textBox1.Text = "1";
        _textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _textBox1.Location = new Point(670, 180);
        _textBox1.Size = new Size(100, 30);

        _fileDialog.FileOk += FileDialogOnFileOk;


        _debug.Location = new Point(100, 30);
        _debug.AutoSize = true;
        _debug.Text = "Debug";

        ClientSize = new Size(800, 600);
        WindowState = FormWindowState.Maximized;


        Controls.Add(_pictureBox1);
        Controls.Add(_setMeter);
        Controls.Add(_textBox1);
        Controls.Add(_debug);
        Controls.Add(_resetMeter);
        Controls.Add(_fileSelect);
    }

    private void FileDialogOnFileOk(object? sender, CancelEventArgs e)
    {
        var filename = _fileDialog.FileName;
        _pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        _pictureBox1.ImageLocation = filename;
    }

    private void FileSelectOnClick(object? sender, EventArgs e)
    {
        _fileDialog.Filter = "Images| *.JPG;*.PNG;*.JPEG;*.BMP";
        _fileDialog.ShowDialog();
    }

    private void ResetMeterClick(object? sender, EventArgs e)
    {
        _state = SelectSelect.None;
        var resetPoint = new Point(-1, -1);
        startPoint = endPoint = resetPoint;
        _pictureBox1.Invalidate();
    }

    private void pictureBox1OnClick(object? sender, EventArgs e)
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

        if (_textBox1.Text != "1")
        {
            counter = Convert.ToInt32(_textBox1.Text);
            koeficient = meterLenght / counter;
        }

        if (endPoint.X != -1)
        {
            meterLenght = Vector2.Distance(start, end);
            var final = meterLenght / koeficient;
            _debug.Text = Convert.ToString(final);
            //_debug.Text = Convert.ToString(meterLenght);
        }


        _pictureBox1.Invalidate();
    }

    private void pictureBox1OnPaint(object? sender, PaintEventArgs e)
    {
        var graphics = e.Graphics;
        var pen = new Pen(Color.Aqua);
        if (_state == SelectSelect.Line && endPoint.X != -1) graphics.DrawLine(pen, startPoint, endPoint);
    }


    private void SetMeterClick(object? sender, EventArgs e)
    {
        _state = SelectSelect.Line;
    }
}