using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebAppUsers.Admin
{
    public partial class InsertStudent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            {

                /*pri kliku na dugme css klase resetujemo na default
                 jer je mozda prethodno opet kliknuto dugme i ispravljena
                greska pa da se vise ne crvene textbox-ovi.*/

                try
                {

                    TextBoxFirstName.CssClass = "form-control";
                    TextBoxLastName.CssClass = "form-control";
                    TextBoxYear.CssClass = "form-control";

                    if (Page.IsValid) //ako je stranica validna, tj sve provere svih validatora su prosle
                    {
                        lblOutput.ForeColor = System.Drawing.Color.DarkGreen;
                        lblOutput.Text = "Page Valid!";

                        string connectionString = GetConnectionString();

                        SqlConnection con = new SqlConnection(connectionString);

                        //dohvatam podatke iz TextBox-a
                        string lastName = TextBoxFirstName.Text.Trim();
                        string firstName = TextBoxLastName.Text.Trim();
                        int year = int.Parse(TextBoxYear.Text);

                        InsertStudent(con, lastName, firstName, year);

                        /*redirektujemo se(prebacujemo) na istu stranicu
                         * tj. kao da je automatski refresh-ujemo
                        da bismo videli u tabeli novog ubacenog studenta.
                        Ponovnim ucitavanjem stranice takodje vracamo css klase
                        TextBox-ova na prvobitne tj. da se ne crvene vise ako jesu prethodno.
                        Metod Redirect ima 2 argmenta, prvi je stranica na koju idemo,
                        a drugi je bool endResponse koji govori da li treba ili
                        ne prekinuti izvrsavanje trenutne stranice. Stavljamo uvek
                        false da ne bismo dobili Exception:
                        System.Threading.ThreadAbortException' in mscorlib.dll

                        Lokacija stranice u odnosu na koreni(root) direktorijum projekta je folder Admin
                        i u tom folderu stranica InsertStudent

                        */

                        Response.Redirect("~/Admin/InsertStudent", false);
                    }
                    else //inace ako se desio neki problem pri validaciji
                    {
                        /*
                         hocemo da se svaki text box zacrveni ako unos u njega
                        nije dobar. Koristimo += da bi na vec postojece CSS klase
                        dodali one koje crvene TextBox.
                        Provera moze biti preko toga da li je validator odgovarajuceg textbox-a
                        validan ili preko ispitivanja vrednosti u TextBox-u tj. da li je
                        nesto uneto u njega. Lakse je i brze preko validatora nego
                        da rucno ispistujemo unos u TextBox-u, pogotovo za prezime i ime
                         */


                        if (RequiredFieldValidator1.IsValid == false || RegularExpressionValidator1.IsValid == false)
                            TextBoxFirstName.CssClass += " alert-danger textbox-warning";
                        if (RequiredFieldValidator2.IsValid == false || RegularExpressionValidator2.IsValid == false)
                            TextBoxLastName.CssClass += " alert-danger textbox-warning";
                        if (TextBoxYear.Text.Trim().Equals("") || (TextBoxYear.Text.Trim() != "1" && TextBoxYear.Text.Trim() != "2") && TextBoxYear.Text.Trim() != "3" && TextBoxYear.Text.Trim() != "4")
                            TextBoxYear.CssClass += " alert-danger textbox-warning";

                        lblOutput.ForeColor = System.Drawing.Color.Red;
                        lblOutput.Text = "Page Invalid!";

                    }

                }
                catch (Exception ex)
                {
                    ErrorLabel.Text = "SERVER ERROR";
                    ErrorLabel.ForeColor = System.Drawing.Color.Red;
                    System.Diagnostics.Debug.WriteLine("Exception Message: " + ex.Message);
                    System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);
                }
            }

            string GetConnectionString()
            {
                return "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=University;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            }

            void InsertStudent(SqlConnection con, string lastName, string firstName, int year)
            {
                try
                {
                    using (con)
                    {
                        con.Open();

                        SqlParameter p1 = new SqlParameter();
                        SqlParameter p2 = new SqlParameter();
                        SqlParameter p3 = new SqlParameter();

                        p1.Value = firstName;
                        p2.Value = lastName;
                        p3.Value = year;

                        p1.ParameterName = "@Name";
                        p2.ParameterName = "@LastName";
                        p3.ParameterName = "@year";

                        string query = "INSERT INTO Students (FirstName, LastName, Year) " +
                                        "VALUES (@Name, @LastName, @year)";

                        SqlCommand command = new SqlCommand(query, con);

                        command.Parameters.Add(p1);
                        command.Parameters.Add(p2);
                        command.Parameters.Add(p3);

                        command.ExecuteNonQuery();
                    }

                }
                catch (Exception ex)
                {
                    ErrorLabel.Text = "SERVER ERROR";
                    ErrorLabel.ForeColor = System.Drawing.Color.Red;
                    System.Diagnostics.Debug.WriteLine("Exception Message: " + ex.Message);
                    System.Diagnostics.Debug.WriteLine("Stack Trace: " + ex.StackTrace);
                }
            }
        }
    }
}