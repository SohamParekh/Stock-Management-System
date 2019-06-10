using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Threading;
//indexers,method overridding,overloading,interface,abstract class,constructors,partial class,properties
namespace inheritance
{


    class Adminpanel
    {
        
        static string connection = "SERVER = 127.0.0.1;PORT = 3306; DATABASE = stockmanagement; UID = root";
        static MySqlConnection con = new MySqlConnection(connection);
        MySqlCommand cmd;
        MySqlDataReader reader;
        static string username = "admin";
        static string password = "admin";
        public void display()
        {
            Console.Clear();
            string usname;
            string paswd, product;
            int sh, quantity, price;
            con.Open();
            do

            {
                Console.Write("Username :");
                usname = Console.ReadLine();
                Console.Write("Password :");
                paswd = Console.ReadLine();
                if (username.Equals(usname) && password.Equals(paswd))
                {
                    Console.Clear();
                    Thread.Sleep(500);
                    Console.WriteLine("1-Add Product");
                    Thread.Sleep(500);
                    Console.WriteLine("2-View Products");
                    Thread.Sleep(500);
                    Console.WriteLine("3-Delete Products");
                    sh = int.Parse(Console.ReadLine());
                    if (sh == 1)
                    {
                        Console.Clear();
                        Console.WriteLine("Enter Product Name : ");
                        product = Console.ReadLine();
                        Console.WriteLine("Enter Product Quantity : ");
                        quantity = int.Parse(Console.ReadLine());
                        Console.WriteLine("Enter Product Price : ");
                        price = int.Parse(Console.ReadLine());
                        try
                        {
                            cmd = new MySqlCommand("insert into product values(@p,@q,@p1)", con);
                            cmd.Parameters.AddWithValue("@p", product);
                            cmd.Parameters.AddWithValue("@q", quantity);
                            cmd.Parameters.AddWithValue("@p1", price);
                            cmd.ExecuteNonQuery();
                            Thread.Sleep(1000);
                            Console.WriteLine("Product Added successfully!");
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }
                    else if (sh == 2)
                    {
                        Console.Clear();
                        cmd = new MySqlCommand("select * from product", con);
                        reader = cmd.ExecuteReader();
                        Console.WriteLine("Quantity    Product       Price");
                        while (reader.Read())
                        {
                            Console.WriteLine("{0}         {1}         {2}", reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2));
                        }
                    }
                    else if(sh == 3)
                    {
                        Console.WriteLine("Enter the product Name : ");
                        product = Console.ReadLine();
                        cmd = new MySqlCommand("delete from product where name=@p", con);
                        cmd.Parameters.AddWithValue("@p", product);
                        
                        cmd.ExecuteNonQuery();
                        Thread.Sleep(1000);
                        Console.WriteLine("Product Deleted successfully!");
                    }
                    else
                        Console.WriteLine("Invalid Option");
                        Console.Read();



                }
                if (!(username.Equals(usname) && password.Equals(paswd)))
                    Console.WriteLine("Invalid Username or Password");
            } while (!(username.Equals(usname) && password.Equals(paswd)));



        }
    }


    class Guestpanel
    {
        static string connection = "SERVER = 127.0.0.1;PORT = 3306; DATABASE = stockmanagement; UID = root";
        static MySqlConnection con = new MySqlConnection(connection);
        public string product_name, email, name;
        public int priceinitial,quantity, product_Quan;
        
        public long number;
        MySqlCommand cmd;
        MySqlDataReader reader;
        public void display1()
        {
            con.Open();
            Console.Clear();
            string   s1;

            
            int re;
            Console.Clear();
            Console.WriteLine("Enter Your Name : ");
            name = Console.ReadLine();
            Console.WriteLine("Enter Your Contact No: ");
            number = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Your Email: ");
            email = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("Welcome {0}", name);
            cmd = new MySqlCommand("create table bill(product VARCHAR(30) , quantity INT(30) , price INT (30))", con);
            cmd.ExecuteNonQuery();
            try
            {
                cmd = new MySqlCommand("select * from product", con);
                reader = cmd.ExecuteReader();
                Console.WriteLine("Product    Quantity       Price");
                while (reader.Read())
                {
                    
                    Console.WriteLine("{0}         {1}         {2}", reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2));
                    
                }
                reader.Close();


                do
                {

                    Thread.Sleep(500);
                    Console.WriteLine("Enter the product : ");
                    product_name = Console.ReadLine();
                    cmd = new MySqlCommand("select * from product where product=@pn", con);
                     cmd.Parameters.AddWithValue("@pn", product_name);
                    reader = cmd.ExecuteReader();
                    while(reader.Read())
                    {
                        product_name = reader.GetString(0);
                        quantity = reader.GetInt32(1);
                        priceinitial = reader.GetInt32(2);
                        Console.WriteLine("{0}         {1}         {2}", reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2));
                    }
                    reader.Close();
                    cmd.ExecuteNonQuery();
                    if(quantity == 0)
                    {
                        Console.WriteLine("Product Not Available");
                    }
                    else
                    {
                        Console.WriteLine("Enter the Quantity : ");
                        product_Quan = int.Parse(Console.ReadLine());
                        quantity -= product_Quan;

                        cmd = new MySqlCommand("update product set quantity=@quan where product=@pn", con);
                        cmd.Parameters.AddWithValue("@quan", quantity);
                        cmd.Parameters.AddWithValue("@pn", product_name);
                        cmd.ExecuteNonQuery();
                        cmd = new MySqlCommand("insert into bill values(@pn , @q2, @p2)", con);
                        cmd.Parameters.AddWithValue("@pn", product_name);
                        cmd.Parameters.AddWithValue("@q2", product_Quan);
                        cmd.Parameters.AddWithValue("@p2", (product_Quan * priceinitial));
                        re = cmd.ExecuteNonQuery();


                        if (re == 1)
                        {
                            Thread.Sleep(500);
                            Console.WriteLine("Product Purchased!");
                        }
                            
                    }
                    
                    
                    
                    Console.WriteLine("Would you like to buy other stock(y/n) : ");
                    s1 = Console.ReadLine();
                } while (s1.Equals("y"));
                //Console.ReadLine();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            try
            {
                Console.WriteLine("Products Buyed");
                cmd = new MySqlCommand("select * from bill", con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("{0}         {1}         {2}", reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2));
                }
                reader.Close();

                Console.WriteLine("Press any key to Generate Bill");
                int sh = int.Parse(Console.ReadLine());
                bill();
                Console.Read();
            }
            catch (Exception e) { Console.WriteLine(e.Message); }

        }
        public void bill()
        {
            try
            {
                Console.Clear();
                Console.WriteLine("BILL");
                Console.WriteLine("Name : {0}", name);
                Console.WriteLine("Contact No. : {0}", number);
                Console.WriteLine("E-mail : {0}", email);
                cmd = new MySqlCommand("select * from bill", con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("{0}         {1}         {2}", reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2));
                }
                reader.Close();
                cmd = new MySqlCommand("SELECT SUM(price) FROM bill", con);
                reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine("Grand Total : {0}", reader.GetInt32(0));
                }
                reader.Close();

            }
            catch (Exception e) { Console.WriteLine(e.Message); }
            cmd = new MySqlCommand("drop table bill", con);
            cmd.ExecuteNonQuery();
            Console.Read();
        }
    }
    //class generatebill : Guestpanel
    //{
    //    static string connection = "SERVER = 127.0.0.1;PORT = 3306; DATABASE = stockmanagement; UID = root";
    //    static MySqlConnection con = new MySqlConnection(connection);
    //    public void finalbill()
    //    {

    //        MySqlCommand cmd;
    //        MySqlDataReader reader;
    //        try
    //        {
    //            con.Open();
    //            Console.WriteLine("Name : {0}", name);

    //        }
    //        catch (Exception e) { Console.WriteLine(e.Message); }
    //        cmd = new MySqlCommand("drop table bill", con);
    //        cmd.ExecuteNonQuery();
    //        Console.Read();
    //    }
    //}
    //class bill : Guestpanel
    //{
    //    static string connection = "SERVER = 127.0.0.1;PORT = 3306; DATABASE = stockmanagement; UID = root";
    //    static MySqlConnection con = new MySqlConnection(connection);
    //    MySqlCommand cmd;
    //    MySqlDataReader reader;
    //    public int final_price;
    //    public void adddata()
    //    {
    //        con.Open();
    //        cmd = new MySqlCommand("insert into bill values(@n , @q, @p)", con);
    //        cmd.Parameters.AddWithValue("@n", product_name);
    //        cmd.Parameters.AddWithValue("@q", product_Quan);
    //        final_price = product_Quan * price;
    //        cmd.Parameters.AddWithValue("@p", final_price);
    //        cmd.ExecuteNonQuery();
    //        Console.WriteLine("Products Buyed");
    //        cmd = new MySqlCommand("select * from bill", con);
    //        reader = cmd.ExecuteReader();
    //        while (reader.Read())
    //        {
    //            Console.WriteLine("{0}         {1}         {2}", reader.GetString(0), reader.GetInt32(1), reader.GetInt32(2));
    //        }
    //        reader.Close();
    //    }
    //}
    class IndexedNames
    {
        private string[] namelist = new string[size];
        static public int size = 3;

        public IndexedNames()
        {
            for (int i = 0; i < size; i++)
                namelist[i] = "N. A.";
        }
        public string this[int index]
        {
            get
            {
                string tmp;

                if (index >= 0 && index <= size - 1)
                {
                    tmp = namelist[index];
                }
                else
                {
                    tmp = "";
                }

                return (tmp);
            }
            set
            {
                if (index >= 0 && index <= size - 1)
                {
                    namelist[index] = value;
                }
            }
        }
    }
        class Program
    {
        static string connection = "SERVER = 127.0.0.1;PORT = 3306; DATABASE = stockmanagement; UID = root";
        static MySqlConnection con = new MySqlConnection(connection);
         static void Main(string[] args)
        {
                
                    Program p = new Program();
            con.Open();
            p.stockmanagement();
            
        }
        
        public void stockmanagement()
        {
            int sh;
            try
            {



                IndexedNames names = new IndexedNames();
                names[0] = "3";
                names[1] = "2";
                names[2] = "1";
                for (int i = 0; i < IndexedNames.size; i++)
                {
                    Console.WriteLine("\t"+names[i]);
                    Thread.Sleep(1000);
                    Console.Clear();
                }
                Console.WriteLine("\tLets Start");
                Thread.Sleep(1000);
                Console.Clear();
                Console.WriteLine("\tWelcome to Stock Management Systen");
                Thread.Sleep(500);
                Console.WriteLine("\t1-Admin ");
                Thread.Sleep(500);
                Console.WriteLine("\t2-Guest");
                Thread.Sleep(500);
                sh = int.Parse(Console.ReadLine());
                if (sh == 1)
                {
                    Adminpanel ap = new Adminpanel();
                    ap.display();
                }
                else if (sh == 2)
                {
                    Guestpanel gp = new Guestpanel();
                    gp.display1();
                }
                Console.Read();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        
    }
}
