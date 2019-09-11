using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviceContextTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Win32API の4つの関数 CreateFont、SelectObject、TextOut、DeleteObject を宣言します。
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private extern static System.IntPtr CreateFont(
        int nHeight, int nWidth, int nEscapement, int nOrientation, int fnWeight, bool fdwItalic,
        bool fdwUnderline, bool fdwStrikeOut, int fdwCharSet, int fdwOutputPrecision, int fdwClipPrecision,
        int fdwQuality, int fdwPitchAndFamily, string lpszFace);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private extern static System.IntPtr SelectObject(System.IntPtr hObject, System.IntPtr hFont);
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private extern static int TextOut(IntPtr hdc, int nXStart, int nYStart, string lpString, int cbString);
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private extern static bool DeleteObject(System.IntPtr hObject);
        private void Button1_Click(object sender, EventArgs e)
        {
            PrintDocument pd = new PrintDocument();
            pd.PrinterSettings.PrinterName = comboBox1.Text;
            pd.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(pd_PrintPage);
            pd.Print();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //コンピュータにインストールされているすべてのプリンタの名前を出力する
            foreach (string s in System.Drawing.Printing.PrinterSettings.InstalledPrinters)
            {
                comboBox1.Items.Add(s);
            }   

            foreach (FontFamily ff in new System.Drawing.Text.InstalledFontCollection().Families)
            {
                comboBox2.Items.Add(ff.Name);
            }
        }

        private void pd_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            //クラスメンバ変数として mFont を宣言します。
           
            //            論理フォントを作成しデバイスコンテキストに設定後、TextOut 関数にてバーコードフォントを描画しま
            //す。 本サンプルコードでは PrintDocument クラスによる印刷方法を解説します。PrintDocument クラス
            //の PrintPage イベントハンドラーに下記コードを記述します。

            IntPtr hdc = e.Graphics.GetHdc();
            String BarFontName = "TEC-BarFont Code128";
            int BarFontSize = 500;
            IntPtr mFont = CreateFont(BarFontSize, 0, 0, 0, 0, false, false, false, 0, 0, 0, 0, 0, BarFontName);
            SelectObject(hdc, mFont);
            String BarcodeData = "ABCDEFG12345";
            TextOut(hdc, 118, 236, BarcodeData, BarcodeData.Length);
            DeleteObject(mFont);

            String BarFontName2 = comboBox2.Text;
            int BarFontSize2 = 100;
            IntPtr mFont2 = CreateFont(BarFontSize2, 0, 0, 0, 0, false, false, false, 128, 0, 0, 0, 0, BarFontName2);
            SelectObject(hdc, mFont2);
            String BarcodeDat2 = "文字列";
            TextOut(hdc, 0, 0, BarcodeDat2, Encoding.GetEncoding("Shift_JIS").GetByteCount(BarcodeDat2));
            DeleteObject(mFont2);

            e.Graphics.ReleaseHdc(hdc);
        }
    }
}
