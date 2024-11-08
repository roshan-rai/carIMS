using Microsoft.Data.SqlClient;
using Milestone2.FirstApplication.JsonInputClasses;
using Milestone2.FirstApplication.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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

namespace Milestone2.FirstApplication
{
    /// <summary>
    /// </summary>
    public partial class MainWindow : Window
    {
        Milestone2DBContext db = new Milestone2DBContext();
        bool loadCarOwnership;
        bool loadMakeData;
        bool loadCarData;
        bool loadOwnerData;
         public MainWindow()
        {
            InitializeComponent();
            LoadDataToDataBase.IsEnabled = false;
        }
   

            private void ReadAndLoadCarOwnershipData(bool loadDataToDb)
        {
            if (loadDataToDb==false) return;
            string fileContents = File.ReadAllText("JsonFilesFromMilestone1\\CleanData.json");
            List<CarOwnerInfoJson> carInfos = JsonConvert.DeserializeObject<List<CarOwnerInfoJson>>(fileContents);//banayeko class ko obj ma rakheko

            foreach (CarOwnerInfoJson carInfo in carInfos)
            {
                CarOwnership carOwnershipObj = new CarOwnership();
                carOwnershipObj.Vin = carInfo.VIN;
                foreach (var owner in carInfo.Owners)
                {
                    carOwnershipObj.OwnerId=db.Owners
                                            .Where(x => x.OwnerName == owner.Name)//hamrow db ma naam repeat chaina teibhayera ownername check garya
                                            .Select(x => x.OwnerId)
                                            .FirstOrDefault();
                    carOwnershipObj.PurchaseDate = owner.PurchaseDate;
                    carOwnershipObj.SaleDate= owner.SaleDate;
                    if (carOwnershipObj.OwnershipId!=0)//if not 0 then give it zero
                    {
                        carOwnershipObj.OwnershipId = 0;
                    }
                    db.CarOwnerships.Add(carOwnershipObj);
                    db.SaveChanges();
                }   
            }
        }

        

        private void ReadAndLoadOwnerData(bool loadDataToDb)
        {
            if (loadDataToDb==false)
            {
                return;
            }
            string fileContents = File.ReadAllText("JsonFilesFromMilestone1\\Owners.json");
            List<string> ownerNames = JsonConvert.DeserializeObject<List<string>>(fileContents);
            foreach (string ownerName in ownerNames)
            {
                Owner owner = new Owner();
                owner.OwnerName= ownerName;
                db.Owners.Add(owner);
                db.SaveChanges();
            }
        }

        private void ReadAndLoadCarData(bool loadDataToDb)
        {
            if(loadDataToDb==false) { return; }
            string fileContents = File.ReadAllText("JsonFilesFromMilestone1\\CleanData.json");
            List<CarJson> carJsons = JsonConvert.DeserializeObject<List<CarJson>>(fileContents);//json lai obj convert garya
            foreach (CarJson carJson in carJsons)
            {
                Car car= new Car();
                car.Vin = carJson.VIN;
                car.Year = carJson.Year;
                car.Color= carJson.Color;
                car.Type = carJson.Type;
                car.Model= carJson.Model;
                //car.MakeId=db.Makes.Find(car.MakeId).MakeId;
                car.MakeId = db.Makes.Where(m=> m.BrandName==carJson.Make.Name).FirstOrDefault().MakeId;
                db.Cars.Add(car);
                db.SaveChanges();
            }
        }

        private void ReadAndLoadMakeData(bool loadDataToDb)
        {
            if (loadDataToDb==false)
            {
                return;
            }
            string fileContents = File.ReadAllText("JsonFilesFromMilestone1\\Make.json");
            List<MakeJson> makeObjects= JsonConvert.DeserializeObject<List<MakeJson>>(fileContents);
            foreach (MakeJson m in makeObjects)
            {
                Make make = new Make();
                make.BrandName = m.Name;
                make.Ceoname = m.CEO;
                make.FoundingYear = m.FoundingYear;
                make.WebsiteLink = m.Website;
                db.Makes.Add(make);
                db.SaveChanges();
            }

        }

      

        private void LoadDataToDataBase_Click(object sender, RoutedEventArgs e)
        {
            //true if you have not already added data else make it false
            MessageBox.Show("Working !!!");
            loadMakeData = true;
            loadCarData = true;
            loadOwnerData = true;
            loadCarOwnership = true;
            ReadAndLoadMakeData(loadMakeData);
            ReadAndLoadOwnerData(loadOwnerData);
            ReadAndLoadCarData(loadCarData);
            ReadAndLoadCarOwnershipData(loadCarOwnership);
            LoadDataToDataBase.IsEnabled = false;
            MessageBox.Show("Loaded successfully !!!");
        }

        private void CheckButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Working !!!");
            if (db.Makes.Count() >= 10 && db.Owners.Count() >= 10 && db.Cars.Count() >= 10 && db.CarOwnerships.Count() >= 10)
            {
                MessageBox.Show("Already added !!!");
            }
            else
            {
                MessageBox.Show("There is no data in the database.");
                LoadDataToDataBase.IsEnabled= true;
            }
        }

        private void DeleteAll_Click(object sender, RoutedEventArgs e)
        {
            var carOwnerships = db.CarOwnerships.ToList();
            db.CarOwnerships.RemoveRange(carOwnerships);
            db.SaveChanges();

            var cars = db.Cars.ToList();
            db.Cars.RemoveRange(cars);

            var makes = db.Makes.ToList();
            db.Makes.RemoveRange(makes);

            var owners = db.Owners.ToList();
            db.Owners.RemoveRange(owners);

            db.SaveChanges();
           

            LoadDataToDataBase.IsEnabled = true;

            MessageBox.Show("All data deleted from Cars, Owners, CarOwnerships, and Makes tables.");
        }
    }
}
