using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Text.RegularExpressions;
using System.IO;
using MySql.Data.MySqlClient;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        private long receive_count = 0; //接收字节计数
        private StringBuilder sb = new StringBuilder();    //为了避免在接收处理函数中反复调用，依然声明为一个全局变量
        private DateTime current_time = new DateTime();    //为了避免在接收处理函数中反复调用，依然声明为一个全局变量

        private StringBuilder builder = new StringBuilder();    //避免在事件处理方法中反复创建，定义为全局
        public Form1()
        {
            InitializeComponent();
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(SerialPort1_DataReceived);       //添加串口中断事件

            x = this.Width;
            y = this.Height;
            SetTag(this);

            this.Resize += new EventHandler(Form1_Resize);//窗体调整大小时引发事件
        }

        //串口接收事件处理
        private void SerialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            int num = serialPort1.BytesToRead;      //获取接收缓冲区中的字节数
            byte[] received_buf = new byte[num];    //声明一个大小为num的字节数据用于存放读出的byte型数据


            receive_count += num;                   //接收字节计数变量增加num
            serialPort1.Read(received_buf, 0, num);   //读取接收缓冲区中num个字节到byte数组中

            sb.Clear();     //防止出错,首先清空字符串构造器
            sb.Append(Encoding.ASCII.GetString(received_buf));  //将整个数组解码为ASCII数组
            try
            {
                //因为要访问UI资源，所以需要使用invoke方式同步ui
                Invoke((EventHandler)(delegate
                {
                    if (checkBox1.Checked)
                    {
                        //显示时间
                        current_time = System.DateTime.Now;     //获取当前时间
                        textBox1.AppendText(current_time.ToString("HH:mm:ss") + "  " + sb.ToString());

                    }
                    else
                    {
                        //不显示时间 
                        textBox1.AppendText(sb.ToString() +";");
                    }
                }
                  )
                );
            }
            catch (Exception ex)
            {
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(ex.Message);

            }
        }



        private void ReceiveData_Click(object sender, EventArgs e)
        {
            try
            {
                //将可能产生异常的代码放置在try块中
                //根据当前串口属性来判断是否打开
                if (serialPort1.IsOpen)
                {
                    //串口已经处于打开状态
                    serialPort1.Close();    //关闭串口
                    ReceiveData.Text = "打开串口";
                    ReceiveData.BackColor = Color.ForestGreen;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    comboBox3.Enabled = true;
                    comboBox4.Enabled = true;
                    comboBox5.Enabled = true;
                }
                else
                {
                    //串口已经处于关闭状态，则设置好串口属性后打开
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = false;
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                    serialPort1.DataBits = Convert.ToInt16(comboBox3.Text);

                    if (comboBox4.Text.Equals("None"))
                        serialPort1.Parity = System.IO.Ports.Parity.None;
                    else if (comboBox4.Text.Equals("Odd"))
                        serialPort1.Parity = System.IO.Ports.Parity.Odd;
                    else if (comboBox4.Text.Equals("Even"))
                        serialPort1.Parity = System.IO.Ports.Parity.Even;
                    else if (comboBox4.Text.Equals("Mark"))
                        serialPort1.Parity = System.IO.Ports.Parity.Mark;
                    else if (comboBox4.Text.Equals("Space"))
                        serialPort1.Parity = System.IO.Ports.Parity.Space;

                    if (comboBox5.Text.Equals("1"))
                        serialPort1.StopBits = System.IO.Ports.StopBits.One;
                    else if (comboBox5.Text.Equals("1.5"))
                        serialPort1.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    else if (comboBox5.Text.Equals("2"))
                        serialPort1.StopBits = System.IO.Ports.StopBits.Two;

                    serialPort1.Open();     //打开串口
                    ReceiveData.Text = "关闭串口";
                    ReceiveData.BackColor = Color.Firebrick;

                }
            }
            catch (Exception ex)
            {
                //捕获可能发生的异常并进行处理

                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                ReceiveData.Text = "打开串口";
                ReceiveData.BackColor = Color.ForestGreen;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox5.Enabled = true;

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int i;
            //单个添加
            for (i = 300; i <= 38400; i = i * 2)
            {
                comboBox2.Items.Add(i.ToString());  //添加波特率列表
            }

            //批量添加波特率列表
            string[] baud = { "43000", "56000", "57600", "115200", "128000", "230400", "256000", "460800" };
            comboBox2.Items.AddRange(baud);

            //获取电脑当前可用串口并添加到选项列表中
            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());

            //设置选项默认值
            comboBox2.Text = "115200";
            comboBox3.Text = "8";
            comboBox4.Text = "None";
            comboBox5.Text = "1";
        }



        /// <summary>
        /// 将串口接收数据添加至数据库，并处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AnalyzeData_Click(object sender, EventArgs e)
        {
            
            string txtStr = textBox1.Text;
            string Str = txtStr.Trim();
            string[] split = Str.Split(new char[] { '-', ';' }, StringSplitOptions.RemoveEmptyEntries);

            BIL.GroupID groupID = new BIL.GroupID();
            groupID.AddData(groupID.GetID(split));

            BIL.AverageDB averageDB = new BIL.AverageDB();
            averageDB.OrderBy();//对rssiaverage从大到小排序

        }

        private void Location_Click(object sender, EventArgs e)
        {
            BIL.Algorithm algorithm = new BIL.Algorithm();
            MODEL.Location location = new MODEL.Location();

            location=algorithm.Caculate();
            textBox2.Text +="x="+ location.X.ToString() +",y=" +location.Y.ToString()+"\r\n";

            Image image = pictureBox1.Image;
            float width = image.Width;
            float height = image.Height;
            float xper = width / 12;//地图长宽各12大格,10大格表示5
            float yper = height / 12;
            float x0 = xper;
            float y0 = height - yper;

            float x = (float)location.X * (10 / 5)*xper + x0;
            float y = -(float)location.Y * (10 / 5)*yper + y0;
            //float x = 4.8F*2.0F*xper+x0;
            //float y = -4.3F*2.0F*yper+y0;

            string imgPath = "E:\\新建文件夹\\WindowsFormsApp2\\Car.png";
            Image Carimage;
            FileStream fileStream = new FileStream(imgPath, FileMode.Open, FileAccess.Read);//读取图片文件流
            int byteLength = (int)fileStream.Length;    //小车图片字节数
            byte[] fileBytes = new byte[byteLength];    //根据图片字节数创建一个存储该图片的字节数组
            fileStream.Read(fileBytes, 0, byteLength);  //读取小车图片到数组
            fileStream.Close();                         //关闭文件流，解除对外部文件的锁定
            Carimage = Image.FromStream(new MemoryStream(fileBytes));
            Graphics g = Graphics.FromImage(pictureBox1.Image);  //创建背景图片的Graphics对象（调用该对象在背景图片上绘图）
            g.DrawImage(Carimage, x - Carimage.Width / 2, y - Carimage.Height / 2, Carimage.Width, Carimage.Height);

            pictureBox1.Refresh();//刷新
        }

        /// <summary>
        /// 从硬盘添加地图至界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Addmap_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";//默认路径，注意这里写路径时要用c:\\而不是c:\
            openFileDialog.Title = "打开地图";//设置打开文件对话框标题
            openFileDialog.Filter = "jpg图片|*.JPG|gif图片|*.GIF|png图片|*.PNG|jpeg图片|*.JPEG";//过滤的文件，以|隔开，如“文本文件|*.*|Java文件|*.java”
            openFileDialog.RestoreDirectory = true;//打开对话框后，文件内容有改变了，是否同步刷新
            openFileDialog.FilterIndex = 1;//当filter有多个时，选择默认的filter，注意，下标时从1开始，如果只有一个filter可以不用写这个属性
           if (openFileDialog.ShowDialog() == DialogResult.OK)//这个是关键，意思是当你选择了文件后并点击了OK按钮
           {
                pictureBox1.Image = Image.FromFile(openFileDialog.FileName);
           }

        }


        #region 控件大小随窗体大小等比例缩放
        private float x;//定义当前窗体的宽度
        private float y;//定义当前窗体的高度
        private void SetTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size;
                if (con.Controls.Count > 0)
                {
                    SetTag(con);
                }
            }
        }

        private void SetControls(float newx, float newy, Control cons)
        {
            //遍历窗体中的控件，重新设置控件的值
            foreach (Control con in cons.Controls)
            {
                //获取控件的Tag属性值，并分割后存储字符串数组
                if (con.Tag != null)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
                    //根据窗体缩放的比例确定控件的值
                    con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);//宽度
                    con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);//高度
                    con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);//左边距
                    con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);//顶边距
                    Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;//字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        SetControls(newx, newy, con);
                    }
                }
            }
        }


        private void Form1_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / x;
            float newy = (this.Height) / y;
            SetControls(newx, newy, this);
        }


        #endregion

        /// <summary>
        /// 查询,在datagridview显示
        /// </summary>
        //private void Query()
        //{
        //    BIL.UserDB biluser = new BIL.UserDB();

        //    dataGridView1.Rows.Clear();

        //    string strwhere = "id='A'";//语句可改
        //    //查
        //    List<MODEL.User> userlist = biluser.GetModelList(strwhere);
        //    foreach (MODEL.User item in userlist)
        //    {
        //        DataGridViewRow row = new DataGridViewRow();

        //        DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
        //        cell.Value = item.ID;
        //        DataGridViewTextBoxCell cell2 = new DataGridViewTextBoxCell();
        //        cell2.Value = item.RSSI;

        //        row.Cells.Add(cell);
        //        row.Cells.Add(cell2);

        //        dataGridView1.Rows.Add(row);
        //    }

        //}


    }


}
