using Syncfusion.Blazor.Data;
using Syncfusion.Blazor;
using static Custom_binding.Pages.Index;
using System.Collections;
using System.Linq;

namespace Custom_binding.Data
{
    /// <summary>
    /// Implementing custom adaptor by extending the <see cref=“DataAdaptor”/> class.
    /// The DataGrid component support for custom data binding, which enables the binding and manipulation of data in a personalized way, using user-defined methods.
    /// </summary>
    public class GridCustomDataAdaptor : DataAdaptor
    {
        /// <summary>
        /// Returns the data collection after performing data operations based on request from <see cref=”DataManagerRequest”/>
        /// </summary>
        /// <param name="dataManagerRequest">DataManagerRequest containes the information regarding paging, grouping, filtering, searching which is handled on the DataGrid component side</param>
        /// <param name="additionalParam">An optional parameter that can be used to perform additional data operations.</param>
        /// <returns>The data collection's type is determined by how this method has been implemented.</returns>
        public override object? Read(DataManagerRequest dataManagerRequest, string? additionalParam = null)
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

            // Handling Grouping in Custom Adaptor            
            DataResult DataObject = new DataResult();
            if (dataManagerRequest.Group != null)
            {
                //Grouping                
                foreach (var group in dataManagerRequest.Group)
                {
                    dataSource = DataUtil.Group<Order>(dataSource, group, dataManagerRequest.Aggregates, 0, dataManagerRequest.GroupByFormatter);
                }
                DataObject.Result = dataSource;
                DataObject.Count = count;
                DataObject.Aggregates = aggregates;
                return dataManagerRequest.RequiresCounts ? DataObject : (object)dataSource;
            }


            //Here RequiresCount is passed from the control side itself, where ever the ondemand data fetching is needed then the RequiresCount is set as true in component side itself.
            // In the above case we are using Paging so datas are loaded in ondemand bases whenever the next page is clicked in DataGrid side.
            return dataManagerRequest.RequiresCounts ? new DataResult() { Result = dataSource, Count = count, Aggregates = aggregates } : dataSource;
        }

        /// <summary>
        /// Inserts a new data item into the data collection.
        /// </summary>
        /// <param name="dataManager">The DataManager is a data management component used for performing data operations in applications.</param>
        /// <param name="record">The new record which is need to be inserted.</param>
        /// <param name="additionalParam">An optional parameter that can be used to perform additional data operations.</param>
        /// <returns>Returns the newly inserted record details.</returns>
        public override object Insert(DataManager dataManager, object record, string additionalParam)
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
        /// <param name="additionalParam">An optional parameter that can be used to perform additional data operations.</param>
        /// <returns>Returns the removed data item.</returns>
        public override object Remove(DataManager dataManager, object primaryColumnValue, string primaryColumnName, string additionalParam)
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
        /// <param name="record">The modified record which is need to be updated.</param>
        /// <param name="primaryColumnName">The primaryColumnName specifies the field name of the primary column.</param>
        /// <param name="additionalParam">An optional parameter that can be used to perform additional data operations.</param>
        /// <returns>Returns the updated data item.</returns>
        public override object Update(DataManager dataManager, object record, string primaryColumnName, string additionalParam)
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
