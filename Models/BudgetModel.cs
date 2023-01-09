using ProgiTest.Constants;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ProgiTest.Models
{
    public class BudgetModel : INotifyPropertyChanged
    {
        double? budget = 0.0;
        double? newBudget = 0.0;
        double? maximumVehiculeAmount = 0.0;
        double? basic = 0.0;
        double? special = 0.0;
        double? association = 0.0;
        double? storage = BudgetConstants.STORAGECONSTANT;
        public double? Budget
        { 
            get { return budget; } 
            set 
            { 
                budget = value; 
                OnPropertyChanged(); 
            } 
        }
        public double? NewBudget { get { return newBudget; } set { newBudget = value; OnPropertyChanged(); } }
        public double? MaximumVehiculeAmount { get { return maximumVehiculeAmount; } set { maximumVehiculeAmount = value; OnPropertyChanged(); } }
        public double? Basic { get { return basic;  } set { basic = value; OnPropertyChanged(); } }
        public double? Special { get { return special; } set { special = value; OnPropertyChanged(); } }
        public double? Association { get { return association; } set { association = value; OnPropertyChanged(); } }
        public double? Storage { get { return storage; } set { storage = value; OnPropertyChanged(); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

    }

    
}
