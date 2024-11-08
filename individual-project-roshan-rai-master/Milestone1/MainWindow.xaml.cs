using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Enumeration;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json.Nodes;
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
using System.Xml.Linq;
using Newtonsoft.Json;
using static System.Formats.Asn1.AsnWriter;

namespace Milestone1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Make> makeList = new List<Make>();//to add the make items from this class to make class
        List<string> ownerList = new List<string>(); //To add the ownerlist items from this class to ownerlist class
        List<Car> carList = new List<Car>();
        string[] colors = { "Black","Blue","Brown","Gray","Green","Grey","Maroon","Orange","Pink","Purple","Red","Silver","White","Yellow"};
        string[] types = {"Convertible","Coupe","Hatchback","Sedan","SUV","Truck","Van"};
        public MainWindow()
        {
            InitializeComponent();
            fileTypeComboBox.Items.Add("JSON");
            fileTypeComboBox.Items.Add("CSV");

            ExportButton.IsEnabled = false;
            fileTypeComboBox.IsEnabled = false;

            OutputComboBox.Items.Add("Clean Data");
            OutputComboBox.Items.Add("Owners");
            OutputComboBox.Items.Add("Makes");
            OutputComboBox.IsEnabled= false;
        }

        private void FileButton_Click(object sender, RoutedEventArgs e)
        {
            //path to users downloads directory
            string path = Environment.GetEnvironmentVariable("USERPROFILE") + @"\Downloads";
            // Configure open file dialog box
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            //properties of dlg are initialDiretry, defaultExt, Filter
            dlg.InitialDirectory = path;
            dlg.DefaultExt = ".csv"; // Default file extension
            dlg.Filter = "Excel file (.csv)|*.csv"; // Filter files by extension

            // Show open file dialog box/file explorer
            var result = dlg.ShowDialog();

            if (result == true)
            {
                CleanAndLoadData(dlg.FileName);
            }
            fileTypeComboBox.IsEnabled = true;
            
        }

        private void CleanAndLoadData(string fileName)
        {
//VIN	Year	Make	Model	Owners	Color	Type
//1GD02ZCG9BF078327	2004	Volkswagen|Oliver Blume|1937|https://www.vw.com/ R32 Roshan;2022/12/23|Hari;2022/11/30;2023/08/09	Silver	Hatchback
            string[] allLines= File.ReadAllLines(fileName).Skip(1).ToArray();
            foreach (string line in allLines) 
            {
                string[] lineComponents = line.Split(',');
                string vin = lineComponents[0];
                string year = lineComponents[1];
                string[] makeComponent = lineComponents[2].Split('|');
                //Volkswagen|Oliver Blume|1937|https://www.vw.com/
                string makeName = makeComponent[0];
                string makeCEO= makeComponent[1];
                string makeFoundingYear = makeComponent[2];
                string makeLink= makeComponent[3];

                Make make = new Make(makeName,makeCEO,makeFoundingYear,makeLink);
                //doesnt contain then add to makeList
                if (!makeList.Any(m => m.Name == make.Name))
                {
                    makeList.Add(make);
                }

                string model = lineComponents[3];
                string color = lineComponents[5];
                string rightColor = CheckSpelling(color, colors);
                string type = lineComponents[6];
                string rightType = CheckSpelling(type, types);

                Car car = new Car(vin, year, make, model, rightColor, rightType);
                //Essy Kelsell;1/17/1997|Jemiah Byneth;1/17/1997
                string[] ownerComponents = lineComponents[4].Split('|');
                Owner owner = null;
                foreach (string multipleOwner in ownerComponents)
                { 
                    string[] oneOwner = multipleOwner.Split(";");
                    //Coretta Pomfret;3/4/2004
                    string ownerName = oneOwner[0];

                    DateTime ownerPurchaseDate = DateTime.Parse(oneOwner[1]);
                    
                    try
                    {
                        DateTime ownerSaleDate = DateTime.Parse(oneOwner[2]);
                        owner = new Owner(ownerName, ownerPurchaseDate, ownerSaleDate);
                    }
                    catch (Exception)
                    {
                        owner = new Owner(ownerName, ownerPurchaseDate);
                    }
                    if (!ownerList.Contains(owner.Name))//adding unique owner to list to output it.
                    {
                        ownerList.Add(owner.Name);
                    }
                    car.Owners.Add(owner);
                   
                }
                carList.Add(car);
            }

            foreach (Car car in carList)
            {
                CarListBox.Items.Add(car);
            }
            //alphabetical order ray
            makeList.Sort();
            ownerList.Sort();

        }
        
        private string CheckSpelling(string wordToCheck, string[] arrayToCheckFrom)
        {
            if (arrayToCheckFrom.Contains(wordToCheck))
            {
                return wordToCheck;
            }
            else
            {
                var rightWord=FuzzySharp.Process.ExtractOne(wordToCheck, arrayToCheckFrom);
                return rightWord.Value;
            }
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {   
            //check if file type is selected in the combo box if so then export in json or csv whatever is selected
                Output(fileTypeComboBox.SelectedItem.ToString(), OutputComboBox.SelectedItem.ToString());           
        }

        public void Output(string fileType, string choosenOutput)
        {
            switch (choosenOutput)
            {
                case "Clean Data":
                    {
                        if (fileType == "JSON")
                        {
                            ExportJSONData(carList, "CleanData.json");
                        }
                        else
                        {
                            StringBuilder csvContent = new StringBuilder();
                            //directoryPath FileName
                            //string filePath = "C:\\Users\\Roshan Rai\\source\\repos\\individual-project-roshan-rai\\Milestone1\\CSV Files\\cleanData.csv";
                            string filePath = "CleanData.csv";
                            string csvLine = "VIN,Year,Make,Model,Owner/s,Color,Type\n";
                            csvContent.Append(csvLine);
                            foreach (Car car in carList)
                            {
                                if (car.Owners.Count == 1)//multiple owners no
                                {
                                    csvLine = $"{car.VIN},{car.Year},{car.Make},{car.Model},{car.Owners[0]},{car.Color},{car.Type}";
                                }
                                else
                                {
                                    string ownersFormatted = "";
                                    foreach (Owner owner in car.Owners)
                                    {
                                        ownersFormatted += owner.Name+" | ";
                                    }
                                    csvLine = $"{car.VIN},{car.Year},{car.Make},{car.Model},{ownersFormatted.Trim(' ').Trim('|')},{car.Color},{car.Type}";
                                }
                                // Append the CSV line to the content
                                csvContent.AppendLine(csvLine);
                            }
                            // Write the CSV string to a text file
                            
                            File.WriteAllText(filePath, csvContent.ToString());
                            MessageBox.Show("Exported Successfully. Find the CleanData.csv in the CSV Files folder.");
                            ExportButton.IsEnabled = false;

                        }
                           }
                    break;

                case "Owners":
                    {
                        if (fileType == "JSON")
                        {
                            ExportJSONData(ownerList, "Owners.json");

                        }
                        else
                        {
                            StringBuilder csvContent = new StringBuilder();
                            //directoryPath FileName
                            //string filePath = "C:\\Users\\Roshan Rai\\source\\repos\\individual-project-roshan-rai\\Milestone1\\CSV Files\\OwnerList.csv";
                            string filePath = "OwnerList.csv";
                            string csvLine = "Owner Name\n";
                            csvContent.Append(csvLine);
                            foreach (var ownerName in ownerList)
                            {
                                    csvLine = $"{ownerName}";
                                // Append the CSV line to the content
                                csvContent.AppendLine(csvLine);
                            }
                            // Write the CSV string to a text file
                            File.WriteAllText(filePath, csvContent.ToString());
                            MessageBox.Show("Exported Successfully. Find the OwnerList.csv in the CSV Files folder.");
                            ExportButton.IsEnabled = false;
                        }
                    }
                    break;

                case "Makes":
                    {
                        if (fileType == "JSON")
                        {
                            ExportJSONData(makeList, "Make.json");
                        }
                        else
                        {
                            StringBuilder csvContent = new StringBuilder();
                            //directoryPath FileName
                            // Specify the file path where you want to save the JSON
                            //string filePath = "C:\\Users\\Roshan Rai\\source\\repos\\individual-project-roshan-rai\\Milestone1\\CSV Files\\Make.csv";
                            string filePath = "Make.csv";
                            string csvLine = "Name,CEO,Founding Year,Website\n";
                            csvContent.Append(csvLine);
                            foreach (var make in makeList)
                            {
                                // We need to format each car's data into a CSV line
                                // For example, if Car has properties like Make, Color, Owners, etc.
                                csvLine = $"{make.Name},{make.CEO},{make.FoundingYear},{make.Website}";
                                // Append the CSV line to the content
                                csvContent.AppendLine(csvLine);
                            }
                            File.WriteAllText(filePath, csvContent.ToString());
                            MessageBox.Show("Exported Successfully. Find the Make.csv in the CSV Files folder.");
                            ExportButton.IsEnabled = false;
                        }
                    }
                    break;
            }
        }

        private void fileTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (fileTypeComboBox.SelectedItem != null)
            {
                OutputComboBox.IsEnabled = true;
            }

        }

        private void OutputComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (OutputComboBox.SelectedItem != null)
            {
                ExportButton.IsEnabled = true;
            }
        }
        private void ExportJSONData<T>(List<T> dataList, string fileName)
        {
            string jsonData = JsonConvert.SerializeObject(dataList, Formatting.Indented);
            string filePath = fileName;
            //string filePath = "C:\\Users\\Roshan Rai\\source\\repos\\individual-project-roshan-rai\\Milestone1\\JSON Files\\" + fileName;
            File.WriteAllText(filePath, jsonData);
            MessageBox.Show($"Exported Successfully. Find the {fileName} in the JSON Files folder.");
        }

      

    }
}
