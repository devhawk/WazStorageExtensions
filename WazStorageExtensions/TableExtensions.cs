using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DevHawk.WazStorageExtensions
{
    public static class TaskFactoryExtensions
    {
        public static Task<T> FromCancellableAsync<T>(this TaskFactory factory, Func<AsyncCallback, object, ICancellableAsyncResult> begin, Func<IAsyncResult, T> end, CancellationToken cancellationToken)
        {
            Func<AsyncCallback, Object, IAsyncResult> f = (cb, ob) =>
                {
                    var icr = begin(cb, ob);

                    if (cancellationToken != null)
                        cancellationToken.Register(icr.Cancel);

                    return icr;
                };

            return factory.FromAsync<T>(f, end, null);
        }
    }

    public static class TableExtensions
    {
        public static Task<TableQuerySegment<T>> ExecuteQuerySegmentedAsync<T>(this CloudTable table, TableQuery query, TableContinuationToken token = null)
        {
            return Task.Factory.FromAsync<TableQuerySegment<T>>(
                (cb, ob) => table.BeginExecuteQuerySegmented(query, token, cb, ob),
                table.EndExecuteQuerySegmented<T>,
                null);
        }

        public static Task<IEnumerable<T>> ExecuteAsync<T>(this DataServiceQuery<T> query)
        {
            return Task.Factory.FromAsync<IEnumerable<T>>(
                query.BeginExecute,
                query.EndExecute,
                null);
        }

        public static Task<bool> CreateIfNotExistsAsync(this CloudTable table, TableRequestOptions options = null, OperationContext context = null)
        {
            return Task.Factory.FromAsync<bool>(
                (cb, ob) => table.BeginCreateIfNotExists(options, context, cb, ob),
                table.EndCreateIfNotExists,
                null);
        }

        public static Task<TableResult> ExecuteAsync(this CloudTable table, TableOperation operation, TableRequestOptions options = null, OperationContext context = null)
        {
            return Task.Factory.FromAsync<TableResult>(
                (cb, ob) => table.BeginExecute(operation, options, context, cb, ob),
                table.EndExecute,
                null);
        }

    }
}
