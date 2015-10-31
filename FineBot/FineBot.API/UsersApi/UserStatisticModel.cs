using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FineBot.API.UsersApi {
    public class UserStatisticModel {
        public int[] DisplayableFinesForYear
        {
            get
            {
                int[] values = {0,0,0,0,0,0,0,0,0,0,0,0};
                foreach (MonthlyGraphModel graphModel in FinesMonthlyModels)
                {
                    values[graphModel.MonthIndex] = graphModel.Count;
                }

                return values;
            }
        }

        public int[] DisplayablePaymentsForYear
        {
            get
            {
                int[] values = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                foreach (MonthlyGraphModel graphModel in PaymentsMonthlyModels) {
                    values[graphModel.MonthIndex] = graphModel.Count;
                }

                return values;
            }
        }

        public int TotalFinesEver { get; set; }
        public int TotalPaymentsEver { get; set; }
        public int TotalFinesForMonth { get; set; }
        public int TotalPaymentsForMonth { get; set; }
        public string UserDisplayName { get; set; }

        public List<MonthlyGraphModel> PaymentsMonthlyModels { get; set; }
        public List<MonthlyGraphModel> FinesMonthlyModels { get; set; }
    }

    public class MonthlyGraphModel
    {
        public int MonthIndex { get; set; }
        public int Count { get; set; }
    }
}
