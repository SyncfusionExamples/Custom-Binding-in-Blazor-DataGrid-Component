using Syncfusion.Blazor.Data;
using Syncfusion.Blazor;
using static Custom_binding.Pages.Index;
using System.Collections;

namespace Custom_binding.Data
{
    /// <summary>
    /// A class the extends <see cref=“DataAdaptor”/> to customize data retrieval and data operations for Blazor DataGrid. 
    /// DataGrid supports custom data binding that allows you to customize the data retrieval and data operations manually.
    /// </summary>
    public class GridCustomDataAdaptor : DataAdaptor
    {
        /// <summary>
        /// Returns the data collection after performing data operations based on request from <see cref=”DataManagerRequest”/>
        /// </summary>
        /// <param name="dataManagerRequest">DataManagerRequest is used on the server side to model-bind posted data.</param>
        /// <param name="key">An optional parameter that can be used to perform additional data operations.</param>
        /// <returns>A collection of data, the type of which is determined by the implementation of this method.</returns>
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

        /// <summary>
        /// Inserts a new data item into the data collection.
        /// </summary>
        /// <param name="dataManager">The DataManager is a data management component used for performing data operations in applications</param>
        /// <param name="value">The data item to be inserted.</param>
        /// <param name="key">An optional key that can be used to identify the data item being inserted.</param>
        /// <returns>The newly inserted data item.</returns>
        public override object Insert(DataManager dataManager, object value, string key)
        {
            Orders?.Insert(0, value as Order);
            return value;
        }

        /// <summary>
        /// Removes a data item from the data collection.
        /// </summary>
        /// <param name="dataManager">The DataManager is a data management component used for performing data operations in applications</param>
        /// <param name="value">The value to be removed item.</param>
        /// <param name="keyField">The key field identifier of the item to be removed.</param>
        /// <param name="key">The key value of the item to be removed.</param>
        /// <returns>The removed data item.</returns>
        public override object Remove(DataManager dataManager, object value, string keyField, string key)
        {
            int data = (int)value;
            Orders?.Remove(Orders.Where((Order) => Order.OrderID == data).FirstOrDefault());
            return value;
        }

        /// <summary>
        /// Updates an existing data item in the data collection.
        /// </summary>
        /// <param name="dataManager">The DataManager is a data management component used for performing data operations in applications</param>
        /// <param name="value">The value to be Updated item.</param>
        /// <param name="keyField">The key field identifier of the item to be updated.</param>
        /// <param name="key">The key value of the item to be updated.</param>
        /// <returns>The updated data item.</returns>
        public override object Update(DataManager dataManager, object value, string keyField, string key)
        {
            var val = value as Order;
            var data = Orders.Where((Order) => Order.OrderID == val?.OrderID).FirstOrDefault();
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
