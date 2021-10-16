using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace AppDataStudent
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string StrConn = "Provider = Microsoft.ACE.OLEDB.12.0;data source = C:/Users/class/Desktop/Database/db_student.accdb";
        public string sql;
        OleDbConnection Conn = new OleDbConnection();
        OleDbDataAdapter da;
        DataSet ds = new DataSet();
        bool IsFind = false;

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Conn.State == ConnectionState.Open)
            {
                Conn.Close();
            }
            Conn.ConnectionString = StrConn;
            Conn.Open();

            ShowAllStudent();
            FormatDataStudent();
            ClearAllStudent();
        }

        private void ShowAllStudent()
        {
            string sqlStu = "select * from tb_histstudent";
            if (IsFind == true)
            {
                ds.Tables["tb_hisstudent"].Clear();
            }
            da = new OleDbDataAdapter(sqlStu, Conn);
            da.Fill(ds, "tb_histstudent");
            if (ds.Tables["tb_histstudent"].Rows.Count != 0){
                IsFind = true;
                dgvShow.DataSource = ds.Tables["tb_histstudent"];
            }
            else
            {
                IsFind = false;
            }
        }

        private void FormatDataStudent()
        {
            DataGridViewCellStyle cs = new DataGridViewCellStyle();
            cs.Font = new Font("Ms Sans Serif", 9, FontStyle.Regular);
            dgvShow.ColumnHeadersDefaultCellStyle = cs;
            dgvShow.Columns[0].HeaderText = "รหัสนักศึกษา";
            dgvShow.Columns[1].HeaderText = "ชื่อ";
            dgvShow.Columns[2].HeaderText = "นามสกุล";
            dgvShow.Columns[3].HeaderText = "วัน เดือน ปี เกิด";
            dgvShow.Columns[4].HeaderText = "น้ำหนัก";

            dgvShow.Columns[0].Width = 80;
            dgvShow.Columns[1].Width = 120;
            dgvShow.Columns[2].Width = 120;
            dgvShow.Columns[3].Width = 100;
            dgvShow.Columns[4].Width = 80;
        }

        private void dgvShow_CellMouseUp(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex == dgvShow.Rows.Count - 1)
            {
                return;
            }
            try
            {
                txtid.Text = dgvShow.Rows[e.RowIndex].Cells[0].Value.ToString();
                txtName.Text = dgvShow.Rows[e.RowIndex].Cells[1].Value.ToString();
                txtSName.Text = dgvShow.Rows[e.RowIndex].Cells[2].Value.ToString();
                dtBirth.Value = Convert.ToDateTime(dgvShow.Rows[e.RowIndex].Cells[3].Value);
                txtWeight.Text = dgvShow.Rows[e.RowIndex].Cells[4].Value.ToString();
                
            }
            catch
            {
                MessageBox.Show("เกิดข้อผิดพลาด");
            }
        }

        private void ClearAllStudent()
        {
            txtid.Text = "";
            txtName.Text = "";
            txtSName.Text = "";
            dtBirth.Value = DateTime.Now;
            txtWeight.Text = "";
            txtid.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearAllStudent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtid.Text == "")
            {
                MessageBox.Show("กรุณากรอกข้อมูลให้ครบ","ผิดพลาด");
                txtid.Focus();
                return;
            }
            string sqlAdd = "";
            OleDbCommand ComAdd = new OleDbCommand();
            try
            {
                if (MessageBox.Show("คุณต้องการเพิ่มข้อมูลใช่หรือไม่ ?","คำเตือน",MessageBoxButtons.YesNo,MessageBoxIcon.Question)== DialogResult.Yes)
                {
                    sqlAdd = "insert into tb_histstudent(stu_id,stu_name,stu_surname,stu_bdate,stu_weight)value('"
                        + txtWeight.Text + "','" 
                        + txtName.Text + "','" 
                        + txtSName.Text + "','" 
                        + dtBirth.Value + "','" 
                        + txtWeight.Text + "')";

                    if(Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                }
                Conn.ConnectionString = StrConn;
                Conn.Open();

                ComAdd.CommandType = CommandType.Text;
                ComAdd.CommandText = sqlAdd;
                ComAdd.Connection = Conn;
                ComAdd.ExecuteNonQuery();

                MessageBox.Show("ได้บันทึกข้อมูลเรียบร้อยแล้ว","ผลการดำเนินการ");
                ClearAllStudent();
                ShowAllStudent();
            }
            catch
            {
                MessageBox.Show("ไม่สามารถติดต่อฐานข้อมูลได้","ผิดพลาด");
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (txtName.Text == "")
            {
                MessageBox.Show("กรุณากรอกข้อมูลที่จะแก้ไขให้ครบ", "ผิดพลาด");
                txtName.Focus();
                return;
            }
            string sqlEdit = "";
            OleDbCommand ComEdit = new OleDbCommand();
            try
            {
                if (MessageBox.Show("คุณต้องการแก้ไขข้อมูลใช่หรือไม่ ?", "ยืนยัน", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    sqlEdit = "update tb_histstudent set stu_name = '" + txtName.Text + "',stu_surname = '" + txtSName.Text + "',stu_bdate = '" + dtBirth.Value + "',stu_weight = '" 
                        + txtWeight.Text+ "',where stu_id ='" + txtid.Text + "'";

                    if (Conn.State == ConnectionState.Open)
                    {
                        Conn.Close();
                    }
                }
                Conn.ConnectionString = StrConn;
                Conn.Open();

                ComEdit.CommandType = CommandType.Text;
                ComEdit.CommandText = sqlEdit;
                ComEdit.Connection = Conn;
                ComEdit.ExecuteNonQuery();

                MessageBox.Show("แก้ไขข้อมูลเรียบร้อยแล้ว", "ผลการดำเนินการ");
                ClearAllStudent();
                ShowAllStudent();
            }
            catch
            {
                MessageBox.Show("ข้อมูลผิดพลาด", "ผิดพลาด");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtid.Text == "")
                {
                    MessageBox.Show("กรุณาเลือกข้อมูลที่จะลบ", "ผิดพลาด");
                    return;
                }
                string sqlDel = "delete from tb_histstudent where stu_id = '" + txtid.Text + "'";
                if (Conn.State == ConnectionState.Open)
                {
                    Conn.Close();
                }

                Conn.ConnectionString = StrConn;
                Conn.Open();
                OleDbCommand ComDel = new OleDbCommand(sqlDel, Conn);
                if (MessageBox.Show("คุณต้องการลบข้อมูลนี้ใช่หรือไม่ ?", "ยืนยัน", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {

                    ComDel.ExecuteNonQuery();
                    MessageBox.Show("ลบข้อมูลเรียบร้อยแล้ว", "ผลการดำเนินการ");
                    ClearAllStudent();
                    ShowAllStudent();
                }
             }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("คุณต้องการจบการทำงานหรือไม่?", "Exit ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                MessageBox.Show("จบการทำงานครับ", "End");
                this.Close();
            }
            else
            {
                MessageBox.Show("คุณยังคงทำงานต่อไป","Continue");
            }
        }
       
    }
}
