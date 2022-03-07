using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LinqToSql201
{
    public partial class Form1 : Form
    {
      librairieDataContext db = new librairieDataContext();


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            var c = db.EDITEUR;
            dataGridView1.DataSource = c;


        }

        private void button2_Click(object sender, EventArgs e)
        {


          //  afficher la liste des noms et des villes de tous les editeurs
            var ed = from edi in db.EDITEUR
                     select new { edi.NOMED, edi.VILLEED };


            dataGridView1.DataSource = ed;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //afficher la liste des noms et des villes des editeurs qui habitent a paris

            // avec ==
            //var ed = from edi in db.EDITEUR
            //         where edi.VILLEED == "paris"
            //         select new { edi.NOMED, edi.VILLEED };

            //avec equals
            var ed = from edi in db.EDITEUR
                     where edi.VILLEED.Equals("paris")
                     select new { edi.NOMED, edi.VILLEED };

           

            dataGridView1.DataSource = ed;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //afficher la liste des noms et des villes des editeurs qui habitent a paris
            //avec le tri descendant sur les nom

            //var x = db.EDITEUR;
         
            //var a = x.Where( c => c.VILLEED == "paris");
            //var b = a.OrderBy(c => c.NOMED); //OrderByDescending
            //var ed = b.Select(c => new { c.NOMED, c.VILLEED });


            var ed = db.EDITEUR
                .Where(c => c.VILLEED == "paris")
                .OrderBy(c => c.NOMED) //OrderByDescending
                .Select(c => new { c.NOMED, c.VILLEED });

            dataGridView1.DataSource = ed;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            // liste des editeurs et des ouvrages


            //produit cartesien
            //var ed = (from edi in db.EDITEUR
            //          from ouv in db.OUVRAGE
            //          where edi.NOMED == ouv.NOMED
            //          select new { edi.NOMED, ouv.NOMOUVR }).Distinct();


            //jointure interne (inner join)
            var ed = (from edi in db.EDITEUR
                      join ouv in db.OUVRAGE
                      on edi.NOMED equals ouv.NOMED
                      select new { edi.NOMED, ouv.NOMOUVR }).Distinct();
            label1.Text = ed.Count().ToString();


            dataGridView1.DataSource = ed;


        }
    }
}
