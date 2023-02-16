using Syncfusion.Blazor.Data;
using Syncfusion.Blazor;
using static Custom_binding.Pages.Index;
using System.Collections;
using System.Linq;

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
            IEnumerable? dataSource = Orders;

            //Performs filtering
            if (dataManagerRequest.Where != null && dataManagerRequest.Where.Count > 0)
            {
                dataSource = DataOperations.PerformFiltering(dataSource, dataManagerRequest.Where, dataManagerRequest.Where[0].Operator);
            }
            int count = dataSource.Cast<Order>().Count();

            //Performs sorting
            if (dataManagerRequest.Sorted?.Count > 0)
            {
                dataSource = DataOperations.PerformSorting(dataSource, dataManagerRequest.Sorted);
            }
            
            //Performs grouping
            if (dataManagerRequest.Group != null && dataManagerRequest.Group.Any()) //Grouping
            {
                foreach (var group in dataManagerRequest.Group)
                {
                    dataSource = DataUtil.Group<Order>(dataSource, group, dataManagerRequest.Aggregates, 0, dataManagerRequest.GroupByFormatter);
                }
            }

            //Performs aggregation
            IDictionary<string, object> aggregates = null;
            if (dataManagerRequest.Aggregates != null) // Aggregation
            {
                aggregates = DataUtil.PerformAggregation(dataSource, dataManagerRequest.Aggregates);
            }
            
            //Performs paging. For example, Skip is 0 and Take is equal to Page size for 1st Page. 
            if (dataManagerRequest.Skip != 0)
            {
                dataSource = DataOperations.PerformSkip(dataSource, dataManagerRequest.Skip);
            }

            if (dataManagerRequest.Take != 0)
            {
                dataSource = DataOperations.PerformTake(dataSource, dataManagerRequest.Take);
            }

            //Returning the DataResult or data collection based on the DataManagerRequest
            return dataManagerRequest.RequiresCounts ? new DataResult() { Result = dataSource, Count = count, Aggregates = aggregates } : dataSource;
        }

        /// <summary>
        /// Inserts a new data item into the data collection.
        /// </summary>
        /// <param name="dataManager">The DataManager is a data management component used for performing data operations in applications</param>
        /// <param name="value">The data item to be inserted.</param>
        /// <param name="key">The key value denotes the primary column value.</param>
        /// <returns>returns newly inserted data item.</returns>
        public override object Insert(DataManager dataManager, object record, string key)
        {
            Orders?.Insert(0, record as Order);
            return record;
        }

        /// <summary>
        /// Removes a data item from the data collection.
        /// </summary>
        /// <param name="dataManager">The DataManager is a data management component used for performing data operations in applications</param>
        /// <param name="value">The value to be removed item.</param>
        /// <param name="primaryColumnName">The key field denotes the primary column name.</param>
        /// <param name="key">The key value denotes the primary column value.</param>
        /// <returns>returns the removed data item.</returns>
        public override object Remove(DataManager dataManager, object primaryColumnValue, string primaryColumnName, string key)
        {
            //Since, OrderID column is marked as primary column in DataGrid, we can directly use the primaryColumnValue as OrderID.
            int data = (int)primaryColumnValue;
            Orders.Remove(Orders.Where((Order) => Order.OrderID == data).FirstOrDefault());
            return data;
        }

        /// <summary>
        /// Updates an existing data item in the data collection.
        /// </summary>
        /// <param name="dataManager">The DataManager is a data management component used for performing data operations in applications</param>
        /// <param name="value">The value to be Updated item.</param>
        /// <param name="primaryColumnName">The key field denotes the primary column name.</param>
        /// <param name="key">The key value denotes the primary column value.</param>
        /// <returns>returns the updated data item.</returns>
        public override object Update(DataManager dataManager, object record, string primaryColumnName, string key)
        {
            var order= record as Order;

            //Here, used OrderID directly, since OrderID column is marked as primary column in DataGrid.
            //Otherwise, you can use the primaryColumnName to get the primary column value. For eg: ReflectionExtension.GetValue(record, keyField)
            var dataObject = Orders.Where(x => x.OrderID == order.OrderID).FirstOrDefault();

            if (dataObject != null)
            {
                dataObject.OrderID = order.OrderID;
                dataObject.CustomerID = order.CustomerID;
                dataObject.Freight = order.Freight;
            }
            return dataObject;
        }
    }
}
