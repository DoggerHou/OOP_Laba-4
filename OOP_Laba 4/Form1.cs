﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP_Laba_4
{

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Storage myStorage = new Storage();
        bool controlUp = false;
        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < myStorage.getSize(); i++)
            {
                myStorage.getCCircle(i).OnPaint(e);
            }

        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            myStorage.AddObject(new CCircle(e.Location), e, controlUp);
            label1.Text = myStorage.getSize().ToString();
            pictureBox.Invalidate();
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Control)
                controlUp = true;
            else if (e.KeyData == Keys.Delete)
                myStorage.deleteDetailedCCircle();
            label1.Text = myStorage.getSize().ToString();
            pictureBox.Invalidate();
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            controlUp = false;
        }
    }

    public class CCircle 
    {
        private const int RADIX = 80;
        private Point location;
        private bool detail;
        public CCircle(Point location)
        {
            this.location = location;
            detail = true;
        }
        public void changeDetail_toFalse()
        {
            detail = false;
        }
        public bool getDetail()
        {
            return detail;
        }
        public void OnPaint(PaintEventArgs e)
        {
            if(detail == false)
                e.Graphics.DrawEllipse(new Pen(Color.Red,5f), location.X - RADIX/2, location.Y - RADIX/2, RADIX, RADIX);
            else
                e.Graphics.DrawEllipse(new Pen(Color.Black, 8f), location.X - RADIX/2, location.Y - RADIX/2, RADIX, RADIX);
        }
        public bool isPicked(MouseEventArgs e, bool controlUp)
        {
            if (Math.Pow(location.X - e.X, 2) + Math.Pow(location.Y - e.Y, 2) <= RADIX * RADIX  
                & controlUp )
            {
                detail = true;
                return true;
            }
            else
                return false;
        }
    }

    public class Storage
    {
        private List<CCircle> objects;
        public Storage()
        {
            objects = new List<CCircle>();
        }
        public void AddObject(CCircle temp_object, MouseEventArgs e, bool controlUp)  //добавляет объект
        {
            for (int i = 0; i < objects.Count(); i++)
                if (objects[i].isPicked(e, controlUp) == true)
                {
                    return;
                }
            objects.Add(temp_object);
            for (int i = 0; i < objects.Count() - 1; i++)
                objects[i].changeDetail_toFalse();
        }

        public int getSize()        //возвращает размер Списка
        {
            return objects.Count();
        }
        public CCircle getCCircle(int index)    //возвращает объект CCircle
        {
            return objects[index];
        }
        public void deleteDetailedCCircle()    //удаляет все "помеченные" объекты
        {
            for(int i = objects.Count()-1; i>=0;i--)
                if (objects[i].getDetail() == true)
                    objects.RemoveAt(i);
        }
    }
}
