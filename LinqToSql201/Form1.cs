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
            var ed = from edi in db.EDITEUR
                     select new { edi.NOMED, edi.VILLEED, edi.TELED };

            comboBox1.DisplayMember = "nomed";
            comboBox1.ValueMember = "nomed";
            comboBox1.DataSource = ed;

            listBox1.DisplayMember = "nomed";
            listBox1.ValueMember = "nomed";
            listBox1.DataSource = ed;


            textBox1.DataBindings.Add("text", ed, "nomed");
            textBox2.DataBindings.Add("text", ed, "villeed");
            textBox3.DataBindings.Add("text", ed, "teled");

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

        private void button6_Click(object sender, EventArgs e)
        {
            // passer à une table sans jointure en utilisant la clé étrangère

            var ed = (from ouv in db.OUVRAGE
                      select new { ouv.EDITEUR.VILLEED, ouv.NOMOUVR });
            dataGridView1.DataSource = ed;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //appliquer distinct sur le resultat de linq

            //var ed = (from edi in db.EDITEUR
            //          select new { edi.NOMED }).Distinct();
            //label1.Text = ed.Count().ToString();

            //var ed = (from ouv in db.OUVRAGE
            //          select new { ouv.EDITEUR.NOMED, ouv.NOMOUVR });


            //liste des editeurs qui ont des ouvrages
            //31 resultats
            //var ed = (from edi in db.EDITEUR
            //          where edi.OUVRAGE.Count() > 0 //.Any()
            //          select new { edi.NOMED }).Distinct();

            //liste de tous les editeurs
            //33 resulats
            //var ed = (from edi in db.EDITEUR
            //          select new { edi.NOMED }).Distinct();

            ////liste des editeurs qui ont des ouvrages mais en utilisant join
            //31 resultats
            var ed = (from edi in db.EDITEUR
                      join ouv in db.OUVRAGE
                      on edi.NOMED equals ouv.NOMED
                      select new { edi.NOMED }).Distinct();



            label1.Text = ed.Count().ToString();
            dataGridView1.DataSource = ed;

        }

        private void button8_Click(object sender, EventArgs e)
        {
            //left join
            var ed = (from edi in db.EDITEUR
                      join ouv in db.OUVRAGE
                      on edi.NOMED equals ouv.NOMED into nomJointure
                    from o in nomJointure.DefaultIfEmpty()
                      select new { nom = edi.NOMED, titre = o.NOMOUVR }).Distinct();
            label1.Text = ed.Count().ToString();

            dataGridView1.DataSource = ed;


        }

        private void button9_Click(object sender, EventArgs e)
        {
            //nombre des editeurs par ville
            var ed = from edi in db.EDITEUR
                     group edi by edi.VILLEED into G_edi
                     select new { Ville = G_edi.Key, nb = G_edi.Count() }; //expression lambda
            dataGridView1.DataSource = ed;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            //nombre des ouvrages par editeur
            var ed = from edi in db.EDITEUR
                     join ouv in db.OUVRAGE
                     on edi.NOMED equals ouv.NOMED
                     group edi by edi.NOMED into G_edi
                     select new { G_edi.Key, nbOuvrages = G_edi.Count() };
            dataGridView1.DataSource = ed;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //somme des prix de vente par editeur
            var ed = from edi in db.EDITEUR
                     join ouv in db.OUVRAGE on edi.NOMED equals ouv.NOMED
                     join tar in db.TARIFER on ouv.NUMOUVR equals tar.NUMOUVR
                     group tar by tar.OUVRAGE.EDITEUR.NOMED into Gtar
                     select new { Editeur = Gtar.Key, total = Gtar.Sum(c => c.PRIXVENTE) };
            dataGridView1.DataSource = ed;
        }

        private void button12_Click(object sender, EventArgs e)
        {

            EDITEUR ed = new EDITEUR();
            ed.NOMED = "hamza9";
            ed.VILLEED = "Tanger";
            ed.ADRED = "boulevard";
            ed.TELED = "0654845675421";
            db.EDITEUR.InsertOnSubmit(ed);
            db.SubmitChanges();
        }
    }
}
