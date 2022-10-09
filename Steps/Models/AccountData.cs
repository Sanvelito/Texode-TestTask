using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.Win32;

namespace Steps.Models
{
    internal static class AccountData
    {
        public static List<List<Account>> GetAllUsers()
        {
            string[] fileName = Directory.GetFiles(@"Days");
            if (Directory.Exists(@"Days"))
            {
                List<List<Account>> days = new List<List<Account>>();
                foreach(string n in fileName)
                {
                    List<Account> accounts = JsonConvert.DeserializeObject<List<Account>>(File.ReadAllText(n));
                    days.Add(accounts);
                    
                }
                return days;
            }
            return null;
        }
        public static List<Account> GetAccount(string name)
        {
            return GetAllUsers().Select(day => day.FirstOrDefault(acc => acc.User == name)).Where(day => day != null).ToList();
        }
        public static double AverageSteps(List<Account> accounts)
        {
            if(accounts.Count == 0)
            {
                return 0;
            }
            return Math.Round(accounts.Average(acc => acc.Steps),2);
        }
        public static double MaximumSteps(List<Account> accounts)
        {
            if (accounts.Count == 0)
            {
                return 0;
            }
            return accounts.Max(acc => acc.Steps);
        }
        public static double MinimumSteps(List<Account> accounts)
        {
            if (accounts.Count == 0)
            {
                return 0;
            }
            return accounts.Min(acc => acc.Steps);
        }
        public static string UnderrateSteps(Account accounts)
        {

            if (accounts.Maximum > accounts.Average + accounts.Average * 0.2)
            {
                return "yes";
            }
            else if(accounts.Minimum < accounts.Average - accounts.Average * 0.2)
            {
                return "yes";
            }
            else
                return "no";
        }
        public static List<Account> GetActiveUsers()
        {
            List<Account> activeAccounts = new List<Account>();
            activeAccounts = GetAllUsers().First();
            
            for (int i = 0; i < activeAccounts.Count; i++)
            {
                activeAccounts[i].Average = AverageSteps(GetAccount(activeAccounts[i].User));
                activeAccounts[i].Maximum = MaximumSteps(GetAccount(activeAccounts[i].User));
                activeAccounts[i].Minimum = MinimumSteps(GetAccount(activeAccounts[i].User));
                activeAccounts[i].Underrate = UnderrateSteps(activeAccounts[i]);
            }
            return activeAccounts;
        }
        public static List<int> GetAllDaySteps(Account account)
        {
            List<int> daySteps = new List<int>();
            List<Account> selectedAccount = new List<Account>();
            if (account == null)
            {
                return null;
            }
            else
            {
                selectedAccount = GetAccount(account.User);
            
                for (int i = 0; i < selectedAccount.Count; i++)
                {
                    daySteps.Add(selectedAccount[i].Steps);
                }
                return daySteps;
            }

        }
        public static void SaveUserStats(Account account)
        {
            List<Account> selectedAccount = new List<Account>();

            selectedAccount = GetAccount(account.User);
            for(int i = 0; i < selectedAccount.Count; i++)
            {
                selectedAccount[i].Average = AverageSteps(selectedAccount);
                selectedAccount[i].Maximum = MaximumSteps(selectedAccount);
                selectedAccount[i].Minimum = MinimumSteps(selectedAccount);
                
            }
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == true)
            {
                
                using (StreamWriter file = File.CreateText(saveFileDialog.FileName))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, selectedAccount);
                }
            }
        }
    }
}
