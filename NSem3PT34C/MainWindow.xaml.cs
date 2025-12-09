using Microsoft.Win32;
using NSem3PT34.Classes;
using NSem3PT34.Classes.Command;
using NSem3PT34.Classes.Structure;
using NSem3PT34.Classes.Util;
using NSem3PT34.Classes.Visitor;
using NSem3PT34.Classes.VM;
using NSem3PT34C.Classes.VM;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NSem3PT34C.Classes.Structure;
using Brushes = System.Windows.Media.Brushes;
using Color = System.Drawing.Color;
using CommandManager = NSem3PT34.Classes.Command.CommandManager;
using Font = System.Drawing.Font;
using ICommand = NSem3PT34.Classes.Command.ICommand;
using Point = System.Windows.Point;

namespace NSem3PT34
{
    public partial class MainWindow : Window, IObserver, ISpellingErrorHandler
    {
        private DrawingContext graphics;
        private Composition comp;
        private ICompositor compositor;
        private double x1, y1, x2, y2;
        private Document document;
        private int index;
        private bool spellCheckEnabled;
        private SelectionRange selectionRange;
        private List<UiGlyph[]> misspelledGlyphs;

        public MainWindow()
        {
            InitializeComponent();
            graphics = DrawingCanvas.RenderStart();
            comp = new Composition();
            document = new ConcreteDocument();
            document = new ScrollableDocument(document);
            this.comp.RegisterObserver(this);
            this.compositor = new SimpleCompositor();
            this.spellCheckEnabled = false;
            FontComboBox.Items.Clear();
            foreach (var f in Fonts.SystemFontFamilies.OrderBy(ff => ff.Source))
            {
                FontComboBox.Items.Add(new ComboBoxItem { Content = f.Source });
            }
            FontComboBox.SelectedIndex = 0;

            List<int> fontSizes = new List<int>();
            fontSizes.Add(18);
            fontSizes.Add(20);
            fontSizes.Add(22);
            fontSizes.Add(24);
            fontSizes.Add(26);
            fontSizes.Add(28);
            fontSizes.Add(36);
            fontSizes.Add(48);
            fontSizes.Add(72);

            FontSizeComboBox.Items.Clear();
            foreach (var f in fontSizes)
            {
                FontSizeComboBox.Items.Add(new ComboBoxItem { Content = f });
            }
            FontSizeComboBox.SelectedIndex = 4;

            Title = "Text Editor";
            SpellChecker.GetInstance().LoadDictionary("C:\\Users\\Vadim\\source\\repos\\NSem3PT34C\\NSem3PT34C\\Dictionaries\\english");
            this.PreviewKeyDown += MainWindow_PreviewKeyDown;
            this.PreviewMouseDown += MainWindow_PreviewMouseDown;
            this.PreviewMouseUp += MainWindow_PreviewMouseUp;
        }

        private void ToggleBold_Click(object sender, RoutedEventArgs e)
        {
            if(selectionRange != null)
            {
                ICommand cmd = null;
                int startFrom = this.GetStartFrom();
                int endAt = this.GetEndAt();
                this.selectionRange = null;
                cmd = new ToggleBoldCommand(graphics, this.comp, startFrom, endAt);
                CommandManager.GetInstance().Execute(cmd);
            }
        }

        private void ToggleItalic_Click(object sender, RoutedEventArgs e)
        {
            if (selectionRange != null)
            {
                ICommand cmd = null;
                int startFrom = this.GetStartFrom();
                int endAt = this.GetEndAt();
                this.selectionRange = null;
                cmd = new ToggleItalicCommand(graphics, this.comp, startFrom, endAt);
                CommandManager.GetInstance().Execute(cmd);
            }
        }

        private void ToggleSpellCheck_Click(object sender, RoutedEventArgs e)
        {
            spellCheckEnabled = !spellCheckEnabled;
            UpdateObserver();
        }


        private void MainWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(DrawingCanvas);
            x1 = mousePos.X;
            y1 = mousePos.Y;
        }

        public static int IsPointGreater(Point p1, Point p2)
        {
            int i = 0;
            if (p1.X < p2.X)
            {
                i = -1;
            }
            else if (p1.X > p2.X)
            {
                i = 1;
            }
            else if (p1.Y < p2.Y)
            {
                i = -1;
            }
            else if (p1.Y > p2.Y)
            {
                i = 1;
            }

            return i;
        }

        private void MainWindow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = e.GetPosition(DrawingCanvas);
            x2 = mousePos.X;
            y2 = mousePos.Y;
            Point p1 = new Point(x1, y1);
            Point p2 = new Point(x2, y2);
            if (IsPointGreater(p1, p2) == 1)
            {
                Point temp = p1;
                p1 = p2;
                p2 = temp;
            }
	
            x1 = p1.X;
            y1 = p1.Y;
            x2 = p2.X;
            y2 = p2.Y;

            if (y1 < 0 || y2 < 0)
                return;

            int i, j;
            i = j = 0;
            List<Row> rows = document.GetRows();
            for (i = 0 + index; i < rows.Count; i++)
            {
                Row row = rows[i];
                if (row.Bounds().Bottom >= y1)
                {
                    for (j = 0; j < row.GetUiGlyphs().Count; j++)
                    {
                        UiGlyph glyph = row.GetUiGlyphs()[j];
                        if (glyph.GetPosition().X >= x1)
                        {
                            break;
                        }
                    }

                    break;
                }
            }

            SelectionRange range = new SelectionRange();
            j = j == 0 ? j : j - 1;
            range.SetStartRow(i);
            range.SetStartCol(j);

            for (i = 0 + index; i < rows.Count; i++)
            {
                Row row = rows[i];
                if (row.Bounds().Bottom >= y2)
                {
                    for (j = 0; j < row.GetUiGlyphs().Count; j++)
                    {
                        UiGlyph glyph = row.GetUiGlyphs()[j];
                        if (glyph.GetPosition().X >= x2)
                        {
                            break;
                        }
                    }

                    break;
                }
            }

            j = j == 0 ? j : j - 1;
            if (i >= rows.Count && rows.Count > 0)
            {
                i = rows.Count - 1;
                j = rows[i].GetUiGlyphs().Count - 1;
            }

            range.SetEndRow(i);
            range.SetEndCol(j);

            if (range.GetStartRow() > range.GetEndRow())
            {
                int startx = range.GetEndRow();
                int starty = range.GetEndCol();
                range.SetEndRow(range.GetStartRow());
                range.SetEndCol(range.GetStartCol());
                range.SetStartRow(startx);
                range.SetStartCol(starty);
            }

            if (range.GetEndRow() >= rows.Count)
            {
                range.SetEndRow(range.GetEndRow() - 1);
            }

            if (range.GetStartRow() < rows.Count)
            {
                this.selectionRange = range;
                UpdateObserver();
            }
        }

        private void MainWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            Glyph glyph = null;
            ICommand cmd = null;
            if (e.Key == Key.Escape)
            {
                this.selectionRange = null;
            }
            else if (e.Key == Key.Delete)
            {
                if (this.selectionRange != null)
                {
                    int startFrom = this.GetStartFrom();
                    int endAt = this.GetEndAt();
                    this.selectionRange = null;
                    cmd = new DeleteCommand(comp, startFrom, endAt);
                    CommandManager.GetInstance().Execute(cmd);
                }
            }
            else if (e.Key == Key.Back)
            {
                if (this.document.GetRows().Count > 0)
                {
                    int startFrom = this.document.GetRows()[this.document.GetRows().Count - 1].GetUiGlyphs()
                        [this.document.GetRows()[this.document.GetRows().Count - 1].GetUiGlyphs().Count - 1].GetPhysicalIndex();
                    int endAt = this.document.GetRows()[this.document.GetRows().Count - 1].GetUiGlyphs()
                        [this.document.GetRows()[this.document.GetRows().Count - 1].GetUiGlyphs().Count - 1].GetPhysicalIndex();
                    if (selectionRange == null || !selectionRange.IsSingleGlyphSelection())
                    {
                        this.selectionRange = null;
                        cmd = new DeleteCommand(comp, startFrom, endAt);
                        CommandManager.GetInstance().Execute(cmd);
                    }
                    else if(selectionRange != null && selectionRange.IsSingleGlyphSelection())
                    {
                        cmd = new DeleteCommand(comp, GetStartFrom(), GetEndAt());
                        CommandManager.GetInstance().Execute(cmd);
                        ViewEventArgs param = new ViewEventArgs(null, 4, 4, MyCanvasBorder.ActualWidth,
                            MyCanvasBorder.ActualHeight);
                        List<Row> rows = this.compositor.Compose(this.comp.GetChildren(), param);
                        this.selectionRange.SetEndCol(this.selectionRange.GetEndCol() - 1);
                        this.selectionRange.SetStartCol(this.selectionRange.GetStartCol() - 1);
                        if (selectionRange.GetStartCol() < 0)
                        {
                            if (this.selectionRange.GetStartRow() != 0)
                            {
                                this.selectionRange.SetEndRow(this.selectionRange.GetEndRow() - 1);
                                this.selectionRange.SetStartRow(this.selectionRange.GetStartRow() - 1);
                                this.selectionRange.SetEndCol(rows[selectionRange.GetStartRow()].GetUiGlyphs().Count - 1);
                                this.selectionRange.SetStartCol(selectionRange.GetEndCol());
                            }
                            else
                                this.selectionRange = null;
                        }
                        UpdateObserver();
                    }
                }
            }
            else if (e.Key == Key.G && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                int startFrom = this.GetStartFrom();
                int endAt = this.GetEndAt();
                this.selectionRange = null;
                cmd = new IncreaseFontSizeCommand(graphics, this.comp, startFrom, endAt);
                CommandManager.GetInstance().Execute(cmd);
            }
            else if (e.Key == Key.L && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                int startFrom = this.GetStartFrom();
                int endAt = this.GetEndAt();
                this.selectionRange = null;
                cmd = new DecreaseFontSizeCommand(graphics, this.comp, startFrom, endAt);
                CommandManager.GetInstance().Execute(cmd);
            }
            else if (e.Key == Key.B && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                int startFrom = this.GetStartFrom();
                int endAt = this.GetEndAt();
                this.selectionRange = null;
                cmd = new ToggleBoldCommand(graphics, this.comp, startFrom, endAt);
                CommandManager.GetInstance().Execute(cmd);
            }
            else if (e.Key == Key.I && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                int startFrom = this.GetStartFrom();
                int endAt = this.GetEndAt();
                this.selectionRange = null;
                cmd = new ToggleItalicCommand(graphics, this.comp, startFrom, endAt);
                CommandManager.GetInstance().Execute(cmd);
            }
            else if (e.Key == Key.Z && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                this.selectionRange = null;
                CommandManager.GetInstance().Undo();
            }
            else if (e.Key == Key.Y && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
            {
                this.selectionRange = null;
                CommandManager.GetInstance().Redo();
            }
            else if (e.Key == Key.PageUp)
            {
                if (index > 0)
                {
                    index -= 1;
                }
                UpdateObserver();
            }
            else if (e.Key == Key.PageDown)
            {
                if (this.document.NeedScrolling(MyCanvasBorder.ActualHeight))
                {
                    if (index < (this.document.GetRows().Count - 1))
                    {
                        index += 1;
                    }
                    UpdateObserver();
                }
            }
            else if (e.Key == Key.Enter)
            {
                Classes.Util.Font font = new Classes.Util.Font();
                font.Name = FontComboBox.Text;
                font.Style = Classes.Util.FontStyle.Normal;
                font.Size = Int32.Parse(FontSizeComboBox.Text);
                glyph = new BreakGlyph(font);
                this.InsertGlyph(glyph);
                ViewEventArgs param = new ViewEventArgs(null, 4, 4, MyCanvasBorder.ActualWidth,
                    MyCanvasBorder.ActualHeight);
                List<Row> rows = this.compositor.Compose(this.comp.GetChildren(), param);
                if (this.selectionRange != null && this.selectionRange.GetEndRow() == rows.Count - 1 &&
                    this.selectionRange.GetEndCol() == rows[rows.Count - 1].GetUiGlyphs().Count - 2)
                    this.selectionRange = null;
                if (this.selectionRange != null && this.selectionRange.IsSingleGlyphSelection())
                {
                    this.selectionRange.SetEndCol(0);
                    this.selectionRange.SetStartCol(0);
                    this.selectionRange.SetEndRow(this.selectionRange.GetEndRow() + 1);
                    this.selectionRange.SetStartRow(this.selectionRange.GetStartRow() + 1);
                }
                UpdateObserver();
            }
            else if (e.Key == Key.Right)
            {
                if (this.selectionRange != null)
                {
                    ViewEventArgs param = new ViewEventArgs(null, 4, 4, MyCanvasBorder.ActualWidth,
                        MyCanvasBorder.ActualHeight);
                    List<Row> rows = this.compositor.Compose(this.comp.GetChildren(), param);
                    if (this.selectionRange.GetEndRow() == rows.Count - 1 && this.selectionRange.GetEndCol() == rows[rows.Count - 1].GetUiGlyphs().Count - 1)
                    {
                        this.selectionRange = null;
                    }
                    else
                    {
                        this.selectionRange.SetEndCol(this.selectionRange.GetEndCol() + 1);
                        this.selectionRange.SetStartCol(this.selectionRange.GetEndCol());
                        if (rows[selectionRange.GetStartRow()].GetUiGlyphs().Count <= selectionRange.GetStartCol())
                        {
                            this.selectionRange.SetEndCol(0);
                            this.selectionRange.SetStartCol(0);
                            this.selectionRange.SetEndRow(this.selectionRange.GetEndRow() + 1);
                            this.selectionRange.SetStartRow(this.selectionRange.GetEndRow());
                        }
                    } 
                    UpdateObserver();
                }
            }
            else if (e.Key == Key.Left)
            {
                if (this.selectionRange != null)
                {
                    ViewEventArgs param = new ViewEventArgs(null, 4, 4, MyCanvasBorder.ActualWidth,
                        MyCanvasBorder.ActualHeight);
                    List<Row> rows = this.compositor.Compose(this.comp.GetChildren(), param);
                    if (!(selectionRange.GetStartRow() == 0 && selectionRange.GetStartCol() == 0))
                    {
                        this.selectionRange.SetStartCol(this.selectionRange.GetStartCol() - 1);
                        this.selectionRange.SetEndCol(this.selectionRange.GetStartCol());
                        if (selectionRange.GetStartCol() < 0)
                        {
                            this.selectionRange.SetStartRow(this.selectionRange.GetStartRow() - 1);
                            this.selectionRange.SetEndRow(this.selectionRange.GetStartRow());
                            this.selectionRange.SetEndCol(rows[selectionRange.GetStartRow()].GetUiGlyphs().Count - 1);
                            this.selectionRange.SetStartCol(selectionRange.GetEndCol());
                        }
                    }
                    UpdateObserver();
                }
            }
            else if (e.Key == Key.Up)
            {
                if (this.selectionRange != null)
                {
                    ViewEventArgs param = new ViewEventArgs(null, 4, 4, MyCanvasBorder.ActualWidth,
                        MyCanvasBorder.ActualHeight);
                    List<Row> rows = this.compositor.Compose(this.comp.GetChildren(), param);
                    Point posMem = rows[this.selectionRange.GetStartRow()].GetUiGlyphs()[this.selectionRange.GetStartCol()].GetPosition();
                    if (selectionRange.GetStartRow() != 0)
                    {
                        this.selectionRange.SetStartRow(this.selectionRange.GetStartRow() - 1);
                        this.selectionRange.SetEndRow(this.selectionRange.GetStartRow());
                        if (rows[this.selectionRange.GetStartRow()].GetUiGlyphs()[rows[this.selectionRange.GetStartRow()].GetUiGlyphs().Count - 1].GetPosition().X < posMem.X)
                        {
                            this.selectionRange.SetStartCol(rows[this.selectionRange.GetStartRow()].GetUiGlyphs().Count - 1);
                            this.selectionRange.SetEndCol(this.selectionRange.GetStartCol());
                        }
                        else
                        {
                            List<UiGlyph> glyphs = rows[this.selectionRange.GetStartRow()].GetUiGlyphs();
                            int i = 0;
                            foreach (UiGlyph uiGlyph in glyphs)
                            {
                                if (uiGlyph.GetPosition().X >= posMem.X)
                                    break;
                                i++;
                            }
                            this.selectionRange.SetStartCol(i);
                            this.selectionRange.SetEndCol(this.selectionRange.GetStartCol());
                        }
                    }
                    UpdateObserver();
                }
            }
            else if (e.Key == Key.Down)
            {
                if (this.selectionRange != null)
                {
                    ViewEventArgs param = new ViewEventArgs(null, 4, 4, MyCanvasBorder.ActualWidth,
                        MyCanvasBorder.ActualHeight);
                    List<Row> rows = this.compositor.Compose(this.comp.GetChildren(), param);
                    Point posMem = rows[this.selectionRange.GetStartRow()].GetUiGlyphs()[this.selectionRange.GetStartCol()].GetPosition();
                    if (selectionRange.GetEndRow() != rows.Count - 1)
                    {
                        this.selectionRange.SetEndRow(this.selectionRange.GetEndRow() + 1);
                        this.selectionRange.SetStartRow(this.selectionRange.GetEndRow());
                        if (rows[this.selectionRange.GetStartRow()].GetUiGlyphs()[rows[this.selectionRange.GetStartRow()].GetUiGlyphs().Count - 1].GetPosition().X < posMem.X)
                        {
                            this.selectionRange.SetStartCol(rows[this.selectionRange.GetStartRow()].GetUiGlyphs().Count - 1);
                            this.selectionRange.SetEndCol(this.selectionRange.GetStartCol());
                        }
                        else
                        {
                            List<UiGlyph> glyphs = rows[this.selectionRange.GetStartRow()].GetUiGlyphs();
                            int i = 0;
                            foreach (UiGlyph uiGlyph in glyphs)
                            {
                                if (uiGlyph.GetPosition().X >= posMem.X)
                                    break;
                                i++;
                            }
                            this.selectionRange.SetStartCol(i);
                            this.selectionRange.SetEndCol(this.selectionRange.GetStartCol());
                        }
                    }
                    UpdateObserver();
                }
            }
            else
            {
                if (!((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control) && e.Key != Key.LeftShift)
                {
                    Classes.Util.Font font = new Classes.Util.Font();
                    font.Name = FontComboBox.Text;
                    font.Style = Classes.Util.FontStyle.Normal;
                    font.Size = Int32.Parse(FontSizeComboBox.Text);
                    glyph = new CharGlyph(GetCharFromKey(e.Key), font);
                    this.InsertGlyph(glyph);
                    ViewEventArgs param = new ViewEventArgs(null, 4, 4, MyCanvasBorder.ActualWidth,
                MyCanvasBorder.ActualHeight);
                    List<Row> rows = this.compositor.Compose(this.comp.GetChildren(), param);

                    if (this.selectionRange != null && this.selectionRange.IsSingleGlyphSelection())
                    {
                        this.selectionRange.SetEndCol(this.selectionRange.GetEndCol() + 1);
                        this.selectionRange.SetStartCol(this.selectionRange.GetStartCol() + 1);
                        if (rows[selectionRange.GetStartRow()].GetUiGlyphs().Count <= selectionRange.GetStartCol())
                        {
                            this.selectionRange.SetEndCol(0);
                            this.selectionRange.SetStartCol(0);
                            this.selectionRange.SetEndRow(this.selectionRange.GetEndRow() + 1);
                            this.selectionRange.SetStartRow(this.selectionRange.GetStartRow() + 1);
                        }
                    }
                    UpdateObserver();
                }
            }
        }

        public void UpdateObserver()
        {
            DrawingContext dc = DrawingCanvas.RenderStart();
            this.graphics = dc;
            ViewEventArgs param = new ViewEventArgs(dc, 4, 4, MyCanvasBorder.ActualWidth,
                MyCanvasBorder.ActualHeight);
            List<Row> rows = this.compositor.Compose(this.comp.GetChildren(), param);
            HandleDrawing(rows, param);
            DrawingCanvas.RenderStop(dc);
        }
        public int GetStartFrom()
        {
            return this.document.GetRows()[this.selectionRange.GetStartRow()].GetUiGlyphs()
                [this.selectionRange.GetStartCol()].GetPhysicalIndex();
        }

        public int GetEndAt()
        {
            return this.document.GetRows()[this.selectionRange.GetEndRow()].GetUiGlyphs()
                [this.selectionRange.GetEndCol()].GetPhysicalIndex();
        }

        private void InsertGlyph(Glyph glyph)
        {
            ICommand cmd = null;
            int physicalIndex = Int32.MinValue;
            if (this.selectionRange == null)
            {
                physicalIndex = this.comp.GetChildren().Count;
            }
            else if (this.selectionRange != null && this.selectionRange.IsSingleGlyphSelection())
            {
                physicalIndex = this.document.GetRows()[this.selectionRange.GetStartRow()].GetUiGlyphs()
                    [this.selectionRange.GetStartCol()].GetPhysicalIndex() + 1;
            }

            if (physicalIndex != Int32.MinValue)
            {
                cmd = new InsertCommand(this.comp, glyph, physicalIndex);
                CommandManager.GetInstance().Execute(cmd);

            }
        }

        public void HandleDrawing(List<Row> rows, ViewEventArgs args)
        {
            this.document.Draw(rows, args, this.index);
            this.UpdateLogicalLocations(args);

            if (this.spellCheckEnabled)
            {
                IVisitor visitor = new SpellingCheckingVisitor(this);
                foreach (Row row in rows)
                {
                    row.Accept(visitor);
                }
                
            }

            if (this.selectionRange != null)
            {
                this.SelectGlyphs(args);
            }

            
        }

        public void UpdateLogicalLocations(ViewEventArgs args)
        {
            int i, j;
            Point dummyPoint = new Point(Int32.MinValue, Int32.MinValue);
            for (i = 0; i < this.index; i++)
            {
                Row currentRow = this.document.GetRows()[i];
                currentRow.SetTop(Int32.MinValue);
                currentRow.SetLeft(Int32.MinValue);
                foreach (UiGlyph uiGlyph in currentRow.GetUiGlyphs())
                {
                    uiGlyph.SetPosition(dummyPoint);
                }
            }

            double currentTop = args.GetTop();
            double currentLeft = args.GetLeft();
            for (j = i; j < this.document.GetRows().Count; j++)
            {
                Row currentRow = this.document.GetRows()[j];
                currentRow.SetTop(currentTop);
                currentRow.SetLeft(currentLeft);
                foreach (UiGlyph uiGlyph in currentRow.GetUiGlyphs())
                {
                    Point position = new Point(currentLeft, currentTop);
                    uiGlyph.SetPosition(position);
                    currentLeft += uiGlyph.GetGlyph().GetWidth() + 2;
                }

                currentTop += currentRow.GetHeight();
                currentLeft = args.GetLeft();
            }
        }
        public void SelectGlyphs(ViewEventArgs args)
        {
            int start, end;
            for (int i = this.selectionRange.GetStartRow(); i <= this.selectionRange.GetEndRow(); i++)
            {
                Row row = this.document.GetRows()[i];
                start = 0;
                end = row.GetUiGlyphs().Count - 1;
                if (i == this.selectionRange.GetStartRow())
                {
                    start = this.selectionRange.GetStartCol();
                }

                if (i == this.selectionRange.GetEndRow())
                {
                    end = this.selectionRange.GetEndCol();
                }

                row.Select(args.GetGraphics(), Brushes.Black, Brushes.White, row.GetTop(), row.GetLeft(), start, end);
            }
        }

        public void HandleSpellingError(Dictionary<UiGlyph, Row> glyphs)
        {
            foreach (KeyValuePair<UiGlyph, Row> entry in glyphs)
            {
                entry.Key.GetGlyph().Select(this.graphics, Brushes.White, Brushes.Red, entry.Key.GetPosition().X,
                    entry.Key.GetPosition().Y + entry.Value.Bounds().Height - entry.Key.GetGlyph().Bounds().Height);
            }
        }






        public enum MapType : uint
        {
            MAPVK_VK_TO_VSC = 0x0,
            MAPVK_VSC_TO_VK = 0x1,
            MAPVK_VK_TO_CHAR = 0x2,
            MAPVK_VSC_TO_VK_EX = 0x3,
        }

        [DllImport("user32.dll")]
        public static extern int ToUnicode(
            uint wVirtKey,
            uint wScanCode,
            byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr, SizeParamIndex = 4)]
            StringBuilder pwszBuff,
            int cchBuff,
            uint wFlags);

        [DllImport("user32.dll")]
        public static extern bool GetKeyboardState(byte[] lpKeyState);

        [DllImport("user32.dll")]
        public static extern uint MapVirtualKey(uint uCode, MapType uMapType);

        public static char GetCharFromKey(Key key)
        {
            if (key == Key.Space)
            {
                return ' ';
            }
            char ch = ' ';

            int virtualKey = KeyInterop.VirtualKeyFromKey(key);
            byte[] keyboardState = new byte[256];
            GetKeyboardState(keyboardState);

            uint scanCode = MapVirtualKey((uint)virtualKey, MapType.MAPVK_VK_TO_VSC);
            StringBuilder stringBuilder = new StringBuilder(2);

            int result = ToUnicode((uint)virtualKey, scanCode, keyboardState, stringBuilder, stringBuilder.Capacity, 0);
            switch (result)
            {
                case -1:
                    break;
                case 0:
                    break;
                case 1:
                {
                    ch = stringBuilder[0];
                    break;
                }
                default:
                {
                    ch = stringBuilder[0];
                    break;
                }
            }
            return ch;
        }


        private void MainWindow_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            this.selectionRange = null;
            UpdateObserver();
        }
    }


}
