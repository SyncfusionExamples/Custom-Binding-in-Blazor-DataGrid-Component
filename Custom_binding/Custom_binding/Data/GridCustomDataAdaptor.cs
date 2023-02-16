using Syncfusion.Blazor.Data;
using Syncfusion.Blazor;
using System.Collections;
namespace Custom_binding.Data
{
    /// <summary>
    /// A class the extends <see cref=“DataAdaptor”/> 
    /// Custom data binding in a data grid gives developers the ability to customize the way data is retrieved and processed.
    /// </summary>
    public class GridCustomDataAdaptor : DataAdaptor
    {
        public List<Order>? Orders { get; set; }
        public GridCustomDataAdaptor()
        {
            Orders = Enumerable.Range(1,200).Select(x => new Order()
            {
                OrderID = 1000 + x,
                CustomerID = (new string[] { "ALFKI", "ANANTR", "ANTON", "BLONP", "BOLID" })[new Random().Next(5)],
                Freight = 1.5 * x,
                OrderDate = DateTime.Now.AddDays(x),
            }).ToList();
        }

        /// <summary>
        /// Returns the data collection after performing data operations based on request from <see cref=”DataManagerRequest”/>
        /// </summary>
        /// <param name="dataManagerRequest">DataManagerRequest is used on the server side to model-bind posted data.</param>
        /// <param name="key">An optional parameter that allows to perform additional data operations.</param>
        /// <returns>A collection of data, the type of which is determined by the implementation of this method.</returns>
        public override object? Read(DataManagerRequest dataManagerRequest, string? key = null)
        {
            IEnumerable? gridData = Orders;

            if (dataManagerRequest.Where != null && dataManagerRequest.Where.Count > 0)// Filtering
            {
                gridData = DataOperations.PerformFiltering(gridData, dataManagerRequest.Where, dataManagerRequest.Where[0].Operator);
            }

            if (dataManagerRequest.Sorted?.Count > 0) // perform Sorting
            {
                gridData = DataOperations.PerformSorting(gridData, dataManagerRequest.Sorted);
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
        /// Inserts a new data item into the collection of data items. If the item already exists, updates the existing item with the new values. Returns the inserted or updated data item.
        /// </summary>
        /// <param name="dataManager">An instance of the DataManager class responsible for managing the data store in the grid data</param>
        /// <param name="value">Represents the newly added data in a crud operation.</param>
        /// <returns>Returns the data that was inserted as part of the operation.</returns>
        public override object Insert(DataManager dataManager, object value, string key)
        {
            Orders?.Insert(0, value as Order);
            return value;
        }

        /// <summary>
        /// Removes a data item from the collection of data items.
        /// </summary>
        /// <param name="dataManager">An instance of the DataManager class responsible for managing the data store in the grid data</param>
        /// <param name="value">Represents the removed item in a crud methods.</param>
        /// <param name="keyField">The key field denotes the primary column name in the grid data.</param>
        /// <returns>Returns the data item that was removed from the collection.</returns>
        public override object Remove(DataManager dataManager, object value, string keyField, string key)
        {
            int data = (int)value;
            Orders?.Remove(Orders.Where((Order) => Order.OrderID == data).FirstOrDefault());
            return value;
        }

        /// <summary>
        /// Updates a data item from the collection of data items.
        /// </summary>
        /// <param name="dataManager">An instance of the DataManager class responsible for managing the data store in the grid data</param>
        /// <param name="value">Represents the Updated item in a crud methods.</param>
        /// <param name="keyField">The key field denotes the primary column name in the grid data.</param>
        /// <returns>Returns the data item that was Updated from the collection.</returns>
        public override object Update(DataManager dataManager, object value, string keyField, string key)
        {
            var primary_key = value.GetType().GetProperty(keyField).GetValue(value);
            var data = Orders.Where(x => x.OrderID == (int)primary_key).FirstOrDefault();
            if (data != null)
            {
                data.CustomerID = (value as Order)?.CustomerID;
                data.Freight = (value as Order)?.Freight;
                data.OrderDate = (value as Order)?.OrderDate;
            }
            return value;
        }
    }
}
