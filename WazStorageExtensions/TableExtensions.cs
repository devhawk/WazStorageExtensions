﻿using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Data.Services.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smarx.WazStorageExtensions
{
    public static class TableExtensions
    {
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