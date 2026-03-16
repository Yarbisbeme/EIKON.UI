using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using DevExpress.DataAccess.Json;
using DevExpress.DataAccess.Web;
using DevExpress.ReportServer.ServiceModel.DataContracts;
using EIKON.UI.Datos;
using EIKON.UI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;

namespace EIKON.UI.Services
{
    public class CustomJsonDataConnectionProviderFactory : IJsonDataConnectionProviderFactory {
        protected ReportDbContext DbContext { get; }

        public CustomJsonDataConnectionProviderFactory(ReportDbContext dbContext) {
            DbContext = dbContext;
        }

        public IJsonDataConnectionProviderService Create() {
            //var jsonSerializerSettings = new JsonSerializerSettings
            //{
            //    ContractResolver = new CamelCasePropertyNamesContractResolver()
            //};
            // return Json( DbContext.JsonDataConnections.ToList(), new JsonSerializerSettings { ContractResolver = new DefaultContractResolver });
            //var myJsonInput = DbContext.JsonDataConnections.ToList();
            //var interimObject = JsonConvert.DeserializeObject<List<Trhem000>>(myJsonInput.ToString);
            //var interim2Object= JsonConvert.DeserializeObject<IEnumerable<Trhem000>>(myJsonInput.ToString());
            //var myJsonOutput = JsonConvert.SerializeObject(interim2Object, jsonSerializerSettings);
            //var json = JsonConverter.DeserializeObject<string>(DbContext.JsonDataConnections.ToList());

            // Guardar el JSON en un archivo en el disco
            //var filePath = Path.Combine(Directory.GetCurrentDirectory(), "reportData.json");
            //await System.IO.File.WriteAllTextAsync(filePath, json);


            return new WebDocumentViewerJsonDataConnectionProvider(DbContext.JsonDataConnections.ToList()); 
        }
    }

    public class WebDocumentViewerJsonDataConnectionProvider : IJsonDataConnectionProviderService
    {
        readonly IEnumerable<DataConnection> jsonDataConnections;
        public WebDocumentViewerJsonDataConnectionProvider(IEnumerable<DataConnection> jsonDataConnections) {
            this.jsonDataConnections = jsonDataConnections;
            //this.jsonDataConnections(MvcNewtonsoftJsonOptionsExtensions.UseCamelCasing());
        }
        public JsonDataConnection GetJsonDataConnection(string name) {
            var connection = jsonDataConnections.FirstOrDefault(x => x.Name == name);
            
            if(connection == null)
                throw new InvalidOperationException();
            return CustomDataSourceWizardJsonDataConnectionStorage.CreateJsonDataConnectionFromString(connection);
        }
    }
}