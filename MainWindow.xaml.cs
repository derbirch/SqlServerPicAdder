using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.Win32;

namespace 添加图片
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        FileStream fs;
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            BitmapImage tempImage;
            OpenFileDialog openfiledialog = new OpenFileDialog { Filter = "图像文件|*.jpg;*.png;*.jpeg;*.bmp;*.gif|所有文件|*.*" };
            if ((bool)openfiledialog.ShowDialog())
            {
                tempImage = new BitmapImage(new Uri(openfiledialog.FileName));
                imgBpx.Source = tempImage;
                fs = new FileStream(openfiledialog.FileName.ToString(), FileMode.Open, FileAccess.Read);
            }
            byte[] Data = new byte[fs.Length];
            fs.Read(Data, 0, Convert.ToInt32(fs.Length));
            fs.Close();
            try
            {
                SqlConnection con = new SqlConnection("server=localhost;database=图书征订;Integrated Security=True");
                con.Open();
                SqlCommand cmd = new SqlCommand("UPDATE 图书信息表 SET 封面图片=@Image where 图书编号=@index;", con);
                cmd.Parameters.Add("@Image", SqlDbType.Image);
                //不会写不会写 
                cmd.Parameters.AddWithValue("@index", tb1.Text.Trim());
                cmd.Parameters["@Image"].Value = Data;
                cmd.ExecuteNonQuery();

                con.Close();
                MessageBox.Show("图片上传成功");

            }
            catch
            {
                MessageBox.Show("您选择的图片不能被读取或文件类型不对！", "错误");

            }
        }
    }
}
