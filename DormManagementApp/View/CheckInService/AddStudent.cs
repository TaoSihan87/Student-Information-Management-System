﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DormManagementApp
{
    public partial class AddStudent : Form
    {
        public AddStudent()
        {
            InitializeComponent();
        }

        protected override void OnShown(EventArgs e)
        {
            List<Domitory> list = new List<Domitory>();
            var result = APIHelper.Get<ApiResult<List<Domitory>>>(WebSetting.GetUrl("Domitory/List"));

            if (result.Code == 0)
            {
                list = result.Data;
            }

            if (list.Count < 1)
                MessageBox.Show("没有宿舍可选");

            this.comboBox1.DisplayMember = "DomitoryID";
            this.comboBox1.ValueMember = "DomitoryID";
            this.comboBox1.DataSource = list;

            this.dateTimePicker1.Value = DateTime.Now;
            base.OnShown(e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            long no = Convert.ToInt64(this.textBox1.Text);
            string name = this.textBox2.Text;
            long phone = Convert.ToInt64(this.textBox3.Text);
            string subject = this.textBox4.Text;
            string className = this.textBox5.Text;
            long domitoryNo = this.comboBox1.SelectedValue != null ? Convert.ToInt64(this.comboBox1.SelectedValue.ToString()) : 0;
            DateTime checkInTime = this.dateTimePicker1.Value;

            if (domitoryNo < 1)
            {
                MessageBox.Show("请选择宿舍");
                return;
            }
            Student student = new Student()
            {
                StudentID = no,
                Name = name,
                PhoneNumber = phone,
                SubjectName = subject,
                ClassName = className,
                RoomNumber = domitoryNo,
                CheckInTime = checkInTime
            };

            var result = APIHelper.Post<ApiResult>(WebSetting.GetUrl("Student/Add"),
                new Dictionary<string, object>() {
                    { "StudentID", student.StudentID },
                    { "Name", student.Name },
                    { "PhoneNumber", student.PhoneNumber } ,
                    { "SubjectName", student.SubjectName } ,
                    { "ClassName", student.ClassName } ,
                    { "DomitoryID", student.RoomNumber } ,
                    { "CheckInTime", student.CheckInTime } ,
                });

            if (result.Code == 0)
                MessageBox.Show(result.Msg);
            else
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
