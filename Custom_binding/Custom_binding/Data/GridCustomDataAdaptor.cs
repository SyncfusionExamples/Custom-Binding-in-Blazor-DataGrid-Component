using Syncfusion.Blazor.Data;
using Syncfusion.Blazor;
using static Custom_binding.Pages.Index;
using System.Collections;
using System.Linq;

namespace Custom_binding.Data
{
    /// <summary>
    /// Implementing custom adaptor by extending the <see cref=“DataAdaptor”/> class.
    /// DataGrid supports custom data binding that allows you to perform manual operations on the data.
    /// </summary>
    public class GridCustomDataAdaptor : DataAdaptor
    {
        /// <summary>
        /// Returns the data collection after performing data operations based on request from <see cref=”DataManagerRequest”/>
        /// </summary>
        /// <param name="dataManagerRequest">DataManagerRequest is used to model bind posted data at server side.</param>
        /// <param name="key">An optional parameter that can be used to perform additional data operations.</param>
        /// <returns>The data collection's type is determined by how this method has been implemented.</returns>
        public override object? Read(DataManagerRequest dataManagerRequest, string? key = null)
        {
            IEnumerable? dataSource = Orders;

            // Handling Filtering in Custom Adaptor.
            if (dataManagerRequest.Where != null && dataManagerRequest.Where.Count > 0)
            {
                dataSource = DataOperations.PerformFiltering(dataSource, dataManagerRequest.Where, dataManagerRequest.Where[0].Operator);
            }
            int count = dataSource.Cast<Order>().Count();

            // Handling Sorting in Custom Adaptor.
            if (dataManagerRequest.Sorted?.Count > 0)
            {
                dataSource = DataOperations.PerformSorting(dataSource, dataManagerRequest.Sorted);
            }
            
            // Handling Grouping in Custom Adaptor
            if (dataManagerRequest.Group != null && dataManagerRequest.Group.Any()) //Grouping
            {
                foreach (var group in dataManagerRequest.Group)
                {
                    dataSource = DataUtil.Group<Order>(dataSource, group, dataManagerRequest.Aggregates, 0, dataManagerRequest.GroupByFormatter);
                }
            }

            // Handling Aggregates in Custom Adaptor.
            IDictionary<string, object> aggregates = null;
            if (dataManagerRequest.Aggregates != null)
            {
                aggregates = DataUtil.PerformAggregation(dataSource, dataManagerRequest.Aggregates);
            }
            
            // Handling Paging in Custom Adaptor. For example, Skip is 0 and Take is equal to page size for first page. 
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
        /// <param name="dataManager">The DataManager is a data management component used for performing data operations in applications.</param>
        /// <param name="record">The new record which is need to be inserted.</param>
        /// <param name="key">The key value denotes the primary column value.</param>
        /// <returns>Returns the newly inserted record details.</returns>
        public override object Insert(DataManager dataManager, object record, string key)
        {
            Orders?.Insert(0, record as Order);
            return record;
        }

        /// <summary>
        /// Removes a data item from the data collection.
        /// </summary>
        /// <param name="dataManager">The DataManager is a data management component used for performing data operations in applications.</param>
        /// <param name="primaryColumnValue">The primaryColumnValue specifies the primary column value which is needs to be removed from the grid record.</param>
        /// <param name="primaryColumnName">The primaryColumnName specifies the field name of the primary column.</param>
        /// <param name="key">The key value denotes the primary column value.</param>
        /// <returns>Returns the removed data item.</returns>
        public override object Remove(DataManager dataManager, object primaryColumnValue, string primaryColumnName, string key)
        {
            // Given that the OrderID column is identified as the primary column in the DataGrid, the primaryColumnValue can be utilized as OrderID directly.
            int data = (int)primaryColumnValue;
            Orders.Remove(Orders.Where((Order) => Order.OrderID == data).FirstOrDefault());
            return data;
        }

        /// <summary>
        /// Updates an existing data item in the data collection.
        /// </summary>
        /// <param name="dataManager">The DataManager is a data management component used for performing data operations in applications.</param>
        /// <param name="record">The value which is need to be updated.</param>
        /// <param name="primaryColumnName">The primaryColumnName specifies the field name of the primary column.</param>
        /// <param name="key">The key value denotes the primary column value.</param>
        /// <returns>Returns the updated data item.</returns>
        public override object Update(DataManager dataManager, object record, string primaryColumnName, string key)
        {
            var order= record as Order;

            // Given that the OrderID column is identified as the primary column in the DataGrid, the primaryColumnValue can be utilized as OrderID directly.
            // If not, the primaryColumnName can be utilized to obtain the primary column value, such as through the use of ReflectionExtension.GetValue(record, keyField).
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