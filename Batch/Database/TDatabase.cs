using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;
using Batch.Models;

namespace Batch.Database
{
	public class TDatabase
	{
		private const string dbPath = "BatchDatabase.json";

		public static string DatabasePath
		{
			get
			{
				var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
				return Path.Combine(basePath, dbPath);
			}
		}

		public static void SaveDatabase(List<Deck> items)
		{
			var json = JsonSerializer.Serialize(items);
			File.WriteAllText(DatabasePath, json);
		}

		public static List<Deck> LoadDatabase()
		{
			string text;
			try
			{
				text = File.ReadAllText(DatabasePath);
			}
			catch // file doesn't exist yet
			{
				return new List<Deck>();
			}
			return JsonSerializer.Deserialize<List<Deck>>(text);
		}
	}


}
