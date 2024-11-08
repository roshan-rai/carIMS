using Intersoft.Crosslight;
using Milestone2.SecondApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Milestone2.SecondApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Milestone2SecondAppDBContext db= new Milestone2SecondAppDBContext();
        private bool isTextChanged = false;

        public MainWindow()
        {
            InitializeComponent();
            PopulateListBoxCar();
            PopulateListBoxOwner();
            PopulateMakeListBox();
            AddMakeButton.IsEnabled = false;
            SaveMakeButton.IsEnabled = false;
            DeleteMakeButton.IsEnabled= false;
            EditMakeButton.IsEnabled = false;
            EditButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            SaveButton.IsEnabled = false;
            AddButton.IsEnabled = true;
            MakeComboBox.Visibility= Visibility.Collapsed;
            VinInput.Visibility = Visibility.Collapsed;
            CreateButton.Visibility = Visibility.Collapsed;
            SaveOwnerButton.IsEnabled= false;
            EditOwnerButton.IsEnabled = false;
        }

        private void PopulateMakeListBox()
        {
            MakeListBox.Items.Clear();
            foreach (Make make in db.Makes)
            {
                MakeListBox.Items.Add(make);
            }
            AddMakeButton.IsEnabled = false;
            SaveMakeButton.IsEnabled = false;
            DeleteMakeButton.IsEnabled = false;
            EditMakeButton.IsEnabled = false;
        }

        public void PopulateListBoxCar()
        { 
            listBoxCars.Items.Clear();
            foreach (Car car in db.Cars) 
            { 
                listBoxCars.Items.Add(car);
            }

        }
        public void PopulateListBoxOwner()
        {
            listBoxOwners.Items.Clear();
            foreach (Owner owner in db.Owners)
            {
                listBoxOwners.Items.Add(owner);
            }
        }
        public void EditButton_Click(object sender, RoutedEventArgs e)
        {
            VinLabelInput.Content = string.Empty;
            MakeLabel.Content=string.Empty;

            var selectedItem = (Car)listBoxCars.SelectedItem;
            MakeComboBox.Visibility = Visibility.Collapsed;
            VinInput.Visibility = Visibility.Collapsed;
            CreateButton.Visibility = Visibility.Collapsed;
            if (selectedItem != null)
            {
                ColorTB.Text = selectedItem.Color;
                TypeTB.Text = selectedItem.Type;
                YearTB.Text = selectedItem.Year;
                ModelTB.Text = selectedItem.Model;
                VinLabel.Content = "You are editing :" + selectedItem.Vin;
            }
            else 
            {
                MessageBox.Show("Please select the car you want to edit.");
            }
        }

        private void listBoxCars_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxCars.SelectedItem != null)
            {
                EditButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string color = ColorTB.Text;
            string type = TypeTB.Text;
            string model= ModelTB.Text;
            string year = YearTB.Text;

            
            Car selectedCar = (Car)listBoxCars.SelectedItem;
            string vin=selectedCar.Vin;
            Car car = db.Cars.Find(vin); //using vin to find car
            car.Year = year;
            car.Model= model;
            car.Type = type;
            car.Color= color;

            db.SaveChanges();
            MessageBox.Show("Changes saved successfully !!!");
            VinLabel.Content = string.Empty;
            PopulateListBoxCar();
            ClearTextBox();
        }

        private void TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            isTextChanged = (ColorTB.Text.Trim() != "" || YearTB.Text.Trim() != "" || TypeTB.Text.Trim() != "" || ModelTB.Text.Trim() != "") && listBoxCars.SelectedItem != null && EditButton.IsEnabled;

            SaveButton.IsEnabled = isTextChanged;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem =listBoxCars.SelectedItem;
            if (selectedItem!=null)
            {
                Car car = (Car)selectedItem;
                List<CarOwnership> carOwnershipThatMatchVIN = db.CarOwnerships.Where(co=>co.Vin==car.Vin).ToList();
                if (carOwnershipThatMatchVIN.Count()>0)//yedi car chai kosailay own garcha bhane chai o bhanda dherai
                {
                    foreach (CarOwnership co in carOwnershipThatMatchVIN)
                    {
                        db.CarOwnerships.Remove(co);
                        db.SaveChanges();
                    }
                }
                db.Cars.Remove(car);
                db.SaveChanges();
                MessageBox.Show($"{car.Vin} has been deleted successfully.");
                PopulateListBoxCar();
            }

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)//to add the car
        {
            //when add button is there edit must be turned off
            VinLabel.Content = string.Empty;
            MakeLabel.Content = "Select Make";//label
            MakeComboBox.Visibility = Visibility.Visible;
            foreach(Make make in db.Makes)
            {
                MakeComboBox.Items.Add(make);
            }
            VinInput.Visibility = Visibility.Visible;
            VinLabelInput.Content = "Enter Vin";//label
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (MakeComboBox.SelectedItem!=null && ColorTB.Text.Trim()!=""&& ModelTB.Text.Trim() != ""&& TypeTB.Text.Trim() != "" && YearTB.Text.Trim() != "" && VinInput.Text.Trim()!=null)
            {
                Car car = new Car();
                car.Year=YearTB.Text;
                car.Model=ModelTB.Text;
                car.Color=ColorTB.Text;
                car.Vin=VinInput.Text;
                //check if that VIN already exist in DB
                if (db.Cars.Find(car.Vin) != null)
                {
                    MessageBox.Show("We already have car with that VIN.");
                    return;
                }
                car.Type=TypeTB.Text;
                Make selectedMake = (Make)MakeComboBox.SelectedItem;
                car.MakeId= selectedMake.MakeId;
                db.Cars.Add(car);
                db.SaveChanges();
                MessageBox.Show("Created successfully !!!");
                PopulateListBoxCar();
                ClearTextBox();
            }
        }

        private void VinInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            CreateButton.Visibility = Visibility.Visible;
            AddButton.IsEnabled = false;
        }
        private void ClearTextBox()
        { 
            TypeTB.Text = string.Empty;
            ColorTB.Text = string.Empty;
            ModelTB.Text = string.Empty;
            YearTB.Text = string.Empty;
            if(VinInput.Visibility==Visibility.Visible)
            { 
                VinInput.Text=string.Empty;
            }
            if (MakeComboBox.Visibility==Visibility.Visible)
            {
                MakeComboBox.SelectedItem = null;
            }
        }

        private void MakeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AddButton.IsEnabled = false;
        }

        private void EditOwnerButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (Owner)listBoxOwners.SelectedItem;
            SaveOwnerButton.IsEnabled = true;
            if (selectedItem != null)
            {
                OwnerNameTB.Text = selectedItem.OwnerName;
            }
            else
            {
                MessageBox.Show("Please select the owner you want to edit.");
            }

        }

        private void SaveOwnerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(OwnerNameTB.Text))
            {
                var selectedItem = (Owner)listBoxOwners.SelectedItem;
                selectedItem.OwnerName = OwnerNameTB.Text;
                db.SaveChanges();
                MessageBox.Show("Changes saved successfully !!!");
                PopulateListBoxOwner();
                OwnerNameTB.Clear();
            }
            else
            {
                MessageBox.Show("Please enter a valid input in the owner name.");
            }
        }

        private void AddOwnerButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(OwnerNameTB.Text) && listBoxOwners.SelectedItem == null)
            {
                if (db.Owners.Where(o => o.OwnerName.ToLower() == OwnerNameTB.Text.Trim().ToLower()).ToList().Count() == 0)
                {
                    Owner o = new Owner();
                    o.OwnerName = OwnerNameTB.Text;
                    db.Owners.Add(o);
                    db.SaveChanges();
                    MessageBox.Show("Added successfully !!!");
                    OwnerNameTB.Clear();
                    PopulateListBoxOwner();
                }
                else 
                {
                    MessageBox.Show("This owner name already exists bruh.");
                }
            }
            else
            {
                MessageBox.Show("You have selected an item or the owner name is empty.");
            }

        }

        private void listBoxOwners_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           EditOwnerButton.IsEnabled = true;
        }

        private void DeleteOwnerButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxOwners.SelectedItem != null)
            {
                var selectedItem = (Owner)listBoxOwners.SelectedItem;
                var carOwnershipsToDelete = db.CarOwnerships.Where(x => x.OwnerId == selectedItem.OwnerId).ToList();
                db.CarOwnerships.RemoveRange(carOwnershipsToDelete);
                db.Owners.Remove(selectedItem);
                db.SaveChanges();
                PopulateListBoxOwner();
                MessageBox.Show("Owner deleted succesfully !!!");
            }
            else
            {
                MessageBox.Show("Please select an owner that you wanna delete.");
            }
        }

        private void AssignCarButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxOwners.SelectedItem != null && listBoxCars.SelectedItem != null)
            {
                Owner selectedOwner = (Owner)listBoxOwners.SelectedItem;
                int ownerIDTBA = selectedOwner.OwnerId;
                Car selectedCar = (Car)listBoxCars.SelectedItem;
                string VINTBC = selectedCar.Vin;
                CarOwnership[] carOwnerships = db.CarOwnerships.Where(co => co.Vin == VINTBC).ToArray();//car kosailay paila own garcha ki gardaina
                if (carOwnerships.Length == 0)//sale bhayeko chaina bhane bhitra jau
                {
                    CarOwnership co = new CarOwnership();
                    co.Vin = VINTBC;
                    co.OwnerId = ownerIDTBA;
                    co.PurchaseDate = DateTime.Now;
                    co.SaleDate = null;
                    db.CarOwnerships.Add(co);
                }

                else
                {   //if multiple carownership meaning that many times car purchased and sold then get the one that isn't sold yet with sale date null
                    CarOwnership coNullSaleDate = carOwnerships.Where(co => co.SaleDate == null).FirstOrDefault();//null sale date bhako chai deu
                    coNullSaleDate.SaleDate = DateTime.Now;
                    CarOwnership newBuyer = new CarOwnership();//naya banayeko kina bhane naya owner ayo tei bhauera conullsaleDate chai purano owner
                    newBuyer.OwnerId = ownerIDTBA;
                    newBuyer.PurchaseDate = DateTime.Now;
                    newBuyer.Vin = VINTBC;
                    db.CarOwnerships.Add(newBuyer);
                }
                db.SaveChanges();
                MessageBox.Show($"Car with VIN {VINTBC} assigned a new owner with ownerID of {ownerIDTBA}");
            }
            else
            {
                MessageBox.Show("Please select both car and owner.");
            }
        }

        private void AddMakeButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(BrandNameTB.Text) || string.IsNullOrEmpty(CEOTB.Text) || string.IsNullOrEmpty(LinkTB.Text) || string.IsNullOrEmpty(FoundingYearTB.Text))
            {
                MessageBox.Show("Please fill up brand name, CEO, year and website link.");
            }
            else 
            {
                string BrandName=BrandNameTB.Text;
                string Ceoname=CEOTB.Text;
                string Link=LinkTB.Text;
                string FoundingYear=FoundingYearTB.Text;

                Make make = new Make();
                make.BrandName = BrandName;
                make.Ceoname = Ceoname;
                make.WebsiteLink = Link;
                make.FoundingYear= FoundingYear;
                db.Makes.Add(make);
                db.SaveChanges();
                MessageBox.Show($"{make.BrandName} added successfully.");
                BrandNameTB.Clear();
                CEOTB.Clear();
                LinkTB.Clear();
                FoundingYearTB.Clear();
                PopulateMakeListBox();
            }
        }
        private void MakeTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            isTextChanged = (BrandNameTB.Text.Trim() != "" || CEOTB.Text.Trim() != "" || LinkTB.Text.Trim() != "" || FoundingYearTB.Text.Trim() != "");
            AddMakeButton.IsEnabled = isTextChanged;
        }

        private void MakeListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //when makelistbox select huncha taba chai enable gara
            if (MakeListBox.SelectedItem != null)
            {
                DeleteMakeButton.IsEnabled = true;
                EditMakeButton.IsEnabled = true;
            }

        }

        private void DeleteMakeButton_Click(object sender, RoutedEventArgs e)
        {
            if (MakeListBox.SelectedItem!=null)
            {
                Make makeToDelete = (Make)MakeListBox.SelectedItem;
                var carsToDelete = db.Cars.Where(x => x.MakeId == makeToDelete.MakeId).ToList();
                //co delete
                foreach (Car item in carsToDelete)
                {
                    var carOwnershipsToDelete = db.CarOwnerships.Where(x => x.Vin == item.Vin).ToList();
                    db.CarOwnerships.RemoveRange(carOwnershipsToDelete);

                }

                db.Cars.RemoveRange(carsToDelete);


                db.Makes.Remove(makeToDelete);
                db.SaveChanges();
                MessageBox.Show($"{makeToDelete.BrandName} has been deleted successfully.");
                PopulateMakeListBox();
                PopulateListBoxCar();
            }
        }

        private void EditMakeButton_Click(object sender, RoutedEventArgs e)
        {
            if (MakeListBox.SelectedItem != null)
            {
                Make makeToEdit = (Make)MakeListBox.SelectedItem;
                BrandNameTB.Text = makeToEdit.BrandName;
                CEOTB.Text = makeToEdit.Ceoname;
                LinkTB.Text = makeToEdit.WebsiteLink;
                FoundingYearTB.Text = makeToEdit.FoundingYear;
                SaveMakeButton.IsEnabled = true;

            }
            
        }

        private void SaveMakeButton_Click(object sender, RoutedEventArgs e)
        {
            if (MakeListBox.SelectedItem != null)
            {
                Make makeToEdit = (Make)MakeListBox.SelectedItem;
                int idToEdit = makeToEdit.MakeId;

                Make makeToSave = db.Makes.Find(idToEdit);
                makeToSave.BrandName=BrandNameTB.Text;
                makeToSave.Ceoname=CEOTB.Text ;
                makeToSave.WebsiteLink=LinkTB.Text;
                makeToSave.FoundingYear=FoundingYearTB.Text;
                db.SaveChanges();
                MessageBox.Show($"{makeToSave.BrandName} has been edited successfully.");
                BrandNameTB.Clear();
                CEOTB.Clear();
                LinkTB.Clear();
                FoundingYearTB.Clear();
                PopulateMakeListBox();
            }
        }

        private void ShowDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxCars.SelectedItem != null)
            {
                CarDetails c= new CarDetails();
                c.Show();
                c.CarDetailsListBox.Items.Clear();

                Car selectedCar= (Car)listBoxCars.SelectedItem;
                List<CarOwnership> carOwnerships= db.CarOwnerships.Where(c => c.Vin == selectedCar.Vin).ToList();
                foreach (var item in carOwnerships)
                {
                    c.CarDetailsListBox.Items.Add(item);

                }

            }
            else { MessageBox.Show("Please select the car to see the details.");
            }
        }
    }
}
