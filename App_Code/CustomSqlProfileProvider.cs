using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Web.Profile;

public class CustomSqlProfileProvider : ProfileProvider
{
	private string connectionString;

	public override void Initialize(string name, NameValueCollection config)
	{
		base.Initialize(name, config);

		string connectionStringName = config["connectionStringName"];
		if (string.IsNullOrEmpty(connectionStringName))
		{
			throw new ProviderException("Connection string name not specified");
		}

		connectionString = ConfigurationManager.ConnectionStrings[connectionStringName].ConnectionString;
	}

	public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection collection)
	{
		SettingsPropertyValueCollection values = new SettingsPropertyValueCollection();

		if (collection.Count < 1 || context["UserName"] == null)
			return values;

		string username = (string)context["UserName"];
		if (string.IsNullOrEmpty(username))
			return values;

		using (SqlConnection conn = new SqlConnection(connectionString))
		{
			SqlCommand cmd = new SqlCommand("SELECT * FROM Profile WHERE UserId = @UserId", conn);
			cmd.Parameters.AddWithValue("@UserId", username); // assuming UserId is stored as username

			conn.Open();
			SqlDataReader reader = cmd.ExecuteReader();

			if (reader.Read())
			{
				foreach (SettingsProperty prop in collection)
				{
					SettingsPropertyValue value = new SettingsPropertyValue(prop)
					{
						PropertyValue = reader[prop.Name],
						IsDirty = false
					};
					values.Add(value);
				}
			}
			reader.Close();
		}

		return values;
	}

	public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection collection)
	{
		string username = (string)context["UserName"];
		if (string.IsNullOrEmpty(username) || collection.Count < 1)
			return;

		using (SqlConnection conn = new SqlConnection(connectionString))
		{
			SqlCommand cmd = new SqlCommand("UPDATE Profile SET Email = @Email, FirstName = @FirstName, LastName = @LastName WHERE UserId = @UserId", conn);
			cmd.Parameters.AddWithValue("@UserId", username); // assuming UserId is stored as username

			foreach (SettingsPropertyValue value in collection)
			{
				cmd.Parameters.AddWithValue("@" + value.Property.Name, value.PropertyValue);
			}

			conn.Open();
			cmd.ExecuteNonQuery();
		}
	}

	// Implement other required methods...
	public override string ApplicationName { get; set; }
	public override int DeleteInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate) { throw new NotImplementedException(); }
	public override int DeleteProfiles(string[] usernames) { throw new NotImplementedException(); }
	public override int DeleteProfiles(ProfileInfoCollection profiles) { throw new NotImplementedException(); }
	public override ProfileInfoCollection FindInactiveProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords) { throw new NotImplementedException(); }
	public override ProfileInfoCollection FindProfilesByUserName(ProfileAuthenticationOption authenticationOption, string usernameToMatch, int pageIndex, int pageSize, out int totalRecords) { throw new NotImplementedException(); }
	public override ProfileInfoCollection GetAllInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate, int pageIndex, int pageSize, out int totalRecords) { throw new NotImplementedException(); }
	public override ProfileInfoCollection GetAllProfiles(ProfileAuthenticationOption authenticationOption, int pageIndex, int pageSize, out int totalRecords) { throw new NotImplementedException(); }
	public override int GetNumberOfInactiveProfiles(ProfileAuthenticationOption authenticationOption, DateTime userInactiveSinceDate) { throw new NotImplementedException(); }
}