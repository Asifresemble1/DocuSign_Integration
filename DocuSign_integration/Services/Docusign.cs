using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using DocuSign.eSign.Api;
using DocuSign.eSign.Client;
using DocuSign.eSign.Model;
using System.Threading.Tasks;
using Newtonsoft;
using System.Net.Http;
using Newtonsoft.Json;
using System.IO;

namespace DocuSign_integration.Services
{
	public class Docusign
	{
		//sendenvelope
		//public string sendenvelope()

		//{
		// Constants need to be set:
		private const string accessToken = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsImtpZCI6IjY4MTg1ZmYxLTRlNTEtNGNlOS1hZjFjLTY4OTgxMjIwMzMxNyJ9.eyJUb2tlblR5cGUiOjUsIklzc3VlSW5zdGFudCI6MTU5NTE0NjA0NywiZXhwIjoxNTk1MTc0ODQ3LCJVc2VySWQiOiI0OGZiMmJkNS1lM2NiLTRhNDMtYWFlNy1kODQxZDA3OTY4ZjIiLCJzaXRlaWQiOjEsInNjcCI6WyJzaWduYXR1cmUiLCJjbGljay5tYW5hZ2UiLCJvcmdhbml6YXRpb25fcmVhZCIsInJvb21fZm9ybXMiLCJncm91cF9yZWFkIiwicGVybWlzc2lvbl9yZWFkIiwidXNlcl9yZWFkIiwidXNlcl93cml0ZSIsImFjY291bnRfcmVhZCIsImRvbWFpbl9yZWFkIiwiaWRlbnRpdHlfcHJvdmlkZXJfcmVhZCIsImR0ci5yb29tcy5yZWFkIiwiZHRyLnJvb21zLndyaXRlIiwiZHRyLmRvY3VtZW50cy5yZWFkIiwiZHRyLmRvY3VtZW50cy53cml0ZSIsImR0ci5wcm9maWxlLnJlYWQiLCJkdHIucHJvZmlsZS53cml0ZSIsImR0ci5jb21wYW55LnJlYWQiLCJkdHIuY29tcGFueS53cml0ZSJdLCJhdWQiOiJmMGYyN2YwZS04NTdkLTRhNzEtYTRkYS0zMmNlY2FlM2E5NzgiLCJhenAiOiJmMGYyN2YwZS04NTdkLTRhNzEtYTRkYS0zMmNlY2FlM2E5NzgiLCJpc3MiOiJodHRwczovL2FjY291bnQtZC5kb2N1c2lnbi5jb20vIiwic3ViIjoiNDhmYjJiZDUtZTNjYi00YTQzLWFhZTctZDg0MWQwNzk2OGYyIiwiYW1yIjpbImludGVyYWN0aXZlIl0sImF1dGhfdGltZSI6MTU5NTE0NjA0NiwicHdpZCI6ImU2NGFlYTAwLTYyM2QtNDZiMC04NTVjLTk4NWMyNTRkMGVkOSJ9.ICHGc1H9TfXIuP62E7Iz8qrDzUklhnt6kPuv4jVHseXZViWumPqCNumXbR80bNMAfmVZsWVnDsqXEcpECGF5NoMrKaj40q_pLhdWs3OUIm9NP8h6QSdbP9hWhf8lKaZW8UopMnA-zyoTXa0FLht8plUBzOaV7vyUnrkhdAbV1ZHz7KGSiO64Pqf4fHtwJ2rvvgIExEAU2QLPH3myNCrf5GDXACJmcVrP-EyTbyjOT76KNpunyfiyIN7e1iFYkvVTx89VjDfU1wgvYnQkyVraFxPQta_zeJNczbqzgt1xB9gnLLVkKOdn8p6ry0Kq_CW1zD2nMBiKiTNR-tyVA7rrFw";
		private const string accountId = "10900453";
		private const string signerName = "Asif Moula";
		private const string signerEmail = "asif@resemblesystem.com";
		private const string docName = "World_Wide_Corp_lorem.pdf";

		// Additional constants
		private const string basePath = "https://demo.docusign.net/restapi";

		public void OnGet()
		{
		}

		public async Task<string> OnPostAsync()
		{
			// Send envelope. Signer's request to sign is sent by email.
			// 1. Create envelope request obj
			// 2. Use the SDK to create and send the envelope

			// 1. Create envelope request object
			//    Start with the different components of the request
			//    Create the document object
			Document document = new Document
			{
				DocumentBase64 = Convert.ToBase64String(ReadContent(docName)),
				Name = "Lorem Ipsum",
				FileExtension = "pdf",
				DocumentId = "1"
			};
			Document[] documents = new Document[] { document };

			// Create the signer recipient object 
			Signer signer = new Signer
			{
				Email = signerEmail,
				Name = signerName,
				RecipientId = "1",
				RoutingOrder = "1"
			};

			// Create the sign here tab (signing field on the document)
			SignHere signHereTab = new SignHere
			{
				DocumentId = "1",
				PageNumber = "1",
				RecipientId = "1",
				TabLabel = "Sign Here Tab",
				XPosition = "195",
				YPosition = "147"
			};
			SignHere[] signHereTabs = new SignHere[] { signHereTab };

			// Add the sign here tab array to the signer object.
			signer.Tabs = new Tabs { SignHereTabs = new List<SignHere>(signHereTabs) };
			// Create array of signer objects
			Signer[] signers = new Signer[] { signer };
			// Create recipients object
			Recipients recipients = new Recipients { Signers = new List<Signer>(signers) };
			// Bring the objects together in the EnvelopeDefinition
			EnvelopeDefinition envelopeDefinition = new EnvelopeDefinition
			{
				EmailSubject = "Please sign the document",
				Documents = new List<Document>(documents),
				Recipients = recipients,
				Status = "sent"
			};

			// 2. Use the SDK to create and send the envelope
			ApiClient apiClient = new ApiClient(basePath);
			apiClient.Configuration.AddDefaultHeader("Authorization", "Bearer " + accessToken);
			EnvelopesApi envelopesApi = new EnvelopesApi(apiClient.Configuration);
			EnvelopeSummary results = await envelopesApi.CreateEnvelopeAsync(accountId, envelopeDefinition);
			//ViewData["results"] = $"Envelope status: {results.Status}. Envelope ID: {results.EnvelopeId}";
			var envelopedata = new
			{
				envelopestatus = results.Status,
				envelopeid= results.EnvelopeId
				
			};
			return JsonConvert.SerializeObject(envelopedata) ;
		}

		/// <summary>
		/// This method read bytes content from the project's Resources directory
		/// </summary>
		/// <param name="fileName">resource path</param>
		/// <returns>return bytes array</returns>
		internal static byte[] ReadContent(string fileName)
		{
			byte[] buff = null;
			//string path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", fileName);
			string path = "C:\\Resources";
			using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
			{
				using (BinaryReader br = new BinaryReader(stream))
				{
					long numBytes = new FileInfo(path).Length;
					buff = br.ReadBytes((int)numBytes);
				}
			}

			return buff;
		}
	}


	
    }

