using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Data.SqlClient;
using System.Windows.Forms.DataVisualization.Charting;

namespace UltraThinApp
{

    public partial class Form1 : Form
    {
        /// <summary>
        /// 保存本机COM口
        /// </summary>
        private string[] strCom;
        /// <summary>
        /// 通信
        /// </summary>
        public static SerialPort Sp = new SerialPort();
        public static SerialPortBase spb;
        private PortInfo _portInfo;
        private static readonly object synObj = new object();

        //private static UI_Main frm = new UI_Main();
        public PortInfo _PortInfo
        {
            get { return this._portInfo; }
            set { this._portInfo = value; }
        }
        public Form1()
        {
            InitializeComponent();
            _portInfo = new PortInfo();
            spb = new SerialPortBase();
            GetPort();
            Init(strCom, _portInfo);
            //加载机型下拉列表    

        }

        private void btn_com_set_Click(object sender, EventArgs e)
        {
            _portInfo.com = cmb_Com.Text;
            _portInfo.baud = cmb_Bau.Text;
            _portInfo.byteSize = cmb_Byte.Text;
            _portInfo.parity = cmb_Parity.Text;
            _portInfo.stopBits = cmb_Stop.Text;
        }
        /// <summary>
        /// 初始化下拉列表
        /// </summary>
        private void Init(string[] strCom, PortInfo port)
        {
            if (strCom.Length > 0)
            {
                foreach (string str in strCom)
                {
                    cmb_Com.Items.Add(str);
                    if (str == port.com)
                        cmb_Com.Text = str;
                    else
                        cmb_Com.SelectedIndex = 0;
                }
            }
            //加载波特率列表
            cmb_Bau.Items.Add("1200");
            cmb_Bau.Items.Add("2400");
            cmb_Bau.Items.Add("4800");
            cmb_Bau.Items.Add("9600");
            cmb_Bau.Items.Add("14400");
            cmb_Bau.Items.Add("19200");
            cmb_Bau.Items.Add("38400");
            cmb_Bau.Items.Add("57600");
            cmb_Bau.Items.Add("115200");
            cmb_Bau.Items.Add("128000");
            cmb_Bau.Text = port.baud;
            //加载校验位列表
            cmb_Parity.Items.Add("None");
            cmb_Parity.Items.Add("Odd");
            cmb_Parity.Items.Add("Even");
            cmb_Parity.Items.Add("Mark");
            cmb_Parity.Items.Add("Space");
            cmb_Parity.Text = port.parity;
            //加载数据位列表
            for (int i = 5; i < 9; i++)
                cmb_Byte.Items.Add(i.ToString());
            cmb_Byte.Text = port.byteSize;
            //加载停止位列表
            cmb_Stop.Items.Add("One");
            cmb_Stop.Items.Add("Two");
            cmb_Stop.Items.Add("OnePointFive");
            cmb_Stop.Text = port.stopBits;
        }
        /// <summary>
        /// 加载通讯设置
        /// </summary>
        public void GetPort()
        {
            if (string.IsNullOrEmpty(_portInfo.baud))
            {
                _portInfo.baud = "115200";
                _portInfo.parity = "None";
                _portInfo.byteSize = "8";
                _portInfo.stopBits = "One";
            }
            strCom = SerialPort.GetPortNames();
            if (string.IsNullOrEmpty(_portInfo.com))
            {
                if (strCom.Length > 0)
                    _portInfo.com = strCom[0];
            }
        }

        private void btn_open_comm_Click(object sender, EventArgs e)
        {
            if (!ValiDate()) return;
            else
            {
                dpjixing.Enabled = false;
                txtSn.Enabled = false;
                if (dpjixing.SelectedItem.ToString() == "E2教育机" || dpjixing.SelectedItem.ToString() == "商教机S1" || dpjixing.SelectedItem.ToString() == "3LCDE1" || dpjixing.SelectedItem.ToString() == "3LCDE3" || dpjixing.SelectedItem.ToString() == "影院光源X10" || dpjixing.SelectedItem.ToString() == "影院光源X20" || dpjixing.SelectedItem.ToString() == "影院光源X60" || dpjixing.SelectedItem.ToString() == "影院光源V36" || dpjixing.SelectedItem.ToString() == "影院光源23B" || dpjixing.SelectedItem.ToString() == "影院光源32B" || dpjixing.SelectedItem.ToString() == "影院光源E80" || dpjixing.SelectedItem.ToString() == "影院光源CP2215" || dpjixing.SelectedItem.ToString() == "舞台灯" || dpjixing.SelectedItem.ToString() == "MCP激光" || dpjixing.SelectedItem.ToString() == "ALPD4.0")
                {
                    Sp = new SerialPort();
                    Sp.BaudRate = Convert.ToInt32(cmb_Bau.Text.ToString());
                    //Sp.PortName = "COM7";
                    Sp.PortName = cmb_Com.Text.ToString();
                    Sp.DataBits = 8;
                    Sp.Open();//打开串口
                }
                else
                {
                    string errString = "";
                    GetPort();
                    if (strCom.Length < 1)
                    {
                        MessageBox.Show("本机没有COM口！", "提示", MessageBoxButtons.OK);
                        return;
                    }
                    else
                    {
                        _portInfo.com = cmb_Com.Text;
                        _portInfo.baud = cmb_Bau.Text;
                        _portInfo.byteSize = cmb_Byte.Text;
                        _portInfo.parity = cmb_Parity.Text;
                        _portInfo.stopBits = cmb_Stop.Text;
                        bool _result = spb.OpenPort(_portInfo, ref errString);
                    }
                }
            }
        }
        private void btn_close_comm_Click(object sender, EventArgs e)
        {

            if (dpjixing.SelectedItem.ToString() == "E2教育机" || dpjixing.SelectedItem.ToString() == "商教机S1" || dpjixing.SelectedItem.ToString() == "3LCDE1" || dpjixing.SelectedItem.ToString() == "3LCDE3" || dpjixing.SelectedItem.ToString() == "影院光源X10" || dpjixing.SelectedItem.ToString() == "影院光源X20" || dpjixing.SelectedItem.ToString() == "影院光源X60" || dpjixing.SelectedItem.ToString() == "影院光源V36" || dpjixing.SelectedItem.ToString() == "影院光源23B" || dpjixing.SelectedItem.ToString() == "影院光源32B" || dpjixing.SelectedItem.ToString() == "影院光源E80" || dpjixing.SelectedItem.ToString() == "影院光源CP2215" || dpjixing.SelectedItem.ToString() == "舞台灯" || dpjixing.SelectedItem.ToString() == "MCP激光" || dpjixing.SelectedItem.ToString() == "ALPD4.0")
            {
                dpjixing.Enabled = true;
                txtSn.Enabled = true;
                Sp.Close();//关闭串口
            }
            else
            {
                dpjixing.Enabled = true;
                txtSn.Enabled = true;
                string errString = "";
                if (strCom.Length < 1)
                {
                    MessageBox.Show("本机没有COM口！", "提示", MessageBoxButtons.OK);
                }
                else
                {
                    bool _result = spb.ClosePort(ref errString);
                }
            }
        }
        private bool ValiDate()
        {
            bool b = true;
            if (dpjixing.SelectedItem == null)
            {
                MessageBox.Show("请选择机型", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                dpjixing.Focus();
                b = false;
            }
            else if (string.IsNullOrWhiteSpace(txtSn.Text))
            {
                MessageBox.Show("请输入机器序号", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtSn.Focus();
                b = false;
            }
            return b;
        }
        public byte[] send = new byte[20];
        public void sendData()
        {
            send[0] = 0xAB;
            send[1] = 0x00;
            send[2] = 0x00;
            send[3] = 0x00;
            send[4] = 0x37;
            send[5] = 0x00;
            send[6] = 0x01;
            send[7] = 0x01;
            send[8] = 0x01;
            send[9] = 0x01;
            send[10] = 0x00;
            send[11] = 0x00;
            send[12] = 0x00;
            send[13] = 0x00;
            send[14] = 0x00;
            send[15] = 0x00;
            send[16] = 0x00;
            send[17] = 0x00;
            send[18] = 0x00;
            send[19] = 0xE6;

        }
        //工程机
        public byte[] send3 = new byte[20];
        public void sendData3()
        {
            send3[0] = 0xAF;
            send3[1] = 0x00;
            send3[2] = 0x00;
            send3[3] = 0x00;
            send3[4] = 0x37;
            send3[5] = 0x02;
            send3[6] = 0x00;
            send3[7] = 0x00;
            send3[8] = 0x00;
            send3[9] = 0x00;
            send3[10] = 0x00;
            send3[11] = 0x00;
            send3[12] = 0x00;
            send3[13] = 0x00;
            send3[14] = 0x00;
            send3[15] = 0x00;
            send3[16] = 0x00;
            send3[17] = 0x00;
            send3[18] = 0x00;
            send3[19] = 0xE8;
        }
        public byte[] send4 = new byte[20];
        public void sendData4()
        {
            send4[0] = 0xAF;
            send4[1] = 0x00;
            send4[2] = 0x00;
            send4[3] = 0x00;
            send4[4] = 0x39;
            send4[5] = 0x02;
            send4[6] = 0x00;
            send4[7] = 0x00;
            send4[8] = 0x00;
            send4[9] = 0x00;
            send4[10] = 0x00;
            send4[11] = 0x00;
            send4[12] = 0x00;
            send4[13] = 0x00;
            send4[14] = 0x00;
            send4[15] = 0x00;
            send4[16] = 0x00;
            send4[17] = 0x00;
            send4[18] = 0x00;
            send4[19] = 0xEA;
        }
        //机芯升级
        public byte[] send1 = new byte[16];
        public void sendData1()
        {

            send1[0] = 0x5A;
            send1[1] = 0x01;
            send1[2] = 0x01;
            send1[3] = 0x00;
            send1[4] = 0x1B;
            send1[5] = 0x00;
            send1[6] = 0x00;
            send1[7] = 0x00;
            send1[8] = 0x00;
            send1[9] = 0x00;
            send1[10] = 0x00;
            send1[11] = 0x00;
            send1[12] = 0x00;
            send1[13] = 0x00;
            send1[14] = 0x1B;
            send1[15] = 0xA5;
        }
        public byte[] send2 = new byte[16];
        public void sendData2()
        {

            send2[0] = 0x5A;
            send2[1] = 0x01;
            send2[2] = 0x01;
            send2[3] = 0x00;
            send2[4] = 0x1C;
            send2[5] = 0x00;
            send2[6] = 0x00;
            send2[7] = 0x00;
            send2[8] = 0x00;
            send2[9] = 0x00;
            send2[10] = 0x00;
            send2[11] = 0x00;
            send2[12] = 0x00;
            send2[13] = 0x00;
            send2[14] = 0x1C;
            send2[15] = 0xA5;
        }
        /*
        private void InitChart() {
            DateTime time = DateTime.Now;
            //  Series Random = chart1.Series[0];
            //   chart1.Series["DMD温度"].ChartType = SeriesChartType.Spline;
            //设置X轴的显示方式
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            chart1.Series[0].XValueMember = time.ToString();
            chart1.ChartAreas[0].AxisX.Interval = 10;
         chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
         chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;
           
            chart1.ChartAreas[0].AxisX.ScaleView.Size = 5;
            chart1.Series["DMD温度"].BorderWidth = 3;
            chart1.Series["激光器1温度"].BorderWidth = 3;
            chart1.Series["激光器2温度"].BorderWidth = 3;
            chart1.Series["环境温度"].BorderWidth = 3;
            // chart1.Series["DMD温度"].Points.AddXY(time,txtDMD.Text.ToString());                    
           chart1.Series["DMD温度"].Points.AddY(txtDMD.Text.ToString());
            chart1.Series["激光器1温度"].Points.AddY(txtjig1.Text.ToString());
           chart1.Series["激光器2温度"].Points.AddY(txtjig2.Text.ToString());
           chart1.Series["环境温度"].Points.AddY(txthuanwen.Text.ToString());
        }
   /*     private void InitChart1()
        {
            DateTime time = DateTime.Now;
            //  Series Random = chart1.Series[0];
            //   chart1.Series["DMD温度"].ChartType = SeriesChartType.Spline;
            //设置X轴的显示方式
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";
            chart1.Series[0].XValueMember = time.ToString();
            chart1.ChartAreas[0].AxisX.Interval = 10;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.Enabled = true;

            chart1.ChartAreas[0].AxisX.ScaleView.Size = 5;
            
            chart1.Series["激光器1温度"].BorderWidth = 3;
            chart1.Series["激光器2温度"].BorderWidth = 3;
            chart1.Series["环境温度"].BorderWidth = 3;
            // chart1.Series["DMD温度"].Points.AddXY(time,txtDMD.Text.ToString());                    
      
            chart1.Series["激光器1温度"].Points.AddY(txtjig1.Text.ToString());
            chart1.Series["激光器2温度"].Points.AddY(txtjig2.Text.ToString());
            chart1.Series["环境温度"].Points.AddY(txthuanwen.Text.ToString());
        }    
        */
        private void btn_send_byte_Click(object sender, EventArgs e)
        {
            string str = "Server=172.32.252.51;database=equipment;uid= sa;pwd=Dqa123456";
            SqlConnection conn = new SqlConnection(str);
            try
            {
                conn.Open();
            }
            catch (Exception)
            {
                MessageBox.Show("请确认连接到公司内部网络", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (dpjixing.SelectedItem.ToString() == "超薄拼墙")
            {
                bool result = false;
                byte[] receiveData = null;
                sendData();
                result = spb.SendCmd(send, out receiveData);
                if (receiveData != null)
                {
                    string strRevData = BytesToHexStr(receiveData);
                    //       label_recv_data.Text = strRevData.Replace("\r\n ", " <br/> ");
                    //  byte[] f = new byte[0];                           
                    txthuanwen.Text = receiveData[16].ToString();
                    txtDMD.Text = receiveData[17].ToString();
                    txtjig1.Text = receiveData[14].ToString();
                    txtjig2.Text = receiveData[15].ToString();
                    lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                    //  InitChart();
                    try
                    {
                        string strSQL = "insert into dbo.pingqiangmessage(jigwendu1,jigwendu2,dmdwendu,huanwen,sn,date)values('" + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "','" + this.txtDMD.Text.Trim().ToString() + "','" + this.txthuanwen.Text.Trim().ToString() + "','" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "')";
                        if (txthuanwen.Text != "")
                        {
                            SqlCommand mycom = new SqlCommand(strSQL, conn);
                            mycom.ExecuteNonQuery();
                        }
                        if (txtTime.Text == "")
                        {
                            timer1.Interval = 30000;
                        }
                        else if (txtTime.Text != "")
                        {
                            timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                        }

                    }
                    catch
                    {

                    }
                    timer1.Start();
                    conn.Close();
                }

            }
            else if (dpjixing.SelectedItem.ToString() == "机芯升级")
            {
                bool result1 = false;
                bool result2 = false;
                byte[] receiveData1 = null;
                byte[] receiveData2 = null;
                // SendData = HexStringToByteArray(txt_send_byte.Text);
                sendData1();
                result1 = spb.SendCmd1(send1, out receiveData1);
                Thread.Sleep(250);//延时  
                sendData2();
                result2 = spb.SendCmd2(send2, out receiveData2);
                if (receiveData1 != null && receiveData2 != null)
                {
                    string strRevData1 = BytesToHexStr(receiveData1);
                    txtjig1.Text = receiveData1[7].ToString();
                    txtjig2.Text = receiveData1[8].ToString();
                    txthuanwen.Text = receiveData2[5].ToString();
                    txtDMD.Text = receiveData2[6].ToString();
                    lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                    //InitChart();
                    try
                    {
                        string strSQL = "insert into dbo.jixinshengji(jigwendu1,jigwendu2,dmdwendu,huanwen,sn,date)values('" + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "','" + this.txtDMD.Text.Trim().ToString() + "','" + this.txthuanwen.Text.Trim().ToString() + "','" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "')";
                        if (txthuanwen.Text != "")
                        {
                            SqlCommand mycom = new SqlCommand(strSQL, conn);
                            mycom.ExecuteNonQuery();
                        }
                        if (txtTime.Text == "")
                        {
                            timer1.Interval = 30000;
                        }
                        else if (txtTime.Text != "")
                        {
                            timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                        }
                    }
                    catch
                    {

                    }
                    timer1.Start();
                    conn.Close();
                }
            }
            //工程机L2
            else if (dpjixing.SelectedItem.ToString() == "工程机L2")
            {
                bool result = false;
                bool result1 = false;
                byte[] receiveData = null;
                byte[] receiveData4 = null;
                sendData3();
                result = spb.SendCmd3(send3, out receiveData);
                Thread.Sleep(250);//延时
                sendData4();
                result1 = spb.SendCmd4(send4, out receiveData4);
                if (receiveData != null && receiveData4 != null)
                {
                    string strRevData = BytesToHexStr(receiveData);
                    //       label_recv_data.Text = strRevData.Replace("\r\n ", " <br/> ");
                    //  byte[] f = new byte[0];
                    //温度信息
                    txtDMD.Text = receiveData[8].ToString();
                    txthuanwen.Text = receiveData[9].ToString();
                    txtselun.Text = receiveData[10].ToString();
                    txtjig1.Text = receiveData[11].ToString();
                    txtjig2.Text = receiveData[12].ToString();
                    txtjig3.Text = receiveData[13].ToString();
                    txtjig4.Text = receiveData[14].ToString();
                    txtjig5.Text = receiveData[15].ToString();
                    txtjig6.Text = receiveData[16].ToString();
                    txtjig7.Text = receiveData[17].ToString();
                    txtjig8.Text = receiveData[18].ToString();
                    //风扇信息
                    txtfs1.Text = receiveData4[9].ToString() + "00";
                    txtfs2.Text = receiveData4[10].ToString() + "00";
                    txtfs3.Text = receiveData4[11].ToString() + "00";
                    txtfs4.Text = receiveData4[12].ToString() + "00";
                    txtfs5.Text = receiveData4[13].ToString() + "00";
                    txtfs6.Text = receiveData4[14].ToString() + "00";
                    txtfs7.Text = receiveData4[15].ToString() + "00";
                    txtfs8.Text = receiveData4[16].ToString() + "00";
                    txtldcgq.Text = receiveData4[17].ToString() + "00";
                    txtldbfb.Text = receiveData4[18].ToString() + "00";
                    lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                    //   InitChart();
                    try
                    {
                        string strSQL = "insert into dbo.L2message(jigwendu1,jigwendu2,jigwendu3,jigwendu4,jigwendu5,jigwendu6,jigwendu7,jigwendu8,fengshan1,fengshan2,fengshan3,fengshan4,fengshan5,fengshan6,fengshan7,fengshan8,fengshan9,fengshan10,selunwendu,dmdwendu,huanwen,sn,date)values ('" + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "', '" + this.txtjig3.Text.Trim().ToString() + "', '" + this.txtjig4.Text.Trim().ToString() + "', '" + this.txtjig5.Text.Trim().ToString() + "', '" + this.txtjig6.Text.Trim().ToString() + "', '" + this.txtjig7.Text.Trim().ToString() + "', '" + this.txtjig8.Text.Trim().ToString() + "', '" + this.txtfs1.Text.Trim().ToString() + "', '" + this.txtfs2.Text.Trim().ToString() + "', '" + this.txtfs3.Text.Trim().ToString() + "', '" + this.txtfs4.Text.Trim().ToString() + "', '" + this.txtfs5.Text.Trim().ToString() + "', '" + this.txtfs6.Text.Trim().ToString() + "', '" + this.txtfs7.Text.Trim().ToString() + "', '" + this.txtfs8.Text.Trim().ToString() + "', '" + this.txtldcgq.Text.Trim().ToString() + "', '" + this.txtldbfb.Text.Trim().ToString() + "', '" + this.txtselun.Text.Trim().ToString() + "', '" + this.txtDMD.Text.Trim().ToString() + "', '" + this.txthuanwen.Text.Trim().ToString() + "', '" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "')";
                        if (txthuanwen.Text != "")
                        {
                            SqlCommand mycom = new SqlCommand(strSQL, conn);
                            mycom.ExecuteNonQuery();
                        }
                        if (txtTime.Text == "")
                        {
                            timer1.Interval = 30000;
                        }
                        else if (txtTime.Text != "")
                        {
                            timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                        }

                    }
                    catch
                    {

                    }
                    timer1.Start();
                    conn.Close();
                }
            }
            else if (dpjixing.SelectedItem.ToString() == "工程机L1")
            {

                bool result = false;
                bool result1 = false;
                byte[] receiveData = null;
                byte[] receiveData4 = null;
                sendData3();
                result = spb.SendCmd3(send3, out receiveData);
                Thread.Sleep(250);//延时
                sendData4();
                result1 = spb.SendCmd4(send4, out receiveData4);

                if (receiveData != null && receiveData4 != null)
                {

                    string strRevData = BytesToHexStr(receiveData);

                    //       label_recv_data.Text = strRevData.Replace("\r\n ", " <br/> ");
                    //  byte[] f = new byte[0];
                    //温度信息
                    txtDMD.Text = receiveData[8].ToString();
                    txthuanwen.Text = receiveData[9].ToString();
                    txtselun.Text = receiveData[10].ToString();
                    txtjig1.Text = receiveData[11].ToString();
                    txtjig2.Text = receiveData[12].ToString();
                    txtjig3.Text = receiveData[13].ToString();
                    //风扇信息
                    txtfs1.Text = receiveData4[9].ToString() + "00";
                    txtfs2.Text = receiveData4[10].ToString() + "00";
                    txtfs3.Text = receiveData4[11].ToString() + "00";
                    txtfs6.Text = receiveData4[14].ToString() + "00";
                    lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                    //   InitChart();
                    try
                    {
                        string strSQL = "insert into dbo.L1message(jigwendu1,jigwendu2,jigwendu3,fengshan1,fengshan2,fengshan3,fengshan6,selunwendu,dmdwendu,huanwen,sn,date)values ('" + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "', '" + this.txtjig3.Text.Trim().ToString() + "', '" + this.txtfs1.Text.Trim().ToString() + "', '" + this.txtfs2.Text.Trim().ToString() + "', '" + this.txtfs3.Text.Trim().ToString() + "', '" + this.txtfs6.Text.Trim().ToString() + "', '" + this.txtselun.Text.Trim().ToString() + "', '" + this.txtDMD.Text.Trim().ToString() + "', '" + this.txthuanwen.Text.Trim().ToString() + "', '" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "')";
                        if (txthuanwen.Text != "")
                        {
                            SqlCommand mycom = new SqlCommand(strSQL, conn);
                            mycom.ExecuteNonQuery();
                        }
                        if (txtTime.Text == "")
                        {
                            timer1.Interval = 30000;
                        }
                        else if (txtTime.Text != "")
                        {
                            timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                        }

                    }
                    catch
                    {

                    }
                    timer1.Start();
                    conn.Close();
                }
            }
            else if (dpjixing.SelectedItem.ToString() == "E2教育机")
            {
                Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived);
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                //  InitChart1();
                try
                {
                    string strSQL = "insert into dbo.messageE2(jigwendu1,jigwendu2,huanwen,sn,date)values('" + this.txtjig1.Text.Trim().ToString() + "','"
                    + this.txtjig2.Text.Trim().ToString() + "','" + this.txthuanwen.Text.Trim().ToString() + "','" + this.txtSn.Text.Trim().ToString() + "','"
                    + this.lblshijian.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }

                }
                catch
                {

                }
                timer1.Start();
                conn.Close();
            }
            else if (dpjixing.SelectedItem.ToString() == "商教机S1")
            {
                Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived1);
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                //  InitChart1();
                try
                {
                    string strSQL = "insert into dbo.messageS1(jigwendu1,jigwendu2,jigwendu3,jigwend4,huanwen,selunwendu,sn,date)values('" + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "','" + this.txthuanwen.Text.Trim().ToString() + "','" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }
                }
                catch
                {

                }
                timer1.Start();
                conn.Close();
            }
            //3LCDE1
            else if (dpjixing.SelectedItem.ToString() == "3LCDE1")
            {
                //获取温度数据
                List<byte> buffer = new List<byte>(4096);
                byte[] send = new byte[4];
                send[0] = 0x47;
                send[1] = 0x52;
                send[2] = 0x36;
                send[3] = 0x0D;
                Sp.Write(send, 0, send.Length);
                byte[] buf = new byte[18];
                byte[] readBuffer = new byte[18];
                // readBuffer = null;
                string msg = BytesToHexStr(send);
                Thread.Sleep(250);//延时                  
                buf = null;
                buf = new byte[Sp.BytesToRead];

                Sp.Read(buf, 0, buf.Length);
                buffer.AddRange(buf);
                if (buffer.Count >= 18)
                {
                    buffer.CopyTo(0, readBuffer, 0, readBuffer.Length);
                    byte[] recvData1 = new byte[5];
                    byte[] recvData2 = new byte[5];
                    byte[] recvData3 = new byte[5];
                    buffer.CopyTo(1, recvData1, 0, 5);
                    buffer.CopyTo(7, recvData2, 0, 5);
                    buffer.CopyTo(13, recvData3, 0, 5);
                    string recv = System.Text.Encoding.Default.GetString(recvData1);
                    string recv1 = System.Text.Encoding.Default.GetString(recvData2);
                    string recv2 = System.Text.Encoding.Default.GetString(recvData3);
                    txthuanwen.Text = recv.ToString();
                    txtpwendu.Text = recv1.ToString();
                    txtjig1.Text = recv2.ToString();
                }
                Thread.Sleep(250);//延时  
                //获取风扇数据
                byte[] send1 = new byte[12];
                send1[0] = 0x67;
                send1[1] = 0x63;
                send1[2] = 0x73;
                send1[3] = 0x67;
                send1[4] = 0x65;
                send1[5] = 0x74;
                send1[6] = 0x66;
                send1[7] = 0x61;
                send1[8] = 0x6E;
                send1[9] = 0x20;
                send1[10] = 0x30;
                send1[11] = 0x0D;
                Sp.Write(send1, 0, send1.Length);
                byte[] buf1 = new byte[15];
                byte[] readBuffer1 = new byte[15];
                // readBuffer = null;
                string msg1 = BytesToHexStr(send1);
                Thread.Sleep(250);//延时                  
                buf1 = null;
                buf1 = new byte[Sp.BytesToRead];
                Sp.Read(buf1, 0, buf1.Length);
                string str1 = Encoding.Default.GetString(buf1);
                if (str1.Length > 4)
                {
                    string str2 = str1.Substring(0, 4);
                    txtfs1.Text = str2;
                }
                Thread.Sleep(250);//延时  
                byte[] send2 = new byte[12];
                send2[0] = 0x67;
                send2[1] = 0x63;
                send2[2] = 0x73;
                send2[3] = 0x67;
                send2[4] = 0x65;
                send2[5] = 0x74;
                send2[6] = 0x66;
                send2[7] = 0x61;
                send2[8] = 0x6E;
                send2[9] = 0x20;
                send2[10] = 0x31;
                send2[11] = 0x0D;
                Sp.Write(send2, 0, send2.Length);
                byte[] buf2 = new byte[15];
                byte[] readBuffer2 = new byte[15];
                // readBuffer = null;
                string msg2 = BytesToHexStr(send2);
                Thread.Sleep(250);//延时                  
                buf2 = null;
                buf2 = new byte[Sp.BytesToRead];
                Sp.Read(buf2, 0, buf2.Length);
                if (buf2.Length >= 15)
                {
                    string str3 = Encoding.Default.GetString(buf2);
                    string str4 = str3.Substring(0, 4);
                    txtfs2.Text = str4;
                }
                //第3个风扇数据
                Thread.Sleep(250);//延时  
                byte[] send3 = new byte[12];
                send3[0] = 0x67;
                send3[1] = 0x63;
                send3[2] = 0x73;
                send3[3] = 0x67;
                send3[4] = 0x65;
                send3[5] = 0x74;
                send3[6] = 0x66;
                send3[7] = 0x61;
                send3[8] = 0x6E;
                send3[9] = 0x20;
                send3[10] = 0x33;
                send3[11] = 0x0D;
                Sp.Write(send3, 0, send3.Length);
                byte[] buf3 = new byte[15];
                byte[] readBuffer3 = new byte[15];
                // readBuffer = null;
                string msg3 = BytesToHexStr(send3);
                Thread.Sleep(250);//延时                  
                buf3 = null;
                buf3 = new byte[Sp.BytesToRead];
                Sp.Read(buf3, 0, buf3.Length);
                if (buf3.Length >= 15)
                {
                    string str5 = Encoding.Default.GetString(buf3);
                    string str6 = str5.Substring(0, 4);
                    txtfs3.Text = str6;
                }
                Thread.Sleep(250);//延时  
                byte[] send4 = new byte[12];
                send4[0] = 0x67;
                send4[1] = 0x63;
                send4[2] = 0x73;
                send4[3] = 0x67;
                send4[4] = 0x65;
                send4[5] = 0x74;
                send4[6] = 0x66;
                send4[7] = 0x61;
                send4[8] = 0x6E;
                send4[9] = 0x20;
                send4[10] = 0x34;
                send4[11] = 0x0D;
                Sp.Write(send4, 0, send4.Length);
                byte[] buf4 = new byte[15];
                byte[] readBuffer4 = new byte[15];
                // readBuffer = null;
                string msg4 = BytesToHexStr(send4);
                Thread.Sleep(250);//延时                  
                buf4 = null;
                buf4 = new byte[Sp.BytesToRead];
                Sp.Read(buf4, 0, buf4.Length);
                if (buf4.Length >= 15)
                {
                    string str7 = Encoding.Default.GetString(buf4);
                    string str8 = str7.Substring(0, 4);
                    txtfs4.Text = str8;
                }
                Thread.Sleep(250);//延时  
                byte[] send5 = new byte[12];
                send5[0] = 0x67;
                send5[1] = 0x63;
                send5[2] = 0x73;
                send5[3] = 0x67;
                send5[4] = 0x65;
                send5[5] = 0x74;
                send5[6] = 0x66;
                send5[7] = 0x61;
                send5[8] = 0x6E;
                send5[9] = 0x20;
                send5[10] = 0x35;
                send5[11] = 0x0D;
                Sp.Write(send5, 0, send5.Length);
                byte[] buf5 = new byte[15];
                byte[] readBuffer5 = new byte[15];
                // readBuffer = null;
                string msg5 = BytesToHexStr(send5);
                Thread.Sleep(250);//延时                  
                buf5 = null;
                buf5 = new byte[Sp.BytesToRead];
                Sp.Read(buf5, 0, buf5.Length);
                if (buf5.Length >= 15)
                {
                    string str9 = Encoding.Default.GetString(buf5);
                    string str10 = str9.Substring(0, 4);
                    txtfs5.Text = str10;
                }
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                try
                {
                    string strSQL = "insert into dbo.messageE1(jigwendu1,fengshan1,fengshan2,fengshan3,fengshan4,fengshan5,pingwendu,huanwen,sn,date)values ('" + this.txtjig1.Text.Trim().ToString() + "','" + this.txtfs1.Text.Trim().ToString() + "', '" + this.txtfs2.Text.Trim().ToString() + "', '" + this.txtfs3.Text.Trim().ToString() + "', '" + this.txtfs4.Text.Trim().ToString() + "', '" + this.txtfs5.Text.Trim().ToString() + "', '" + this.txtpwendu.Text.Trim().ToString() + "', '" + this.txthuanwen.Text.Trim().ToString() + "', '" + this.txtSn.Text.Trim().ToString() + "', '" + this.lblshijian.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }

                }
                catch
                {

                }
                timer1.Start();
                conn.Close();
            }
            else if (dpjixing.SelectedItem.ToString() == "3LCDE3")
            {
                //获取温度数据
                List<byte> buffer = new List<byte>(4096);
                byte[] send = new byte[4];
                send[0] = 0x47;
                send[1] = 0x52;
                send[2] = 0x36;
                send[3] = 0x0D;
                Sp.Write(send, 0, send.Length);
                byte[] buf = new byte[15];
                byte[] readBuffer = new byte[15];
                // readBuffer = null;
                string msg = BytesToHexStr(send);
                Thread.Sleep(250);//延时                  
                buf = null;
                buf = new byte[Sp.BytesToRead];
                Sp.Read(buf, 0, buf.Length);
                buffer.AddRange(buf);
                if (buffer.Count >= 15)
                {
                    buffer.CopyTo(0, readBuffer, 0, readBuffer.Length);
                    byte[] recvData1 = new byte[4];
                    byte[] recvData2 = new byte[4];
                    byte[] recvData3 = new byte[4];
                    buffer.CopyTo(0, recvData1, 0, 4);
                    buffer.CopyTo(5, recvData2, 0, 4);
                    buffer.CopyTo(10, recvData3, 0, 4);
                    string recv = System.Text.Encoding.Default.GetString(recvData1);
                    string recv1 = System.Text.Encoding.Default.GetString(recvData2);
                    string recv2 = System.Text.Encoding.Default.GetString(recvData3);
                    txthuanwen.Text = recv.ToString();
                    txtpwendu.Text = recv1.ToString();
                    txtjig1.Text = recv2.ToString();
                }
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                try
                {
                    string strSQL = "insert into dbo.message3LCDE3(jigwendu1,pingwendu,huanwen,sn,date)values ('" + this.txtjig1.Text.Trim().ToString() + "','" + this.txtpwendu.Text.Trim().ToString() + "', '" + this.txthuanwen.Text.Trim().ToString() + "', '" + this.txtSn.Text.Trim().ToString() + "', '" + this.lblshijian.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }

                }
                catch
                {

                }
                timer1.Start();
                conn.Close();
            }
            else if (dpjixing.SelectedItem.ToString() == "影院光源X10")
            {
                Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived2);
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                try
                {
                    string strSQL = "insert into dbo.messageCP2220(ljigwendu1,ljigwendu2,ljigwendu3,ljigwendu4,ljigwendu5,ljigwendu6,ljigwendu7,ljigwendu8,ljigwendu9,ljigwendu10,ljigwendu11,ljigwendu12,ljigwendu13,ljigwendu14,ljigwendu15,ljigwendu16,ljigwendu17,ljigwendu18,ljigwendu19,ljigwendu20,ljigwendu21,ljigwendu22,ljigwendu23,ljigwendu24,hongjigwendu1,hongjigwendu2,ljigdianliu1,ljigdianliu2,ljigdianliu3,ljigdianliu4,ljigdianliu5,ljigdianliu6,ljigdianliu7,ljigdianliu8,ljigdianliu9,ljigdianliu10,ljigdianliu11,ljigdianliu12,ljigdianliu13,ljigdianliu14,ljigdianliu15,ljigdianliu16,ljigdianliu17,ljigdianliu18,ljigdianliu19,ljigdianliu20,ljigdianliu21,ljigdianliu22,ljigdianliu23,ljigdianliu24,hongjigdianliu1,hongjigdianliu2,dianyuanwendu1,dianyuanwendu2,dianyuanwendu3,dianyuanwendu4,dianyuanwendu5,dianyuanwendu6,shuibengzhuansu,shuibengzhuansu2,shuibengzhuansu3,shuibengzhuansu4,shuibengzhuansu5,madawendu,madazhuansu,hongjigqiangtishidu,hongjigqiangtiwendu,lmwendu1,lmwendu2,rmwendu1,rmwendu2,huanwen,sn,date,dianyuanfengshan1,dianyuanfengshan2,dianyuanfengshan3,dianyuanfengshan4)values('"
                    + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "','" + this.txtjig3.Text.Trim().ToString() + "','" + this.txtjig4.Text.Trim().ToString() + "','" + this.txtjig5.Text.Trim().ToString() + "','" + this.txtjig6.Text.Trim().ToString() + "','" + this.txtjig7.Text.Trim().ToString() + "','" + this.txtjig8.Text.Trim().ToString() + "','" + this.txtjig9.Text.Trim().ToString() + "','" + this.txtjig10.Text.Trim().ToString() + "','" + this.txtjig11.Text.Trim().ToString() + "','" + this.txtjig12.Text.Trim().ToString() + "','" + this.txtjig13.Text.Trim().ToString() + "','" + this.txtjig14.Text.Trim().ToString() + "','" + this.txtjig15.Text.Trim().ToString() + "','" + this.txtjig16.Text.Trim().ToString() + "','" + this.txtjig17.Text.Trim().ToString() + "','" + this.txtjig18.Text.Trim().ToString() + "','" + this.txtjig19.Text.Trim().ToString() + "','" + this.txtjig20.Text.Trim().ToString() + "','" + this.txtjig21.Text.Trim().ToString() + "','" + this.txtjig22.Text.Trim().ToString() + "','" + this.txtjig23.Text.Trim().ToString() + "','" + this.txtjig24.Text.Trim().ToString() + "','"
                    + this.txthjig1.Text.Trim().ToString() + "','" + this.txthjig2.Text.Trim().ToString() + "','" + this.txtljgdianliu1.Text.Trim().ToString() + "','" + this.txtljgdianliu2.Text.Trim().ToString() + "','" + this.txtljgdianliu3.Text.Trim().ToString() + "','" + this.txtljgdianliu4.Text.Trim().ToString() + "','" + this.txtljgdianliu5.Text.Trim().ToString() + "','" + this.txtljgdianliu6.Text.Trim().ToString() + "','" + this.txtljgdianliu7.Text.Trim().ToString() + "','" + this.txtljgdianliu8.Text.Trim().ToString() + "','" + this.txtljgdianliu9.Text.Trim().ToString() + "','" + this.txtljgdianliu10.Text.Trim().ToString() + "','" + this.txtljgdianliu11.Text.Trim().ToString() + "','" + this.txtljgdianliu12.Text.Trim().ToString() + "','" + this.txtljgdianliu13.Text.Trim().ToString() + "','" + this.txtljgdianliu14.Text.Trim().ToString() + "','" + this.txtljgdianliu15.Text.Trim().ToString() + "','" + this.txtljgdianliu16.Text.Trim().ToString() + "','" + this.txtljgdianliu17.Text.Trim().ToString() + "','" + this.txtljgdianliu18.Text.Trim().ToString() + "','" + this.txtljgdianliu19.Text.Trim().ToString() + "','" + this.txtljgdianliu20.Text.Trim().ToString() + "','" + this.txtljgdianliu21.Text.Trim().ToString() + "','" + this.txtljgdianliu22.Text.Trim().ToString() + "','" + this.txtljgdianliu23.Text.Trim().ToString() + "','" + this.txtljgdianliu24.Text.Trim().ToString() + "','" + this.txthjgdianliu1.Text.Trim().ToString() + "','" + this.txthjgdianliu2.Text.Trim().ToString() + "','"
                    + this.txtdywendu1.Text.Trim().ToString() + "','" + this.txtdywendu2.Text.Trim().ToString() + "','" + this.txtdywendu3.Text.Trim().ToString() + "','" + this.txtslfs6.Text.Trim().ToString() + "','" + this.txtdywendu5.Text.Trim().ToString() + "','" + this.txtdywendu6.Text.Trim().ToString() + "','" + this.txtsbzhuansu.Text.Trim().ToString() + "','" + this.txtslfs1.Text.Trim().ToString() + "','" + this.txtslfs2.Text.Trim().ToString() + "','"
                    + this.txtslfs3.Text.Trim().ToString() + "','" + this.txtslfs4.Text.Trim().ToString() + "','" + this.txtmdwendu.Text.Trim().ToString() + "','" + this.txtmdzhuansu.Text.Trim().ToString() + "','" + this.txtqtshidu.Text.Trim().ToString() + "','" + this.txtqtwendu.Text.Trim().ToString() + "','" + this.txtlmwendu1.Text.Trim().ToString() + "','" + this.txtlmwendu2.Text.Trim().ToString() + "','" + this.txtrmwendu1.Text.Trim().ToString() + "','" + this.txtrmwendu2.Text.Trim().ToString() + "','" + this.txthuanwen.Text.Trim().ToString() + "','" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "','" + this.txtfs1.Text.Trim().ToString() + "','" + this.txtfs2.Text.Trim().ToString() + "','" + this.txtfs3.Text.Trim().ToString() + "','" + this.txtfs4.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }

                }
                catch
                {

                }
                timer1.Start();
                conn.Close();
            }
            else if (dpjixing.SelectedItem.ToString() == "影院光源X20")
            {
                Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived3);
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                try
                {
                    string strSQL = "insert into dbo.messageCP2220(ljigwendu1,ljigwendu2,ljigwendu3,ljigwendu4,ljigdianliu1,ljigdianliu2,ljigdianliu3,ljigdianliu4,ljigdianliu5,ljigdianliu6,ljigdianliu7,ljigdianliu8,ljigdianliu9,ljigdianliu10,ljigdianliu11,ljigdianliu12,ljigdianliu13,ljigdianliu14,ljigdianliu15,ljigdianliu16,ljigdianliu17,ljigdianliu18,ljigdianliu19,ljigdianliu20,ljigdianliu21,ljigdianliu22,ljigdianliu23,ljigdianliu24,shuibengzhuansu,shuibengzhuansu2,shuibengzhuansu3,shuibengzhuansu4,shuibengzhuansu5,madawendu,madazhuansu,hongjigqiangtishidu,hongjigqiangtiwendu,rmwendu1,rmwendu2,huanwen,sn,date,dianyuanfengshan1,dianyuanfengshan2)values('"
                    + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "','" + this.txtjig3.Text.Trim().ToString() + "','" + this.txtjig4.Text.Trim().ToString() + "','" + this.txtljgdianliu1.Text.Trim().ToString() + "','" + this.txtljgdianliu2.Text.Trim().ToString() + "','" + this.txtljgdianliu3.Text.Trim().ToString() + "','" + this.txtljgdianliu4.Text.Trim().ToString() + "','" + this.txtljgdianliu5.Text.Trim().ToString() + "','" + this.txtljgdianliu6.Text.Trim().ToString() + "','" + this.txtljgdianliu7.Text.Trim().ToString() + "','" + this.txtljgdianliu8.Text.Trim().ToString() + "','" + this.txtljgdianliu9.Text.Trim().ToString() + "','" + this.txtljgdianliu10.Text.Trim().ToString() + "','" + this.txtljgdianliu11.Text.Trim().ToString() + "','" + this.txtljgdianliu12.Text.Trim().ToString() + "','" + this.txtljgdianliu13.Text.Trim().ToString() + "','" + this.txtljgdianliu14.Text.Trim().ToString() + "','" + this.txtljgdianliu15.Text.Trim().ToString() + "','" + this.txtljgdianliu16.Text.Trim().ToString() + "','" + this.txtljgdianliu17.Text.Trim().ToString() + "','" + this.txtljgdianliu18.Text.Trim().ToString() + "','" + this.txtljgdianliu19.Text.Trim().ToString() + "','" + this.txtljgdianliu20.Text.Trim().ToString() + "','" + this.txtljgdianliu21.Text.Trim().ToString() + "','" + this.txtljgdianliu22.Text.Trim().ToString() + "','" + this.txtljgdianliu23.Text.Trim().ToString() + "','" + this.txtljgdianliu24.Text.Trim().ToString() + "','"
                    + this.txtsbzhuansu.Text.Trim().ToString() + "','" + this.txtslfs1.Text.Trim().ToString() + "','" + this.txtslfs2.Text.Trim().ToString() + "','" + this.txtslfs3.Text.Trim().ToString() + "','" + this.txtslfs4.Text.Trim().ToString() + "','" + this.txtmdwendu.Text.Trim().ToString() + "','" + this.txtmdzhuansu.Text.Trim().ToString() + "','" + this.txtmdwendu2.Text.Trim().ToString() + "','" + this.txtmdzhuansu2.Text.Trim().ToString() + "','" + this.txtldcgq.Text.Trim().ToString() + "','" + this.txtldbfb.Text.Trim().ToString() + "','" + this.txthuanwen.Text.Trim().ToString() + "','" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "','" + this.txtfs1.Text.Trim().ToString() + "','" + this.txtfs2.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }

                }
                catch
                {

                }
                timer1.Start();
                conn.Close();
            }
            else if (dpjixing.SelectedItem.ToString() == "影院光源X60")
            {
                Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived4);
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                try
                {
                    string strSQL = "insert into dbo.messageCP2220(ljigwendu1,ljigwendu2,ljigwendu3,ljigwendu4,ljigwendu5,ljigwendu6,ljigwendu7,ljigwendu8,ljigwendu9,ljigwendu10,ljigwendu11,ljigwendu12,ljigwendu13,ljigwendu14,ljigwendu15,ljigwendu16,ljigwendu17,ljigwendu18,ljigwendu19,ljigwendu20,ljigwendu21,ljigwendu22,ljigwendu23,ljigwendu24,hongjigwendu1,hongjigwendu2,ljigdianliu1,ljigdianliu2,ljigdianliu3,ljigdianliu4,ljigdianliu5,ljigdianliu6,ljigdianliu7,ljigdianliu8,ljigdianliu9,ljigdianliu10,ljigdianliu11,ljigdianliu12,ljigdianliu13,ljigdianliu14,ljigdianliu15,ljigdianliu16,ljigdianliu17,ljigdianliu18,ljigdianliu19,ljigdianliu20,ljigdianliu21,ljigdianliu22,ljigdianliu23,ljigdianliu24,hongjigdianliu1,hongjigdianliu2,dianyuanwendu1,dianyuanwendu2,dianyuanwendu3,dianyuanwendu4,dianyuanwendu5,dianyuanwendu6,shuibengzhuansu,shuibengzhuansu2,shuibengzhuansu3,shuibengzhuansu4,shuibengzhuansu5,madawendu,madazhuansu,hongjigqiangtishidu,hongjigqiangtiwendu,lmwendu1,lmwendu2,rmwendu1,rmwendu2,huanwen,sn,date,dianyuanfengshan1,dianyuanfengshan2,dianyuanfengshan3,dianyuanfengshan4)values('"
                    + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "','" + this.txtjig3.Text.Trim().ToString() + "','" + this.txtjig4.Text.Trim().ToString() + "','" + this.txtjig5.Text.Trim().ToString() + "','" + this.txtjig6.Text.Trim().ToString() + "','" + this.txtjig7.Text.Trim().ToString() + "','" + this.txtjig8.Text.Trim().ToString() + "','" + this.txtjig9.Text.Trim().ToString() + "','" + this.txtjig10.Text.Trim().ToString() + "','" + this.txtjig11.Text.Trim().ToString() + "','" + this.txtjig12.Text.Trim().ToString() + "','" + this.txtjig13.Text.Trim().ToString() + "','" + this.txtjig14.Text.Trim().ToString() + "','" + this.txtjig15.Text.Trim().ToString() + "','" + this.txtjig16.Text.Trim().ToString() + "','" + this.txtjig17.Text.Trim().ToString() + "','" + this.txtjig18.Text.Trim().ToString() + "','" + this.txtjig19.Text.Trim().ToString() + "','" + this.txtjig20.Text.Trim().ToString() + "','" + this.txtjig21.Text.Trim().ToString() + "','" + this.txtjig22.Text.Trim().ToString() + "','" + this.txtjig23.Text.Trim().ToString() + "','" + this.txtjig24.Text.Trim().ToString() + "','"
                    + this.txthjig1.Text.Trim().ToString() + "','" + this.txthjig2.Text.Trim().ToString() + "','" + this.txtljgdianliu1.Text.Trim().ToString() + "','" + this.txtljgdianliu2.Text.Trim().ToString() + "','" + this.txtljgdianliu3.Text.Trim().ToString() + "','" + this.txtljgdianliu4.Text.Trim().ToString() + "','" + this.txtljgdianliu5.Text.Trim().ToString() + "','" + this.txtljgdianliu6.Text.Trim().ToString() + "','" + this.txtljgdianliu7.Text.Trim().ToString() + "','" + this.txtljgdianliu8.Text.Trim().ToString() + "','" + this.txtljgdianliu9.Text.Trim().ToString() + "','" + this.txtljgdianliu10.Text.Trim().ToString() + "','" + this.txtljgdianliu11.Text.Trim().ToString() + "','" + this.txtljgdianliu12.Text.Trim().ToString() + "','" + this.txtljgdianliu13.Text.Trim().ToString() + "','" + this.txtljgdianliu14.Text.Trim().ToString() + "','" + this.txtljgdianliu15.Text.Trim().ToString() + "','" + this.txtljgdianliu16.Text.Trim().ToString() + "','" + this.txtljgdianliu17.Text.Trim().ToString() + "','" + this.txtljgdianliu18.Text.Trim().ToString() + "','" + this.txtljgdianliu19.Text.Trim().ToString() + "','" + this.txtljgdianliu20.Text.Trim().ToString() + "','" + this.txtljgdianliu21.Text.Trim().ToString() + "','" + this.txtljgdianliu22.Text.Trim().ToString() + "','" + this.txtljgdianliu23.Text.Trim().ToString() + "','" + this.txtljgdianliu24.Text.Trim().ToString() + "','" + this.txthjgdianliu1.Text.Trim().ToString() + "','" + this.txthjgdianliu2.Text.Trim().ToString() + "','"
                    + this.txtdywendu1.Text.Trim().ToString() + "','" + this.txtdywendu2.Text.Trim().ToString() + "','" + this.txtdywendu3.Text.Trim().ToString() + "','" + this.txtdywendu4.Text.Trim().ToString() + "','" + this.txtdywendu5.Text.Trim().ToString() + "','" + this.txtdywendu6.Text.Trim().ToString() + "','" + this.txtsbzhuansu.Text.Trim().ToString() + "','" + this.txtslfs1.Text.Trim().ToString() + "','" + this.txtslfs2.Text.Trim().ToString() + "','"
                    + this.txtslfs3.Text.Trim().ToString() + "','" + this.txtslfs4.Text.Trim().ToString() + "','" + this.txtmdwendu.Text.Trim().ToString() + "','" + this.txtmdzhuansu.Text.Trim().ToString() + "','" + this.txtqtshidu.Text.Trim().ToString() + "','" + this.txtqtwendu.Text.Trim().ToString() + "','" + this.txtlmwendu1.Text.Trim().ToString() + "','" + this.txtlmwendu2.Text.Trim().ToString() + "','" + this.txtrmwendu1.Text.Trim().ToString() + "','" + this.txtrmwendu2.Text.Trim().ToString() + "','" + this.txthuanwen.Text.Trim().ToString() + "','" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "','" + this.txtfs1.Text.Trim().ToString() + "','" + this.txtfs2.Text.Trim().ToString() + "','" + this.txtfs3.Text.Trim().ToString() + "','" + this.txtfs4.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }

                }
                catch
                {

                }
                timer1.Start();
                conn.Close();
            }
            else if (dpjixing.SelectedItem.ToString() == "影院光源V36")
            {
                Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived5);
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                try
                {
                    string strSQL = "insert into dbo.messageCP2220(ljigwendu1,ljigwendu2,ljigwendu3,ljigwendu4,ljigwendu5,ljigwendu6,ljigwendu7,ljigwendu8,ljigwendu9,ljigwendu10,ljigwendu11,ljigwendu12,ljigwendu13,ljigwendu14,ljigwendu15,ljigwendu16,ljigwendu17,ljigwendu18,ljigwendu19,ljigwendu20,ljigwendu21,ljigwendu22,ljigwendu23,ljigwendu24,hongjigwendu1,hongjigwendu2,ljigdianliu1,ljigdianliu2,ljigdianliu3,ljigdianliu4,ljigdianliu5,ljigdianliu6,ljigdianliu7,ljigdianliu8,ljigdianliu9,ljigdianliu10,ljigdianliu11,ljigdianliu12,ljigdianliu13,ljigdianliu14,ljigdianliu15,ljigdianliu16,ljigdianliu17,ljigdianliu18,ljigdianliu19,ljigdianliu20,ljigdianliu21,ljigdianliu22,ljigdianliu23,ljigdianliu24,hongjigdianliu1,hongjigdianliu2,dianyuanwendu1,dianyuanwendu2,dianyuanwendu3,dianyuanwendu4,dianyuanwendu5,dianyuanwendu6,shuibengzhuansu,shuibengzhuansu2,shuibengzhuansu3,shuibengzhuansu4,shuibengzhuansu5,madawendu,madazhuansu,hongjigqiangtishidu,hongjigqiangtiwendu,lmwendu1,lmwendu2,rmwendu1,rmwendu2,huanwen,sn,date,dianyuanfengshan1,dianyuanfengshan2,dianyuanfengshan3,dianyuanfengshan4)values('"
                    + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "','" + this.txtjig3.Text.Trim().ToString() + "','" + this.txtjig4.Text.Trim().ToString() + "','" + this.txtjig5.Text.Trim().ToString() + "','" + this.txtjig6.Text.Trim().ToString() + "','" + this.txtjig7.Text.Trim().ToString() + "','" + this.txtjig8.Text.Trim().ToString() + "','" + this.txtjig9.Text.Trim().ToString() + "','" + this.txtjig10.Text.Trim().ToString() + "','" + this.txtjig11.Text.Trim().ToString() + "','" + this.txtjig12.Text.Trim().ToString() + "','" + this.txtjig13.Text.Trim().ToString() + "','" + this.txtjig14.Text.Trim().ToString() + "','" + this.txtjig15.Text.Trim().ToString() + "','" + this.txtjig16.Text.Trim().ToString() + "','" + this.txtjig17.Text.Trim().ToString() + "','" + this.txtjig18.Text.Trim().ToString() + "','" + this.txtjig19.Text.Trim().ToString() + "','" + this.txtjig20.Text.Trim().ToString() + "','" + this.txtjig21.Text.Trim().ToString() + "','" + this.txtjig22.Text.Trim().ToString() + "','" + this.txtjig23.Text.Trim().ToString() + "','" + this.txtjig24.Text.Trim().ToString() + "','"
                    + this.txthjig1.Text.Trim().ToString() + "','" + this.txthjig2.Text.Trim().ToString() + "','" + this.txtljgdianliu1.Text.Trim().ToString() + "','" + this.txtljgdianliu2.Text.Trim().ToString() + "','" + this.txtljgdianliu3.Text.Trim().ToString() + "','" + this.txtljgdianliu4.Text.Trim().ToString() + "','" + this.txtljgdianliu5.Text.Trim().ToString() + "','" + this.txtljgdianliu6.Text.Trim().ToString() + "','" + this.txtljgdianliu7.Text.Trim().ToString() + "','" + this.txtljgdianliu8.Text.Trim().ToString() + "','" + this.txtljgdianliu9.Text.Trim().ToString() + "','" + this.txtljgdianliu10.Text.Trim().ToString() + "','" + this.txtljgdianliu11.Text.Trim().ToString() + "','" + this.txtljgdianliu12.Text.Trim().ToString() + "','" + this.txtljgdianliu13.Text.Trim().ToString() + "','" + this.txtljgdianliu14.Text.Trim().ToString() + "','" + this.txtljgdianliu15.Text.Trim().ToString() + "','" + this.txtljgdianliu16.Text.Trim().ToString() + "','" + this.txtljgdianliu17.Text.Trim().ToString() + "','" + this.txtljgdianliu18.Text.Trim().ToString() + "','" + this.txtljgdianliu19.Text.Trim().ToString() + "','" + this.txtljgdianliu20.Text.Trim().ToString() + "','" + this.txtljgdianliu21.Text.Trim().ToString() + "','" + this.txtljgdianliu22.Text.Trim().ToString() + "','" + this.txtljgdianliu23.Text.Trim().ToString() + "','" + this.txtljgdianliu24.Text.Trim().ToString() + "','" + this.txthjgdianliu1.Text.Trim().ToString() + "','" + this.txthjgdianliu2.Text.Trim().ToString() + "','"
                    + this.txtdywendu1.Text.Trim().ToString() + "','" + this.txtdywendu2.Text.Trim().ToString() + "','" + this.txtdywendu3.Text.Trim().ToString() + "','" + this.txtdywendu4.Text.Trim().ToString() + "','" + this.txtdywendu5.Text.Trim().ToString() + "','" + this.txtdywendu6.Text.Trim().ToString() + "','" + this.txtsbzhuansu.Text.Trim().ToString() + "','" + this.txtslfs1.Text.Trim().ToString() + "','" + this.txtslfs2.Text.Trim().ToString() + "','"
                    + this.txtslfs3.Text.Trim().ToString() + "','" + this.txtslfs4.Text.Trim().ToString() + "','" + this.txtmdwendu.Text.Trim().ToString() + "','" + this.txtmdzhuansu.Text.Trim().ToString() + "','" + this.txtqtshidu.Text.Trim().ToString() + "','" + this.txtqtwendu.Text.Trim().ToString() + "','" + this.txtlmwendu1.Text.Trim().ToString() + "','" + this.txtlmwendu2.Text.Trim().ToString() + "','" + this.txtrmwendu1.Text.Trim().ToString() + "','" + this.txtrmwendu2.Text.Trim().ToString() + "','" + this.txthuanwen.Text.Trim().ToString() + "','" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "','" + this.txtfs1.Text.Trim().ToString() + "','" + this.txtfs2.Text.Trim().ToString() + "','" + this.txtfs3.Text.Trim().ToString() + "','" + this.txtfs4.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }

                }
                catch
                {

                }
                timer1.Start();
                conn.Close();
            }
            else if (dpjixing.SelectedItem.ToString() == "影院光源23B")
            {
                Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived6);
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                //try
                //{
                    string strSQL = "insert into dbo.messageCP2220(ljigwendu1,ljigwendu2,ljigwendu3,ljigwendu4,ljigdianliu1,ljigdianliu2,ljigdianliu3,ljigdianliu4,ljigdianliu5,ljigdianliu6,ljigdianliu7,ljigdianliu8,ljigdianliu9,ljigdianliu10,ljigdianliu11,ljigdianliu12,ljigdianliu13,ljigdianliu14,ljigdianliu15,ljigdianliu16,ljigdianliu17,ljigdianliu18,ljigdianliu19,ljigdianliu20,ljigdianliu21,ljigdianliu22,ljigdianliu23,ljigdianliu24,shuibengzhuansu,shuibengzhuansu2,shuibengzhuansu3,shuibengzhuansu4,shuibengzhuansu5,shuibengzhuansu6,madawendu,madazhuansu,hongjigqiangtishidu,hongjigqiangtiwendu,rmwendu1,rmwendu2,huanwen,sn,date,dianyuanfengshan1,dianyuanfengshan2)values('"
                    + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "','" + this.txtjig3.Text.Trim().ToString() + "','" + this.txtjig4.Text.Trim().ToString() + "','" + this.txtljgdianliu1.Text.Trim().ToString() + "','" + this.txtljgdianliu2.Text.Trim().ToString() + "','" + this.txtljgdianliu3.Text.Trim().ToString() + "','" + this.txtljgdianliu4.Text.Trim().ToString() + "','" + this.txtljgdianliu5.Text.Trim().ToString() + "','" + this.txtljgdianliu6.Text.Trim().ToString() + "','" + this.txtljgdianliu7.Text.Trim().ToString() + "','" + this.txtljgdianliu8.Text.Trim().ToString() + "','" + this.txtljgdianliu9.Text.Trim().ToString() + "','" + this.txtljgdianliu10.Text.Trim().ToString() + "','" + this.txtljgdianliu11.Text.Trim().ToString() + "','" + this.txtljgdianliu12.Text.Trim().ToString() + "','" + this.txtljgdianliu13.Text.Trim().ToString() + "','" + this.txtljgdianliu14.Text.Trim().ToString() + "','" + this.txtljgdianliu15.Text.Trim().ToString() + "','" + this.txtljgdianliu16.Text.Trim().ToString() + "','" + this.txtljgdianliu17.Text.Trim().ToString() + "','" + this.txtljgdianliu18.Text.Trim().ToString() + "','" + this.txtljgdianliu19.Text.Trim().ToString() + "','" + this.txtljgdianliu20.Text.Trim().ToString() + "','" + this.txtljgdianliu21.Text.Trim().ToString() + "','" + this.txtljgdianliu22.Text.Trim().ToString() + "','" + this.txtljgdianliu23.Text.Trim().ToString() + "','" + this.txtljgdianliu24.Text.Trim().ToString() + "','"
                    + this.txtsbzhuansu.Text.Trim().ToString() + "','" + this.txtslfs1.Text.Trim().ToString() + "','" + this.txtslfs2.Text.Trim().ToString() + "','" + this.txtslfs3.Text.Trim().ToString() + "','" + this.txtslfs4.Text.Trim().ToString() + "','" + this.txtslfs5.Text.Trim().ToString() + "','" + this.txtmdwendu.Text.Trim().ToString() + "','" + this.txtmdzhuansu.Text.Trim().ToString() + "','" + this.txtmdwendu2.Text.Trim().ToString() + "','" + this.txtmdzhuansu2.Text.Trim().ToString() + "','" + this.txtldcgq.Text.Trim().ToString() + "','" + this.txtldbfb.Text.Trim().ToString() + "','" + this.txthuanwen.Text.Trim().ToString() + "','" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "','" + this.txtfs1.Text.Trim().ToString() + "','" + this.txtfs2.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }

                //}
                //catch
                //{

                //}
                timer1.Start();
                conn.Close();
            }
            else if (dpjixing.SelectedItem.ToString() == "影院光源32B")
            {
                Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived7);
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                try
                {
                    string strSQL = "insert into dbo.messageCP2220(ljigwendu1,ljigwendu2,ljigwendu3,ljigwendu4,ljigwendu5,ljigwendu6,ljigwendu7,ljigwendu8,ljigwendu9,ljigwendu10,ljigwendu11,ljigwendu12,ljigwendu13,ljigwendu14,ljigwendu15,ljigwendu16,ljigwendu17,ljigwendu18,ljigwendu19,ljigwendu20,ljigwendu21,ljigwendu22,ljigwendu23,ljigwendu24,hongjigwendu1,hongjigwendu2,ljigdianliu1,ljigdianliu2,ljigdianliu3,ljigdianliu4,ljigdianliu5,ljigdianliu6,ljigdianliu7,ljigdianliu8,ljigdianliu9,ljigdianliu10,ljigdianliu11,ljigdianliu12,ljigdianliu13,ljigdianliu14,ljigdianliu15,ljigdianliu16,ljigdianliu17,ljigdianliu18,ljigdianliu19,ljigdianliu20,ljigdianliu21,ljigdianliu22,ljigdianliu23,ljigdianliu24,hongjigdianliu1,hongjigdianliu2,dianyuanwendu1,dianyuanwendu2,dianyuanwendu3,dianyuanwendu4,dianyuanwendu5,dianyuanwendu6,shuibengzhuansu,shuibengzhuansu2,shuibengzhuansu3,shuibengzhuansu4,shuibengzhuansu5,madawendu,madazhuansu,hongjigqiangtishidu,hongjigqiangtiwendu,lmwendu1,lmwendu2,rmwendu1,rmwendu2,huanwen,sn,date,dianyuanfengshan1,dianyuanfengshan2,dianyuanfengshan3,dianyuanfengshan4)values('"
                        + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "','" + this.txtjig3.Text.Trim().ToString() + "','" + this.txtjig4.Text.Trim().ToString() + "','" + this.txtjig5.Text.Trim().ToString() + "','" + this.txtjig6.Text.Trim().ToString() + "','" + this.txtjig7.Text.Trim().ToString() + "','" + this.txtjig8.Text.Trim().ToString() + "','" + this.txtjig9.Text.Trim().ToString() + "','" + this.txtjig10.Text.Trim().ToString() + "','" + this.txtjig11.Text.Trim().ToString() + "','" + this.txtjig12.Text.Trim().ToString() + "','" + this.txtjig13.Text.Trim().ToString() + "','" + this.txtjig14.Text.Trim().ToString() + "','" + this.txtjig15.Text.Trim().ToString() + "','" + this.txtjig16.Text.Trim().ToString() + "','" + this.txtjig17.Text.Trim().ToString() + "','" + this.txtjig18.Text.Trim().ToString() + "','" + this.txtjig19.Text.Trim().ToString() + "','" + this.txtjig20.Text.Trim().ToString() + "','" + this.txtjig21.Text.Trim().ToString() + "','" + this.txtjig22.Text.Trim().ToString() + "','" + this.txtjig23.Text.Trim().ToString() + "','" + this.txtjig24.Text.Trim().ToString() + "','"
                        + this.txthjig1.Text.Trim().ToString() + "','" + this.txthjig2.Text.Trim().ToString() + "','" + this.txtljgdianliu1.Text.Trim().ToString() + "','" + this.txtljgdianliu2.Text.Trim().ToString() + "','" + this.txtljgdianliu3.Text.Trim().ToString() + "','" + this.txtljgdianliu4.Text.Trim().ToString() + "','" + this.txtljgdianliu5.Text.Trim().ToString() + "','" + this.txtljgdianliu6.Text.Trim().ToString() + "','" + this.txtljgdianliu7.Text.Trim().ToString() + "','" + this.txtljgdianliu8.Text.Trim().ToString() + "','" + this.txtljgdianliu9.Text.Trim().ToString() + "','" + this.txtljgdianliu10.Text.Trim().ToString() + "','" + this.txtljgdianliu11.Text.Trim().ToString() + "','" + this.txtljgdianliu12.Text.Trim().ToString() + "','" + this.txtljgdianliu13.Text.Trim().ToString() + "','" + this.txtljgdianliu14.Text.Trim().ToString() + "','" + this.txtljgdianliu15.Text.Trim().ToString() + "','" + this.txtljgdianliu16.Text.Trim().ToString() + "','" + this.txtljgdianliu17.Text.Trim().ToString() + "','" + this.txtljgdianliu18.Text.Trim().ToString() + "','" + this.txtljgdianliu19.Text.Trim().ToString() + "','" + this.txtljgdianliu20.Text.Trim().ToString() + "','" + this.txtljgdianliu21.Text.Trim().ToString() + "','" + this.txtljgdianliu22.Text.Trim().ToString() + "','" + this.txtljgdianliu23.Text.Trim().ToString() + "','" + this.txtljgdianliu24.Text.Trim().ToString() + "','" + this.txthjgdianliu1.Text.Trim().ToString() + "','" + this.txthjgdianliu2.Text.Trim().ToString() + "','"
                        + this.txtdywendu1.Text.Trim().ToString() + "','" + this.txtdywendu2.Text.Trim().ToString() + "','" + this.txtdywendu3.Text.Trim().ToString() + "','" + this.txtdywendu4.Text.Trim().ToString() + "','" + this.txtdywendu5.Text.Trim().ToString() + "','" + this.txtdywendu6.Text.Trim().ToString() + "','" + this.txtsbzhuansu.Text.Trim().ToString() + "','" + this.txtslfs1.Text.Trim().ToString() + "','" + this.txtslfs2.Text.Trim().ToString() + "','"
                        + this.txtslfs3.Text.Trim().ToString() + "','" + this.txtslfs4.Text.Trim().ToString() + "','" + this.txtmdwendu.Text.Trim().ToString() + "','" + this.txtmdzhuansu.Text.Trim().ToString() + "','" + this.txtqtshidu.Text.Trim().ToString() + "','" + this.txtqtwendu.Text.Trim().ToString() + "','" + this.txtlmwendu1.Text.Trim().ToString() + "','" + this.txtlmwendu2.Text.Trim().ToString() + "','" + this.txtrmwendu1.Text.Trim().ToString() + "','" + this.txtrmwendu2.Text.Trim().ToString() + "','" + this.txthuanwen.Text.Trim().ToString() + "','" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "','" + this.txtfs1.Text.Trim().ToString() + "','" + this.txtfs2.Text.Trim().ToString() + "','" + this.txtfs3.Text.Trim().ToString() + "','" + this.txtfs4.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }

                }
                catch
                {

                }
                timer1.Start();
                conn.Close();
            }
            else if (dpjixing.SelectedItem.ToString() == "影院光源E80")
            {
                Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived8);
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                try
                {
                    string strSQL = "insert into dbo.messageCP2220(ljigwendu1,ljigwendu2,ljigwendu3,ljigwendu4,ljigwendu5,ljigwendu6,ljigwendu7,ljigwendu8,ljigdianliu1,ljigdianliu2,ljigdianliu3,ljigdianliu4,ljigdianliu5,ljigdianliu6,ljigdianliu7,ljigdianliu8,huanwen,sn,date)values('"
                    + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "','" + this.txtjig3.Text.Trim().ToString() + "','" + this.txtjig4.Text.Trim().ToString() + "','" + this.txtjig5.Text.Trim().ToString() + "','" + this.txtjig6.Text.Trim().ToString() + "','" + this.txtjig7.Text.Trim().ToString() + "','" + this.txtjig8.Text.Trim().ToString() + "','" + this.txtljgdianliu1.Text.Trim().ToString() + "','" + this.txtljgdianliu2.Text.Trim().ToString() + "','" + this.txtljgdianliu3.Text.Trim().ToString() + "','" + this.txtljgdianliu4.Text.Trim().ToString() + "','" + this.txtljgdianliu5.Text.Trim().ToString() + "','" + this.txtljgdianliu6.Text.Trim().ToString() + "','" + this.txtljgdianliu7.Text.Trim().ToString() + "','" + this.txtljgdianliu8.Text.Trim().ToString() + "','" + this.txthuanwen.Text.Trim().ToString() + "','" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }

                }
                catch
                {

                }
                timer1.Start();
                conn.Close();
            }
            else if (dpjixing.SelectedItem.ToString() == "影院光源CP2215")
            {
                Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived9);
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                try
                {
                    string strSQL = "insert into dbo.messageCP2220(ljigwendu1,ljigwendu2,ljigwendu3,ljigwendu4,ljigwendu5,ljigwendu6,ljigwendu7,ljigwendu8,ljigwendu9,ljigwendu10,ljigwendu11,ljigwendu12,hongjigwendu1,ljigdianliu1,ljigdianliu2,ljigdianliu3,ljigdianliu4,ljigdianliu5,ljigdianliu6,ljigdianliu7,ljigdianliu8,ljigdianliu9,ljigdianliu10,ljigdianliu11,ljigdianliu12,hongjigdianliu1,dianyuanwendu1,dianyuanwendu2,dianyuanwendu3,shuibengzhuansu,shuibengzhuansu2,shuibengzhuansu3,shuibengzhuansu4,madawendu,madazhuansu,hongjigqiangtishidu,hongjigqiangtiwendu,lmwendu1,rmwendu1,huanwen,sn,date,dianyuanfengshan1,dianyuanfengshan2)values('"
                    + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "','" + this.txtjig3.Text.Trim().ToString() + "','" + this.txtjig4.Text.Trim().ToString() + "','" + this.txtjig5.Text.Trim().ToString() + "','" + this.txtjig6.Text.Trim().ToString() + "','" + this.txtjig7.Text.Trim().ToString() + "','" + this.txtjig8.Text.Trim().ToString() + "','" + this.txtjig9.Text.Trim().ToString() + "','" + this.txtjig10.Text.Trim().ToString() + "','" + this.txtjig11.Text.Trim().ToString() + "','" + this.txtjig12.Text.Trim().ToString() + "','"
                    + this.txthjig1.Text.Trim().ToString() + "','" + this.txtljgdianliu1.Text.Trim().ToString() + "','" + this.txtljgdianliu2.Text.Trim().ToString() + "','" + this.txtljgdianliu3.Text.Trim().ToString() + "','" + this.txtljgdianliu4.Text.Trim().ToString() + "','" + this.txtljgdianliu5.Text.Trim().ToString() + "','" + this.txtljgdianliu6.Text.Trim().ToString() + "','" + this.txtljgdianliu7.Text.Trim().ToString() + "','" + this.txtljgdianliu8.Text.Trim().ToString() + "','" + this.txtljgdianliu9.Text.Trim().ToString() + "','" + this.txtljgdianliu10.Text.Trim().ToString() + "','" + this.txtljgdianliu11.Text.Trim().ToString() + "','" + this.txtljgdianliu12.Text.Trim().ToString() + "','" + this.txthjgdianliu1.Text.Trim().ToString() + "','"
                    + this.txtdywendu1.Text.Trim().ToString() + "','" + this.txtdywendu2.Text.Trim().ToString() + "','" + this.txtdywendu3.Text.Trim().ToString() + "','" + this.txtsbzhuansu.Text.Trim().ToString() + "','" + this.txtslfs1.Text.Trim().ToString() + "','" + this.txtslfs2.Text.Trim().ToString() + "','"
                    + this.txtslfs3.Text.Trim().ToString() + "','" + this.txtmdwendu.Text.Trim().ToString() + "','" + this.txtmdzhuansu.Text.Trim().ToString() + "','" + this.txtqtshidu.Text.Trim().ToString() + "','" + this.txtqtwendu.Text.Trim().ToString() + "','" + this.txtlmwendu1.Text.Trim().ToString() + "','" + this.txtrmwendu1.Text.Trim().ToString() + "','" + this.txthuanwen.Text.Trim().ToString() + "','" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "','" + this.txtfs1.Text.Trim().ToString() + "','" + this.txtfs2.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }

                }
                catch
                {

                }
                timer1.Start();
                conn.Close();
            }
            else if (dpjixing.SelectedItem.ToString() == "舞台灯")
            {
                Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived10);
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                try
                {
                    string strSQL = "insert into dbo.messageCP2220(ljigwendu1,madazhuansu)values('"
                    + this.txtjig1.Text.Trim().ToString() + this.txtmdzhuansu.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }

                }
                catch
                {

                }
                timer1.Start();
                conn.Close();
            }
            else if (dpjixing.SelectedItem.ToString() == "MCP激光")
            {
                Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived11);
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                try
                {
                    string strSQL = "insert into dbo.messageCP2220(ljigwendu1,ljigwendu2,ljigwendu3,ljigwendu4,ljigwendu5,ljigdianliu1,ljigdianliu2,ljigdianliu3,ljigdianliu4,ljigdianliu5,huanwen,sn,date)values('"
                    + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "','" + this.txtjig3.Text.Trim().ToString() + "','" + this.txtjig4.Text.Trim().ToString() + "','" + this.txtjig5.Text.Trim().ToString() + "','"
                    + this.txtljgdianliu1.Text.Trim().ToString() + "','" + this.txtljgdianliu2.Text.Trim().ToString() + "','" + this.txtljgdianliu3.Text.Trim().ToString() + "','" + this.txtljgdianliu4.Text.Trim().ToString() + "','" + this.txtljgdianliu5.Text.Trim().ToString() + "','"
                    + this.txthuanwen.Text.Trim().ToString() + "','" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }

                }
                catch
                {

                }
                timer1.Start();
                conn.Close();
            }
            else if (dpjixing.SelectedItem.ToString() == "ALPD4.0")
            {
                Sp.DataReceived += new SerialDataReceivedEventHandler(Sp_DataReceived12);
                lblshijian.Text = DateTime.Now.ToString("yyyy - MM - dd HH: mm:ss");
                try
                {
                    string strSQL = "insert into dbo.messageCP2220(ljigwendu1,ljigwendu2,ljigwendu3,ljigwendu4,ljigdianliu1,ljigdianliu2,ljigdianliu3,ljigdianliu4,huanwen,sn,date)values('"
                    + this.txtjig1.Text.Trim().ToString() + "','" + this.txtjig2.Text.Trim().ToString() + "','" + this.txtjig3.Text.Trim().ToString() + "','" + this.txtjig4.Text.Trim().ToString() + "','"
                    + this.txtljgdianliu1.Text.Trim().ToString() + "','" + this.txtljgdianliu2.Text.Trim().ToString() + "','" + this.txtljgdianliu3.Text.Trim().ToString() + "','" + this.txtljgdianliu4.Text.Trim().ToString() + "','"
                    + this.txthuanwen.Text.Trim().ToString() + "','" + this.txtSn.Text.Trim().ToString() + "','" + this.lblshijian.Text.Trim().ToString() + "')";
                    if (txthuanwen.Text != "")
                    {
                        SqlCommand mycom = new SqlCommand(strSQL, conn);
                        mycom.ExecuteNonQuery();
                    }
                    if (txtTime.Text == "")
                    {
                        timer1.Interval = 30000;
                    }
                    else if (txtTime.Text != "")
                    {
                        timer1.Interval = Convert.ToInt32(txtTime.Text.ToString());
                    }

                }
                catch
                {

                }
                timer1.Start();
                conn.Close();
            }
        }

        //教育机E2
        public void Sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(DoUpdate));
            //  InitChart1();
        }
        // 商教机S1
        public void Sp_DataReceived1(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(DoUpdate1));
            //  InitChart1();
        }
        //影院光源X10
        public void Sp_DataReceived2(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(DoUpdate2));
            //  InitChart1();
        }
        //影院光源X20
        public void Sp_DataReceived3(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(DoUpdate3));
            //  InitChart1();
        }
        //影院光源X60
        public void Sp_DataReceived4(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(DoUpdate4));
            //  InitChart1();
        }
        //影院光源V36
        public void Sp_DataReceived5(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(DoUpdate5));
            //  InitChart1();
        }
        //影院光源23B
        public void Sp_DataReceived6(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(DoUpdate6));
            //  InitChart1();
        }
        //影院光源32B
        public void Sp_DataReceived7(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(DoUpdate7));
            //  InitChart1();
        }
        //影院光源E80
        public void Sp_DataReceived8(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(DoUpdate8));
            //  InitChart1();
        }
        //影院光源CP2215
        public void Sp_DataReceived9(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(DoUpdate9));
            //  InitChart1();
        }
        //舞台灯
        public void Sp_DataReceived10(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(DoUpdate10));
            //  InitChart1();
        }
        //MCP激光
        public void Sp_DataReceived11(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(DoUpdate11));
            //  InitChart1();
        }
        //ALPD4.0
        public void Sp_DataReceived12(object sender, SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(DoUpdate12));
            //  InitChart1();
        }
        //影院光源X10
        private void DoUpdate2(object s, EventArgs e)
        {
            String readBuffer = Sp.ReadExisting();
            String chl = "Local Temp";
            if (readBuffer.IndexOf(chl) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl) + 12, 4);
                    this.txthuanwen.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl6 = "Wheel Motor";
            if (readBuffer.IndexOf(chl6) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl6) + 30, 4);
                    this.txtmdzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl6) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl6) + 87, 2);
                    this.txtmdwendu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            String chl1 = "Laser Temperature";
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 33, 2);

                    this.txtjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 40, 2);
                    this.txtjig2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 47, 2);
                    this.txtjig3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 54, 2);
                    this.txtjig4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 61, 2);
                    this.txtjig5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 68, 2);
                    this.txtjig6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 75, 2);
                    this.txtjig7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 82, 2);
                    this.txtjig8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 94, 2);
                    this.txthjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl2 = "Laser Current";
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 29, 4);
                    this.txtljgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 36, 4);
                    this.txtljgdianliu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 43, 4);
                    this.txtljgdianliu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 50, 4);
                    this.txtljgdianliu4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 57, 4);
                    this.txtljgdianliu5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 64, 4);
                    this.txtljgdianliu6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 71, 4);
                    this.txtljgdianliu7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 78, 4);
                    this.txtljgdianliu8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 92, 4);
                    this.txthjgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl3 = "Cooling Info";
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 36, 4);
                    this.txtsbzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 75, 4);
                    this.txtslfs1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 84, 4);
                    this.txtslfs2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 93, 4);
                    this.txtslfs3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 102, 4);
                    this.txtslfs4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 111, 4);
                    this.txtslfs5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 120, 4);
                    this.txtslfs6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl4 = "Power Info";
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 31, 4);
                    this.txtfs1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 40, 4);
                    this.txtfs2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 206, 4);
                    this.txtfs3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 215, 4);
                    this.txtfs4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 55, 4);
                    this.txtfs5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 61, 4);
                    this.txtfs6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 67, 4);
                    this.txtfs7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 73, 4);
                    this.txtfs8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 108, 2);
                    this.txtdywendu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 116, 2);
                    this.txtdywendu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 124, 2);
                    this.txtdywendu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl5 = "Laser Humidity";
            if (readBuffer.IndexOf(chl5) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl5) + 43, 4);
                    this.txtqtshidu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl5) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl5) + 53, 4);
                    this.txtqtwendu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl7 = "TEC Param";
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 73, 4);
                    this.txtlmwendu1.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 135, 4);
                    this.txtlmwendu2.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 89, 4);
                    this.txtrmwendu1.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 151, 4);
                    this.txtrmwendu2.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            string chl8 = "Brightness Info";
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 30, 3);
                    this.txtldcgq.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 42, 1);
                    this.txtldbfb.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
        }
        //影院光源X20
        private void DoUpdate3(object s, EventArgs e)
        {
            String readBuffer = Sp.ReadExisting();

            String Environment = "Environment";
            String current = "Yellow current  1-5";
            if (readBuffer.IndexOf(Environment) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(Environment) + 13, 2);
                    this.txthuanwen.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 22, 4);
                    this.txtljgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 31, 4);
                    this.txtljgdianliu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 40, 4);
                    this.txtljgdianliu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 49, 4);
                    this.txtljgdianliu4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 58, 4);
                    this.txtljgdianliu5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 87, 4);
                    this.txtljgdianliu6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 96, 4);
                    this.txtljgdianliu7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 105, 4);
                    this.txtljgdianliu8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 114, 4);
                    this.txtljgdianliu9.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 123, 4);
                    this.txtljgdianliu10.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 152, 4);
                    this.txtljgdianliu11.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 161, 4);
                    this.txtljgdianliu12.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 170, 4);
                    this.txtljgdianliu13.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 179, 4);
                    this.txtljgdianliu14.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 188, 4);
                    this.txtljgdianliu15.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 217, 4);
                    this.txtljgdianliu16.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 226, 4);
                    this.txtljgdianliu17.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 235, 4);
                    this.txtljgdianliu18.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 244, 4);
                    this.txtljgdianliu19.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 253, 4);
                    this.txtljgdianliu20.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 282, 4);
                    this.txtljgdianliu21.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 291, 4);
                    this.txtljgdianliu22.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 300, 4);
                    this.txtljgdianliu23.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 309, 4);
                    this.txtljgdianliu24.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string MotorFan = "Motor Fan";
            if (readBuffer.IndexOf(MotorFan) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(MotorFan) + 13, 4);
                    this.txtfs1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(MotorFan) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(MotorFan) + 22, 4);
                    this.txtfs2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl8 = "Brightness";
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 13, 4);
                    this.txtldcgq.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 30, 2);
                    this.txtldbfb.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }

            String Motor = "Motor 12V";
            String Motor1 = "Motor 36V";
            String MotorTEMP = "Motor TEMP";
            if (readBuffer.IndexOf(Motor) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(Motor) + 14, 4);
                    this.txtmdzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            if (readBuffer.IndexOf(Motor1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(Motor1) + 13, 5);
                    this.txtmdzhuansu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(MotorTEMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(MotorTEMP) + 13, 2);
                    this.txtmdwendu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(MotorTEMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(MotorTEMP) + 20, 2);
                    this.txtmdwendu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            String LaserTEMP = "Laser TEMP";
            if (readBuffer.IndexOf(LaserTEMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LaserTEMP) + 13, 2);
                    this.txtjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(LaserTEMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LaserTEMP) + 20, 2);
                    this.txtjig2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(LaserTEMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LaserTEMP) + 27, 2);
                    this.txtjig3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(LaserTEMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LaserTEMP) + 34, 2);
                    this.txtjig4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string CoolPUMP = "Cool  PUMP";
            if (readBuffer.IndexOf(CoolPUMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(CoolPUMP) + 13, 4);
                    this.txtsbzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(CoolPUMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(CoolPUMP) + 34, 4);
                    this.txtslfs1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(CoolPUMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(CoolPUMP) + 43, 4);
                    this.txtslfs2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(CoolPUMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(CoolPUMP) + 52, 4);
                    this.txtslfs3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(CoolPUMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(CoolPUMP) + 61, 4);
                    this.txtslfs4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
        }
        //影院光源X60
        private void DoUpdate4(object s, EventArgs e)
        {
            String readBuffer = Sp.ReadExisting();
            String chl = "Local Temp";
            String chl10 = "Yellow Curr";
            if (readBuffer.IndexOf(chl) != -1 & readBuffer.IndexOf(chl10) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl) + 12, 2);
                    this.txthuanwen.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            String chl9 = "Slave Sys";
            if (readBuffer.IndexOf(chl9) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl9) + 63, 2);
                    this.txthuanwen2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            string chl6 = "Wheel Motor";
            if (readBuffer.IndexOf(chl6) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl6) + 30, 4);
                    this.txtmdzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl6) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl6) + 87, 2);
                    this.txtmdwendu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl6) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl6) + 102, 4);
                    this.txtmdzhuansu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl6) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl6) + 159, 2);
                    this.txtmdwendu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl8 = "Brightness Info";
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 63, 4);
                    this.txtldcgq.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 100, 3);
                    this.txtldbfb.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 191, 4);
                    this.txtldcgq2.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 228, 2);
                    this.txtldbfb2.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            String chl1 = "Laser Temperature";
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 33, 2);

                    this.txtjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 40, 2);
                    this.txtjig2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 47, 2);
                    this.txtjig3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 54, 2);
                    this.txtjig4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 61, 2);
                    this.txtjig5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 68, 2);
                    this.txtjig6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 75, 2);
                    this.txtjig7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 82, 2);
                    this.txtjig8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 89, 2);
                    this.txtjig9.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 96, 2);
                    this.txtjig10.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 103, 2);
                    this.txtjig11.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 110, 2);
                    this.txtjig12.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 119, 2);
                    this.txtjig13.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 126, 2);
                    this.txtjig14.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 133, 2);
                    this.txtjig15.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 140, 2);
                    this.txtjig16.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 147, 2);
                    this.txtjig17.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 154, 2);
                    this.txtjig18.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 161, 2);
                    this.txtjig19.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 168, 2);
                    this.txtjig20.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 175, 2);
                    this.txtjig21.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 182, 2);
                    this.txtjig22.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 189, 2);
                    this.txtjig23.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 196, 2);
                    this.txtjig24.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 208, 2);
                    this.txthjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 215, 2);
                    this.txthjig2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 224, 2);
                    this.txtjig25.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 231, 2);
                    this.txtjig26.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 238, 2);
                    this.txtjig27.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 245, 2);
                    this.txtjig28.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 252, 2);
                    this.txtjig29.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 259, 2);
                    this.txtjig30.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 266, 2);
                    this.txtjig31.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 273, 2);
                    this.txtjig32.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 280, 2);
                    this.txtjig33.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 287, 2);
                    this.txtjig34.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 294, 2);
                    this.txtjig35.Text = tmp;
                }
                catch
                {
                    return;
                }

            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 301, 2);
                    this.txtjig36.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 310, 2);
                    this.txtjig37.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 317, 2);
                    this.txtjig38.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 324, 2);
                    this.txtjig39.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 331, 2);
                    this.txtjig40.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 338, 2);
                    this.txtjig41.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 345, 2);
                    this.txtjig42.Text = tmp;

                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 352, 2);
                    this.txtjig43.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 359, 2);
                    this.txtjig44.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 366, 2);
                    this.txtjig45.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 373, 2);
                    this.txtjig46.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 380, 2);
                    this.txtjig47.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 387, 2);
                    this.txtjig48.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 399, 2);
                    this.txthjig3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 406, 2);
                    this.txthjig4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl2 = "Laser Current";
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 29, 4);
                    this.txtljgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 36, 4);
                    this.txtljgdianliu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 43, 4);
                    this.txtljgdianliu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 50, 4);
                    this.txtljgdianliu4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 57, 4);
                    this.txtljgdianliu5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 64, 4);
                    this.txtljgdianliu6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 71, 4);
                    this.txtljgdianliu7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 78, 4);
                    this.txtljgdianliu8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 85, 4);
                    this.txtljgdianliu9.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 92, 4);
                    this.txtljgdianliu10.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 99, 4);
                    this.txtljgdianliu11.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 106, 4);
                    this.txtljgdianliu12.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 115, 4);
                    this.txtljgdianliu13.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 122, 4);
                    this.txtljgdianliu14.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 129, 4);
                    this.txtljgdianliu15.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 136, 4);
                    this.txtljgdianliu16.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 143, 4);
                    this.txtljgdianliu17.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 150, 4);
                    this.txtljgdianliu18.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 157, 4);
                    this.txtljgdianliu19.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 164, 4);
                    this.txtljgdianliu20.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 171, 4);
                    this.txtljgdianliu21.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 178, 4);
                    this.txtljgdianliu22.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 185, 4);
                    this.txtljgdianliu23.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 192, 4);
                    this.txtljgdianliu24.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 204, 4);
                    this.txthjgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 211, 4);
                    this.txthjgdianliu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 220, 4);
                    this.txtljgdianliu25.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 227, 4);
                    this.txtljgdianliu26.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 234, 4);
                    this.txtljgdianliu27.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 241, 4);
                    this.txtljgdianliu28.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 248, 4);
                    this.txtljgdianliu29.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 255, 4);
                    this.txtljgdianliu30.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 262, 4);
                    this.txtljgdianliu31.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 269, 4);
                    this.txtljgdianliu32.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 276, 4);
                    this.txtljgdianliu33.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 283, 4);
                    this.txtljgdianliu34.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 290, 4);
                    this.txtljgdianliu35.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 297, 4);
                    this.txtljgdianliu36.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 306, 4);
                    this.txtljgdianliu37.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 313, 4);
                    this.txtljgdianliu38.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 320, 4);
                    this.txtljgdianliu39.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 327, 4);
                    this.txtljgdianliu40.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 334, 4);
                    this.txtljgdianliu41.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 341, 4);
                    this.txtljgdianliu42.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 348, 4);
                    this.txtljgdianliu43.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 355, 4);
                    this.txtljgdianliu44.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 362, 4);
                    this.txtljgdianliu45.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 369, 4);
                    this.txtljgdianliu46.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 376, 4);
                    this.txtljgdianliu47.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 383, 4);
                    this.txtljgdianliu48.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 395, 4);
                    this.txthjgdianliu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 402, 4);
                    this.txthjgdianliu4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl3 = "Cooling Info";
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 36, 4);
                    this.txtsbzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 45, 4);
                    this.txtsbzhuansu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 78, 4);
                    this.txtslfs1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 87, 4);
                    this.txtslfs2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 96, 4);
                    this.txtslfs3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 105, 4);
                    this.txtslfs4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 114, 4);
                    this.txtslfs5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 123, 4);
                    this.txtslfs6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl4 = "Power Info";
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 31, 4);
                    this.txtfs1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 40, 4);
                    this.txtfs2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 206, 4);
                    this.txtfs3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 215, 4);
                    this.txtfs4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 381, 4);
                    this.txtfs5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 390, 4);
                    this.txtfs6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 556, 4);
                    this.txtfs7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
                if (readBuffer.IndexOf(chl4) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 565, 4);
                        this.txtfs8.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl4) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 108, 2);
                        this.txtdywendu1.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl4) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 116, 2);
                        this.txtdywendu2.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl4) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 124, 2);
                        this.txtdywendu3.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl4) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 283, 2);
                        this.txtdywendu4.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl4) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 291, 2);
                        this.txtdywendu5.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl4) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 299, 2);
                        this.txtdywendu6.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl4) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 458, 2);
                        this.txtdywendu7.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl4) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 466, 2);
                        this.txtdywendu8.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl4) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 474, 2);
                        this.txtdywendu9.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl4) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 633, 2);
                        this.txtdywendu10.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl4) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 641, 2);
                        this.txtdywendu11.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl4) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 649, 2);
                        this.txtdywendu12.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                string chl5 = "Laser Humidity";
                if (readBuffer.IndexOf(chl5) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl5) + 43, 4);
                        this.txtqtshidu.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl5) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl5) + 53, 4);
                        this.txtqtwendu.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl5) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl5) + 75, 4);
                        this.txtqtshidu2.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl5) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl5) + 85, 4);
                        this.txtqtwendu2.Text = tmp;
                    }
                    catch
                    {
                        return;
                    }
                }

                string chl7 = "TEC Param";
                if (readBuffer.IndexOf(chl7) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 73, 4);
                        this.txtlmwendu1.Text = tmp.ToString();
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl7) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 135, 4);
                        this.txtlmwendu2.Text = tmp.ToString();
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl7) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 89, 4);
                        this.txtrmwendu1.Text = tmp.ToString();
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl7) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 151, 4);
                        this.txtrmwendu2.Text = tmp.ToString();
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl7) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 305, 4);
                        this.txt2lmwendu1.Text = tmp.ToString();
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl7) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 367, 4);
                        this.txt2lmwendu2.Text = tmp.ToString();
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl7) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 321, 4);
                        this.txt2rmwendu1.Text = tmp.ToString();
                    }
                    catch
                    {
                        return;
                    }
                }
                if (readBuffer.IndexOf(chl7) != -1)
                {
                    try
                    {
                        string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 383, 4);
                        this.txt2rmwendu2.Text = tmp.ToString();
                    }
                    catch
                    {
                        return;
                    }
                }
            
        }
        //影院光源V36
        private void DoUpdate5(object s, EventArgs e)
        {
            String readBuffer = Sp.ReadExisting();

            String chl = "Local Temp";
            if (readBuffer.IndexOf(chl) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl) + 12, 4);
                    this.txthuanwen.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            string chl6 = "Wheel Motor";
            if (readBuffer.IndexOf(chl6) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl6) + 30, 4);
                    this.txtmdzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl6) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl6) + 87, 2);
                    this.txtmdwendu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            String chl1 = "Laser Temperature";
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 28, 2);

                    this.txtjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 35, 2);
                    this.txtjig2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 42, 2);
                    this.txtjig3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 49, 2);
                    this.txtjig4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 56, 2);
                    this.txtjig5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 63, 2);
                    this.txtjig6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 70, 2);
                    this.txtjig7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 77, 2);
                    this.txtjig8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 84, 2);
                    this.txtjig9.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 91, 2);
                    this.txtjig10.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 98, 2);
                    this.txtjig11.Text = tmp;
                }
                catch
                {
                    return;
                }

            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 105, 2);
                    this.txtjig12.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 114, 2);
                    this.txtjig13.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 121, 2);
                    this.txtjig14.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 128, 2);
                    this.txtjig15.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 135, 2);
                    this.txtjig16.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 142, 2);
                    this.txtjig17.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 149, 2);
                    this.txtjig18.Text = tmp;

                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 156, 2);
                    this.txtjig19.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 163, 2);
                    this.txtjig20.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 170, 2);
                    this.txtjig21.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 177, 2);
                    this.txtjig22.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 184, 2);
                    this.txtjig23.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 191, 2);
                    this.txtjig24.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 200, 2);
                    this.txthjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 207, 2);
                    this.txthjig2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl2 = "Laser1 Current";
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 24, 4);
                    this.txtljgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 30, 4);
                    this.txtljgdianliu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 36, 4);
                    this.txtljgdianliu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 42, 4);
                    this.txtljgdianliu4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 48, 4);
                    this.txtljgdianliu5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 54, 4);
                    this.txtljgdianliu6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 60, 4);
                    this.txtljgdianliu7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 66, 4);
                    this.txtljgdianliu8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 72, 4);
                    this.txtljgdianliu9.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 78, 4);
                    this.txtljgdianliu10.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 84, 4);
                    this.txtljgdianliu11.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 90, 4);
                    this.txtljgdianliu12.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 136, 4);
                    this.txtljgdianliu13.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 142, 4);
                    this.txtljgdianliu14.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 148, 4);
                    this.txtljgdianliu15.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 154, 4);
                    this.txtljgdianliu16.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 160, 4);
                    this.txtljgdianliu17.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 166, 4);
                    this.txtljgdianliu18.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 172, 4);
                    this.txtljgdianliu19.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 178, 4);
                    this.txtljgdianliu20.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 184, 4);
                    this.txtljgdianliu21.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 190, 4);
                    this.txtljgdianliu22.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 196, 4);
                    this.txtljgdianliu23.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 202, 4);
                    this.txtljgdianliu24.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 96, 4);
                    this.txthjgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 208, 4);
                    this.txthjgdianliu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl3 = "Cooling Info";
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 40, 4);
                    this.txtsbzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 74, 4);
                    this.txtslfs1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 82, 4);
                    this.txtslfs2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 90, 4);
                    this.txtslfs3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 98, 4);
                    this.txtslfs4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 106, 4);
                    this.txtslfs5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl4 = "Power1 Info";
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 26, 4);
                    this.txtfs1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 34, 4);
                    this.txtfs2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 201, 4);
                    this.txtfs3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 209, 4);
                    this.txtfs4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 55, 4);
                    this.txtfs5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 61, 4);
                    this.txtfs6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 67, 4);
                    this.txtfs7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 73, 4);
                    this.txtfs8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 114, 2);
                    this.txtdywendu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 120, 2);
                    this.txtdywendu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 126, 2);
                    this.txtdywendu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }


            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 289, 2);
                    this.txtdywendu4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 295, 2);
                    this.txtdywendu5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 301, 2);
                    this.txtdywendu6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl5 = "Laser Humidity";
            if (readBuffer.IndexOf(chl5) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl5) + 54, 3);
                    this.txtqtshidu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl5) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl5) + 63, 5);
                    this.txtqtwendu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            string chl7 = "TEC Info";
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 47, 4);
                    this.txtlmwendu1.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 146, 4);
                    this.txtlmwendu2.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 59, 4);
                    this.txtrmwendu1.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 158, 4);
                    this.txtrmwendu2.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            string chl8 = "Brightness info";
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 65, 4);
                    this.txtldcgq.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 95, 1);
                    this.txtldbfb.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }

        }
        //影院光源23B
        private void DoUpdate6(object s, EventArgs e)
        {
            String readBuffer = Sp.ReadExisting();

            String Environment = "Environment";
            String current = "Yellow current  1-5";
            if (readBuffer.IndexOf(Environment) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(Environment) + 13, 2);
                    this.txthuanwen.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 22, 4);
                    this.txtljgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 31, 4);
                    this.txtljgdianliu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 40, 4);
                    this.txtljgdianliu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 49, 4);
                    this.txtljgdianliu4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 58, 4);
                    this.txtljgdianliu5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 87, 4);
                    this.txtljgdianliu6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 96, 4);
                    this.txtljgdianliu7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 105, 4);
                    this.txtljgdianliu8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 114, 4);
                    this.txtljgdianliu9.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 123, 4);
                    this.txtljgdianliu10.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 152, 4);
                    this.txtljgdianliu11.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 161, 4);
                    this.txtljgdianliu12.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 170, 4);
                    this.txtljgdianliu13.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 179, 4);
                    this.txtljgdianliu14.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 188, 4);
                    this.txtljgdianliu15.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 217, 4);
                    this.txtljgdianliu16.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 226, 4);
                    this.txtljgdianliu17.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 235, 4);
                    this.txtljgdianliu18.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 244, 4);
                    this.txtljgdianliu19.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 253, 4);
                    this.txtljgdianliu20.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 282, 4);
                    this.txtljgdianliu21.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 291, 4);
                    this.txtljgdianliu22.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 300, 4);
                    this.txtljgdianliu23.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(current) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(current) + 309, 4);
                    this.txtljgdianliu24.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string MotorFan = "Motor Fan";
            if (readBuffer.IndexOf(MotorFan) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(MotorFan) + 13, 4);
                    this.txtfs1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(MotorFan) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(MotorFan) + 22, 4);
                    this.txtfs2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl8 = "Brightness";
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 13, 4);
                    this.txtldcgq.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 30, 2);
                    string tmp1 = tmp.Replace("%", "");
                    this.txtldbfb.Text = tmp1.ToString();
                }
                catch
                {
                    return;
                }
            }

            String Motor = "Motor 12V";
            String Motor1 = "Motor 36V";
            String MotorTEMP = "Motor TEMP";
            if (readBuffer.IndexOf(Motor) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(Motor) + 14, 4);
                    this.txtmdzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            if (readBuffer.IndexOf(Motor1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(Motor1) + 13, 5);
                    this.txtmdzhuansu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(MotorTEMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(MotorTEMP) + 13, 2);
                    this.txtmdwendu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(MotorTEMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(MotorTEMP) + 20, 2);
                    this.txtmdwendu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            String LaserTEMP = "Laser TEMP";
            if (readBuffer.IndexOf(LaserTEMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LaserTEMP) + 13, 2);
                    this.txtjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(LaserTEMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LaserTEMP) + 20, 2);
                    this.txtjig2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(LaserTEMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LaserTEMP) + 27, 2);
                    this.txtjig3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(LaserTEMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LaserTEMP) + 34, 2);
                    this.txtjig4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string CoolPUMP = "Cool  PUMP";
            if (readBuffer.IndexOf(CoolPUMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(CoolPUMP) + 13, 4);
                    this.txtsbzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(CoolPUMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(CoolPUMP) + 34, 4);
                    this.txtslfs1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(CoolPUMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(CoolPUMP) + 43, 4);
                    this.txtslfs2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(CoolPUMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(CoolPUMP) + 52, 4);
                    this.txtslfs3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(CoolPUMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(CoolPUMP) + 61, 4);
                    this.txtslfs4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(CoolPUMP) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(CoolPUMP) + 70, 4);
                    this.txtslfs5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
        }
        //影院光源32B
        private void DoUpdate7(object s, EventArgs e)
        {
            String readBuffer = Sp.ReadExisting();
            String chl = "Local Temp";
            if (readBuffer.IndexOf(chl) != -1)
            {
                string tmp = readBuffer.Substring(readBuffer.IndexOf(chl) + 12, 4);
                this.txthuanwen.Text = tmp;
            }

            string chl6 = "Wheel Motor";
            if (readBuffer.IndexOf(chl6) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl6) + 30, 4);
                    this.txtmdzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl6) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl6) + 87, 2);
                    this.txtmdwendu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl8 = "Brightness Info";
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 28, 4);
                    this.txtldcgq.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 42, 2);
                    this.txtldbfb.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            String chl1 = "Laser Temperature";
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 33, 2);

                    this.txtjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 40, 2);
                    this.txtjig2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 47, 2);
                    this.txtjig3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 54, 2);
                    this.txtjig4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 61, 2);
                    this.txtjig5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 68, 2);
                    this.txtjig6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 75, 2);
                    this.txtjig7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 82, 2);
                    this.txtjig8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 89, 2);
                    this.txtjig9.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 96, 2);
                    this.txtjig10.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 103, 2);
                    this.txtjig11.Text = tmp;
                }
                catch
                {
                    return;
                }

            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 110, 2);
                    this.txtjig12.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 119, 2);
                    this.txtjig13.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 126, 2);
                    this.txtjig14.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 133, 2);
                    this.txtjig15.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 140, 2);
                    this.txtjig16.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 147, 2);
                    this.txtjig17.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 154, 2);
                    this.txtjig18.Text = tmp;

                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 161, 2);
                    this.txtjig19.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 168, 2);
                    this.txtjig20.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 175, 2);
                    this.txtjig21.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 182, 2);
                    this.txtjig22.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 189, 2);
                    this.txtjig23.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 196, 2);
                    this.txtjig24.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 208, 2);
                    this.txthjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 215, 2);
                    this.txthjig2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl2 = "Laser Current";
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 29, 4);
                    this.txtljgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 36, 4);
                    this.txtljgdianliu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 43, 4);
                    this.txtljgdianliu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 50, 4);
                    this.txtljgdianliu4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 57, 4);
                    this.txtljgdianliu5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 64, 4);
                    this.txtljgdianliu6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 71, 4);
                    this.txtljgdianliu7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 78, 4);
                    this.txtljgdianliu8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 85, 4);
                    this.txtljgdianliu9.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 92, 4);
                    this.txtljgdianliu10.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 99, 4);
                    this.txtljgdianliu11.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 106, 4);
                    this.txtljgdianliu12.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 115, 4);
                    this.txtljgdianliu13.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 122, 4);
                    this.txtljgdianliu14.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 129, 4);
                    this.txtljgdianliu15.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 136, 4);
                    this.txtljgdianliu16.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 143, 4);
                    this.txtljgdianliu17.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 150, 4);
                    this.txtljgdianliu18.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 157, 4);
                    this.txtljgdianliu19.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 164, 4);
                    this.txtljgdianliu20.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 171, 4);
                    this.txtljgdianliu21.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 178, 4);
                    this.txtljgdianliu22.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 185, 4);
                    this.txtljgdianliu23.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 192, 4);
                    this.txtljgdianliu24.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 204, 4);
                    this.txthjgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 211, 4);
                    this.txthjgdianliu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl3 = "Cooling Info";
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 36, 4);
                    this.txtsbzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 75, 4);
                    this.txtslfs1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 84, 4);
                    this.txtslfs2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 93, 4);
                    this.txtslfs3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 102, 4);
                    this.txtslfs4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 111, 4);
                    this.txtslfs5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl4 = "Power Info";
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 31, 4);
                    this.txtfs1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 40, 4);
                    this.txtfs2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 206, 4);
                    this.txtfs3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 215, 4);
                    this.txtfs4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 55, 4);
                    this.txtfs5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 61, 4);
                    this.txtfs6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 67, 4);
                    this.txtfs7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 73, 4);
                    this.txtfs8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 108, 2);
                    this.txtdywendu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 116, 2);
                    this.txtdywendu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 124, 2);
                    this.txtdywendu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }


            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 283, 2);
                    this.txtdywendu4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 291, 2);
                    this.txtdywendu5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 299, 2);
                    this.txtdywendu6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl5 = "Laser Humidity";
            if (readBuffer.IndexOf(chl5) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl5) + 43, 4);
                    this.txtqtshidu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl5) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl5) + 53, 4);
                    this.txtqtwendu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            string chl7 = "TEC Param";
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 73, 4);
                    this.txtlmwendu1.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 135, 4);
                    this.txtlmwendu2.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 89, 4);
                    this.txtrmwendu1.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 151, 4);
                    this.txtrmwendu2.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }

        }
        //影院光源E80
        private void DoUpdate8(object s, EventArgs e)
        {
            String readBuffer = Sp.ReadExisting();

            String env = "env";
            String cw = "cw";
            String LD0 = "LD0";
            if (readBuffer.IndexOf(env) != -1 && readBuffer.IndexOf(cw) != -1)
            {
                string tmp = readBuffer.Substring(readBuffer.IndexOf(env) + 4, 2);
                this.txthuanwen.Text = tmp;

            }
            if (readBuffer.IndexOf(cw) != -1 && readBuffer.IndexOf(LD0) != -1)
            {
                string tmp = readBuffer.Substring(readBuffer.IndexOf(cw) + 3, 2);
                this.txtselun.Text = tmp;
            }
            String cf = "cf";
            String dmd = "dmd";

            if (readBuffer.IndexOf(cf) != -1 && readBuffer.IndexOf(dmd) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(cf) + 11, 5);
                    this.txtljgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(cf) != -1 && readBuffer.IndexOf(dmd) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(cf) + 27, 5);
                    this.txtljgdianliu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(cf) != -1 && readBuffer.IndexOf(dmd) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(cf) + 43, 5);
                    this.txtljgdianliu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(cf) != -1 && readBuffer.IndexOf(dmd) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(cf) + 59, 5);
                    this.txtljgdianliu4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(cf) != -1 && readBuffer.IndexOf(dmd) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(cf) + 75, 5);
                    this.txtljgdianliu5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(cf) != -1 && readBuffer.IndexOf(dmd) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(cf) + 91, 5);
                    this.txtljgdianliu6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(cf) != -1 && readBuffer.IndexOf(dmd) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(cf) + 107, 5);
                    this.txtljgdianliu7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(cf) != -1 && readBuffer.IndexOf(dmd) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(cf) + 123, 5);
                    this.txtljgdianliu8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            String LD1 = "LD1";
            String LD2 = "LD2";
            String LD3 = "LD3";
            String LD4 = "LD4";
            String LD5 = "LD5";
            String LD6 = "LD6";
            String LD7 = "LD7";
            String LD8 = "LD8";
            if (readBuffer.IndexOf(LD0) != -1 && readBuffer.IndexOf(LD1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LD0) + 4, 2);
                    this.txtjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            if (readBuffer.IndexOf(LD0) != -1 && readBuffer.IndexOf(LD2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LD0) + 11, 2);
                    this.txtjig2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(LD2) != -1 && readBuffer.IndexOf(LD3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LD2) + 4, 2);
                    this.txtjig3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(LD3) != -1 && readBuffer.IndexOf(LD4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LD3) + 4, 2);
                    this.txtjig4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(LD4) != -1 && readBuffer.IndexOf(LD5) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LD4) + 4, 2);
                    this.txtjig5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(LD5) != -1 && readBuffer.IndexOf(LD6) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LD5) + 4, 2);
                    this.txtjig6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(LD6) != -1 && readBuffer.IndexOf(LD7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LD6) + 4, 2);
                    this.txtjig7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(LD7) != -1 && readBuffer.IndexOf(LD8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(LD7) + 4, 2);
                    this.txtjig8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl4 = "Power Info";
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 67, 4);
                    this.txtfs7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
        }
        //影院光源CP2215
        private void DoUpdate9(object s, EventArgs e)
        {
            String readBuffer = Sp.ReadExisting();

            String chl = "Local Temp";
            if (readBuffer.IndexOf(chl) != -1)
            {
                string tmp = readBuffer.Substring(readBuffer.IndexOf(chl) + 12, 4);
                this.txthuanwen.Text = tmp;
            }

            string chl6 = "Wheel Motor";
            if (readBuffer.IndexOf(chl6) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl6) + 30, 4);
                    this.txtmdzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl6) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl6) + 87, 2);
                    this.txtmdwendu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            String chl1 = "Laser Temperature";
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 33, 2);

                    this.txtjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 40, 2);
                    this.txtjig2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 47, 2);
                    this.txtjig3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 54, 2);
                    this.txtjig4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 61, 2);
                    this.txtjig5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 68, 2);
                    this.txtjig6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 75, 2);
                    this.txtjig7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 82, 2);
                    this.txtjig8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 89, 2);
                    this.txtjig9.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 96, 2);
                    this.txtjig10.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 103, 2);
                    this.txtjig11.Text = tmp;
                }
                catch
                {
                    return;
                }

            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 110, 2);
                    this.txtjig12.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 124, 2);
                    this.txthjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl2 = "Laser Current";
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 28, 4);
                    this.txtljgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 34, 4);
                    this.txtljgdianliu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 40, 4);
                    this.txtljgdianliu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 46, 4);
                    this.txtljgdianliu4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 52, 4);
                    this.txtljgdianliu5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 58, 4);
                    this.txtljgdianliu6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 64, 4);
                    this.txtljgdianliu7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 70, 4);
                    this.txtljgdianliu8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 76, 4);
                    this.txtljgdianliu9.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 82, 4);
                    this.txtljgdianliu10.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 88, 4);
                    this.txtljgdianliu11.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 94, 4);
                    this.txtljgdianliu12.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 113, 4);
                    this.txthjgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl3 = "Cooling Info";
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 36, 4);
                    this.txtsbzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 75, 4);
                    this.txtslfs1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 84, 4);
                    this.txtslfs2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl3) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 93, 4);
                    this.txtslfs3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl4 = "Power Info";
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 31, 4);
                    this.txtfs1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 40, 4);
                    this.txtfs2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 206, 4);
                    this.txtfs3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 215, 4);
                    this.txtfs4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 55, 4);
                    this.txtfs5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 61, 4);
                    this.txtfs6.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 67, 4);
                    this.txtfs7.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 73, 4);
                    this.txtfs8.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 108, 2);
                    this.txtdywendu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 116, 2);
                    this.txtdywendu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl4) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 124, 2);
                    this.txtdywendu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl5 = "Laser Humidity";
            if (readBuffer.IndexOf(chl5) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl5) + 43, 4);
                    this.txtqtshidu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl5) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl5) + 53, 4);
                    this.txtqtwendu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            string chl7 = "TEC Param";
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 73, 4);
                    this.txtlmwendu1.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 135, 4);
                    this.txtlmwendu2.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 89, 4);
                    this.txtrmwendu1.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl7) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl7) + 151, 4);
                    this.txtrmwendu2.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            string chl8 = "Brightness Info";
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 28, 3);
                    this.txtldcgq.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl8) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl8) + 41, 2);
                    this.txtldbfb.Text = tmp.ToString();
                }
                catch
                {
                    return;
                }
            }

        }
        //舞台灯
        private void DoUpdate10(object s, EventArgs e)
        {
            String readBuffer = Sp.ReadExisting();
            string chl = "motor:12V";
            if (readBuffer.IndexOf(chl) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl) + 11, 4);
                    this.txtmdzhuansu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl1 = "Laser TEMP";
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 12, 2);
                    this.txtmdwendu.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
        }
        //MCP激光
        private void DoUpdate11(object s, EventArgs e)
        {
            String readBuffer = Sp.ReadExisting();

            String chl = "Local Temp";
            if (readBuffer.IndexOf(chl) != -1)
            {
                string tmp = readBuffer.Substring(readBuffer.IndexOf(chl) + 12, 4);
                this.txthuanwen.Text = tmp;
            }
            String chl1 = "Laser Temperature";
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 33, 2);

                    this.txtjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 40, 2);
                    this.txtjig2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 47, 2);
                    this.txtjig3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 54, 2);
                    this.txtjig4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 61, 2);
                    this.txtjig5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl2 = "Laser Current";
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 29, 4);
                    this.txtljgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 36, 4);
                    this.txtljgdianliu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 43, 4);
                    this.txtljgdianliu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 50, 4);
                    this.txtljgdianliu4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 57, 4);
                    this.txtljgdianliu5.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
        }
        //ALPD4.0
        private void DoUpdate12(object s, EventArgs e)
        {
            String readBuffer = Sp.ReadExisting();

            String chl = "Local Temp";
            if (readBuffer.IndexOf(chl) != -1)
            {
                string tmp = readBuffer.Substring(readBuffer.IndexOf(chl) + 12, 4);
                this.txthuanwen.Text = tmp;
            }
            String chl1 = "Laser Temperature";
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 33, 2);

                    this.txtjig1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }

            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 40, 2);
                    this.txtjig2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 47, 2);
                    this.txtjig3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 54, 2);
                    this.txtjig4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            string chl2 = "Laser Current";
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 29, 4);
                    this.txtljgdianliu1.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 36, 4);
                    this.txtljgdianliu2.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 43, 4);
                    this.txtljgdianliu3.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl2) != -1)
            {
                try
                {
                    string tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 50, 4);
                    this.txtljgdianliu4.Text = tmp;
                }
                catch
                {
                    return;
                }
            }
        }

        //教育机E2
        private void DoUpdate(object s, EventArgs e)
        {
            String tmp = null;
            String chl = "channel = 3";
            String chl1 = "channel = 4";
            String chl2 = "channel = 6";
            String t = "temperture";
            String m = "mec=000";
            String p = "perture";

            String readBuffer = Sp.ReadExisting();
            if (readBuffer.IndexOf(chl) != -1 && readBuffer.IndexOf(t) != -1 && readBuffer.IndexOf(m) != -1)
            {

                try
                {
                    tmp = readBuffer.Substring(readBuffer.IndexOf(chl) + 81, 16);
                    String tmp1 = tmp.Substring(tmp.IndexOf(p) + 10, 2);
                    this.txthuanwen.Text = tmp1;
                }
                catch
                {
                    return;
                }
            }
            if (readBuffer.IndexOf(chl1) != -1 && readBuffer.IndexOf(t) != -1 && readBuffer.IndexOf(m) != -1)
            {

                try
                {
                    tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 79, 16);
                    String tmp1 = tmp.Substring(tmp.IndexOf(p) + 10, 2);
                    this.txtjig1.Text = tmp1;
                }
                catch
                {
                    return;
                }

            }
            if (readBuffer.IndexOf(chl2) != -1 && readBuffer.IndexOf(t) != -1 && readBuffer.IndexOf(m) != -1)
            {

                try
                {
                    tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 79, 16);
                    String tmp1 = tmp.Substring(tmp.IndexOf(p) + 10, 2);
                    this.txtjig2.Text = tmp1;
                }
                catch
                {
                    return;
                }
            }
        }
        //商教机S1
        private void DoUpdate1(object s, EventArgs e)
        {
            String tmp = null;
            String chl = "channel = 0";
            String chl1 = "channel = 1";
            String chl2 = "channel = 2";
            String chl3 = "channel = 4";
            String chl4 = "channel = 5";
            String chl5 = "channel = 6";
            String chl6 = "channel = 7";
            String t = "temperture";
            String m = "mec=000";
            String p = "perture";
            String readBuffer = Sp.ReadExisting();
            if (readBuffer.IndexOf(chl) != -1 && readBuffer.IndexOf(t) != -1 && readBuffer.IndexOf(m) != -1)
            {
                tmp = readBuffer.Substring(readBuffer.IndexOf(chl) + 81, 16);
                String tmp1 = tmp.Substring(tmp.IndexOf(p) + 10, 2);
                this.txthuanwen.Text = tmp1;
            }
            else if (readBuffer.IndexOf(chl1) != -1 && readBuffer.IndexOf(t) != -1 && readBuffer.IndexOf(m) != -1)
            {
                tmp = readBuffer.Substring(readBuffer.IndexOf(chl1) + 81, 16);
                String tmp1 = tmp.Substring(tmp.IndexOf(p) + 10, 2);
                this.txtselun.Text = tmp1;
            }
            else if (readBuffer.IndexOf(chl2) != -1 && readBuffer.IndexOf(t) != -1 && readBuffer.IndexOf(m) != -1)
            {
                tmp = readBuffer.Substring(readBuffer.IndexOf(chl2) + 81, 16);
                String tmp1 = tmp.Substring(tmp.IndexOf(p) + 10, 2);
                this.txtDMD.Text = tmp1;

            }
            else if (readBuffer.IndexOf(chl3) != -1 && readBuffer.IndexOf(t) != -1 && readBuffer.IndexOf(m) != -1)
            {
                tmp = readBuffer.Substring(readBuffer.IndexOf(chl3) + 81, 16);
                String tmp1 = tmp.Substring(tmp.IndexOf(p) + 10, 2);
                this.txtjig1.Text = tmp1;

            }
            else if (readBuffer.IndexOf(chl4) != -1 && readBuffer.IndexOf(t) != -1 && readBuffer.IndexOf(m) != -1)
            {
                tmp = readBuffer.Substring(readBuffer.IndexOf(chl4) + 81, 16);
                String tmp1 = tmp.Substring(tmp.IndexOf(p) + 10, 2);
                this.txtjig2.Text = tmp1;
            }
            else if (readBuffer.IndexOf(chl5) != -1 && readBuffer.IndexOf(t) != -1 && readBuffer.IndexOf(m) != -1)
            {
                tmp = readBuffer.Substring(readBuffer.IndexOf(chl5) + 81, 16);
                String tmp1 = tmp.Substring(tmp.IndexOf(p) + 10, 2);
                this.txtjig3.Text = tmp1;
            }
            else if (readBuffer.IndexOf(chl6) != -1 && readBuffer.IndexOf(t) != -1 && readBuffer.IndexOf(m) != -1)
            {
                tmp = readBuffer.Substring(readBuffer.IndexOf(chl6) + 81, 16);
                String tmp1 = tmp.Substring(tmp.IndexOf(p) + 10, 2);
                this.txtjig4.Text = tmp1;
            }
        }
        public static byte[] HexStringToByteArray(string s)
        {
            s = s.Replace(" ", "");
            byte[] buffer = new byte[s.Length / 2];
            for (int i = 0; i < s.Length; i += 2)
                buffer[i / 2] = (byte)Convert.ToByte(s.Substring(i, 2), 16);
            return buffer;
        }
        /// <summary>
        /// byte数组转换为16进制字符串
        /// </summary>
        /// <param name="bytes">byte数组</param>
        /// <returns>16进制字符串</returns>
        public string BytesToHexStr(byte[] bytes)
        {
            // 初始化返回缓冲区
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                short s = (short)bytes[i];
                sb.Append(s.ToString("X2") + " ");
            }
            return sb.ToString();
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            //  Series series = chart1.Series["DMD温度"];
            //  chart1.ChartAreas[0].AxisX.ScaleView.Position = series.Points.Count - 5;
            //  Series series1 = chart1.Series["激光器1温度"];
            //  chart1.ChartAreas[0].AxisX.ScaleView.Position = series1.Points.Count - 5;
            //  Series series2 = chart1.Series["激光器2温度"];
            //   chart1.ChartAreas[0].AxisX.ScaleView.Position = series2.Points.Count - 5;
            //   Series series3 = chart1.Series["环境温度"];
            //   chart1.ChartAreas[0].AxisX.ScaleView.Position = series3.Points.Count - 5;
            btn_send_byte_Click(btn_send_byte, null);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“equipmentDataSet1.pingqiangmessage”中。您可以根据需要移动或删除它。
            this.pingqiangmessageTableAdapter1.Fill(this.equipmentDataSet1.pingqiangmessage);
            // TODO: 这行代码将数据加载到表“equipmentDataSet.pingqiangmessage”中。您可以根据需要移动或删除它。
            this.pingqiangmessageTableAdapter.Fill(this.equipmentDataSet.pingqiangmessage);

        }
        private void btn_stop_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void lblhuanwen_Click(object sender, EventArgs e)
        {

        }

        private void labelX6_Click(object sender, EventArgs e)
        {

        }

        private void txtSn_TextChanged(object sender, EventArgs e)
        {

        }

        private void dpjixing_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label_recv_data_Click(object sender, EventArgs e)
        {

        }

        private void label98_Click(object sender, EventArgs e)
        {

        }

        private void txthuanwen_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtldcgq_TextChanged(object sender, EventArgs e)
        {

        }
    }
    public class PortInfo
    {
        /// <summary>
        /// COM口
        /// </summary>
        public string com;
        /// <summary>
        /// 波特率
        /// </summary>
        public string baud;
        /// <summary>
        /// 校验位
        /// </summary>
        public string parity;
        /// <summary>
        /// 数据位
        /// </summary>
        public string byteSize;
        /// <summary>
        /// 停止位
        /// </summary>
        public string stopBits;
    }
    /// <summary>
    /// 串口操作类
    /// </summary>
    public class SerialPortBase
    {
        /// <summary>
        /// 串口
        /// </summary>
        public static SerialPort Sp = new SerialPort();
        public SerialPortBase()
        {

        }
        /// <summary>
        /// 打开串口
        /// </summary>
        /// <param name="pi">串口信息</param>
        /// <param name="str">错误提示</param>
        /// <returns></returns>
        public bool OpenPort(PortInfo pi, ref string errString)
        {
            bool _result = false;
            if (Sp.IsOpen)
            {
                Sp.Close();
            }
            try
            {
                Sp.PortName = pi.com;
                Sp.BaudRate = int.Parse(pi.baud);
                Sp.DataBits = int.Parse(pi.byteSize);
                #region 校验位和停止位
                switch (pi.parity)
                {
                    case "None":
                        Sp.Parity = Parity.None;
                        break;
                    case "Odd":
                        Sp.Parity = Parity.Odd;
                        break;
                    case "Even":
                        Sp.Parity = Parity.Even;
                        break;
                    case "Mark":
                        Sp.Parity = Parity.Mark;
                        break;
                    default:
                        Sp.Parity = Parity.Space;
                        break;
                }
                switch (pi.stopBits)
                {
                    case "One":
                        Sp.StopBits = StopBits.One;
                        break;
                    case "Two":
                        Sp.StopBits = StopBits.Two;
                        break;
                    case "OnePointFive":
                        Sp.StopBits = StopBits.OnePointFive;
                        break;
                    default:
                        break;
                }
                #endregion
                Sp.Open();
                if (Sp.IsOpen)
                {
                    _result = true;
                }
                else
                {
                    _result = false;
                }
            }
            catch (Exception ex)
            {
                string st = string.Format("出现未处理异常：" + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
               ex.GetType().Name, ex.Message, ex.StackTrace);
                errString = ex.Message;
                return false;
            }
            return _result;
        }
        /// <summary>
        /// 关闭串口
        /// </summary>
        /// <param name="str">错误提示</param>
        /// <returns></returns>
        public bool ClosePort(ref string str)
        {
            bool _result = false;
            if (Sp.IsOpen)
            {
                try
                {
                    Sp.Close();
                    _result = true;
                }
                catch (Exception ex)
                {
                    string st = string.Format("出现未处理异常：" + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
               ex.GetType().Name, ex.Message, ex.StackTrace);
                    return false;
                }
            }
            else
                _result = true;
            return _result;
        }
        /// <summary>
        /// byte数组转换为16进制字符串
        /// </summary>
        /// <param name="bytes">byte数组</param>
        /// <returns>16进制字符串</returns>
        public static string BytesToHexStr(byte[] bytes)
        {
            // 初始化返回缓冲区
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                short s = (short)bytes[i];
                sb.Append(s.ToString("X2") + " ");
            }
            return sb.ToString();
        }
        /// <summary>
        /// 校验位计算(异或)
        /// </summary>
        /// <param name="sourceFrame"></param>
        /// <returns></returns>
        public static byte GetCS(byte[] sourceFrame)
        {
            byte _CS = 0x00;
            for (int i = 0; i < sourceFrame.Length; i++)
            {
                _CS ^= sourceFrame[i];
            }
            return _CS;
        }
        /// <summary>
        /// 计算累加和
        /// </summary>
        /// <param name="sourceFram"></param>
        /// <returns></returns>
        public static byte GetCheck(byte[] sourceFram)
        {
            int num = 0;
            //for (int i = 0; i < 19; i++)//sunny
            for (int i = 0; i < sourceFram.Length - 1; i++)
            {
                num = (num + sourceFram[i]) & 0xFF;
            }
            return Convert.ToByte(num);
        }
        /// <summary>
        /// 计算与或
        /// </summary>
        /// <param name="sourceFram"></param>
        /// <returns></returns>
        public static byte GetCheck1(byte[] sourceFram)
        {
            int num = 0;
            //for (int i = 0; i < 15; i++)//sunny
            for (int i = 1; i < sourceFram.Length - 2; i++)
            {
                num = (num ^ sourceFram[i]);// & 0xFF;
            }
            return Convert.ToByte(num);
        }
        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="sendData">发送帧</param>
        /// <param name="receiveData">返回帧</param>
        /// <returns></returns>
        public bool SendCmd(byte[] sendData, out byte[] receiveData)
        {
            bool result = false;
            if (!Sp.IsOpen)
            {
                MessageBox.Show("请先打开串口...", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                receiveData = null;
                result = false;
            }
            try
            {
                Sp.Write(sendData, 0, sendData.Length);
                string msg = BytesToHexStr(sendData);
                Thread.Sleep(250);//延时  
                result = sp_DataReceived(out receiveData);
                //result = true;
            }
            catch (Exception ex)
            {
                string st = string.Format("出现未处理异常：" + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                ex.GetType().Name, ex.Message, ex.StackTrace);
                receiveData = null;
                result = false;
            }
            //receiveData = null;
            return result;
        }
        /// <summary>
        /// 发送数据（工程机）
        /// </summary>
        /// <param name="sendData">发送帧</param>
        /// <param name="receiveData">返回帧</param>
        /// <returns></returns>
        public bool SendCmd3(byte[] sendData3, out byte[] receiveData)
        {
            bool result = false;
            if (!Sp.IsOpen)
            {
                MessageBox.Show("请先打开串口...", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                receiveData = null;
                result = false;
            }
            try
            {
                Sp.Write(sendData3, 0, sendData3.Length);
                string msg = BytesToHexStr(sendData3);
                Thread.Sleep(250);//延时  
                result = sp_DataReceived3(out receiveData);
                //result = true;
            }
            catch (Exception ex)
            {
                string st = string.Format("出现未处理异常：" + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                ex.GetType().Name, ex.Message, ex.StackTrace);
                receiveData = null;
                result = false;
            }
            //receiveData = null;
            return result;
        }
        /// <summary>
        /// 发送数据（工程机）
        /// </summary>
        /// <param name="sendData">发送帧</param>
        /// <param name="receiveData">返回帧</param>
        /// <returns></returns>
        public bool SendCmd4(byte[] sendData4, out byte[] receiveData4)
        {
            bool result = false;
            if (!Sp.IsOpen)
            {
                MessageBox.Show("请先打开串口...", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                receiveData4 = null;
                result = false;
            }
            try
            {
                Sp.Write(sendData4, 0, sendData4.Length);
                string msg = BytesToHexStr(sendData4);
                Thread.Sleep(250);//延时  
                result = sp_DataReceived4(out receiveData4);
                //result = true;
            }
            catch (Exception ex)
            {
                string st = string.Format("出现未处理异常：" + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                ex.GetType().Name, ex.Message, ex.StackTrace);
                receiveData4 = null;
                result = false;
            }
            //receiveData = null;
            return result;
        }
        /// <summary>
        /// 发送数据(机芯升级)
        /// </summary>
        /// <param name="sendData1">发送帧</param>
        /// <param name="receiveData">返回帧</param>
        /// <returns></returns>
        public bool SendCmd1(byte[] sendData1, out byte[] receiveData1)
        {
            bool result = false;
            if (!Sp.IsOpen)
            {
                MessageBox.Show("请先打开串口...", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                receiveData1 = null;
                result = false;
            }
            try
            {
                Sp.Write(sendData1, 0, sendData1.Length);
                string msg = BytesToHexStr(sendData1);
                Thread.Sleep(250);//延时  
                result = sp_DataReceived1(out receiveData1);
                //result = true;
            }
            catch (Exception ex)
            {
                string st = string.Format("出现未处理异常：" + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                ex.GetType().Name, ex.Message, ex.StackTrace);
                receiveData1 = null;
                result = false;
            }
            //receiveData = null;
            return result;
        }
        /// <summary>
        /// 发送数据(机芯升级)
        /// </summary>
        /// <param name="sendData1">发送帧</param>
        /// <param name="receiveData">返回帧</param>
        /// <returns></returns>
        public bool SendCmd2(byte[] sendData2, out byte[] receiveData2)
        {
            bool result = false;
            if (!Sp.IsOpen)
            {
                MessageBox.Show("请先打开串口...", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                receiveData2 = null;
                result = false;
            }
            try
            {
                Sp.Write(sendData2, 0, sendData2.Length);
                string msg = BytesToHexStr(sendData2);
                Thread.Sleep(250);//延时  
                result = sp_DataReceived2(out receiveData2);
                //result = true;
            }
            catch (Exception ex)
            {
                string st = string.Format("出现未处理异常：" + "异常类型：{0}\r\n异常消息：{1}\r\n异常信息：{2}\r\n",
                ex.GetType().Name, ex.Message, ex.StackTrace);
                receiveData2 = null;
                result = false;
            }
            //receiveData = null;
            return result;
        }
        /// <summary>
        /// 发送数据，不处理返回
        /// </summary>
        /// <param name="sendData"></param>
        public void SendCmd(byte[] sendData)
        {
            if (!Sp.IsOpen)
            {
                MessageBox.Show("请先打开串口...", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            Sp.Write(sendData, 0, sendData.Length);
            string msg = BytesToHexStr(sendData);
        }

        private List<byte> buffer = new List<byte>(4096);
        private bool sp_DataReceived(out byte[] rcvData) //sp是串口控件
        {
            bool result = false;
            rcvData = null;
            byte[] readBuffer = null;
            //int n = Global.Sp.BytesToRead;
            //byte[] buf = new byte[n];//堆空间
            byte[] buf = new byte[Sp.BytesToRead];
            //byte[] buf = new byte[256];//栈空间
            readBuffer = new byte[20];
            Sp.Read(buf, 0, buf.Length);
            //Global.Sp.Read(buf, 0, 256);
            //1.缓存数据           
            buffer.AddRange(buf);
            //2.完整性判断         
            while (buffer.Count >= 4)
            //for (int i = 0; i < buffer.Count; i++)
            {
                //n = Global.Sp.BytesToRead;
                //Global.Sp.Read(buf, 0, n);
                //至少包含标头(1字节),长度(1字节),校验位(2字节)等等
                //2.1 查找数据标记头      
                if (((buffer[0] & 0x0F) == 0x0A) && ((buffer[0] & 0xF0) >= 0xB0))
                {
                    if ((buffer.Count) >= 20)
                    {
                        //得到完整的数据，复制到readBuffer中    
                        buffer.CopyTo(0, readBuffer, 0, 20);
                    }
                    else
                    {
                        //数据未接收完整跳出循环
                        MessageBox.Show("数据未接收完整,请确认是否设置单元ID！", "警告", MessageBoxButtons.OK);
                        result = false;
                        break;
                    }
                    //从缓冲区中清除
                    buffer.RemoveRange(0, 20);
                    //readBuffer[5] = (byte)((0x80 & isFirstHalf) | (rowIndex & 0x7F));
                    byte checkNum2 = GetCheck(readBuffer);//计算校验
                    if (checkNum2 != readBuffer[19])
                    {
                        //buffer.RemoveRange(0, 20);
                        result = false;
                        MessageBox.Show("数据包不正确，校验错误！");
                        continue;
                    }
                    else
                    {
                        //buffer.RemoveRange(0, 20);
                        result = true;
                        rcvData = readBuffer;
                        break;
                    }
                    //触发外部处理接收消息事件

                }
                else //开始标记或版本号不正确时清除           
                {
                    result = false;
                    buffer.RemoveAt(0);
                    rcvData = null;
                }
            }
            return result;
        }
        //工程机

        private bool sp_DataReceived3(out byte[] rcvData) //sp是串口控件
        {
            bool result = false;
            rcvData = null;
            byte[] readBuffer = null;
            //int n = Global.Sp.BytesToRead;
            //byte[] buf = new byte[n];//堆空间
            byte[] buf = new byte[Sp.BytesToRead];
            //byte[] buf = new byte[256];//栈空间
            readBuffer = new byte[20];
            Sp.Read(buf, 0, buf.Length);
            //Global.Sp.Read(buf, 0, 256);
            //1.缓存数据           
            buffer.AddRange(buf);
            //2.完整性判断         
            while (buffer.Count >= 4)
            //for (int i = 0; i < buffer.Count; i++)
            {
                //n = Global.Sp.BytesToRead;
                //Global.Sp.Read(buf, 0, n);
                //至少包含标头(1字节),长度(1字节),校验位(2字节)等等
                //2.1 查找数据标记头      
                if (((buffer[0] & 0x0F) == 0x0A) && ((buffer[0] & 0xF0) == 0xF0))
                {
                    if ((buffer.Count) >= 20)
                    {
                        //得到完整的数据，复制到readBuffer中    
                        buffer.CopyTo(0, readBuffer, 0, 20);
                    }
                    else
                    {
                        //数据未接收完整跳出循环
                        MessageBox.Show("数据未接收完整,请确认是否设置单元ID！", "警告", MessageBoxButtons.OK);
                        result = false;
                        break;
                    }
                    //从缓冲区中清除
                    buffer.RemoveRange(0, 20);
                    //readBuffer[5] = (byte)((0x80 & isFirstHalf) | (rowIndex & 0x7F));
                    byte checkNum2 = GetCheck(readBuffer);//计算校验
                    if (checkNum2 != readBuffer[19])
                    {
                        //buffer.RemoveRange(0, 20);
                        result = false;
                        MessageBox.Show("数据包不正确，校验错误！");
                        continue;
                    }
                    else
                    {
                        //buffer.RemoveRange(0, 20);
                        result = true;
                        rcvData = readBuffer;
                        break;
                    }
                    //触发外部处理接收消息事件

                }
                else //开始标记或版本号不正确时清除           
                {
                    result = false;
                    buffer.RemoveAt(0);
                    rcvData = null;
                }
            }
            return result;
        }
        private bool sp_DataReceived4(out byte[] rcvData4) //sp是串口控件
        {
            bool result = false;
            rcvData4 = null;
            byte[] readBuffer = null;
            //int n = Global.Sp.BytesToRead;
            //byte[] buf = new byte[n];//堆空间
            byte[] buf = new byte[Sp.BytesToRead];
            //byte[] buf = new byte[256];//栈空间
            readBuffer = new byte[20];
            Sp.Read(buf, 0, buf.Length);
            //Global.Sp.Read(buf, 0, 256);
            //1.缓存数据           
            buffer.AddRange(buf);
            //2.完整性判断         
            while (buffer.Count >= 4)
            //for (int i = 0; i < buffer.Count; i++)
            {
                //n = Global.Sp.BytesToRead;
                //Global.Sp.Read(buf, 0, n);
                //至少包含标头(1字节),长度(1字节),校验位(2字节)等等
                //2.1 查找数据标记头      
                if (((buffer[0] & 0x0F) == 0x0A) && ((buffer[0] & 0xF0) == 0xF0))
                {
                    if ((buffer.Count) >= 20)
                    {
                        //得到完整的数据，复制到readBuffer中    
                        buffer.CopyTo(0, readBuffer, 0, 20);
                    }
                    else
                    {
                        //数据未接收完整跳出循环
                        MessageBox.Show("数据未接收完整,请确认是否设置单元ID！", "警告", MessageBoxButtons.OK);
                        result = false;
                        break;
                    }
                    //从缓冲区中清除
                    buffer.RemoveRange(0, 20);
                    //readBuffer[5] = (byte)((0x80 & isFirstHalf) | (rowIndex & 0x7F));
                    byte checkNum2 = GetCheck(readBuffer);//计算校验
                    if (checkNum2 != readBuffer[19])
                    {
                        //buffer.RemoveRange(0, 20);
                        result = false;
                        MessageBox.Show("数据包不正确，校验错误！");
                        continue;
                    }
                    else
                    {
                        //buffer.RemoveRange(0, 20);
                        result = true;
                        rcvData4 = readBuffer;
                        break;
                    }
                    //触发外部处理接收消息事件

                }
                else //开始标记或版本号不正确时清除           
                {
                    result = false;
                    buffer.RemoveAt(0);
                    rcvData4 = null;
                }
            }
            return result;
        }
        //机芯升级
        private List<byte> buffer1 = new List<byte>(4096);
        private bool sp_DataReceived1(out byte[] rcvData1) //sp是串口控件
        {
            bool result1 = false;
            rcvData1 = null;
            byte[] readBuffer1 = null;
            //  int n = Global.Sp.BytesToRead;
            // byte[] buf = new byte[n];//堆空间
            byte[] buf1 = new byte[Sp.BytesToRead];
            readBuffer1 = new byte[16];
            Sp.Read(buf1, 0, buf1.Length);
            string str = Encoding.Default.GetString(buf1);
            str = str.Replace(" ", "");
            str = str.Replace("\r", "");
            str = str.Replace("\n", "");
            if ((str.Length % 2) != 0)
                str += " ";
            byte[] returnBytes = new byte[str.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);



            //Global.Sp.Read(buf, 0, 256);
            //1.缓存数据           
            buffer.AddRange(returnBytes);
            //2.完整性判断         
            while (buffer.Count >= 4)
            {
                //n = Global.Sp.BytesToRead;
                //Global.Sp.Read(buf, 0, n);
                //至少包含标头(1字节),长度(1字节),校验位(2字节)等等
                //2.1 查找数据标记头    
                //        for (int i = 0; i < buffer.Count; i++)              
                if (((buffer[0] & 0x0F) == 0x05) && ((buffer[0] & 0xF0) == 0xA0) && ((buffer[15] & 0x0F) == 0x05) && ((buffer[15] & 0xF0) == 0xA0))
                // if(buffer[0]==buffer[15])
                {
                    if ((buffer.Count) >= 16)
                    {
                        //得到完整的数据，复制到readBuffer中    
                        buffer.CopyTo(0, readBuffer1, 0, 16);
                    }
                    else
                    {
                        //数据未接收完整跳出循环
                        MessageBox.Show("数据未接收完整,请确认是否设置单元ID！", "警告", MessageBoxButtons.OK);
                        result1 = false;
                        break;
                    }
                    //从缓冲区中清除
                    buffer.RemoveRange(0, 16);
                    //readBuffer[5] = (byte)((0x80 & isFirstHalf) | (rowIndex & 0x7F));
                    byte checkNum2 = GetCheck1(readBuffer1);//计算校验
                    if (checkNum2 != readBuffer1[14])
                    {
                        // buffer.RemoveRange(0, 16);
                        result1 = false;

                        MessageBox.Show("数据包不正确，校验错误！");
                        continue;
                    }
                    else
                    {

                        result1 = true;
                        rcvData1 = readBuffer1;
                        break;
                    }
                    //触发外部处理接收消息事件

                }


                else //开始标记或版本号不正确时清除           
                {
                    result1 = false;
                    buffer.RemoveAt(0);
                    rcvData1 = null;
                }
            }

            return result1;

        }
        private List<byte> buffer2 = new List<byte>(4096);
        private bool sp_DataReceived2(out byte[] rcvData2) //sp是串口控件
        {
            bool result1 = false;
            rcvData2 = null;
            byte[] readBuffer = null;
            //  int n = Global.Sp.BytesToRead;
            // byte[] buf = new byte[n];//堆空间
            byte[] buf1 = new byte[Sp.BytesToRead];

            readBuffer = new byte[16];
            Sp.Read(buf1, 0, buf1.Length);
            string str = Encoding.Default.GetString(buf1);
            str = str.Replace(" ", "");
            str = str.Replace("\r", "");
            str = str.Replace("\n", "");
            if ((str.Length % 2) != 0)
                str += " ";
            byte[] returnBytes = new byte[str.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(str.Substring(i * 2, 2), 16);



            //Global.Sp.Read(buf, 0, 256);
            //1.缓存数据           
            buffer.AddRange(returnBytes);
            //2.完整性判断         
            while (buffer.Count >= 4)
            {
                //n = Global.Sp.BytesToRead;
                //Global.Sp.Read(buf, 0, n);
                //至少包含标头(1字节),长度(1字节),校验位(2字节)等等
                //2.1 查找数据标记头    
                //        for (int i = 0; i < buffer.Count; i++)              
                if (((buffer[0] & 0x0F) == 0x05) && ((buffer[0] & 0xF0) == 0xA0) && ((buffer[15] & 0x0F) == 0x05) && ((buffer[15] & 0xF0) == 0xA0))
                // if(buffer[0]==buffer[15])
                {
                    if ((buffer.Count) >= 16)
                    {
                        //得到完整的数据，复制到readBuffer中    
                        buffer.CopyTo(0, readBuffer, 0, 16);
                    }
                    else
                    {
                        //数据未接收完整跳出循环
                        MessageBox.Show("数据未接收完整,请确认是否设置单元ID！", "警告", MessageBoxButtons.OK);
                        result1 = false;
                        break;
                    }
                    //从缓冲区中清除
                    buffer.RemoveRange(0, 16);
                    //readBuffer[5] = (byte)((0x80 & isFirstHalf) | (rowIndex & 0x7F));
                    byte checkNum2 = GetCheck1(readBuffer);//计算校验
                    if (checkNum2 != readBuffer[14])
                    {
                        // buffer.RemoveRange(0, 16);
                        result1 = false;

                        MessageBox.Show("数据包不正确，校验错误！");
                        continue;
                    }
                    else
                    {

                        result1 = true;
                        rcvData2 = readBuffer;
                        break;
                    }
                    //触发外部处理接收消息事件

                }


                else //开始标记或版本号不正确时清除           
                {
                    result1 = false;
                    buffer.RemoveAt(0);
                    rcvData2 = null;
                }
            }

            return result1;

        }
    }
}