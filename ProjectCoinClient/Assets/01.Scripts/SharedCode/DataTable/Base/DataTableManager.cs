using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace H00N.DataTables
{
    public static partial class DataTableManager
    {
        private static Dictionary<Type, IDataTable> dataTableDictionary = null;

        private static bool initialized = false;
        public static bool Initialized => initialized;

        public static void Initialize(Dictionary<string, string> jsonDatas)
        {
            if (initialized)
                return;

            dataTableDictionary = LoadAllDataTable(tableType => {
                if(jsonDatas.TryGetValue(tableType.Name, out string jsonData))
                    return CreateTable(tableType, jsonData);
                return CreateTable(tableType, "");
            });

            initialized = true;
        }

        public static void Release()
        {
            initialized = false;
            dataTableDictionary = null;
        }

        public static TTable GetTable<TTable>() where TTable : class, IDataTable
        {
            Type tableType = typeof(TTable);
            if(dataTableDictionary.TryGetValue(tableType, out IDataTable dataTable) == false)
                return null;

            return dataTable as TTable;
        }

        private static Dictionary<Type, IDataTable> LoadAllDataTable(Func<Type, IDataTable> dataTableFactory)
        {
            Dictionary<Type, IDataTable> tableDictionary = new Dictionary<Type, IDataTable>();
            try {
                IEnumerable<Type> dataTableTypes = GetDataTableTypes();
                foreach(Type dataTableType in dataTableTypes)
                {
                    IDataTable dataTable = dataTableFactory.Invoke(dataTableType);
                    tableDictionary.Add(dataTableType, dataTable);
                }
            } catch (Exception err) {
                Console.WriteLine(err);
            }

            return tableDictionary;
        }

        private static IDataTable CreateTable(Type dataTableType, string jsonData)
        {
            IDataTable dataTable = Activator.CreateInstance(dataTableType) as IDataTable;
            if(dataTable == null)
                return null;

            dataTable.CreateTable(jsonData);
            return dataTable;
        }

        private static IEnumerable<Type> GetDataTableTypes()
        {
            Type dataTableInterface = typeof(IDataTable);
            IEnumerable<Type> dataTableTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(GetTypesFromAssembly) // 어셈블리의 모든 타입 가져오기
                .Where(t => t.IsClass && !t.IsAbstract && dataTableInterface.IsAssignableFrom(t));

            IEnumerable<Type> GetTypesFromAssembly(Assembly assembly)
            {
                try
                {
                    return assembly.GetTypes();
                }
                catch (ReflectionTypeLoadException ex)
                {
                    return ex.Types.Where(t => t != null); // 로드 가능한 타입만 반환
                }
            }

            return dataTableTypes;
        }
    }
}