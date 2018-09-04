using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

namespace Ecryption
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string path = @"C:\Users\HugoZ\Documents\Hugo.txt";

        public MainWindow()
        {
            InitializeComponent();
        }

        string hash = "f0xle@rn";

        private void Btn_Encrypt_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = UTF8Encoding.UTF8.GetBytes(txtValue.Text);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() {
                    Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    txtEncrypt.Text = Convert.ToBase64String(results, 0, results.Length);
                }
            }
        }

        private void Btn_Decrypt_Click(object sender, RoutedEventArgs e)
        {
            byte[] data = Convert.FromBase64String(txtEncrypt.Text);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hash));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider()
                {
                    Key = keys,
                    Mode = CipherMode.ECB,
                    Padding = PaddingMode.PKCS7
                })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    txtDecrypt.Text = UTF8Encoding.UTF8.GetString(results);
                }
            }
        }

        private void Import_File_Click(object sender, RoutedEventArgs e)
        {
            txtValue.Text = File.ReadAllText(@"C:\Users\HugoZ\Documents\Hugo.txt");
        }

        private void Save_File_Click(object sender, RoutedEventArgs e)
        {
            string text = txtEncrypt.Text; // text to encrypt
            string name = location.Text;
            // Create a New fileDialog (this allows you to set the save location Manually
            SaveFileDialog saveFileDialog = new SaveFileDialog(); 
            saveFileDialog.DefaultExt = ".txt";
            saveFileDialog.AddExtension = true;
            //If the user clicks on SAVE
            if (saveFileDialog.ShowDialog()==true)
            {
                //get the filename
                string myFileName = saveFileDialog.FileName;
                // write to file
                File.WriteAllText(myFileName, text);
            }

            //try
            //{
            //    TextWriter txtwr = new StreamWriter(@"C:\Users\HugoZ\Documents\" + name + ".txt", true);
            //}
            //catch (IOException)
            //{

            //}

            ////File.Create(name + ".txt");
            ////Create(@"C:\Users\HugoZ\Documents" + name + ".txt");
            //File.WriteAllText(@"C:\Users\HugoZ\Documents\" + name + ".txt", text);
        }
    }
}
