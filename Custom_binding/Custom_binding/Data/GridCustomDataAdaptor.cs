using Syncfusion.Blazor.Data;
using Syncfusion.Blazor;
using static Custom_binding.Pages.Index;
using System.Collections;

namespace Custom_binding.Data
{
    /// <summary>
    /// Custom data binding in DataGrid allows for manual control over the data displayed in the grid.
    /// With custom binding, the developer is responsible for providing the data each time it is requested by the grid
    /// </summary>
    public class GridCustomDataAdaptor : DataAdaptor
    {
        /// <summary>
        ///The custom data binding can be performed in the DataGrid component by providing the overriding
        ///the Read or ReadAsync method of the DataAdaptor abstract class.
        ///It is performs data Read operation, Apply the given criteria against the datasource and return the records.
        /// </summary>
        /// <param name="dataManagerRequest"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public override object? Read(DataManagerRequest dataManagerRequest, string? key = null)
        {
            IEnumerable? gridData = Orders;

            if (dataManagerRequest.Sorted?.Count > 0) // perform Sorting
            {
                gridData = DataOperations.PerformSorting(gridData, dataManagerRequest.Sorted);
            }

            if (dataManagerRequest.Where != null && dataManagerRequest.Where.Count > 0)// Filtering
            {
                gridData = DataOperations.PerformFiltering(gridData, dataManagerRequest.Where, dataManagerRequest.Where[0].Operator);
            }

            if (dataManagerRequest.Search != null && dataManagerRequest.Search.Count > 0)// Searching
            {
                gridData = DataOperations.PerformSearching(gridData, dataManagerRequest.Search);
            }

            if (dataManagerRequest.Skip != 0)
            {
                gridData = DataOperations.PerformSkip(gridData, dataManagerRequest.Skip); //Paging
            }

            if (dataManagerRequest.Take != 0)
            {
                gridData = DataOperations.PerformTake(gridData, dataManagerRequest.Take);
            }

            IDictionary<string, object> aggregates = new Dictionary<string, object>();
            if (dataManagerRequest.Aggregates != null) // Aggregation
            {
                aggregates = DataUtil.PerformAggregation(gridData, dataManagerRequest.Aggregates);
            }

            if (dataManagerRequest.Group != null && dataManagerRequest.Group.Any()) //Grouping
            {
                foreach (var group in dataManagerRequest.Group)
                {
                    gridData = DataUtil.Group<Order>(gridData, group, dataManagerRequest.Aggregates, 0, dataManagerRequest.GroupByFormatter);
                }
            }

            return dataManagerRequest.RequiresCounts ? new DataResult() { Result = gridData, Count = Orders.Count, Aggregates = aggregates } : gridData;
        }
        public override object Insert(DataManager dataManager, object value, string key)
        {
            Orders?.Insert(0, value as Order);
            return value;
        }
        public override object Remove(DataManager dataManager, object value, string keyField, string key)
        {
            int data = (int)value;
            Orders?.Remove(Orders?.Where((Order) => Order.OrderID == data).FirstOrDefault());
            return value;
        }
        public override object Update(DataManager dataManager, object value, string keyField, string key)
        {
            var val = (value as Order);
            var data = Orders?.Where((Order) => Order.OrderID == val?.OrderID).FirstOrDefault();
            if (data != null)
            {
                data.CustomerID = val?.CustomerID;
                data.Freight = val?.Freight;
                data.OrderDate = val?.OrderDate;
            }
            return value;
        }
      
    }
}
