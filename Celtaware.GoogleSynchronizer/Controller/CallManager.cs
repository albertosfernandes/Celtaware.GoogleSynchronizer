﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Celtaware.GoogleSynchronizer
{
    public class CallManager
    {
        //static string[] Scopes = { Google.Apis.Drive.v3.DriveService.Scope.DriveReadonly };
        //static string ApplicationName = "Drive API .NET Quickstart";

        private static Google.Apis.Auth.OAuth2.UserCredential Auth(out string status)
        {
            Google.Apis.Auth.OAuth2.UserCredential credential;
            status = null;
            try
            {
                using (var stream = new System.IO.FileStream(@"C:\Temp\Gdrive\Security\client_id.json", System.IO.FileMode.Open, System.IO.FileAccess.Read))
                {
                    var diretorioAtual = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                    var diretorioCredenciais = System.IO.Path.Combine(diretorioAtual, "credential");

                    credential = Google.Apis.Auth.OAuth2.GoogleWebAuthorizationBroker.AuthorizeAsync(
                        Google.Apis.Auth.OAuth2.GoogleClientSecrets.Load(stream).Secrets,
                        new[] { Google.Apis.Drive.v3.DriveService.Scope.DriveReadonly },
                        "user",
                        System.Threading.CancellationToken.None,
                        new Google.Apis.Util.Store.FileDataStore(diretorioCredenciais, true)).Result;
                }
                return credential;
            }
            catch (Exception err)
            {
                status = err.Message;
                credential = null;
                return credential;
            }
        }

        private static Google.Apis.Drive.v3.DriveService OpenService(Google.Apis.Auth.OAuth2.UserCredential credential)
        {
            return new Google.Apis.Drive.v3.DriveService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });
        }

        public static IList<string> ListFilesInFolder(string folderId)
        {

            string status = null;
            var credenciais = Auth(out status);
            List<string> lista = new List<string>();

            using (var servico = OpenService(credenciais))
            {
                var request = servico.Files.List();
                request.Fields = "files(id, name, createdTime, modifiedTime, size)";
                request.Q = $"'{folderId}' in parents";
                var resultado = request.Execute();
                var arquivos = resultado.Files;

                if (arquivos != null && arquivos.Any())
                {
                    foreach (var arquivo in arquivos)
                    {
                        lista.Add(arquivo.Name + " " + arquivo.Id + " " + arquivo.CreatedTime + " " + arquivo.ModifiedTime + " " + arquivo.Size + "<br>");
                    }
                }
            }

           
            return lista;
        }

        public static IList<string> ListFolders(string folderName)
        {
            string status = null;
            List<string> lista = new List<string>();
            var credenciais = Auth(out status);

            using (var servico = OpenService(credenciais))
            {
                var request = servico.Files.List();
                request.Fields = "files(id, name)";
                //request.Q = "mimeType='application/vnd.google-apps.folder'";
                // request.Q = "'17CYl9ni0xJEERcMZeKalLZ5D3Kp-vjw4' in parents";
                request.Q = $"name contains '{folderName}'";
                //request.Spaces = "Srv Backup DataBases";
                var resultado = request.Execute();
                var arquivos = resultado.Files;


                if (arquivos != null && arquivos.Any())
                {
                    foreach (var arquivo in arquivos)
                    {
                        lista.Add(arquivo.Name + " " + arquivo.Id);
                    }
                }
            }

           
            return lista;
        }
    }
}
