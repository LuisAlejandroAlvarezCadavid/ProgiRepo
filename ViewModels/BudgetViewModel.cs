using GalaSoft.MvvmLight.Command;
using ProgiTest.Models;
using System;
using System.Windows.Input;
using Windows.UI.Popups;
using ProgiTest.Constants;
using Windows.UI.Xaml;
using System.Threading.Tasks;

namespace ProgiTest.ViewModels
{
    public class BudgetViewModel
    {
        public ICommand BudGetCalculateCommand { get; set; }

        public BudgetModel BudgetModel { get; set; }

        public BudgetViewModel() 
        {
            BudgetModel = new BudgetModel();
            BudGetCalculateCommand = new RelayCommand(new Action( () => BudGetCalculate()));
        }

        private async void BudGetCalculate()
        {
            if (BudgetModel.Budget == null || BudgetModel.Budget.Value == 0) {await new MessageDialog("You must enter a valid Badget value").ShowAsync(); }
            if(BudgetModel.Storage == 0.0) BudgetModel.Storage = BudgetConstants.STORAGECONSTANT;
            CalculateCA();
            if (await CalculateMaximunCarPrice())
            {
                CalculateBasicValue();
                CalculateSpecialFee();
                CalculateNewBudget();
            }
            else
            {
                ClearAllValues();
            }
            
        }

        

        private void CalculateCA()
        {
            if (BudgetModel.Budget.Value >= 1 && BudgetModel.Budget.Value <= 500) 
            { 
                BudgetModel.Association = BudgetConstants.ASSOCIATION_5; 
                return; 
            }
            if(BudgetModel.Budget.Value > 500 && BudgetModel.Budget.Value <= 1000)
            {
                BudgetModel.Association = BudgetConstants.ASSOCIATION_10;
                return;
            }
            if (BudgetModel.Budget.Value > 1000 && BudgetModel.Budget.Value <= 3000)
            {
                BudgetModel.Association = BudgetConstants.ASSOCIATION_15;
                return;
            }
            if (BudgetModel.Budget.Value > 3000)
            {
                BudgetModel.Association = BudgetConstants.ASSOCIATION_20;
                return;
            }
        }



        private void CalculateCAWithVehiculePrice()
        {
            if(BudgetModel.MaximumVehiculeAmount.Value < 1)
            {
                BudgetModel.Association = BudgetConstants.ASSOCIATION_0;
                return;
            }
            if (BudgetModel.MaximumVehiculeAmount.Value >= 1 && BudgetModel.MaximumVehiculeAmount.Value <= 500)
            {
                BudgetModel.Association = BudgetConstants.ASSOCIATION_5;
                return;
            }
            if (BudgetModel.MaximumVehiculeAmount.Value > 500 && BudgetModel.MaximumVehiculeAmount.Value <= 1000)
            {
                BudgetModel.Association = BudgetConstants.ASSOCIATION_10;
                return;
            }
            if (BudgetModel.MaximumVehiculeAmount.Value > 1000 && BudgetModel.MaximumVehiculeAmount.Value <= 3000)
            {
                BudgetModel.Association = BudgetConstants.ASSOCIATION_15;
                return;
            }
            if (BudgetModel.MaximumVehiculeAmount.Value > 3000)
            {
                BudgetModel.Association = BudgetConstants.ASSOCIATION_20;
                return;
            }
        }

        private async Task<bool> CalculateMaximunCarPrice()
        {
            if ((BudgetModel.Budget - BudgetModel.Storage - BudgetConstants.BASICFEEMAMINIMUN) <= 0)
            {
                await new MessageDialog("You must enter a valid Budget").ShowAsync();
                return false;
            }
            BudgetModel.MaximumVehiculeAmount = Math.Round((BudgetModel.Budget - BudgetModel.Association - BudgetModel.Storage).Value / (1.12), 2);           
            return true;
        }

        private void CalculateBasicValue()
        {
            BudgetModel.Basic = Math.Round((BudgetModel.MaximumVehiculeAmount * BudgetConstants.BASICPERCENTAGE).Value, 2);
            if (BudgetModel.Basic < BudgetConstants.BASICFEEMAMINIMUN ) BudgetModel.Basic = BudgetConstants.BASICFEEMAMINIMUN;
            if (BudgetModel.Basic > BudgetConstants.BASICFEEMAXIMUN)
            {
                BudgetModel.MaximumVehiculeAmount += BudgetModel.Basic - BudgetConstants.BASICFEEMAXIMUN;
                BudgetModel.Basic = BudgetConstants.BASICFEEMAXIMUN;
            }
        }

        private void CalculateSpecialFee()
        {
            BudgetModel.Special = Math.Round(( BudgetModel.MaximumVehiculeAmount * BudgetConstants.SPECIALFEEPERCENTAGE ).Value , 2 );
        }

        private void CalculateNewBudget()
        {
            CalculateCAWithVehiculePrice();
            BudgetModel.NewBudget = BudgetModel.Association + BudgetModel.Storage + BudgetModel.MaximumVehiculeAmount + BudgetModel.Basic + BudgetModel.Special;
            if(BudgetModel.NewBudget > BudgetModel.Budget)
            {
                double? AjustCarValue = Math.Round((BudgetModel.NewBudget - BudgetModel.Budget).Value, 2);
                BudgetModel.MaximumVehiculeAmount -= AjustCarValue.Value;
                if (BudgetModel.MaximumVehiculeAmount < 0)
                {
                    BudgetModel.MaximumVehiculeAmount = 1.0 - 0.01;                    
                }
                CalculateSpecialFee();
                CalculateNewBudget();
            }
            if(BudgetModel.NewBudget < BudgetModel.Budget)
            {
                if (BudgetModel.MaximumVehiculeAmount < 1)
                {
                    BudgetModel.MaximumVehiculeAmount = 1.0;
                    CalculateCAWithVehiculePrice();
                    CalculateNewBudget();
                }
            }
            
        }

        private void ClearAllValues()
        {
            BudgetModel.NewBudget = 0.0;
            BudgetModel.Special = 0.0;            
            BudgetModel.Basic = 0.0;
            BudgetModel.Association = 0.0;
            BudgetModel.MaximumVehiculeAmount = 0.0;
            BudgetModel.Storage = 0.0;
        }
    }
}
